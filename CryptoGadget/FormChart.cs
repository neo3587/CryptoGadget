using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.VisualStyles;

using Newtonsoft.Json.Linq;



namespace CryptoGadget {

	public partial class FormChart : Form {

		private Series SerieCandles { get => mainChart.Series[0]; set => mainChart.Series[0] = value; }
		private ChartArea ChartAreaCandles { get => mainChart.ChartAreas[0]; set => mainChart.ChartAreas[0] = value; }

		private Settings.StChart _sett = new Settings.StChart();
		private (string coin, string target) _pair = ("", "");
		private bool _chart_clicked = false; // avoids DragMove until left-click is released if chart area was clicked
		private (int begin, int end, int last) _axis_x = (0, 0, 0); // (shown_first, shown_last, last_clicked)
		private bool _data_remaining = true;
		private (CCRequest.HistoType type, int step) _req_format = (CCRequest.HistoType.Minute, 20);
		private List<DataPoint> _serie_data = new List<DataPoint>();
		private (double min, double max) _serie_bounds = (0.0, 0.0); // shown values only


		private const int X_LEFT = 55, X_RIGHT = 25, Y_UP = 10, Y_DOWN = 90, XY_TICK = 7;
		private readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);


		internal void ApplySettings() {

			mainChart.Paint -= mainChart_Paint;

			Settings tmp_sett = new Settings();
			Global.Sett.CloneTo(tmp_sett);
			_sett = tmp_sett.Chart;

			SetColors();
			labelMin.Visible = labelMax.Visible = _sett.ShowMinMax;
			if(_sett.ShowMinMax) 
				mainChart.Paint += mainChart_Paint;

		}


		private void RefreshMinMax() {
			(double min, double max) tmp = (double.MaxValue, double.MinValue);
			foreach(DataPoint dp in SerieCandles.Points) 
				tmp = (Math.Min(tmp.min, dp.YValues[1]), Math.Max(tmp.max, dp.YValues[0]));
			_serie_bounds = tmp;
			labelMin.Text = _serie_bounds.min.ToString();
			labelMax.Text = _serie_bounds.max.ToString();
		}

		private void ButtonColorClick(object sender, EventArgs e) {
			Global.ControlApply<Button>(this, (ctrl) => {
				ctrl.ForeColor = _sett.Color.ForeGround;
				ctrl.BackColor = _sett.Color.BackGround;
				ctrl.Tag = false;
			});
			(sender as Button).ForeColor = _sett.Color.BackGround;
			(sender as Button).BackColor = _sett.Color.ForeGround;
			(sender as Button).Tag = true; // avoid color inversion on recoloring from FormSettings
		}
		private void SetColors() {

			ChartAreaCandles.AxisX.LineColor = ChartAreaCandles.AxisY.LineColor = _sett.Color.ForeGround;
			ChartAreaCandles.AxisX.LabelStyle.ForeColor = ChartAreaCandles.AxisY.LabelStyle.ForeColor = _sett.Color.ForeGround;
			
			BackColor = _sett.Color.BackGround;
			ChartAreaCandles.BackColor = mainChart.BackColor = _sett.Color.BackGround;

			Global.ControlApply<Label>(this, (ctrl) => {
				ctrl.ForeColor = _sett.Color.ForeGround;
				ctrl.BackColor = _sett.Color.BackGround;
			});
			Global.ControlApply<Button>(this, (ctrl) => {
				ctrl.ForeColor = (ctrl.Tag is bool && (bool)ctrl.Tag == false) ? _sett.Color.ForeGround : _sett.Color.BackGround;
				ctrl.BackColor = (ctrl.Tag is bool && (bool)ctrl.Tag == false) ? _sett.Color.BackGround : _sett.Color.ForeGround;
			});

			ChartAreaCandles.AxisX.MajorGrid.LineColor = ChartAreaCandles.AxisY.MajorGrid.LineColor = _sett.Color.Grid;
			ChartAreaCandles.AxisX.MajorTickMark.LineColor = ChartAreaCandles.AxisY.MajorTickMark.LineColor = _sett.Color.Grid;

			ChartAreaCandles.CursorX.LineColor = ChartAreaCandles.CursorY.LineColor = _sett.Color.CursorLines;

			comboStep.ForeColor = numStep.ForeColor = _sett.Color.ForeGround;
			comboStep.BackColor = numStep.BackColor = _sett.Color.BackGround;

			foreach(DataPoint dp in _serie_data)
				dp.BackSecondaryColor = dp.Color = dp.YValues[3] >= dp.YValues[2] ? _sett.Color.CandleUp : _sett.Color.CandleDown;
			
		}
		
		private DataPoint GenerateDataPoint(JToken jtok) {

			DataPoint dp = new DataPoint();

			double high = jtok["high"].ToObject<double>();
			double low = jtok["low"].ToObject<double>();
			double open = jtok["open"].ToObject<double>();
			double close = jtok["close"].ToObject<double>();

			dp.YValues = new double[] { high, low, open, close };
			dp.Tag = jtok["time"].ToObject<Int64>();
			DateTime time = Epoch.AddSeconds(jtok["time"].ToObject<Int64>());
			dp.AxisLabel = time.ToShortDateString() + "\n" + time.TimeOfDay;
			dp.BackSecondaryColor = dp.Color = close >= open ? _sett.Color.CandleUp : _sett.Color.CandleDown;

			return dp;
		}
		private void TryFetchData() { // fill the chart with extra data only if possible

			if(_axis_x.begin == 0 && _data_remaining) {
				try {
					labelError.Text = "Fetching Data";
					labelError.Update();

					JArray jarr = CCRequest.HttpRequest(CCRequest.HistoQuery(_pair.coin, _pair.target, _req_format.type, 1000, _req_format.step, (Int64)_serie_data[0].Tag), new System.Net.WebClient(), false)["Data"].ToObject<JArray>();

					for(int i = 0; i < jarr.Count; i++)
						_serie_data.Insert(i, GenerateDataPoint(jarr[i]));

					_axis_x.begin += jarr.Count; _axis_x.end += jarr.Count;
					_data_remaining = jarr.Count != 0 && (Int64)_serie_data[0].Tag > 1230768000; // avoid < 01/01/2009 dates (since cryptos didn't even exists)

					labelError.Text = _data_remaining ? "" : "All possible data fetched";
				} catch {
					labelError.Text = "ERROR: Can't connect with CryptoCompare";
				}
			}

		}
		private void ChartFill(CCRequest.HistoType type, int step, Int64 time = -1) {

			try {
				labelError.Text = "Fetching Data";
				labelError.Update();

				JArray jarr = CCRequest.HttpRequest(CCRequest.HistoQuery(_pair.coin, _pair.target, type, 120, step, time))["Data"].ToObject<JArray>();

				_req_format = (type, step);
				_data_remaining = true;
				SerieCandles.Points.Clear();
				_serie_data.Clear();

				for(int i = 0; i < jarr.Count; i++) 
					_serie_data.Add(GenerateDataPoint(jarr[i]));

				_axis_x.begin = Math.Max(_serie_data.Count - 80, 0);
				_axis_x.end = _serie_data.Count;

				for(int i = _axis_x.begin; i < _axis_x.end; i++) 
					SerieCandles.Points.Add(_serie_data[i]);

				ChartAreaCandles.AxisX.Interval = SerieCandles.Points.Count / 13;
				RefreshMinMax();

				labelError.Text = "";
			} catch {
				labelError.Text = "ERROR: Can't connect with CryptoCompare";
			}

			Update();
		}

		
		public FormChart(string coin, string target) {

			InitializeComponent();
			_pair = (coin, target);
			
			Load += (sender, e) => {

				DoubleBuffered = true;
				Text = labelPair.Text = _pair.coin + " → " + _pair.target;

				ApplySettings();
				
				ChartAreaCandles.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
				ChartAreaCandles.AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
				ChartAreaCandles.AxisX.Interval = 4;
				ChartAreaCandles.AxisX.LabelStyle.Font = new Font(new FontFamily("Microsoft Sans Serif"), 8);
				ChartAreaCandles.AxisY.LabelStyle.Font = new Font(new FontFamily("Microsoft Sans Serif"), 8);
				ChartAreaCandles.CursorY.Interval = 0;
				ChartAreaCandles.AxisY.IsStartedFromZero = false;

				ChartAreaCandles.Position = new ElementPosition() {
					Auto = false,
					Height = 100,
					Width = 100,
					X = 0,
					Y = 0
				};
				ChartAreaCandles.AxisX.LabelStyle.Angle = -90;
				ChartAreaCandles.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.None;

				comboStep.Click += Global.DropDownOnClick;
				comboStep.KeyPress += Global.DropDownOnKeyPress;
				MouseDown += Global.DragMove;
				Global.ControlApply<Label>(this, (ctrl) => ctrl.MouseDown += Global.DragMove);
				Global.ControlApply<Button>(this, (ctrl) => ctrl.GotFocus += (f_sender, f_e) => ActiveControl = mainChart);
				
				mainChart.MouseWheel += mainChart_MouseWheel;

				button30d.Click += ButtonColorClick;
				button7d.Click += ButtonColorClick;
				button3d.Click += ButtonColorClick;
				button24h.Click += ButtonColorClick;
				button6h.Click += ButtonColorClick;
				button1h.Click += ButtonColorClick;
				button20m.Click += ButtonColorClick;
				button5m.Click += ButtonColorClick;
				button1m.Click += ButtonColorClick;

				switch(_sett.DefaultStep) {
					case 0: button1m.PerformClick(); break;
					case 1: button5m.PerformClick(); break;
					case 2: button20m.PerformClick(); break;
					case 3: button1h.PerformClick(); break;
					case 4: button6h.PerformClick(); break;
					case 5: button24h.PerformClick(); break;
					case 6: button3d.PerformClick(); break;
					case 7: button7d.PerformClick(); break;
					default: button30d.PerformClick(); break;
				}
				
			};

		}


		private void toolStripCustomStepping_Click(object sender, EventArgs e) {
			foreach(Control ctrl in new Control[] { button1m, button5m, button20m, button1h, button6h, button24h, button3d, button7d, button30d })
				ctrl.Visible = toolStripCustomStepping.Checked;
			foreach(Control ctrl in new Control[] { comboStep, numStep, buttonApplyStep })
				ctrl.Visible = !toolStripCustomStepping.Checked;
			toolStripCustomStepping.Checked = !toolStripCustomStepping.Checked;
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
					double axis_x = ChartAreaCandles.AxisX.PixelPositionToValue(e.X);
					double axis_y = ChartAreaCandles.AxisY.PixelPositionToValue(e.Y);
					if(axis_x >= ChartAreaCandles.AxisX.Minimum && axis_x <= ChartAreaCandles.AxisX.Maximum && axis_y >= ChartAreaCandles.AxisY.Minimum && axis_y <= ChartAreaCandles.AxisY.Maximum) {

						_chart_clicked = true;

						if(_axis_x.last < (int)axis_x) { // shift left
							TryFetchData();
							for(int i = _axis_x.last; i < (int)axis_x &&_axis_x.begin > 0; i++) { 
								_axis_x.begin--; _axis_x.end--;
								SerieCandles.Points.RemoveAt(SerieCandles.Points.Count - 1);
								SerieCandles.Points.Insert(0, _serie_data[_axis_x.begin]);
							}
							ChartAreaCandles.RecalculateAxesScale();
						}
						else if(_axis_x.last > (int)axis_x) { // shift right
							for(int i = (int)axis_x; i < _axis_x.last && _axis_x.end < _serie_data.Count; i++) {
								_axis_x.begin++; _axis_x.end++;
								SerieCandles.Points.RemoveAt(0);
								SerieCandles.Points.Add(_serie_data[_axis_x.end - 1]);
							}
							ChartAreaCandles.RecalculateAxesScale();
						}

						RefreshMinMax();
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

				Point location = new Point((sender as Control).Location.X - mainChart.Location.X + e.X, (sender as Control).Location.Y - mainChart.Location.Y + e.Y); // just for MouseMove on labels
				ChartAreaCandles.CursorX.SetCursorPixelPosition(location, true);
				ChartAreaCandles.CursorY.SetCursorPixelPosition(location, true);

				double axis_x = ChartAreaCandles.AxisX.PixelPositionToValue(location.X);
				double axis_y = ChartAreaCandles.AxisY.PixelPositionToValue(location.Y);
				_axis_x.last = (int)axis_x;

				axis_x = Global.Constrain(axis_x, ChartAreaCandles.AxisX.Minimum, ChartAreaCandles.AxisX.Maximum); 
				axis_y = Global.Constrain(axis_y, ChartAreaCandles.AxisY.Minimum, ChartAreaCandles.AxisY.Maximum);

				int index_x = Global.Constrain((int)Math.Round(axis_x) - 1, 0, SerieCandles.Points.Count - 1);
				DataPoint dp = SerieCandles.Points[index_x];

				labelValue.Text = axis_y.ToString("0.000000000").Substring(0, 10); 
				labelTime.Text = Epoch.AddSeconds((Int64)dp.Tag).ToString(); 

				labelHigh.Text	= dp.YValues[0].ToString();
				labelLow.Text	= dp.YValues[1].ToString();
				labelOpen.Text	= dp.YValues[2].ToString();
				labelClose.Text	= dp.YValues[3].ToString();
				
				labelHigh.ForeColor	= labelLow.ForeColor = labelOpen.ForeColor = labelClose.ForeColor = dp.Color;

				labelValue.Visible = labelTime.Visible = true;
				labelValue.Location = new Point(X_LEFT - labelValue.Width + mainChart.Location.X, 
												(int)ChartAreaCandles.AxisY.ValueToPixelPosition(axis_y) + labelValue.Height / 2);
				labelTime.Location  = new Point(Global.Constrain((int)ChartAreaCandles.AxisX.ValueToPixelPosition(index_x) - labelTime.Width / 2 + 22, X_LEFT + mainChart.Location.X - 1, Size.Width - labelTime.Width), 
												mainChart.Height - Y_DOWN + 7 + labelTime.Height);
				labelValue.BringToFront();
			} catch { }

			Update();

		}
		private void mainChart_MouseWheel(object sender, MouseEventArgs e) {

			Func<int, int> WorkLeft = (n_steps) => {
				TryFetchData();
				for(int i = 0; i < n_steps; i++) {
					if(e.Delta > 0 && _axis_x.end - _axis_x.begin > 40) { // Zoom in
						_axis_x.begin++;
						SerieCandles.Points.RemoveAt(0);
					}
					else if(e.Delta < 0 && _axis_x.begin > 0) { // Zoom out
						_axis_x.begin--;
						SerieCandles.Points.Insert(0, _serie_data[_axis_x.begin]);
					}
					else {
						return n_steps - i;
					}
				}
				return 0;
			};
			Func<int, int> WorkRight = (n_steps) => {
				TryFetchData();
				for(int i = 0; i < n_steps; i++) {
					if(e.Delta > 0 && _axis_x.end - _axis_x.begin > 40) { // Zoom in
						_axis_x.end--;
						SerieCandles.Points.RemoveAt(SerieCandles.Points.Count - 1);
					}
					else if(e.Delta < 0 && _axis_x.end < _serie_data.Count) { // Zoom out
						_axis_x.end++;
						SerieCandles.Points.Add(_serie_data[_axis_x.end - 1]);
					}
					else {
						return n_steps - i;
					}
				}
				return 0;
			};

			Global.SuspendDrawing(mainChart);

			int steps = SerieCandles.Points.Count / 15; // asimetric zoom
			int steps_left = (int)Math.Round(steps * (float)(e.X -  X_LEFT) / (Size.Width - X_RIGHT - X_LEFT)); // zoom % on left/right depending on mouse position
			int steps_right = steps - steps_left;

			WorkRight(WorkLeft(steps_left));
			WorkLeft(WorkRight(steps_right));

			Global.ResumeDrawing(mainChart);

			ChartAreaCandles.AxisX.Interval = SerieCandles.Points.Count / 13;
			RefreshMinMax();
			mainChart_MouseMove(sender, e);
		}

		private void FormChart_Resize(object sender, EventArgs e) {
			// Keep pixel position of chart axes
			ChartAreaCandles.InnerPlotPosition.Height = (1 - (float)Y_DOWN / mainChart.Size.Height) * 100;
			ChartAreaCandles.InnerPlotPosition.Width = (1 - (float)X_RIGHT / mainChart.Size.Width) * 100;
			ChartAreaCandles.InnerPlotPosition.X = ((float)X_LEFT / mainChart.Size.Width) * 100;
			ChartAreaCandles.InnerPlotPosition.Y = ((float)Y_UP / mainChart.Size.Height) * 100;
			ChartAreaCandles.AxisY.MajorTickMark.Size = ((float)XY_TICK / mainChart.Size.Width) * 100;
			ChartAreaCandles.AxisX.MajorTickMark.Size = ((float)XY_TICK / mainChart.Size.Width) * 100;

			ChartAreaCandles.CursorX.Position = ChartAreaCandles.CursorY.Position = double.PositiveInfinity;
			labelValue.Visible = labelTime.Visible = false;

			Refresh(); // avoid resize gripper graphic glitches 
		}

		private void button30d_Click(object sender, EventArgs e) {
			ChartFill(CCRequest.HistoType.Day, 30);
		}
		private void button7d_Click(object sender, EventArgs e) {
			ChartFill(CCRequest.HistoType.Day, 7);
		}
		private void button3d_Click(object sender, EventArgs e) {
			ChartFill(CCRequest.HistoType.Day, 3);
		}
		private void button24h_Click(object sender, EventArgs e) {
			ChartFill(CCRequest.HistoType.Hour, 24);
		}
		private void button6h_Click(object sender, EventArgs e) {
			ChartFill(CCRequest.HistoType.Hour, 6);
		}
		private void button1h_Click(object sender, EventArgs e) {
			ChartFill(CCRequest.HistoType.Hour, 1);
		}
		private void button20m_Click(object sender, EventArgs e) {
			ChartFill(CCRequest.HistoType.Minute, 20);
		}
		private void button5m_Click(object sender, EventArgs e) {
			ChartFill(CCRequest.HistoType.Minute, 5);
		}
		private void button1m_Click(object sender, EventArgs e) {
			ChartFill(CCRequest.HistoType.Minute, 1);
		}

		private void buttonApplyStep_Click(object sender, EventArgs e) {
			CCRequest.HistoType htype = comboStep.SelectedIndex == 0 ? CCRequest.HistoType.Minute :
										comboStep.SelectedIndex == 1 ? CCRequest.HistoType.Hour : 
										CCRequest.HistoType.Day;
			ChartFill(htype, (int)numStep.Value);		
		}

		private void mainChart_Paint(object sender, PaintEventArgs e) {
			int px_min = (int)ChartAreaCandles.AxisY.ValueToPixelPosition(_serie_bounds.min);
			int px_max = (int)ChartAreaCandles.AxisY.ValueToPixelPosition(_serie_bounds.max);
			e.Graphics.DrawLine(new Pen(_sett.Color.MinMaxLines), new Point(X_LEFT, px_min), new Point(Width - X_RIGHT, px_min));
			e.Graphics.DrawLine(new Pen(_sett.Color.MinMaxLines), new Point(X_LEFT, px_max), new Point(Width - X_RIGHT, px_max));
			labelMin.Location = new Point(X_LEFT - labelMin.Width + mainChart.Location.X, px_min + labelMin.Height / 2);
			labelMax.Location = new Point(X_LEFT - labelMax.Width + mainChart.Location.X, px_max + labelMax.Height / 2);
		}
		private void FormChart_Paint(object sender, PaintEventArgs e) {
			if(VisualStyleRenderer.IsElementDefined(VisualStyleElement.Status.Gripper.Normal)) {
				VisualStyleRenderer renderer = new VisualStyleRenderer(VisualStyleElement.Status.Gripper.Normal);
				renderer.DrawBackground(e.Graphics, new Rectangle(Width - 18, Height - 20, 20, 20));
			}
		}

		// Override to get a sizable Borderless Form
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
