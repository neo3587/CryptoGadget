

using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.IO;

using Newtonsoft.Json.Linq;
using IniParser.Model;



namespace CryptoGadget {

    class Common {

        public static string iniLocation  = Application.StartupPath + "\\settings.ini";
        public static string iconLocation = Application.StartupPath + "\\ico\\";
        public static string jsonLocation = Application.StartupPath + "\\CoinList.json";

        public static IniData ini = new IniData();

        public enum DefaultType {
            Basic = 0x01,
            Advanced = 0x02,
            ColorsLight = 0x04,
            ColorsDark = 0x08,
            All = Basic | Advanced | ColorsLight
        }


        public static IniData DefaultIni(IniData data = null, DefaultType dt = DefaultType.All) {

            data = data ?? new IniData();

            if((dt & DefaultType.Basic) != 0) {
                data["Coins"]["Bitcoin"]  = "BTC";
                data["Coins"]["Ethereum"] = "ETH";
                data["Coins"]["Ethereum Classic"] = "ETC";
                data["Coins"]["Litecoin"] = "LTC";
                data["Coins"]["Zcash"]    = "ZEC";
                data["Coins"]["Ripple"]   = "XRP";
                data["Coins"]["Decred"]   = "DCR";
                data["Coins"]["Monero"]   = "XMR";

                data["Visibility"]["Icon"]    = "True";
                data["Visibility"]["Coin"]    = "True";
                data["Visibility"]["Value"]   = "True";
                data["Visibility"]["Change"]  = "True";
                data["Visibility"]["Header"]  = "True";
                data["Visibility"]["Edge"]    = "True";
                data["Visibility"]["Refresh"] = "True";

                data["Others"]["RefreshRate"] = "10";
                data["Others"]["TargetCoin"]  = "USD";
                data["Others"]["OpenStartup"] = "False";
            }

            if((dt & DefaultType.Advanced) != 0) {
                data["Metrics"]["Icon"]     = "25";
                data["Metrics"]["Coin"]     = "40";
                data["Metrics"]["Value"]    = "60";
                data["Metrics"]["Change"]   = "55";
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

                data["Others"]["MaxValueDigits"]    = "7";
                data["Others"]["MaxValueDecimals"]  = "6";
                data["Others"]["MaxChangeDigits"]   = "5";
                data["Others"]["MaxChangeDecimals"] = "4";
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

        public static JObject HttpRequest(string input, string output) {
            HttpWebRequest HttpReq = (HttpWebRequest)WebRequest.Create("https://api.cryptonator.com/api/ticker/" + input.ToLower() + "-" + output.ToLower());
            return JObject.Parse(new StreamReader(((HttpWebResponse)HttpReq.GetResponse()).GetResponseStream()).ReadToEnd());
        }

    }

}
