
using System;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Drawing;

using Newtonsoft.Json.Linq;




namespace CryptoGadget {

    public partial class ProgressForm : Form {
        
        public List<string> badConvs = new List<string>(); // FormType.Check
        public JObject coindb = null; // FormType.Download

        public enum FormType {
            Check = 0x01,
            CoinList = 0x02,
            Icons = 0x03
        };


        public ProgressForm(SettingsForm form, FormType ft) {

            InitializeComponent();

            // SettingsForm.buttonCheck
            if(ft == FormType.Check) { 

                Text = "Cryptogadget Settings [Check]";

                progressBar.Maximum = form.coinGrid.RowCount;

                HandleCreated += (sender, ev) => {
                    new Thread(() => {

                        int errCount = 0;

                        for(int i = 0; i < form.coinGrid.RowCount;) {

                            DataGridViewRow row = form.coinGrid.Rows[i];

                            try {
                                Invoke((MethodInvoker)delegate {
                                    labelProgress.Text = row.Cells[1].Value + " (" + row.Cells[2].Value + ") -> " + row.Cells[3].Value + " (" + row.Cells[4].Value + ")";
                                    progressBar.Value = i;
                                });
                            } catch(Exception) { }

                            try {
                                JObject json = Common.HttpRequest(row.Cells[1].Value.ToString(), row.Cells[3].Value.ToString());
                                if(json[row.Cells[3].Value.ToString().ToUpper()] == null)
                                    badConvs.Add(row.Cells[1].Value.ToString() + " (" + row.Cells[2].Value.ToString() + ") -> " + row.Cells[3].Value.ToString() + " (" + row.Cells[4].Value.ToString() + ")");
                                i++;
                            } catch(Exception) {
                                if(errCount++ == 5) {
                                    badConvs.Add(row.Cells[1].Value.ToString() + " (" + row.Cells[2].Value.ToString() + ") -> " + row.Cells[3].Value.ToString() + " (" + row.Cells[4].Value.ToString() + ")");
                                    errCount = 0;
                                    i++;
                                }
                            }

                        }

                        Invoke((MethodInvoker)delegate {
                            Close();
                        });

                    }).Start();

                };

            }

            // SettingsForm.buttonDownloadList
            else if(ft == FormType.CoinList) {

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

            // SettingsForm.buttonDownloadMissingIcons
            else if(ft == FormType.Icons) {

                Text = "Cryptogadget Settings [Download]";
                labelProgress.Text = "Searching missing icons (0/" + ((JObject)Common.json["Data"]).Count + ")";
                progressBar.Maximum = ((JObject)Common.json["Data"]).Count;

                HandleCreated += (sender, ev) => {

                    new Thread(() => {

                        WebClient client = new WebClient();
                        List<Tuple<string, string>> misses = new List<Tuple<string, string>>();

                        int coinCount = 0, noUrl = 0, failed = 0;
                        foreach(JToken coin in Common.json["Data"].Values()) {
                            Invoke((MethodInvoker)delegate {
                                labelProgress.Text = "Searching missing icons (" + coinCount + "/" + ((JObject)Common.json["Data"]).Count + ")";
                                progressBar.Value = coinCount++;
                            });

                            if(Common.GetIcon(coin["Name"].ToString()).Height == 1) {
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

                                    Bitmap bmp = new Bitmap(32, 32);
                                    
                                    // Minimum quality loss resize
                                    using(Graphics gr = Graphics.FromImage(bmp)) {
                                        gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                                        gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                        gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                                        gr.DrawImage(Image.FromStream(data), new Rectangle(0, 0, 32, 32));
                                    }

                                    bmp.Save(Common.iconLocation + misses[i].Item1.ToLower() + ".ico", System.Drawing.Imaging.ImageFormat.Icon);
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

}
