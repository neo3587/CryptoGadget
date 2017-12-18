using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;




namespace CryptoGadget {

	public class Settings {
		
		public class _PropManager<T> : INotifyPropertyChanged {
			public static PropertyInfo[] GetProps() {
				return typeof(T).GetProperties().Where(p => p.GetIndexParameters().Length == 0).ToArray();
			}
			public object this[string prop] {
				get { return GetType().GetProperty(prop).GetValue(this, null); }
				set { GetType().GetProperty(prop).SetValue(this, value, null); }
			}

			public event PropertyChangedEventHandler PropertyChanged;
			protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "") {
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public class StCoin : _PropManager<StCoin> {
			private Bitmap _icon = null;
			private string _coin = "";
			private string _coin_name = "";
			private string _target = "";
			private string _target_name = "";

			public class StAlarm : _PropManager<StAlarm> {
				private float _up = 0.0f;
				private float _down = 0.0f;

				public float Up {
					get => _up;
					set { _up = value; NotifyPropertyChanged(); }
				}
				public float Down {
					get => _down;
					set { _down = value; NotifyPropertyChanged(); }
				}
			}
			public class StGraph : _PropManager<StAlarm> {
				private int _pos_x = 50;
				private int _pos_y = 50;
				private int _size_x = 100;
				private int _size_y = 100;
				private bool _lock_pos = false;
				private bool _exit_save = true;
				private int _refresh_rate = 60;
				private bool _startup = false;

				public int PosX {
					get => _pos_x;
					set { _pos_x = value; NotifyPropertyChanged(); }
				}
				public int PosY {
					get => _pos_y;
					set { _pos_y = value; NotifyPropertyChanged(); }
				}
				public int SizeX {
					get => _size_x;
					set { _size_x = value; NotifyPropertyChanged(); }
				}
				public int SizeY {
					get => _size_y;
					set { _size_y = value; NotifyPropertyChanged(); }
				}
				public bool LockPos {
					get => _lock_pos;
					set { _lock_pos = value; NotifyPropertyChanged(); }
				}
				public bool ExitSave { // saves pos & size
					get => _exit_save;
					set { _exit_save = value; NotifyPropertyChanged(); }
				} 
				public int RefreshRate {
					get => _refresh_rate;
					set { _refresh_rate = value; NotifyPropertyChanged(); }
				}
				public bool Startup {
					get => _startup;
					set { _startup = value; NotifyPropertyChanged(); }
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
			public StAlarm Alarm = new StAlarm();
			public StGraph Graph = new StGraph();
		}
		public class StBasic : _PropManager<StBasic> {
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
		public class StVisibility : _PropManager<StVisibility> {
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
		public class StColor : _PropManager<StColor> {
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
		public class StCoords : _PropManager<StCoords> {
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
		public class StMetrics : _PropManager<StMetrics> {
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
		public class StPages : _PropManager<StPages> {
			private int _size;
			private int _default;
			private bool _rotate;
			private float _rotate_rate;

			public int Size {
				get => _size;
				set { _size = value; NotifyPropertyChanged(); }
			}
			public int Default {
				get => _default;
				set { _default = value; NotifyPropertyChanged(); }
			}
			public bool Rotate {
				get => _rotate;
				set { _rotate = value; NotifyPropertyChanged(); }
			}
			public float RotateRate {
				get => _rotate_rate;
				set { _rotate_rate = value; NotifyPropertyChanged(); }
			}
		}

		public class StColumn : _PropManager<StColumn> {
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
		public class StGrid : _PropManager<StGrid> {
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
																		 ("LastMarket",		"Last Market",		"") };
			public static ValueTuple<string, string, string>[] jsget = props.Where(tp => tp.Item3 != "").ToArray();

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
		public class CoinList : BindingList<StCoin> {
			public int FindConv(string coin, string target) {
				for(int i = 0; i < Count; i++)
					if(this[i].Coin == coin && this[i].Target == target)
						return i;
				return -1;
			}
		}

		public CoinList[] Coins = CreateCoinList(); // Coins[page][coin_pos]
		public StBasic Basic 		   = new StBasic();
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
				Coins      = JsonConvert.DeserializeObject<CoinList[]>(_json["Coins"].ToString()); 
				Basic	   = JsonConvert.DeserializeObject<StBasic>(_json["Basic"].ToString());
				Visibility = JsonConvert.DeserializeObject<StVisibility>(_json["Visibility"].ToString());
				Color	   = JsonConvert.DeserializeObject<StColor>(_json["Color"].ToString());
				Coords     = JsonConvert.DeserializeObject<StCoords>(_json["Coords"].ToString());
				Metrics    = JsonConvert.DeserializeObject<StMetrics>(_json["Metrics"].ToString());
				Pages      = JsonConvert.DeserializeObject<StPages>(_json["Pages"].ToString());
				Grid       = JsonConvert.DeserializeObject<StGrid>(_json["Grid"].ToString());
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

				ThrowRule<int>()(Basic.RefreshRate, x => x >= 1);

				foreach(PropertyInfo prop in StMetrics.GetProps()) {
					if(Metrics[prop.Name] is int)
						ThrowRule<int>()(Metrics[prop.Name], x => x >= 1);
					else
						ThrowRule<float>()(Metrics[prop.Name], x => x >= 1.0f);
				}

				ThrowRule<int>()(Pages.Size, x => (x >= 1 && x <= 10));
				ThrowRule<int>()(Pages.Default, x => (x >= 0 && x < Pages.Size));
				ThrowRule<float>()(Pages.RotateRate, x => x >= 1.0f);

				foreach(PropertyInfo prop in StGrid.GetProps()) {
					ThrowRule<int>()((Grid[prop.Name] as StColumn).Width, x => x >= 1);
					ThrowRule<int>()((Grid[prop.Name] as StColumn).Digits, x => x >= 0);
				}

				for(int i = 0; i < 10; i++) {
					foreach(StCoin st in Coins[i]) {
						ThrowRule<float>()(st.Alarm.Up, x => x >= 0);
						ThrowRule<float>()(st.Alarm.Down, x => x >= 0);
						ThrowRule<int>()(st.Graph.SizeX, x => x >= 20);
						ThrowRule<int>()(st.Graph.SizeY, x => x >= 20);
						ThrowRule<int>()(st.Graph.RefreshRate, x => x >= 1);
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
					(Grid[prop.Item1] as StColumn).Digits = 7;
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
		public Settings Clone() {
			Settings sett = new Settings();
			sett._json = JObject.Parse(JsonConvert.SerializeObject(this));
			sett.Load();
			sett._json = _json;
			sett._file_path = _file_path;
			return sett;
		}

		public static CoinList[] CreateCoinList() {
			CoinList[] ret = new CoinList[10];
			ret.Initialize();
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
