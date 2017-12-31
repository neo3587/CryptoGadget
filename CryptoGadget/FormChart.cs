using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using Newtonsoft.Json.Linq;



namespace CryptoGadget {

	public partial class FormChart : Form {

		private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		private Point? _prev_pos = null;
		private ToolTip _tooltip = new ToolTip();



		public FormChart() {

			InitializeComponent();

			Load += (sender, e) => {

				Color light_gray = Color.FromArgb(200, 200, 200);

				mainChart.ChartAreas[0].BackColor = mainChart.BackColor = Color.FromArgb(40, 40, 40);
				mainChart.Series[0].Color = mainChart.Series[0].BorderColor = light_gray;
				mainChart.ChartAreas[0].AxisX.LineColor = mainChart.ChartAreas[0].AxisY.LineColor = light_gray;
				mainChart.ChartAreas[0].AxisX.MajorGrid.LineColor = mainChart.ChartAreas[0].AxisY.MajorGrid.LineColor = light_gray;

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

			};
			
		}

		private void toolStripClose_Click(object sender, EventArgs e) {
			Close();
		}

		private void mainChart_GetToolTipText(object sender, ToolTipEventArgs e) {
			if(e.HitTestResult.ChartElementType == ChartElementType.DataPoint) {
				int i = e.HitTestResult.PointIndex;
				DataPoint dp = e.HitTestResult.Series.Points[i];
				e.Text = string.Format("{0:F1}, {1:F1}", dp.XValue, dp.YValues[0]);
			}
		}
	}

}
