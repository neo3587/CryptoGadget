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
			this.mainChart.BorderSkin.BackSecondaryColor = System.Drawing.Color.White;
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
			this.mainChart.GetToolTipText += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs>(this.mainChart_GetToolTipText);
			// 
			// FormChart
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(716, 339);
			this.ContextMenuStrip = this.contextMenu;
			this.Controls.Add(this.mainChart);
			this.Name = "FormChart";
			this.Text = "FormChart";
			this.contextMenu.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.mainChart)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ContextMenuStrip contextMenu;
		private System.Windows.Forms.ToolStripMenuItem toolStripClose;
		private System.Windows.Forms.DataVisualization.Charting.Chart mainChart;
	}
}