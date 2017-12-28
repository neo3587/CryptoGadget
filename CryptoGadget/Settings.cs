using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Drawing;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;




namespace CryptoGadget {

	public class Settings {

		public class StCoin : PropManager<StCoin> {
			private Bitmap _icon = null;
			private string _coin = "";
			private string _coin_name = "";
			private string _target = "";
			private string _target_name = "";

			public class StAlert : PropManager<StAlert> {
				private float _above = 0.0f;
				private float _below = 0.0f;

				public float Above {
					get => _above;
					set { _above = value; NotifyPropertyChanged(); }
				}
				public float Below {
					get => _below;
					set { _below = value; NotifyPropertyChanged(); }
				}
			}
			[JsonIgnore]
			public Bitmap Icon {
				get => _icon;
				set { _icon = value; NotifyPropertyChanged(); }
			}
			public string Coin {
				get => _coin;
				set { _coin = value; NotifyPropertyChanged(); }
			}
			public string CoinName {
				get => _coin_name;
				set { _coin_name = value; NotifyPropertyChanged(); }
			}
			public string Target {
				get => _target;
				set { _target = value; NotifyPropertyChanged(); }
			}
			public string TargetName {
				get => _target_name;
				set { _target_name = value; NotifyPropertyChanged(); }
			}
			public StAlert Alert = new StAlert();
		}
		public class StBasic : PropManager<StBasic> {
			private int _refresh_rate;
			private bool _startup;

			public int RefreshRate {
				get => _refresh_rate;
				set { _refresh_rate = value; NotifyPropertyChanged(); }
			}
			public bool Startup {
				get => _startup;
				set { _startup = value; NotifyPropertyChanged(); }
			}
		}
		public class StVisibility : PropManager<StVisibility> {
			private bool _header;
			private bool _edge;
			private bool _refresh;

			public bool Header {
				get => _header;
				set { _header = value; NotifyPropertyChanged(); }
			}
			public bool Edge {
				get => _edge;
				set { _edge = value; NotifyPropertyChanged(); }
			}
			public bool Refresh {
				get => _refresh;
				set { _refresh = value; NotifyPropertyChanged(); }
			}
		}
		public class StColor : PropManager<StColor> {
			private Color _positive_change;
			private Color _negative_change;
			private Color _background1;
			private Color _background2;
			private Color _positive_refresh;
			private Color _negative_refresh;
			private Color _header_text;
			private Color _rows_text;
			private Color _rows_values;
			private Color _header_background;
			private Color _edge;

			public Color PositiveChange {
				get => _positive_change;
				set { _positive_change = value; NotifyPropertyChanged(); }
			}
			public Color NegativeChange {
				get => _negative_change;
				set { _negative_change = value; NotifyPropertyChanged(); }
			}
			public Color Background1 {
				get => _background1;
				set { _background1 = value; NotifyPropertyChanged(); }
			}
			public Color Background2 {
				get => _background2;
				set { _background2 = value; NotifyPropertyChanged(); }
			}
			public Color PositiveRefresh {
				get => _positive_refresh;
				set { _positive_refresh = value; NotifyPropertyChanged(); }
			}
			public Color NegativeRefresh {
				get => _negative_refresh;
				set { _negative_refresh = value; NotifyPropertyChanged(); }
			}
			public Color HeaderText {
				get => _header_text;
				set { _header_text = value; NotifyPropertyChanged(); }
			}
			public Color RowsText {
				get => _rows_text;
				set { _rows_text = value; NotifyPropertyChanged(); }
			}
			public Color RowsValues {
				get => _rows_values;
				set { _rows_values = value; NotifyPropertyChanged(); }
			}
			public Color HeaderBackground {
				get => _header_background;
				set { _header_background = value; NotifyPropertyChanged(); }
			}
			public Color Edge {
				get => _edge;
				set { _edge = value; NotifyPropertyChanged(); }
			}
		}
		public class StCoords : PropManager<StCoords> {
			private int _pos_x;
			private int _pos_y;
			private bool _exit_save;
			private bool _lock_pos;

			public int PosX {
				get => _pos_x;
				set { _pos_x = value; NotifyPropertyChanged(); }
			}
			public int PosY {
				get => _pos_y;
				set { _pos_y = value; NotifyPropertyChanged(); }
			}
			public bool ExitSave {
				get => _exit_save;
				set { _exit_save = value; NotifyPropertyChanged(); }
			}
			public bool LockPos {
				get => _lock_pos;
				set { _lock_pos = value; NotifyPropertyChanged(); }
			}
		}
		public class StMetrics : PropManager<StMetrics> {
			private int _header;
			private int _edge;
			private int _rows;
			private int _icon_size;
			private float _header_text;
			private float _rows_values;

			public int Header {
				get => _header;
				set { _header = value; NotifyPropertyChanged(); }
			}
			public int Edge {
				get => _edge;
				set { _edge = value; NotifyPropertyChanged(); }
			}
			public int Rows {
				get => _rows;
				set { _rows = value; NotifyPropertyChanged(); }
			}
			public int IconSize {
				get => _icon_size;
				set { _icon_size = value; NotifyPropertyChanged(); }
			}
			public float HeaderText {
				get => _header_text;
				set { _header_text = value; NotifyPropertyChanged(); }
			}
			public float RowsValues {
				get => _rows_values;
				set { _rows_values = value; NotifyPropertyChanged(); }
			}
		}
		public class StPages : PropManager<StPages> {
			private int _default;
			private bool _exit_save;

			public int Default {
				get => _default;
				set { _default = value; NotifyPropertyChanged(); }
			}
			public bool ExitSave {
				get => _exit_save;
				set { _exit_save = value; NotifyPropertyChanged(); }
			}
		}
		public class StMarket : PropManager<StMarket> {
			private string _market;

			public string Market {
				get => _market;
				set { _market = value; NotifyPropertyChanged(); }
			}
		}

		public class StColumn : PropManager<StColumn> {
			private string _column;
			private string _name;
			private int _width;
			private int _digits;
			private bool _enabled;

			public string Column {
				get => _column;
				set { _column = value; NotifyPropertyChanged(); }
			}
			public string Name {
				get => _name;
				set { _name = value; NotifyPropertyChanged(); }
			}
			public int Width {
				get => _width;
				set { _width = value; NotifyPropertyChanged(); }
			}
			public int Digits {
				get => _digits;
				set { _digits = value; NotifyPropertyChanged(); }
			}
			public bool Enabled {
				get => _enabled;
				set { _enabled = value; NotifyPropertyChanged(); }
			}
		}
		public class StGrid : PropManager<StGrid> {
			// <PropertyName, default_ShownName, JsonName, default_width, default_digits, default_enabled>
			public static ValueTuple<string, string, string, int, int, bool>[] props = { ("Icon",			"",					"",					25, 0, true),
																						 ("Coin",			"Coin",				"",					40, 0, true),
																						 ("TargetIcon",		"",					"",					25, 0, false),
																						 ("Target",			"Target",			"",					40, 0, false),
																						 ("Value",			"Value",			"PRICE",			60, 7, true),
																						 ("ChangeDay",		"Chg Day",			"CHANGEDAY",		60, 7, false),
																						 ("ChangeDayPct",	"Chg Day (%)",		"CHANGEPCTDAY",		70, 7, false),
																						 ("Change24",		"Chg 24h",			"CHANGE24HOUR",		60, 7, true),
																						 ("Change24Pct",	"Chg 24h (%)",		"CHANGEPCT24HOUR",  70, 7, false),
																						 ("VolumeDay",		"Vol Day",			"VOLUMEDAY",        70, 8, false),
																						 ("Volume24",		"Vol 24h",			"VOLUME24HOUR",     70, 8, false),
																						 ("TotalVolume24",	"Total Vol 24h",	"TOTALVOLUME24H",   70, 8, false),
																						 ("OpenDay",		"Open Day",			"OPENDAY",          60, 7, false),
																						 ("Open24",			"Open 24h",         "OPEN24HOUR",       60, 7, false),
																						 ("HighDay",		"High Day",			"HIGHDAY",          60, 7, false),
																						 ("High24",			"High 24h",			"HIGH24HOUR",       60, 7, false),
																						 ("LowDay",			"Low Day",			"LOWDAY",           60, 7, false),
																						 ("Low24",			"Low 24h",			"LOW24HOUR",        60, 7, false),
																						 ("Supply",			"Supply",			"SUPPLY",           80, 9, false),
																						 ("MktCap",			"Mkt Cap",			"MKTCAP",           80, 9, false),
																						 ("LastMarket",		"Last Mkt",			"",                 60, 0, false) };
			public static ValueTuple<string, string>[] jsget = props.Where(tp => tp.Item3 != "").Select(tp => (tp.Item1, tp.Item3)).ToArray();

			[JsonIgnore] public StColumn Icon { get; set; } // skip this on json get
			[JsonIgnore] public StColumn Coin { get; set; } // skip this on json get
			[JsonIgnore] public StColumn TargetIcon { get; set; } // skip this on json get
			[JsonIgnore] public StColumn Target { get; set; } // skip this on json get
			[JsonIgnore] public StColumn Value { get; set; }
			[JsonIgnore] public StColumn ChangeDay { get; set; }
			[JsonIgnore] public StColumn ChangeDayPct { get; set; }
			[JsonIgnore] public StColumn Change24 { get; set; }
			[JsonIgnore] public StColumn Change24Pct { get; set; }
			[JsonIgnore] public StColumn VolumeDay { get; set; }
			[JsonIgnore] public StColumn Volume24 { get; set; }
			[JsonIgnore] public StColumn TotalVolume24 { get; set; }
			[JsonIgnore] public StColumn OpenDay { get; set; }
			[JsonIgnore] public StColumn Open24 { get; set; }
			[JsonIgnore] public StColumn HighDay { get; set; }
			[JsonIgnore] public StColumn High24 { get; set; }
			[JsonIgnore] public StColumn LowDay { get; set; }
			[JsonIgnore] public StColumn Low24 { get; set; }
			[JsonIgnore] public StColumn Supply { get; set; }
			[JsonIgnore] public StColumn MktCap { get; set; }
			[JsonIgnore] public StColumn LastMarket { get; set; }

			public BindingList<StColumn> Columns = new BindingList<StColumn>();

			public void BindGridPtr() {
				foreach(StColumn st in Columns) 
					this[st.Column] = st;
			}
		}

		public enum DefaultType {
			Coins		= 0x0001,
			Basic		= 0x0002,
			Visibility	= 0x0004,
			ColorLight	= 0x0008,
			ColorDark	= 0x0010,
			Coords		= 0x0020,
			Metrics		= 0x0080,
			Pages		= 0x0100,
			Market		= 0x0200,
			Grid		= 0x0400,
			All			= 0xFFFF
		}
		public class CoinList : BindingList<StCoin> {
			public int FindConv(string coin, string target) {
				for(int i = 0; i < Count; i++)
					if(this[i].Coin == coin && this[i].Target == target)
						return i;
				return -1;
			}
		}

		public CoinList[] Coins = CreateCoinList(); // Coins[page][coin_pos]
		public StBasic Basic			= new StBasic();
		public StVisibility Visibility	= new StVisibility();
		public StColor Color			= new StColor();
		public StCoords Coords			= new StCoords();
		public StMetrics Metrics		= new StMetrics();
		public StPages Pages			= new StPages();
		public StMarket Market			= new StMarket();
		public StGrid Grid				= new StGrid();

		[JsonIgnore]
		private string _file_path = "";
		
		[JsonIgnore]
		private JObject _json = new JObject();

		public bool BindFile(string file_path) {
			_file_path = file_path;
			try {
				using(StreamReader file = File.OpenText(file_path)) 
				using(JsonTextReader reader = new JsonTextReader(file)) { 
					_json = (JObject)JToken.ReadFrom(reader);
				}
			} catch(Exception e) {
				Global.DbgPrint("ERROR: " + e.Message);
				return false;
			}
			return true;
		}
		public bool Load() {

			try {
				Default(); // Prevents errors from missing/null values
				JsonConvert.PopulateObject(_json.ToString(), this, new JsonSerializerSettings {
					NullValueHandling = NullValueHandling.Ignore,
					MissingMemberHandling = MissingMemberHandling.Ignore,
					ObjectCreationHandling = ObjectCreationHandling.Replace
				});
				Grid.BindGridPtr();
			} catch(Exception e) {
				Global.DbgMsgShow("ERROR: " + e.ToString());
				return false;
			}
			
			return true;
		}
		public void Store() {
			_json = JObject.Parse(JsonConvert.SerializeObject(this));
		}
		public bool Check() {

			// Check loaded properties

			Action<object, Func<T, bool>> ThrowRule<T>() {
				return (data, fn) => {
					if(!fn((T)data))
						throw new Exception(data.ToString());
				};
			};

			try {

				// Basic
				ThrowRule<int>()(Basic.RefreshRate, x => x >= 1);

				// Metrics
				foreach(PropertyInfo prop in StMetrics.GetProps()) {
					if(Metrics[prop.Name] is int)
						ThrowRule<int>()(Metrics[prop.Name], x => x >= 1);
					else
						ThrowRule<float>()(Metrics[prop.Name], x => x >= 1.0f);
				}

				// Pages
				ThrowRule<int>()(Pages.Default, x => (x >= 0 && x <= 9));

				// Grid
				foreach(PropertyInfo prop in StGrid.GetProps()) {
					ThrowRule<int>()((Grid[prop.Name] as StColumn).Width, x => x >= 1);
					ThrowRule<int>()((Grid[prop.Name] as StColumn).Digits, x => x >= 0);
				}
				ThrowRule<int>()(Grid.Columns.Count, x => x == StGrid.GetProps().Count());
				ThrowRule<int>()(Grid.Columns.Except(StGrid.GetProps().Select(p => Grid[p.Name])).Count(), x => x == 0);
				ThrowRule<int>()(StGrid.GetProps().Select(p => Grid[p.Name]).Except(Grid.Columns).Count(), x => x == 0);

				// Coins
				for(int i = 0; i < 10; i++) {
					foreach(StCoin st in Coins[i]) {
						ThrowRule<float>()(st.Alert.Above, x => x >= 0);
						ThrowRule<float>()(st.Alert.Below, x => x >= 0);
					}
				}

			}
			catch(Exception e) {
				Global.DbgMsgShow("ERROR: " + e.Message);
				return false;
			}


			return true;
		}
		public bool Save() {
			if(_file_path.Length == 0)
				return false;
			try {
				CreateSettFile(_file_path);
				using(StreamWriter file = new StreamWriter(_file_path))
				using(JsonTextWriter writer = new JsonTextWriter(file)) {
					writer.Formatting = Formatting.Indented;
					_json.WriteTo(writer);
				}
			} catch(Exception e) {
				Global.DbgMsgShow("ERROR: " + e.ToString());
				return false;
			}
			return true;
		}
		public void Default(DefaultType type = DefaultType.All, int page = -1) {
			Func<string, Color> StrHexToColor = (str) => {
				return System.Drawing.Color.FromArgb(int.Parse(str, System.Globalization.NumberStyles.HexNumber));
			};

			if((type & DefaultType.Coins) != 0) {
				ValueTuple<string, string>[] coins = { ("BTC", "Bitcoin"),
													   ("ETH", "Ethereum"),
													   ("ETC", "Ethereum Classic"),
													   ("LTC", "Litecoin"),
													   ("ZEC", "ZCash"),
													   ("VTC", "VertCoin"),
													   ("LBC", "LBRY Credits"),
													   ("DASH", "DigitalCash"),
													   ("XMR", "Monero"),
													   ("DOGE", "Dogecoin")
													 };

				int from = page >= 0 ? page : 0;
				int to = page >= 0 ? page+1 : 10;
				for(int i = from; i < to; i++) {
					Coins[i].Clear();
					foreach(ValueTuple<string, string> tp in coins) {
						StCoin st = new StCoin();
						st.Icon = Global.GetIcon(tp.Item1, 16);
						st.Coin = tp.Item1;
						st.CoinName = tp.Item2;
						st.Target = "USD";
						st.TargetName = "United States Dollar";
						Coins[i].Add(st);
					}
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
					Color[prop.Name] = StrHexToColor("FF000000");
				Color.Background1	   = StrHexToColor("FFF3F7F7");
				Color.Background2	   = StrHexToColor("FFFFFFFF");
				Color.PositiveRefresh  = StrHexToColor("FFCEEBD3");
				Color.NegativeRefresh  = StrHexToColor("FFF6D4D1");
				Color.Edge			   = StrHexToColor("FFAFAFAF");
				Color.PositiveChange   = StrHexToColor("FF27892F");
				Color.NegativeChange   = StrHexToColor("FFCF6563");
				Color.HeaderText	   = StrHexToColor("FF000000");
				Color.HeaderBackground = StrHexToColor("FFF0F0F0");
			}
			else if((type & DefaultType.ColorDark) != 0) {
				foreach(PropertyInfo prop in StColor.GetProps())
					Color[prop.Name] = StrHexToColor("FFDADADA");
				Color.Background1	   = StrHexToColor("FF1E1E1E");
				Color.Background2	   = StrHexToColor("FF2F2F2F");
				Color.PositiveRefresh  = StrHexToColor("FF3A8F49");
				Color.NegativeRefresh  = StrHexToColor("FF96261D");
				Color.Edge			   = StrHexToColor("FF535353");
				Color.PositiveChange   = StrHexToColor("FF27892F");
				Color.NegativeChange   = StrHexToColor("FFCF6563");
				Color.HeaderText	   = StrHexToColor("FFC7C7C7");
				Color.HeaderBackground = StrHexToColor("FF2C2C2C");

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
				Pages.Default = 0;
				Pages.ExitSave = false;
			}
			if((type & DefaultType.Market) != 0) {
				Market.Market = "";
			}
			if((type & DefaultType.Grid) != 0) {
				Grid.Columns.Clear();
				foreach(ValueTuple<string, string, string, int, int, bool> prop in StGrid.props) {
					StColumn st = new StColumn();
					st.Column = prop.Item1;
					st.Name = prop.Item2;
					st.Width = prop.Item4;
					st.Digits = prop.Item5;
					st.Enabled = prop.Item6;
					Grid.Columns.Add(st);
				}
				Grid.BindGridPtr();
			}
		}
		public void CloneTo(Settings sett) {
			sett._json = JObject.Parse(JsonConvert.SerializeObject(this));
			sett.Load();
		}
		public void CloneFileTo(Settings sett) {
			sett._json = (JObject)_json.DeepClone();
			sett._file_path = _file_path;
		}

		public static CoinList[] CreateCoinList() {
			CoinList[] ret = new CoinList[10];
			ret.Initialize();
			for(int i = 0; i < 10; i++)
				ret[i] = new CoinList();
			return ret;
		}
		public static bool CreateSettFile(string file_path) {
			try {
				if(!Directory.Exists(file_path)) 
					Directory.CreateDirectory(Path.GetDirectoryName(file_path));
				using(StreamWriter file = File.CreateText(file_path))
				using(JsonTextWriter writer = new JsonTextWriter(file)) {
					new JObject().WriteTo(writer);
				}
			} catch(Exception e) {
				Global.DbgPrint("ERROR: " + e.ToString());
				return false;
			}
			return true;
		}

	}

}
