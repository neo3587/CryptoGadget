namespace CryptoGadget {
    partial class FormMain {
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
			this.coinGrid = new System.Windows.Forms.DataGridView();
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.contextMenuSettings = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuHide = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuExit = new System.Windows.Forms.ToolStripMenuItem();
			this.coinGridImg = new System.Windows.Forms.DataGridViewImageColumn();
			this.coinGridName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.coinGridValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.coinGridChange24 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.coinGridChange24Pct = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.coinGrid)).BeginInit();
			this.contextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// coinGrid
			// 
			this.coinGrid.AllowUserToAddRows = false;
			this.coinGrid.AllowUserToDeleteRows = false;
			this.coinGrid.AllowUserToResizeColumns = false;
			this.coinGrid.AllowUserToResizeRows = false;
			this.coinGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.coinGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			this.coinGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.coinGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.coinGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.coinGridImg,
            this.coinGridName,
            this.coinGridValue,
            this.coinGridChange24,
            this.coinGridChange24Pct});
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HotTrack;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.coinGrid.DefaultCellStyle = dataGridViewCellStyle1;
			this.coinGrid.EnableHeadersVisualStyles = false;
			this.coinGrid.Location = new System.Drawing.Point(0, 0);
			this.coinGrid.MultiSelect = false;
			this.coinGrid.Name = "coinGrid";
			this.coinGrid.ReadOnly = true;
			this.coinGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.coinGrid.RowHeadersVisible = false;
			this.coinGrid.RowTemplate.ReadOnly = true;
			this.coinGrid.ShowCellErrors = false;
			this.coinGrid.ShowEditingIcon = false;
			this.coinGrid.ShowRowErrors = false;
			this.coinGrid.Size = new System.Drawing.Size(182, 22);
			this.coinGrid.TabIndex = 0;
			this.coinGrid.SelectionChanged += new System.EventHandler(this.coinGrid_SelectionChanged);
			// 
			// notifyIcon
			// 
			this.notifyIcon.ContextMenuStrip = this.contextMenu;
			this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
			this.notifyIcon.Text = "CryptoGadget";
			this.notifyIcon.Visible = true;
			this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
			// 
			// contextMenu
			// 
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextMenuSettings,
            this.contextMenuHide,
            this.contextMenuExit});
			this.contextMenu.Name = "contextMenu";
			this.contextMenu.Size = new System.Drawing.Size(117, 70);
			// 
			// contextMenuSettings
			// 
			this.contextMenuSettings.Name = "contextMenuSettings";
			this.contextMenuSettings.Size = new System.Drawing.Size(116, 22);
			this.contextMenuSettings.Text = "Settings";
			this.contextMenuSettings.Click += new System.EventHandler(this.contextMenuSettings_Click);
			// 
			// contextMenuHide
			// 
			this.contextMenuHide.Name = "contextMenuHide";
			this.contextMenuHide.Size = new System.Drawing.Size(116, 22);
			this.contextMenuHide.Text = "Hide";
			this.contextMenuHide.Click += new System.EventHandler(this.contextMenuHide_Click);
			// 
			// contextMenuExit
			// 
			this.contextMenuExit.Name = "contextMenuExit";
			this.contextMenuExit.Size = new System.Drawing.Size(116, 22);
			this.contextMenuExit.Text = "Exit";
			this.contextMenuExit.Click += new System.EventHandler(this.contextMenuExit_Click);
			// 
			// coinGridImg
			// 
			this.coinGridImg.FillWeight = 78.32401F;
			this.coinGridImg.HeaderText = "";
			this.coinGridImg.Name = "coinGridImg";
			this.coinGridImg.ReadOnly = true;
			this.coinGridImg.Width = 25;
			// 
			// coinGridName
			// 
			this.coinGridName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.coinGridName.FillWeight = 91.44361F;
			this.coinGridName.HeaderText = "Coin";
			this.coinGridName.MaxInputLength = 10;
			this.coinGridName.Name = "coinGridName";
			this.coinGridName.ReadOnly = true;
			this.coinGridName.Width = 44;
			// 
			// coinGridValue
			// 
			this.coinGridValue.FillWeight = 128.7096F;
			this.coinGridValue.HeaderText = "Value";
			this.coinGridValue.MaxInputLength = 10;
			this.coinGridValue.Name = "coinGridValue";
			this.coinGridValue.ReadOnly = true;
			this.coinGridValue.Width = 63;
			// 
			// coinGridChange24
			// 
			this.coinGridChange24.FillWeight = 101.5228F;
			this.coinGridChange24.HeaderText = "Change";
			this.coinGridChange24.MaxInputLength = 10;
			this.coinGridChange24.Name = "coinGridChange24";
			this.coinGridChange24.ReadOnly = true;
			this.coinGridChange24.Width = 50;
			// 
			// coinGridChange24Pct
			// 
			this.coinGridChange24Pct.HeaderText = "Change(%)";
			this.coinGridChange24Pct.Name = "coinGridChange24Pct";
			this.coinGridChange24Pct.ReadOnly = true;
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(182, 22);
			this.ContextMenuStrip = this.contextMenu;
			this.Controls.Add(this.coinGrid);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormMain";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "CryptoGadget";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
			((System.ComponentModel.ISupportInitialize)(this.coinGrid)).EndInit();
			this.contextMenu.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView coinGrid;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip  contextMenu;
        private System.Windows.Forms.ToolStripMenuItem contextMenuSettings;
        private System.Windows.Forms.ToolStripMenuItem contextMenuHide;
        private System.Windows.Forms.ToolStripMenuItem contextMenuExit;
		private System.Windows.Forms.DataGridViewImageColumn coinGridImg;
		private System.Windows.Forms.DataGridViewTextBoxColumn coinGridName;
		private System.Windows.Forms.DataGridViewTextBoxColumn coinGridValue;
		private System.Windows.Forms.DataGridViewTextBoxColumn coinGridChange24;
		private System.Windows.Forms.DataGridViewTextBoxColumn coinGridChange24Pct;
	}
}

