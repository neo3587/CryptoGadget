using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Drawing;

using Newtonsoft.Json.Linq;



namespace CryptoGadget {

	class CCRequest { // CryptoCompareRequest

		private static string _GetInOutArgs(Settings.CoinList[] cl_list, string market) {
			SortedSet<string> input = new SortedSet<string>();
			SortedSet<string> output = new SortedSet<string>();
			foreach(Settings.CoinList cl in cl_list) {
				foreach(Settings.StCoin st in cl) {
					input.Add(st.Coin);
					output.Add(st.Target);
				}
			}
			return "?fsyms=" + string.Join(",", input).ToUpper() +
					"&tsyms=" + string.Join(",", output).ToUpper() +
					(market != "" ? "&e=" + market : "") +
					"&extraParams=CryptoGadget";
		}

		public enum HistoType {
			Minute = 0x00,
			Hour = 0x01,
			Day = 0x02
		}

		public static string ConvertQueryBasic(Settings.CoinList cl, string market = "") {
			Settings.CoinList[] cl_list = new Settings.CoinList[1] { cl };
			return "https://min-api.cryptocompare.com/data/pricemulti" + _GetInOutArgs(cl_list, market);
		}
		public static string ConvertQueryBasic(Settings.CoinList[] cl_list, string market = "") {
			return "https://min-api.cryptocompare.com/data/pricemulti" + _GetInOutArgs(cl_list, market);
		}
		public static string ConvertQueryFull(Settings.CoinList cl, string market = "") {
			Settings.CoinList[] cl_list = new Settings.CoinList[1] { cl };
			return "https://min-api.cryptocompare.com/data/pricemultifull" + _GetInOutArgs(cl_list, market);
		}
		public static string ConvertQueryFull(Settings.CoinList[] cl_list, string market = "") {
			return "https://min-api.cryptocompare.com/data/pricemulti" + _GetInOutArgs(cl_list, market);
		}
		public static string HistoQuery(string coin, string target, HistoType type, int size = 60, int step = 1, int time = -1) {
			string str_type = type == HistoType.Minute ? "minute" : (type == HistoType.Hour ? "hour" : "day");
			return "https://min-api.cryptocompare.com/data/histo" + str_type + 
					"?fsym=" + coin + 
					"&tsym=" + target + 
					"&limit=" + size.ToString() + 
					"&aggregate=" + step.ToString() + 
					(time >= 0 ? "&toTs=" + time.ToString() : "") +
					"&extraParams=CryptoGadget";
		}

		public static JObject HttpRequest(string query) {
			HttpWebRequest HttpReq = (HttpWebRequest)WebRequest.Create(query);
			return JObject.Parse(new StreamReader(((HttpWebResponse)HttpReq.GetResponse()).GetResponseStream()).ReadToEnd());
		}

		public static Bitmap DownloadIcon(string query) {
			WebClient client = new WebClient();
			using(MemoryStream data = new MemoryStream()) {
				byte[] buffer = client.DownloadData(new Uri(query));
				data.Write(buffer, 0, buffer.Length);
				return Global.IconResize(Image.FromStream(data), 32);
			}
		}

	}

}
