
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

using Newtonsoft.Json.Linq;



namespace CryptoGadget {

    public class Global : PropManager<Global> {

		#region DragMove Method Details
		[DllImport("user32.dll")]
		private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		[DllImport("user32.dll")]
		private static extern bool ReleaseCapture();
		#endregion

		public static readonly string ProfileIniLocation = Application.StartupPath + "\\profile_default.ini";
        public static readonly string CoinListLocation	 = Application.StartupPath + "\\CoinList.json";
		public static readonly string IconsFolder		 = Application.StartupPath + "\\ico\\";
		public static readonly string ProfilesFolder	 = Application.StartupPath + "\\profiles\\";

		public static Settings Sett = new Settings();
		public static JObject Json	= null;

		public static string Profile = "Default.json";


		public static void DragMove(object sender, MouseEventArgs e) {
			if(e.Button == MouseButtons.Left) {
				ReleaseCapture();
				SendMessage((sender as Control).FindForm().Handle, 0xA1, 0x02, 0);
			}
		}

		public static Bitmap GetIcon(string name, int size = 0) {
			Bitmap bmp;
			name = name.ToLower().Replace("*", "_star");
			try {
                try {
                    bmp = (size == 0 ? new Icon(IconsFolder + name + ".ico") : new Icon(IconsFolder + name + ".ico", new Size(size, size))).ToBitmap(); // it looks slightly better if you can load it as a icon
                }
                catch {
                    bmp = size == 0 ? new Bitmap(IconsFolder + name + ".ico") : new Bitmap(Image.FromFile(IconsFolder + name + ".ico"), new Size(size, size));
                }
            }
            catch {
                bmp = new Bitmap(1,1);
            }
			return size > 0 ? IconResize(bmp, size) : bmp;
		}
        public static Bitmap GetIcon(Stream stream, int size = 0) {
            Bitmap bmp;
            try {
                try {
                    bmp = (size == 0 ? new Icon(stream) : new Icon(stream, new Size(size, size))).ToBitmap(); // it looks slightly better if you can load it as a icon
                } catch(Exception) {
                    bmp = size == 0 ? new Bitmap(stream) : new Bitmap(Image.FromStream(stream), new Size(size, size));
                }
            } catch(Exception) {
                bmp = new Bitmap(1,1);
            }
			return size > 0 ? IconResize(bmp, size) : bmp;			
		}

		public static void SetIcon(string name, Bitmap bmp) {
			name = name.ToLower().Replace("*", "_star");
			bmp.Save(IconsFolder + name + ".ico", System.Drawing.Imaging.ImageFormat.Icon);
		}
		public static void SetIcon(string name, Stream stream) {
			name = name.ToLower().Replace("*", "_star");
			try {
				using(StreamWriter writer = new StreamWriter(IconsFolder + name + ".ico")) {
					stream.CopyTo(writer.BaseStream);
				}
			} catch { }
		}

		public static bool JsonIsValid(JObject js) {
            System.Threading.Tasks.ParallelLoopResult result = System.Threading.Tasks.Parallel.ForEach(js["Data"], (coin, state) => {
                JToken val = (coin as JProperty).Value;
                if(val["Name"] == null || val["CoinName"] == null || val["FullName"] == null) {
                    state.Break();
                    return;
                }
            });
            return result.IsCompleted && !result.LowestBreakIteration.HasValue;
        }

        public static Bitmap IconResize(Image img, int size) {
            Bitmap bmp = new Bitmap(size, size);
            using(Graphics gr = Graphics.FromImage(bmp)) {
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                gr.DrawImage(img, new Rectangle(0, 0, size, size));
            }
            return bmp;
        }

		public static void DbgPrint<T>(T text) {
			#if DEBUG
			Console.WriteLine(text.ToString());
			#endif
		}
		public static void DbgMsgShow<T>(T text) {
			#if DEBUG
			MessageBox.Show(text.ToString());
			#endif
		}

	};

}
