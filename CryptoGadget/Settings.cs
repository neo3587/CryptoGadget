using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Drawing;

using IniParser.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;




namespace CryptoGadget {

	public class Settings : ICloneable {

		public class _PropGetter<T> {
			public static PropertyInfo[] GetProps() {
				return typeof(T).GetProperties().Where(p => p.GetIndexParameters().Length == 0).ToArray();
			}
			public object this[string prop] {
				get { return GetType().GetProperty(prop).GetValue(this, null); }
				set { GetType().GetProperty(prop).SetValue(this, value, null); }
			}
		}

		public class StCoin : _PropGetter<StCoin> {
			public class StAlarm : _PropGetter<StAlarm> {
				public float Up { get; set; }
				public float Down { get; set; }
			}
			public class StGraph : _PropGetter<StAlarm> {
				public int PosX { get; set; }
				public int PosY { get; set; }
				public int SizeX { get; set; }
				public int SizeY { get; set; }
				public bool LockPos { get; set; }
				public bool ExitSave { get; set; } // saves pos & size
				public int RefreshRate { get; set; }
				public bool Startup { get; set; }
			}
			public Bitmap Icon { get; set; }
			public string Coin { get; set; }
			public string CoinName { get; set; }
			public string Target { get; set; }
			public string TargetName { get; set; }
			public StAlarm Alarm = new StAlarm();
			public StGraph Graph = new StGraph();
		}
		public class StBasic : _PropGetter<StBasic> {
			public int RefreshRate { get; set; }
			public bool Startup { get; set; }
		}
		public class StVisibility : _PropGetter<StVisibility> {
			public bool Header { get; set; }
			public bool Edge { get; set; }
			public bool Refresh { get; set; }
		}
		public class StColor : _PropGetter<StColor> {
			public Color Coin { get; set; }
			public Color Values { get; set; }
			public Color PositiveChange { get; set; }
			public Color NegativeChange { get; set; }
			public Color Background1 { get; set; }
			public Color Background2 { get; set; }
			public Color PositiveRefresh { get; set; }
			public Color NegativeRefresh { get; set; }
			public Color HeaderText { get; set; }
			public Color RowsText { get; set; }
			public Color HeaderBackground { get; set; }
			public Color Edge { get; set; }
		}
		public class StCoords : _PropGetter<StCoords> {
			public int PosX { get; set; }
			public int PosY { get; set; }
			public bool ExitSave { get; set; }
			public bool LockPos { get; set; }
		}
		public class StMetrics : _PropGetter<StMetrics> {
			public int Header { get; set; }
			public int Edge { get; set; }
			public int Rows { get; set; }
			public int IconSize { get; set; }
			public float HeaderText { get; set; }
			public float RowsValues { get; set; }
		}
		public class StPages : _PropGetter<StPages> {
			public int Size { get; set; }
			public int Default { get; set; }
			public bool Rotate { get; set; }
			public float RotateRate { get; set; }
		}

		public class StColumn : _PropGetter<StColumn> {
			public string Column { get; set; }
			public string Name { get; set; }
			public int Width { get; set; }
			public int Digits { get; set; }
			public bool Enabled { get; set; }
		}
		public class StGrid : _PropGetter<StGrid> {
			// <PropertyName, ShownName, JsonName>
			public static ValueTuple<string, string, string>[] props = { ("Icon",			"",					""),
																		 ("Coin",			"Coin",				""),
																		 ("TargetIcon",		"",					""),
																		 ("Target",			"Target",			""),
																		 ("Value",			"Value",			"PRICE"),
																		 ("ChangeDay",		"Change Day",		"CHANGEDAY"),
																		 ("ChangeDayPct",	"Change Day (%)",	"CHANGEPCTDAY"),
																		 ("Change24",		"Change 24h",		"CHANGE24HOUR"),
																		 ("Change24Pct",	"Change 24h (%)",	"CHANGEPCT24HOUR"),
																		 ("VolumeDay",		"Volume Day",		"VOLUMEDAY"),
																		 ("Volume24",		"Volume 24h",		"VOLUME24HOUR"),
																		 ("TotalVolume24",	"Total Volume 24h",	"TOTALVOLUME24H"),
																		 ("OpenDay",		"Open Day",			"OPENDAY"),
																		 ("Open24",			"Open 24h",         "OPEN24HOUR"),
																		 ("HighDay",		"High Day",			"HIGHDAY"),
																		 ("High24",			"High 24h",			"HIGH24HOUR"),
																		 ("LowDay",			"Low Day",			"LOWDAY"),
																		 ("Low24",			"Low 24h",			"LOW24HOUR"),
																		 ("Supply",			"Supply",			"SUPPLY"),
																		 ("MktCap",			"Market Cap",		"MKTCAP"),
																		 ("LastMarket",		"Last Market",		"LASTMARKET") };
			public StColumn Icon { get; set; } = new StColumn(); // skip this on json get
			public StColumn Coin { get; set; } = new StColumn(); // skip this on json get
			public StColumn TargetIcon { get; set; } = new StColumn(); // skip this on json get
			public StColumn Target { get; set; } = new StColumn(); // skip this on json get
			public StColumn Value { get; set; } = new StColumn();
			public StColumn ChangeDay { get; set; } = new StColumn();
			public StColumn ChangeDayPct { get; set; } = new StColumn();
			public StColumn Change24 { get; set; } = new StColumn();
			public StColumn Change24Pct { get; set; } = new StColumn();
			public StColumn VolumeDay { get; set; } = new StColumn();
			public StColumn Volume24 { get; set; } = new StColumn();
			public StColumn TotalVolume24 { get; set; } = new StColumn();
			public StColumn OpenDay { get; set; } = new StColumn();
			public StColumn Open24 { get; set; } = new StColumn();
			public StColumn HighDay { get; set; } = new StColumn();
			public StColumn High24 { get; set; } = new StColumn();
			public StColumn LowDay { get; set; } = new StColumn();
			public StColumn Low24 { get; set; } = new StColumn();
			public StColumn Supply { get; set; } = new StColumn();
			public StColumn MktCap { get; set; } = new StColumn();
			public StColumn LastMarket { get; set; } = new StColumn(); 
		}

		public enum DefaultType {
			Coins      = 0x0001,
			Basic      = 0x0002,
			Visibility = 0x0004,
			ColorLight = 0x0008,
			ColorDark  = 0x0010,
			Coords     = 0x0020,
			Metrics    = 0x0080,
			Pages      = 0x0100,
			Grid       = 0x0200,
			All        = 0xFFFF
		}
		public class CoinList : BindingList<StCoin> { }

		public CoinList[] Coins = CreateCoinList(); // Coins[page][coin_pos]
		public StBasic Basic		   = new StBasic();
		public StVisibility Visibility = new StVisibility();
		public StColor Color		   = new StColor();
		public StCoords Coords		   = new StCoords();
		public StMetrics Metrics	   = new StMetrics();
		public StPages Pages		   = new StPages();
		public StGrid Grid             = new StGrid();

		private string _file_path = "";
		private JObject _json = new JObject();

		public bool BindFile(string file_path) {
			_file_path = file_path;
			try {
				using(StreamReader file = File.OpenText(@"c:\videogames.json"))
				using(JsonTextReader reader = new JsonTextReader(file)) { 
				    _json = (JObject)JToken.ReadFrom(reader);
				}
			} catch {
				Global.DbgPrint("Settings.BindFile");
				return false;
			}
			return true;
		}
		public bool Load() {

			Func<string, Func<T, bool>, T> AssignRule<T>() where T : IConvertible {
				return (data, fn) => {
					T value = (T)Convert.ChangeType(data, default(T).GetTypeCode());
					if(!fn(value))
						throw new Exception();
					return value;
				};
			};

			try {
				Coins      = JsonConvert.DeserializeObject<CoinList[]>(_json["Coins"].ToString());
				Basic	   = JsonConvert.DeserializeObject<StBasic>(_json["Basic"].ToString());
				Visibility = JsonConvert.DeserializeObject<StVisibility>(_json["Visibility"].ToString());
				Color	   = JsonConvert.DeserializeObject<StColor>(_json["Color"].ToString());
				Coords     = JsonConvert.DeserializeObject<StCoords>(_json["Coords"].ToString());
				Metrics    = JsonConvert.DeserializeObject<StMetrics>(_json["Metrics"].ToString());
				Pages      = JsonConvert.DeserializeObject<StPages>(_json["Pages"].ToString());
				Grid       = JsonConvert.DeserializeObject<StGrid>(_json["Grid"].ToString());
			} catch(Exception e) {
				Global.DbgPrint(e.ToString());
				return false;
			}
			
			return true;
		}
		public void Store() {
			_json = JObject.Parse(JsonConvert.SerializeObject(this));
		}
		public bool Save() {
			if(_file_path.Length == 0)
				return false;
			try {
				using(StreamWriter file = File.CreateText(_file_path))
				using(JsonTextWriter writer = new JsonTextWriter(file)) {
					writer.Formatting = Formatting.Indented;
					_json.WriteTo(writer);
				}
			} catch(Exception e) {
				System.Windows.Forms.MessageBox.Show(e.ToString());
				return false;
			}
			return true;
		}
		public void Default(DefaultType type = DefaultType.All) {
			if((type & DefaultType.Coins) != 0) {
				Coins = CreateCoinList();
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
				Visibility.Edge     = true;
				Visibility.Header   = true;
				Visibility.Refresh  = true;
			}
			if((type & DefaultType.ColorLight) != 0) {
				foreach(PropertyInfo prop in StColor.GetProps())
					Color[prop.Name] = Global.StrHexToColor("FF000000");
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
				foreach(PropertyInfo prop in StColor.GetProps())
					Color[prop.Name] = Global.StrHexToColor("FFDADADA");
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
			if((type & DefaultType.Metrics) != 0) {
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
			if((type & DefaultType.Grid) != 0) {
				foreach(ValueTuple<string, string, string> prop in StGrid.props) { 
					(Grid[prop.Item1] as StColumn).Column = prop.Item1;
					(Grid[prop.Item1] as StColumn).Name = prop.Item2;
					(Grid[prop.Item1] as StColumn).Width = 60;
					(Grid[prop.Item1] as StColumn).Enabled = false;
				}
				Grid.Icon.Enabled = Grid.Coin.Enabled = Grid.Value.Enabled = Grid.Change24.Enabled = true;
				Grid.Icon.Width = 25;
				Grid.Coin.Width = 45;
				Grid.TargetIcon.Width = 25;
				Grid.Target.Width     = 45;
			}
		}
		public object Clone() {
			return MemberwiseClone();
		}

		public static CoinList[] CreateCoinList() {
			CoinList[] ret = new CoinList[10];
			for(int i = 0; i < 10; i++)
				ret[i] = new CoinList();
			return ret;
		}
		public static bool CreateSettFile(string file_path) {
			try {
				using(StreamWriter file = File.CreateText(file_path))
				using(JsonTextWriter writer = new JsonTextWriter(file)) {
					new JObject().WriteTo(writer);
				}
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
