

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
using System.Reflection;
using System.Runtime.InteropServices;

using Newtonsoft.Json.Linq;
using Microsoft.Win32;



namespace CryptoGadget {

    public partial class FormMain : Form {

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
		
        private System.Threading.Timer _timer_req = null;
        private volatile bool _timer_disposed = false;
		private Mutex _timer_mtx = new Mutex();
		private string _query = "";
		private int _page = 0;
		private BindingList<CoinRow> _coin_list = new BindingList<CoinRow>();

		internal void ApplySettings() {
			TimerRoutineKill();
			Point curr_loc = Location; // prevent the form realocation
			GridInit();
			ResizeForm();
			Location = curr_loc;
			TimerRoutineStart();
		}
		internal void SwapPage(int page) {

			TimerRoutineKill();

			((contextMenu.Items[0] as ToolStripMenuItem).DropDownItems[_page] as ToolStripMenuItem).Checked = false;
			_page = page;
			((contextMenu.Items[0] as ToolStripMenuItem).DropDownItems[_page] as ToolStripMenuItem).Checked = true;

			RowsInit();
			mainGrid.DataSource = _coin_list;
			ResizeForm();

			TimerRoutineStart();
		}

		private void TimerRoutineStart() {
			_timer_disposed = false;
			_timer_req = new System.Threading.Timer(TimerRoutine, null, 0, Global.Sett.Basic.RefreshRate * 1000);
		}
		private void TimerRoutineKill() {
			using(ManualResetEvent wait = new ManualResetEvent(false)) {
				_timer_disposed = true;
				_timer_mtx.WaitOne();
				if(_timer_req.Dispose(wait)) 
					wait.WaitOne();
				_timer_mtx.ReleaseMutex();
			}
		}

		private void TimerRoutine(object state) {

			if(!_timer_mtx.WaitOne(10)) // prevent deadlock if Dispose() is called just before executing this lock
				return;

			Func<double, int, string> AdaptValue = (val, maxDigit) => {
				int decimals = Math.Max(0, maxDigit - (int)Math.Floor(Math.Log10(Math.Max(1.0, Math.Abs(val))) + 1));
				return Math.Round(val, decimals).ToString("0." + new string('0', decimals));
			};

			List<double> last_values = new List<double>();
			foreach(CoinRow row in _coin_list)
				last_values.Add(double.Parse(row.Value));

			try {

				JObject json = CCRequest.HttpRequest(_query);

				if(json != null && json["Response"]?.ToString().ToLower() != "error") {
					for(int i = 0; i < _coin_list.Count; i++) {
						try {
							JToken jtok = json["RAW"][Global.Sett.Coins[_page][i].Coin][Global.Sett.Coins[_page][i].Target];

							foreach(ValueTuple<string, string> tp in Settings.StGrid.jsget)
								_coin_list[i][tp.Item1] = AdaptValue(jtok[tp.Item2].ToObject<double>(), (Global.Sett.Grid[tp.Item1] as Settings.StColumn).Digits);
							_coin_list[i].LastMarket = jtok["LASTMARKET"].ToString();

							string[] changes = { "Change24", "Change24Pct", "ChangeDay", "ChangeDayPct" };
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

				Invoke((MethodInvoker)delegate { Refresh(); });

				if(Global.Sett.Visibility.Refresh)
					TimerHighlight(last_values);
			} catch { }

			_timer_mtx.ReleaseMutex();

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

            for(float opacity = 0.0f; opacity < 1.0f && !_timer_disposed; opacity += 0.05f) {

                for(int i = 0; i < last_values.Count; i++) {

                    Color bgcolor = i % 2 == 0 ? Global.Sett.Color.Background1 : Global.Sett.Color.Background2;
                    double currentValue = double.Parse(_coin_list[i].Value);

                    if(currentValue > last_values[i]) 
						mainGrid.Rows[i].DefaultCellStyle.BackColor = ColorApply(Global.Sett.Color.PositiveRefresh, bgcolor, opacity);
                    else if(currentValue < last_values[i]) 
                        mainGrid.Rows[i].DefaultCellStyle.BackColor = ColorApply(Global.Sett.Color.NegativeRefresh, bgcolor, opacity);

				}

				Thread.Sleep(60);
            }

            DefaultColors();

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

			_query = CCRequest.ConvertQuery(Global.Sett.Coins[_page]);
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
						Global.Binds.Profile = reader.ReadLine();
					}
				} catch(Exception exc) {
					Global.DbgMsgShow("ERROR: " + exc.Message);
					MessageBox.Show("The default profile can't be determinated because the \"default_profile.ini\" file is missing, the \"Default.json\" profile will be used");
					using(StreamWriter writer = new StreamWriter(Global.ProfileIniLocation)) {
						writer.WriteLine("Default.json");
					}
  				}
				
				if(!Global.Sett.BindFile(Global.ProfilesFolder + Global.Binds.Profile)) {
					MessageBox.Show("The last profile marked as default is not available, a new default profile will be created and used");
					Settings.CreateSettFile(Global.ProfilesFolder + "Default.json");
					Global.Sett.BindFile(Global.ProfilesFolder + "Default.json");
					Global.Sett.Default();
					Global.Sett.Store();
					Global.Sett.Save();

					using(StreamWriter writer = new StreamWriter(Global.ProfileIniLocation)) {
						writer.WriteLine("Default.json");
					}
					Global.Binds.Profile = "Default.json";
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
					(contextMenu.Items[0] as ToolStripMenuItem).DropDownItems.Add(ts_item);
				}
				((contextMenu.Items[0] as ToolStripMenuItem).DropDownItems[_page] as ToolStripMenuItem).Checked = true;
				
				mainGrid.DoubleBuffered(true);
                FormBorderStyle = FormBorderStyle.None; // avoid alt-tab

				TimerRoutineStart();
            };

        }

        private void toolStripSettings_Click(object sender, EventArgs e) {
            FormSettings form2 = new FormSettings(this);
            form2.ShowDialog();
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

			_timer_disposed = true;
			_timer_mtx.WaitOne(500);

			bool change = false;

			if(Global.Sett.Coords.ExitSave && (Location.X != Global.Sett.Coords.PosX || Location.Y != Global.Sett.Coords.PosY)) {
				Global.Sett.Coords.PosX = Location.X;
				Global.Sett.Coords.PosY = Location.Y;
				change = true;
			}
			if(Global.Sett.Pages.ExitSave && Global.Sett.Pages.Default != _page) {
				Global.Sett.Pages.Default = _page;
				change = true;
			}

			if(change) {
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

			ToolStripManager.Merge(contextMenu, cm);
			cm.Closing += (cm_sender, cm_e) => { // Avoid weird random flickers when closing the dropdown toolstrip
				if((cm.Items[2] as ToolStripMenuItem).DropDown.Visible) {
					cm_e.Cancel = true;
					(cm.Items[2] as ToolStripMenuItem).DropDown.Closed += (ts_sender, ts_e) => cm.Close();
					(cm.Items[2] as ToolStripMenuItem).DropDown.Close();
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
