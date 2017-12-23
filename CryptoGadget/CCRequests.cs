using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Drawing;

using Newtonsoft.Json.Linq;



namespace CryptoGadget {

	class CCRequest { // CryptoCompareRequest

		public enum HistoType {
			Minute = 0x00,
			Hour = 0x01,
			Day = 0x02
		}

		public static string ConvertQuery(Settings.CoinList list_st) {
			SortedSet<string> input = new SortedSet<string>();
			SortedSet<string> output = new SortedSet<string>();
			foreach(Settings.StCoin st in list_st) {
				input.Add(st.Coin);
				output.Add(st.Target);
			}
			return "https://min-api.cryptocompare.com/data/pricemultifull?fsyms=" + string.Join(",", input).ToUpper() + "&tsyms=" + string.Join(",", output).ToUpper() + "&extraParams=CryptoGadget";
		}
		public static string HistoQuery(Settings.StCoin st, HistoType type, int size = 60, int step = 1, int time = -1) {
			string str_type = type == HistoType.Minute ? "Minute" : (type == HistoType.Hour ? "Hour" : "Day");
			return "https://min-api.cryptocompare.com/data/histo" + str_type + "?fsym=" + st.Coin + "&tsym=" + st.Target + 
				   "&limit=" + size.ToString() + "&aggregate=" + step.ToString() + (time < 0 ? "" : "&toTs=" + time.ToString()) + 
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
