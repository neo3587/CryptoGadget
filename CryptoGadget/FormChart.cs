using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using Newtonsoft.Json.Linq;



namespace CryptoGadget {

	public partial class FormChart : Form {

		private readonly Settings.StCoin _coin = null;

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

		private void SetColors() {
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

			Color fore_color = Color.FromArgb(200, 200, 200);
			Color grid_color = Color.FromArgb(20, 20, 20);
			Color back_color = Color.FromArgb(40, 40, 40);
			Color line_color = Color.FromArgb(120, 120, 120);
			Color candle_up_color = Color.Green;
			Color candle_down_color = Color.IndianRed;

			mainChart.ChartAreas[0].AxisX.LineColor = mainChart.ChartAreas[0].AxisY.LineColor = fore_color;
			mainChart.ChartAreas[0].AxisX.LabelStyle.ForeColor = mainChart.ChartAreas[0].AxisY.LabelStyle.ForeColor = fore_color;

			mainChart.ChartAreas[0].BackColor = mainChart.BackColor = back_color;

			LabelColor(this, fore_color, back_color);

			mainChart.ChartAreas[0].AxisX.MajorGrid.LineColor = mainChart.ChartAreas[0].AxisY.MajorGrid.LineColor = grid_color;
			mainChart.ChartAreas[0].AxisX.MajorTickMark.LineColor = mainChart.ChartAreas[0].AxisY.MajorTickMark.LineColor = grid_color;

			mainChart.ChartAreas[0].CursorX.LineColor = mainChart.ChartAreas[0].CursorY.LineColor = line_color;

			mainChart.Series[0]["PriceUpColor"] = candle_up_color.ToArgb().ToString();
			mainChart.Series[0]["PriceDownColor"] = candle_down_color.ToArgb().ToString();
			
		}

		public FormChart(Settings.StCoin coin) {

			InitializeComponent();
			_coin = coin;

			Load += (sender, e) => {

				SetColors();

				mainChart.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
				mainChart.ChartAreas[0].AxisX.Interval = 4;
				//mainChart.Series[0].BorderWidth = 1;

				try {

					JObject json = CCRequest.HttpRequest(CCRequest.HistoQuery(_coin, CCRequest.HistoType.Hour, 120, 3));
					if(json != null && json["Response"]?.ToString().ToLower() != "error") {
						double min_bounds = double.MaxValue, max_bounds = double.MinValue;
						foreach(JToken jtok in json["Data"]) {
							DataPoint dp = new DataPoint();
							double high  = jtok["high"].ToObject<double>();
							double low   = jtok["low"].ToObject<double>();
							double open	 = jtok["open"].ToObject<double>();
							double close = jtok["close"].ToObject<double>();
							dp.YValues = new double[] { high, low, open, close };
							dp.AxisLabel = Epoch.AddSeconds(jtok["time"].ToObject<UInt64>()).ToString();
							dp.Color = close >= open ? Color.Green : Color.IndianRed;
							mainChart.Series[0].Points.Add(dp);
							min_bounds = Math.Min(min_bounds, low);
							max_bounds = Math.Max(max_bounds, high);
						}
						mainChart.ChartAreas[0].AxisY.Minimum = min_bounds * 0.9;
						mainChart.ChartAreas[0].AxisY.Maximum = max_bounds * 1.1;
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
