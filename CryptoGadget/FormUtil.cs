using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;



namespace neo {

    public class FormUtil {

        #region DragMove Method Details

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        #endregion


        public static void DragMove(object sender, MouseEventArgs e) {
            if(e.Button == MouseButtons.Left) {
                ReleaseCapture();
                SendMessage((sender as Control).FindForm().Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }


        public static void textBoxSignedInt(object sender, KeyPressEventArgs e) {
            if(!Char.IsDigit(e.KeyChar) && !(e.KeyChar == (char)Keys.Back) && !(e.KeyChar == '-' && (sender as TextBox).SelectionStart == 0))
                e.Handled = true;
        }
        public static void textBoxUnsignedInt(object sender, KeyPressEventArgs e) {
            if(!Char.IsDigit(e.KeyChar) && !(e.KeyChar == (char)Keys.Back))
                e.Handled = true;
        }
        public static void textBoxUnsignedFloat(object sender, KeyPressEventArgs e) {
            if(!Char.IsDigit(e.KeyChar) && !(e.KeyChar == (char)Keys.Back) && 
               !((sender as TextBox).SelectionStart != 0 && e.KeyChar == ',' && !(sender as TextBox).Text.Contains(",")))
                e.Handled = true;
        }
        public static void textBoxSignedFloat(object sender, KeyPressEventArgs e) {
            if(!Char.IsDigit(e.KeyChar) && !(e.KeyChar == (char)Keys.Back) &&
               !((sender as TextBox).SelectionStart == 0 ? e.KeyChar == '-' : (e.KeyChar == ',' && !(sender as TextBox).Text.Contains(",") && (sender as TextBox).Text.IndexOfAny("0123456789".ToCharArray()) != -1)))
                e.Handled = true;
        }

        public static void buttonColorPick(object sender, EventArgs e) {
            ColorDialog cd = new ColorDialog();
            cd.Color = (sender as Button).BackColor;
            cd.FullOpen = true;
            cd.ShowDialog();
            (sender as Button).BackColor = cd.Color;
        }

	}

}
	

