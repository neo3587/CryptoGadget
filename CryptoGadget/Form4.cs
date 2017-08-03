
using System;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using System.IO;

using Newtonsoft.Json.Linq;







namespace CryptoGadget {

    public partial class ProgressForm : Form {
        
        public List<string> badCoins = new List<string>(); // FormType.Check
        public JObject coindb = null; // FormType.Download

        public enum FormType {
            Check = 0x01,
            Download = 0x02
        };

        // SettingsForm.buttonCheck
        public ProgressForm(SettingsForm form, FormType ft) {

            InitializeComponent();

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
                                if(json["success"].ToString().ToLower() == "false" || json["ticker"] == null)
                                    badCoins.Add(row.Cells[1].Value + " (" + row.Cells[2].Value + ")");
                                i++;
                            } catch(Exception) {
                                if(errCount++ == 5) {
                                    badCoins.Add(row.Cells[1].Value + " (" + row.Cells[2].Value + ")");
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

            else if(ft == FormType.Download) {

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

        }

    }

}
