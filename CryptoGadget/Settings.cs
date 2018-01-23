using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Drawing;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



namespace CryptoGadget {

	public class Settings {

		public string Version { get => Global.Version; }

		public class StCoin : PropManager<StCoin> {
			private Bitmap _icon = null;
			private string _coin = "";
			private string _coin_name = "";
			private string _target = "";
			private string _target_name = "";

			public class StAlert : PropManager<StAlert> {
				private decimal _above = 0.0m;
				private decimal _below = 0.0m;

				public decimal Above {
					get => _above;
					set { _above = value; NotifyPropertyChanged(); }
				}
				public decimal Below {
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
			[JsonIgnore]
			public char AlertType { get; set; } = '-';
			public StAlert Alert = new StAlert();
		}
		public class StBasic : PropManager<StBasic> {
			private int _refresh_rate;
			private int _alert_check_rate;
			private bool _startup;
			private bool _notify_new_version;

			public int RefreshRate {
				get => _refresh_rate;
				set { _refresh_rate = value; NotifyPropertyChanged(); }
			}
			public int AlertCheckRate {
				get => _alert_check_rate;
				set { _alert_check_rate = value; NotifyPropertyChanged(); }
			}
			public bool Startup {
				get => _startup;
				set { _startup = value; NotifyPropertyChanged(); }
			}
			public bool NotifyNewVersion {
				get => _notify_new_version;
				set { _notify_new_version = value; NotifyPropertyChanged(); }
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
			private bool _auto_rotate;
			private int _rotate_rate;
			private int _max_page_rotate;

			public int Default {
				get => _default;
				set { _default = value; NotifyPropertyChanged(); }
			}
			public bool ExitSave {
				get => _exit_save;
				set { _exit_save = value; NotifyPropertyChanged(); }
			}
			public bool AutoRotate {
				get => _auto_rotate;
				set { _auto_rotate = value; NotifyPropertyChanged(); }
			}
			public int RotateRate {
				get => _rotate_rate;
				set { _rotate_rate = value; NotifyPropertyChanged(); }
			}
			public int MaxPageRotate {
				get => _max_page_rotate;
				set { _max_page_rotate = value; NotifyPropertyChanged(); }
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

			public void BindColsPtr() {
				foreach(StColumn st in Columns) 
					this[st.Column] = st;
			}
		}

		public class StChart : PropManager<StChart> {

			public class StColor : PropManager<StColor> {
				private Color _foreground;
				private Color _background;
				private Color _grid;
				private Color _cursor_lines;
				private Color _candle_up;
				private Color _candle_down;

				public Color ForeGround {
					get => _foreground;
					set { _foreground = value; NotifyPropertyChanged(); }
				}
				public Color BackGround {
					get => _background;
					set { _background = value; NotifyPropertyChanged(); }
				}
				public Color Grid {
					get => _grid;
					set { _grid = value; NotifyPropertyChanged(); }
				}
				public Color CursorLines {
					get => _cursor_lines;
					set { _cursor_lines = value; NotifyPropertyChanged(); }
				}
				public Color CandleUp {
					get => _candle_up;
					set { _candle_up = value; NotifyPropertyChanged(); }
				}
				public Color CandleDown {
					get => _candle_down;
					set { _candle_down = value; NotifyPropertyChanged(); }
				}
			}
			
			private int _default_step;
			private bool _show_minmax;
			
			public int DefaultStep {
				get => _default_step;
				set { _default_step = value; NotifyPropertyChanged(); }
			}
			public bool ShowMinMax {
				get => _show_minmax;
				set { _show_minmax = value; NotifyPropertyChanged(); }
			}
			public StColor Color = new StColor();
		}

		public enum DefaultType {
			Coins			= 0x0001,
			Basic			= 0x0002,
			Visibility		= 0x0004,
			ColorLight		= 0x0008,
			ColorDark		= 0x0010,
			Coords			= 0x0020,
			Metrics			= 0x0040,
			Pages			= 0x0080,
			Market			= 0x0100,
			Grid			= 0x0200,
			Chart			= 0x0400,
			ChartColorLight = 0x0800,
			ChartColorDark	= 0x1000,
			All				= 0xFFFF
		}
		public class CoinList : BindingList<StCoin> {
			public CoinList() : base() { }
			public CoinList(IList<StCoin> list) : base(list) { }
			public int FindConv(string coin, string target) {
				for(int i = 0; i < Count; i++)
					if(this[i].Coin == coin && this[i].Target == target)
						return i;
				return -1;
			}
		}

		public CoinList[] Coins         = new CoinList[10].Select(x => new CoinList()).ToArray(); // Coins[page][coin_pos]
		public StBasic Basic			= new StBasic();
		public StVisibility Visibility	= new StVisibility();
		public StColor Color			= new StColor();
		public StCoords Coords			= new StCoords();
		public StMetrics Metrics		= new StMetrics();
		public StPages Pages			= new StPages();
		public StMarket Market			= new StMarket();
		[JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
		public StGrid Grid				= new StGrid();
		public StChart Chart			= new StChart();

		[JsonIgnore]
		private string _file_path = "";
		
		[JsonIgnore]
		private JObject _json = new JObject();

		public bool BindFile(string file_path) {
			_file_path = file_path;
			try {
				using(StreamReader file = File.OpenText(file_path)) 
					using(JsonTextReader reader = new JsonTextReader(file)) 
						_json = VersionUpgrade((JObject)JToken.ReadFrom(reader));
			} catch(Exception e) {
				Global.DbgPrint("Settings.BindFile ERROR: " + e.Message);
				return false;
			}
			return true;
		}
		public bool Load() {

			try {
				Default(); // Prevents errors from missing/null values
				JsonConvert.PopulateObject(_json.ToString(), this, new JsonSerializerSettings {
					NullValueHandling = NullValueHandling.Ignore,
					MissingMemberHandling = MissingMemberHandling.Ignore
				});
				Grid.BindColsPtr();
			} catch(Exception e) {
				Global.DbgMsgShow("Settings.Load ERROR:\n" + e.StackTrace);
				return false;
			}
			
			return true;
		}
		public void Store() {
			_json = JObject.Parse(JsonConvert.SerializeObject(this));
		}
		public bool Check() {

			Action<object, Func<T, bool>> ThrowRule<T>(object data, Func<T, bool> fn) {
				if(!fn((T)data))
					throw new Exception(data.ToString());
				return null;
			};

			try {

				// Basic
				ThrowRule<int>(Basic.RefreshRate, x => (x >= 3 && x <= 3600));
				ThrowRule<int>(Basic.AlertCheckRate, x => (x >= 3 && x <= 10800));

				// Metrics
				foreach(PropertyInfo prop in StMetrics.GetProps()) {
					if(Metrics[prop.Name] is int)
						ThrowRule<int>(Metrics[prop.Name], x => x >= 1);
					else
						ThrowRule<float>(Metrics[prop.Name], x => x >= 1.0f);
				}

				// Pages
				ThrowRule<int>(Pages.Default, x => (x >= 0 && x <= 9));
				ThrowRule<int>(Pages.MaxPageRotate, x => (x >= 0 && x <= 9));
				ThrowRule<int>(Pages.RotateRate, x => (x >= 3 && x <= 10800));

				// Grid
				foreach(PropertyInfo prop in StGrid.GetProps()) {
					ThrowRule<int>((Grid[prop.Name] as StColumn).Width, x => (x >= 1 && x <= 999));
					ThrowRule<int>((Grid[prop.Name] as StColumn).Digits, x => (x >= 0 && x <= 20));
				}
				ThrowRule<int>(Grid.Columns.Count, x => x == StGrid.GetProps().Count());
				ThrowRule<int>(Grid.Columns.Except(StGrid.GetProps().Select(p => Grid[p.Name])).Count(), x => x == 0);
				ThrowRule<int>(StGrid.GetProps().Select(p => Grid[p.Name]).Except(Grid.Columns).Count(), x => x == 0);

				// Coins
				for(int i = 0; i < 10; i++) {
					foreach(StCoin st in Coins[i]) {
						ThrowRule<decimal>(st.Alert.Above, x => x >= 0);
						ThrowRule<decimal>(st.Alert.Below, x => x >= 0.0m);
					}
				}

			}
			catch(Exception e) {
				Global.DbgMsgShow("Settings.Check ERROR: Value = " + e.Message + "\n" + e.StackTrace);
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
				Global.DbgMsgShow("Settings.Save ERROR:\n" + e.StackTrace);
				return false;
			}
			return true;
		}
		public void Default(DefaultType type = DefaultType.All, int page = -1) {
			if((type & DefaultType.Coins) != 0) {
				Action<int> FillCoinPage = (int n_page) => {
					ValueTuple<string, string>[] coins = {
						("BTC", "Bitcoin"),
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
					Coins[n_page].Clear();
					foreach(ValueTuple<string, string> tp in coins) {
						StCoin st = new StCoin();
						st.Coin = tp.Item1;
						st.CoinName = tp.Item2;
						st.Target = "USD";
						st.TargetName = "United States Dollar";
						Coins[n_page].Add(st);
					}
				};
				if(page < 0) {
					for(int i = 0; i < Coins.Length; i++)
						FillCoinPage(i);
				}
				else {
					FillCoinPage(page);
				}
			}
			if((type & DefaultType.Basic) != 0) {
				Basic.RefreshRate		= 20;
				Basic.AlertCheckRate	= 60;
				Basic.Startup			= false;
				Basic.NotifyNewVersion	= true;
			}
			if((type & DefaultType.Visibility) != 0) {
				Visibility.Edge     = true;
				Visibility.Header   = true;
				Visibility.Refresh  = true;
			}
			if((type & DefaultType.ColorDark) != 0) {
				Color.RowsText			= Global.StrHexToColor("FFDADADA");
				Color.RowsValues		= Global.StrHexToColor("FFDADADA");
				Color.Background1		= Global.StrHexToColor("FF1E1E1E");
				Color.Background2		= Global.StrHexToColor("FF2F2F2F");
				Color.PositiveRefresh	= Global.StrHexToColor("FF3A8F49");
				Color.NegativeRefresh	= Global.StrHexToColor("FF96261D");
				Color.PositiveChange	= Global.StrHexToColor("FF27892F");
				Color.NegativeChange	= Global.StrHexToColor("FFCF6563");
				Color.HeaderText		= Global.StrHexToColor("FFC7C7C7");
				Color.HeaderBackground	= Global.StrHexToColor("FF2C2C2C");
				Color.Edge				= Global.StrHexToColor("FF535353");
			}
			else if((type & DefaultType.ColorLight) != 0) {
				Color.RowsText			= Global.StrHexToColor("FF000000");
				Color.RowsValues		= Global.StrHexToColor("FF000000");
				Color.Background1		= Global.StrHexToColor("FFF3F7F7");
				Color.Background2		= Global.StrHexToColor("FFFFFFFF");
				Color.PositiveRefresh	= Global.StrHexToColor("FFCEEBD3");
				Color.NegativeRefresh	= Global.StrHexToColor("FFF6D4D1");
				Color.PositiveChange	= Global.StrHexToColor("FF27892F");
				Color.NegativeChange	= Global.StrHexToColor("FFCF6563");
				Color.HeaderText		= Global.StrHexToColor("FF000000");
				Color.HeaderBackground	= Global.StrHexToColor("FFF0F0F0");
				Color.Edge				= Global.StrHexToColor("FFAFAFAF");
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
				Pages.AutoRotate = false;
				Pages.RotateRate = 60;
				Pages.MaxPageRotate = 9;
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
				Grid.BindColsPtr();
			}
			if((type & DefaultType.Chart) != 0) {
				Chart.DefaultStep = 2; // 20m
				Chart.ShowMinMax  = true;
			}
			if((type & DefaultType.ChartColorDark) != 0) {
				Chart.Color.ForeGround	 = Global.StrHexToColor("FFC8C8C8");
				Chart.Color.BackGround	 = Global.StrHexToColor("FF1B262D");
				Chart.Color.Grid		 = Global.StrHexToColor("FF28343C");
				Chart.Color.CursorLines = Global.StrHexToColor("FF787878");
				Chart.Color.CandleUp	 = Global.StrHexToColor("FF6A833A");
				Chart.Color.CandleDown	 = Global.StrHexToColor("FF8A3A3B");
			}
			else if((type & DefaultType.ChartColorLight) != 0) {
				Chart.Color.ForeGround  = Global.StrHexToColor("FF070707");
				Chart.Color.BackGround  = Global.StrHexToColor("FFF0F0F0");
				Chart.Color.Grid		 = Global.StrHexToColor("FFAFAFAF");
				Chart.Color.CursorLines = Global.StrHexToColor("FF008FDB");
				Chart.Color.CandleUp	 = Global.StrHexToColor("FF68C221");
				Chart.Color.CandleDown  = Global.StrHexToColor("FFCB2C4B");
			}
		}
		public void CloneTo(Settings sett) {
			sett._json = JObject.Parse(JsonConvert.SerializeObject(this));
			sett.Load();
			sett._json = (JObject)_json.DeepClone();
			sett._file_path = _file_path;
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
				Global.DbgPrint("Settings.CreateSettFile ERROR: " + e.ToString());
				return false;
			}
			return true;
		}

		public CoinList GetAlarmCoins() {
			CoinList list = new CoinList();
			foreach(CoinList cl in Coins) 
				list = new CoinList(list.Concat(cl.Where(x => (x.Alert.Above > 0.0m || x.Alert.Below > 0.0m))).ToList());
			return list;
		}

		public JObject VersionUpgrade(JObject json) {

			if(json["Version"] == null) { // <= 2.6.0
				json["Version"] = "2.6.1";
				json["Chart"]["Color"] = new JObject(
					new JProperty("ForeGround",  json["Chart"]["ForeColor"]),
					new JProperty("BackGround",	 json["Chart"]["BackColor"]),
					new JProperty("Grid",		 json["Chart"]["GridColor"]),
					new JProperty("CursorLines", json["Chart"]["CursorLinesColor"]),
					new JProperty("CandleUp",	 json["Chart"]["CandleUpColor"]),
					new JProperty("CandleDown",  json["Chart"]["CandleDownColor"])
				);
			}

			return json;
		}

	}

}
