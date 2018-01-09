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
			this.toolStripCustomStepping = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripClose = new System.Windows.Forms.ToolStripMenuItem();
			this.mainChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.labelClose = new System.Windows.Forms.Label();
			this.labelLow = new System.Windows.Forms.Label();
			this.labelHigh = new System.Windows.Forms.Label();
			this.labelOpen = new System.Windows.Forms.Label();
			this.labelValue = new System.Windows.Forms.Label();
			this.labelTime = new System.Windows.Forms.Label();
			this.button1m = new System.Windows.Forms.Button();
			this.button5m = new System.Windows.Forms.Button();
			this.button20m = new System.Windows.Forms.Button();
			this.button1h = new System.Windows.Forms.Button();
			this.button6h = new System.Windows.Forms.Button();
			this.button24h = new System.Windows.Forms.Button();
			this.button3d = new System.Windows.Forms.Button();
			this.button7d = new System.Windows.Forms.Button();
			this.button30d = new System.Windows.Forms.Button();
			this.labelError = new System.Windows.Forms.Label();
			this.buttonClose = new System.Windows.Forms.Button();
			this.labelPair = new System.Windows.Forms.Label();
			this.buttonMinimize = new System.Windows.Forms.Button();
			this.buttonMaximize = new System.Windows.Forms.Button();
			this.numStep = new System.Windows.Forms.NumericUpDown();
			this.comboStep = new System.Windows.Forms.ComboBox();
			this.buttonApplyStep = new System.Windows.Forms.Button();
			this.contextMenu.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.mainChart)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numStep)).BeginInit();
			this.SuspendLayout();
			// 
			// contextMenu
			// 
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripCustomStepping,
            this.toolStripClose});
			this.contextMenu.Name = "contextMenu";
			this.contextMenu.Size = new System.Drawing.Size(167, 48);
			// 
			// toolStripCustomStepping
			// 
			this.toolStripCustomStepping.Name = "toolStripCustomStepping";
			this.toolStripCustomStepping.Size = new System.Drawing.Size(166, 22);
			this.toolStripCustomStepping.Text = "Custom Stepping";
			this.toolStripCustomStepping.Click += new System.EventHandler(this.toolStripCustomStepping_Click);
			// 
			// toolStripClose
			// 
			this.toolStripClose.Name = "toolStripClose";
			this.toolStripClose.Size = new System.Drawing.Size(166, 22);
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
			this.mainChart.Location = new System.Drawing.Point(12, 12);
			this.mainChart.Name = "mainChart";
			series1.ChartArea = "ChartAreaCandle";
			series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;
			series1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
			series1.IsVisibleInLegend = false;
			series1.Legend = "LegendCandle";
			series1.Name = "SeriesCandle";
			series1.YValuesPerPoint = 4;
			this.mainChart.Series.Add(series1);
			this.mainChart.Size = new System.Drawing.Size(658, 312);
			this.mainChart.TabIndex = 1;
			this.mainChart.Text = "(no text)";
			this.mainChart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mainChart_MouseMove);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Location = new System.Drawing.Point(7, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(36, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Open:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Location = new System.Drawing.Point(101, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(32, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "High:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.Location = new System.Drawing.Point(191, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(30, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Low:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.BackColor = System.Drawing.Color.Transparent;
			this.label4.Location = new System.Drawing.Point(279, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(36, 13);
			this.label4.TabIndex = 5;
			this.label4.Text = "Close:";
			// 
			// labelClose
			// 
			this.labelClose.AutoSize = true;
			this.labelClose.Location = new System.Drawing.Point(311, 0);
			this.labelClose.Name = "labelClose";
			this.labelClose.Size = new System.Drawing.Size(28, 13);
			this.labelClose.TabIndex = 6;
			this.labelClose.Text = "0.00";
			// 
			// labelLow
			// 
			this.labelLow.AutoSize = true;
			this.labelLow.Location = new System.Drawing.Point(217, 0);
			this.labelLow.Name = "labelLow";
			this.labelLow.Size = new System.Drawing.Size(28, 13);
			this.labelLow.TabIndex = 7;
			this.labelLow.Text = "0.00";
			// 
			// labelHigh
			// 
			this.labelHigh.AutoSize = true;
			this.labelHigh.Location = new System.Drawing.Point(129, 0);
			this.labelHigh.Name = "labelHigh";
			this.labelHigh.Size = new System.Drawing.Size(28, 13);
			this.labelHigh.TabIndex = 8;
			this.labelHigh.Text = "0.00";
			// 
			// labelOpen
			// 
			this.labelOpen.AutoSize = true;
			this.labelOpen.Location = new System.Drawing.Point(39, 0);
			this.labelOpen.Name = "labelOpen";
			this.labelOpen.Size = new System.Drawing.Size(28, 13);
			this.labelOpen.TabIndex = 9;
			this.labelOpen.Text = "0.00";
			// 
			// labelValue
			// 
			this.labelValue.AutoSize = true;
			this.labelValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelValue.Location = new System.Drawing.Point(543, 0);
			this.labelValue.Name = "labelValue";
			this.labelValue.Size = new System.Drawing.Size(30, 15);
			this.labelValue.TabIndex = 13;
			this.labelValue.Text = "0.00";
			this.labelValue.Visible = false;
			this.labelValue.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mainChart_MouseMove);
			// 
			// labelTime
			// 
			this.labelTime.AutoSize = true;
			this.labelTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelTime.Location = new System.Drawing.Point(579, 0);
			this.labelTime.Name = "labelTime";
			this.labelTime.Size = new System.Drawing.Size(36, 15);
			this.labelTime.TabIndex = 12;
			this.labelTime.Text = "00:00";
			this.labelTime.Visible = false;
			this.labelTime.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mainChart_MouseMove);
			// 
			// button1m
			// 
			this.button1m.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1m.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1m.Location = new System.Drawing.Point(305, 312);
			this.button1m.Name = "button1m";
			this.button1m.Size = new System.Drawing.Size(37, 23);
			this.button1m.TabIndex = 14;
			this.button1m.Text = "1m";
			this.button1m.UseVisualStyleBackColor = true;
			this.button1m.Click += new System.EventHandler(this.button1m_Click);
			// 
			// button5m
			// 
			this.button5m.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button5m.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button5m.Location = new System.Drawing.Point(267, 312);
			this.button5m.Name = "button5m";
			this.button5m.Size = new System.Drawing.Size(37, 23);
			this.button5m.TabIndex = 15;
			this.button5m.Text = "5m";
			this.button5m.UseVisualStyleBackColor = true;
			this.button5m.Click += new System.EventHandler(this.button5m_Click);
			// 
			// button20m
			// 
			this.button20m.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button20m.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button20m.Location = new System.Drawing.Point(229, 312);
			this.button20m.Name = "button20m";
			this.button20m.Size = new System.Drawing.Size(37, 23);
			this.button20m.TabIndex = 16;
			this.button20m.Text = "20m";
			this.button20m.UseVisualStyleBackColor = true;
			this.button20m.Click += new System.EventHandler(this.button20m_Click);
			// 
			// button1h
			// 
			this.button1h.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1h.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1h.Location = new System.Drawing.Point(191, 312);
			this.button1h.Name = "button1h";
			this.button1h.Size = new System.Drawing.Size(37, 23);
			this.button1h.TabIndex = 17;
			this.button1h.Text = "1h";
			this.button1h.UseVisualStyleBackColor = true;
			this.button1h.Click += new System.EventHandler(this.button1h_Click);
			// 
			// button6h
			// 
			this.button6h.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button6h.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button6h.Location = new System.Drawing.Point(153, 312);
			this.button6h.Name = "button6h";
			this.button6h.Size = new System.Drawing.Size(37, 23);
			this.button6h.TabIndex = 18;
			this.button6h.Text = "6h";
			this.button6h.UseVisualStyleBackColor = true;
			this.button6h.Click += new System.EventHandler(this.button6h_Click);
			// 
			// button24h
			// 
			this.button24h.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button24h.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button24h.Location = new System.Drawing.Point(115, 312);
			this.button24h.Name = "button24h";
			this.button24h.Size = new System.Drawing.Size(37, 23);
			this.button24h.TabIndex = 19;
			this.button24h.Text = "24h";
			this.button24h.UseVisualStyleBackColor = true;
			this.button24h.Click += new System.EventHandler(this.button1m_Click);
			// 
			// button3d
			// 
			this.button3d.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button3d.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button3d.Location = new System.Drawing.Point(77, 312);
			this.button3d.Name = "button3d";
			this.button3d.Size = new System.Drawing.Size(37, 23);
			this.button3d.TabIndex = 20;
			this.button3d.Text = "3d";
			this.button3d.UseVisualStyleBackColor = true;
			this.button3d.Click += new System.EventHandler(this.button3d_Click);
			// 
			// button7d
			// 
			this.button7d.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button7d.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button7d.Location = new System.Drawing.Point(39, 312);
			this.button7d.Name = "button7d";
			this.button7d.Size = new System.Drawing.Size(37, 23);
			this.button7d.TabIndex = 21;
			this.button7d.Text = "7d";
			this.button7d.UseVisualStyleBackColor = true;
			this.button7d.Click += new System.EventHandler(this.button7d_Click);
			// 
			// button30d
			// 
			this.button30d.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button30d.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button30d.Location = new System.Drawing.Point(1, 312);
			this.button30d.Name = "button30d";
			this.button30d.Size = new System.Drawing.Size(37, 23);
			this.button30d.TabIndex = 22;
			this.button30d.Text = "30d";
			this.button30d.UseVisualStyleBackColor = true;
			this.button30d.Click += new System.EventHandler(this.button30d_Click);
			// 
			// labelError
			// 
			this.labelError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelError.AutoSize = true;
			this.labelError.BackColor = System.Drawing.Color.Transparent;
			this.labelError.Location = new System.Drawing.Point(348, 315);
			this.labelError.Name = "labelError";
			this.labelError.Size = new System.Drawing.Size(0, 13);
			this.labelError.TabIndex = 23;
			// 
			// buttonClose
			// 
			this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonClose.Location = new System.Drawing.Point(663, 0);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.Size = new System.Drawing.Size(19, 19);
			this.buttonClose.TabIndex = 24;
			this.buttonClose.Text = "×";
			this.buttonClose.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonClose.UseCompatibleTextRendering = true;
			this.buttonClose.UseVisualStyleBackColor = true;
			this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
			// 
			// labelPair
			// 
			this.labelPair.AutoSize = true;
			this.labelPair.Location = new System.Drawing.Point(462, 0);
			this.labelPair.Name = "labelPair";
			this.labelPair.Size = new System.Drawing.Size(66, 13);
			this.labelPair.TabIndex = 25;
			this.labelPair.Text = "BTC -> USD";
			// 
			// buttonMinimize
			// 
			this.buttonMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonMinimize.Location = new System.Drawing.Point(627, 0);
			this.buttonMinimize.Name = "buttonMinimize";
			this.buttonMinimize.Size = new System.Drawing.Size(19, 19);
			this.buttonMinimize.TabIndex = 26;
			this.buttonMinimize.Text = "-";
			this.buttonMinimize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonMinimize.UseCompatibleTextRendering = true;
			this.buttonMinimize.UseVisualStyleBackColor = true;
			this.buttonMinimize.Click += new System.EventHandler(this.buttonMinimize_Click);
			// 
			// buttonMaximize
			// 
			this.buttonMaximize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonMaximize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonMaximize.Location = new System.Drawing.Point(645, 0);
			this.buttonMaximize.Name = "buttonMaximize";
			this.buttonMaximize.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.buttonMaximize.Size = new System.Drawing.Size(19, 19);
			this.buttonMaximize.TabIndex = 27;
			this.buttonMaximize.Text = "◻";
			this.buttonMaximize.UseCompatibleTextRendering = true;
			this.buttonMaximize.UseVisualStyleBackColor = true;
			this.buttonMaximize.Click += new System.EventHandler(this.buttonMaximize_Click);
			// 
			// numStep
			// 
			this.numStep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.numStep.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.numStep.Location = new System.Drawing.Point(99, 313);
			this.numStep.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
			this.numStep.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numStep.Name = "numStep";
			this.numStep.Size = new System.Drawing.Size(67, 20);
			this.numStep.TabIndex = 28;
			this.numStep.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numStep.Visible = false;
			// 
			// comboStep
			// 
			this.comboStep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.comboStep.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.comboStep.FormattingEnabled = true;
			this.comboStep.Items.AddRange(new object[] {
            "Minute",
            "Hour",
            "Day"});
			this.comboStep.Location = new System.Drawing.Point(2, 312);
			this.comboStep.Name = "comboStep";
			this.comboStep.Size = new System.Drawing.Size(91, 21);
			this.comboStep.TabIndex = 29;
			this.comboStep.Text = "Hour";
			this.comboStep.Visible = false;
			// 
			// buttonApplyStep
			// 
			this.buttonApplyStep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonApplyStep.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonApplyStep.Location = new System.Drawing.Point(172, 312);
			this.buttonApplyStep.Name = "buttonApplyStep";
			this.buttonApplyStep.Size = new System.Drawing.Size(49, 23);
			this.buttonApplyStep.TabIndex = 30;
			this.buttonApplyStep.Text = "Apply";
			this.buttonApplyStep.UseVisualStyleBackColor = true;
			this.buttonApplyStep.Visible = false;
			this.buttonApplyStep.Click += new System.EventHandler(this.buttonApplyStep_Click);
			// 
			// FormChart
			// 
			this.AcceptButton = this.buttonApplyStep;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.ClientSize = new System.Drawing.Size(682, 336);
			this.ContextMenuStrip = this.contextMenu;
			this.Controls.Add(this.buttonApplyStep);
			this.Controls.Add(this.comboStep);
			this.Controls.Add(this.numStep);
			this.Controls.Add(this.buttonMaximize);
			this.Controls.Add(this.buttonMinimize);
			this.Controls.Add(this.labelPair);
			this.Controls.Add(this.buttonClose);
			this.Controls.Add(this.labelError);
			this.Controls.Add(this.button30d);
			this.Controls.Add(this.button7d);
			this.Controls.Add(this.button3d);
			this.Controls.Add(this.button24h);
			this.Controls.Add(this.button6h);
			this.Controls.Add(this.button1h);
			this.Controls.Add(this.button20m);
			this.Controls.Add(this.button5m);
			this.Controls.Add(this.button1m);
			this.Controls.Add(this.labelValue);
			this.Controls.Add(this.labelTime);
			this.Controls.Add(this.labelOpen);
			this.Controls.Add(this.labelHigh);
			this.Controls.Add(this.labelLow);
			this.Controls.Add(this.labelClose);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.mainChart);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(682, 336);
			this.Name = "FormChart";
			this.Text = "FormChart";
			this.Resize += new System.EventHandler(this.FormChart_Resize);
			this.contextMenu.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.mainChart)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numStep)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ContextMenuStrip contextMenu;
		private System.Windows.Forms.ToolStripMenuItem toolStripClose;
		private System.Windows.Forms.DataVisualization.Charting.Chart mainChart;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label labelClose;
		private System.Windows.Forms.Label labelLow;
		private System.Windows.Forms.Label labelHigh;
		private System.Windows.Forms.Label labelOpen;
		private System.Windows.Forms.Label labelValue;
		private System.Windows.Forms.Label labelTime;
		private System.Windows.Forms.Button button1m;
		private System.Windows.Forms.Button button5m;
		private System.Windows.Forms.Button button20m;
		private System.Windows.Forms.Button button1h;
		private System.Windows.Forms.Button button6h;
		private System.Windows.Forms.Button button24h;
		private System.Windows.Forms.Button button3d;
		private System.Windows.Forms.Button button7d;
		private System.Windows.Forms.Button button30d;
		private System.Windows.Forms.Label labelError;
		private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.Label labelPair;
		private System.Windows.Forms.Button buttonMinimize;
		private System.Windows.Forms.Button buttonMaximize;
		private System.Windows.Forms.ToolStripMenuItem toolStripCustomStepping;
		private System.Windows.Forms.NumericUpDown numStep;
		private System.Windows.Forms.ComboBox comboStep;
		private System.Windows.Forms.Button buttonApplyStep;
	}
}