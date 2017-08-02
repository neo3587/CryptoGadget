﻿
using System;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using System.IO;

using Newtonsoft.Json.Linq;







namespace CryptoGadget {

    public partial class ProgressForm : Form {
        
        internal List<string> badCoins = new List<string>(); // SettingsForm.buttonCheck
        internal JObject coindb = null; // SettingsForm.GetCoinDB

        // SettingsForm.buttonCheck
        public ProgressForm(List<Tuple<string, string>> coinList, string target) {

            InitializeComponent();

            Text = "Cryptogadget Settings [Check]";

            string targetCoin = target.Substring(0, target.LastIndexOf('(')).Trim(' ');
            progressBar.Maximum = coinList.Count;

            HandleCreated += (sender, ev) => {
                new Thread(() => {

                    int errCount = 0;

                    for(int i = 0; i < coinList.Count;) {
                        try {
                            Invoke((MethodInvoker)delegate {
                                labelProgress.Text = coinList[i].Item1 + " (" + coinList[i].Item2 + ") -> " + target;
                                progressBar.Value = i;
                            });
                        } catch(Exception) { }

                        Thread.Sleep(200);

                        try {
                            JObject json = Common.HttpRequest(coinList[i].Item1, targetCoin);
                            if(json["success"].ToString().ToLower() == "false" || json["ticker"] == null)
                                badCoins.Add(coinList[i].Item1 + " (" + coinList[i].Item2 + ")");
                            i++;
                        } catch(Exception) {
                            if(errCount++ == 10) {
                                badCoins.Add(coinList[i].Item1 + " (" + coinList[i].Item2 + ")");
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

        // SettignsForm.GetCoinDB
        public ProgressForm() {

            InitializeComponent();

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
