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
			this.contextMenuSettings = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuHide = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuExit = new System.Windows.Forms.ToolStripMenuItem();
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
        private System.Windows.Forms.ToolStripMenuItem contextMenuSettings;
        private System.Windows.Forms.ToolStripMenuItem contextMenuHide;
        private System.Windows.Forms.ToolStripMenuItem contextMenuExit;
	}
}

