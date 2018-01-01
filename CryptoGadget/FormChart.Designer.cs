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
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripClose = new System.Windows.Forms.ToolStripMenuItem();
			this.mainChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.labelOpenText = new System.Windows.Forms.Label();
			this.labelHighText = new System.Windows.Forms.Label();
			this.labelLowText = new System.Windows.Forms.Label();
			this.labelCloseText = new System.Windows.Forms.Label();
			this.labelCloseval = new System.Windows.Forms.Label();
			this.labelLowVal = new System.Windows.Forms.Label();
			this.labelHighVal = new System.Windows.Forms.Label();
			this.labelOpenVal = new System.Windows.Forms.Label();
			this.labelValueVal = new System.Windows.Forms.Label();
			this.labelDateVal = new System.Windows.Forms.Label();
			this.labelDateText = new System.Windows.Forms.Label();
			this.labelValueText = new System.Windows.Forms.Label();
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
			chartArea1.Name = "ChartArea1";
			this.mainChart.ChartAreas.Add(chartArea1);
			this.mainChart.ContextMenuStrip = this.contextMenu;
			legend1.Name = "Legend1";
			this.mainChart.Legends.Add(legend1);
			this.mainChart.Location = new System.Drawing.Point(0, 0);
			this.mainChart.Name = "mainChart";
			series1.ChartArea = "ChartArea1";
			series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;
			series1.IsVisibleInLegend = false;
			series1.Legend = "Legend1";
			series1.Name = "Series1";
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
			this.labelOpenText.Location = new System.Drawing.Point(82, 0);
			this.labelOpenText.Name = "labelOpenText";
			this.labelOpenText.Size = new System.Drawing.Size(36, 13);
			this.labelOpenText.TabIndex = 2;
			this.labelOpenText.Text = "Open:";
			// 
			// labelHighText
			// 
			this.labelHighText.AutoSize = true;
			this.labelHighText.BackColor = System.Drawing.Color.Transparent;
			this.labelHighText.Location = new System.Drawing.Point(176, 0);
			this.labelHighText.Name = "labelHighText";
			this.labelHighText.Size = new System.Drawing.Size(32, 13);
			this.labelHighText.TabIndex = 3;
			this.labelHighText.Text = "High:";
			// 
			// labelLowText
			// 
			this.labelLowText.AutoSize = true;
			this.labelLowText.BackColor = System.Drawing.Color.Transparent;
			this.labelLowText.Location = new System.Drawing.Point(266, 0);
			this.labelLowText.Name = "labelLowText";
			this.labelLowText.Size = new System.Drawing.Size(30, 13);
			this.labelLowText.TabIndex = 4;
			this.labelLowText.Text = "Low:";
			// 
			// labelCloseText
			// 
			this.labelCloseText.AutoSize = true;
			this.labelCloseText.BackColor = System.Drawing.Color.Transparent;
			this.labelCloseText.Location = new System.Drawing.Point(354, 0);
			this.labelCloseText.Name = "labelCloseText";
			this.labelCloseText.Size = new System.Drawing.Size(36, 13);
			this.labelCloseText.TabIndex = 5;
			this.labelCloseText.Text = "Close:";
			// 
			// labelCloseval
			// 
			this.labelCloseval.AutoSize = true;
			this.labelCloseval.Location = new System.Drawing.Point(386, 0);
			this.labelCloseval.Name = "labelCloseval";
			this.labelCloseval.Size = new System.Drawing.Size(28, 13);
			this.labelCloseval.TabIndex = 6;
			this.labelCloseval.Text = "0.00";
			// 
			// labelLowVal
			// 
			this.labelLowVal.AutoSize = true;
			this.labelLowVal.Location = new System.Drawing.Point(292, 0);
			this.labelLowVal.Name = "labelLowVal";
			this.labelLowVal.Size = new System.Drawing.Size(28, 13);
			this.labelLowVal.TabIndex = 7;
			this.labelLowVal.Text = "0.00";
			// 
			// labelHighVal
			// 
			this.labelHighVal.AutoSize = true;
			this.labelHighVal.Location = new System.Drawing.Point(204, 0);
			this.labelHighVal.Name = "labelHighVal";
			this.labelHighVal.Size = new System.Drawing.Size(28, 13);
			this.labelHighVal.TabIndex = 8;
			this.labelHighVal.Text = "0.00";
			// 
			// labelOpenVal
			// 
			this.labelOpenVal.AutoSize = true;
			this.labelOpenVal.Location = new System.Drawing.Point(114, 0);
			this.labelOpenVal.Name = "labelOpenVal";
			this.labelOpenVal.Size = new System.Drawing.Size(28, 13);
			this.labelOpenVal.TabIndex = 9;
			this.labelOpenVal.Text = "0.00";
			// 
			// labelValueVal
			// 
			this.labelValueVal.AutoSize = true;
			this.labelValueVal.Location = new System.Drawing.Point(481, 0);
			this.labelValueVal.Name = "labelValueVal";
			this.labelValueVal.Size = new System.Drawing.Size(28, 13);
			this.labelValueVal.TabIndex = 13;
			this.labelValueVal.Text = "0.00";
			// 
			// labelDateVal
			// 
			this.labelDateVal.AutoSize = true;
			this.labelDateVal.Location = new System.Drawing.Point(591, 0);
			this.labelDateVal.Name = "labelDateVal";
			this.labelDateVal.Size = new System.Drawing.Size(34, 13);
			this.labelDateVal.TabIndex = 12;
			this.labelDateVal.Text = "00:00";
			// 
			// labelDateText
			// 
			this.labelDateText.AutoSize = true;
			this.labelDateText.BackColor = System.Drawing.Color.Transparent;
			this.labelDateText.Location = new System.Drawing.Point(562, 0);
			this.labelDateText.Name = "labelDateText";
			this.labelDateText.Size = new System.Drawing.Size(33, 13);
			this.labelDateText.TabIndex = 11;
			this.labelDateText.Text = "Date:";
			// 
			// labelValueText
			// 
			this.labelValueText.AutoSize = true;
			this.labelValueText.BackColor = System.Drawing.Color.Transparent;
			this.labelValueText.Location = new System.Drawing.Point(448, 0);
			this.labelValueText.Name = "labelValueText";
			this.labelValueText.Size = new System.Drawing.Size(37, 13);
			this.labelValueText.TabIndex = 10;
			this.labelValueText.Text = "Value:";
			// 
			// FormChart
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.ClientSize = new System.Drawing.Size(716, 339);
			this.ContextMenuStrip = this.contextMenu;
			this.Controls.Add(this.labelValueVal);
			this.Controls.Add(this.labelDateVal);
			this.Controls.Add(this.labelDateText);
			this.Controls.Add(this.labelValueText);
			this.Controls.Add(this.labelOpenVal);
			this.Controls.Add(this.labelHighVal);
			this.Controls.Add(this.labelLowVal);
			this.Controls.Add(this.labelCloseval);
			this.Controls.Add(this.labelCloseText);
			this.Controls.Add(this.labelLowText);
			this.Controls.Add(this.labelHighText);
			this.Controls.Add(this.labelOpenText);
			this.Controls.Add(this.mainChart);
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
		private System.Windows.Forms.Label labelCloseval;
		private System.Windows.Forms.Label labelLowVal;
		private System.Windows.Forms.Label labelHighVal;
		private System.Windows.Forms.Label labelOpenVal;
		private System.Windows.Forms.Label labelValueVal;
		private System.Windows.Forms.Label labelDateVal;
		private System.Windows.Forms.Label labelDateText;
		private System.Windows.Forms.Label labelValueText;
	}
}