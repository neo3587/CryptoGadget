﻿

/*
*	This application is possible thanks to CryptoCompare: https://www.cryptocompare.com
*   
* 
*/






using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;

using Newtonsoft.Json.Linq;
using Microsoft.Win32;

using neo;






namespace CryptoGadget {

    public partial class FormMain : Form {

        private System.Threading.Timer _timer_req;
        private volatile bool _timer_disposed = false;
		private string[] _queries = new string[10];
		private int _page = 0;
		private Dictionary<string, string> _digit_adapts = new Dictionary<string, string>();

        private void TimerRoutine(object state) {

            try {
                _timer_req.Change(Timeout.Infinite, Timeout.Infinite);
            } catch {
                return;
            }

            Stopwatch watch = Stopwatch.StartNew();

			Func<double, int, string> AdaptValue = (val, maxDigit) => {
				int decimals = Math.Max(0, maxDigit - (int)Math.Floor(Math.Log10(Math.Max(1.0, Math.Abs(val))) + 1));
				return Math.Round(val, decimals).ToString("0." + new string('0', decimals));
			};
			Action<int, double, double, double> UpdateRow = (row, val, chg, per) => { 

				// for(int i = 0; i < props.Length; i++) && prop.visible && !specialized(?)
				mainGrid.Rows[row].Cells[mainGridValue.Index].Value = AdaptValue(val, Global.Sett.Digits.Value);
				mainGrid.Rows[row].Cells[mainGridChange24.Index].Value = (chg >= 0 ? "+" : "") + AdaptValue(chg, Global.Sett.Digits.Change24);
				mainGrid.Rows[row].Cells[mainGridChange24Pct.Index].Value = (per >= 0 ? "+" : "") + AdaptValue(per, Global.Sett.Digits.Change24Pct) + "%";
				mainGrid.Rows[row].Cells[mainGridChange24.Index].Style.ForeColor = chg >= 0.0 ? Global.Sett.Color.PositiveChange : Global.Sett.Color.NegativeChange;
				mainGrid.Rows[row].Cells[mainGridChange24Pct.Index].Style.ForeColor = per >= 0.0 ? Global.Sett.Color.PositiveChange : Global.Sett.Color.NegativeChange;
			};

			List<double> lastValues = new List<double>();
            foreach(DataGridViewRow row in mainGrid.Rows)
                lastValues.Add(double.Parse(row.Cells[mainGridValue.Index].Value.ToString()));
            
            try {
				JObject json = CCRequest.HttpRequest(_queries[_page]);
                if(json == null || json["Response"]?.ToString().ToLower() == "error") {
                    for(int i = 0; i < Global.Sett.Coins[_page].Count; i++)
                        UpdateRow(i, 0.00, 0.00, 0.00);
                }
                else  {
                    for(int i = 0; i < Global.Sett.Coins[_page].Count; i++) {
						JToken jtok = json["RAW"][Global.Sett.Coins[_page][i].Coin][Global.Sett.Coins[_page][i].Target];
						UpdateRow(i, jtok["PRICE"].ToObject<double>(),
                                     jtok["CHANGE24HOUR"].ToObject<double>(),
                                     jtok["CHANGEPCT24HOUR"].ToObject<double>());
                    }
                }
            } catch { }

            if(Global.Sett.Visibility.Refresh)
                TimerHighlight(lastValues);

            try {
                _timer_req.Change(Math.Max(0, Global.Sett.Basic.RefreshRate - watch.ElapsedMilliseconds), Global.Sett.Basic.RefreshRate);
            } catch { }

        }
        private void TimerHighlight(List<double> lastValues) {

            Func<Color, Color, float, Color> ColorApply = (color, bgcolor, opacity) => {
                byte[] bytecolor = BitConverter.GetBytes(color.ToArgb());
                byte[] bytebgcolor = BitConverter.GetBytes(bgcolor.ToArgb());
                for(int i = 0; i < 4; i++)
                    bytecolor[i] = (byte)(bytebgcolor[i] * opacity + bytecolor[i] * (1 - opacity));
                return Color.FromArgb(BitConverter.ToInt32(bytecolor, 0));
            };
            Action DefaultColors = () => {
                for(int i = 0; i < lastValues.Count; i++)
                    mainGrid.Rows[i].DefaultCellStyle.BackColor = i % 2 == 0 ? Global.Sett.Color.Background1 : Global.Sett.Color.Background2;
            };

            for(float opacity = 0.0f; opacity < 1.0f; opacity += 0.05f) {

                if(_timer_disposed) {
                    DefaultColors();
                    return;
                }

                for(int i = 0; i < lastValues.Count; i++) {

                    Color bgcolor = i % 2 == 0 ? Global.Sett.Color.Background1 : Global.Sett.Color.Background2;
                    double currentValue = double.Parse(mainGrid.Rows[i].Cells[2].Value.ToString());

                    if(currentValue > lastValues[i]) {
                        Color color = ColorApply(Global.Sett.Color.PositiveRefresh, bgcolor, opacity);
                        mainGrid.Rows[i].DefaultCellStyle.BackColor = color;
                    }
                    else if(currentValue < lastValues[i]) {
                        Color color = ColorApply(Global.Sett.Color.NegativeRefresh, bgcolor, opacity);
                        mainGrid.Rows[i].DefaultCellStyle.BackColor = color;
                    }
                }

                Thread.Sleep(60);
            }

            DefaultColors();

        }

		private void RotatePage() {
			_page = (_page + 1) % Global.Sett.Pages.Size;
			RowsInit();
			ResizeForm();
		}

		private void ResizeForm() {
            
            int X = 0;
            int Y = mainGrid.ColumnHeadersVisible ? mainGrid.ColumnHeadersHeight : 0;
            int edge = Global.Sett.Visibility.Edge ? Global.Sett.Metrics.Edge : 0;

            foreach(DataGridViewColumn col in mainGrid.Columns)
                X += col.Visible ? col.Width : 0;
            foreach(DataGridViewRow row in mainGrid.Rows)
                Y += row.Height;

            mainGrid.Location = new Point(edge, edge);
            mainGrid.Size = new Size(X, Y);
            Size = new Size(X + edge * 2, Y + edge * 2);
        }
        private void GridInit() {

			mainGrid.Rows.Clear();

			for(int i = 0; i < Global.Sett.Pages.Size; i++) {
				_queries[i] = CCRequest.ConvertQuery(Global.Sett.Coins[i]);
			}
			_page = Global.Sett.Pages.Default;

			if(Global.Json == null && File.Exists(Global.JsonLocation)) {
                Global.Json = JObject.Parse(new StreamReader(File.Open(Global.JsonLocation, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)).ReadToEnd());
                if(!Global.JsonIsValid(Global.Json))
                    Global.Json = null;
            }

			RowsInit();

            #region Metrics & Visibility

			// for(int i = 0; i < props.length; i++) 
            mainGrid.Columns[0].Width = Global.Sett.Metrics.Icon;
            mainGrid.Columns[1].Width = Global.Sett.Metrics.Coin;
            mainGrid.Columns[2].Width = Global.Sett.Metrics.Value;
            mainGrid.Columns[3].Width = Global.Sett.Metrics.Change24;
            mainGrid.Columns[4].Width = Global.Sett.Metrics.Change24Pct;
            mainGrid.ColumnHeadersHeight = Global.Sett.Metrics.Header;
            mainGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", Global.Sett.Metrics.HeaderText);
            mainGrid.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", Global.Sett.Metrics.RowsValues);

            foreach(DataGridViewRow row in mainGrid.Rows)
                row.Height = Global.Sett.Metrics.Rows;

			// for(int i = 0; i < props.length; i++) 
			mainGrid.Columns[0].Visible = Global.Sett.Visibility.Icon;
            mainGrid.Columns[1].Visible = Global.Sett.Visibility.Coin;
            mainGrid.Columns[2].Visible = Global.Sett.Visibility.Value;
            mainGrid.Columns[3].Visible = Global.Sett.Visibility.Change24;
            mainGrid.Columns[4].Visible = Global.Sett.Visibility.Change24Pct;
            mainGrid.ColumnHeadersVisible = Global.Sett.Visibility.Header;

            #endregion

            #region Color Init

            mainGrid.RowsDefaultCellStyle.BackColor = Global.Sett.Color.Background1;
            mainGrid.AlternatingRowsDefaultCellStyle.BackColor = Global.Sett.Color.Background2;
            mainGrid.Columns[1].DefaultCellStyle.ForeColor = Global.Sett.Color.Coin;
            mainGrid.Columns[2].DefaultCellStyle.ForeColor = Global.Sett.Color.Value;

            mainGrid.ColumnHeadersDefaultCellStyle.ForeColor = Global.Sett.Color.HeaderText;
            mainGrid.ColumnHeadersDefaultCellStyle.BackColor = Global.Sett.Color.HeaderBackground;

            BackColor = Global.Sett.Color.Edge;

            #endregion

            #region Coordinates Init

            StartPosition = FormStartPosition.Manual;
            Location = new Point(Global.Sett.Coords.PosX, Global.Sett.Coords.PosY);

            MouseDown -= FormUtil.DragMove;
            mainGrid.MouseDown -= FormUtil.DragMove;
            if(!Global.Sett.Coords.LockPos) {
                MouseDown += FormUtil.DragMove;
                mainGrid.MouseDown += FormUtil.DragMove;
            }

            #endregion

            #region Other Stuff Init

            // Open on Startup
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if((regKey.GetValue("CryptoGadget", null) != null) != Global.Sett.Basic.Startup) {
                if(Global.Sett.Basic.Startup)
                    regKey.SetValue("CryptoGadget", "\"" + Application.ExecutablePath.ToString() + "\"");
                else
                    regKey.DeleteValue("CryptoGadget", false);
            }

            #endregion

        }
		private void RowsInit() {

			foreach(Settings.StCoin st in Global.Sett.Coins[_page]) {

				int index = mainGrid.Rows.Add(Global.IconResize(Global.GetIcon(st.Coin), Global.Sett.Metrics.IconSize), st.Coin, 0.00, 0.00);

				// Context Menu
				ContextMenuStrip cm = new ContextMenuStrip();

				if(Global.Json != null) {
					JToken coin = Global.Json["Data"][st.Coin];
					if(coin != null && coin["Url"] != null) {
						cm.Items.Add(new ToolStripMenuItem(st.Coin + " website", null, (sender, e) => Process.Start("https://www.cryptocompare.com" + coin["Url"])));
						cm.Items.Add(new ToolStripSeparator());
					}
					st.CoinName   = Global.Json["Data"][st.Coin]["CoinName"].ToString();
					st.TargetName = Global.Json["Data"][st.Target]["CoinName"].ToString();
				}

				cm.Items.Add("Settings", null, contextMenuSettings_Click);
				cm.Items.Add("Hide", null, contextMenuHide_Click);
				cm.Items.Add("Exit", null, contextMenuExit_Click);

				mainGrid.Rows[index].ContextMenuStrip = cm;

				// Name Tooltip
				if(Global.Json != null) {
					string name = Global.Json["Data"][st.Coin]["CoinName"].ToString();
					foreach(DataGridViewCell cell in mainGrid.Rows[index].Cells)
						cell.ToolTipText = name;
				}

			}

		}

		public FormMain() {

            InitializeComponent();

            notifyIcon.Text = typeof(FormMain).Assembly.GetName().Name + " " + typeof(FormMain).Assembly.GetName().Version;
            notifyIcon.Text = notifyIcon.Text.Remove(notifyIcon.Text.Length - 2);

			#if DEBUG
			File.Delete(Global.IniLocation);
			#endif

			Load += (sender, e) => {
				
				if(!Global.Sett.BindFile(Global.IniLocation)) {
					Settings.CreateIni(Global.IniLocation);
					Global.Sett.BindFile(Global.IniLocation);
					Global.Sett.Default();
					Global.Sett.Store();
					Global.Sett.Save();
				}
				if(!Global.Sett.Load()) {
					MessageBox.Show("The settings file is corrupted, a new settings file with the default values will be used");
					Global.Sett.Default();
					Global.Sett.Store();
					Global.Sett.Save();
					Global.Sett.Load();
				}

				GridInit();
                ResizeForm();

                mainGrid.DoubleBuffered(true);
                FormBorderStyle = FormBorderStyle.None; // avoid alt-tab

                _timer_req = new System.Threading.Timer(TimerRoutine, null, 0, Global.Sett.Basic.RefreshRate);
            };
        }

        private void contextMenuSettings_Click(object sender, EventArgs e) {

            WaitHandle wait = new AutoResetEvent(false);
            _timer_req.Dispose(wait);
            _timer_disposed = true;

            FormSettings form2 = new FormSettings(this);
            form2.ShowDialog();

            wait.WaitOne();

            if(form2.accept) {
				Point currLoc = Location; // prevent the form realocation
                GridInit();
                ResizeForm();
                Location = currLoc;
			}

            _timer_disposed = false;
            _timer_req = new System.Threading.Timer(TimerRoutine, null, 0, Global.Sett.Basic.RefreshRate);
        }
        private void contextMenuHide_Click(object sender, EventArgs e) {
            Visible = !Visible;
			contextMenuHide.Checked = !Visible;
        }
        private void contextMenuExit_Click(object sender, EventArgs e) {
            Close();
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e) {
            Activate();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e) {
			if(Global.Sett.Coords.ExitSave && (Location.X != Global.Sett.Coords.PosX || Location.Y != Global.Sett.Coords.PosY)) {
				Global.Sett.Coords.PosX = Location.X;
				Global.Sett.Coords.PosY = Location.Y;
				Global.Sett.Store();
				Global.Sett.Save();
			}
        }

        private void coinGrid_SelectionChanged(object sender, EventArgs e) {
            mainGrid.ClearSelection();
        }
    }

    /// <summary>
    /// Adds a double buffer to the grid (avoids flickering)
    /// </summary>
    public static class DataGridViewExtensioncs {
        public static void DoubleBuffered(this DataGridView dgv, bool setting) {
            Type dgvType = dgv.GetType();
            dgvType.GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(dgv, setting, null);
        }
    }

}