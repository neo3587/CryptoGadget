
using System;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Diagnostics;

using Newtonsoft.Json.Linq;



namespace CryptoGadget {

    public partial class FormSettings : Form {

        private FormMain _ptr_form = null;
		private Settings _sett = new Settings();
		private int _page = 0;
		private Settings.StCoin _last_conv = new Settings.StCoin();


		private void ApplySettings() {

			_sett.CloneTo(Global.Sett);
			Global.Sett.Store();
			Global.Sett.Save();

			_ptr_form.ApplySettings();
			Global.Profile = textBoxProfileName.Text;

			Global.Charts.mtx.WaitOne();
			System.Threading.Tasks.Parallel.ForEach(Global.Charts.dict.Values, (item, state) => {
				item.form.Invoke((MethodInvoker)delegate { item.form.ApplySettings(); });
			});
			Global.Charts.mtx.ReleaseMutex();
		}
		
        private JObject DownloadCoinDB() {

			Enabled = false;
            FormProgressBar form = new FormProgressBar(this, FormProgressBar.FormType.CoinList);
            form.ShowDialog();
            Enabled = true;
            if(form.CoinDataBase == null)
                throw new System.Net.WebException("Couldn't download the Coin List Database");

            ValueTuple<string, string>[] fiat = {
				("AED", "United Arab Emirates Dirham"), ("AFN", "Afghan Afghani"),
                ("ARS", "Argentine Peso"),             ("AUD", "Australian Dollar"),
                ("AZN", "Azerbaijani Manat"),          ("BDT", "Bangladeshi Taka"),
                ("BGN", "Bulgarian Lev"),              ("BND", "Brunei Dollar"),
                ("BRL", "Brazilian Real"),             ("BWP", "Botswana Pula"),
                ("BYN", "Belarusian Ruble"),           ("BYR", "Belarusian Ruble"),
                ("CAD", "Canadian Dollar"),            ("CHF", "Swiss Franc"),
                ("CNY", "Chinese Yuan Renminbi"),      ("COP", "Colombian Peso"),
                ("CZK", "Czech Koruna"),               ("DOP", "Dominican Peso"),
                ("DZD", "Algerian Dinar"),             ("DKK", "Danish Krone"),
                ("EGP", "Egyptian Pound"),             ("ETB", "Ethiopian Birr"),
                ("EUR", "Euro"),                       ("GBP", "Great Britain Pound"),
                ("GEL", "Georgian Iari"),              ("GGP", "Guernsey Pound"),
				("GHS", "Ghanaian Cedi"),              ("GIP", "ibraltar Pound"),
				("GOLD", "Gold Grams"),                ("GTQ", "Guatemalan Quetzal"),
				("HKD", "Hong Kong Dollar"),           ("HNL", "Honduran Lempira"),
				("HRK", "Croatian Kuna"),              ("HUF", "Hungarian Forint"),
				("IDR", "Indonesian Rupiah"),          ("ILS", "Isreali New Shekel"),
				("INR", "Indian Rupee"),               ("IQD", "Iraqi Dinar"),
				("IRR", "Iranian Rial"),               ("ISK", "Icelandic Krona"),
				("JMD", "Jamaican Dollar"),            ("JOD", "Jordanian Dinar"),
				("JPY", "Japanese Yen"),               ("KES", "Kenyan Shilling"),
				("KGS", "Kyryzstani Som"),             ("KHR", "Cambodian Riel"),
				("KRW", "South Korean Won"),           ("KWD", "Kuwati Dinar"),
				("KZT", "Kazakhstani Tenge"),          ("LBP", "Lebanese Pound"),
				("LKR", "Sri Lankan Rupee"),           ("LSL", "Lesotho Loti"),
				("MDL", "Moldovan Leu"),               ("MUR", "Mauritian Rupee"),
				("MXN", "Mexican Peso"),               ("NAD", "Namibian Dollar"),
				("NGN", "Nigerian Naira"),             ("NHD", "Bahraini dinar"),
				("MMK", "Burmese Kyat"),               ("NOK", "Norwegian Krone"),
				("NPR", "Napalese Rupee"),             ("NZD", "New Zealand Dollar"),
				("OMR", "Omani Rial"),                 ("PAB", "Panamanian Balboa"),
				("PHP", "Philippine Peso"),            ("PKR", "Pakistani Rupee"),
				("PLN", "Polish zloty"),               ("PYG", "Paraguayan Guarani"),
				("QAR", "Qatari Riyal"),               ("SAR", "Saudi Riyal"),
				("SEK", "Swedish Krona"),              ("SGD", "Singapore Dollar"),
				("RON", "Romanian Leu"),               ("RSD", "Serbian Dinar"),
				("RUR", "Russian Ruble"),              ("RWF", "Rwandan Franc"),
				("THB", "Thai Baht"),                  ("TND", "unisian Dinar"),
				("TRY", "Turkish Lira"),               ("TTD", "Trinidad and Tobago Dollar"),
				("TWD", "Taiwan Dollar"),              ("UAH", "Ukrainian Hryvnia"),
				("UGX", "Ugandan Shilling"),           ("USD", "United States Dollar"),
				("UYU", "Uruguayan Peso"),             ("VEF", "Venezuelan Bolivar"),
				("VND", "Vietnamese Dong"),            ("XAG", "Troy Ounce of Silver"),
				("XOF", "West African CFA Franc"),     ("ZAR", "South African Rand"),
				("ZMW", "Zambian Kwacha")
			};

            for(int i = 0; i < fiat.Length; i++) {
                try {
                    (form.CoinDataBase["Data"] as JObject).Add(fiat[i].Item1, JToken.Parse("{ \"Name\": \"" + fiat[i].Item1 + "\", \"CoinName\": \"" + fiat[i].Item2 +
																						   "\", \"FullName\": \"" + fiat[i].Item2 + " (" + fiat[i].Item1 + ")\"" + ", \"FiatCurrency\": \"true\"" + " }"));
                } catch { }
            }

            return form.CoinDataBase;
        }
        private bool GetCoinDB() {

            Action<JObject> JsonToFile = (data) => {
				using(StreamWriter writer = new StreamWriter(Global.LocationCoinList)) {
					writer.Write(data.ToString(Newtonsoft.Json.Formatting.Indented));
				}
            };

            if(Global.Json != null)
                return true;

            try {
                if(!File.Exists(Global.LocationCoinList)) {
                    Global.Json = DownloadCoinDB();
                    JsonToFile(Global.Json);
                    return true;
                }
                else {
                    Global.Json = JObject.Parse(new StreamReader(File.Open(Global.LocationCoinList, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)).ReadToEnd());
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

		private void BindCoins() {
			
			foreach(Settings.StCoin st in _sett.Coins[_page]) {
				st.Icon = Global.GetIcon(st.Coin, 16);
				st.AlertType = (st.Alert.Above > 0.0m && st.Alert.Below > 0.0m) ? '↕' :
							   (st.Alert.Above > 0.0m) ? '↑' : 
							   (st.Alert.Below > 0.0m) ? '↓' : '-';
			}
			if(Global.Json == null && GetCoinDB()) {
				foreach(Settings.StCoin st in _sett.Coins[_page]) {
					st.CoinName = Global.Json["Data"][st.Coin]["CoinName"].ToString();
					st.TargetName = Global.Json["Data"][st.Target]["CoinName"].ToString();
				}
			}

			PropertyInfo[] coin_props = Settings.StCoin.GetProps();
			for(int i = 0; i < coin_props.Length; i++)
				coinGrid.Columns[i].DataPropertyName = coin_props[i].Name;

			coinGrid.DataSource = new BindingSource() { DataSource = _sett.Coins[_page] };
		}
		private void BindSettings() {

			// Coins

			BindCoins();

			// Basic

			numRefreshRate.DataBindings.Add("Value", _sett.Basic, "RefreshRate");
			numAlertCheckRate.DataBindings.Add("Value", _sett.Basic, "AlertCheckRate");
			checkStartup.DataBindings.Add("Checked", _sett.Basic, "Startup");
			checkNotifyNewUpdate.DataBindings.Add("Checked", _sett.Basic, "NotifyNewVersion");

			// Visibility

			checkEnableHeader.DataBindings.Add("Checked", _sett.Visibility, "Header");
			checkEnableEdge.DataBindings.Add("Checked", _sett.Visibility, "Edge");
			checkEnableRefresh.DataBindings.Add("Checked", _sett.Visibility, "Refresh");

			// Color

			buttonColorRowsText.DataBindings.Add("BackColor", _sett.Color, "RowsText");
			buttonColorRowsValues.DataBindings.Add("BackColor", _sett.Color, "RowsValues");
			buttonColorBackground1.DataBindings.Add("BackColor", _sett.Color, "Background1");
			buttonColorBackground2.DataBindings.Add("BackColor", _sett.Color, "Background2");
			buttonColorPositiveRefresh.DataBindings.Add("BackColor", _sett.Color, "PositiveRefresh");
			buttonColorNegativeRefresh.DataBindings.Add("BackColor", _sett.Color, "NegativeRefresh");
			buttonColorPositiveChange.DataBindings.Add("BackColor", _sett.Color, "PositiveChange");
			buttonColorNegativeChange.DataBindings.Add("BackColor", _sett.Color, "NegativeChange");
			buttonColorHeaderText.DataBindings.Add("BackColor", _sett.Color, "HeaderText");
			buttonColorHeaderBackground.DataBindings.Add("BackColor", _sett.Color, "HeaderBackground");
			buttonColorEdge.DataBindings.Add("BackColor", _sett.Color, "Edge");

			// Coords

			numCoordsPosX.DataBindings.Add("Value", _sett.Coords, "PosX");
			numCoordsPosY.DataBindings.Add("Value", _sett.Coords, "PosY");
			checkCoordsExitSave.DataBindings.Add("Checked", _sett.Coords, "ExitSave");
			checkCoordsLockPos.DataBindings.Add("Checked", _sett.Coords, "LockPos");

			// Metrics

			numMetricsEdge.DataBindings.Add("Value", _sett.Metrics, "Edge");
			numMetricsHeader.DataBindings.Add("Value", _sett.Metrics, "Header");
			numMetricsRows.DataBindings.Add("Value", _sett.Metrics, "Rows");
			numMetricsIconSize.DataBindings.Add("Value", _sett.Metrics, "IconSize");
			numMetricsHeaderText.DataBindings.Add("Value", _sett.Metrics, "HeaderText");
			numMetricsRowsValues.DataBindings.Add("Value", _sett.Metrics, "RowsValues");

			// Pages

			numPagesDefault.DataBindings.Add("Value", _sett.Pages, "Default");
			checkPagesExitSave.DataBindings.Add("Checked", _sett.Pages, "ExitSave");
			checkPagesAutoRotate.DataBindings.Add("Checked", _sett.Pages, "AutoRotate");
			numPagesRotateRate.DataBindings.Add("Value", _sett.Pages, "RotateRate");
			numPagesMaxRotatePage.DataBindings.Add("Value", _sett.Pages, "MaxPageRotate");

			// Market

			textBoxSelectMarket.DataBindings.Add("Text", _sett.Market, "Market");

			// Grid

			PropertyInfo[] cols_props = Settings.StColumn.GetProps();
			for(int i = 0; i < cols_props.Length; i++)
				colsGrid.Columns[i].DataPropertyName = cols_props[i].Name;

			colsGrid.DataSource = _sett.Grid.Columns;

			// Charts

			buttonChartForeColor.DataBindings.Add("BackColor", _sett.Chart, "ForeColor");
			buttonChartBackColor.DataBindings.Add("BackColor", _sett.Chart, "BackColor");
			buttonChartGridColor.DataBindings.Add("BackColor", _sett.Chart, "GridColor");
			buttonChartCursorLinesColor.DataBindings.Add("BackColor", _sett.Chart, "CursorLinesColor");
			buttonChartCandleUpColor.DataBindings.Add("BackColor", _sett.Chart, "CandleUpColor");
			buttonChartCandleDownColor.DataBindings.Add("BackColor", _sett.Chart, "CandleDownColor");
			comboChartStep.DataBindings.Add("SelectedIndex", _sett.Chart, "DefaultStep");

			// Other (not actually binds)

			textBoxProfileName.Text = Global.Profile;
			comboTheme.Text = "";
			labelNewVersion.Text = "New Version Available (" + Global.LastVersion + ")";
			labelNewVersion.Visible = Global.Version != Global.LastVersion;

		}
		private void ClearBindings() {
			Action<Control.ControlCollection> BindChildsClear = null;
			BindChildsClear = (parent) => {
				foreach(Control child in parent) {
					if(child.HasChildren)
						BindChildsClear(child.Controls);
					child.DataBindings.Clear();
				}
			};
			BindChildsClear(Controls);
		}


		public FormSettings(FormMain form) {

			InitializeComponent();

			labelCryptoGadgetVersion.Text = typeof(FormMain).Assembly.GetName().Name + " " + Global.Version;

			_ptr_form = form;

			Global.Sett.CloneTo(_sett);

			coinGrid.DoubleBuffered(true);
			colsGrid.DoubleBuffered(true);

			comboPages.SelectedIndex = 0;

			comboPages.Click += Global.DropDownOnClick;
			comboPages.KeyPress += Global.DropDownOnKeyPress;
			comboChartStep.Click += Global.DropDownOnClick;
			comboChartStep.KeyPress += Global.DropDownOnKeyPress;

			HandleCreated += (sender, e) => BindSettings();
		}
        

        #region Currencies Tab

        private void buttonConvAdd_Click(object sender, EventArgs e) {

			if(Global.Json == null) {
                MessageBox.Show("You cannot add a coin to the grid until the coin list is obtained");
                return;
            }

			FormPairSettings form = new FormPairSettings(_sett.Coins[_page], _last_conv);
			form.ShowDialog();

			if(form.CoinResult != null) {
				_last_conv = form.CoinResult;
				int insertPos = coinGrid.SelectedRows.Count > 0 ? coinGrid.SelectedRows[0].Index +1 : 0;
				_sett.Coins[_page].Insert(insertPos, form.CoinResult);
				coinGrid.Rows[insertPos].Selected = true;
			}
        }
        private void buttonConvSub_Click(object sender, EventArgs e) {
			if(coinGrid.SelectedRows.Count > 0) {
				_sett.Coins[_page].RemoveAt(coinGrid.SelectedRows[0].Index);
				if(coinGrid.SelectedRows.Count <= 0 && coinGrid.RowCount > 0)
					coinGrid.Rows[coinGrid.RowCount - 1].Selected = true;
			}
        }
		private void buttonConvSettings_Click(object sender, EventArgs e) {
			if(coinGrid.SelectedRows.Count <= 0) {
				return;
			}
			if(Global.Json == null) {
				MessageBox.Show("You cannot modify a coin to the grid until the coin list is obtained, there's a good reason for that, trust me :)");
				return;
			}
			FormPairSettings form = new FormPairSettings(_sett.Coins[_page], _sett.Coins[_page][coinGrid.SelectedRows[0].Index], true);
			form.ShowDialog();
			if(form.CoinResult != null) {
				_sett.Coins[_page][coinGrid.SelectedRows[0].Index] = form.CoinResult;
			}
		}
		private void buttonConvUp_Click(object sender, EventArgs e) {
            if(coinGrid.SelectedRows.Count > 0 && coinGrid.SelectedRows[0].Index > 0) {
                int index1 = coinGrid.SelectedRows[0].Index;
                int index2 = coinGrid.SelectedRows[0].Index - 1;
				Settings.StCoin ptr = _sett.Coins[_page][index1];
				_sett.Coins[_page].RemoveAt(index1);
				_sett.Coins[_page].Insert(index2, ptr);
                coinGrid.Rows[index2].Selected = true;
            }
        }
        private void buttonConvDown_Click(object sender, EventArgs e) {
            if(coinGrid.SelectedRows.Count > 0 && coinGrid.SelectedRows[0].Index < coinGrid.RowCount - 1) {
                int index1 = coinGrid.SelectedRows[0].Index;
                int index2 = coinGrid.SelectedRows[0].Index + 1;
				Settings.StCoin ptr = _sett.Coins[_page][index1];
				_sett.Coins[_page].RemoveAt(index1);
				_sett.Coins[_page].Insert(index2, ptr);
				coinGrid.Rows[index2].Selected = true;
            }
        }

        private void buttonUpdateCoinList_Click(object sender, EventArgs e) {

            try {
                JObject check = DownloadCoinDB();
                if(!JToken.DeepEquals(check, Global.Json)) {
                    Global.Json = check;
					using(StreamWriter writer = new StreamWriter(Global.LocationCoinList)) {
						writer.Write(Global.Json.ToString(Newtonsoft.Json.Formatting.Indented));
					}
                    MessageBox.Show("New coins were added to the coin list database");
                }
                else {
                    MessageBox.Show("There are not new coins to add on the coin list database");
                }
            } catch(System.Net.WebException ex) {
                MessageBox.Show(ex.Message);
            }

        }
        private void buttonCheck_Click(object sender, EventArgs e) {

            if(coinGrid.RowCount <= 0) {
                MessageBox.Show("You need to provide at least one conversion to compare", "Error");
                return;
            }

            FormProgressBar form = new FormProgressBar((_sett.Coins[_page], _sett.Market.Market), FormProgressBar.FormType.Check);
            form.ShowDialog();

			if(form.BadConvs != null)
				MessageBox.Show(form.BadConvs.Count == 0 ? "All currencies conversions of the current page are correct" : "List of problematics currencies conversions:\n\n" + " - " + string.Join("\n - ", form.BadConvs));
			else
				MessageBox.Show("Couldn't check the currencies conversions");
        }
        private void buttonDownloadMissingIcons_Click(object sender, EventArgs e) {

            Enabled = false;

            FormProgressBar form = new FormProgressBar(this, FormProgressBar.FormType.Icons);
            form.ShowDialog();

            Enabled = true;

        }

        private void buttonDefaultCurrencies_Click(object sender, EventArgs e) {
			_sett.Default(Settings.DefaultType.Coins, _page);
			foreach(Settings.CoinList cl in _sett.Coins) // default doesn't load the icons
				foreach(Settings.StCoin st in cl)
					st.Icon = Global.GetIcon(st.Coin, 16);
		}

		#endregion

		#region Basic Tab

		private void buttonProfileMakeDefault_Click(object sender, EventArgs e) {
			using(StreamWriter writer = new StreamWriter(Global.LocationProfileIni)) {
				writer.WriteLine(textBoxProfileName.Text);
			}
			MessageBox.Show(textBoxProfileName.Text + " marked as default profile");
		}
		private void buttonProfileOpen_Click(object sender, EventArgs e) {

			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = "Select Profile";
			ofd.Filter = "Profile Files (.json)|*.json";
			ofd.InitialDirectory = Global.FolderProfiles;
			ofd.Multiselect = false;

			ofd.FileOk += (f_sender, f_ev) => {

				Settings sett = new Settings();
				if(!sett.BindFile(ofd.FileName) || !sett.Load() || !sett.Check()) {
					MessageBox.Show("The provided profile is either invalid for this version or corrupted");
					return;
				}

				sett.CloneTo(_sett);

				if(Global.FolderProfiles != (Path.GetDirectoryName(ofd.FileName) + "\\")) {
					using(Stream stream = (f_sender as OpenFileDialog).OpenFile()) {
						stream.Position = 0;
						using(StreamWriter writer = new StreamWriter(Global.FolderProfiles + ofd.SafeFileName)) {
							stream.CopyTo(writer.BaseStream);
						}
					}
				}

				ClearBindings();
				BindSettings();
				textBoxProfileName.Text = ofd.SafeFileName;
			};

			ofd.ShowDialog();
			
		}
		private void buttonProfileCreate_Click(object sender, EventArgs e) {

			string name = Microsoft.VisualBasic.Interaction.InputBox("Enter the name of the new Profile", "Create Profile", "", 100, 100);

			if(name != "") {

				if(File.Exists(Global.FolderProfiles + name + ".json")) {
					MessageBox.Show("There's already a profile named " + name);
					return;
				}

				Settings.CreateSettFile(Global.FolderProfiles + name + ".json");

				_sett.BindFile(Global.FolderProfiles + name + ".json");
				_sett.Store();
				_sett.Save();

				ClearBindings();
				BindSettings();
				textBoxProfileName.Text = name + ".json";
			}
		}
		private void buttonProfileOpenFolder_Click(object sender, EventArgs e) {
			Process.Start(Global.FolderProfiles);
		}

		private void comboTheme_SelectedIndexChanged(object sender, EventArgs e) {
			_sett.Default(comboTheme.SelectedIndex == 0 ? Settings.DefaultType.ColorLight : Settings.DefaultType.ColorDark);
		}

        private void buttonDefaultBasic_Click(object sender, EventArgs e) {
			_sett.Default(Settings.DefaultType.Basic | Settings.DefaultType.Visibility);
		}

		#endregion

		#region Columns Tab

		private void buttonColUp_Click(object sender, EventArgs e) {
			if(colsGrid.SelectedRows.Count > 0 && colsGrid.SelectedRows[0].Index > 0) {
				int index1 = colsGrid.SelectedRows[0].Index;
				int index2 = colsGrid.SelectedRows[0].Index - 1;
				Settings.StColumn ptr = _sett.Grid.Columns[index1];
				_sett.Grid.Columns.RemoveAt(index1);
				_sett.Grid.Columns.Insert(index2, ptr);
				colsGrid.Rows[index2].Selected = true;
			}
		}
		private void buttonColDown_Click(object sender, EventArgs e) {
			if(colsGrid.SelectedRows.Count > 0 && colsGrid.SelectedRows[0].Index < colsGrid.RowCount - 1) {
				int index1 = colsGrid.SelectedRows[0].Index;
				int index2 = colsGrid.SelectedRows[0].Index + 1;
				Settings.StColumn ptr = _sett.Grid.Columns[index1];
				_sett.Grid.Columns.RemoveAt(index1);
				_sett.Grid.Columns.Insert(index2, ptr);
				colsGrid.Rows[index2].Selected = true;
			}
		}

		private void buttonDefaultColumns_Click(object sender, EventArgs e) {
			_sett.Default(Settings.DefaultType.Grid);
		}

		#endregion

		#region Advanced Tab

		private void linkAcceptedMarkets_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start("https://www.cryptocompare.com/exchanges/#/overview");
		}

		private void buttonDefaultAdvanced_Click(object sender, EventArgs e) {
			_sett.Default(Settings.DefaultType.Coords | Settings.DefaultType.Metrics | Settings.DefaultType.Pages | Settings.DefaultType.Market);
        }

		#endregion

		#region Charts Tab

		private void comboChartTheme_SelectedIndexChanged(object sender, EventArgs e) {
			_sett.Default(comboChartTheme.SelectedIndex == 0 ? Settings.DefaultType.ChartColorLight : Settings.DefaultType.ChartColorDark);
		}

		private void buttonDefaultCharts_Click(object sender, EventArgs e) {
			_sett.Default(Settings.DefaultType.Chart);
		}

		#endregion

		#region Additional tab

		private void OpenUrlLink(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start((sender as LinkLabel).Text);
		}

		private void buttonDonationCopyBTC_Click(object sender, EventArgs e) {
			Clipboard.SetText(textBoxDonationBTC.Text);
			MessageBox.Show("Bitcoin (BTC) Donation Address copied to the clipboard");
		}
		private void buttonDonationCopyBCH_Click(object sender, EventArgs e) {
			Clipboard.SetText(textBoxDonationBCH.Text);
			MessageBox.Show("Bitcoin Cash (BCH) Donation Address copied to the clipboard");
		}
		private void buttonDonationCopyETH_Click(object sender, EventArgs e) {
			Clipboard.SetText(textBoxDonationETH.Text);
			MessageBox.Show("Ethereum (ETH) Donation Address copied to the clipboard");
		}
		private void buttonDonationCopyDASH_Click(object sender, EventArgs e) {
			Clipboard.SetText(textBoxDonationDASH.Text);
			MessageBox.Show("Digital Cash (DASH) Donation Address copied to the clipboard");
		}
		private void buttonDonationCopyLTC_Click(object sender, EventArgs e) {
			Clipboard.SetText(textBoxDonationLTC.Text);
			MessageBox.Show("Litecoin (LTC) Donation Address copied to the clipboard");
		}

		private void buttonDonationQrBTC_Click(object sender, EventArgs e) {
			labelQrCodeName.Text = "BTC QR Code";
			pictureBoxQrCode.Image = Properties.Resources.qr_btc_icon;
		}
		private void buttonDonationQrBCH_Click(object sender, EventArgs e) {
			labelQrCodeName.Text = "BCH QR Code";
			pictureBoxQrCode.Image = Properties.Resources.qr_bch_icon;
		}
		private void buttonDonationQrETH_Click(object sender, EventArgs e) {
			labelQrCodeName.Text = "ETH QR Code";
			pictureBoxQrCode.Image = Properties.Resources.qr_eth_icon;
		}
		private void buttonDonationQrDASH_Click(object sender, EventArgs e) {
			labelQrCodeName.Text = "DASH QR Code";
			pictureBoxQrCode.Image = Properties.Resources.qr_dash_icon;
		}
		private void buttonDonationQrLTC_Click(object sender, EventArgs e) {
			labelQrCodeName.Text = "LTC QR Code";
			pictureBoxQrCode.Image = Properties.Resources.qr_ltc_icon;
		}

		private void linkCryptoCompareAPI_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start("https://www.cryptocompare.com/api/#");
		}

		#endregion

		#region Shared on all Tabs

		private void buttonPageSwap_Click(object sender, EventArgs e) {
			_ptr_form.SwapPage(_page);
		}
		private void buttonApply_Click(object sender, EventArgs e) {
			ApplySettings();
		}
		private void buttonAccept_Click(object sender, EventArgs e) {
			ApplySettings();
            Close();
        }
        private void buttonCancel_Click(object sender, EventArgs e) {
            Close();
        }

		private void comboPages_SelectedIndexChanged(object sender, EventArgs e) {
			_page = comboPages.SelectedIndex;
			BindCoins();
		}
		public void ButtonColorPick(object sender, EventArgs e) {
			ColorDialog cd = new ColorDialog();
			cd.Color = (sender as Button).BackColor;
			cd.FullOpen = true;
			cd.ShowDialog();
			(sender as Button).BackColor = cd.Color;
		}
		public void DropDownOnClick(object sender, EventArgs e) {
			(sender as ComboBox).DroppedDown = true;
		}

		#endregion

	}

}
