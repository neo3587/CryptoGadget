
using System;
using System.Windows.Forms;
using System.IO;

using Newtonsoft.Json.Linq;


using CoinPair = System.Collections.Generic.KeyValuePair<string, string>;



// this will get some additions in future versions (Alarms, Graphs, ??)

namespace CryptoGadget {

    public partial class FormCoinSettings : Form {

        private Settings.CoinList _ptr_list = null;
        private BindingSource _coin_bind   = new BindingSource();
		private BindingSource _target_bind = new BindingSource();
		private bool _editing = false;

		public Settings.StCoin CoinResult = null;


		private void IndexName(ComboBox combo_box) {

			string selected = "[" + ((CoinPair)combo_box.SelectedItem).Value + ", " + ((CoinPair)combo_box.SelectedItem).Key + "]";

			for(int i = 0; i < (combo_box.DataSource as BindingSource).Count; i++) {
				CoinPair cp = (CoinPair)(combo_box.DataSource as BindingSource)[i];
				(combo_box.DataSource as BindingSource)[i] = new CoinPair(cp.Value, cp.Key);
			}

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


        public FormCoinSettings(Settings.CoinList coin_list, Settings.StCoin default_conv, bool editing = false) {

            InitializeComponent();
            _ptr_list = coin_list; // just for FindConv
			_editing = editing;

            HandleCreated += (sender, e) => {

				foreach(JProperty jprop in Global.Json["Data"]) {
					string name		 = jprop.Value["Name"].ToString();
					string full_name = jprop.Value["CoinName"].ToString();
					_coin_bind.Add(new CoinPair(name, full_name));
					_target_bind.Add(new CoinPair(name, full_name));
				}

				comboCoin.DataSource   = _coin_bind;
                comboTarget.DataSource = _target_bind;

				comboCoin.SelectedIndex   = default_conv.Coin == "" ? 0 : Math.Max(comboCoin.FindStringExact("[" + default_conv.Coin + ", " + default_conv.CoinName + "]"), 0);
				comboTarget.SelectedIndex = Math.Max(comboTarget.FindStringExact(default_conv.Target == "" ? "[USD, United States Dollar]" : "[" + default_conv.Target + ", " + default_conv.TargetName + "]"), 0);

            };

        }


        private void buttonAccept_Click(object sender, EventArgs e) {

            CoinPair left = (CoinPair)comboCoin.SelectedItem;
            CoinPair right = (CoinPair)comboTarget.SelectedItem;

            if(checkCoinIndexName.Checked) 
                left = new CoinPair(left.Value, left.Key);
			if(checkTargetIndexName.Checked)
                right = new CoinPair(right.Value, right.Key);
            
            if(!_editing && _ptr_list.FindConv(left.Key, right.Key) != -1) {
				MessageBox.Show(left.Key + " => " + right.Key + " conversion is already being used");
				return;
            }

			CoinResult = new Settings.StCoin();
			CoinResult.Icon       = Global.GetIcon(left.Key, 16);
			CoinResult.Coin       = left.Key;
			CoinResult.CoinName   = left.Value;
			CoinResult.Target	  = right.Key;
			CoinResult.TargetName = right.Value;
			CoinResult.Alert.Above = (float)numAlertAbove.Value;
			CoinResult.Alert.Below = (float)numAlertBelow.Value;

			Close();
        }
        private void buttonCancel_Click(object sender, EventArgs e) {
            Close();
        }


		private void checkCoinIndexName_CheckedChanged(object sender, EventArgs e) {
			IndexName(comboCoin);			
		}
		private void checkCoinOnlyFiat_CheckedChanged(object sender, EventArgs e) {
			OnlyFiat(comboCoin, _coin_bind, checkCoinIndexName.Checked, checkCoinOnlyFiat.Checked);
		}

		private void checkTargetIndexName_CheckedChanged(object sender, EventArgs e) {
			IndexName(comboTarget);
		}
		private void checkTargetOnlyFiat_CheckedChanged(object sender, EventArgs e) {
			OnlyFiat(comboTarget, _target_bind, checkTargetIndexName.Checked, checkTargetOnlyFiat.Checked);
		}

		private void DropDownOnClick(object sender, EventArgs e) {
			(sender as ComboBox).DroppedDown = true;
		}
		private void DropDownOnKeyPress(object sender, KeyPressEventArgs e) {
			(sender as ComboBox).DroppedDown = true;
		}

		private void buttonIconSwap_Click(object sender, EventArgs e) {

			OpenFileDialog ofd = new OpenFileDialog();

			CoinPair coin = (CoinPair)comboCoin.SelectedItem;
			if(checkCoinIndexName.Checked)
				coin = new CoinPair(coin.Value, coin.Key);

			ofd.Title = "Select Icon for " + coin.Key + " (" + coin.Value + ")";
			ofd.Filter = "Icon Files (*.ico, *.png, *.jpg)|*.ico;*png;*jpg";
			ofd.Multiselect = false;

			ofd.FileOk += (f_sender, f_ev) => {
				using(Stream stream = (f_sender as OpenFileDialog).OpenFile()) {
					stream.Position = 0;
					Global.SetIcon(coin.Key, stream);
				}
			};

			ofd.ShowDialog();
		}
		private void buttonIconReDownload_Click(object sender, EventArgs e) {

			CoinPair coin = (CoinPair)comboCoin.SelectedItem;
			if(checkCoinIndexName.Checked)
				coin = new CoinPair(coin.Value, coin.Key);
			try {
				Global.SetIcon(coin.Key, CCRequest.DownloadIcon("https://www.cryptocompare.com" + Global.Json["Data"][coin.Key]["ImageUrl"]));
				MessageBox.Show(coin.Key + " succesfully updated");
			} catch(System.Net.WebException) {
				MessageBox.Show("Couldn't download the " + coin.Key + "icon from the server");
			} catch(InvalidOperationException) {
				MessageBox.Show(coin.Key + " doesn't have an associated url for this coin, it may get fixed by re-downloading the coin list");
			} catch {
				MessageBox.Show("Unexpected error");
			}
			
		}

	}

}
