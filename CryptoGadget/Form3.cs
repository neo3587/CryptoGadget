
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
        BindingSource binds = new BindingSource();
        BindingSource binds2 = new BindingSource();

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

                binds.DataSource = pairs;
                binds2.DataSource = pairs;
                
                boxCoin.DataSource = binds;
                boxTarget.DataSource = binds2;

                boxCoin.SelectedIndex   = boxCoin.Items.Count == 0 ? -1 : 0;
                boxTarget.SelectedIndex = boxTarget.Items.Count == 0 ? -1 : 0;

                boxCoin.KeyPress += (ksender, ke) => boxCoin.DroppedDown = true;
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
            
            boxTarget.Items.Clear();

            foreach(JProperty coin in Common.json["Data"]) {
                if(checkOnlyFiat.Checked && coin.Value["FiatCurrency"] == null) 
                    continue;
                boxTarget.Items.Add(coin.Value["Name"] + " (" + coin.Value["CoinName"] + ")");
            }
            boxCoin.SelectedIndex = boxCoin.Items.Count == 0 ? -1 : 0;
            boxTarget.SelectedIndex = boxTarget.Items.Count == 0 ? -1 : 0;
        }

        private void checkIndexName_CheckedChanged(object sender, EventArgs e) {
            
            BindList bl = new BindList();
            for(int i = 0; i < pairs.Count; i++)
                pairs[i] = new  KeyValuePair<string, string>(pairs[i].Value, pairs[i].Key);

            binds.ResetBindings(false);
            binds2.ResetBindings(false);
        }
    }

}
