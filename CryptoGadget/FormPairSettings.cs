
using System;
using System.Windows.Forms;
using System.IO;
using System.Linq;

using Newtonsoft.Json.Linq;



namespace CryptoGadget {

    public partial class FormPairSettings : Form {

		private class CoinPair {
			public string Left { get; set; }
			public string Right { get; set; }

			public CoinPair(string left, string right) {
				Left = left;
				Right = right;
			}
			public void Reverse() {
				string buff = Left;
				Left = Right;
				Right = buff;
			}
			public string OriginalLeft(bool reversed) {
				return reversed ? Right : Left;
			}
			public string OriginalRight(bool reversed) {
				return reversed ? Left : Right;
			}
			public override string ToString() {
				return "[" + Left + ", " + Right + "]";
			}
		}

        private readonly Settings.CoinList _ptr_list = null;
		private readonly Settings.StCoin _default_conv = null;
		private (BindingSource coin, BindingSource target) _bind = (new BindingSource(), new BindingSource());
		private bool _editing = false;
		private static bool _show_alerts = false;

		public Settings.StCoin CoinResult = null;


		private T GetSelectedItem<T>(ComboBox box) { // using TAB key will set SelectedIndex to -1, but the text remains
			return (T)box.Items[box.FindStringExact(box.Text)];
		}


        public FormPairSettings(Settings.CoinList coin_list, Settings.StCoin default_conv, bool editing = false) {

            InitializeComponent();
            _ptr_list = coin_list; // just for FindConv
			_default_conv = default_conv;
			_editing = editing;

			Global.ControlApply<ComboBox>(this, ctrl => {
				ctrl.Click += Global.DropDownOnClick;
				ctrl.KeyPress += Global.DropDownOnKeyPress;
			});

			HandleCreated += (sender, e) => {

				Text = "CryptoGadget Settings " + (_editing ? "[Edit Coin]" : "[Add Coin]");
				ShowAlerts(_show_alerts);

				foreach(JProperty jprop in Global.Json["Data"]) {
					string name		 = jprop.Value["Name"].ToString();
					string full_name = jprop.Value["CoinName"].ToString();
					_bind.coin.Add(new CoinPair(name, full_name));
					_bind.target.Add(new CoinPair(name, full_name));
				}

				comboCoin.DataSource   = _bind.coin;
                comboTarget.DataSource = _bind.target;

				comboCoin.SelectedIndex   = _default_conv.Coin == "" ? 0 : Math.Max(comboCoin.FindStringExact("[" + _default_conv.Coin + ", " + _default_conv.CoinName + "]"), 0);
				comboTarget.SelectedIndex = Math.Max(comboTarget.FindStringExact(_default_conv.Target == "" ? "[USD, United States Dollar]" : "[" + _default_conv.Target + ", " + _default_conv.TargetName + "]"), 0);

				if(editing) {
					comboAlertAbove.Items.AddRange(_default_conv.Alert.Above.Cast<object>().ToArray());
					comboAlertBelow.Items.AddRange(_default_conv.Alert.Below.Cast<object>().ToArray());
					comboAlertAbove.SelectedIndex = comboAlertAbove.Items.Count > 0 ? 0 : -1;
					comboAlertBelow.SelectedIndex = comboAlertBelow.Items.Count > 0 ? 0 : -1;
				}

			};

        }


		#region Pair methods

		private void IndexName(ComboBox combo_box) {

			string selected = "[" + (combo_box.SelectedItem as CoinPair).Right + ", " + (combo_box.SelectedItem as CoinPair).Left + "]";

			for(int i = 0; i < (combo_box.DataSource as BindingSource).Count; i++)
				((combo_box.DataSource as BindingSource)[i] as CoinPair).Reverse();

			(combo_box.DataSource as BindingSource).ResetBindings(false);
			combo_box.SelectedIndex = combo_box.FindStringExact(selected);
		}
		private void OnlyFiat(ComboBox combo_box, BindingSource bind, bool index_fullname, bool only_fiat) {

			if(only_fiat) {

				BindingSource fiat_list = new BindingSource();
				string left = !index_fullname ? "Name" : "CoinName";
				string right = index_fullname ? "Name" : "CoinName";

				foreach(JProperty jprop in Global.Json["Data"]) {
					if(jprop.Value["FiatCurrency"] != null)
						fiat_list.Add(new CoinPair(jprop.Value[left].ToString(), jprop.Value[right].ToString()));
				}
				combo_box.DataSource = fiat_list;
			}
			else {
				combo_box.DataSource = bind;
			}

			(combo_box.DataSource as BindingSource).ResetBindings(false);
			combo_box.SelectedIndex = 0;
		}
		private void IconReDownload(string coin) {
			try {
				Directory.CreateDirectory(Global.FolderIcons);
				if(Global.Json["Data"]?[coin]?["FiatCurrency"] != null) {
					MessageBox.Show("Fiat currencies icons are not available for download, find a icon for yourself and use \"Icon Swap\" to add/swap it");
					return;
				}
				Global.SetIcon(coin, CCRequest.DownloadIcon("https://www.cryptocompare.com" + Global.Json["Data"][coin]["ImageUrl"]));
				MessageBox.Show(coin + " succesfully updated");
			} catch(System.Net.WebException) {
				MessageBox.Show("Couldn't download the " + coin + " icon from the server");
			} catch(InvalidOperationException) {
				MessageBox.Show(coin + " doesn't have an associated url for this coin, it may get fixed by re-downloading the coin list");
			} catch {
				MessageBox.Show("Unexpected error");
			}
		}
		private void IconSwap(string coin, string fullname) {

			OpenFileDialog ofd = new OpenFileDialog();

			ofd.Title = "Select Icon for " + coin + " (" + fullname + ")";
			ofd.Filter = "Icon Files (*.ico, *.png, *.jpg)|*.ico;*png;*jpg";
			ofd.Multiselect = false;

			ofd.FileOk += (f_sender, f_ev) => {
				Directory.CreateDirectory(Global.FolderIcons);
				using(Stream stream = (f_sender as OpenFileDialog).OpenFile()) {
					stream.Position = 0;
					Global.SetIcon(coin, stream);
				}
			};

			ofd.ShowDialog();
		}


		private void checkCoinIndexName_CheckedChanged(object sender, EventArgs e) {
			IndexName(comboCoin);
		}
		private void checkCoinOnlyFiat_CheckedChanged(object sender, EventArgs e) {
			OnlyFiat(comboCoin, _bind.coin, checkCoinIndexName.Checked, checkCoinOnlyFiat.Checked);
		}
		private void buttonIconReDownload_Click(object sender, EventArgs e) {
			IconReDownload(GetSelectedItem<CoinPair>(comboCoin).OriginalLeft(checkCoinIndexName.Checked));
		}
		private void buttonIconSwap_Click(object sender, EventArgs e) {
			CoinPair cp = GetSelectedItem<CoinPair>(comboCoin);
			IconSwap(cp.OriginalLeft(checkCoinIndexName.Checked), cp.OriginalRight(checkCoinIndexName.Checked));
		}

		private void checkTargetIndexName_CheckedChanged(object sender, EventArgs e) {
			IndexName(comboTarget);
		}
		private void checkTargetOnlyFiat_CheckedChanged(object sender, EventArgs e) {
			OnlyFiat(comboTarget, _bind.target, checkTargetIndexName.Checked, checkTargetOnlyFiat.Checked);
		}
		private void buttonIconTargetReDownload_Click(object sender, EventArgs e) {
			IconReDownload(GetSelectedItem<CoinPair>(comboTarget).OriginalLeft(checkTargetIndexName.Checked));
		}
		private void buttonIconTargetSwap_Click(object sender, EventArgs e) {
			CoinPair cp = GetSelectedItem<CoinPair>(comboTarget);
			IconSwap(cp.OriginalLeft(checkTargetIndexName.Checked), cp.OriginalRight(checkTargetIndexName.Checked));
		}

		#endregion

		#region Alert methods

		private void ShowAlerts(bool show) {
			groupBoxAlertAbove.Visible = groupBoxAlertBelow.Visible = show;
			buttonSwitchAlertView.Text = show ? "Hide Alerts" : "Show Alerts";
			Size = new System.Drawing.Size(Width, show ? 265 : 181);
		}
		private void NumericUpDownDecSeparator(object sender, KeyPressEventArgs e) {
			if(e.KeyChar.Equals('.') || e.KeyChar.Equals(','))
				e.KeyChar = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToCharArray()[0];
		}
		private void NumericUpDownTrim(object sender, EventArgs e) {
			string str = (sender as NumericUpDown).Value.ToString("0.00000000");
			(sender as NumericUpDown).DecimalPlaces = Global.Constrain(str.IndexOfAny(new char[] { ',', '.' }) != -1 ? 8 - (str.Length - str.TrimEnd('0').Length) : 0, 0, 8);
		}
		private void AddAlert(ComboBox box, NumericUpDown numeric) {
			if(numeric.Value > 0 && !box.Items.Cast<decimal>().Contains(numeric.Value))
				box.SelectedIndex = box.Items.Add(numeric.Value);
		}
		private void RemoveAlert(ComboBox box) {
			if(box.Items.Count > 0) {
				int index = box.SelectedIndex;
				box.Items.Remove(GetSelectedItem<decimal>(box));
				box.SelectedIndex = Math.Min(box.Items.Count -1, index);
				if(box.Items.Count == 0)
					box.Text = "";
			}
		}

		private void buttonSwitchAlertView_Click(object sender, EventArgs e) {
			_show_alerts = !_show_alerts;
			ShowAlerts(_show_alerts);
		}

		private void comboAlertAbove_SelectedIndexChanged(object sender, EventArgs e) {
			numAlertAbove.Value = GetSelectedItem<decimal>(comboAlertAbove);
		}
		private void buttonAddAlertAbove_Click(object sender, EventArgs e) {
			AddAlert(comboAlertAbove, numAlertAbove);
		}
		private void buttonRemoveAlertAbove_Click(object sender, EventArgs e) {
			RemoveAlert(comboAlertAbove);
		}

		private void comboAlertBelow_SelectedIndexChanged(object sender, EventArgs e) {
			numAlertBelow.Value = GetSelectedItem<decimal>(comboAlertBelow);
		}
		private void buttonAddAlertBelow_Click(object sender, EventArgs e) {
			AddAlert(comboAlertBelow, numAlertBelow);
		}
		private void buttonRemoveAlertBelow_Click(object sender, EventArgs e) {
			RemoveAlert(comboAlertBelow);
		}

		#endregion

		private void buttonAccept_Click(object sender, EventArgs e) {

			CoinPair coin = GetSelectedItem<CoinPair>(comboCoin);
			CoinPair target = GetSelectedItem<CoinPair>(comboTarget);

			if(checkCoinIndexName.Checked)
				coin.Reverse();
			if(checkTargetIndexName.Checked)
				target.Reverse();
			if(_ptr_list.FindConv(coin.Left, target.Left) != -1 && (!_editing || (_editing && (coin.Left != _default_conv.Coin || target.Left != _default_conv.Target)))) {
				MessageBox.Show(coin.Left + " => " + target.Left + " conversion is already being used");
				return;
			}

			CoinResult = new Settings.StCoin();
			CoinResult.Icon = Global.GetIcon(coin.Left, 16);
			CoinResult.Coin = coin.Left;
			CoinResult.CoinName = coin.Right;
			CoinResult.Target = target.Left;
			CoinResult.TargetName = target.Right;
			CoinResult.Alert.Above = comboAlertAbove.Items.Cast<decimal>().ToList();
			CoinResult.Alert.Below = comboAlertBelow.Items.Cast<decimal>().ToList();
			CoinResult.AlertType = (CoinResult.Alert.Above.Count > 0 && CoinResult.Alert.Below.Count > 0) ? '↕' :
								   (CoinResult.Alert.Above.Count > 0) ? '↑' :
								   (CoinResult.Alert.Below.Count > 0) ? '↓' : '-';

			Close();
		}
		private void buttonCancel_Click(object sender, EventArgs e) {
			Close();
		}

		
	}

}
