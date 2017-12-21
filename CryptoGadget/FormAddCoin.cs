
using System;
using System.Windows.Forms;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using CoinPair = System.Collections.Generic.KeyValuePair<string, string>;



// this will get a full revamp in future versions

namespace CryptoGadget {

    public partial class FormAddCoin : Form {

        private Settings.CoinList _ptr_sett = null;
		private DataGridView	  _ptr_grid = null;
        private BindingSource _coin_bind   = new BindingSource();
		private BindingSource _target_bind = new BindingSource();

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


        public FormAddCoin(Settings.CoinList sett, DataGridView grid) {

            InitializeComponent();
            _ptr_sett = sett;
			_ptr_grid = grid;

            HandleCreated += (sender, e) => {

				foreach(JProperty jprop in Global.Json["Data"]) {
					string name		 = jprop.Value["Name"].ToString();
					string full_name = jprop.Value["CoinName"].ToString();
					_coin_bind.Add(new CoinPair(name, full_name));
					_target_bind.Add(new CoinPair(name, full_name));
				}

				boxCoin.DataSource   = _coin_bind;
                boxTarget.DataSource = _target_bind;

                boxCoin.SelectedIndex   = 0;
                boxTarget.SelectedIndex = Math.Max(boxTarget.FindStringExact("[USD, United States Dollar]"), 0);

            };

        }


        private void buttonAdd_Click(object sender, EventArgs e) {

            CoinPair left = (CoinPair)boxCoin.SelectedItem;
            CoinPair right = (CoinPair)boxTarget.SelectedItem;

            if(checkCoinIndexName.Checked) 
                left = new CoinPair(left.Value, left.Key);
			if(checkTargetIndexName.Checked)
                right = new CoinPair(right.Value, right.Key);
            
            if(_ptr_sett.FindConv(left.Key, right.Key) != -1) {
                MessageBox.Show(left.Key + " => " + right.Key + " conversion is already being used");
                return;
            }

			Settings.StCoin st = new Settings.StCoin();
			st.Icon       = Global.GetIcon(left.Key, 16);
			st.Coin       = left.Key;
			st.CoinName   = left.Value;
			st.Target	  = right.Key;
			st.TargetName = right.Value;

			int insertPos = _ptr_grid.SelectedRows.Count > 0 ? _ptr_grid.SelectedRows[0].Index +1 : 0;
			_ptr_sett.Insert(insertPos, st);
            _ptr_grid.Rows[Math.Min(_ptr_grid.SelectedRows[0].Index +1, _ptr_grid.RowCount-1)].Selected = true;
        }
        private void buttonDone_Click(object sender, EventArgs e) {
            Close();
        }


		private void checkCoinIndexName_CheckedChanged(object sender, EventArgs e) {
			IndexName(boxCoin);			
		}
		private void checkCoinOnlyFiat_CheckedChanged(object sender, EventArgs e) {
			OnlyFiat(boxCoin, _coin_bind, checkCoinIndexName.Checked, checkCoinOnlyFiat.Checked);
		}

		private void checkTargetIndexName_CheckedChanged(object sender, EventArgs e) {
			IndexName(boxTarget);
		}
		private void checkTargetOnlyFiat_CheckedChanged(object sender, EventArgs e) {
			OnlyFiat(boxTarget, _target_bind, checkTargetIndexName.Checked, checkTargetOnlyFiat.Checked);
		}

		private void DropDownOnClick(object sender, EventArgs e) {
			(sender as ComboBox).DroppedDown = true;
		}
		private void DropDownOnKeyPress(object sender, KeyPressEventArgs e) {
			(sender as ComboBox).DroppedDown = true;
		}


	}

}
