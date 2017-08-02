
using System;
using System.Drawing;
using System.Windows.Forms;






namespace CryptoGadget {

    public partial class AddCoinForm : Form {

        DataGridView ptrGrid;
        ComboBox ptrBox;

        public AddCoinForm(DataGridView grid) {

            InitializeComponent();

            ptrGrid = grid;

            HandleCreated += (sender, e) => {
                foreach(System.Object coin in ptrBox.Items)
                    boxCoins.Items.Add(coin);
                boxCoins.SelectedIndex = boxCoins.Items.Count == 0 ? -1 : 0;
                
            };

        }

        private void buttonAdd_Click(object sender, EventArgs e) {

            Func<string, string, bool> FindCoin = (strcoin, strname) => {
                foreach(DataGridViewRow row in ptrGrid.Rows)
                    if(row.Cells[1].Value.ToString() == strcoin && row.Cells[2].Value.ToString() == strname)
                        return true;
                return false;
            };

            if(ptrGrid.RowCount >= 10 && !Common.advertise10) {
                Common.advertise10 = true;
                if(MessageBox.Show("The server only allows up to 10 coin conversions requests at a time, each coin added above 10 will add an average time of 250ms to perform all the requests", "Warning", MessageBoxButtons.OKCancel) == DialogResult.Cancel) {
                    Close();
                    return;
                }
            }

            if(boxCoins.Items.Count <= 0 || boxCoins.SelectedIndex == -1)
               return;

            string str = (string)boxCoins.SelectedItem;
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
            boxCoins.DroppedDown = true;
        }

    }

}
