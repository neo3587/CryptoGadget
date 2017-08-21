
using System;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;

using Newtonsoft.Json.Linq;
using Microsoft.Win32;
using IniParser.Model;

using neo;





/* IDEAS:
 - Graphs (will require a new tab)
 - Alarm when a configurable threshold is passed
*/

/* TODO:
 - Fix unable to have 2 instance of the same coin with differents target coins (new ini library required)
*/




namespace CryptoGadget {

    public partial class MainForm : Form {

        private System.Threading.Timer timerRequest;
        private volatile bool timerDisposed = false;

        private void TimerRoutine(object state) {

            try {
                timerRequest.Change(Timeout.Infinite, Timeout.Infinite);
            } catch(Exception) {
                return;
            }

            Stopwatch watch = Stopwatch.StartNew();

            Func<double, int, int, double> AdaptValue = (val, maxDigit, maxDecimal) => {
                int decimals = Math.Min(maxDecimal, maxDigit - (int)Math.Floor(Math.Log10(Math.Max(1.0, Math.Abs(val))) + 1));
                return Math.Round(val, Math.Max(0, Math.Min(decimals, 15)));
            };
            Func<double, int, int, string> AdaptValueStr = (val, maxDigit, maxDecimal) => {
                int decimals = Math.Min(maxDecimal, maxDigit - (int)Math.Floor(Math.Log10(Math.Max(1.0, Math.Abs(val))) + 1));
                return val.ToString("0." + new string('0', Math.Max(0, decimals)));
            };

            List<Tuple<double, double, double>> prices = new List<Tuple<double, double, double>>(); // < last_price, new_price, change >

            string[] inputs  = Enumerable.ToArray(Enumerable.Select(Data.converts, (t => t.Item1)));
            string[] outputs = Enumerable.ToArray(Enumerable.Select(Data.converts, (t => t.Item2)));
            string usePercent = Data.others.showPercentage ? "CHANGEPCT24HOUR" : "CHANGE24HOUR";

            try {
                JObject json = Common.HttpRequest(inputs, outputs, Data.visible.change);
                if(json == null || json["Response"]?.ToString().ToLower() == "error") {
                    prices.Add(new Tuple<double, double, double>(0.00, 0.00, 0.00));
                }
                else if(Data.visible.change) {
                    for(int i = 0; i < Data.converts.Count; i++) {
                        prices.Add(new Tuple<double, double, double>(double.Parse(coinGrid.Rows[i].Cells[2].Value.ToString()),
                            AdaptValue(json["RAW"][Data.converts[i].Item1][Data.converts[i].Item2]["PRICE"].ToObject<double>(), Data.others.maxValueDigits, Data.others.maxValueDecimals),
                            AdaptValue(json["RAW"][Data.converts[i].Item1][Data.converts[i].Item2][usePercent].ToObject<double>(), Data.others.maxChangeDigits, Data.others.maxChangeDecimals)));
                    }
                }
                else {
                    for(int i = 0; i < Data.converts.Count; i++) {
                        prices.Add(new Tuple<double, double, double>(double.Parse(coinGrid.Rows[i].Cells[2].Value.ToString()),
                            AdaptValue(json[Data.converts[i].Item1][Data.converts[i].Item2].ToObject<double>(), Data.others.maxValueDigits, Data.others.maxValueDecimals),
                            0));
                    }
                }
            } catch(Exception) { }

            for(int i = 0; i < prices.Count; i++) {
                coinGrid.Rows[i].Cells[2].Value = AdaptValueStr(prices[i].Item2, Data.others.maxValueDigits, Data.others.maxValueDecimals);
                coinGrid.Rows[i].Cells[3].Value = (prices[i].Item3 >= 0 ? "+" : "") + AdaptValueStr(prices[i].Item3, Data.others.maxChangeDigits, Data.others.maxChangeDecimals) + (Data.others.showPercentage ? "%" : "");
                coinGrid.Rows[i].Cells[3].Style.ForeColor = prices[i].Item3 >= 0.0 ? Data.colors.positiveChange : Data.colors.negativeChange;
            }

            if(Data.visible.refresh)
                TimerHighlight(prices);

            try {
                timerRequest.Change(Math.Max(0, Data.others.refreshRate - watch.ElapsedMilliseconds), Data.others.refreshRate);
            } catch(Exception) { }

        }

        private void TimerHighlight(List<Tuple<double, double, double>> prices) {

            Func<Color, Color, float, Color> ColorApply = (color, bgcolor, opacity) => {
                byte[] bytecolor = BitConverter.GetBytes(color.ToArgb());
                byte[] bytebgcolor = BitConverter.GetBytes(bgcolor.ToArgb());
                for(int i = 0; i < 4; i++)
                    bytecolor[i] = (byte)(bytebgcolor[i] * opacity + bytecolor[i] * (1 - opacity));
                return Color.FromArgb(BitConverter.ToInt32(bytecolor, 0));
            };
            Action DefaultColors = () => {
                for(int i = 0; i < prices.Count; i++)
                    coinGrid.Rows[i].DefaultCellStyle.BackColor = i % 2 == 0 ? Data.colors.background1 : Data.colors.background2;
            };

            for(float opacity = 0.0f; opacity < 1.0f; opacity += 0.05f) {

                if(timerDisposed) {
                    DefaultColors();
                    return;
                }

                for(int i = 0; i < prices.Count; i++) {

                    Color bgcolor = i % 2 == 0 ? Data.colors.background1 : Data.colors.background2;

                    if(prices[i].Item2 > prices[i].Item1) {
                        Color color = ColorApply(Data.colors.positiveRefresh, bgcolor, opacity);
                        coinGrid.Rows[i].DefaultCellStyle.BackColor = color;
                    }
                    else if(prices[i].Item2 < prices[i].Item1) {
                        Color color = ColorApply(Data.colors.negativeRefresh, bgcolor, opacity);
                        coinGrid.Rows[i].DefaultCellStyle.BackColor = color;
                    }
                }

                Thread.Sleep(60);
            }

            DefaultColors();

        }

        /// <summary>
        /// Saves the current XY coordinates to the ini file if "Save Position on Exit" is checked and the current coordinates are different than the saved.
        /// </summary>
        /// 
        /// <returns>
        /// true if the ini has been rewritten, false otherwise
        /// </returns>
        private bool SaveCoords() {
            if(Data.coords.exitSave && (Data.coords.startX != Location.X || Data.coords.startY != Location.Y)) {
                Common.ini["Coordinates"]["StartX"] = Location.X.ToString();
                Common.ini["Coordinates"]["StartY"] = Location.Y.ToString();
                new IniParser.FileIniDataParser().WriteFile(Common.iniLocation, Common.ini);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Resizes the MainForm objects size depending on ini["Metrics"]
        /// </summary>
        private void ResizeForm() {
            
            int X = 0;
            int Y = coinGrid.ColumnHeadersVisible ? coinGrid.ColumnHeadersHeight : 0;
            int edge = Data.visible.edge ? Data.metrics.edge : 0;

            foreach(DataGridViewColumn col in coinGrid.Columns)
                X += col.Visible ? col.Width : 0;
            foreach(DataGridViewRow row in coinGrid.Rows)
                Y += row.Height;

            coinGrid.Location = new Point(edge, edge);
            coinGrid.Size = new Size(X, Y);
            Size = new Size(X + edge * 2, Y + edge * 2);
        }
        /// <summary>
        /// Initializes every feature of the MainForm and checks if the ini file is correct (in case of bad manupulation from user)
        /// </summary>
        private void GridInit() {

            Func<IniData, IniData, bool> IsIniSubset = (sub, set) => {
                foreach(SectionData sect in set.Sections) {
                    if(!sub.Sections.ContainsSection(sect.SectionName))
                        return false;
                    foreach(KeyData key in sect.Keys)
                        if(!sub[sect.SectionName].ContainsKey(key.KeyName))
                            return false;
                }
                return true;
            };

            coinGrid.Rows.Clear();
            Data.converts.Clear();

            IniParser.FileIniDataParser parser = new IniParser.FileIniDataParser();

            try {
                Common.ini = parser.ReadFile(Common.iniLocation);
            } catch(Exception) {
                parser.WriteFile(Common.iniLocation, Common.DefaultIni());
                Common.ini = parser.ReadFile(Common.iniLocation);
            }

            IniData checkIni = Common.DefaultIni(null);
            checkIni.Sections["Coins"].RemoveAllKeys();

            if(!IsIniSubset(Common.ini, checkIni)) {
                MessageBox.Show("Corrupted settings.ini file, default values will be used", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                parser.WriteFile(Common.iniLocation, Common.DefaultIni());
                Common.ini = parser.ReadFile(Common.iniLocation);
            }

            try {

                #region Data Struct Init & Integrity Check 

                Func<string, string, Func<T, bool>, T> AssignRule<T>() where T : IConvertible {
                    return (sect, key, fn) => {
                        T value = (T)Convert.ChangeType(Common.ini[sect][key], default(T).GetTypeCode());
                        if(!fn(value))
                            throw new Exception();
                        return value;
                    };
                };

                foreach(KeyData conv in Common.ini["Coins"]) 
                    Data.converts.Add(new Tuple<string, string>(conv.KeyName, conv.Value));

                Data.others.refreshRate = (int)(AssignRule<decimal>()("Others", "RefreshRate", (obj) => obj >= 0.0m) * 1000);
                Data.others.openStartup = bool.Parse(Common.ini["Others"]["OpenStartup"]);

                Data.others.maxValueDigits    = AssignRule<int>()("Others", "MaxValueDigits",    (obj) => obj >= 0);
                Data.others.maxValueDecimals  = AssignRule<int>()("Others", "MaxValueDecimals",  (obj) => obj >= 0);
                Data.others.maxChangeDigits   = AssignRule<int>()("Others", "MaxChangeDigits",   (obj) => obj >= 0);
                Data.others.maxChangeDecimals = AssignRule<int>()("Others", "MaxChangeDecimals", (obj) => obj >= 0);

                Data.others.showPercentage = bool.Parse(Common.ini["Others"]["ShowPercentage"]);

                Data.visible.icon    = bool.Parse(Common.ini["Visibility"]["Icon"]);
                Data.visible.coin    = bool.Parse(Common.ini["Visibility"]["Coin"]);
                Data.visible.value   = bool.Parse(Common.ini["Visibility"]["Value"]);
                Data.visible.change  = bool.Parse(Common.ini["Visibility"]["Change"]);
                Data.visible.header  = bool.Parse(Common.ini["Visibility"]["Header"]);
                Data.visible.edge    = bool.Parse(Common.ini["Visibility"]["Edge"]);
                Data.visible.refresh = bool.Parse(Common.ini["Visibility"]["Refresh"]);

                Data.metrics.icon     = AssignRule<int>()("Metrics", "Icon",     (obj) => obj >= 0);
                Data.metrics.coin     = AssignRule<int>()("Metrics", "Coin",     (obj) => obj >= 0);
                Data.metrics.value    = AssignRule<int>()("Metrics", "Value",    (obj) => obj >= 0);
                Data.metrics.change   = AssignRule<int>()("Metrics", "Change",   (obj) => obj >= 0);
                Data.metrics.edge     = AssignRule<int>()("Metrics", "Edge",     (obj) => obj >= 0);
                Data.metrics.header   = AssignRule<int>()("Metrics", "Header",   (obj) => obj >= 0);
                Data.metrics.rows     = AssignRule<int>()("Metrics", "Rows",     (obj) => obj >= 0);
                Data.metrics.iconSize = AssignRule<int>()("Metrics", "IconSize", (obj) => obj >= 0);
                Data.metrics.text     = AssignRule<float>()("Metrics", "Text",    (obj) => obj >= 0.0f);
                Data.metrics.numbers  = AssignRule<float>()("Metrics", "Numbers", (obj) => obj >= 0.0f);

                Data.coords.startX = int.Parse(Common.ini["Coordinates"]["StartX"]);
                Data.coords.startY = int.Parse(Common.ini["Coordinates"]["StartY"]);
                Data.coords.exitSave     = bool.Parse(Common.ini["Coordinates"]["ExitSave"]);
                Data.coords.lockPosition = bool.Parse(Common.ini["Coordinates"]["LockPosition"]);

                Data.colors.coins            = Common.StrHexToColor(Common.ini["Colors"]["Coins"]);
                Data.colors.values           = Common.StrHexToColor(Common.ini["Colors"]["Values"]);
                Data.colors.background1      = Common.StrHexToColor(Common.ini["Colors"]["BackGround1"]);
                Data.colors.background2      = Common.StrHexToColor(Common.ini["Colors"]["BackGround2"]);
                Data.colors.positiveRefresh  = Common.StrHexToColor(Common.ini["Colors"]["PositiveRefresh"]);
                Data.colors.negativeRefresh  = Common.StrHexToColor(Common.ini["Colors"]["NegativeRefresh"]);
                Data.colors.edge             = Common.StrHexToColor(Common.ini["Colors"]["Edge"]);
                Data.colors.positiveChange   = Common.StrHexToColor(Common.ini["Colors"]["PositiveChange"]);
                Data.colors.negativeChange   = Common.StrHexToColor(Common.ini["Colors"]["NegativeChange"]);
                Data.colors.headerText       = Common.StrHexToColor(Common.ini["Colors"]["HeaderText"]);
                Data.colors.headerBackGround = Common.StrHexToColor(Common.ini["Colors"]["HeaderBackGround"]);

                #endregion

                #region Coin Rows Init

                for(int i = 0; i < Data.converts.Count; i++) {
                  
                    int size = Data.metrics.iconSize;
                    Bitmap bmp = new Bitmap(size, size);

                    // Minimum quality loss resize
                    using(Graphics gr = Graphics.FromImage(bmp)) { 
                        gr.SmoothingMode     = SmoothingMode.HighQuality;
                        gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        gr.PixelOffsetMode   = PixelOffsetMode.HighQuality;
                        gr.DrawImage(Common.GetIcon(Data.converts[i].Item1), new Rectangle(0, 0, size, size)); 
                    }

                    coinGrid.Rows.Add(bmp, Data.converts[i].Item1, 0.00, 0.00);

                    //ContextMenuStrip buff = new ContextMenuStrip();
                    //coinGrid.Rows[i].ContextMenuStrip = buff;
                }

                #endregion

                #region Metrics & Visibility

                coinGrid.Columns[0].Width = Data.metrics.icon;
                coinGrid.Columns[1].Width = Data.metrics.coin;
                coinGrid.Columns[2].Width = Data.metrics.value;
                coinGrid.Columns[3].Width = Data.metrics.change;
                coinGrid.ColumnHeadersHeight = Data.metrics.header;
                coinGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", Data.metrics.text);
                coinGrid.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", Data.metrics.numbers);

                foreach(DataGridViewRow row in coinGrid.Rows)
                    row.Height = Data.metrics.rows;

                coinGrid.Columns[0].Visible = Data.visible.icon;
                coinGrid.Columns[1].Visible = Data.visible.coin;
                coinGrid.Columns[2].Visible = Data.visible.value;
                coinGrid.Columns[3].Visible = Data.visible.change;
                coinGrid.ColumnHeadersVisible = Data.visible.header;

                #endregion

                #region Color Init

                coinGrid.RowsDefaultCellStyle.BackColor = Data.colors.background1;
                coinGrid.AlternatingRowsDefaultCellStyle.BackColor = Data.colors.background2;
                coinGrid.Columns[1].DefaultCellStyle.ForeColor = Data.colors.coins;
                coinGrid.Columns[2].DefaultCellStyle.ForeColor = Data.colors.values;

                coinGrid.ColumnHeadersDefaultCellStyle.ForeColor = Data.colors.headerText;
                coinGrid.ColumnHeadersDefaultCellStyle.BackColor = Data.colors.headerBackGround;

                BackColor = Data.colors.edge;

                #endregion

                #region Coordinates Init

                StartPosition = FormStartPosition.Manual;
                Location = new Point(Data.coords.startX, Data.coords.startY);

                MouseDown -= FormUtil.DragMove;
                coinGrid.MouseDown -= FormUtil.DragMove;
                if(!Data.coords.lockPosition) {
                    MouseDown += FormUtil.DragMove;
                    coinGrid.MouseDown += FormUtil.DragMove;
                }

                #endregion

                #region Other Stuff Init

                // bandaid for initial selection (clearSelection isn't working at all)
                if(coinGrid.RowCount > 0) { 
                    for(int i = 0; i < coinGrid.Rows[0].Cells.Count; i++) {
                        if(coinGrid.Rows[0].Cells[i].Visible) {
                            coinGrid.Rows[0].Cells[i].Selected = true;
                            coinGrid.Rows[0].Cells[i].Selected = false;
                            break;
                        }
                    }
                }

                // Open on Startup
                RegistryKey regKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if((regKey.GetValue("CryptoGadget", null) != null) != Data.others.openStartup) {
                    if(Data.others.openStartup)
                        regKey.SetValue("CryptoGadget", "\"" + Application.ExecutablePath.ToString() + "\"");
                    else
                        regKey.DeleteValue("CryptoGadget", false);
                }


                #endregion

            } catch(Exception) {
                MessageBox.Show("Corrupted settings.ini file, default values will be used", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                parser.WriteFile(Common.iniLocation, Common.DefaultIni());
                Common.ini = parser.ReadFile(Common.iniLocation);
                GridInit();
            }

        }



        public MainForm() {

            InitializeComponent();

            notifyIcon.Text = typeof(MainForm).Assembly.GetName().Name + " " + typeof(MainForm).Assembly.GetName().Version;
            notifyIcon.Text = notifyIcon.Text.Remove(notifyIcon.Text.Length - 2);

            Load += (sender, e) => {
                GridInit();
                ResizeForm();

                coinGrid.DoubleBuffered(true);
                FormBorderStyle = FormBorderStyle.None; // avoid alt-tab

                timerRequest = new System.Threading.Timer(TimerRoutine, null, 0, Data.others.refreshRate);
            };
        }

        private void contextMenuSettings_Click(object sender, EventArgs e) {

            WaitHandle wait = new AutoResetEvent(false);
            timerRequest.Dispose(wait);
            timerDisposed = true;

            SettingsForm form2 = new SettingsForm(this);
            form2.ShowDialog();

            wait.WaitOne();

            if(form2.accept) {
                if(!SaveCoords())
                    new IniParser.FileIniDataParser().WriteFile(Common.iniLocation, Common.ini);
                GridInit();
                ResizeForm();
            }

            timerDisposed = false;
            timerRequest = new System.Threading.Timer(TimerRoutine, null, 0, Data.others.refreshRate);
        }
        private void contextMenuHide_Click(object sender, EventArgs e) {
            Visible = !Visible;
            hideStripMenuItem.Text = Visible ? "Hide" : "Show";
        }
        private void contextMenuExit_Click(object sender, EventArgs e) {
            Close();
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e) {
            Activate();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e) {
            SaveCoords();
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
