
using System;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Drawing;

using Newtonsoft.Json.Linq;




namespace CryptoGadget {

    public partial class FormProgressBar : Form {
        
        public List<string> badConvs = new List<string>(); // FormType.Check
        public JObject coindb = null; // FormType.CoinList

        public enum FormType {
            Check = 0x01,
            CoinList = 0x02,
            Icons = 0x03
        };


        public FormProgressBar(object param, FormType ft) {

            InitializeComponent();

			switch(ft) {
				case FormType.Check: // SettingsForm.buttonCheck
					FormCheck((Settings.CoinList)param);
					break;
				case FormType.CoinList:   // SettingsForm.buttonDownloadList
					FormDownloadCoinList();
					break;
				case FormType.Icons: // SettingsForm.buttonDownloadMissingIcons
					FormDownloadMissingIcons();
					break;
			}

        }

		public void FormCheck(Settings.CoinList coin_list) {

			Text = "Cryptogadget Settings [Check]";

			progressBar.Maximum = coin_list.Count;

			HandleCreated += (sender, ev) => {

				new Thread(() => {

					JObject json = CCRequest.HttpRequest(CCRequest.ConvertQuery(coin_list));

					for(int i = 0; i < coin_list.Count; i++) {

						Settings.StCoin st = coin_list[i];

						try {
							Invoke((MethodInvoker)delegate {
								labelProgress.Text = st.Coin + " (" + st.CoinName + ") -> " + st.Target + " (" + st.TargetName + ")";
								progressBar.Value = i;
							});
						} catch { }

						if(json["RAW"]?[st.Coin]?[st.Target] == null)
							badConvs.Add(st.Coin + " (" + st.CoinName + ") -> " + st.Target + " (" + st.TargetName + ")");
						
					}

					Invoke((MethodInvoker)delegate { Close(); });

				}).Start();

			};
		}

		public void FormDownloadCoinList() {

			Text = "Cryptogadget Settings [Download]";
			labelProgress.Text = "Downloading Coin List (0%)";
			progressBar.Maximum = 100;

			HandleCreated += (sender, ev) => {

				new Thread(async () => {

					WebClient client = new WebClient();
					MemoryStream data = new MemoryStream();

					client.DownloadProgressChanged += new DownloadProgressChangedEventHandler((dpc_sender, dpc_ev) => {
						Invoke((MethodInvoker)delegate {
							labelProgress.Text = "Downloading Coin List (" + dpc_ev.ProgressPercentage + "%)";
							progressBar.Value = dpc_ev.ProgressPercentage;
						});
					});
					client.DownloadDataCompleted += new DownloadDataCompletedEventHandler((orc_sender, orc_ev) => {
						try {
							data.Write(orc_ev.Result, 0, orc_ev.Result.Length);
						} catch(Exception) { }
					});

					try {
						await client.DownloadDataTaskAsync(new Uri("https://www.cryptocompare.com/api/data/coinlist/"));

					} catch(Exception) {
						Invoke((MethodInvoker)delegate { Close(); });
						return;
					}

					data.Position = 0;
					try {
						if(data.Length > 0) {
							coindb = JObject.Parse(new StreamReader(data).ReadToEnd());
						}
						else {
							Invoke((MethodInvoker)delegate { Close(); });
							return;
						}
					} catch(Exception) {
						Invoke((MethodInvoker)delegate { Close(); });
						return;
					}

					Invoke((MethodInvoker)delegate { Close(); });

				}).Start();

			};
		}

		public void FormDownloadMissingIcons() {

			Text = "Cryptogadget Settings [Download]";
			labelProgress.Text = "Searching missing icons (0/" + ((JObject)Global.Json["Data"]).Count + ")";
			progressBar.Maximum = ((JObject)Global.Json["Data"]).Count;

			HandleCreated += (sender, ev) => {

				new Thread(() => {

					WebClient client = new WebClient();
					List<Tuple<string, string>> misses = new List<Tuple<string, string>>();

					int coinCount = 0, noUrl = 0, failed = 0;
					foreach(JToken coin in Global.Json["Data"].Values()) {
						Invoke((MethodInvoker)delegate {
							labelProgress.Text = "Searching missing icons (" + coinCount + "/" + ((JObject)Global.Json["Data"]).Count + ")";
							progressBar.Value = coinCount++;
						});

						if(Global.GetIcon(coin["Name"].ToString()).Height == 1) {
							if(coin["ImageUrl"] != null)
								misses.Add(new Tuple<string, string>(coin["Name"].ToString(), coin["ImageUrl"].ToString()));
							else
								noUrl++;
						}
					}

					Invoke((MethodInvoker)delegate {
						progressBar.Maximum = misses.Count;
					});

					for(int i = 0; i < misses.Count; i++) {
						try {
							Invoke((MethodInvoker)delegate {
								labelProgress.Text = "Downloading Missing Icons (" + i + "/" + misses.Count + ")";
								progressBar.Value = i;
							});
							using(MemoryStream data = new MemoryStream()) {
								byte[] buffer = client.DownloadData(new Uri("https://www.cryptocompare.com" + misses[i].Item2));
								data.Write(buffer, 0, buffer.Length);
								Global.IconResize(Image.FromStream(data), 32).Save(Global.IconsFolder + misses[i].Item1.ToLower() + ".ico", System.Drawing.Imaging.ImageFormat.Icon);
							}
						} catch(Exception) {
							failed++;
						}
					}

					MessageBox.Show((misses.Count - failed).ToString() + " icons were downloaded\n" +
									noUrl.ToString() + " coins doesn't have an associated download address\n" +
									failed.ToString() + " icons couldn't be downloaded");


					Invoke((MethodInvoker)delegate { Close(); });

				}).Start();

			};
		}

    }

}
