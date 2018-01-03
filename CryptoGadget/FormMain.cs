

/*
*	Author: neo3587
*	
*	Description: CryptoGadget is an application that provides a customizable interface to display the current
*				 price and some additional information of any cryptocurrency available in CryptoCompare.
*	
* 
*	Source code: https://github.com/neo3587/CryptoGadget
*	
*	This application is possible thanks to the CryptoCompare API: https://www.cryptocompare.com/api/
*/



using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics; 
using System.ComponentModel;

using Microsoft.Win32;
using Newtonsoft.Json.Linq;



namespace CryptoGadget {

    public partial class FormMain : Form {

		public class TimerRequest {
			private System.Threading.Timer _timer = null;
			private volatile bool _disposed = true;
			private Mutex _mutex = new Mutex();
			private Action<TimerRequest> _callback;
			
			public bool Disposed { get => _disposed; }

			public TimerRequest(Action<TimerRequest> callback) {
				_callback = callback;
			}

			public bool Start(int period) {
				if(!_disposed)
					return false;
				_disposed = false;
				_timer = new System.Threading.Timer((state) => {
					if(!_mutex.WaitOne(10)) { // prevent deadlock if Dispose() is called just before executing this lock
						return;
					}
					_callback(this);
					_mutex.ReleaseMutex();
				}, null, 0, period);
				return true;
			}
			public bool Kill(int wait = Timeout.Infinite) {
				if(_disposed)
					return false;
				using(ManualResetEvent reset_ev = new ManualResetEvent(false)) {
					_disposed = true;
					if(!_mutex.WaitOne(wait))
						return false;
					if(_timer.Dispose(reset_ev))
						reset_ev.WaitOne();
					_mutex.ReleaseMutex();
				}
				return true;
			}
		}
		public class CoinRow : PropManager<CoinRow> {
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

		private TimerRequest _timer_rows = null;
		private TimerRequest _timer_alert = null;
		private string _query_rows = ""; // full query -> current page
		private string _query_alert = ""; // basic query -> all pages
		private volatile bool _save_on_close = false;
		private int _page = 0;
		private BindingList<CoinRow> _coin_list = new BindingList<CoinRow>();
		private Settings.CoinList _alert_list = new Settings.CoinList();
		private Dictionary<(string, string), (FormChart, Thread)> _charts = new Dictionary<(string, string), (FormChart, Thread)>();
		private Mutex _charts_mtx = new Mutex();


		internal void ApplySettings() {
			_timer_rows.Kill();
			Point curr_loc = Location; // prevent the form realocation
			GridInit();
			ResizeForm();
			Location = curr_loc;
			_timer_rows.Start(Global.Sett.Basic.RefreshRate * 1000);
		}
		internal void SwapPage(int page) {

			_timer_rows.Kill();

			((contextMenu.Items[0] as ToolStripMenuItem).DropDownItems[_page] as ToolStripMenuItem).Checked = false;
			_page = page;
			((contextMenu.Items[0] as ToolStripMenuItem).DropDownItems[_page] as ToolStripMenuItem).Checked = true;

			RowsInit();
			mainGrid.DataSource = _coin_list;
			ResizeForm();

			_timer_rows.Start(Global.Sett.Basic.RefreshRate * 1000);
		}


		private void TimerRowsRoutine(TimerRequest state) {

			Func<double, int, string> AdaptValue = (val, maxDigit) => {
				int decimals = Math.Max(0, maxDigit - (int)Math.Floor(Math.Log10(Math.Max(1.0, Math.Abs(val))) + 1));
				return Math.Round(val, decimals).ToString("0." + new string('0', decimals));
			};

			List<double> last_values = new List<double>();
			foreach(CoinRow row in _coin_list)
				last_values.Add(double.Parse(row.Value));

			try {

				JObject json = CCRequest.HttpRequest(_query_rows);

				if(json != null && json["Response"]?.ToString().ToLower() != "error") {
					for(int i = 0; i < _coin_list.Count; i++) {
						try {
							JToken jtok = json["RAW"][Global.Sett.Coins[_page][i].Coin][Global.Sett.Coins[_page][i].Target];

							foreach(ValueTuple<string, string> tp in Settings.StGrid.jsget)
								_coin_list[i][tp.Item1] = AdaptValue(jtok[tp.Item2].ToObject<double>(), (Global.Sett.Grid[tp.Item1] as Settings.StColumn).Digits);
							_coin_list[i].LastMarket = jtok["LASTMARKET"].ToString(); // non-numeric column

							string[] changes = { "Change24", "Change24Pct", "ChangeDay", "ChangeDayPct" }; // special coloring and formatting columns
							foreach(string chg in changes) {
								if((_coin_list[i][chg] as string)[0] != '-')
									_coin_list[i][chg] = "+" + (_coin_list[i][chg] as string);
								mainGrid.Rows[i].Cells[chg].Style.ForeColor = (_coin_list[i][chg] as string)[0] == '+' ? Global.Sett.Color.PositiveChange : Global.Sett.Color.NegativeChange;
							}
							_coin_list[i].Change24Pct += "%";
							_coin_list[i].ChangeDayPct += "%";
						} catch { }
					}
				}

				if(Global.Sett.Visibility.Refresh) {
					Func<Color, Color, float, Color> ColorApply = (color, bg_color, opacity) => {
						byte[] byte_color = BitConverter.GetBytes(color.ToArgb());
						byte[] byte_bg_color = BitConverter.GetBytes(bg_color.ToArgb());
						for(int i = 0; i < 4; i++)
							byte_color[i] = (byte)(byte_bg_color[i] * opacity + byte_color[i] * (1 - opacity));
						return Color.FromArgb(BitConverter.ToInt32(byte_color, 0));
					};
					Action DefaultColors = () => {
						for(int i = 0; i < last_values.Count; i++)
							mainGrid.Rows[i].DefaultCellStyle.BackColor = i % 2 == 0 ? Global.Sett.Color.Background1 : Global.Sett.Color.Background2;
					};

					for(float opacity = 0.0f; opacity < 1.0f && !state.Disposed; opacity += 0.05f) {

						for(int i = 0; i < last_values.Count; i++) {

							Color bg_color = i % 2 == 0 ? Global.Sett.Color.Background1 : Global.Sett.Color.Background2;
							double current_value = double.Parse(_coin_list[i].Value);

							if(current_value > last_values[i])
								mainGrid.Rows[i].DefaultCellStyle.BackColor = ColorApply(Global.Sett.Color.PositiveRefresh, bg_color, opacity);
							else if(current_value < last_values[i])
								mainGrid.Rows[i].DefaultCellStyle.BackColor = ColorApply(Global.Sett.Color.NegativeRefresh, bg_color, opacity);

						}

						Thread.Sleep(60);
					}

					DefaultColors();
				}

			} catch { }

		}
		private void TimerAlertRoutine(TimerRequest state) {

			try {
				
				JObject json = CCRequest.HttpRequest(_query_alert);

				if(json != null && json["Response"]?.ToString().ToLower() != "error") {
					foreach(Settings.StCoin st in _alert_list) {
						decimal val = decimal.Parse(json[st.Coin][st.Target].ToString());
						Invoke((MethodInvoker)delegate {
							if(st.Alert.Above > 0.0m && val > st.Alert.Above) {
								notifyIcon.ShowBalloonTip(5000, "CryptoGadget", st.Coin + " -> " + st.Target + " current value: " + val + "\nAlarm Above was set at: " + st.Alert.Above, ToolTipIcon.None);
								st.Alert.Above = 0.0m;
								_save_on_close = true;
							}
							else if(st.Alert.Below > 0.0m && val < st.Alert.Below) {
								notifyIcon.ShowBalloonTip(5000, "CryptoGadget", st.Coin + " -> " + st.Target + " current value: " + val + "\nAlarm Below was set at: " + st.Alert.Below, ToolTipIcon.None);
								st.Alert.Below = 0.0m;
								_save_on_close = true;
							}
						});
					}
				}

			} catch { }
			 
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
			mainGrid.DataSource = null;
			mainGrid.Columns.Clear();

			// Rows & Columns init and binding

			RowsInit();

			foreach(Settings.StColumn st in Global.Sett.Grid.Columns) {
				DataGridViewColumn col = new DataGridViewColumn();
				col.HeaderText = st.Name; 
				col.Name = st.Column;
				col.DataPropertyName = st.Column;
				col.CellTemplate = new DataGridViewTextBoxCell();
				col.Visible = st.Enabled;
				col.Width = st.Width;
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

			mainGrid.DataSource = _coin_list;

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

            MouseDown -= Global.DragMove;
            mainGrid.MouseDown -= Global.DragMove;
            if(!Global.Sett.Coords.LockPos) {
                MouseDown += Global.DragMove;
                mainGrid.MouseDown += Global.DragMove;
            }

            // Open on Startup

            RegistryKey regKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if((regKey.GetValue("CryptoGadget", null) != null) != Global.Sett.Basic.Startup) {
                if(Global.Sett.Basic.Startup)
                    regKey.SetValue("CryptoGadget", "\"" + Application.ExecutablePath.ToString() + "\"");
                else
                    regKey.DeleteValue("CryptoGadget", false);
            }

			// Other

			_alert_list = Global.Sett.GetAlarmCoins();
			if(_alert_list.Count > 0)
				_query_alert = CCRequest.ConvertQueryBasic(_alert_list, Global.Sett.Market.Market);

		}
		private void RowsInit() {

			_coin_list = new BindingList<CoinRow>();

			for(int i = 0; i < Global.Sett.Coins[_page].Count; i++) {

				Settings.StCoin st = Global.Sett.Coins[_page][i];
				CoinRow row = new CoinRow();

				if(Global.Json != null) {
					st.CoinName = Global.Json["Data"]?[st.Coin]?["CoinName"]?.ToString();
					st.TargetName = Global.Json["Data"]?[st.Target]?["CoinName"]?.ToString();
				}

				row.Icon = Global.GetIcon(st.Coin, Global.Sett.Metrics.IconSize);
				row.Coin = st.Coin;
				row.TargetIcon = Global.GetIcon(st.Target, Global.Sett.Metrics.IconSize);
				row.Target = st.Target;

				_coin_list.Add(row);
			}

			_query_rows = CCRequest.ConvertQueryFull(Global.Sett.Coins[_page], Global.Sett.Market.Market);
		}


		public FormMain() {

            InitializeComponent();

            notifyIcon.Text = typeof(FormMain).Assembly.GetName().Name + " " + typeof(FormMain).Assembly.GetName().Version;
            notifyIcon.Text = notifyIcon.Text.Remove(notifyIcon.Text.Length - 2);

			Load += (sender, e) => {
				
				if(Global.Json == null && File.Exists(Global.CoinListLocation)) {
					Global.Json = JObject.Parse(new StreamReader(File.Open(Global.CoinListLocation, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)).ReadToEnd());
					if(!Global.JsonIsValid(Global.Json))
						Global.Json = null;
				}

				try {
					using(StreamReader reader = new StreamReader(Global.ProfileIniLocation)) {
						Global.Profile = reader.ReadLine();
					}
				} catch(Exception ex) {
					Global.DbgMsgShow("FormMain Constructor (default_profile.ini) ERROR:\n" + ex.Message);
					MessageBox.Show("The default profile can't be determinated because the \"default_profile.ini\" file is missing, the \"Default.json\" profile will be used");
					using(StreamWriter writer = new StreamWriter(Global.ProfileIniLocation)) {
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

					using(StreamWriter writer = new StreamWriter(Global.ProfileIniLocation)) {
						writer.WriteLine("Default.json");
					}
					Global.Profile = "Default.json";
				}
				if(!Global.Sett.Load() || !Global.Sett.Check()) {
					MessageBox.Show("The settings file is corrupted or outdated (not valid for this version), a new settings file with the default values will be used");
					Global.Sett.Default();
					Global.Sett.Store();
					Global.Sett.Save();
				}

				_page = Global.Sett.Pages.Default;

				GridInit();
                ResizeForm();

				for(int i = 0; i < 10; i++) {
					ToolStripMenuItem ts_item = new ToolStripMenuItem();
					ts_item.Text = "Page " + i;
					ts_item.Tag = i;
					ts_item.Click += (cm_sender, cm_ev) => {
						SwapPage((int)(cm_sender as ToolStripMenuItem).Tag);
					};
					toolStripPages.DropDownItems.Add(ts_item);
				}
				(toolStripPages.DropDownItems[_page] as ToolStripMenuItem).Checked = true;
				
				mainGrid.DoubleBuffered(true);
                FormBorderStyle = FormBorderStyle.None; // avoid alt-tab

				_timer_rows = new TimerRequest(TimerRowsRoutine);
				_timer_alert = new TimerRequest(TimerAlertRoutine);
				_timer_rows.Start(Global.Sett.Basic.RefreshRate * 1000);
				if(_alert_list.Count > 0) {
					_timer_alert.Start(Global.Sett.Basic.AlertCheckRate * 1000);
				}

            };

			#if DEBUG
			_charts.Add(("BTC", "USD"), (new FormChart("BTC", "USD"), new Thread(() => {
				_charts_mtx.WaitOne();
				FormChart chart = _charts[("BTC", "USD")].Item1;
				_charts_mtx.ReleaseMutex();
				chart.ShowDialog();
				_charts_mtx.WaitOne();
				_charts.Remove(("BTC", "USD"));
				_charts_mtx.ReleaseMutex();
			})));
			_charts[("BTC", "USD")].Item2.Start();
			#endif

		}

		private void toolStripSettings_Click(object sender, EventArgs e) {
			_timer_alert.Kill();
            FormSettings form2 = new FormSettings(this);
            form2.ShowDialog();
			if(_alert_list.Count > 0) {
				_timer_alert.Start(Global.Sett.Basic.AlertCheckRate * 1000);
			}
		}
        private void toolStripHide_Click(object sender, EventArgs e) {
            Visible = !Visible;
			toolStripHide.Checked = !Visible;
        }
        private void toolStripExit_Click(object sender, EventArgs e) {
            Close();
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e) {
            Activate();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e) {

			_timer_alert.Kill(500);
			_timer_rows.Kill(500);

			foreach((FormChart, Thread) chart in _charts.Values) {
				chart.Item1.Invoke((MethodInvoker)delegate { chart.Item1.Close(); });
			}

			if(Global.Sett.Coords.ExitSave && (Location.X != Global.Sett.Coords.PosX || Location.Y != Global.Sett.Coords.PosY)) {
				Global.Sett.Coords.PosX = Location.X;
				Global.Sett.Coords.PosY = Location.Y;
				_save_on_close = true;
			}
			if(Global.Sett.Pages.ExitSave && Global.Sett.Pages.Default != _page) {
				Global.Sett.Pages.Default = _page;
				_save_on_close = true;
			}

			if(_save_on_close) {
				Global.Sett.Store();
				Global.Sett.Save();
			}
        }

        private void mainGrid_SelectionChanged(object sender, EventArgs e) {
            mainGrid.ClearSelection();
        }
		private void mainGrid_RowContextMenuStripNeeded(object sender, DataGridViewRowContextMenuStripNeededEventArgs e) {

			ContextMenuStrip cm = new ContextMenuStrip();
			string coin   = mainGrid.Rows[e.RowIndex].Cells["Coin"].Value.ToString();
			string target = mainGrid.Rows[e.RowIndex].Cells["Target"].Value.ToString();

			if(Global.Json != null) {
				JToken jtok = Global.Json["Data"][coin];
				if(jtok != null && jtok["Url"] != null) 
					cm.Items.Add(new ToolStripMenuItem(coin + " website", null, (ts_sender, ts_e) => Process.Start("https://www.cryptocompare.com" + jtok["Url"])));
			}

			cm.Items.Add(new ToolStripMenuItem(coin + " → " + target + " chart", null, (ts_sender, ts_e) => {

				_charts_mtx.WaitOne();

				if(_charts.ContainsKey((coin, target))) {
					_charts[(coin, target)].Item1.Invoke((MethodInvoker)delegate { _charts[(coin, target)].Item1.Activate(); });
					_charts_mtx.ReleaseMutex();
					return;
				}

				_charts.Add((coin, target), (new FormChart(coin, target), new Thread(() => {
					_charts_mtx.WaitOne();
					FormChart chart = _charts[(coin, target)].Item1;
					_charts_mtx.ReleaseMutex();
					chart.ShowDialog();
					_charts_mtx.WaitOne();
					_charts.Remove((coin, target));
					_charts_mtx.ReleaseMutex();
				})));
				_charts[(coin, target)].Item2.Start();

				_charts_mtx.ReleaseMutex();
			}));
			cm.Items.Add(new ToolStripSeparator());

			ToolStripManager.Merge(contextMenu, cm);
			cm.Closing += (cm_sender, cm_e) => { // Avoid weird random flickers when closing the dropdown toolstrip
				if(toolStripPages.DropDown.Visible) {
					cm_e.Cancel = true;
					toolStripPages.DropDown.Closed += (ts_sender, ts_e) => cm.Close();
					toolStripPages.DropDown.Close();
				}
			};
			cm.Closed += (cm_sender, cm_e) => ToolStripManager.RevertMerge(cm, contextMenu); 

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
