
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

            Func<string, string, bool> FindCoin = (strcoin, strname) => {
                foreach(DataGridViewRow row in ptrGrid.Rows)
                    if(row.Cells[1].Value.ToString() == strcoin && row.Cells[2].Value.ToString() == strname)
                        return true;
                return false;
            };

            if(boxCoin.Items.Count <= 0 || boxCoin.SelectedIndex == -1)
               return;

            string str = (string)boxCoin.SelectedItem;
            string coin = str.Substring(0, str.LastIndexOf('(')).Trim(' ');
            string name = str.Substring(str.LastIndexOf('(')).Trim(' ', ')', '(');

            if(FindCoin(coin, name)) {
                MessageBox.Show(str + " is already being used");
                return;
            }

            int insertPos = ptrGrid.SelectedRows.Count > 0 ? ptrGrid.SelectedRows[0].Index +1 : 0;
            try {
                ptrGrid.Rows.Insert(insertPos, new Icon(Common.iconLocation + coin + ".ico", new Size(16, 16)).ToBitmap(), coin, name);
            } catch(Exception) {
                ptrGrid.Rows.Insert(insertPos, new Bitmap(1, 1), coin, name);
            }

        }

        private void buttonDone_Click(object sender, EventArgs e) {
            Close();
        }

        private void boxCoins_Click(object sender, EventArgs e) {
            boxCoin.DroppedDown = true;
        }

    }

}
