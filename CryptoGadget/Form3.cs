
using System;
using System.Drawing;
using System.Windows.Forms;

using Newtonsoft.Json.Linq;





namespace CryptoGadget {

    public partial class AddCoinForm : Form {

        DataGridView ptrGrid;

        public AddCoinForm(DataGridView grid) {

            InitializeComponent();

            ptrGrid = grid;

            HandleCreated += (sender, e) => {

                if(Common.json != null) {
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    foreach(JProperty coin in Common.json["Data"]) {
                        boxCoin.Items.Add(coin.Value["Name"] + " (" + coin.Value["CoinName"] + ")");
                        boxTarget.Items.Add(coin.Value["Name"] + " (" + coin.Value["CoinName"] + ")");
                    }

                    boxCoin.SelectedIndex = boxCoin.Items.Count == 0 ? -1 : 0;
                    boxTarget.SelectedIndex = boxTarget.Items.Count == 0 ? -1 : 0;
                }
            };

        }

        private void buttonAdd_Click(object sender, EventArgs e) {

            Func<string, string, bool> FindCoin = (l_coin, r_coin) => {
                foreach(DataGridViewRow row in ptrGrid.Rows)
                    if(row.Cells[1].Value.ToString() == l_coin && row.Cells[3].Value.ToString() == r_coin)
                        return true;
                return false;
            };

            if(boxCoin.Items.Count <= 0 || boxCoin.SelectedIndex == -1)
               return;

            string coin   = boxCoin.SelectedItem.ToString().Substring(0, boxCoin.SelectedItem.ToString().LastIndexOf('(')).Trim(' ');
            string name   = boxCoin.SelectedItem.ToString().Substring(boxCoin.SelectedItem.ToString().LastIndexOf('(')).Trim(' ', ')', '(');
            string t_coin = boxCoin.SelectedItem.ToString().Substring(0, boxTarget.SelectedItem.ToString().LastIndexOf('(')).Trim(' ');
            string t_name = boxCoin.SelectedItem.ToString().Substring(boxTarget.SelectedItem.ToString().LastIndexOf('(')).Trim(' ', ')', '(');

            if(FindCoin(coin, t_coin)) {
                MessageBox.Show(boxCoin.SelectedItem.ToString() + " -> " + boxTarget.SelectedItem.ToString() + " conversion is already being used");
                return;
            }

            int insertPos = ptrGrid.SelectedRows.Count > 0 ? ptrGrid.SelectedRows[0].Index +1 : 0;
            try {
                ptrGrid.Rows.Insert(insertPos, new Icon(Common.iconLocation + coin + ".ico", new Size(16, 16)).ToBitmap(), coin, name, t_coin, t_name);
            } catch(Exception) {
                ptrGrid.Rows.Insert(insertPos, new Bitmap(1, 1), coin, name, t_coin, t_name);
            }

        }

        private void buttonDone_Click(object sender, EventArgs e) {
            Close();
        }

        private void boxCoins_Click(object sender, EventArgs e) {
            boxCoin.DroppedDown = true;
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
