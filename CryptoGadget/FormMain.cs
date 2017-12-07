

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
				coinGrid.Rows[row].Cells[coinGridValue.Index].Value = AdaptValue(val, Global.Sett.Digits.Value);
				coinGrid.Rows[row].Cells[coinGridChange24.Index].Value = (chg >= 0 ? "+" : "") + AdaptValue(chg, Global.Sett.Digits.Change24);
				coinGrid.Rows[row].Cells[coinGridChange24Pct.Index].Value = (per >= 0 ? "+" : "") + AdaptValue(per, Global.Sett.Digits.Change24Pct) + "%";
				coinGrid.Rows[row].Cells[coinGridChange24.Index].Style.ForeColor = chg >= 0.0 ? Global.Sett.Color.PositiveChange : Global.Sett.Color.NegativeChange;
				coinGrid.Rows[row].Cells[coinGridChange24Pct.Index].Style.ForeColor = per >= 0.0 ? Global.Sett.Color.PositiveChange : Global.Sett.Color.NegativeChange;
			};

			List<double> lastValues = new List<double>();
            foreach(DataGridViewRow row in coinGrid.Rows)
                lastValues.Add(double.Parse(row.Cells[coinGridValue.Index].Value.ToString()));
            
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
                    coinGrid.Rows[i].DefaultCellStyle.BackColor = i % 2 == 0 ? Global.Sett.Color.Background1 : Global.Sett.Color.Background2;
            };

            for(float opacity = 0.0f; opacity < 1.0f; opacity += 0.05f) {

                if(_timer_disposed) {
                    DefaultColors();
                    return;
                }

                for(int i = 0; i < lastValues.Count; i++) {

                    Color bgcolor = i % 2 == 0 ? Global.Sett.Color.Background1 : Global.Sett.Color.Background2;
                    double currentValue = double.Parse(coinGrid.Rows[i].Cells[2].Value.ToString());

                    if(currentValue > lastValues[i]) {
                        Color color = ColorApply(Global.Sett.Color.PositiveRefresh, bgcolor, opacity);
                        coinGrid.Rows[i].DefaultCellStyle.BackColor = color;
                    }
                    else if(currentValue < lastValues[i]) {
                        Color color = ColorApply(Global.Sett.Color.NegativeRefresh, bgcolor, opacity);
                        coinGrid.Rows[i].DefaultCellStyle.BackColor = color;
                    }
                }

                Thread.Sleep(60);
            }

            DefaultColors();

        }

		private void RotatePage() {
			_page = (_page + 1) % Global.Sett.Pages.Size;
			GridInit();
			ResizeForm();
		}

		private void ResizeForm() {
            
            int X = 0;
            int Y = coinGrid.ColumnHeadersVisible ? coinGrid.ColumnHeadersHeight : 0;
            int edge = Global.Sett.Visibility.Edge ? Global.Sett.Metrics.Edge : 0;

            foreach(DataGridViewColumn col in coinGrid.Columns)
                X += col.Visible ? col.Width : 0;
            foreach(DataGridViewRow row in coinGrid.Rows)
                Y += row.Height;

            coinGrid.Location = new Point(edge, edge);
            coinGrid.Size = new Size(X, Y);
            Size = new Size(X + edge * 2, Y + edge * 2);
        }
        private void GridInit() {

			coinGrid.Rows.Clear();

			// skip this when reloading the form ??
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
			
			for(int i = 0; i < Global.Sett.Pages.Size; i++) {
				_queries[i] = CCRequest.ConvertQuery(Global.Sett.Coins[i]);
			}
			_page = Global.Sett.Pages.Default;

			if(Global.Json == null && File.Exists(Global.JsonLocation)) {
                Global.Json = JObject.Parse(new StreamReader(File.Open(Global.JsonLocation, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)).ReadToEnd());
                if(!Global.JsonIsValid(Global.Json))
                    Global.Json = null;
            }
			
			// put on a new function for page swap ?? or just generate all grids from all pages and swap them when needed ??
			#region Coin Rows Init

			foreach(Settings.StCoin st in Global.Sett.Coins[_page]) {

				int index = coinGrid.Rows.Add(Global.IconResize(Global.GetIcon(st.Coin), Global.Sett.Metrics.IconSize), st.Coin, 0.00, 0.00);

                // Context Menu
                ContextMenuStrip cm = new ContextMenuStrip();
                cm.Items.Add("Settings", null, contextMenuSettings_Click);
                cm.Items.Add("Hide", null, contextMenuHide_Click);
                cm.Items.Add("Exit", null, contextMenuExit_Click);
                    
                if(Global.Json != null) {
                    JToken coin = Global.Json["Data"][st.Coin];
					if(coin != null && coin["Url"] != null) {
						cm.Items.Insert(0, new ToolStripMenuItem(st.Coin + " website", null, (sender, e) => Process.Start("https://www.cryptocompare.com" + coin["Url"])));
						cm.Items.Insert(1, new ToolStripSeparator());
					}
                }

                coinGrid.Rows[index].ContextMenuStrip = cm;

                // Name Tooltip
                if(Global.Json != null) {
                    string name = Global.Json["Data"][st.Coin]["CoinName"].ToString();
                    foreach(DataGridViewCell cell in coinGrid.Rows[index].Cells)
                        cell.ToolTipText = name;
                }

            }

            #endregion

            #region Metrics & Visibility

			// for(int i = 0; i < props.length; i++) 
            coinGrid.Columns[0].Width = Global.Sett.Metrics.Icon;
            coinGrid.Columns[1].Width = Global.Sett.Metrics.Coin;
            coinGrid.Columns[2].Width = Global.Sett.Metrics.Value;
            coinGrid.Columns[3].Width = Global.Sett.Metrics.Change24;
            coinGrid.Columns[4].Width = Global.Sett.Metrics.Change24Pct;
            coinGrid.ColumnHeadersHeight = Global.Sett.Metrics.Header;
            coinGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", Global.Sett.Metrics.HeaderText);
            coinGrid.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", Global.Sett.Metrics.RowsValues);

            foreach(DataGridViewRow row in coinGrid.Rows)
                row.Height = Global.Sett.Metrics.Rows;

			// for(int i = 0; i < props.length; i++) 
			coinGrid.Columns[0].Visible = Global.Sett.Visibility.Icon;
            coinGrid.Columns[1].Visible = Global.Sett.Visibility.Coin;
            coinGrid.Columns[2].Visible = Global.Sett.Visibility.Value;
            coinGrid.Columns[3].Visible = Global.Sett.Visibility.Change24;
            coinGrid.Columns[4].Visible = Global.Sett.Visibility.Change24Pct;
            coinGrid.ColumnHeadersVisible = Global.Sett.Visibility.Header;

            #endregion

            #region Color Init

            coinGrid.RowsDefaultCellStyle.BackColor = Global.Sett.Color.Background1;
            coinGrid.AlternatingRowsDefaultCellStyle.BackColor = Global.Sett.Color.Background2;
            coinGrid.Columns[1].DefaultCellStyle.ForeColor = Global.Sett.Color.Coin;
            coinGrid.Columns[2].DefaultCellStyle.ForeColor = Global.Sett.Color.Value;

            coinGrid.ColumnHeadersDefaultCellStyle.ForeColor = Global.Sett.Color.HeaderText;
            coinGrid.ColumnHeadersDefaultCellStyle.BackColor = Global.Sett.Color.HeaderBackground;

            BackColor = Global.Sett.Color.Edge;

            #endregion

            #region Coordinates Init

            StartPosition = FormStartPosition.Manual;
            Location = new Point(Global.Sett.Coords.PosX, Global.Sett.Coords.PosY);

            MouseDown -= FormUtil.DragMove;
            coinGrid.MouseDown -= FormUtil.DragMove;
            if(!Global.Sett.Coords.LockPos) {
                MouseDown += FormUtil.DragMove;
                coinGrid.MouseDown += FormUtil.DragMove;
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


        public FormMain() {

            InitializeComponent();

            notifyIcon.Text = typeof(FormMain).Assembly.GetName().Name + " " + typeof(FormMain).Assembly.GetName().Version;
            notifyIcon.Text = notifyIcon.Text.Remove(notifyIcon.Text.Length - 2);

            Load += (sender, e) => {
                GridInit();
                ResizeForm();

                coinGrid.DoubleBuffered(true);
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
                //Point currLoc = Location; 
                GridInit();
                ResizeForm();
                //Location = currLoc;
            }

            _timer_disposed = false;
            _timer_req = new System.Threading.Timer(TimerRoutine, null, 0, Global.Sett.Basic.RefreshRate);
        }
        private void contextMenuHide_Click(object sender, EventArgs e) {
            Visible = !Visible;
			contextMenuHide.Checked = !Visible;
            //contextMenuHide.Text = Visible ? "Hide" : "Show";
        }
        private void contextMenuExit_Click(object sender, EventArgs e) {
            Close();
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e) {
            Activate();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e) {
			if(Global.Sett.Coords.ExitSave && (Location.X != Global.Sett.Coords.PosX || Location.Y != Global.Sett.Coords.PosY))
				Global.Sett.Save();
        }

        private void coinGrid_SelectionChanged(object sender, EventArgs e) {
            coinGrid.ClearSelection();
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
