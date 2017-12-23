
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
        
        public List<string> BadConvs = null; // FormType.Check
        public JObject CoinDataBase = null; // FormType.CoinList

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

					try {

						JObject json = CCRequest.HttpRequest(CCRequest.ConvertQuery(coin_list));

						BadConvs = new List<string>();

						for(int i = 0; i < coin_list.Count; i++) {

							Settings.StCoin st = coin_list[i];

							try {
								Invoke((MethodInvoker)delegate {
									labelProgress.Text = st.Coin + " (" + st.CoinName + ") -> " + st.Target + " (" + st.TargetName + ")";
									progressBar.Value = i;
								});
							} catch { }

							if(json["RAW"]?[st.Coin]?[st.Target] == null)
								BadConvs.Add(st.Coin + " (" + st.CoinName + ") -> " + st.Target + " (" + st.TargetName + ")");

						}

					} catch { }

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
					client.DownloadDataCompleted += new DownloadDataCompletedEventHandler((ddc_sender, ddc_ev) => {
						try {
							data.Write(ddc_ev.Result, 0, ddc_ev.Result.Length);
						} catch { }
					});

					try {
						await client.DownloadDataTaskAsync(new Uri("https://www.cryptocompare.com/api/data/coinlist/"));

					} catch {
						Invoke((MethodInvoker)delegate { Close(); });
						return;
					}

					data.Position = 0;
					try {
						if(data.Length > 0) {
							CoinDataBase = JObject.Parse(new StreamReader(data).ReadToEnd());
						}
						else {
							Invoke((MethodInvoker)delegate { Close(); });
							return;
						}
					} catch {
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

					List<Tuple<string, string>> misses = new List<Tuple<string, string>>();

					int coin_count = 0, no_url = 0, failed = 0;
					foreach(JToken coin in Global.Json["Data"].Values()) {
						Invoke((MethodInvoker)delegate {
							labelProgress.Text = "Searching missing icons (" + coin_count + "/" + ((JObject)Global.Json["Data"]).Count + ")";
							progressBar.Value = coin_count++;
						});

						if(Global.GetIcon(coin["Name"].ToString()).Height == 1) {
							if(coin["ImageUrl"] != null)
								misses.Add(new Tuple<string, string>(coin["Name"].ToString(), coin["ImageUrl"].ToString()));
							else if(coin["FiatCurrency"] == null)
								no_url++;
						}
					}

					Invoke((MethodInvoker)delegate {
						progressBar.Maximum = misses.Count;
					});

					for(int i = 0; i < misses.Count; i++) {
						try {
							Invoke((MethodInvoker)delegate {
								labelProgress.Text = "Trying to download the missing icons (" + i + "/" + misses.Count + ")";
								progressBar.Value = i;
							});
							CCRequest.DownloadIcon("https://www.cryptocompare.com" + misses[i].Item2).Save(Global.IconsFolder + misses[i].Item1.ToLower() + ".ico", System.Drawing.Imaging.ImageFormat.Icon);
						} catch {
							failed++;
						}
					}

					MessageBox.Show((misses.Count - failed).ToString() + " icons were downloaded\n" +
									no_url.ToString() + " coins doesn't have an associated download address\n" +
									failed.ToString() + " icons couldn't be downloaded");


					Invoke((MethodInvoker)delegate { Close(); });

				}).Start();

			};
		}

    }

}
