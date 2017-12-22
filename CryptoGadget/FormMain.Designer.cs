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
			this.mainGrid = new System.Windows.Forms.DataGridView();
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripPages = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSettings = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripHide = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripExit = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.mainGrid)).BeginInit();
			this.contextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainGrid
			// 
			this.mainGrid.AllowUserToAddRows = false;
			this.mainGrid.AllowUserToDeleteRows = false;
			this.mainGrid.AllowUserToResizeColumns = false;
			this.mainGrid.AllowUserToResizeRows = false;
			this.mainGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.mainGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			this.mainGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.mainGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HotTrack;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.mainGrid.DefaultCellStyle = dataGridViewCellStyle1;
			this.mainGrid.EnableHeadersVisualStyles = false;
			this.mainGrid.Location = new System.Drawing.Point(0, 0);
			this.mainGrid.MultiSelect = false;
			this.mainGrid.Name = "mainGrid";
			this.mainGrid.ReadOnly = true;
			this.mainGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.mainGrid.RowHeadersVisible = false;
			this.mainGrid.RowTemplate.ReadOnly = true;
			this.mainGrid.ShowCellErrors = false;
			this.mainGrid.ShowEditingIcon = false;
			this.mainGrid.ShowRowErrors = false;
			this.mainGrid.Size = new System.Drawing.Size(182, 22);
			this.mainGrid.TabIndex = 0;
			this.mainGrid.RowContextMenuStripNeeded += new System.Windows.Forms.DataGridViewRowContextMenuStripNeededEventHandler(this.mainGrid_RowContextMenuStripNeeded);
			this.mainGrid.SelectionChanged += new System.EventHandler(this.mainGrid_SelectionChanged);
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
            this.toolStripPages,
            this.toolStripSeparator1,
            this.toolStripSettings,
            this.toolStripHide,
            this.toolStripExit});
			this.contextMenu.Name = "contextMenu";
			this.contextMenu.Size = new System.Drawing.Size(117, 98);
			// 
			// toolStripPages
			// 
			this.toolStripPages.AccessibleName = "";
			this.toolStripPages.Name = "toolStripPages";
			this.toolStripPages.Size = new System.Drawing.Size(116, 22);
			this.toolStripPages.Tag = "";
			this.toolStripPages.Text = "Pages";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(113, 6);
			// 
			// toolStripSettings
			// 
			this.toolStripSettings.Name = "toolStripSettings";
			this.toolStripSettings.Size = new System.Drawing.Size(116, 22);
			this.toolStripSettings.Text = "Settings";
			this.toolStripSettings.Click += new System.EventHandler(this.contextMenuSettings_Click);
			// 
			// toolStripHide
			// 
			this.toolStripHide.Name = "toolStripHide";
			this.toolStripHide.Size = new System.Drawing.Size(116, 22);
			this.toolStripHide.Text = "Hide";
			this.toolStripHide.Click += new System.EventHandler(this.contextMenuHide_Click);
			// 
			// toolStripExit
			// 
			this.toolStripExit.Name = "toolStripExit";
			this.toolStripExit.Size = new System.Drawing.Size(116, 22);
			this.toolStripExit.Text = "Exit";
			this.toolStripExit.Click += new System.EventHandler(this.contextMenuExit_Click);
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(182, 22);
			this.ContextMenuStrip = this.contextMenu;
			this.Controls.Add(this.mainGrid);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormMain";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "CryptoGadget";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
			((System.ComponentModel.ISupportInitialize)(this.mainGrid)).EndInit();
			this.contextMenu.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView mainGrid;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip  contextMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripSettings;
        private System.Windows.Forms.ToolStripMenuItem toolStripHide;
        private System.Windows.Forms.ToolStripMenuItem toolStripExit;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem toolStripPages;
	}
}

