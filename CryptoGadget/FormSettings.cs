
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json.Linq;






namespace CryptoGadget {

    public partial class FormSettings : Form {

        private FormMain _ptrForm;
		private Settings _sett = new Settings();
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
            FormProgressBar form = new FormProgressBar(this, FormProgressBar.FormType.CoinList);
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
                StreamWriter writer = new StreamWriter(Global.JsonLocation);
                writer.Write(data.ToString(Newtonsoft.Json.Formatting.Indented));
                writer.Close();
            };

            if(Global.Json != null)
                return true;

            try {
                if(!File.Exists(Global.JsonLocation)) {
                    Global.Json = DownloadCoinDB();
                    JsonToFile(Global.Json);
                    return true;
                }
                else {
                    Global.Json = JObject.Parse(new StreamReader(File.Open(Global.JsonLocation, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)).ReadToEnd());
                    if(Global.JsonIsValid(Global.Json)) {
                        return true;
                    }
                    else {
                        MessageBox.Show("The coin list file is corrupted, downloading a new copy...", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Global.Json = DownloadCoinDB();
                        JsonToFile(Global.Json);
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

		private void BindSettings() {

			// TODO: Coin binding (if possible)

			numRefreshRate.DataBindings.Add("ValueChanged", _sett.Basic, "RefreshRate");

			// for(int i = 0; i < props.Length; i++) 
			checkVisibilityIcon.DataBindings.Add("Checked", _sett.Visibility, "Icon");
			checkVisibilityCoin.DataBindings.Add("Checked", _sett.Visibility, "Coin");
			checkVisibilityValue.DataBindings.Add("Checked", _sett.Visibility, "Value");
			checkVisibilityChange24.DataBindings.Add("Checked", _sett.Visibility, "Change24");
			checkVisibilityChange24Pct.DataBindings.Add("Checked", _sett.Visibility, "Change24Pct");
			checkVisibilityHeader.DataBindings.Add("Checked", _sett.Visibility, "Header");
			checkVisibilityEdge.DataBindings.Add("Checked", _sett.Visibility, "Edge");
			checkVisibilityRefresh.DataBindings.Add("Checked", _sett.Visibility, "Refresh");

			// for(int i = 0; i < props.Length; i++) 
			buttonColorCoin.DataBindings.Add("BackColorChanged", _sett.Color, "Coin");
			buttonColorValue.DataBindings.Add("BackColorChanged", _sett.Color, "Value");
			buttonColorBackground1.DataBindings.Add("BackColorChanged", _sett.Color, "Background1");
			buttonColorBackground2.DataBindings.Add("BackColorChanged", _sett.Color, "Background2");
			buttonColorPositiveRefresh.DataBindings.Add("BackColorChanged", _sett.Color, "PositiveRefresh");
			buttonColorNegativeRefresh.DataBindings.Add("BackColorChanged", _sett.Color, "NegativeRefresh");
			buttonColorPositiveChange.DataBindings.Add("BackColorChanged", _sett.Color, "PositiveChange");
			buttonColorNegativeChange.DataBindings.Add("BackColorChanged", _sett.Color, "NegativeChange");
			buttonColorHeaderText.DataBindings.Add("BackColorChanged", _sett.Color, "HeaderText");
			buttonColorHeaderBackground.DataBindings.Add("BackColorChanged", _sett.Color, "HeaderBackground");
			buttonColorEdge.DataBindings.Add("BackColorChanged", _sett.Color, "Edge");

			numCoordsPosX.DataBindings.Add("ValueChanged", _sett.Coords, "PosX");
			numCoordsPosX.DataBindings.Add("ValueChanged", _sett.Coords, "PosY");
			checkCoordsExitSave.DataBindings.Add("Checked", _sett.Coords, "ExitSave");
			checkCoordsLockPos.DataBindings.Add("Checked", _sett.Coords, "LockPos");

			// for(int i = 0; i < props.Length; i++)
			numDigitsValue.DataBindings.Add("ValueChanged", _sett.Digits, "Value");
			numDigitsChange24.DataBindings.Add("ValueChanged", _sett.Digits, "Change24");
			numDigitsChange24Pct.DataBindings.Add("ValueChanged", _sett.Digits, "Change24Pct");

			// for(int i = 0; i < props.Length; i++)
			numMetricsIcon.DataBindings.Add("ValueChanged", _sett.Metrics, "Icon");
			numMetricsCoin.DataBindings.Add("ValueChanged", _sett.Metrics, "Coin");
			numMetricsValue.DataBindings.Add("ValueChanged", _sett.Metrics, "Value");
			numMetricsChange24.DataBindings.Add("ValueChanged", _sett.Metrics, "Change24");
			numMetricsChange24Pct.DataBindings.Add("ValueChanged", _sett.Metrics, "Change24Pct");
			numMetricsEdge.DataBindings.Add("ValueChanged", _sett.Metrics, "Edge");
			numMetricsHeader.DataBindings.Add("ValueChanged", _sett.Metrics, "Header");
			numMetricsRows.DataBindings.Add("ValueChanged", _sett.Metrics, "Rows");
			numMetricsIconSize.DataBindings.Add("ValueChanged", _sett.Metrics, "IconSize");
			numMetricsHeaderText.DataBindings.Add("ValueChanged", _sett.Metrics, "HeaderText");
			numMetricsRowsValues.DataBindings.Add("ValueChanged", _sett.Metrics, "RowsValues");

		}



		public FormSettings(FormMain form) {
            InitializeComponent();
            _ptrForm = form;
			Global.Sett.CloneSt(ref _sett);
            HandleCreated += (sender, e) => new Thread(() => BindSettings()).Start();
        }
        

        #region Currencies Tab

        private void buttonAdd_Click(object sender, EventArgs e) {
            if(Global.Json == null) {
                MessageBox.Show("You cannot add a coin to the grid until the coin list is obtained");
                return;
            }
            FormAddCoin form = new FormAddCoin(coinGrid);
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
            if(coinGrid.SelectedRows.Count > 0 && coinGrid.SelectedRows[0].Index < coinGrid.RowCount - 1) {
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
                coinGrid.SelectedRows[0].Cells[0].Value = Global.GetIcon(stream, new Size(16, 16));

                buttonAccept.Click += (b_sender, b_ev) => {
                    stream.Position = 0;
                    StreamWriter writer = new StreamWriter(Global.IconLocation + coin.ToLower() + ".ico");
                    stream.CopyTo(writer.BaseStream);
                    writer.Close();
                    stream.Close();
                };

            };

            ofd.ShowDialog();

        }
        private void buttonCoinSettings_Click(object sender, EventArgs e) {

            if(coinGrid.Rows.Count <= 0 || coinGrid.SelectedRows.Count == -1)
                return;

            FormCoinSettings form = new FormCoinSettings();
            form.ShowDialog();


        }

        private void buttonDownloadList_Click(object sender, EventArgs e) {

            try {
                JObject check = DownloadCoinDB();
                if(!JToken.DeepEquals(check, Global.Json)) {
                    Global.Json = check;
                    StreamWriter writer = new StreamWriter(Global.JsonLocation);
                    writer.Write(Global.Json.ToString(Newtonsoft.Json.Formatting.Indented));
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
        private void buttonCheck_Click(object sender, EventArgs e) {

            if(coinGrid.RowCount <= 0) {
                MessageBox.Show("You need to provide at least one coin to compare", "Error");
                return;
            }

            List<Tuple<string, string>> coinList = new List<Tuple<string, string>>(); // < coin, target >
            foreach(DataGridViewRow row in coinGrid.Rows)
                coinList.Add(new Tuple<string, string>(row.Cells[1].Value.ToString(), row.Cells[3].Value.ToString()));

            FormProgressBar form = new FormProgressBar(this, FormProgressBar.FormType.Check);
            form.ShowDialog();

            MessageBox.Show(form.badConvs.Count == 0 ? "All currencies conversions are correct" : "List of problematics currencies conversions:\n\n" + " - " + string.Join("\n - ", form.badConvs));
        }
        private void buttonDownloadMissingIcons_Click(object sender, EventArgs e) {

            Enabled = false;

            FormProgressBar form = new FormProgressBar(this, FormProgressBar.FormType.Icons);
            form.ShowDialog();

            Enabled = true;

        }

        private void buttonDefaultCurrencies_Click(object sender, EventArgs e) {
			_sett.Default(Settings.DefaultType.Coins); // TODO: Add more if coins are not bindable
        }

        #endregion

        #region Basic Tab

        private void boxTheme_SelectedIndexChanged(object sender, EventArgs e) {
			_sett.Default(boxTheme.SelectedIndex == 0 ? Settings.DefaultType.ColorLight : Settings.DefaultType.ColorDark);
        }

        private void buttonDefaultBasic_Click(object sender, EventArgs e) {
			_sett.Default(Settings.DefaultType.Basic | Settings.DefaultType.ColorLight | Settings.DefaultType.Visibility); 
        }

        #endregion

        #region Advanced Tab

        private void buttonDefaultAdvanced_Click(object sender, EventArgs e) {
			_sett.Default(Settings.DefaultType.Coords | Settings.DefaultType.Digits | Settings.DefaultType.Metrics);
        }

        #endregion

        #region Shared on all Tabs

        private void buttonAccept_Click(object sender, EventArgs e) {
            if(!checkVisibilityIcon.Checked && !checkVisibilityCoin.Checked && !checkVisibilityValue.Checked && !checkVisibilityChange24.Checked) {
                MessageBox.Show("One of the following must be enabled: \"Icon Visibility\", \"Coin Visibility\", \"Value Visibility\", \"Change Visibility\"", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else {
				_sett.CloneSt(ref Global.Sett);
				Global.Sett.Save();
                accept = true;
                Close();
            }
        }
        private void buttonCancel_Click(object sender, EventArgs e) {
            Close();
        }

        private void buttonColorPick(object sender, EventArgs e) => neo.FormUtil.buttonColorPick(sender, e);
        private void textSint(object sender, KeyPressEventArgs e) => neo.FormUtil.textBoxSignedInt(sender, e);
        private void textUint(object sender, KeyPressEventArgs e) => neo.FormUtil.textBoxUnsignedInt(sender, e);
        private void textSfloat(object sender, KeyPressEventArgs e) => neo.FormUtil.textBoxSignedFloat(sender, e);
        private void textUfloat(object sender, KeyPressEventArgs e) => neo.FormUtil.textBoxUnsignedFloat(sender, e);

        #endregion


    }

}
