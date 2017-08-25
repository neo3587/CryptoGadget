
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.IO;

using Newtonsoft.Json.Linq;
using IniParser.Model;



namespace CryptoGadget {

    public struct Data {

        public static List<Tuple<string, string>> converts = new List<Tuple<string, string>>();

        public struct others {

            public static int refreshRate = 20000;
            public static bool openStartup = false;

            public static int maxValueDigits     = 7;
            public static int maxValueDecimals   = 6;
            public static int maxChangeDigits    = 5;
            public static int maxChangeDecimals  = 4;
            public static int maxPercentDigits   = 5;
            public static int maxPercentDecimals = 2;

            public static bool showTooltipName = true;
        }

        public struct visible {

            public static bool icon    = true;
            public static bool coin    = true;
            public static bool value   = true;
            public static bool change  = true;
            public static bool percent = false;
            public static bool header  = true;
            public static bool edge    = true;
            public static bool refresh = true;

        }

        public struct metrics {

            public static int icon     = 25;
            public static int coin     = 40;
            public static int value    = 60;
            public static int change   = 55;
            public static int percent  = 55;
            public static int edge     = 2;
            public static int header   = 15;
            public static int rows     = 22;
            public static int iconSize = 16;
            public static float text    = 8.25f;
            public static float numbers = 8.25f;

        }

        public struct coords {

            public static int startX = 50;
            public static int startY = 50;
            public static bool exitSave = true;
            public static bool lockPosition = false;

        }

        public struct colors {

            public static Color coins            = Common.StrHexToColor("FF000000");
            public static Color values           = Common.StrHexToColor("FF000000");
            public static Color background1      = Common.StrHexToColor("FFF3F7F7");
            public static Color background2      = Common.StrHexToColor("FFFFFFFF");
            public static Color positiveRefresh  = Common.StrHexToColor("FFCEEBD3");
            public static Color negativeRefresh  = Common.StrHexToColor("FFF6D4D1");
            public static Color edge             = Common.StrHexToColor("FFAFAFAF");
            public static Color positiveChange   = Common.StrHexToColor("FF27892F");
            public static Color negativeChange   = Common.StrHexToColor("FFCF6563");
            public static Color headerText       = Common.StrHexToColor("FF000000");
            public static Color headerBackGround = Common.StrHexToColor("FFF0F0F0");

        }

    }

    class Common {

        public static string iniLocation  = Application.StartupPath + "\\settings.ini";
        public static string iconLocation = Application.StartupPath + "\\ico\\";
        public static string jsonLocation = Application.StartupPath + "\\CoinList.json";

        public static IniData ini = new IniData();
        public static JObject json = null;
        
        public enum DefaultType {
            Coins = 0x01,
            Basic = 0x02,
            Advanced = 0x04,
            ColorsLight = 0x08,
            ColorsDark = 0x10,
            All = Coins | Basic | Advanced | ColorsLight
        }


        public static IniData DefaultIni(IniData data = null, DefaultType dt = DefaultType.All) {

            data = data ?? new IniData();

            if((dt & DefaultType.Coins) != 0) {
                data["Coins"]["BTC"] = "USD";
                data["Coins"]["ETH"] = "USD";
                data["Coins"]["ETC"] = "USD";
                data["Coins"]["LTC"] = "USD";
                data["Coins"]["ZEC"] = "USD";
                data["Coins"]["XRP"] = "USD";
                data["Coins"]["DCR"] = "USD";
                data["Coins"]["XMR"] = "USD";
            }

            if((dt & DefaultType.Basic) != 0) {
                data["Visibility"]["Icon"]    = "True";
                data["Visibility"]["Coin"]    = "True";
                data["Visibility"]["Value"]   = "True";
                data["Visibility"]["Change"]  = "True";
                data["Visibility"]["Percent"] = "False";
                data["Visibility"]["Header"]  = "True";
                data["Visibility"]["Edge"]    = "True";
                data["Visibility"]["Refresh"] = "True";

                data["Others"]["RefreshRate"] = "20";
                data["Others"]["OpenStartup"] = "False";
                data["Others"]["ShowTooltipName"] = "True";
            }

            if((dt & DefaultType.Advanced) != 0) {
                data["Metrics"]["Icon"]     = "25";
                data["Metrics"]["Coin"]     = "40";
                data["Metrics"]["Value"]    = "60";
                data["Metrics"]["Change"]   = "55";
                data["Metrics"]["Percent"]  = "60";
                data["Metrics"]["Edge"]     = "2";
                data["Metrics"]["Header"]   = "15";
                data["Metrics"]["Rows"]     = "22";
                data["Metrics"]["IconSize"] = "16";
                data["Metrics"]["Text"]     = "8,25";
                data["Metrics"]["Numbers"]  = "8,25";

                data["Coordinates"]["StartX"]       = "50";
                data["Coordinates"]["StartY"]       = "50";
                data["Coordinates"]["ExitSave"]     = "True";
                data["Coordinates"]["LockPosition"] = "False";

                data["Others"]["MaxValueDigits"]     = "7";
                data["Others"]["MaxValueDecimals"]   = "6";
                data["Others"]["MaxChangeDigits"]    = "5";
                data["Others"]["MaxChangeDecimals"]  = "4";
                data["Others"]["MaxPercentDigits"]   = "5";
                data["Others"]["MaxPercentDecimals"] = "2";
            }

            if((dt & DefaultType.ColorsLight) != 0) {
                data["Colors"]["Coins"]            = "FF000000";
                data["Colors"]["Values"]           = "FF000000";
                data["Colors"]["BackGround1"]      = "FFF3F7F7";
                data["Colors"]["BackGround2"]      = "FFFFFFFF";
                data["Colors"]["PositiveRefresh"]  = "FFCEEBD3";
                data["Colors"]["NegativeRefresh"]  = "FFF6D4D1";
                data["Colors"]["Edge"]             = "FFAFAFAF";
                data["Colors"]["PositiveChange"]   = "FF27892F";
                data["Colors"]["NegativeChange"]   = "FFCF6563";
                data["Colors"]["HeaderText"]       = "FF000000";
                data["Colors"]["HeaderBackGround"] = "FFF0F0F0";
            }

            if((dt & DefaultType.ColorsDark) != 0) {
                data["Colors"]["Coins"]            = "FFDADADA";
                data["Colors"]["Values"]           = "FFDADADA";
                data["Colors"]["BackGround1"]      = "FF1E1E1E";
                data["Colors"]["BackGround2"]      = "FF2F2F2F";
                data["Colors"]["PositiveRefresh"]  = "FF3A8F49";
                data["Colors"]["NegativeRefresh"]  = "FF96261D";
                data["Colors"]["Edge"]             = "FF535353";
                data["Colors"]["PositiveChange"]   = "FF27892F";
                data["Colors"]["NegativeChange"]   = "FFCF6563";
                data["Colors"]["HeaderText"]       = "FFC7C7C7";
                data["Colors"]["HeaderBackGround"] = "FF2C2C2C";
            }

            return data;
        }

        public static Color StrHexToColor(string str) {
            return Color.FromArgb(int.Parse(str, System.Globalization.NumberStyles.HexNumber));
        }
        public static string ColorToStrHex(Color col) {
            return col.ToArgb().ToString("X8");
        }

        public static JObject HttpRequest(string input_coin, string output_coin) {
            HttpWebRequest HttpReq = (HttpWebRequest)WebRequest.Create("https://min-api.cryptocompare.com/data/price?fsym=" + input_coin.ToUpper() + "&tsyms=" + output_coin.ToUpper());
            return JObject.Parse(new StreamReader(((HttpWebResponse)HttpReq.GetResponse()).GetResponseStream()).ReadToEnd());
        }
        public static JObject HttpRequest(string[] input_coin, string[] output_coin, bool full = true) {
            HttpWebRequest HttpReq = (HttpWebRequest)WebRequest.Create("https://min-api.cryptocompare.com/data/pricemulti" + (full ? "full" : "") + "?fsyms=" + string.Join(",", input_coin).ToUpper() + "&tsyms=" + string.Join(",", output_coin).ToUpper());
            return JObject.Parse(new StreamReader(((HttpWebResponse)HttpReq.GetResponse()).GetResponseStream()).ReadToEnd());
        }

        public static Bitmap GetIcon(string name, Size size = new Size()) {
            Bitmap bmp;
            name = name.ToLower();
            try {
                try {
                    bmp = (size.IsEmpty ? new Icon(iconLocation + name + ".ico") : new Icon(iconLocation + name + ".ico", size)).ToBitmap(); // it looks slightly better if you can load it as a icon
                }
                catch(Exception) {
                    bmp = size.IsEmpty ? new Bitmap(iconLocation + name + ".ico") : new Bitmap(Image.FromFile(iconLocation + name + ".ico"), size);
                }
            }
            catch(Exception) {
                bmp = new Bitmap(1,1);
            }
            return bmp;
        }
        public static Bitmap GetIcon(Stream stream, Size size = new Size()) {
            Bitmap bmp;
            try {
                try {
                    bmp = (size.IsEmpty ? new Icon(stream) : new Icon(stream, size)).ToBitmap(); // it looks slightly better if you can load it as a icon
                } catch(Exception) {
                    bmp = size.IsEmpty ? new Bitmap(stream) : new Bitmap(Image.FromStream(stream), size);
                }
            } catch(Exception) {
                bmp = new Bitmap(0, 0);
            }
            return bmp;
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

    }

}
