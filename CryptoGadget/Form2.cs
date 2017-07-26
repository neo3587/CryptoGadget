
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json.Linq;
using IniParser.Model;






namespace CryptoGadget {

    public partial class SettingsForm : Form {

        private MainForm ptrForm;
        private static JObject json = null;
        internal bool accept = false;

        public enum DataType {
            Basic = 0x01,
            Advanced = 0x02,
            Colors = 0x04,
            All = Basic | Advanced | Colors
        }

        /// <summary>
        /// Reads and checks the CoinList.json or downloads a new one (if corrupted or not available) and triggers a MessageBox if something goes wrong.
        /// </summary>
        /// 
        /// <returns>
        /// Returns a valid JObject if everything was correct or null otherwise
        /// </returns>
        private JObject GetCoinDB() {

            Func<JObject> DownloadDB = () => {
                Enabled = false;
                ProgressForm form = new ProgressForm();
                form.ShowDialog();
                Enabled = true;
                if(form.coindb == null)
                    throw new System.Net.WebException("Couldn't download the Coin List Database");
                return form.coindb;
            };

            if(json != null)
                return json;

            try {
                if(!File.Exists(Common.jsonLocation)) {
                    json = DownloadDB();
                }
                else {
                    json = JObject.Parse(new StreamReader(File.Open(Common.jsonLocation, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)).ReadToEnd());
                    try {
                        foreach(JToken coin in json["rows"]) {
                            if(coin["code"] == null || coin["name"] == null)
                                throw new Exception();
                        }
                    } catch(Exception) {
                        MessageBox.Show("The coin list file is corrupted, downloading a new copy...", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        json = DownloadDB();
                    }
                }

            } catch(System.Net.WebException e) {
                MessageBox.Show("There was an error when trying to download the coin list file:\n\n" + e.Message + "\n\nEverything will work in a normal way, but you can't add new coins or swap the target coin",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            } catch(FileNotFoundException e) {
                MessageBox.Show("There was an error when trying to read the coin list file:\n\n" + e.Message + "\n\nEverything will work in a normal way, but you can't add new coins or swap the target coin",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            } catch(IOException e) {
                MessageBox.Show("A program is blocking the access to the coin list file:\n\n" + e.Message + "\n\nEverything will work in a normal way, but you can't add new coins or swap the target coin",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            } catch(Exception e) {
                MessageBox.Show("There was an unexpected error with the coin list file:\n\n" + e.Message + "\n\nEverything will work in a normal way, but you can't add new coins or swap the target coin",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return json;
        }

        private void LoadData(IniData data, DataType dt = DataType.All) {

            Invoke((MethodInvoker)delegate {

                if((dt & DataType.Basic) != 0) {

                    boxTargetCoin.Items.Clear();
                    coinGrid.Rows.Clear();

                    checkIconVisible.Checked = bool.Parse(data["Visibility"]["Icon"]);
                    checkCoinVisible.Checked = bool.Parse(data["Visibility"]["Coin"]);
                    checkValueVisible.Checked = bool.Parse(data["Visibility"]["Value"]);
                    checkChangeVisible.Checked = bool.Parse(data["Visibility"]["Change"]);
                    checkHeaderVisible.Checked = bool.Parse(data["Visibility"]["Header"]);
                    checkEdgeVisible.Checked = bool.Parse(data["Visibility"]["Edge"]);
                    checkRefreshVisible.Checked = bool.Parse(data["Visibility"]["Refresh"]);
                    checkStartup.Checked = bool.Parse(data["Others"]["OpenStartup"]);

                    numericRefreshRate.Value = decimal.Parse(data["Others"]["RefreshRate"]);

                    foreach(KeyData coin in data["Coins"]) {
                        try {
                            coinGrid.Rows.Add(new Icon(Common.iconLocation + coin.Value + ".ico", new Size(16, 16)).ToBitmap(), coin.Value, coin.KeyName);
                        } catch(Exception) {
                            coinGrid.Rows.Add(new Bitmap(1, 1), coin.Value, coin.KeyName);
                        }
                    }

                    if(coinGrid.RowCount > 0)
                        coinGrid.Rows[0].Selected = true;

                    JObject json = GetCoinDB();

                    if(json != null)
                        foreach(JToken coin in json["rows"])
                            boxTargetCoin.Items.Add(coin["code"] + " (" + coin["name"] + ")");

                    boxTargetCoin.SelectedIndex = boxTargetCoin.FindString(data["Others"]["TargetCoin"]);
                    if(boxTargetCoin.SelectedIndex == -1) {
                        boxTargetCoin.Items.Add(data["Others"]["TargetCoin"]);
                        boxTargetCoin.SelectedIndex = 0;
                    }

                }

                if((dt & DataType.Advanced) != 0) {

                    boxIconWidth.Text = data["Metrics"]["Icon"];
                    boxCoinWidth.Text = data["Metrics"]["Coin"];
                    boxValueWidth.Text = data["Metrics"]["Value"];
                    boxChangeWidth.Text = data["Metrics"]["Change"];
                    boxEdgeWidth.Text = data["Metrics"]["Edge"];
                    boxHeaderHeight.Text = data["Metrics"]["Header"];
                    boxRowsHeight.Text = data["Metrics"]["Rows"];
                    boxIconSize.Text = data["Metrics"]["IconSize"];
                    boxTextSize.Text = data["Metrics"]["Text"];
                    boxNumbersSize.Text = data["Metrics"]["Numbers"];

                    boxMaxValueDigits.Text = data["Others"]["MaxValueDigits"];
                    boxMaxValueDecimals.Text = data["Others"]["MaxValueDecimals"];
                    boxMaxChangeDigits.Text = data["Others"]["MaxChangeDigits"];
                    boxMaxChangeDecimals.Text = data["Others"]["MaxChangeDecimals"];

                    boxStartX.Text = data["Coordinates"]["StartX"];
                    boxStartY.Text = data["Coordinates"]["StartY"];
                    checkExitSave.Checked = bool.Parse(data["Coordinates"]["ExitSave"]);
                    checkLockPosition.Checked = bool.Parse(data["Coordinates"]["LockPosition"]);

                }

                if((dt & DataType.Colors) != 0) {

                    buttonColorText.BackColor = Common.StrHexToColor(data["Colors"]["Text"]);
                    buttonColorBackGround1.BackColor = Common.StrHexToColor(data["Colors"]["BackGround1"]);
                    buttonColorBackGround2.BackColor = Common.StrHexToColor(data["Colors"]["BackGround2"]);
                    buttonColorEdge.BackColor = Common.StrHexToColor(data["Colors"]["Edge"]);
                    buttonColorPositiveRefresh.BackColor = Common.StrHexToColor(data["Colors"]["PositiveRefresh"]);
                    buttonColorNegativeRefresh.BackColor = Common.StrHexToColor(data["Colors"]["NegativeRefresh"]);
                    buttonColorPositiveChange.BackColor = Common.StrHexToColor(data["Colors"]["PositiveChange"]);
                    buttonColorNegativeChange.BackColor = Common.StrHexToColor(data["Colors"]["NegativeChange"]);
                    buttonColorHeaderText.BackColor = Common.StrHexToColor(data["Colors"]["HeaderText"]);
                    buttonColorHeaderBackGround.BackColor = Common.StrHexToColor(data["Colors"]["HeaderBackGround"]);

                }

            });
        }
        private void SaveData(DataType dt = DataType.All) {
            
            Action<string, string, string> AssignIfValue = (sect, key, text) => {
                if(text != "")
                    Common.ini[sect][key] = text;
            };

            if((dt & DataType.Basic) != 0) {
                Common.ini["Others"]["RefreshRate"] = numericRefreshRate.Value.ToString();
                Common.ini["Others"]["OpenStartup"] = checkStartup.Checked.ToString();

                Common.ini["Visibility"]["Icon"]    = checkIconVisible.Checked.ToString();
                Common.ini["Visibility"]["Coin"]    = checkCoinVisible.Checked.ToString();
                Common.ini["Visibility"]["Value"]   = checkValueVisible.Checked.ToString();
                Common.ini["Visibility"]["Change"]  = checkChangeVisible.Checked.ToString();
                Common.ini["Visibility"]["Header"]  = checkHeaderVisible.Checked.ToString();
                Common.ini["Visibility"]["Edge"]    = checkEdgeVisible.Checked.ToString();
                Common.ini["Visibility"]["Refresh"] = checkRefreshVisible.Checked.ToString();

                Common.ini["Coins"].RemoveAllKeys();
                foreach(DataGridViewRow row in coinGrid.Rows)
                    Common.ini["Coins"][row.Cells[2].Value.ToString()] = row.Cells[1].Value.ToString();

                string strCoin = (string)boxTargetCoin.SelectedItem;
                Common.ini["Others"]["TargetCoin"] = strCoin.Substring(0, strCoin.LastIndexOf('(')).Trim(' ');
            }

            if((dt & DataType.Advanced) != 0) {
                AssignIfValue("Metrics", "Icon",     boxIconWidth.Text);
                AssignIfValue("Metrics", "Coin",     boxCoinWidth.Text);
                AssignIfValue("Metrics", "Value",    boxValueWidth.Text);
                AssignIfValue("Metrics", "Change",   boxChangeWidth.Text);
                AssignIfValue("Metrics", "Edge",     boxEdgeWidth.Text);
                AssignIfValue("Metrics", "Header",   boxHeaderHeight.Text);
                AssignIfValue("Metrics", "Rows",     boxRowsHeight.Text);
                AssignIfValue("Metrics", "IconSize", boxIconSize.Text);
                AssignIfValue("Metrics", "Text",     boxTextSize.Text);
                AssignIfValue("Metrics", "Numbers",  boxNumbersSize.Text);

                AssignIfValue("Others", "MaxValueDigits",    boxMaxValueDigits.Text);
                AssignIfValue("Others", "MaxValueDecimals",  boxMaxValueDecimals.Text);
                AssignIfValue("Others", "MaxChangeDigits",   boxMaxChangeDigits.Text);
                AssignIfValue("Others", "MaxChangeDecimals", boxMaxChangeDecimals.Text);

                AssignIfValue("Coordinates", "StartX", boxStartX.Text);
                AssignIfValue("Coordinates", "StartY", boxStartY.Text);
                Common.ini["Coordinates"]["ExitSave"]     = checkExitSave.Checked.ToString();
                Common.ini["Coordinates"]["LockPosition"] = checkLockPosition.Checked.ToString();
            }

            if((dt & DataType.Colors) != 0) {
                Common.ini["Colors"]["Text"]             = Common.ColorToStrHex(buttonColorText.BackColor);
                Common.ini["Colors"]["BackGround1"]      = Common.ColorToStrHex(buttonColorBackGround1.BackColor);
                Common.ini["Colors"]["BackGround2"]      = Common.ColorToStrHex(buttonColorBackGround2.BackColor);
                Common.ini["Colors"]["Edge"]             = Common.ColorToStrHex(buttonColorEdge.BackColor);
                Common.ini["Colors"]["PositiveRefresh"]  = Common.ColorToStrHex(buttonColorPositiveRefresh.BackColor);
                Common.ini["Colors"]["NegativeRefresh"]  = Common.ColorToStrHex(buttonColorNegativeRefresh.BackColor);
                Common.ini["Colors"]["PositiveChange"]   = Common.ColorToStrHex(buttonColorPositiveChange.BackColor);
                Common.ini["Colors"]["NegativeChange"]   = Common.ColorToStrHex(buttonColorNegativeChange.BackColor);
                Common.ini["Colors"]["HeaderText"]       = Common.ColorToStrHex(buttonColorHeaderText.BackColor);
                Common.ini["Colors"]["HeaderBackGround"] = Common.ColorToStrHex(buttonColorHeaderBackGround.BackColor);
            }

        }



        public SettingsForm(MainForm form) {
            InitializeComponent();
            ptrForm = form;
            new Thread(() => LoadData(Common.ini)).Start();
        }


        /// <summary>
        /// Event method to change the backcolor of a button on press
        /// </summary>
        private void buttonSharedColorPick(object sender, EventArgs e) {
            ColorDialog cd = new ColorDialog();
            cd.Color = (sender as Button).BackColor;
            cd.FullOpen = true;
            cd.ShowDialog();
            (sender as Button).BackColor = cd.Color;
        }

        private void buttonAccept_Click(object sender, EventArgs e) {
            if(!checkIconVisible.Checked && !checkCoinVisible.Checked && !checkValueVisible.Checked && !checkChangeVisible.Checked) {
                MessageBox.Show("One of the following must be enabled: \"Icon Visibility\", \"Coin Visibility\", \"Value Visibility\", \"Change Visibility\"", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else {
                SaveData();
                accept = true;
                Close();
            }
        }
        private void buttonCancel_Click(object sender, EventArgs e) {
            Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e) {
            AddCoinForm form = new AddCoinForm(boxTargetCoin, coinGrid);
            form.ShowDialog();
            
        }
        private void buttonSub_Click(object sender, EventArgs e) {
            if(coinGrid.SelectedRows.Count > 0) {
                 coinGrid.Rows.Remove(coinGrid.SelectedRows[0]);
            }
        }
        private void buttonUp_Click(object sender, EventArgs e) {
            if(coinGrid.SelectedRows.Count > 0 && coinGrid.SelectedRows[0].Index > 0) {
                int index1 = coinGrid.SelectedRows[0].Index;
                int index2 = coinGrid.SelectedRows[0].Index - 1;
                DataGridViewRow tmp1 = coinGrid.Rows[index1];
                DataGridViewRow tmp2 = coinGrid.Rows[index2];
                coinGrid.Rows.Remove(tmp1);
                coinGrid.Rows.Remove(tmp2);
                coinGrid.Rows.Insert(index2, tmp2);
                coinGrid.Rows.Insert(index2, tmp1);
                coinGrid.Rows[index2].Selected = true;
            }
        }
        private void buttonDown_Click(object sender, EventArgs e) {
            if(coinGrid.SelectedRows.Count > 0  && coinGrid.SelectedRows[0].Index < coinGrid.RowCount-1) {
                int index1 = coinGrid.SelectedRows[0].Index;
                int index2 = coinGrid.SelectedRows[0].Index + 1;
                DataGridViewRow tmp1 = coinGrid.Rows[index1];
                DataGridViewRow tmp2 = coinGrid.Rows[index2];
                coinGrid.Rows.Remove(tmp1);
                coinGrid.Rows.Remove(tmp2);
                coinGrid.Rows.Insert(index1, tmp1);
                coinGrid.Rows.Insert(index1, tmp2);
                coinGrid.Rows[index2].Selected = true;
            }
        }

        private void boxTargetCoin_Click(object sender, EventArgs e) {
            boxTargetCoin.DroppedDown = true;
        }

        private void buttonDefaultBasic_Click(object sender, EventArgs e) {
            IniData ini = (IniData)Common.ini.Clone();
            ini["Coins"].RemoveAllKeys();
            ini.Merge(Common.DefaultIni(null, Common.DefaultType.Basic));
            LoadData(ini, DataType.Basic);
        }
        private void buttonDefaultAdvanced_Click(object sender, EventArgs e) {
            IniData ini = (IniData)Common.ini.Clone();
            ini.Merge(Common.DefaultIni(null, Common.DefaultType.Advanced));
            LoadData(ini, DataType.Advanced);
        }
        private void buttonDefaultColors_Click(object sender, EventArgs e) {
            IniData ini = (IniData)Common.ini.Clone();
            ini.Merge(Common.DefaultIni(null, Common.DefaultType.ColorsLight));
            LoadData(ini, DataType.Colors);
        }

        private void boxTheme_SelectedIndexChanged(object sender, EventArgs e) {
            IniData ini = (IniData)Common.ini.Clone();
            ini.Merge(Common.DefaultIni(null, boxTheme.SelectedIndex == 0 ? Common.DefaultType.ColorsLight : Common.DefaultType.ColorsDark));
            LoadData(ini, DataType.Colors);
        }

        /// <summary>
        /// Event method that checks if the API doesn't reject any coin conversion
        /// </summary>
        private void buttonCheck_Click(object sender, EventArgs e) {

            if(boxTargetCoin.Items.Count <= 0 || boxTargetCoin.SelectedIndex == -1) {
                MessageBox.Show("You need to provide a target coin", "Error");
                return;
            }
            if(coinGrid.RowCount <= 0) {
                MessageBox.Show("You need to provide at least one coin to compare", "Error");
                return;
            }

            List<Tuple<string, string>> coinList = new List<Tuple<string, string>>(); // < coin, name >
            foreach(DataGridViewRow row in coinGrid.Rows)
                coinList.Add(new Tuple<string, string>(row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString()));

            ProgressForm form = new ProgressForm(coinList, (string)boxTargetCoin.SelectedItem);
            form.ShowDialog();

            string msg = "";
            foreach(string bad in form.badCoins)
                msg += " - " + bad + "\n";

            MessageBox.Show(msg == "" ? "All coins are correct" : "List of problematic coins with the current target coin:\n" + msg);
        }

        /// <summary>
        /// Event method to add or swap the icon of the selected coin
        /// </summary>
        private void buttonAddIcon_Click(object sender, EventArgs e) {

            if(coinGrid.Rows.Count <= 0 || coinGrid.SelectedRows.Count == -1)
                return;

            OpenFileDialog ofd = new OpenFileDialog();

            string coin = coinGrid.SelectedRows[0].Cells[1].Value.ToString();

            ofd.Title = "Select Icon for " + coin + " (" + coinGrid.SelectedRows[0].Cells[2].Value + ")";
            ofd.Filter = "Icon Files (.ico)|*.ico";
            ofd.Multiselect = false;

            ofd.FileOk += (f_sender, f_ev) => {

                MemoryStream stream = new MemoryStream();
                (f_sender as OpenFileDialog).OpenFile().CopyTo(stream);

                stream.Position = 0;
                coinGrid.SelectedRows[0].Cells[0].Value = new Icon(stream, new Size(16, 16));

                buttonAccept.Click += (b_sender, b_ev) => {
                    stream.Position = 0;
                    StreamWriter writer = new StreamWriter(Common.iconLocation + coin.ToLower() + ".ico");
                    stream.CopyTo(writer.BaseStream);
                    writer.Close();
                    stream.Close();
                };

            };

            ofd.ShowDialog();

        }


    }

}
