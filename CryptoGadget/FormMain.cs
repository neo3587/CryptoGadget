

/*
*	This application is possible thanks to CryptoCompare: https://www.cryptocompare.com
*   
* 
*/



using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using System.Reflection;

using Newtonsoft.Json.Linq;
using Microsoft.Win32;



/* TODO:
- Make Profile swapping and default stuff
*/


namespace CryptoGadget {

    public partial class FormMain : Form {

		public class CoinRow : Settings._PropManager<CoinRow> {
			public Bitmap Icon { get; set; } = null; // skip this on json get
			public string Coin { get; set; } = "";// skip this on json get
			public Bitmap TargetIcon { get; set; } = null; // skip this on json get
			public string Target { get; set; } = ""; // skip this on json get
			public string Value { get; set; } = "0";
			public string ChangeDay { get; set; } = "0";
			public string ChangeDayPct { get; set; } = "0";
			public string Change24 { get; set; } = "0";
			public string Change24Pct { get; set; } = "0";
			public string VolumeDay { get; set; } = "0";
			public string Volume24 { get; set; } = "0";
			public string TotalVolume24 { get; set; } = "0";
			public string OpenDay { get; set; } = "0";
			public string Open24 { get; set; } = "0";
			public string HighDay { get; set; } = "0";
			public string High24 { get; set; } = "0";
			public string LowDay { get; set; } = "0";
			public string Low24 { get; set; } = "0";
			public string Supply { get; set; } = "0";
			public string MktCap { get; set; } = "0";
			public string LastMarket { get; set; } = "?";
		}
		
        private System.Threading.Timer _timer_req = null;
        private volatile bool _timer_disposed = false;
		private string _query = "";
		private int _page = 0;
		private BindingList<CoinRow> _coinGrid = new BindingList<CoinRow>();


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

			List<double> last_values = new List<double>();
			foreach(CoinRow row in _coinGrid)
				last_values.Add(double.Parse(row.Value));

			JObject json = CCRequest.HttpRequest(_query);
            if(json != null && json["Response"]?.ToString().ToLower() != "error") {
				try {
					for(int i = 0; i < _coinGrid.Count; i++) {
						JToken jtok = json["RAW"][Global.Sett.Coins[_page][i].Coin][Global.Sett.Coins[_page][i].Target];

						foreach(ValueTuple<string, string, string> tp in Settings.StGrid.jsget) 
							_coinGrid[i][tp.Item1] = AdaptValue(jtok[tp.Item3].ToObject<double>(), (Global.Sett.Grid[tp.Item1] as Settings.StColumn).Digits);
						_coinGrid[i].LastMarket = jtok["LASTMARKET"].ToString();

						string[] changes = {"Change24", "Change24Pct", "ChangeDay", "ChangeDayPct"};
						foreach(string chg in changes) {
							if((_coinGrid[i][chg] as string)[0] != '-')
								_coinGrid[i][chg] = "+" + (_coinGrid[i][chg] as string);
							mainGrid.Rows[i].Cells[chg].Style.ForeColor = (_coinGrid[i][chg] as string)[0] == '+' ? Global.Sett.Color.PositiveChange : Global.Sett.Color.NegativeChange;
						}
						_coinGrid[i].Change24Pct  += "%";
						_coinGrid[i].ChangeDayPct += "%";
					}
				}
				catch (Exception e) {
					Global.DbgPrint(e.Message);
				}
			}

			try {
				Invoke((MethodInvoker)delegate { Refresh(); });
			} catch { return; }

            if(Global.Sett.Visibility.Refresh)
				TimerHighlight(last_values);

            try {
                _timer_req.Change(Math.Max(0, Global.Sett.Basic.RefreshRate - watch.ElapsedMilliseconds), Global.Sett.Basic.RefreshRate);
            } catch { }

        }
        private void TimerHighlight(List<double> last_values) {

            Func<Color, Color, float, Color> ColorApply = (color, bgcolor, opacity) => {
                byte[] bytecolor = BitConverter.GetBytes(color.ToArgb());
                byte[] bytebgcolor = BitConverter.GetBytes(bgcolor.ToArgb());
                for(int i = 0; i < 4; i++)
                    bytecolor[i] = (byte)(bytebgcolor[i] * opacity + bytecolor[i] * (1 - opacity));
                return Color.FromArgb(BitConverter.ToInt32(bytecolor, 0));
            };
            Action DefaultColors = () => {
                for(int i = 0; i < last_values.Count; i++)
                    mainGrid.Rows[i].DefaultCellStyle.BackColor = i % 2 == 0 ? Global.Sett.Color.Background1 : Global.Sett.Color.Background2;
            };

            for(float opacity = 0.0f; opacity < 1.0f; opacity += 0.05f) {

                if(_timer_disposed) {
                    DefaultColors();
                    return;
                }

                for(int i = 0; i < last_values.Count; i++) {

                    Color bgcolor = i % 2 == 0 ? Global.Sett.Color.Background1 : Global.Sett.Color.Background2;
                    double currentValue = double.Parse(_coinGrid[i].Value);

                    if(currentValue > last_values[i]) 
						mainGrid.Rows[i].DefaultCellStyle.BackColor = ColorApply(Global.Sett.Color.PositiveRefresh, bgcolor, opacity);
                    else if(currentValue < last_values[i]) 
                        mainGrid.Rows[i].DefaultCellStyle.BackColor = ColorApply(Global.Sett.Color.NegativeRefresh, bgcolor, opacity);
                }

                Thread.Sleep(60);
            }

            DefaultColors();

        }

		private void RotatePage() {
			// care with thread datashare
			_page = (_page + 1) % Global.Sett.Pages.Size;
			_query = CCRequest.ConvertQuery(Global.Sett.Coins[_page]);
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
			mainGrid.Columns.Clear();
			mainGrid.DataSource = null;

			_page  = Global.Sett.Pages.Default;
			_query = CCRequest.ConvertQuery(Global.Sett.Coins[_page]);

			// Rows & Columns init and binding

			RowsInit();

			foreach(PropertyInfo prop in Settings.StGrid.GetProps()) {
				DataGridViewColumn col = new DataGridViewColumn();
				col.HeaderText = (Global.Sett.Grid[prop.Name] as Settings.StColumn).Name; 
				col.Name = prop.Name;
				col.DataPropertyName = prop.Name;
				col.CellTemplate = new DataGridViewTextBoxCell();
				col.Visible = (Global.Sett.Grid[prop.Name] as Settings.StColumn).Enabled;
				col.Width = (Global.Sett.Grid[prop.Name] as Settings.StColumn).Width;
				col.DefaultCellStyle.ForeColor = Global.Sett.Color.RowsValues;
				mainGrid.Columns.Add(col);
			}

			mainGrid.Columns["Icon"].CellTemplate       = new DataGridViewImageCell();
			mainGrid.Columns["TargetIcon"].CellTemplate = new DataGridViewImageCell();
			mainGrid.Columns["Icon"].DefaultCellStyle.Alignment		  = DataGridViewContentAlignment.MiddleCenter;
			mainGrid.Columns["TargetIcon"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

			mainGrid.Columns["Coin"].DefaultCellStyle.ForeColor       = Global.Sett.Color.RowsText;
			mainGrid.Columns["Target"].DefaultCellStyle.ForeColor     = Global.Sett.Color.RowsText;
			mainGrid.Columns["LastMarket"].DefaultCellStyle.ForeColor = Global.Sett.Color.RowsText;

			mainGrid.AutoGenerateColumns = false;

			BindingSource coin_bind = new BindingSource();
			coin_bind.DataSource = _coinGrid;
			mainGrid.DataSource = coin_bind;

			// Metrics & Visibility

			mainGrid.ColumnHeadersHeight = Global.Sett.Metrics.Header;
            mainGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", Global.Sett.Metrics.HeaderText);
            mainGrid.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", Global.Sett.Metrics.RowsValues);

            foreach(DataGridViewRow row in mainGrid.Rows)
                row.Height = Global.Sett.Metrics.Rows;

            mainGrid.ColumnHeadersVisible = Global.Sett.Visibility.Header;

			// Color

            mainGrid.RowsDefaultCellStyle.BackColor            = Global.Sett.Color.Background1;
            mainGrid.AlternatingRowsDefaultCellStyle.BackColor = Global.Sett.Color.Background2;
			
            mainGrid.ColumnHeadersDefaultCellStyle.ForeColor = Global.Sett.Color.HeaderText;
            mainGrid.ColumnHeadersDefaultCellStyle.BackColor = Global.Sett.Color.HeaderBackground;

            BackColor = Global.Sett.Color.Edge;

			// Coords

            StartPosition = FormStartPosition.Manual;
            Location = new Point(Global.Sett.Coords.PosX, Global.Sett.Coords.PosY);

            MouseDown -= neo.FormUtil.DragMove;
            mainGrid.MouseDown -= neo.FormUtil.DragMove;
            if(!Global.Sett.Coords.LockPos) {
                MouseDown += neo.FormUtil.DragMove;
                mainGrid.MouseDown += neo.FormUtil.DragMove;
            }

            // Open on Startup

            RegistryKey regKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if((regKey.GetValue("CryptoGadget", null) != null) != Global.Sett.Basic.Startup) {
                if(Global.Sett.Basic.Startup)
                    regKey.SetValue("CryptoGadget", "\"" + Application.ExecutablePath.ToString() + "\"");
                else
                    regKey.DeleteValue("CryptoGadget", false);
            }

        }
		private void RowsInit() {

			_coinGrid = new BindingList<CoinRow>();

			for(int i = 0; i < Global.Sett.Coins[_page].Count; i++) {

				Settings.StCoin st = Global.Sett.Coins[_page][i];
				CoinRow coin = new CoinRow();

				if(Global.Json != null) {
					st.CoinName   = Global.Json["Data"]?[st.Coin]?["CoinName"]?.ToString();
					st.TargetName = Global.Json["Data"]?[st.Target]?["CoinName"]?.ToString();
				}

				coin.Icon		= Global.GetIcon(st.Coin, Global.Sett.Metrics.IconSize);
				coin.Coin		= st.Coin;
				coin.TargetIcon = Global.GetIcon(st.Target, Global.Sett.Metrics.IconSize);
				coin.Target     = st.Target;

				_coinGrid.Add(coin);
			}

		}

		public FormMain() {

            InitializeComponent();

            notifyIcon.Text = typeof(FormMain).Assembly.GetName().Name + " " + typeof(FormMain).Assembly.GetName().Version;
            notifyIcon.Text = notifyIcon.Text.Remove(notifyIcon.Text.Length - 2);

			Load += (sender, e) => {
				
				if(Global.Json == null && File.Exists(Global.JsonLocation)) {
					Global.Json = JObject.Parse(new StreamReader(File.Open(Global.JsonLocation, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)).ReadToEnd());
					if(!Global.JsonIsValid(Global.Json))
						Global.Json = null;
				}

				try {
					using(StreamReader reader = new StreamReader(Global.IniLocation)) {
						Global.Profile = reader.ReadLine();
					}
				} catch(Exception exc) {
					Global.DbgMsgShow("ERROR: " + exc.Message);
					MessageBox.Show("The default profile can't be determinated because the \"default_profile.ini\" file is missing, the \"Default.json\" profile will be used");
					using(StreamWriter writer = new StreamWriter(Global.IniLocation)) {
						writer.WriteLine("Default.json");
					}
  				}
				
				if(!Global.Sett.BindFile(Global.ProfilesFolder + Global.Profile)) {
					MessageBox.Show("The last profile marked as default is not available, a new default profile will be created and used");
					Settings.CreateSettFile(Global.ProfilesFolder + "Default.json");
					Global.Sett.BindFile(Global.ProfilesFolder + "Default.json");
					Global.Sett.Default();
					Global.Sett.Store();
					Global.Sett.Save();

					using(StreamWriter writer = new StreamWriter(Global.IniLocation)) {
						writer.WriteLine("Default.json");
					}
					Global.Profile = "Default.json";
				}
				if(!Global.Sett.Load() || !Global.Sett.Check()) {
					MessageBox.Show("The settings file is corrupted, a new settings file with the default values will be used");
					Global.Sett.Default();
					Global.Sett.Store();
					Global.Sett.Save();
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

        private void mainGrid_SelectionChanged(object sender, EventArgs e) {
            mainGrid.ClearSelection();
        }

		private void mainGrid_RowContextMenuStripNeeded(object sender, DataGridViewRowContextMenuStripNeededEventArgs e) {

			ContextMenuStrip cm = new ContextMenuStrip();
			DataGridViewRow row = mainGrid.Rows[e.RowIndex];
			if(Global.Json != null) {
				JToken jtok = Global.Json["Data"][row.Cells["Coin"].Value];
				if(jtok != null && jtok["Url"] != null) {
					cm.Items.Add(new ToolStripMenuItem(row.Cells["Coin"].Value + " website", null, (ts_sender, ts_e) => Process.Start("https://www.cryptocompare.com" + jtok["Url"])));
					cm.Items.Add(new ToolStripSeparator());
				}
			}

			cm.Items.Add("Settings", null, contextMenuSettings_Click);
			cm.Items.Add("Hide", null, contextMenuHide_Click);
			cm.Items.Add("Exit", null, contextMenuExit_Click);

			e.ContextMenuStrip = cm;
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
