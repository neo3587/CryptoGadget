
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;

using Newtonsoft.Json.Linq;
using Microsoft.Win32;
using neo;



/* IDEAS:
 - Support more APIs (start with crytocompare?)
 - use struct of primitive types instead of accessing and parsing the ini file  
 - Individual target coin
 - Graphs (will require a new tab)
*/




namespace CryptoGadget {

    public partial class MainForm : Form {

        private System.Threading.Timer timerRequest;
        private volatile bool timerDisposed = false;

        internal void TimerRoutine(object state) {

            try {
                timerRequest.Change(Timeout.Infinite, Timeout.Infinite);
            } catch(Exception) {
                return;
            }

            Stopwatch watch = Stopwatch.StartNew();

            Func<double, int, int, double> AdaptValue = (val, maxDigit, maxDecimal) => {
                int decimals = maxDigit - (int)Math.Floor(Math.Log10(Math.Abs(val) < 1.0 ? 1.0 : Math.Abs(val)) + 1);
                return Math.Round(val, decimals <= maxDecimal ? decimals : maxDecimal);
            };
            Func<double, int, int, string> AdaptValueStr = (val, maxDigit, maxDecimal) => {
                int decimals = maxDigit - (int)Math.Floor(Math.Log10(Math.Abs(val) < 1.0 ? 1.0 : Math.Abs(val)) + 1);
                return val.ToString("0." + new string('0', decimals <= maxDecimal ? decimals : maxDecimal));
            };

            List<string> coins = new List<string>();
            foreach(DataGridViewRow row in coinGrid.Rows)
                coins.Add(row.Cells[1].Value.ToString());

            List<Tuple<double, double, double>> prices = new List<Tuple<double, double, double>>(); // < last_price, new_price, change >

            int maxValDig = int.Parse(Common.ini["Others"]["MaxValueDigits"]), maxValDec = int.Parse(Common.ini["Others"]["MaxValueDecimals"]);
            int maxChgDig = int.Parse(Common.ini["Others"]["MaxChangeDigits"]), maxChgDec = int.Parse(Common.ini["Others"]["MaxChangeDecimals"]);

            int errCount = 0;

            for(int i = 0; i < coins.Count;) {

                if(timerDisposed)
                    return;

                if(i >= 10)
                    Thread.Sleep(200); // the server allows up to 10 consecutive requests, then it will only allow 1 per ~200ms

                if(timerDisposed)
                    return;

                try {
                    JObject json = Common.HttpRequest(coins[i], Common.ini["Others"]["TargetCoin"]);
                    if(json["success"].ToString().ToLower() == "false" || json["ticker"] == null) {
                        prices.Add(new Tuple<double, double, double>(0.00, 0.00, 0.00));
                    }
                    else {
                        prices.Add(new Tuple<double, double, double>(double.Parse(coinGrid.Rows[i].Cells[2].Value.ToString()),
                            AdaptValue(json["ticker"]["price"].ToObject<double>(), maxValDig, maxValDec),
                            AdaptValue(json["ticker"]["change"].ToObject<double>(), maxChgDig, maxChgDec)));
                    }
                    i++;
                } catch(Exception) {
                    if(errCount++ == 10) {
                        prices.Add(new Tuple<double, double, double>(0.00, 0.00, 0.00));
                        errCount = 0;
                        i++;
                    }
                }
            }

            for(int i = 0; i < prices.Count; i++) {
                coinGrid.Rows[i].Cells[2].Value = AdaptValueStr(prices[i].Item2, maxValDig, maxValDec);
                coinGrid.Rows[i].Cells[3].Value = (prices[i].Item3 >= 0 ? "+" : "") + AdaptValueStr(prices[i].Item3, maxChgDig, maxChgDec);
                coinGrid.Rows[i].Cells[3].Style.ForeColor = Common.StrHexToColor(Common.ini["Colors"][prices[i].Item3 >= 0.0 ? "PositiveChange" : "NegativeChange"]);
            }

            if(bool.Parse(Common.ini["Visibility"]["Refresh"]))
                TimerHighlight(prices);

            int period = (int)(decimal.Parse(Common.ini["Others"]["RefreshRate"]) * 1000);
            try {
                timerRequest.Change(Math.Max(0, period - watch.ElapsedMilliseconds), period);
            } catch(Exception) { }

        }

        private void TimerHighlight(List<Tuple<double, double, double>> prices) {

            Func<Color, Color, float, Color> ColorApply = (color, bgcolor, opacity) => {
                byte[] bytecolor = BitConverter.GetBytes(color.ToArgb());
                byte[] bytebgcolor = BitConverter.GetBytes(bgcolor.ToArgb());
                for(int i = 0; i < 4; i++)
                    bytecolor[i] = (byte)(bytebgcolor[i] * opacity + bytecolor[i] * (1 - opacity));
                int newcolor = BitConverter.ToInt32(bytecolor, 0);
                return Color.FromArgb(newcolor);
            };

            for(float opacity = 0.0f; opacity < 0.9999f; opacity += 0.05f) {

                if(timerDisposed) {
                    for(int i = 0; i < prices.Count; i++) {
                        Color bgcolor = Common.StrHexToColor(Common.ini["Colors"][i % 2 == 0 ? "BackGround1" : "BackGround2"]);
                        coinGrid.Rows[i].DefaultCellStyle.BackColor = bgcolor;
                    }
                    return;
                }

                for(int i = 0; i < prices.Count; i++) {

                    Color bgcolor = Common.StrHexToColor(Common.ini["Colors"][i % 2 == 0 ? "BackGround1" : "BackGround2"]);

                    if(prices[i].Item2 > prices[i].Item1) {
                        Color color = ColorApply(Common.StrHexToColor(Common.ini["Colors"]["PositiveRefresh"]), bgcolor, opacity);
                        coinGrid.Rows[i].DefaultCellStyle.BackColor = color;
                    }
                    else if(prices[i].Item2 < prices[i].Item1) {
                        Color color = ColorApply(Common.StrHexToColor(Common.ini["Colors"]["NegativeRefresh"]), bgcolor, opacity);
                        coinGrid.Rows[i].DefaultCellStyle.BackColor = color;
                    }
                }

                Thread.Sleep(60);
            }

        }

        /// <summary>
        /// Saves the current XY coordinates to the ini file if "Save Position on Exit" is checked and the current coordinates are different than the saved.
        /// </summary>
        /// 
        /// <returns>
        /// true if the ini has been rewritten, false otherwise
        /// </returns>
        private bool SaveCoords() {
            if(bool.Parse(Common.ini["Coordinates"]["ExitSave"]) && (int.Parse(Common.ini["Coordinates"]["StartX"]) != Location.X || int.Parse(Common.ini["Coordinates"]["StartY"]) != Location.Y)) {
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
        internal void ResizeForm() {
            
            int X = 0;
            int Y = coinGrid.ColumnHeadersVisible ? coinGrid.ColumnHeadersHeight : 0;
            int edge = bool.Parse(Common.ini["Visibility"]["Edge"]) ? int.Parse(Common.ini["Metrics"]["Edge"]) : 0;

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
        internal void GridInit() {

            Func<IniParser.Model.IniData, IniParser.Model.IniData, bool> IsIniSubset = (sub, set) => {
                foreach(IniParser.Model.SectionData sect in set.Sections) {
                    if(!sub.Sections.ContainsSection(sect.SectionName))
                        return false;
                    foreach(IniParser.Model.KeyData key in sect.Keys)
                        if(!sub[sect.SectionName].ContainsKey(key.KeyName))
                            return false;
                }
                return true;
            };

            coinGrid.Rows.Clear();

            IniParser.FileIniDataParser parser = new IniParser.FileIniDataParser();

            try {
                Common.ini = parser.ReadFile(Common.iniLocation);
            } catch(Exception) {
                parser.WriteFile(Common.iniLocation, Common.DefaultIni());
                Common.ini = parser.ReadFile(Common.iniLocation);
            }

            IniParser.Model.IniData checkIni = Common.DefaultIni(null, Common.DefaultType.Basic | Common.DefaultType.Advanced | Common.DefaultType.ColorsLight);
            checkIni.Sections["Coins"].RemoveAllKeys();

            if(!IsIniSubset(Common.ini, checkIni)) {
                MessageBox.Show("Corrupted settings.ini file, default values will be used", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                parser.WriteFile(Common.iniLocation, Common.DefaultIni());
                Common.ini = parser.ReadFile(Common.iniLocation);
            }

            try {

                #region Integrity Check 

                // throw exception if cannot be converted and check if the value is correct
                Action<string, string[], Func<string, bool>> IntegrityCheck = (strsect, strkeys, fncmp) => {
                    IniParser.Model.KeyDataCollection sect = Common.ini[strsect];
                    foreach(string key in strkeys)
                        if(!fncmp(sect[key]))
                            throw new Exception();
                };
                // throw exception if cannot be converted
                Action<string, string[], Action<string>> ConversionCheck = (strsect, strkeys, fncmp) => {
                    IniParser.Model.KeyDataCollection sect = Common.ini[strsect];
                    foreach(string key in strkeys)
                        fncmp(sect[key]);
                };

                IntegrityCheck("Metrics", new string[] { "Icon", "Coin", "Value", "Change", "Edge", "Header", "Rows", "IconSize" }, (str) => int.Parse(str) >= 0);
                IntegrityCheck("Metrics", new string[] { "Text", "Numbers" }, (str) => float.Parse(str) > 0.0f);
                IntegrityCheck("Others", new string[] { "MaxValueDigits", "MaxValueDecimals", "MaxChangeDigits", "MaxChangeDecimals" }, (str) => int.Parse(str) >= 0);
                IntegrityCheck("Others", new string[] { "RefreshRate" }, (str) => decimal.Parse(str) > 0);

                ConversionCheck("Visibility", new string[] { "Icon", "Coin", "Value", "Change", "Header", "Edge", "Refresh" }, (str) => bool.Parse(str));
                ConversionCheck("Coordinates", new string[] { "StartX", "StartY" }, (str) => int.Parse(str));
                ConversionCheck("Coordinates", new string[] { "ExitSave", "LockPosition" }, (str) => bool.Parse(str));
                ConversionCheck("Colors", new string[] { "Coins", "Values","BackGround1", "BackGround2", "PositiveRefresh", "NegativeRefresh", "Edge",
                                                         "PositiveChange", "NegativeChange", "HeaderText", "HeaderBackGround" }, (str) => Common.StrHexToColor(str));
                ConversionCheck("Others", new string[] { "OpenStartup" }, (str) => bool.Parse(str));

                #endregion

                #region Coin Rows Init

                foreach(var coin in Common.ini["Coins"]) {
                    try {
                        int size = int.Parse(Common.ini["Metrics"]["IconSize"]);
                        Bitmap bmp = new Bitmap(size,size);

                        // Minimum quality loss resize
                        using(Graphics gr = Graphics.FromImage(bmp)) { 
                            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                            gr.DrawImage(new Icon(Common.iconLocation + coin.Value + ".ico").ToBitmap(), new Rectangle(0, 0, size, size));
                        }

                        coinGrid.Rows.Add(bmp, coin.Value, 0.00, 0.00);
                    } catch(Exception) {
                        coinGrid.Rows.Add(new Bitmap(1, 1), coin.Value, 0.00, 0.00);
                    }
                }

                #endregion

                #region Metrics & Visibility

                coinGrid.Columns[0].Width = int.Parse(Common.ini["Metrics"]["Icon"]);
                coinGrid.Columns[1].Width = int.Parse(Common.ini["Metrics"]["Coin"]);
                coinGrid.Columns[2].Width = int.Parse(Common.ini["Metrics"]["Value"]);
                coinGrid.Columns[3].Width = int.Parse(Common.ini["Metrics"]["Change"]);
                coinGrid.ColumnHeadersHeight = int.Parse(Common.ini["Metrics"]["Header"]);
                coinGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", float.Parse(Common.ini["Metrics"]["Text"]));
                coinGrid.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", float.Parse(Common.ini["Metrics"]["Numbers"]));

                foreach(DataGridViewRow row in coinGrid.Rows)
                    row.Height = int.Parse(Common.ini["Metrics"]["Rows"]);

                coinGrid.Columns[0].Visible = bool.Parse(Common.ini["Visibility"]["Icon"]);
                coinGrid.Columns[1].Visible = bool.Parse(Common.ini["Visibility"]["Coin"]);
                coinGrid.Columns[2].Visible = bool.Parse(Common.ini["Visibility"]["Value"]);
                coinGrid.Columns[3].Visible = bool.Parse(Common.ini["Visibility"]["Change"]);
                coinGrid.ColumnHeadersVisible = bool.Parse(Common.ini["Visibility"]["Header"]);

                #endregion

                #region Color Init

                coinGrid.RowsDefaultCellStyle.BackColor = Common.StrHexToColor(Common.ini["Colors"]["BackGround1"]);
                coinGrid.AlternatingRowsDefaultCellStyle.BackColor = Common.StrHexToColor(Common.ini["Colors"]["BackGround2"]);
                //coinGrid.DefaultCellStyle.ForeColor = Common.StrHexToColor(Common.ini["Colors"]["Coins"]);
                coinGrid.Columns[1].DefaultCellStyle.ForeColor = Common.StrHexToColor(Common.ini["Colors"]["Coins"]);
                coinGrid.Columns[2].DefaultCellStyle.ForeColor = Common.StrHexToColor(Common.ini["Colors"]["Values"]);


                coinGrid.ColumnHeadersDefaultCellStyle.ForeColor = Common.StrHexToColor(Common.ini["Colors"]["HeaderText"]);
                coinGrid.ColumnHeadersDefaultCellStyle.BackColor = Common.StrHexToColor(Common.ini["Colors"]["HeaderBackGround"]);

                BackColor = Common.StrHexToColor(Common.ini["Colors"]["Edge"]);

                #endregion

                #region Coordinates Init

                StartPosition = FormStartPosition.Manual;
                Location = new Point(int.Parse(Common.ini["Coordinates"]["StartX"]), int.Parse(Common.ini["Coordinates"]["StartY"]));

                MouseDown -= FormUtil.DragMove;
                coinGrid.MouseDown -= FormUtil.DragMove;
                if(!bool.Parse(Common.ini["Coordinates"]["LockPosition"])) {
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

                RegistryKey regKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if((regKey.GetValue("CryptoGadget", null) != null) != bool.Parse(Common.ini["Others"]["OpenStartup"])) {
                    if(bool.Parse(Common.ini["Others"]["OpenStartup"]))
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

            Load += (sender, e) => {
                GridInit();
                ResizeForm();

                coinGrid.DoubleBuffered(true);
                FormBorderStyle = FormBorderStyle.None; // avoid alt-tab

                timerRequest = new System.Threading.Timer(TimerRoutine, null, 0, (int)(float.Parse(Common.ini["Others"]["RefreshRate"]) * 1000));
            };
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e) {

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
            timerRequest = new System.Threading.Timer(TimerRoutine, null, 0, (int)(float.Parse(Common.ini["Others"]["RefreshRate"]) * 1000));
        }
        private void hideStripMenuItem_Click(object sender, EventArgs e) {
            Visible = !Visible;
            hideStripMenuItem.Text = Visible ? "Hide" : "Show";
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
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
