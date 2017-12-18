
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

using Newtonsoft.Json.Linq;



namespace CryptoGadget {

    class Global {

        public static string IniLocation	  = Application.StartupPath + "\\settings.ini";
        public static string IconFolder		  = Application.StartupPath + "\\ico\\";
        public static string JsonLocation	  = Application.StartupPath + "\\CoinList.json";
		public static string ProfilesLocation = Application.StartupPath + "\\profiles\\";

		public static Settings Sett = new Settings();
		public static JObject Json = null;
        
        public static Color StrHexToColor(string str) {
            return Color.FromArgb(int.Parse(str, System.Globalization.NumberStyles.HexNumber));
        }
        public static string ColorToStrHex(Color col) {
            return col.ToArgb().ToString("X8");
        }

        public static Bitmap GetIcon(string name, int size = 0) {
            Bitmap bmp;
            name = name.ToLower();
            try {
                try {
                    bmp = (size == 0? new Icon(IconFolder + name + ".ico") : new Icon(IconFolder + name + ".ico", new Size(size, size))).ToBitmap(); // it looks slightly better if you can load it as a icon
                }
                catch {
                    bmp = size == 0 ? new Bitmap(IconFolder + name + ".ico") : new Bitmap(Image.FromFile(IconFolder + name + ".ico"), new Size(size, size));
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

	}

}
