using System;
using System.Collections.Generic;
using System.Reflection;
using System.Drawing;

using IniParser.Model;


namespace CryptoGadget {

	class Settings {

		public class StCoin {
			public static string[] props = {"Coin", "Target", "AlarmUp", "AlarmDown", "GraphPosX", "GraphPosY", "GraphSizeX", "GraphSizeY",
											"GraphLockPos", "GraphExitSave", "GraphRefreshRate", "GraphStartup"};
			public struct StAlarm {
				public float Up { get; set; }
				public float Down { get; set; }
			}
			public struct StGraph {
				public int PosX { get; set; }
				public int PosY { get; set; }
				public int SizeX { get; set; }
				public int SizeY { get; set; }
				public bool LockPos { get; set; }
				public bool ExitSave { get; set; } // saves pos & size
				public int RefreshRate { get; set; }
				public bool Startup { get; set; }
			}
			public string Coin { get; set; }
			public string Target { get; set; }
			public StAlarm Alarm;// = new StAlarm();
			public StGraph Graph;// = new StGraph();
		}
		public class StBasic {
			public int RefreshRate { get; set; }
			public bool Startup { get; set; }
		}
		public class StVisibility {
			public static string[] props = {"Icon", "Coin", "Value", "ChangeDay", "ChangeDayPct", "Change24", "Change24Pct",
											"VolumeDay", "VolumeDay", "TotalVolume24", "OpenDay", "Open24", "HighDay", "High24",
											"LowDay", "Low24", "Supply", "MktCap", "Header", "Edge", "Refresh"};
			public object this[string prop] {
				get { return GetType().GetProperty(prop).GetValue(this, null); }
				set { GetType().GetProperty(prop).SetValue(this, value, null); }
			}
			public bool Icon { get; set; }
			public bool Coin { get; set; }
			public bool Value { get; set; }
			public bool ChangeDay { get; set; }
			public bool ChangeDayPct { get; set; }
			public bool Change24 { get; set; }
			public bool Change24Pct { get; set; }
			public bool VolumeDay { get; set; }
			public bool Volume24 { get; set; }
			public bool TotalVolume24 { get; set; }
			public bool OpenDay { get; set; }
			public bool Open24 { get; set; }
			public bool HighDay { get; set; }
			public bool High24 { get; set; }
			public bool LowDay { get; set; }
			public bool Low24 { get; set; }
			public bool Supply { get; set; }
			public bool MktCap { get; set; }
			public bool Header { get; set; }
			public bool Edge { get; set; }
			public bool Refresh { get; set; }
		}
		public class StColor {
			public static string[] props = {"Coin", "Value", "PositiveChange", "NegativeChange", "Volume", "Open", "High", "Low",
											"Supply", "MktCap", "Background1", "Background2", "PositiveRefresh", "NegativeRefresh",
											"HeaderText", "HeaderBackground", "Edge"};
			public object this[string prop] {
				get { return GetType().GetProperty(prop).GetValue(this, null); }
				set { GetType().GetProperty(prop).SetValue(this, value, null); }
			}
			public Color Coin { get; set; }
			public Color Value { get; set; }
			public Color PositiveChange { get; set; }
			public Color NegativeChange { get; set; }
			public Color Volume { get; set; }
			public Color Open { get; set; }
			public Color High { get; set; }
			public Color Low { get; set; }
			public Color Supply { get; set; }
			public Color MktCap { get; set; }
			public Color Background1 { get; set; }
			public Color Background2 { get; set; }
			public Color PositiveRefresh { get; set; }
			public Color NegativeRefresh { get; set; }
			public Color HeaderText { get; set; }
			public Color HeaderBackground { get; set; }
			public Color Edge { get; set; }
		}
		public class StCoords {
			public int PosX { get; set; }
			public int PosY { get; set; }
			public bool ExitSave { get; set; }
			public bool LockPos { get; set; }
		}
		public class StDigits {
			public static string[] props = {"Value", "ChangeDay", "ChangeDayPct", "Change24", "Change24Pct", "VolumeDay", "Volume24", "TotalVolume24",
											"OpenDay", "Open24", "HighDay", "High24", "LowDay", "Low24", "Supply", "MktCap"};
			public object this[string prop] {
				get { return GetType().GetProperty(prop).GetValue(this, null); }
				set { GetType().GetProperty(prop).SetValue(this, value, null); }
			}
			public int Value { get; set; }
			public int ChangeDay { get; set; }
			public int ChangeDayPct { get; set; }
			public int Change24 { get; set; }
			public int Change24Pct { get; set; }
			public int VolumeDay { get; set; }
			public int Volume24 { get; set; }
			public int TotalVolume24 { get; set; }
			public int OpenDay { get; set; }
			public int Open24 { get; set; }
			public int HighDay { get; set; }
			public int High24 { get; set; }
			public int LowDay { get; set; }
			public int Low24 { get; set; }
			public int Supply { get; set; }
			public int MktCap { get; set; }
		}
		public class StMetrics {
			public static string[] props = {"Icon", "Coin", "Value", "ChangeDay", "ChangeDayPct", "Change24", "Change24Pct",
											"VolumeDay", "Volume24", "TotalVolume24", "OpenDay", "Open24", "HighDay", "High24",
											"LowDay", "Low24", "Supply", "MktCap", "Header", "Edge", "Rows", "IconSize",
											"HeaderText", "RowsValues"};
			public object this[string prop] {
				get { return GetType().GetProperty(prop).GetValue(this, null); }
				set { GetType().GetProperty(prop).SetValue(this, value, null); }
			}
			public int Icon { get; set; }
			public int Coin { get; set; }
			public int Value { get; set; }
			public int ChangeDay { get; set; }
			public int ChangeDayPct { get; set; }
			public int Change24 { get; set; }
			public int Change24Pct { get; set; }
			public int VolumeDay { get; set; }
			public int Volume24 { get; set; }
			public int TotalVolume24 { get; set; }
			public int OpenDay { get; set; }
			public int Open24 { get; set; }
			public int HighDay { get; set; }
			public int High24 { get; set; }
			public int LowDay { get; set; }
			public int Low24 { get; set; }
			public int Supply { get; set; }
			public int MktCap { get; set; }
			public int Header { get; set; }
			public int Edge { get; set; }
			public int Rows { get; set; }
			public int IconSize { get; set; }
			public float HeaderText { get; set; } 
			public float RowsValues { get; set; }
		}
		public class StPages {
			public int Size { get; set; }
			public int Default { get; set; }
			public bool Rotate { get; set; }
			public float RotateRate { get; set; }
		}

		public enum DefaultType {
			Coins	   = 0x0001,
			Basic	   = 0x0002,
			Visibility = 0x0004,
			ColorLight = 0x0008,
			ColorDark  = 0x0010,
			Coords	   = 0x0020,
			Digits	   = 0x0040,
			Metrics	   = 0x0080,
			Pages	   = 0x0100,
			All		   = 0xFFFF
		}

		public List<StCoin>[] Coins = CreateStCoinsList(); // Coins[page][coin_pos]
		public StBasic Basic		   = new StBasic();
		public StVisibility Visibility = new StVisibility();
		public StColor Color		   = new StColor();
		public StCoords Coords		   = new StCoords();
		public StDigits Digits		   = new StDigits();
		public StMetrics Metrics	   = new StMetrics();
		public StPages Pages		   = new StPages();

		private string _file_path = "";
		private IniData _ini = new IniData();

		private static List<StCoin>[] CreateStCoinsList() {
			List<StCoin>[] ret = new List<StCoin>[10];
			for(int i = 0; i < 10; i++)
				ret[i] = new List<StCoin>();
			return ret;
		}

		public bool BindFile(string file_path) {
			_file_path = file_path;
			try {
				_ini = new IniParser.FileIniDataParser().ReadFile(file_path);
			} catch {
				return false;
			}
			return true;
		}
		public bool Load() {

			Func<IniData, bool> MissingAttr = (data) => {
				Settings check = new Settings();
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

			if(MissingAttr(_ini)) {
				Global.DbgPrint("MissingAttr");
				return false;
			}

			try {

				Reset();

				Basic.RefreshRate = AssignRule<int>()(_ini["Basic"]["RefreshRate"], (x) => x >= 1);
				Basic.Startup	  = bool.Parse(_ini["Basic"]["Startup"]);

				Coords.PosX		= int.Parse(_ini["Coords"]["PosX"]);
				Coords.PosY		= int.Parse(_ini["Coords"]["PosY"]);
				Coords.LockPos  = bool.Parse(_ini["Coords"]["LockPos"]);
				Coords.ExitSave = bool.Parse(_ini["Coords"]["ExitSave"]);

				Pages.Size		 = AssignRule<int>()(_ini["Pages"]["Size"], (x) => (x >= 1 && x <= 10));
				Pages.Default    = AssignRule<int>()(_ini["Pages"]["Default"], (x) => (x >= 0 && x < Pages.Size));
				Pages.Rotate	 = bool.Parse(_ini["Pages"]["Rotate"]);
				Pages.RotateRate = AssignRule<int>()(_ini["Pages"]["RotateRate"], (x) => x >= 1);

				foreach(string prop in StVisibility.props) { 
					Visibility[prop] = bool.Parse(_ini["Visibility"][prop]);
				}
				foreach(string prop in StColor.props) {
					Color[prop] = Global.StrHexToColor(_ini["Color"][prop]);
				}
				foreach(string prop in StDigits.props) {
					Digits[prop] = AssignRule<int>()(_ini["Digits"][prop], (x) => x >= 1);
				}
				foreach(string prop in StMetrics.props) {
					try {
						Metrics[prop] = AssignRule<int>()(_ini["Metrics"][prop], (x) => x >= 1);
					} 
					catch {
						Metrics[prop] = AssignRule<float>()(_ini["Metrics"][prop], (x) => x >= 1.0f);
					}
				}

				for(int i = 0; i < 10; i++) {
					foreach(KeyData coin_sectname in _ini["Page" + i.ToString()]) {
						KeyDataCollection coin = _ini[coin_sectname.Value];
						StCoin st = new StCoin();
						st.Coin			  = coin["Coin"];
						st.Target		  = coin["Target"];
						st.Alarm.Up		  = AssignRule<int>()(coin["AlarmUp"], (x) => x >= 0);
						st.Alarm.Down	  = AssignRule<int>()(coin["AlarmDown"], (x) => x >= 0);
						st.Graph.PosX	  = int.Parse(coin["GraphPosX"]);
						st.Graph.PosY	  = int.Parse(coin["GraphPosY"]);
						st.Graph.SizeX	  = AssignRule<int>()(coin["GraphSizeX"], (x) => x >= 0);
						st.Graph.SizeY	  = AssignRule<int>()(coin["GraphSizeY"], (x) => x >= 0);
						st.Graph.LockPos  = bool.Parse(coin["GraphLockPos"]);
						st.Graph.ExitSave = bool.Parse(coin["GraphExitSave"]);
						st.Graph.RefreshRate = int.Parse(coin["GraphRefreshRate"]);
						st.Graph.Startup  = bool.Parse(coin["GraphStartup"]);
						Coins[i].Add(st);
					}
				}

			} catch(Exception e) {
				Global.DbgPrint(e.ToString());
				return false;
			}

			return true;
		}
		public void Store() {

			_ini = new IniData();

			_ini["Basic"]["RefreshRate"] = Basic.RefreshRate.ToString();
			_ini["Basic"]["Startup"]	 = Basic.Startup.ToString();

			_ini["Coords"]["PosX"]     = Coords.PosX.ToString();
			_ini["Coords"]["PosY"]	   = Coords.PosY.ToString();
			_ini["Coords"]["LockPos"]  = Coords.LockPos.ToString();
			_ini["Coords"]["ExitSave"] = Coords.ExitSave.ToString();

			_ini["Pages"]["Size"]		= Pages.Size.ToString();
			_ini["Pages"]["Default"]    = Pages.Default.ToString();
			_ini["Pages"]["Rotate"]		= Pages.Rotate.ToString();
			_ini["Pages"]["RotateRate"] = Pages.RotateRate.ToString();

			foreach(string prop in StVisibility.props)
				_ini["Visibility"][prop] = Visibility[prop].ToString();
			foreach(string prop in StColor.props)
				_ini["Color"][prop] = Global.ColorToStrHex((Color)Color[prop]);
			foreach(string prop in StDigits.props) 
				_ini["Digits"][prop] = Digits[prop].ToString();
			foreach(string prop in StMetrics.props)
				_ini["Metrics"][prop] = Metrics[prop].ToString();

			for(int i = 0; i < 10; i++) {
				string strp = "Page" + i.ToString();
				KeyDataCollection page = _ini[strp];
				foreach(StCoin st in Coins[i]) {
					string strconv = st.Coin + "_" + st.Target;
					page[strconv]  = strp + "_" + strconv;
					KeyDataCollection coin = _ini[strp + "_" + strconv];
					coin["Coin"]		  = st.Coin;
					coin["Target"]		  = st.Target;
					coin["AlarmUp"]		  = st.Alarm.Up.ToString();
					coin["AlarmDown"]	  = st.Alarm.Down.ToString();
					coin["GraphPosX"]	  = st.Graph.PosX.ToString();
					coin["GraphPosY"]	  = st.Graph.PosY.ToString();
					coin["GraphSizeX"]	  = st.Graph.SizeX.ToString();
					coin["GraphSizeY"]	  = st.Graph.SizeY.ToString();
					coin["GraphLockPos"]  = st.Graph.LockPos.ToString();
					coin["GraphExitSave"] = st.Graph.ExitSave.ToString();
					coin["GraphRefreshRate"] = st.Graph.RefreshRate.ToString();
					coin["GraphStartup"]  = st.Graph.Startup.ToString();
				}
			}

		}
		public bool Save() {
			if(_file_path.Length == 0)
				return false;
			try {
				new IniParser.FileIniDataParser().WriteFile(_file_path, _ini);
			} catch {
				return false;
			}
			return true;
		}
		public void Default(DefaultType type = DefaultType.All) {

			if((type & DefaultType.Coins) != 0) {
				Coins = CreateStCoinsList();
				
				string[] coins = { "BTC", "ETH", "ETC", "LTC", "ZEC", "VTC", "LBC", "DASH", "XMR", "DOGE" };
				foreach(string coin in coins) {
					StCoin st = new StCoin();
					st.Coin = coin;
					st.Target = "USD";
					st.Alarm.Up = 0.0f;
					st.Alarm.Down = 0.0f;
					st.Graph.PosX = 100;
					st.Graph.PosY = 100;
					st.Graph.LockPos = false;
					st.Graph.ExitSave = true;
					st.Graph.RefreshRate = 10;
					st.Graph.Startup = false;
					Coins[0].Add(st);
				}
			}
			if((type & DefaultType.Basic) != 0) {
				Basic.RefreshRate	  = 20;
				Basic.Startup		  = false;
			}
			if((type & DefaultType.Visibility) != 0) {
				foreach(string prop in StVisibility.props)
					Visibility[prop] = false;
				Visibility.Icon		= true;
				Visibility.Coin		= true;
				Visibility.Value	= true;
				Visibility.Change24 = true;
			}
			if((type & DefaultType.ColorLight) != 0) {
				foreach(string prop in StColor.props)
					Color[prop] = Global.StrHexToColor("FF000000");
				Color.Background1	   = Global.StrHexToColor("FFF3F7F7");
				Color.Background2	   = Global.StrHexToColor("FFFFFFFF");
				Color.PositiveRefresh  = Global.StrHexToColor("FFCEEBD3");
				Color.NegativeRefresh  = Global.StrHexToColor("FFF6D4D1");
				Color.Edge			   = Global.StrHexToColor("FFAFAFAF");
				Color.PositiveChange   = Global.StrHexToColor("FF27892F");
				Color.NegativeChange   = Global.StrHexToColor("FFCF6563");
				Color.HeaderText	   = Global.StrHexToColor("FF000000");
				Color.HeaderBackground = Global.StrHexToColor("FFF0F0F0");
			}
			else if((type & DefaultType.ColorDark) != 0) {
				foreach(string prop in StColor.props)
					Color[prop] = Global.StrHexToColor("FFDADADA");
				Color.Background1	   = Global.StrHexToColor("FF1E1E1E");
				Color.Background2	   = Global.StrHexToColor("FF2F2F2F");
				Color.PositiveRefresh  = Global.StrHexToColor("FF3A8F49");
				Color.NegativeRefresh  = Global.StrHexToColor("FF96261D");
				Color.Edge			   = Global.StrHexToColor("FF535353");
				Color.PositiveChange   = Global.StrHexToColor("FF27892F");
				Color.NegativeChange   = Global.StrHexToColor("FFCF6563");
				Color.HeaderText	   = Global.StrHexToColor("FFC7C7C7");
				Color.HeaderBackground = Global.StrHexToColor("FF2C2C2C");

			}
			if((type & DefaultType.Coords) != 0) {
				Coords.PosX = 50;
				Coords.PosY = 50;
				Coords.LockPos  = false;
				Coords.ExitSave = true;
			}
			if((type & DefaultType.Digits) != 0) {
				foreach(string prop in StDigits.props) 
					Digits[prop] = 7;
			}
			if((type & DefaultType.Metrics) != 0) {
				foreach(string prop in StMetrics.props)
					Metrics[prop] = 60;
				Metrics.Icon	 = 25;
				Metrics.Coin	 = 40;
				Metrics.Edge	 = 2;
				Metrics.Header	 = 15;
				Metrics.Rows	 = 22;
				Metrics.IconSize = 16;
				Metrics.HeaderText = 8.25f;
				Metrics.RowsValues = 8.25f;
			}
			if((type & DefaultType.Pages) != 0) {
				Pages.Size = 1;
				Pages.Default = 0;
				Pages.Rotate = false;
				Pages.RotateRate = 10.0f;
			}

		}
		public void CloneSt(ref Settings other) {
			other.Coins = Coins;
			other.Basic = Basic;
			other.Visibility = Visibility;
			other.Color = Color;
			other.Coords = Coords;
			other.Digits = Digits;
			other.Metrics = Metrics;
			other.Pages = Pages;
		}
		public void Reset() {
			Coins = CreateStCoinsList();
			Basic = new StBasic();
			Visibility = new StVisibility();
			Color = new StColor();
			Coords = new StCoords();
			Digits = new StDigits();
			Metrics = new StMetrics();
			Pages = new StPages();
		}

		public static bool CreateIni(string file_path) {
			try {
				new IniParser.FileIniDataParser().WriteFile(file_path, new IniData());
			} catch {
				return false;
			}
			return true;
		}

		public bool ContainsConv(int page, string coin, string target) {
			foreach(StCoin st in Coins[page])
				if(st.Coin == coin && st.Target == target)
					return true;
			return false;
		}

	}

}
