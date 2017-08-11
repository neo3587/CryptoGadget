
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using BindList = System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>>;


namespace CryptoGadget {

    

    public partial class AddCoinForm : Form {

        DataGridView ptrGrid;
        BindList pairs = new BindList();
        BindingSource cBind = new BindingSource();
        BindingSource tBind = new BindingSource();

        private static void Swap<T>(ref T a, ref T b) {
            T c = a;
            a = b;
            b = c;
        }

        public AddCoinForm(DataGridView grid) {

            InitializeComponent();
            ptrGrid = grid;

            HandleCreated += (sender, e) => {

                if(Common.json == null)
                    Close();

                foreach(JProperty coin in Common.json["Data"])
                    pairs.Add(new KeyValuePair<string, string>(coin.Value["Name"].ToString(), coin.Value["CoinName"].ToString()));

                cBind.DataSource = pairs;
                tBind.DataSource = pairs;
                
                boxCoin.DataSource   = cBind;
                boxTarget.DataSource = tBind;

                boxCoin.SelectedIndex   = boxCoin.Items.Count == 0 ? -1 : 0;
                boxTarget.SelectedIndex = boxTarget.Items.Count == 0 ? -1 : 0;

                boxCoin.KeyPress   += (ksender, ke) => boxCoin.DroppedDown = true;
                boxTarget.KeyPress += (ksender, ke) => boxTarget.DroppedDown = true;
            };

        }

        private void buttonAdd_Click(object sender, EventArgs e) {

            Func<string, bool> FindCoin = (l_coin) => {
                foreach(DataGridViewRow row in ptrGrid.Rows)
                    if(row.Cells[1].Value.ToString() == l_coin)
                        return true;
                return false;
            };

            if(boxCoin.Items.Count <= 0 || boxCoin.SelectedIndex == -1)
               return;

            string coin   = ((KeyValuePair<string, string>)boxCoin.SelectedItem).Key.ToString();
            string name   = ((KeyValuePair<string, string>)boxCoin.SelectedItem).Value.ToString();
            string t_coin = ((KeyValuePair<string, string>)boxTarget.SelectedItem).Key.ToString();
            string t_name = ((KeyValuePair<string, string>)boxTarget.SelectedItem).Value.ToString();

            if(checkIndexName.Checked) {
                Swap(ref coin, ref name);
                Swap(ref t_coin, ref t_name);
            }

            if(FindCoin(coin)) {
                MessageBox.Show(boxCoin.SelectedItem.ToString() +  " is already being used");
                return;
            }

            int insertPos = ptrGrid.SelectedRows.Count > 0 ? ptrGrid.SelectedRows[0].Index +1 : 0;
            ptrGrid.Rows.Insert(insertPos, Common.GetIcon(coin, new Size(16, 16)), coin, name, t_coin, t_name);
            ptrGrid.Rows[Math.Min(ptrGrid.SelectedRows[0].Index +1, ptrGrid.RowCount-1)].Selected = true;
        }

        private void buttonDone_Click(object sender, EventArgs e) {
            Close();
        }

        private void DropDownOnClick(object sender, EventArgs e) {
            (sender as ComboBox).DroppedDown = true;
        }

        private void checkOnlyFiat_CheckedChanged(object sender, EventArgs e) {

            if(checkOnlyFiat.Checked) {

                BindList bl = new BindList();
                string left = !checkIndexName.Checked ? "Name" : "CoinName";
                string right = checkIndexName.Checked ? "Name" : "CoinName";

                foreach(JProperty coin in Common.json["Data"]) 
                    if(coin.Value["FiatCurrency"] != null)
                        bl.Add(new KeyValuePair<string, string>(coin.Value[left].ToString(), coin.Value[right].ToString()));
                tBind.DataSource = bl;
            }
            else {
                tBind.DataSource = pairs;
            }

            tBind.ResetBindings(false);
            boxTarget.SelectedIndex = boxTarget.Items.Count == 0 ? -1 : 0;
        }

        private void checkIndexName_CheckedChanged(object sender, EventArgs e) {

            for(int i = 0; i < pairs.Count; i++)
                pairs[i] = new KeyValuePair<string, string>(pairs[i].Value, pairs[i].Key);

            if(checkOnlyFiat.Checked) {
                BindList ptr = (tBind.DataSource as BindList);
                for(int i = 0; i < ptr.Count; i++)
                    ptr[i] = new KeyValuePair<string, string>(ptr[i].Value, ptr[i].Key);
            }

            cBind.ResetBindings(false);
            tBind.ResetBindings(false);

            boxCoin.SelectedIndex   = boxCoin.Items.Count == 0 ? -1 : 0;
            boxTarget.SelectedIndex = boxTarget.Items.Count == 0 ? -1 : 0;
        }

    }

}
