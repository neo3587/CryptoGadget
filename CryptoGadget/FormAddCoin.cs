
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using BindList = System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>>;


namespace CryptoGadget {

    public partial class FormAddCoin : Form {

        DataGridView ptrGrid;
        BindList pairs = new BindList();
        BindingSource cBind = new BindingSource();
        BindingSource tBind = new BindingSource();

        public FormAddCoin(DataGridView grid) {

            InitializeComponent();
            ptrGrid = grid;

            HandleCreated += (sender, e) => {

                foreach(JProperty coin in Global.Json["Data"])
                    pairs.Add(new KeyValuePair<string, string>(coin.Value["Name"].ToString(), coin.Value["CoinName"].ToString()));

                cBind.DataSource = pairs;
                tBind.DataSource = pairs;
                
                boxCoin.DataSource   = cBind;
                boxTarget.DataSource = tBind;

                boxCoin.SelectedIndex   = 0;
                boxTarget.SelectedIndex = Math.Max(boxTarget.FindStringExact("[USD, United States Dollar]"), 0);

            };

        }

        private void buttonAdd_Click(object sender, EventArgs e) {

            Func<string, bool> FindCoin = (l_coin) => {
                foreach(DataGridViewRow row in ptrGrid.Rows)
                    if(row.Cells[1].Value.ToString() == l_coin)
                        return true;
                return false;
            };

            KeyValuePair<string, string> left = (KeyValuePair<string, string>)boxCoin.SelectedItem;
            KeyValuePair<string, string> right = (KeyValuePair<string, string>)boxTarget.SelectedItem;

            if(checkIndexName.Checked) {
                left = new KeyValuePair<string, string>(left.Value, left.Key);
                right = new KeyValuePair<string, string>(right.Value, right.Key);
            }

            if(FindCoin(left.Key)) {
                MessageBox.Show(boxCoin.SelectedItem.ToString() +  " is already being used");
                return;
            }

            int insertPos = ptrGrid.SelectedRows.Count > 0 ? ptrGrid.SelectedRows[0].Index +1 : 0;
            ptrGrid.Rows.Insert(insertPos, Global.GetIcon(left.Key, new Size(16, 16)), left.Key, left.Value, right.Key, right.Value);
            ptrGrid.Rows[Math.Min(ptrGrid.SelectedRows[0].Index +1, ptrGrid.RowCount-1)].Selected = true;
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
                tBind.DataSource = bl;
            }
            else {
                tBind.DataSource = pairs;
            }

            tBind.ResetBindings(false);
            boxTarget.SelectedIndex = 0;
        }
        private void checkIndexName_CheckedChanged(object sender, EventArgs e) {

            string i_left =  "[" + ((KeyValuePair<string, string>)boxCoin.SelectedItem).Value   + ", " + ((KeyValuePair<string, string>)boxCoin.SelectedItem).Key   + "]";
            string i_right = "[" + ((KeyValuePair<string, string>)boxTarget.SelectedItem).Value + ", " + ((KeyValuePair<string, string>)boxTarget.SelectedItem).Key + "]";

            for(int i = 0; i < pairs.Count; i++)
                pairs[i] = new KeyValuePair<string, string>(pairs[i].Value, pairs[i].Key);

            if(checkOnlyFiat.Checked) {
                BindList ptr = (tBind.DataSource as BindList);
                for(int i = 0; i < ptr.Count; i++)
                    ptr[i] = new KeyValuePair<string, string>(ptr[i].Value, ptr[i].Key);
            }

            cBind.ResetBindings(false);
            tBind.ResetBindings(false);

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
