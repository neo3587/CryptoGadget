using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.VisualStyles;

using Newtonsoft.Json.Linq;



namespace CryptoGadget {

	public partial class FormChart : Form {

		// need to generate a ChartSettings Form... maybe at 2.4.0 version
		public class ChartSettings {
			public Color ForeColor { get; set; } = Color.FromArgb(200, 200, 200);
			public Color GridColor { get; set; } = Color.FromArgb(40, 52, 60);
			public Color BackColor { get; set; } = Color.FromArgb(27, 38, 45);
			public Color CursorLineColor { get; set; } = Color.FromArgb(120, 120, 120);
			public Color CandleUpColor { get; set; } = Color.FromArgb(106, 131, 58);
			public Color CandleDownColor { get; set; } = Color.FromArgb(138, 58, 59);
			//public int AxisXLength { get; set; } = 60;
			//public string DefaultZoom { get; set; } = "1d";
		}

		private ChartSettings _sett = new ChartSettings();
		private (string coin, string target) _conv = ("", "");
		private bool _chart_clicked = false; // this avois DragMove until left-click is released if chart area was clicked
		private (int begin, int end, int last) _axis_x = (0, 0, 0);
		private bool _data_remaining = true;
		private (CCRequest.HistoType type, int step) _req_format = (CCRequest.HistoType.Minute, 24);
		private JArray _serie_data = null;
		

		private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);


		private DataPoint GenerateDataPoint(JToken jtok) {

			DataPoint dp = new DataPoint();

			double high = jtok["high"].ToObject<double>();
			double low = jtok["low"].ToObject<double>();
			double open = jtok["open"].ToObject<double>();
			double close = jtok["close"].ToObject<double>();

			dp.YValues = new double[] { high, low, open, close };
			dp.Tag = jtok["time"].ToObject<Int64>();
			dp.AxisLabel = Epoch.AddSeconds(jtok["time"].ToObject<Int64>()).ToString();
			dp.BackSecondaryColor = dp.Color = close >= open ? _sett.CandleUpColor : _sett.CandleDownColor;

			return dp;
		}
		private void TryFetchData() { // fill the chart with extra data if possible
			if(_axis_x.begin == 0 && _data_remaining) { 
				try {
					labelError.Text = "Fetching Data";
					labelError.Update();
					JArray jarr = CCRequest.HttpRequest(CCRequest.HistoQuery(_conv.coin, _conv.target, _req_format.type, 120, _req_format.step, _serie_data[0]["time"].ToObject<Int64>()))["Data"].ToObject<JArray>();
					for(int i = 0; i < jarr.Count; i++)
						_serie_data.Insert(i, jarr[i]);
					_axis_x.begin += jarr.Count; _axis_x.end += jarr.Count;
					_data_remaining = jarr.Count != 0 && _serie_data[0]["time"].ToObject<Int64>() > 1230768000; // avoid < 01/01/2009 dates (since cryptos didn't even exists)
					labelError.Text = "";
					labelError.Update();
				} catch {
					labelError.Text = "ERROR: Can't connect with CryptoCompare API";
				}
			}
		}
		private void ButtonColorClick(object sender, EventArgs e) {
			ControlApply<Button>(this, (ctrl) => {
				ctrl.ForeColor = _sett.ForeColor;
				ctrl.BackColor = _sett.BackColor;
			});
			(sender as Button).ForeColor = _sett.BackColor;
			(sender as Button).BackColor = _sett.ForeColor;
		}
		private void ControlApply<T>(Control ctrl, Action<Control> fn) {
			if(ctrl is T) {
				fn(ctrl);
			}
			else {
				foreach(Control child in ctrl.Controls)
					ControlApply<T>(child, fn);
			}
		}
		private void SetColors() {

			mainChart.ChartAreas[0].AxisX.LineColor = mainChart.ChartAreas[0].AxisY.LineColor = _sett.ForeColor;
			mainChart.ChartAreas[0].AxisX.LabelStyle.ForeColor = mainChart.ChartAreas[0].AxisY.LabelStyle.ForeColor = _sett.ForeColor;

			BackColor = _sett.BackColor;
			mainChart.ChartAreas[0].BackColor = mainChart.BackColor = _sett.BackColor;

			ControlApply<Label>(this, (ctrl) => {
				ctrl.ForeColor = _sett.ForeColor;
				ctrl.BackColor = _sett.BackColor;
			});
			ControlApply<Button>(this, (ctrl) => {
				ctrl.ForeColor = _sett.ForeColor;
				ctrl.BackColor = _sett.BackColor;
			});

			mainChart.ChartAreas[0].AxisX.MajorGrid.LineColor = mainChart.ChartAreas[0].AxisY.MajorGrid.LineColor = _sett.GridColor;
			mainChart.ChartAreas[0].AxisX.MajorTickMark.LineColor = mainChart.ChartAreas[0].AxisY.MajorTickMark.LineColor = _sett.GridColor;

			mainChart.ChartAreas[0].CursorX.LineColor = mainChart.ChartAreas[0].CursorY.LineColor = _sett.CursorLineColor;

		}
		private void ChartFill(CCRequest.HistoType type, int step, Int64 time = -1) {
			
			try {

				labelError.Text = "Fetching Data";
				labelError.Update();

				_serie_data = CCRequest.HttpRequest(CCRequest.HistoQuery(_conv.coin, _conv.target, type, 120, step, time))["Data"].ToObject<JArray>();
				_req_format = (type, step);
				_data_remaining = true;
				mainChart.Series[0].Points.Clear();

				_axis_x.begin = Math.Max(_serie_data.Count - 60, 0);
				_axis_x.end   = Math.Min(_axis_x.begin + 60, _serie_data.Count);

				for(int i = _axis_x.begin; i < _axis_x.end; i++) 
					mainChart.Series[0].Points.Add(GenerateDataPoint(_serie_data[i]));

				labelError.Text = "";

			} catch {
				labelError.Text = "ERROR: Can't connect with CryptoCompare API";
			}

		}


		public FormChart(string coin, string target) {

			InitializeComponent();
			_conv.coin = coin;
			_conv.target = target;

			Load += (sender, e) => {

				DoubleBuffered = true;
				labelConv.Text = _conv.coin + " -> " + _conv.target;

				SetColors();

				mainChart.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
				mainChart.ChartAreas[0].AxisX.Interval = 4;
				mainChart.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
				mainChart.ChartAreas[0].AxisY.LabelStyle.Font = new Font(new FontFamily("Microsoft Sans Serif"), 8);
				mainChart.ChartAreas[0].CursorY.Interval = 0;
				mainChart.ChartAreas[0].AxisY.IsStartedFromZero = false;

				MouseDown += Global.DragMove;
				ControlApply<Label>(this, (ctrl) => ctrl.MouseDown += Global.DragMove);
				ControlApply<Button>(this, (ctrl) => ctrl.GotFocus += (f_sender, f_e) => ActiveControl = mainChart);

				mainChart.MouseWheel += mainChart_MouseWheel;

				button3y.Click += ButtonColorClick;
				button1y.Click += ButtonColorClick;
				button3m.Click += ButtonColorClick;
				button1m.Click += ButtonColorClick;
				button7d.Click += ButtonColorClick;
				button3d.Click += ButtonColorClick;
				button1d.Click += ButtonColorClick;
				button6h.Click += ButtonColorClick;
				button1h.Click += ButtonColorClick;

				button1d.PerformClick();

			};

		}


		private void toolStripClose_Click(object sender, EventArgs e) {
			Close();
		}
		private void buttonMaximize_Click(object sender, EventArgs e) {
			WindowState = WindowState == FormWindowState.Normal ? FormWindowState.Maximized : FormWindowState.Normal;
		}
		private void buttonMinimize_Click(object sender, EventArgs e) {
			WindowState = FormWindowState.Minimized;
		}
		private void buttonClose_Click(object sender, EventArgs e) {
			Close();
		}

		private void mainChart_MouseMove(object sender, MouseEventArgs e) {
			
			if(e.Button == MouseButtons.Left) {
				try {
					double axis_x = mainChart.ChartAreas[0].AxisX.PixelPositionToValue(e.X);
					double axis_y = mainChart.ChartAreas[0].AxisY.PixelPositionToValue(e.Y);
					if(axis_x >= mainChart.ChartAreas[0].AxisX.Minimum && axis_x <= mainChart.ChartAreas[0].AxisX.Maximum && axis_y >= mainChart.ChartAreas[0].AxisY.Minimum && axis_y <= mainChart.ChartAreas[0].AxisY.Maximum) {

						_chart_clicked = true;

						if(_axis_x.last < (int)axis_x) { // shift left
							TryFetchData();
							for(int i = _axis_x.last; i < (int)axis_x &&_axis_x.begin > 0; i++) { 
								_axis_x.begin--; _axis_x.end--;
								mainChart.Series[0].Points.RemoveAt(mainChart.Series[0].Points.Count - 1);
								mainChart.Series[0].Points.Insert(0, GenerateDataPoint(_serie_data[_axis_x.begin]));
							}
							mainChart.ChartAreas[0].RecalculateAxesScale();
						}
						else if(_axis_x.last > (int)axis_x) { // shift right
							for(int i = (int)axis_x; i < _axis_x.last && _axis_x.end < _serie_data.Count; i++) {
								_axis_x.begin++; _axis_x.end++;
								mainChart.Series[0].Points.RemoveAt(0);
								mainChart.Series[0].Points.Add(GenerateDataPoint(_serie_data[_axis_x.end - 1]));
							}
							mainChart.ChartAreas[0].RecalculateAxesScale();
						}

					}
					else if(!_chart_clicked) {
						Global.DragMove(sender, e);
						return;
					}
				} catch { }
			}
			else {
				_chart_clicked = false;
			}
			
			try {

				mainChart.ChartAreas[0].CursorX.SetCursorPixelPosition(e.Location, true);
				mainChart.ChartAreas[0].CursorY.SetCursorPixelPosition(e.Location, true);

				double axis_x = mainChart.ChartAreas[0].AxisX.PixelPositionToValue(e.X);
				_axis_x.last = (int)axis_x;
				int pt_index = (int)Math.Round(axis_x) - 1;
				if(pt_index < mainChart.Series[0].Points.Count && pt_index >= 0) {

					DataPoint dp = mainChart.Series[0].Points[pt_index];

					labelValueVal.Text = mainChart.ChartAreas[0].AxisY.PixelPositionToValue(e.Y).ToString().Substring(0, 13);
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

			Update();

		}
		private void mainChart_MouseWheel(object sender, MouseEventArgs e) {
			if(e.Delta > 0 && _axis_x.end - _axis_x.begin > 40) { // Zoom in
				_axis_x.begin++; _axis_x.end--;
				mainChart.Series[0].Points.RemoveAt(0);
				mainChart.Series[0].Points.RemoveAt(mainChart.Series[0].Points.Count -1);
			}
			else if(e.Delta < 0 && _axis_x.end - _axis_x.begin < 120) { // Zoom out
				TryFetchData();
				if(_axis_x.begin > 0) {
					_axis_x.begin--;
					mainChart.Series[0].Points.Insert(0, GenerateDataPoint(_serie_data[_axis_x.begin]));
				}
				if(_axis_x.end < _serie_data.Count) {
					_axis_x.end++;
					mainChart.Series[0].Points.Add(GenerateDataPoint(_serie_data[_axis_x.end - 1]));
				}
			}
		}

		private void FormChart_Resize(object sender, EventArgs e) {
			Refresh(); // avoid resize gripper graphic glitches 
		}

		private void button3y_Click(object sender, EventArgs e) {
			ChartFill(CCRequest.HistoType.Day, 15);
		}
		private void button1y_Click(object sender, EventArgs e) {
			ChartFill(CCRequest.HistoType.Day, 5);
		}
		private void button3m_Click(object sender, EventArgs e) {
			ChartFill(CCRequest.HistoType.Day, 2);
		}
		private void button1m_Click(object sender, EventArgs e) {
			ChartFill(CCRequest.HistoType.Hour, 10);
		}
		private void button7d_Click(object sender, EventArgs e) {
			ChartFill(CCRequest.HistoType.Hour, 3);
		}
		private void button3d_Click(object sender, EventArgs e) {
			ChartFill(CCRequest.HistoType.Hour, 1);
		}
		private void button1d_Click(object sender, EventArgs e) {
			ChartFill(CCRequest.HistoType.Minute, 24);
		}
		private void button6h_Click(object sender, EventArgs e) {
			ChartFill(CCRequest.HistoType.Minute, 6);
		}
		private void button1h_Click(object sender, EventArgs e) {
			ChartFill(CCRequest.HistoType.Minute, 1);
		}


		// Overrides to get a sizable Borderless Form
		protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint(e);
			if(VisualStyleRenderer.IsElementDefined(VisualStyleElement.Status.Gripper.Normal)) {
				VisualStyleRenderer renderer = new VisualStyleRenderer(VisualStyleElement.Status.Gripper.Normal);
				renderer.DrawBackground(e.Graphics, new Rectangle((Width) - 18, (Height) - 20, 20, 20));
			}
		}
		protected override void WndProc(ref Message m) {
			if(m.Msg == 0x84) {
				int x = (int)(m.LParam.ToInt64() & 0xFFFF);
				int y = (int)((m.LParam.ToInt64() & 0xFFFF0000) >> 16);
				Point pt = PointToClient(new Point(x, y));
				if(pt.X >= ClientSize.Width - 16 && pt.Y >= ClientSize.Height - 16 && ClientSize.Height >= 16) {
					m.Result = (IntPtr)(IsMirrored ? 16 : 17);
					return;
				}
			}
			base.WndProc(ref m);
		}

	}

}
