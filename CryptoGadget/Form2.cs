
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
        public bool accept = false;

        public enum DataType {
            Coins = 0x01,
            Basic = 0x02,
            Colors = 0x04,
            Advanced = 0x08,
            All = Coins | Basic | Colors | Advanced 
        }

        private List<Tuple<string, string>> GenerateFiatCurrencies(params string[] str) {

            List<Tuple<string, string>> convs = new List<Tuple<string, string>>();

            for(int i = 0; i < str.Length; i += 2)
                convs.Add(new Tuple<string, string>(str[i], str[i+1]));

            return convs;
        }

        private JObject DownloadCoinDB() {

            Enabled = false;
            ProgressForm form = new ProgressForm(this, ProgressForm.FormType.CoinList);
            form.ShowDialog();
            Enabled = true;
            if(form.coindb == null)
                throw new System.Net.WebException("Couldn't download the Coin List Database");

            List<Tuple<string, string>> fiat = GenerateFiatCurrencies(
                "AED", "United Arab Emirates Dirham", "AFN", "Afghan Afghani",
                "ARS", "Argentine Peso",              "AUD", "Australian Dollar",
                "AZN", "Azerbaijani Manat",           "BDT", "Bangladeshi Taka",
                "BGN", "Bulgarian Lev",               "BND", "Brunei Dollar",
                "BRL", "Brazilian Real",              "BWP", "Botswana Pula",
                "BYN", "Belarusian Ruble",            "BYR", "Belarusian Ruble",
                "CAD", "Canadian Dollar",             "CHF", "Swiss Franc",
                "CNY", "Chinese Yuan Renminbi",       "COP", "Colombian Peso",
                "CZK", "Czech Koruna",                "DOP", "Dominican Peso",
                "DZD", "Algerian Dinar",              "DKK", "Danish Krone",
                "EGP", "Egyptian Pound",              "ETB", "Ethiopian Birr",
                "EUR", "Euro",                        "GBP", "Great Britain Pound",
                "GEL", "Georgian Iari",               "GGP", "Guernsey Pound",
                "GHS", "Ghanaian Cedi",               "GIP", "ibraltar Pound",
                "GOLD", "Gold Grams",                 "GTQ", "Guatemalan Quetzal",
                "HKD", "Hong Kong Dollar",            "HNL", "Honduran Lempira",
                "HRK", "Croatian Kuna",               "HUF", "Hungarian Forint",
                "IDR", "Indonesian Rupiah",           "ILS", "Isreali New Shekel",
                "INR", "Indian Rupee",                "IQD", "Iraqi Dinar",
                "IRR", "Iranian Rial",                "ISK", "Icelandic Krona",
                "JMD", "Jamaican Dollar",             "JOD", "Jordanian Dinar",
                "JPY", "Japanese Yen",                "KES", "Kenyan Shilling",
                "KGS", "Kyryzstani Som",              "KHR", "Cambodian Riel",
                "KRW", "South Korean Won",            "KWD", "Kuwati Dinar",
                "KZT", "Kazakhstani Tenge",           "LBP", "Lebanese Pound",
                "LKR", "Sri Lankan Rupee",            "LSL", "Lesotho Loti",
                "MDL", "Moldovan Leu",                "MUR", "Mauritian Rupee",   
                "MXN", "Mexican Peso",                "NAD", "Namibian Dollar",
                "NGN", "Nigerian Naira",              "NHD", "Bahraini dinar",
                "MMK", "Burmese Kyat",                "NOK", "Norwegian Krone",
                "NPR", "Napalese Rupee",              "NZD", "New Zealand Dollar",
                "OMR", "Omani Rial",                  "PAB", "Panamanian Balboa",
                "PHP", "Philippine Peso",             "PKR", "Pakistani Rupee",
                "PLN", "Polish zloty",                "PYG", "Paraguayan Guarani",
                "QAR", "Qatari Riyal",                "SAR", "Saudi Riyal",
                "SEK", "Swedish Krona",               "SGD", "Singapore Dollar",
                "RON", "Romanian Leu",                "RSD", "Serbian Dinar",
                "RUR", "Russian Ruble",               "RWF", "Rwandan Franc",
                "THB", "Thai Baht",                   "TND", "unisian Dinar",
                "TRY", "Turkish Lira",                "TTD", "Trinidad and Tobago Dollar",
                "TWD", "Taiwan Dollar",               "UAH", "Ukrainian Hryvnia",
                "UGX", "Ugandan Shilling",            "USD", "United States Dollar",
                "UYU", "Uruguayan Peso",              "VEF", "Venezuelan Bolivar",
                "VND", "Vietnamese Dong",             "XAG", "Troy Ounce of Silver",
                "XOF", "West African CFA Franc",      "ZAR", "South African Rand",
                "ZMW", "Zambian Kwacha"
            );

            foreach(Tuple<string, string> fcoin in fiat) {
                try {
                    (form.coindb["Data"] as JObject).Add(fcoin.Item1, JToken.Parse("{ \"Name\": \"" + fcoin.Item1 + "\", \"CoinName\": \"" + fcoin.Item2 +
                                                                                   "\", \"FullName\": \"" + fcoin.Item2 + " (" + fcoin.Item1 + ")\"" + ", \"FiatCurrency\": \"true\"" + " }"));
                } catch(Exception) { }
            }

            return form.coindb;
        }

        /// <summary>
        /// Reads and checks the CoinList.json or downloads a new one (if corrupted or not available) and triggers a MessageBox if something goes wrong.
        /// </summary>
        /// 
        /// <returns>
        /// Returns a valid JObject if everything was correct or null otherwise
        /// </returns>
        private bool GetCoinDB() {

            Action<JObject> JsonToFile = (data) => {
                StreamWriter writer = new StreamWriter(Common.jsonLocation);
                writer.Write(data.ToString(Newtonsoft.Json.Formatting.Indented));
                writer.Close();
            };
            Func<JObject, bool> JObjIsValid = (jobj) => {
                foreach(JProperty coin in jobj["Data"]) {
                    JToken val = coin.Value;
                    if(val["Name"] == null || val["CoinName"] == null || val["FullName"] == null) {
                        MessageBox.Show(coin.Name);
                        return false;
                    }
                }
                return true;
            };

            if(Common.json != null)
                return true;

            try {
                if(!File.Exists(Common.jsonLocation)) {
                    Common.json = DownloadCoinDB();
                    JsonToFile(Common.json);
                    return true;
                }
                else {
                    Common.json = JObject.Parse(new StreamReader(File.Open(Common.jsonLocation, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)).ReadToEnd());
                    if(JObjIsValid(Common.json)) {
                        return true;
                    }
                    else {
                        MessageBox.Show("The coin list file is corrupted, downloading a new copy...", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Common.json = DownloadCoinDB();
                        JsonToFile(Common.json);
                        return true;
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

            return false;
        }

        private void LoadData(IniData data, DataType dt = DataType.All) {

            if((dt & DataType.Coins) != 0) {

                coinGrid.Rows.Clear();

                foreach(KeyData coin in data["Coins"]) 
                    coinGrid.Rows.Add(Common.GetIcon(coin.KeyName, new Size(16, 16)), coin.KeyName, "", coin.Value, "");

                if(coinGrid.RowCount > 0)
                    coinGrid.Rows[0].Selected = true;

                if(GetCoinDB()) { 
                    foreach(DataGridViewRow row in coinGrid.Rows) {
                        row.Cells[2].Value = Common.json["Data"][row.Cells[1].Value.ToString()]["CoinName"];
                        row.Cells[4].Value = Common.json["Data"][row.Cells[3].Value.ToString()]["CoinName"]; 
                    }
                }

                numericRefreshRate.Minimum = 3.00m + (coinGrid.RowCount <= 10 ? 0 : (coinGrid.RowCount - 10) * 0.25m);
                numericRefreshRate.Value = decimal.Parse(data["Others"]["RefreshRate"]);

            }

            if((dt & DataType.Basic) != 0) {

                checkIconVisible.Checked    = bool.Parse(data["Visibility"]["Icon"]);
                checkCoinVisible.Checked    = bool.Parse(data["Visibility"]["Coin"]);
                checkValueVisible.Checked   = bool.Parse(data["Visibility"]["Value"]);
                checkChangeVisible.Checked  = bool.Parse(data["Visibility"]["Change"]);
                checkHeaderVisible.Checked  = bool.Parse(data["Visibility"]["Header"]);
                checkEdgeVisible.Checked    = bool.Parse(data["Visibility"]["Edge"]);
                checkRefreshVisible.Checked = bool.Parse(data["Visibility"]["Refresh"]);
                checkStartup.Checked        = bool.Parse(data["Others"]["OpenStartup"]);
                checkShowPercentage.Checked = bool.Parse(data["Others"]["ShowPercentage"]);
            }

            if((dt & DataType.Colors) != 0) {

                buttonColorCoins.BackColor            = Common.StrHexToColor(data["Colors"]["Coins"]);
                buttonColorValues.BackColor           = Common.StrHexToColor(data["Colors"]["Values"]);
                buttonColorBackGround1.BackColor      = Common.StrHexToColor(data["Colors"]["BackGround1"]);
                buttonColorBackGround2.BackColor      = Common.StrHexToColor(data["Colors"]["BackGround2"]);
                buttonColorEdge.BackColor             = Common.StrHexToColor(data["Colors"]["Edge"]);
                buttonColorPositiveRefresh.BackColor  = Common.StrHexToColor(data["Colors"]["PositiveRefresh"]);
                buttonColorNegativeRefresh.BackColor  = Common.StrHexToColor(data["Colors"]["NegativeRefresh"]);
                buttonColorPositiveChange.BackColor   = Common.StrHexToColor(data["Colors"]["PositiveChange"]);
                buttonColorNegativeChange.BackColor   = Common.StrHexToColor(data["Colors"]["NegativeChange"]);
                buttonColorHeaderText.BackColor       = Common.StrHexToColor(data["Colors"]["HeaderText"]);
                buttonColorHeaderBackGround.BackColor = Common.StrHexToColor(data["Colors"]["HeaderBackGround"]);

            }

            if((dt & DataType.Advanced) != 0) {

                boxIconWidth.Text    = data["Metrics"]["Icon"];
                boxCoinWidth.Text    = data["Metrics"]["Coin"];
                boxValueWidth.Text   = data["Metrics"]["Value"];
                boxChangeWidth.Text  = data["Metrics"]["Change"];
                boxEdgeWidth.Text    = data["Metrics"]["Edge"];
                boxHeaderHeight.Text = data["Metrics"]["Header"];
                boxRowsHeight.Text   = data["Metrics"]["Rows"];
                boxIconSize.Text     = data["Metrics"]["IconSize"];
                boxTextSize.Text     = data["Metrics"]["Text"];
                boxNumbersSize.Text  = data["Metrics"]["Numbers"];

                boxMaxValueDigits.Text    = data["Others"]["MaxValueDigits"];
                boxMaxValueDecimals.Text  = data["Others"]["MaxValueDecimals"];
                boxMaxChangeDigits.Text   = data["Others"]["MaxChangeDigits"];
                boxMaxChangeDecimals.Text = data["Others"]["MaxChangeDecimals"];

                boxStartX.Text = data["Coordinates"]["StartX"];
                boxStartY.Text = data["Coordinates"]["StartY"];
                checkExitSave.Checked     = bool.Parse(data["Coordinates"]["ExitSave"]);
                checkLockPosition.Checked = bool.Parse(data["Coordinates"]["LockPosition"]);

            }

        }
        private void SaveData(DataType dt = DataType.All) {
            
            Action<string, string, string> AssignIfValue = (sect, key, text) => {
                if(text != "")
                    Common.ini[sect][key] = text;
            };

            if((dt & DataType.Coins) != 0) {

                Common.ini["Coins"].RemoveAllKeys();
                foreach(DataGridViewRow row in coinGrid.Rows)
                    Common.ini["Coins"][row.Cells[1].Value.ToString()] = row.Cells[3].Value.ToString();

            }

            if((dt & DataType.Basic) != 0) {

                Common.ini["Others"]["RefreshRate"] = numericRefreshRate.Value.ToString();
                Common.ini["Others"]["OpenStartup"] = checkStartup.Checked.ToString();
                Common.ini["Others"]["ShowPercentage"] = checkShowPercentage.Checked.ToString();

                Common.ini["Visibility"]["Icon"]    = checkIconVisible.Checked.ToString();
                Common.ini["Visibility"]["Coin"]    = checkCoinVisible.Checked.ToString();
                Common.ini["Visibility"]["Value"]   = checkValueVisible.Checked.ToString();
                Common.ini["Visibility"]["Change"]  = checkChangeVisible.Checked.ToString();
                Common.ini["Visibility"]["Header"]  = checkHeaderVisible.Checked.ToString();
                Common.ini["Visibility"]["Edge"]    = checkEdgeVisible.Checked.ToString();
                Common.ini["Visibility"]["Refresh"] = checkRefreshVisible.Checked.ToString();

            }

            if((dt & DataType.Colors) != 0) {

                Common.ini["Colors"]["Coins"]            = Common.ColorToStrHex(buttonColorCoins.BackColor);
                Common.ini["Colors"]["Values"]           = Common.ColorToStrHex(buttonColorValues.BackColor);
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

        }



        public SettingsForm(MainForm form) {
            InitializeComponent();
            ptrForm = form;
            HandleCreated += (sender, e) => new Thread(() => { 
                Invoke((MethodInvoker)delegate { LoadData(Common.ini); });
            }).Start();
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
            AddCoinForm form = new AddCoinForm(coinGrid);
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

        private void buttonDefaultCurrencies_Click(object sender, EventArgs e) {
            IniData ini = (IniData)Common.ini.Clone();
            ini["Coins"].RemoveAllKeys();
            ini.Merge(Common.DefaultIni(null, Common.DefaultType.Coins));
            LoadData(ini, DataType.Coins);
        }
        private void buttonDefaultBasic_Click(object sender, EventArgs e) {
            IniData ini = (IniData)Common.ini.Clone();
            ini.Merge(Common.DefaultIni(null, Common.DefaultType.Basic));
            LoadData(ini, DataType.Basic);
        }
        private void buttonDefaultAdvanced_Click(object sender, EventArgs e) {
            IniData ini = (IniData)Common.ini.Clone();
            ini.Merge(Common.DefaultIni(null, Common.DefaultType.Advanced));
            LoadData(ini, DataType.Advanced);
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

            if(coinGrid.RowCount <= 0) {
                MessageBox.Show("You need to provide at least one coin to compare", "Error");
                return;
            }

            List<Tuple<string, string>> coinList = new List<Tuple<string, string>>(); // < coin, target >
            foreach(DataGridViewRow row in coinGrid.Rows)
                coinList.Add(new Tuple<string, string>(row.Cells[1].Value.ToString(), row.Cells[3].Value.ToString()));

            ProgressForm form = new ProgressForm(this, ProgressForm.FormType.Check);
            form.ShowDialog();

            MessageBox.Show(form.badConvs.Count == 0 ? "All currencies conversions are correct" : "List of problematics currencies conversions:\n\n" + " - " + string.Join("\n - ", form.badConvs));
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

                Stream stream = (f_sender as OpenFileDialog).OpenFile();
                
                stream.Position = 0;
                coinGrid.SelectedRows[0].Cells[0].Value = Common.GetIcon(stream, new Size(16, 16));
                
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

        /// <summary>
        /// Event method to re-download the coin list database
        /// </summary>
        private void buttonDownloadList_Click(object sender, EventArgs e) {

            try {
                JObject check = DownloadCoinDB();
                if(!JToken.DeepEquals(check, Common.json)) {
                    Common.json = check;
                    StreamWriter writer = new StreamWriter(Common.jsonLocation);
                    writer.Write(Common.json.ToString(Newtonsoft.Json.Formatting.Indented));
                    writer.Close();
                    MessageBox.Show("New coins were added to the coin list database");
                }
                else {
                    MessageBox.Show("There are not new coins to add to the coin list database");
                }
            } catch(System.Net.WebException ex) {
                MessageBox.Show(ex.Message);
            }

        }

        private void buttonColorPick(object sender, EventArgs e)    => neo.FormUtil.buttonColorPick(sender, e);
        private void textSint(object sender, KeyPressEventArgs e)   => neo.FormUtil.textBoxSignedInt(sender, e);
        private void textUint(object sender, KeyPressEventArgs e)   => neo.FormUtil.textBoxUnsignedInt(sender, e);
        private void textSfloat(object sender, KeyPressEventArgs e) => neo.FormUtil.textBoxSignedFloat(sender, e);
        private void textUfloat(object sender, KeyPressEventArgs e) => neo.FormUtil.textBoxUnsignedFloat(sender, e);

        private void buttonDownloadMissingIcons_Click(object sender, EventArgs e) {

            Enabled = false;

            ProgressForm form = new ProgressForm(this, ProgressForm.FormType.Icons);
            form.ShowDialog();

            Enabled = true;

        }
    }

}
