
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using BindList = System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>>;


// this will get a full revamp in future versions

namespace CryptoGadget {

    public partial class FormAddCoin : Form {

        private Settings.CoinList _ptr_sett = null;
		private DataGridView _ptr_grid = null;
        private BindList _pairs = new BindList();
        private BindingSource _c_bind = new BindingSource();
        private BindingSource _t_bind = new BindingSource();

        public FormAddCoin(Settings.CoinList sett, DataGridView grid) {

            InitializeComponent();
            _ptr_sett = sett;
			_ptr_grid = grid;

            HandleCreated += (sender, e) => {

                foreach(JProperty coin in Global.Json["Data"])
                    _pairs.Add(new KeyValuePair<string, string>(coin.Value["Name"].ToString(), coin.Value["CoinName"].ToString()));

                _c_bind.DataSource = _pairs;
                _t_bind.DataSource = _pairs;
                
                boxCoin.DataSource   = _c_bind;
                boxTarget.DataSource = _t_bind;

                boxCoin.SelectedIndex   = 0;
                boxTarget.SelectedIndex = Math.Max(boxTarget.FindStringExact("[USD, United States Dollar]"), 0);

            };

        }

        private void buttonAdd_Click(object sender, EventArgs e) {

            KeyValuePair<string, string> left = (KeyValuePair<string, string>)boxCoin.SelectedItem;
            KeyValuePair<string, string> right = (KeyValuePair<string, string>)boxTarget.SelectedItem;

            if(checkIndexName.Checked) {
                left = new KeyValuePair<string, string>(left.Value, left.Key);
                right = new KeyValuePair<string, string>(right.Value, right.Key);
            }

            if(_ptr_sett.FindConv(left.Key, right.Key) != -1) {
                MessageBox.Show(left.Key + " => " + right.Key + " conversion is already being used");
                return;
            }

			Settings.StCoin st = new Settings.StCoin();
			st.Icon = Global.GetIcon(left.Key, new Size(16, 16));
			st.Coin = left.Key;
			st.CoinName = left.Value;
			st.Target = right.Key;
			st.TargetName = right.Value;
			int insertPos = _ptr_grid.SelectedRows.Count > 0 ? _ptr_grid.SelectedRows[0].Index +1 : 0;
			_ptr_sett.Insert(insertPos, st);
            _ptr_grid.Rows[Math.Min(_ptr_grid.SelectedRows[0].Index +1, _ptr_grid.RowCount-1)].Selected = true;
        }
        private void buttonDone_Click(object sender, EventArgs e) {
            Close();
        }

        private void checkOnlyFiat_CheckedChanged(object sender, EventArgs e) {

            if(checkOnlyFiat.Checked) {

                BindList bl = new BindList();
                string left = !checkIndexName.Checked ? "Name" : "CoinName";
                string right = checkIndexName.Checked ? "Name" : "CoinName";

                foreach(JProperty coin in Global.Json["Data"]) 
                    if(coin.Value["FiatCurrency"] != null)
                        bl.Add(new KeyValuePair<string, string>(coin.Value[left].ToString(), coin.Value[right].ToString()));
                _t_bind.DataSource = bl;
            }
            else {
                _t_bind.DataSource = _pairs;
            }

            _t_bind.ResetBindings(false);
            boxTarget.SelectedIndex = 0;
        }
        private void checkIndexName_CheckedChanged(object sender, EventArgs e) {

            string i_left =  "[" + ((KeyValuePair<string, string>)boxCoin.SelectedItem).Value   + ", " + ((KeyValuePair<string, string>)boxCoin.SelectedItem).Key   + "]";
            string i_right = "[" + ((KeyValuePair<string, string>)boxTarget.SelectedItem).Value + ", " + ((KeyValuePair<string, string>)boxTarget.SelectedItem).Key + "]";

            for(int i = 0; i < _pairs.Count; i++)
                _pairs[i] = new KeyValuePair<string, string>(_pairs[i].Value, _pairs[i].Key);

            if(checkOnlyFiat.Checked) {
                BindList ptr = (_t_bind.DataSource as BindList);
                for(int i = 0; i < ptr.Count; i++)
                    ptr[i] = new KeyValuePair<string, string>(ptr[i].Value, ptr[i].Key);
            }

            _c_bind.ResetBindings(false);
            _t_bind.ResetBindings(false);

            boxCoin.SelectedIndex   = boxCoin.FindStringExact(i_left);
            boxTarget.SelectedIndex = boxTarget.FindStringExact(i_right);
        }

        private void DropDownOnClick(object sender, EventArgs e) {
            (sender as ComboBox).DroppedDown = true;
        }
        private void DropDownOnKeyPress(object sender, KeyPressEventArgs e) {
            (sender as ComboBox).DroppedDown = true;
        }

    }

}
