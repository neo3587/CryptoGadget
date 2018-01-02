namespace CryptoGadget {
	partial class FormChart {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormChart));
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripClose = new System.Windows.Forms.ToolStripMenuItem();
			this.mainChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.labelOpenText = new System.Windows.Forms.Label();
			this.labelHighText = new System.Windows.Forms.Label();
			this.labelLowText = new System.Windows.Forms.Label();
			this.labelCloseText = new System.Windows.Forms.Label();
			this.labelCloseVal = new System.Windows.Forms.Label();
			this.labelLowVal = new System.Windows.Forms.Label();
			this.labelHighVal = new System.Windows.Forms.Label();
			this.labelOpenVal = new System.Windows.Forms.Label();
			this.labelValueVal = new System.Windows.Forms.Label();
			this.labelTimeVal = new System.Windows.Forms.Label();
			this.labelTimeText = new System.Windows.Forms.Label();
			this.labelValueText = new System.Windows.Forms.Label();
			this.button1h = new System.Windows.Forms.Button();
			this.button6h = new System.Windows.Forms.Button();
			this.button1d = new System.Windows.Forms.Button();
			this.button3d = new System.Windows.Forms.Button();
			this.button7d = new System.Windows.Forms.Button();
			this.button1m = new System.Windows.Forms.Button();
			this.button3m = new System.Windows.Forms.Button();
			this.button1y = new System.Windows.Forms.Button();
			this.button3y = new System.Windows.Forms.Button();
			this.labelError = new System.Windows.Forms.Label();
			this.contextMenu.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.mainChart)).BeginInit();
			this.SuspendLayout();
			// 
			// contextMenu
			// 
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripClose});
			this.contextMenu.Name = "contextMenu";
			this.contextMenu.Size = new System.Drawing.Size(104, 26);
			// 
			// toolStripClose
			// 
			this.toolStripClose.Name = "toolStripClose";
			this.toolStripClose.Size = new System.Drawing.Size(103, 22);
			this.toolStripClose.Text = "Close";
			this.toolStripClose.Click += new System.EventHandler(this.toolStripClose_Click);
			// 
			// mainChart
			// 
			this.mainChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mainChart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.mainChart.BorderSkin.BackSecondaryColor = System.Drawing.Color.White;
			this.mainChart.BorderSkin.PageColor = System.Drawing.Color.Black;
			chartArea1.Name = "ChartAreaCandle";
			this.mainChart.ChartAreas.Add(chartArea1);
			this.mainChart.ContextMenuStrip = this.contextMenu;
			legend1.Name = "LegendCandle";
			this.mainChart.Legends.Add(legend1);
			this.mainChart.Location = new System.Drawing.Point(0, 0);
			this.mainChart.Name = "mainChart";
			series1.ChartArea = "ChartAreaCandle";
			series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;
			series1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
			series1.IsVisibleInLegend = false;
			series1.Legend = "LegendCandle";
			series1.Name = "SeriesCandle";
			series1.YValuesPerPoint = 4;
			this.mainChart.Series.Add(series1);
			this.mainChart.Size = new System.Drawing.Size(716, 339);
			this.mainChart.TabIndex = 1;
			this.mainChart.Text = "(no text)";
			this.mainChart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mainChart_MouseMove);
			// 
			// labelOpenText
			// 
			this.labelOpenText.AutoSize = true;
			this.labelOpenText.BackColor = System.Drawing.Color.Transparent;
			this.labelOpenText.Location = new System.Drawing.Point(7, 0);
			this.labelOpenText.Name = "labelOpenText";
			this.labelOpenText.Size = new System.Drawing.Size(36, 13);
			this.labelOpenText.TabIndex = 2;
			this.labelOpenText.Text = "Open:";
			// 
			// labelHighText
			// 
			this.labelHighText.AutoSize = true;
			this.labelHighText.BackColor = System.Drawing.Color.Transparent;
			this.labelHighText.Location = new System.Drawing.Point(101, 0);
			this.labelHighText.Name = "labelHighText";
			this.labelHighText.Size = new System.Drawing.Size(32, 13);
			this.labelHighText.TabIndex = 3;
			this.labelHighText.Text = "High:";
			// 
			// labelLowText
			// 
			this.labelLowText.AutoSize = true;
			this.labelLowText.BackColor = System.Drawing.Color.Transparent;
			this.labelLowText.Location = new System.Drawing.Point(191, 0);
			this.labelLowText.Name = "labelLowText";
			this.labelLowText.Size = new System.Drawing.Size(30, 13);
			this.labelLowText.TabIndex = 4;
			this.labelLowText.Text = "Low:";
			// 
			// labelCloseText
			// 
			this.labelCloseText.AutoSize = true;
			this.labelCloseText.BackColor = System.Drawing.Color.Transparent;
			this.labelCloseText.Location = new System.Drawing.Point(279, 0);
			this.labelCloseText.Name = "labelCloseText";
			this.labelCloseText.Size = new System.Drawing.Size(36, 13);
			this.labelCloseText.TabIndex = 5;
			this.labelCloseText.Text = "Close:";
			// 
			// labelCloseVal
			// 
			this.labelCloseVal.AutoSize = true;
			this.labelCloseVal.Location = new System.Drawing.Point(311, 0);
			this.labelCloseVal.Name = "labelCloseVal";
			this.labelCloseVal.Size = new System.Drawing.Size(28, 13);
			this.labelCloseVal.TabIndex = 6;
			this.labelCloseVal.Text = "0.00";
			// 
			// labelLowVal
			// 
			this.labelLowVal.AutoSize = true;
			this.labelLowVal.Location = new System.Drawing.Point(217, 0);
			this.labelLowVal.Name = "labelLowVal";
			this.labelLowVal.Size = new System.Drawing.Size(28, 13);
			this.labelLowVal.TabIndex = 7;
			this.labelLowVal.Text = "0.00";
			// 
			// labelHighVal
			// 
			this.labelHighVal.AutoSize = true;
			this.labelHighVal.Location = new System.Drawing.Point(129, 0);
			this.labelHighVal.Name = "labelHighVal";
			this.labelHighVal.Size = new System.Drawing.Size(28, 13);
			this.labelHighVal.TabIndex = 8;
			this.labelHighVal.Text = "0.00";
			// 
			// labelOpenVal
			// 
			this.labelOpenVal.AutoSize = true;
			this.labelOpenVal.Location = new System.Drawing.Point(39, 0);
			this.labelOpenVal.Name = "labelOpenVal";
			this.labelOpenVal.Size = new System.Drawing.Size(28, 13);
			this.labelOpenVal.TabIndex = 9;
			this.labelOpenVal.Text = "0.00";
			// 
			// labelValueVal
			// 
			this.labelValueVal.AutoSize = true;
			this.labelValueVal.Location = new System.Drawing.Point(406, 0);
			this.labelValueVal.Name = "labelValueVal";
			this.labelValueVal.Size = new System.Drawing.Size(28, 13);
			this.labelValueVal.TabIndex = 13;
			this.labelValueVal.Text = "0.00";
			// 
			// labelTimeVal
			// 
			this.labelTimeVal.AutoSize = true;
			this.labelTimeVal.Location = new System.Drawing.Point(516, 0);
			this.labelTimeVal.Name = "labelTimeVal";
			this.labelTimeVal.Size = new System.Drawing.Size(34, 13);
			this.labelTimeVal.TabIndex = 12;
			this.labelTimeVal.Text = "00:00";
			// 
			// labelTimeText
			// 
			this.labelTimeText.AutoSize = true;
			this.labelTimeText.BackColor = System.Drawing.Color.Transparent;
			this.labelTimeText.Location = new System.Drawing.Point(487, 0);
			this.labelTimeText.Name = "labelTimeText";
			this.labelTimeText.Size = new System.Drawing.Size(33, 13);
			this.labelTimeText.TabIndex = 11;
			this.labelTimeText.Text = "Time:";
			// 
			// labelValueText
			// 
			this.labelValueText.AutoSize = true;
			this.labelValueText.BackColor = System.Drawing.Color.Transparent;
			this.labelValueText.Location = new System.Drawing.Point(373, 0);
			this.labelValueText.Name = "labelValueText";
			this.labelValueText.Size = new System.Drawing.Size(37, 13);
			this.labelValueText.TabIndex = 10;
			this.labelValueText.Text = "Value:";
			// 
			// button1h
			// 
			this.button1h.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1h.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1h.Location = new System.Drawing.Point(259, 313);
			this.button1h.Name = "button1h";
			this.button1h.Size = new System.Drawing.Size(31, 23);
			this.button1h.TabIndex = 14;
			this.button1h.Text = "1h";
			this.button1h.UseVisualStyleBackColor = true;
			this.button1h.Click += new System.EventHandler(this.button1h_Click);
			// 
			// button6h
			// 
			this.button6h.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button6h.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button6h.Location = new System.Drawing.Point(227, 313);
			this.button6h.Name = "button6h";
			this.button6h.Size = new System.Drawing.Size(31, 23);
			this.button6h.TabIndex = 15;
			this.button6h.Text = "6h";
			this.button6h.UseVisualStyleBackColor = true;
			this.button6h.Click += new System.EventHandler(this.button6h_Click);
			// 
			// button1d
			// 
			this.button1d.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1d.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1d.Location = new System.Drawing.Point(195, 313);
			this.button1d.Name = "button1d";
			this.button1d.Size = new System.Drawing.Size(31, 23);
			this.button1d.TabIndex = 16;
			this.button1d.Text = "1d";
			this.button1d.UseVisualStyleBackColor = true;
			this.button1d.Click += new System.EventHandler(this.button1d_Click);
			// 
			// button3d
			// 
			this.button3d.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button3d.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button3d.Location = new System.Drawing.Point(163, 313);
			this.button3d.Name = "button3d";
			this.button3d.Size = new System.Drawing.Size(31, 23);
			this.button3d.TabIndex = 17;
			this.button3d.Text = "3d";
			this.button3d.UseVisualStyleBackColor = true;
			this.button3d.Click += new System.EventHandler(this.button3d_Click);
			// 
			// button7d
			// 
			this.button7d.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button7d.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button7d.Location = new System.Drawing.Point(131, 313);
			this.button7d.Name = "button7d";
			this.button7d.Size = new System.Drawing.Size(31, 23);
			this.button7d.TabIndex = 18;
			this.button7d.Text = "7d";
			this.button7d.UseVisualStyleBackColor = true;
			this.button7d.Click += new System.EventHandler(this.button7d_Click);
			// 
			// button1m
			// 
			this.button1m.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1m.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1m.Location = new System.Drawing.Point(99, 313);
			this.button1m.Name = "button1m";
			this.button1m.Size = new System.Drawing.Size(31, 23);
			this.button1m.TabIndex = 19;
			this.button1m.Text = "1m";
			this.button1m.UseVisualStyleBackColor = true;
			this.button1m.Click += new System.EventHandler(this.button1m_Click);
			// 
			// button3m
			// 
			this.button3m.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button3m.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button3m.Location = new System.Drawing.Point(67, 313);
			this.button3m.Name = "button3m";
			this.button3m.Size = new System.Drawing.Size(31, 23);
			this.button3m.TabIndex = 20;
			this.button3m.Text = "3m";
			this.button3m.UseVisualStyleBackColor = true;
			this.button3m.Click += new System.EventHandler(this.button3m_Click);
			// 
			// button1y
			// 
			this.button1y.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1y.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1y.Location = new System.Drawing.Point(35, 313);
			this.button1y.Name = "button1y";
			this.button1y.Size = new System.Drawing.Size(31, 23);
			this.button1y.TabIndex = 21;
			this.button1y.Text = "1y";
			this.button1y.UseVisualStyleBackColor = true;
			this.button1y.Click += new System.EventHandler(this.button1y_Click);
			// 
			// button3y
			// 
			this.button3y.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button3y.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button3y.Location = new System.Drawing.Point(3, 313);
			this.button3y.Name = "button3y";
			this.button3y.Size = new System.Drawing.Size(31, 23);
			this.button3y.TabIndex = 22;
			this.button3y.Text = "3y";
			this.button3y.UseVisualStyleBackColor = true;
			this.button3y.Click += new System.EventHandler(this.button3y_Click);
			// 
			// labelError
			// 
			this.labelError.AutoSize = true;
			this.labelError.BackColor = System.Drawing.Color.Transparent;
			this.labelError.Location = new System.Drawing.Point(306, 318);
			this.labelError.Name = "labelError";
			this.labelError.Size = new System.Drawing.Size(0, 13);
			this.labelError.TabIndex = 23;
			// 
			// FormChart
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.ClientSize = new System.Drawing.Size(716, 339);
			this.ContextMenuStrip = this.contextMenu;
			this.Controls.Add(this.labelError);
			this.Controls.Add(this.button3y);
			this.Controls.Add(this.button1y);
			this.Controls.Add(this.button3m);
			this.Controls.Add(this.button1m);
			this.Controls.Add(this.button7d);
			this.Controls.Add(this.button3d);
			this.Controls.Add(this.button1d);
			this.Controls.Add(this.button6h);
			this.Controls.Add(this.button1h);
			this.Controls.Add(this.labelValueVal);
			this.Controls.Add(this.labelTimeVal);
			this.Controls.Add(this.labelTimeText);
			this.Controls.Add(this.labelValueText);
			this.Controls.Add(this.labelOpenVal);
			this.Controls.Add(this.labelHighVal);
			this.Controls.Add(this.labelLowVal);
			this.Controls.Add(this.labelCloseVal);
			this.Controls.Add(this.labelCloseText);
			this.Controls.Add(this.labelLowText);
			this.Controls.Add(this.labelHighText);
			this.Controls.Add(this.labelOpenText);
			this.Controls.Add(this.mainChart);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormChart";
			this.Text = "FormChart";
			this.contextMenu.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.mainChart)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ContextMenuStrip contextMenu;
		private System.Windows.Forms.ToolStripMenuItem toolStripClose;
		private System.Windows.Forms.DataVisualization.Charting.Chart mainChart;
		private System.Windows.Forms.Label labelOpenText;
		private System.Windows.Forms.Label labelHighText;
		private System.Windows.Forms.Label labelLowText;
		private System.Windows.Forms.Label labelCloseText;
		private System.Windows.Forms.Label labelCloseVal;
		private System.Windows.Forms.Label labelLowVal;
		private System.Windows.Forms.Label labelHighVal;
		private System.Windows.Forms.Label labelOpenVal;
		private System.Windows.Forms.Label labelValueVal;
		private System.Windows.Forms.Label labelTimeVal;
		private System.Windows.Forms.Label labelTimeText;
		private System.Windows.Forms.Label labelValueText;
		private System.Windows.Forms.Button button1h;
		private System.Windows.Forms.Button button6h;
		private System.Windows.Forms.Button button1d;
		private System.Windows.Forms.Button button3d;
		private System.Windows.Forms.Button button7d;
		private System.Windows.Forms.Button button1m;
		private System.Windows.Forms.Button button3m;
		private System.Windows.Forms.Button button1y;
		private System.Windows.Forms.Button button3y;
		private System.Windows.Forms.Label labelError;
	}
}