using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using Newtonsoft.Json.Linq;



namespace CryptoGadget {

	public partial class FormChart : Form {

		public class ChartSettings {
			public Color ForeColor { get; set; } = Color.FromArgb(200, 200, 200);
			public Color GridColor { get; set; } = Color.FromArgb(40, 52, 60);
			public Color BackColor { get; set; } = Color.FromArgb(27, 38, 45);
			public Color LineColor { get; set; } = Color.FromArgb(120, 120, 120);
			public Color CandleUpColor { get; set; } = Color.FromArgb(106, 131, 58);
			public Color CandleDownColor { get; set; } = Color.FromArgb(138, 58, 59);
		}
		private ChartSettings _sett = new ChartSettings();
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

			Action<Control, Color, Color> LabelColorApply = null;
			LabelColorApply = (ctrl, fore, back) => {
				if(ctrl is Label) {
					ctrl.ForeColor = fore;
					ctrl.BackColor = back;
				}
				else {
					foreach(Control child in ctrl.Controls)
						LabelColorApply(child, fore, back);
				}
			};

			mainChart.ChartAreas[0].AxisX.LineColor = mainChart.ChartAreas[0].AxisY.LineColor = _sett.ForeColor;
			mainChart.ChartAreas[0].AxisX.LabelStyle.ForeColor = mainChart.ChartAreas[0].AxisY.LabelStyle.ForeColor = _sett.ForeColor;

			mainChart.ChartAreas[0].BackColor = mainChart.BackColor = _sett.BackColor;

			LabelColorApply(this, _sett.ForeColor, _sett.BackColor);

			mainChart.ChartAreas[0].AxisX.MajorGrid.LineColor = mainChart.ChartAreas[0].AxisY.MajorGrid.LineColor = _sett.GridColor;
			mainChart.ChartAreas[0].AxisX.MajorTickMark.LineColor = mainChart.ChartAreas[0].AxisY.MajorTickMark.LineColor = _sett.GridColor;

			mainChart.ChartAreas[0].CursorX.LineColor = mainChart.ChartAreas[0].CursorY.LineColor = _sett.LineColor;

			mainChart.Series[0]["PriceUpColor"] = _sett.CandleUpColor.ToArgb().ToString();
			mainChart.Series[0]["PriceDownColor"] = _sett.CandleDownColor.ToArgb().ToString();

		}
		private void ChartInit(JObject json) {

			mainChart.Series[0].Points.Clear();

			if(json != null && json["Response"]?.ToString().ToLower() != "error") {
				
				double min_bounds = double.MaxValue, max_bounds = double.MinValue;
				foreach(JToken jtok in json["Data"]) {

					DataPoint dp = new DataPoint();

					double high  = jtok["high"].ToObject<double>();
					double low   = jtok["low"].ToObject<double>();
					double open  = jtok["open"].ToObject<double>();
					double close = jtok["close"].ToObject<double>();

					dp.YValues = new double[] { high, low, open, close };
					dp.AxisLabel = Epoch.AddSeconds(jtok["time"].ToObject<UInt64>()).ToString();
					dp.Color = close >= open ? _sett.CandleUpColor : _sett.CandleDownColor;
					mainChart.Series[0].Points.Add(dp);

					min_bounds = Math.Min(min_bounds, low);
					max_bounds = Math.Max(max_bounds, high);
				}

				double limit_bounds = (max_bounds - min_bounds) * 0.1;
				mainChart.ChartAreas[0].AxisY.Minimum = min_bounds - limit_bounds;
				mainChart.ChartAreas[0].AxisY.Maximum = max_bounds + limit_bounds;
			}

		}


		public FormChart(Settings.StCoin coin) {

			InitializeComponent();
			_coin = coin;

			Load += (sender, e) => {

				SetColors();

				mainChart.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
				mainChart.ChartAreas[0].AxisX.Interval = 4;
				mainChart.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
				//mainChart.ChartAreas[0].AxisY.Interval = 4;
				//mainChart.Series[0].BorderWidth = 1;
				//mainChart.ChartAreas[0].AxisY.LabelStyle.Interval = 1000;
				mainChart.ChartAreas[0].AxisY.LabelStyle.Font = new Font(new FontFamily("Microsoft Sans Serif"), 8);
				 

				try {

					JObject json = CCRequest.HttpRequest(CCRequest.HistoQuery(_coin, CCRequest.HistoType.Hour, 120, 3));
					ChartInit(json);

				} catch { }

				mainChart.MouseDown += Global.DragMove;

			};

		}


		private void toolStripClose_Click(object sender, EventArgs e) {
			Close();
		}

		private void mainChart_MouseMove(object sender, MouseEventArgs e) {

			try {

				mainChart.ChartAreas[0].CursorX.SetCursorPixelPosition(e.Location, true);
				mainChart.ChartAreas[0].CursorY.SetCursorPixelPosition(e.Location, true);

				int pt_index = (int)Math.Round(mainChart.ChartAreas[0].AxisX.PixelPositionToValue(e.X));
				if(pt_index < mainChart.Series[0].Points.Count) {

					DataPoint dp = mainChart.Series[0].Points[pt_index];

					labelValueVal.Text = mainChart.ChartAreas[0].AxisY.PixelPositionToValue(e.Y).ToString().Substring(0, 12);
					labelTimeVal.Text = dp.AxisLabel;

					labelHighVal.Text = dp.YValues[0].ToString();
					labelLowVal.Text = dp.YValues[1].ToString();
					labelOpenVal.Text = dp.YValues[2].ToString();
					labelCloseVal.Text = dp.YValues[3].ToString();

					labelHighVal.ForeColor = dp.Color;
					labelLowVal.ForeColor = dp.Color;
					labelOpenVal.ForeColor = dp.Color;
					labelCloseVal.ForeColor = dp.Color;
				}

			} catch { }

			Refresh();
			
		}

	}

}
