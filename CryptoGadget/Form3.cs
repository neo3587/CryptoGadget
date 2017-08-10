
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections.Concurrent;

using Newtonsoft.Json.Linq;


namespace CryptoGadget {

    public partial class AddCoinForm : Form {

        DataGridView ptrGrid;
        ConcurrentDictionary<string, string> dict = new ConcurrentDictionary<string, string>();

        public AddCoinForm(DataGridView grid) {

            InitializeComponent();
            ptrGrid = grid;

            HandleCreated += (sender, e) => {

                if(Common.json != null) {

                    System.Threading.Tasks.Parallel.ForEach(Common.json["Data"], coin => 
                        dict.TryAdd((coin as JProperty).Value["Name"].ToString(), (coin as JProperty).Value["CoinName"].ToString())
                    );

                    boxCoin.DataSource = new BindingSource(dict, null);
                    boxTarget.DataSource = new BindingSource(dict, null);

                    boxCoin.SelectedIndex = boxCoin.Items.Count == 0 ? -1 : 0;
                    boxTarget.SelectedIndex = boxTarget.Items.Count == 0 ? -1 : 0;
                 
                }

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

    }

}
