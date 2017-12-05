using System;
using System.Collections.Generic;
using System.Drawing;

using IniParser.Model;


namespace CryptoGadget {

    class SettingsWrapper {

        public struct StCoin {
            public static string[] props = {"Coin", "Target", "AlarmUp", "AlarmDown", "GraphPosX", "GraphPosY", "GraphLockPos",
                                            "GraphExitSave", "GraphSizeX", "GraphSizeY", "GraphSizeSave", "GraphStartup"};
            public struct StAlarm {
                public float Up;
                public float Down;
            }
            public struct StGraph {
                public int PosX;
                public int PosY;
                public int SizeX;
                public int SizeY;
                public bool LockPos;
                public bool ExitSave; // saves pos & size
                public bool Startup;
            }
            public string Coin;
            public string Target;
            public StAlarm Alarm;
            public StGraph Graph;
        }
        public struct StBasic {
            public float RefreshRate;
            public bool ShowTooltipConv;
            public bool Startup;
        }
        public struct StVisibility {
            public static string[] props = {"Icon", "Coin", "Value", "ChangeDay", "ChangeDayPct", "Change24", "Change24Pct",
                                            "VolumeDay", "ColumeDay", "TotalVolume24", "OpenDay", "Open24", "HighDay", "High24",
                                            "LowDay", "Low24", "Supply", "MktCap", "Header", "Edge", "Refresh"};
            public bool this[string prop] {
                get { return (bool)this.GetType().GetProperty(prop).GetValue(this, null); }
                set { this.GetType().GetProperty(prop).SetValue(this, value, null); }
            }
            public bool Icon;
            public bool Coin;
            public bool Value;
            public bool ChangeDay;
            public bool ChangeDayPct;
            public bool Change24;
            public bool Change24Pct;
            public bool VolumeDay;
            public bool Volume24;
            public bool TotalVolume24;
            public bool OpenDay;
            public bool Open24;
            public bool HighDay;
            public bool High24;
            public bool LowDay;
            public bool Low24;
            public bool Supply;
            public bool MktCap;
            public bool Header;
            public bool Edge;
            public bool Refresh;
        }
        public struct StColor {
            public static string[] props = {"Coin", "Value", "PositiveChange", "NegativeChange", "Volume", "Open", "High", "Low",
                                            "Supply", "MktCap", "Background1", "Background2", "PositiveRefresh", "NegativeRefresh",
                                            "HeaderText", "HeaderBackground", "Edge"};
            public Color this[string prop] {
                get { return (Color)this.GetType().GetProperty(prop).GetValue(this, null); }
                set { this.GetType().GetProperty(prop).SetValue(this, value, null); }
            }
            public Color Coin;
            public Color Value;
            public Color PositiveChange;
            public Color NegativeChange;
            public Color Volume;
            public Color Open;
            public Color High;
            public Color Low;
            public Color Supply;
            public Color MktCap;
            public Color Background1;
            public Color Background2;
            public Color PositiveRefresh;
            public Color NegativeRefresh;
            public Color HeaderText;
            public Color HeaderBackground;
            public Color Edge;
        }
        public struct StCoords {
            public int PosX;
            public int PosY;
            public bool ExitSave;
            public bool LockPos;
        }
        public struct StDigits {
            public static string[] props = {"Value", "ChangeDay", "ChangeDayPct", "Change24", "Change24Pct", "VolumeDay", "Volume24", "TotalVolume24",
                                            "OpenDay", "Open24", "HighDay", "High24", "LowDay", "Low24", "Supply", "MktCap"};
            public int this[string prop] {
                get { return (int)this.GetType().GetProperty(prop).GetValue(this, null); }
                set { this.GetType().GetProperty(prop).SetValue(this, value, null); }
            }
            public int Value;
            public int ChangeDay;
            public int ChangeDayPct;
            public int Change24;
            public int Change24Pct;
            public int VolumeDay;
            public int Volume24;
            public int TotalVolume24;
            public int OpenDay;
            public int Open24;
            public int HighDay;
            public int High24;
            public int LowDay;
            public int Low24;
            public int Supply;
            public int MktCap;
        }
        public struct StMetrics {
            public static string[] props = {"Icon", "Coin", "Value", "ChangeDay", "ChangeDayPct", "Change24", "Change24Pct",
                                            "VolumeDay", "Volume24", "TotalVolume24", "OpenDay", "Open24", "HighDay", "High24",
                                            "LowDay", "Low24", "Supply", "MktCap", "Header", "HeaderText", "Edge", "Rows",
                                            "IconSize", "Text", "Number"};
            public int this[string prop] {
                get { return (int)this.GetType().GetProperty(prop).GetValue(this, null); }
                set { this.GetType().GetProperty(prop).SetValue(this, value, null); }
            }
            public int Icon;
            public int Coin;
            public int Value;
            public int ChangeDay;
            public int ChangeDayPct;
            public int Change24;
            public int Change24Pct;
            public int VolumeDay;
            public int Volume24;
            public int TotalVolume24;
            public int OpenDay;
            public int Open24;
            public int HighDay;
            public int High24;
            public int LowDay;
            public int Low24;
            public int Supply;
            public int MktCap;
            public int Header;
            public int HeaderText;
            public int Edge;
            public int Rows;
            public int IconSize;
            public int Text; // will need to use int and divide by 100 when using
            public int Numbers;
        }
        public struct StPages {
            public int Size;
            public bool Rotate;
            public float RotateRate;
        }
        
        public enum DefaultType {
            Coins      = 0x0001,
            Basic      = 0x0002,
            Visibility = 0x0004,
            ColorLight = 0x0008,
            ColorDark  = 0x0010,
            Coords     = 0x0020,
            Digits     = 0x0040,
            Metrics    = 0x0080,
            Pages      = 0x0100,
            All        = 0xFFFF 
        }

        public List<List<StCoin>> Coins = new List<List<StCoin>>(10); // Coins[page][coin_name]
        public StBasic Basic            = new StBasic();
        public StVisibility Visibility  = new StVisibility();
        public StColor Color            = new StColor();
        public StCoords Coords          = new StCoords();
        public StDigits Digits          = new StDigits();
        public StMetrics Metrics        = new StMetrics();
        public StPages Pages            = new StPages();

        private string _file_path = "";
        private IniData _ini = new IniData();

        // [Page0]
        // BTC_USD = Page0_BTC_USD

        public bool BindFile(string file_path) {
            try {
                _ini = new IniParser.FileIniDataParser().ReadFile(file_path);
            } catch {
                return false;
            }
            _file_path = file_path;
            return true;
        }
        public bool Load() {

            Func<IniData, bool> MissingAttr = (data) => {
                SettingsWrapper check = new SettingsWrapper();
                check.Default(~DefaultType.Coins);
                check.Store();
                foreach(SectionData sect in check._ini.Sections) {
                    if(!data.Sections.ContainsSection(sect.SectionName))
                        return true;
                    foreach(KeyData key in sect.Keys)
                        if(!sect.Keys.ContainsKey(key.KeyName))
                            return true;
                }
                for(int i = 0; i < 10; i++) {
                    foreach(KeyData coin in _ini["Page" + i.ToString()]) {
                        if(!_ini.Sections.ContainsSection(coin.Value)) 
                            return true;
                        KeyDataCollection coin_sect = _ini[coin.Value];
                        foreach(string prop in StCoin.props)
                            if(!coin_sect.ContainsKey(prop))
                                return true;
                    }
                }
                return false;
            };
            Func<string, Func<T, bool>, T> AssignRule<T>() where T : IConvertible {
                return (data, fn) => {
                    T value = (T)Convert.ChangeType(data, default(T).GetTypeCode());
                    if(!fn(value))
                        throw new Exception();
                    return value;
                };
            };

            if(MissingAttr(_ini)) 
                return false;
            
            try {

                Basic.RefreshRate     = AssignRule<float>()(_ini["Basic"]["RefreshRate"], (x) => x >= 1.0f);
                Basic.ShowTooltipConv = bool.Parse(_ini["Basic"]["ShowTooltipConv"]);
                Basic.Startup         = bool.Parse(_ini["Basic"]["Startup"]);

                Coords.PosX   = int.Parse(_ini["Coords"]["StartX"]);
                Coords.PosY   = int.Parse(_ini["Coords"]["StartY"]);
                Coords.LockPos  = bool.Parse(_ini["Coords"]["LockPos"]);
                Coords.ExitSave = bool.Parse(_ini["Coords"]["ExitSave"]);

                Pages.Size       = AssignRule<int>()(_ini["Pages"]["Size"], (x) => (x >= 1 && x <= 10));
                Pages.Rotate     = bool.Parse(_ini["Pages"]["Rotate"]);
                Pages.RotateRate = AssignRule<int>()(_ini["Pages"]["RotateRate"], (x) => x >= 1);

                foreach(string prop in StVisibility.props)
                    Visibility[prop] = bool.Parse(_ini["Visibility"][prop]);
                foreach(string prop in StColor.props)
                    Color[prop] = Common.StrHexToColor(_ini["Color"][prop]);
                foreach(string prop in StDigits.props)
                    Digits[prop] = AssignRule<int>()(_ini["Digits"][prop], (x) => x >= 0);
                foreach(string prop in StMetrics.props)
                    Metrics[prop] = AssignRule<int>()(_ini["Metrics"][prop], (x) => x >= 0);

                for(int i = 0; i < 10; i++) {
                    Coins.Add(new List<StCoin>());
                    foreach(KeyData coin_sectname in _ini["Page"+ i.ToString()]) {
                        KeyDataCollection coin = _ini[coin_sectname.Value];
                        StCoin st = new StCoin();
                        st.Coin   = coin["Coin"];
                        st.Target = coin["Target"];
                        st.Alarm.Up   = AssignRule<int>()(coin["AlarmUp"], (x) => x >= 0);
                        st.Alarm.Down = AssignRule<int>()(coin["AlarmDown"], (x) => x >= 0);
                        st.Graph.PosX = int.Parse(coin["GraphPosX"]);
                        st.Graph.PosY = int.Parse(coin["GraphPosY"]);
                        st.Graph.SizeX = AssignRule<int>()(coin["GraphSizeX"], (x) => x >= 0);
                        st.Graph.SizeY = AssignRule<int>()(coin["GraphSizeY"], (x) => x >= 0);
                        st.Graph.LockPos  = bool.Parse(coin["GraphLockPos"]);
                        st.Graph.ExitSave = bool.Parse(coin["GraphExitSave"]);
                        st.Graph.Startup  = bool.Parse(coin["GraphStartup"]);
                        Coins[i].Add(st);
                    }
                }

            }
            catch {
                return false;
            }

            return false;
        }
        public void Store() {

            _ini = new IniData();

            _ini["Basic"]["RefreshRate"]     = Basic.RefreshRate.ToString();
            _ini["Basic"]["ShowTooltipConv"] = Basic.ShowTooltipConv.ToString();
            _ini["Basic"]["Startup"]         = Basic.Startup.ToString();

            _ini["Coords"]["StartX"]   = Coords.PosX.ToString();
            _ini["Coords"]["StartY"]   = Coords.PosY.ToString();
            _ini["Coords"]["LockPos"]  = Coords.LockPos.ToString();
            _ini["Coords"]["ExitSave"] = Coords.ExitSave.ToString();

            _ini["Pages"]["Size"]       = Pages.Size.ToString();
            _ini["Pages"]["Rotate"]     = Pages.Rotate.ToString();
            _ini["Pages"]["RotateRate"] = Pages.RotateRate.ToString();

            foreach(string prop in StVisibility.props)
                _ini["Visibility"][prop] = Visibility[prop].ToString();
            foreach(string prop in StColor.props)
                _ini["Color"][prop] = Common.ColorToStrHex(Color[prop]);
            foreach(string prop in StDigits.props)
                _ini["Digits"][prop] = Digits[prop].ToString();
            foreach(string prop in StMetrics.props)
                _ini["Metrics"][prop] = Metrics[prop].ToString();

            for(int i = 0; i < 10; i++) {
                string strp = "Page" + i.ToString();
                _ini.Sections.AddSection(strp);
                KeyDataCollection page = _ini[strp];
                foreach(StCoin st in Coins[i]) {
                    string strconv = st.Coin + "_" + st.Target;
                    page.AddKey(strconv, strp + "_" + strconv);
                    _ini.Sections.AddSection(strp + "_" + strconv);
                    KeyDataCollection coin = _ini[strp + "_" + strconv];
                    coin["Coin"] = st.Coin;
                    coin["Target"] = st.Target;
                    coin["AlarmUp"] = st.Alarm.Up.ToString();
                    coin["AlarmDown"] = st.Alarm.Down.ToString();
                    coin["GraphPosX"] = st.Graph.PosX.ToString();
                    coin["GraphPosY"] = st.Graph.PosY.ToString();
                    coin["GraphSizeX"] = st.Graph.SizeX.ToString();
                    coin["GraphSizeY"] = st.Graph.SizeY.ToString();
                    coin["GraphLockPos"] = st.Graph.LockPos.ToString();
                    coin["GraphExitSave"] = st.Graph.ExitSave.ToString();
                    coin["GraphStartup"] = st.Graph.Startup.ToString();
                }
                
            }
        }
        public bool Save() {
            if(_file_path.Length == 0)
                return false;
            try {
                new IniParser.FileIniDataParser().WriteFile(_file_path, _ini);
            } 
            catch {
                return false;
            }
            return true;
        }
        public void Default(DefaultType type = DefaultType.All) {

            if((type & DefaultType.Coins) != 0) {
                Coins = new List<List<StCoin>>(10);
                StCoin st = new StCoin();
                st.Target = "USD";
                st.Alarm.Up = 0.0f;
                st.Alarm.Down = 0.0f;
                st.Graph.PosX = 100;
                st.Graph.PosY = 100;
                st.Graph.LockPos = false;
                st.Graph.ExitSave = true;
                st.Graph.Startup = false;

                string[] coins = { "BTC", "ETH", "ETC", "LTC", "ZEC", "VTC", "LBC", "DASH", "XMR", "DOGE" };
                foreach(string coin in coins) {
                    st.Coin = coin;
                    Coins[0].Add(st);
                }
            }
            if((type & DefaultType.Basic) != 0) {
                Basic.RefreshRate = 20.0f;
                Basic.ShowTooltipConv = true;
                Basic.Startup = false;
            }
            if((type & DefaultType.Visibility) != 0) {
                foreach(string prop in StVisibility.props)
                    Visibility[prop] = false;
                Visibility.Icon = true;
                Visibility.Coin = true;
                Visibility.Value = true;
                Visibility.Change24 = true;
            }
            if((type & DefaultType.ColorLight) != 0) {
                foreach(string prop in StColor.props)
                    Color[prop] = Common.StrHexToColor("FF000000");
                Color.Background1      = Common.StrHexToColor("FFF3F7F7");
                Color.Background2      = Common.StrHexToColor("FFFFFFFF");
                Color.PositiveRefresh  = Common.StrHexToColor("FFCEEBD3");
                Color.NegativeRefresh  = Common.StrHexToColor("FFF6D4D1");
                Color.Edge             = Common.StrHexToColor("FFAFAFAF");
                Color.PositiveChange   = Common.StrHexToColor("FF27892F");
                Color.NegativeChange   = Common.StrHexToColor("FFCF6563");
                Color.HeaderText       = Common.StrHexToColor("FF000000");
                Color.HeaderBackground = Common.StrHexToColor("FFF0F0F0");
            }
            else if((type & DefaultType.ColorDark) != 0) {
                foreach(string prop in StColor.props)
                    Color[prop] = Common.StrHexToColor("FFDADADA");
                Color.Background1      = Common.StrHexToColor("FF1E1E1E");
                Color.Background2      = Common.StrHexToColor("FF2F2F2F");
                Color.PositiveRefresh  = Common.StrHexToColor("FF3A8F49");
                Color.NegativeRefresh  = Common.StrHexToColor("FF96261D");
                Color.Edge             = Common.StrHexToColor("FF535353");
                Color.PositiveChange   = Common.StrHexToColor("FF27892F");
                Color.NegativeChange   = Common.StrHexToColor("FFCF6563");
                Color.HeaderText       = Common.StrHexToColor("FFC7C7C7");
                Color.HeaderBackground = Common.StrHexToColor("FF2C2C2C");

            }
            if((type & DefaultType.Coords) != 0) {
                Coords.PosX = 50;
                Coords.PosY = 50;
                Coords.LockPos = false;
                Coords.ExitSave = true;
            }
            if((type & DefaultType.Digits) != 0) {
                foreach(string prop in StDigits.props)
                    Digits[prop] = 7;
            }
            if((type & DefaultType.Metrics) != 0) {
                foreach(string prop in StMetrics.props)
                    Metrics[prop] = 60;
                Metrics.Icon = 25;
                Metrics.Coin = 40;
                Metrics.Edge = 2;
                Metrics.Header = 15;
                Metrics.Rows = 22;
                Metrics.IconSize = 16;
                Metrics.Text = 825;
                Metrics.Numbers = 825;
            }
            if((type & DefaultType.Pages) != 0) {
                Pages.Size = 1;
                Pages.Rotate = false;
                Pages.RotateRate = 10.0f;
            }

        }

        public bool ContainsConv(int page, string coin, string target) {
            // TODO
            return false;
        }

    }

}
