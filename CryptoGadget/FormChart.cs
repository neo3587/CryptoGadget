using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using Newtonsoft.Json.Linq;



namespace CryptoGadget {

	public partial class FormChart : Form {

		private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
		public static void SuspendDrawing(Control parent) {
			SendMessage(parent.Handle, 11, false, 0);
		}
		public static void ResumeDrawing(Control parent) {
			SendMessage(parent.Handle, 11, true, 0);
			parent.Refresh();
		}



		public FormChart() {

			InitializeComponent();

			Load += (sender, e) => {

				Action<Control, Color, Color> LabelColor = null;
				LabelColor = (ctrl, fore, back) => {
					if(ctrl is Label) {
						ctrl.ForeColor = fore;
						ctrl.BackColor = back;
					}
					else {
						foreach(Control child in ctrl.Controls)
							LabelColor(child, fore, back);
					}
				};

				Color light_gray = Color.FromArgb(200, 200, 200);

				mainChart.ChartAreas[0].BackColor = mainChart.BackColor = Color.FromArgb(40, 40, 40);
				mainChart.Series[0].Color = mainChart.Series[0].BorderColor = light_gray;
				mainChart.ChartAreas[0].AxisX.LineColor = mainChart.ChartAreas[0].AxisY.LineColor = light_gray;
				mainChart.ChartAreas[0].AxisX.MajorGrid.LineColor = mainChart.ChartAreas[0].AxisY.MajorGrid.LineColor = light_gray;
				mainChart.ChartAreas[0].CursorX.LineColor = mainChart.ChartAreas[0].CursorY.LineColor = Color.FromArgb(120, 120, 120);
				LabelColor(this, light_gray, Color.FromArgb(40, 40, 40));

				mainChart.Series[0]["PriceUpColor"] = "Green";
				mainChart.Series[0]["PriceDownColor"] = "IndianRed";

				mainChart.ChartAreas[0].AxisX.LabelStyle.ForeColor = mainChart.ChartAreas[0].AxisY.LabelStyle.ForeColor = light_gray;
				mainChart.ChartAreas[0].ShadowColor = light_gray;
				mainChart.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
				mainChart.ChartAreas[0].AxisX.Interval = 4;

				try {

					JObject json = CCRequest.HttpRequest("https://min-api.cryptocompare.com/data/histohour?fsym=BTC&tsym=USD&limit=60&aggregate=3&e=CCCAGG");
					if(json != null && json["Response"]?.ToString().ToLower() != "error") {
						foreach(JToken jtok in json["Data"]) {
							DataPoint dp = new DataPoint();
							dp.YValues = new double[] { jtok["high"].ToObject<double>(), jtok["low"].ToObject<double>(), jtok["open"].ToObject<double>(), jtok["close"].ToObject<double>() };
							dp.AxisLabel = Epoch.AddSeconds(jtok["time"].ToObject<UInt64>()).ToString();
							mainChart.Series[0].Points.Add(dp);
						}
					}

				} catch { }

				mainChart.MouseDown += Global.DragMove;

			};

		}

		private void toolStripClose_Click(object sender, EventArgs e) {
			Close();
		}

		private void mainChart_MouseMove(object sender, MouseEventArgs e) {

			SuspendDrawing(this);

			try {

				mainChart.ChartAreas[0].CursorX.SetCursorPixelPosition(e.Location, true);
				mainChart.ChartAreas[0].CursorY.SetCursorPixelPosition(e.Location, true);

				int pt_index = (int)Math.Round(mainChart.ChartAreas[0].AxisX.PixelPositionToValue(e.X));
				if(pt_index < mainChart.Series[0].Points.Count) {
					DataPoint dp = mainChart.Series[0].Points[pt_index];
					labelDateVal.Text = dp.AxisLabel;
					labelHighVal.Text = dp.YValues[0].ToString();
					labelLowVal.Text = dp.YValues[1].ToString();
					labelOpenVal.Text = dp.YValues[2].ToString();
					labelCloseval.Text = dp.YValues[3].ToString();
				}
				labelValueVal.Text = mainChart.ChartAreas[0].AxisY.PixelPositionToValue(e.Y).ToString().Substring(0, 12);

			} catch { }

			ResumeDrawing(this);
		}

	}

}
