
using System;
using System.Windows.Forms;
using System.IO;

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

		public Settings.StCoin CoinResult = null;


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


        public FormPairSettings(Settings.CoinList coin_list, Settings.StCoin default_conv, bool editing = false) {

            InitializeComponent();
            _ptr_list = coin_list; // just for FindConv
			_default_conv = default_conv;
			_editing = editing;

			comboCoin.Click += Global.DropDownOnClick;
			comboCoin.KeyPress += Global.DropDownOnKeyPress;
			comboTarget.Click += Global.DropDownOnClick;
			comboTarget.KeyPress += Global.DropDownOnKeyPress;

			HandleCreated += (sender, e) => {

				Text = "CryptoGadget Settings " + (_editing ? "[Edit Coin]" : "[Add Coin]");

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
				numAlertAbove.Value = _default_conv.Alert.Above;
				numAlertBelow.Value = _default_conv.Alert.Below;

			};

        }


        private void buttonAccept_Click(object sender, EventArgs e) {

            CoinPair coin = (comboCoin.SelectedItem as CoinPair);
            CoinPair target = (comboTarget.SelectedItem as CoinPair);

			if(checkCoinIndexName.Checked)
				coin.Reverse();
			if(checkTargetIndexName.Checked)
				target.Reverse();
			if(_ptr_list.FindConv(coin.Left, target.Left) != -1 && (!_editing || (_editing && (coin.Left != _default_conv.Coin || target.Left != _default_conv.Target)))) {
				MessageBox.Show(coin.Left + " => " + target.Left + " conversion is already being used");
				return;
            }
			
			CoinResult = new Settings.StCoin();
			CoinResult.Icon       = Global.GetIcon(coin.Left, 16);
			CoinResult.Coin       = coin.Left;
			CoinResult.CoinName   = coin.Right;
			CoinResult.Target	  = target.Left;
			CoinResult.TargetName = target.Right;
			CoinResult.Alert.Above = numAlertAbove.Value;
			CoinResult.Alert.Below = numAlertBelow.Value;

			Close();
        }
        private void buttonCancel_Click(object sender, EventArgs e) {
            Close();
        }


		private void checkCoinIndexName_CheckedChanged(object sender, EventArgs e) {
			IndexName(comboCoin);			
		}
		private void checkCoinOnlyFiat_CheckedChanged(object sender, EventArgs e) {
			OnlyFiat(comboCoin, _bind.coin, checkCoinIndexName.Checked, checkCoinOnlyFiat.Checked);
		}
		private void buttonIconReDownload_Click(object sender, EventArgs e) {
			IconReDownload((comboCoin.SelectedItem as CoinPair).OriginalLeft(checkCoinIndexName.Checked));
		}
		private void buttonIconSwap_Click(object sender, EventArgs e) {
			IconSwap((comboCoin.SelectedItem as CoinPair).OriginalLeft(checkCoinIndexName.Checked), (comboCoin.SelectedItem as CoinPair).OriginalRight(checkCoinIndexName.Checked));
		}

		private void checkTargetIndexName_CheckedChanged(object sender, EventArgs e) {
			IndexName(comboTarget);
		}
		private void checkTargetOnlyFiat_CheckedChanged(object sender, EventArgs e) {
			OnlyFiat(comboTarget, _bind.target, checkTargetIndexName.Checked, checkTargetOnlyFiat.Checked);
		}
		private void buttonIconTargetReDownload_Click(object sender, EventArgs e) {
			IconReDownload((comboTarget.SelectedItem as CoinPair).OriginalLeft(checkTargetIndexName.Checked));
		}
		private void buttonIconTargetSwap_Click(object sender, EventArgs e) {
			IconSwap((comboTarget.SelectedItem as CoinPair).OriginalLeft(checkTargetIndexName.Checked), (comboTarget.SelectedItem as CoinPair).OriginalRight(checkTargetIndexName.Checked));
		}

	}

}
