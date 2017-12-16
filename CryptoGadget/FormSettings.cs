
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Reflection;

using Newtonsoft.Json.Linq;


namespace CryptoGadget {

    public partial class FormSettings : Form {

        private FormMain _ptrForm;
		private Settings _sett = new Settings();
		private int _page = 0;

        public bool accept = false;

		
        private JObject DownloadCoinDB() {
			Enabled = false;
            FormProgressBar form = new FormProgressBar(this, FormProgressBar.FormType.CoinList);
            form.ShowDialog();
            Enabled = true;
            if(form.coindb == null)
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
                    (form.coindb["Data"] as JObject).Add(fiat[i].Item1, JToken.Parse("{ \"Name\": \"" + fiat[i].Item1 + "\", \"CoinName\": \"" + fiat[i].Item2 +
                                                                                     "\", \"FullName\": \"" + fiat[i].Item2 + " (" + fiat[i].Item1 + ")\"" + ", \"FiatCurrency\": \"true\"" + " }"));
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

			foreach(Settings.StCoin st in _sett.Coins[_page]) {
				st.Icon = Global.GetIcon(st.Coin, new Size(16, 16));
			}
			if(Global.Json == null && GetCoinDB()) {
				foreach(Settings.StCoin st in _sett.Coins[_page]) {
					st.CoinName   = Global.Json["Data"][st.Coin]["CoinName"].ToString();
					st.TargetName = Global.Json["Data"][st.Target]["CoinName"].ToString();
				}
			}

			PropertyInfo[] coin_props = Settings.StCoin.GetProps();
			for(int i = 0; i < coin_props.Length; i++)
				coinGrid.Columns[i].DataPropertyName = coin_props[i].Name;

			BindingSource coin_bind = new BindingSource();
			coin_bind.DataSource = _sett.Coins[_page];

			Invoke((MethodInvoker)delegate { coinGrid.DataSource = coin_bind; });


			numRefreshRate.DataBindings.Add("Value", _sett.Basic, "RefreshRate");
			
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
			buttonColorCoin.DataBindings.Add("BackColor", _sett.Color, "Coin");
			buttonColorValue.DataBindings.Add("BackColor", _sett.Color, "Value");
			buttonColorBackground1.DataBindings.Add("BackColor", _sett.Color, "Background1");
			buttonColorBackground2.DataBindings.Add("BackColor", _sett.Color, "Background2");
			buttonColorPositiveRefresh.DataBindings.Add("BackColor", _sett.Color, "PositiveRefresh");
			buttonColorNegativeRefresh.DataBindings.Add("BackColor", _sett.Color, "NegativeRefresh");
			buttonColorPositiveChange.DataBindings.Add("BackColor", _sett.Color, "PositiveChange");
			buttonColorNegativeChange.DataBindings.Add("BackColor", _sett.Color, "NegativeChange");
			buttonColorHeaderText.DataBindings.Add("BackColor", _sett.Color, "HeaderText");
			buttonColorHeaderBackground.DataBindings.Add("BackColor", _sett.Color, "HeaderBackground");
			buttonColorEdge.DataBindings.Add("BackColor", _sett.Color, "Edge");

			numCoordsPosX.DataBindings.Add("Value", _sett.Coords, "PosX");
			numCoordsPosY.DataBindings.Add("Value", _sett.Coords, "PosY");
			checkCoordsExitSave.DataBindings.Add("Checked", _sett.Coords, "ExitSave");
			checkCoordsLockPos.DataBindings.Add("Checked", _sett.Coords, "LockPos");

			// for(int i = 0; i < props.Length; i++)
			numMetricsEdge.DataBindings.Add("Value", _sett.Metrics, "Edge");
			numMetricsHeader.DataBindings.Add("Value", _sett.Metrics, "Header");
			numMetricsRows.DataBindings.Add("Value", _sett.Metrics, "Rows");
			numMetricsIconSize.DataBindings.Add("Value", _sett.Metrics, "IconSize");
			numMetricsHeaderText.DataBindings.Add("Value", _sett.Metrics, "HeaderText");
			numMetricsRowsValues.DataBindings.Add("Value", _sett.Metrics, "RowsValues");


			PropertyInfo[] cols_props = Settings.StColumn.GetProps();
			for(int i = 0; i < cols_props.Length; i++)
				colsGrid.Columns[i].DataPropertyName = cols_props[i].Name;

			BindingList<Settings.StColumn> bl = new BindingList<Settings.StColumn>();
			foreach(PropertyInfo prop in Settings.StGrid.GetProps()) 
				bl.Add((Settings.StColumn)_sett.Grid[prop.Name]);

			BindingSource cols_bind = new BindingSource();
			cols_bind.DataSource = bl;
			Invoke((MethodInvoker)delegate { colsGrid.DataSource = bl; }); 

		}


		public FormSettings(FormMain form) {
            InitializeComponent();
            _ptrForm = form;
			_sett = (Settings)Global.Sett.Clone();
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
            if(coinGrid.SelectedRows.Count > 0) 
				_sett.Coins[_page].RemoveAt(coinGrid.SelectedRows[0].Index);
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
                    StreamWriter writer = new StreamWriter(Global.IconFolder + coin.ToLower() + ".ico");
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
			_sett.Default(Settings.DefaultType.Coins);
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
			_sett.Default(Settings.DefaultType.Coords | Settings.DefaultType.Metrics);
        }

        #endregion

        #region Shared on all Tabs

        private void buttonAccept_Click(object sender, EventArgs e) {
			Global.Sett = (Settings)_sett.Clone();
			Global.Sett.Save();
            accept = true;
            Close();
        }
        private void buttonCancel_Click(object sender, EventArgs e) {
            Close();
        }

        private void buttonColorPick(object sender, EventArgs e) => neo.FormUtil.buttonColorPick(sender, e);

		#endregion


		// drag and drop test -> note: need to use the bound data (thx stack overflow)
		private DataGridView dataGridView1 = new DataGridView();
		private Rectangle dragBoxFromMouseDown;
		private int rowIndexFromMouseDown;
		private int rowIndexOfItemUnderMouseToDrop;
		private void dataGridView1_MouseMove(object sender, MouseEventArgs e) {
			if((e.Button & MouseButtons.Left) == MouseButtons.Left) {
				// If the mouse moves outside the rectangle, start the drag.
				if(dragBoxFromMouseDown != Rectangle.Empty &&
					!dragBoxFromMouseDown.Contains(e.X, e.Y)) {

					// Proceed with the drag and drop, passing in the list item.                    
					DragDropEffects dropEffect = dataGridView1.DoDragDrop(
					dataGridView1.Rows[rowIndexFromMouseDown],
					DragDropEffects.Move);
				}
			}
		}
		private void dataGridView1_MouseDown(object sender, MouseEventArgs e) {
			// Get the index of the item the mouse is below.
			rowIndexFromMouseDown = dataGridView1.HitTest(e.X, e.Y).RowIndex;
			if(rowIndexFromMouseDown != -1) {
				// Remember the point where the mouse down occurred. 
				// The DragSize indicates the size that the mouse can move 
				// before a drag event should be started.                
				Size dragSize = SystemInformation.DragSize;

				// Create a rectangle using the DragSize, with the mouse position being
				// at the center of the rectangle.
				dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
															   e.Y - (dragSize.Height / 2)),
									dragSize);
			}
			else
				// Reset the rectangle if the mouse is not over an item in the ListBox.
				dragBoxFromMouseDown = Rectangle.Empty;
		}
		private void dataGridView1_DragOver(object sender, DragEventArgs e) {
			e.Effect = DragDropEffects.Move;
		}
		private void dataGridView1_DragDrop(object sender, DragEventArgs e) {
			// The mouse locations are relative to the screen, so they must be 
			// converted to client coordinates.
			Point clientPoint = dataGridView1.PointToClient(new Point(e.X, e.Y));

			// Get the row index of the item the mouse is below. 
			rowIndexOfItemUnderMouseToDrop =
				dataGridView1.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

			// If the drag operation was a move then remove and insert the row.
			if(e.Effect == DragDropEffects.Move) {
				DataGridViewRow rowToMove = e.Data.GetData(
					typeof(DataGridViewRow)) as DataGridViewRow;
				dataGridView1.Rows.RemoveAt(rowIndexFromMouseDown);
				dataGridView1.Rows.Insert(rowIndexOfItemUnderMouseToDrop, rowToMove);

			}
		}

	}

}
