namespace CryptoGadget {
    partial class FormPairSettings {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPairSettings));
			this.buttonAccept = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.comboCoin = new System.Windows.Forms.ComboBox();
			this.comboTarget = new System.Windows.Forms.ComboBox();
			this.checkTargetOnlyFiat = new System.Windows.Forms.CheckBox();
			this.checkCoinIndexName = new System.Windows.Forms.CheckBox();
			this.checkCoinOnlyFiat = new System.Windows.Forms.CheckBox();
			this.checkTargetIndexName = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.buttonIconSwap = new System.Windows.Forms.Button();
			this.buttonIconReDownload = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.buttonIconTargetSwap = new System.Windows.Forms.Button();
			this.buttonIconTargetReDownload = new System.Windows.Forms.Button();
			this.numAlertAbove = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBoxAlertAbove = new System.Windows.Forms.GroupBox();
			this.buttonRemoveAlertAbove = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.buttonAddAlertAbove = new System.Windows.Forms.Button();
			this.comboAlertAbove = new System.Windows.Forms.ComboBox();
			this.numAlertBelow = new System.Windows.Forms.NumericUpDown();
			this.groupBoxAlertBelow = new System.Windows.Forms.GroupBox();
			this.buttonRemoveAlertBelow = new System.Windows.Forms.Button();
			this.buttonAddAlertBelow = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.comboAlertBelow = new System.Windows.Forms.ComboBox();
			this.buttonSwitchAlertView = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numAlertAbove)).BeginInit();
			this.groupBoxAlertAbove.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numAlertBelow)).BeginInit();
			this.groupBoxAlertBelow.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonAccept
			// 
			this.buttonAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonAccept.Location = new System.Drawing.Point(328, 195);
			this.buttonAccept.Name = "buttonAccept";
			this.buttonAccept.Size = new System.Drawing.Size(75, 23);
			this.buttonAccept.TabIndex = 0;
			this.buttonAccept.Text = "Accept";
			this.buttonAccept.UseVisualStyleBackColor = true;
			this.buttonAccept.Click += new System.EventHandler(this.buttonAccept_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(409, 195);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 1;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// comboCoin
			// 
			this.comboCoin.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.comboCoin.Location = new System.Drawing.Point(6, 19);
			this.comboCoin.Name = "comboCoin";
			this.comboCoin.Size = new System.Drawing.Size(221, 21);
			this.comboCoin.Sorted = true;
			this.comboCoin.TabIndex = 2;
			// 
			// comboTarget
			// 
			this.comboTarget.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
			this.comboTarget.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.comboTarget.Location = new System.Drawing.Point(6, 19);
			this.comboTarget.Name = "comboTarget";
			this.comboTarget.Size = new System.Drawing.Size(221, 21);
			this.comboTarget.Sorted = true;
			this.comboTarget.TabIndex = 4;
			// 
			// checkTargetOnlyFiat
			// 
			this.checkTargetOnlyFiat.AutoSize = true;
			this.checkTargetOnlyFiat.Location = new System.Drawing.Point(111, 46);
			this.checkTargetOnlyFiat.Name = "checkTargetOnlyFiat";
			this.checkTargetOnlyFiat.Size = new System.Drawing.Size(120, 17);
			this.checkTargetOnlyFiat.TabIndex = 6;
			this.checkTargetOnlyFiat.Text = "Only Fiat Currencies";
			this.checkTargetOnlyFiat.UseVisualStyleBackColor = true;
			this.checkTargetOnlyFiat.CheckedChanged += new System.EventHandler(this.checkTargetOnlyFiat_CheckedChanged);
			// 
			// checkCoinIndexName
			// 
			this.checkCoinIndexName.AutoSize = true;
			this.checkCoinIndexName.Location = new System.Drawing.Point(7, 46);
			this.checkCoinIndexName.Name = "checkCoinIndexName";
			this.checkCoinIndexName.Size = new System.Drawing.Size(97, 17);
			this.checkCoinIndexName.TabIndex = 7;
			this.checkCoinIndexName.Text = "Index by Name";
			this.checkCoinIndexName.UseVisualStyleBackColor = true;
			this.checkCoinIndexName.CheckedChanged += new System.EventHandler(this.checkCoinIndexName_CheckedChanged);
			// 
			// checkCoinOnlyFiat
			// 
			this.checkCoinOnlyFiat.AutoSize = true;
			this.checkCoinOnlyFiat.Location = new System.Drawing.Point(111, 46);
			this.checkCoinOnlyFiat.Name = "checkCoinOnlyFiat";
			this.checkCoinOnlyFiat.Size = new System.Drawing.Size(120, 17);
			this.checkCoinOnlyFiat.TabIndex = 8;
			this.checkCoinOnlyFiat.Text = "Only Fiat Currencies";
			this.checkCoinOnlyFiat.UseVisualStyleBackColor = true;
			this.checkCoinOnlyFiat.CheckedChanged += new System.EventHandler(this.checkCoinOnlyFiat_CheckedChanged);
			// 
			// checkTargetIndexName
			// 
			this.checkTargetIndexName.AutoSize = true;
			this.checkTargetIndexName.Location = new System.Drawing.Point(7, 46);
			this.checkTargetIndexName.Name = "checkTargetIndexName";
			this.checkTargetIndexName.Size = new System.Drawing.Size(97, 17);
			this.checkTargetIndexName.TabIndex = 9;
			this.checkTargetIndexName.Text = "Index by Name";
			this.checkTargetIndexName.UseVisualStyleBackColor = true;
			this.checkTargetIndexName.CheckedChanged += new System.EventHandler(this.checkTargetIndexName_CheckedChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.comboCoin);
			this.groupBox1.Controls.Add(this.buttonIconSwap);
			this.groupBox1.Controls.Add(this.buttonIconReDownload);
			this.groupBox1.Controls.Add(this.checkCoinIndexName);
			this.groupBox1.Controls.Add(this.checkCoinOnlyFiat);
			this.groupBox1.Location = new System.Drawing.Point(12, 9);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(233, 97);
			this.groupBox1.TabIndex = 10;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Coin";
			// 
			// buttonIconSwap
			// 
			this.buttonIconSwap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonIconSwap.Location = new System.Drawing.Point(136, 67);
			this.buttonIconSwap.Name = "buttonIconSwap";
			this.buttonIconSwap.Size = new System.Drawing.Size(91, 23);
			this.buttonIconSwap.TabIndex = 12;
			this.buttonIconSwap.Text = "Swap Icon";
			this.buttonIconSwap.UseVisualStyleBackColor = true;
			this.buttonIconSwap.Click += new System.EventHandler(this.buttonIconSwap_Click);
			// 
			// buttonIconReDownload
			// 
			this.buttonIconReDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonIconReDownload.Location = new System.Drawing.Point(6, 67);
			this.buttonIconReDownload.Name = "buttonIconReDownload";
			this.buttonIconReDownload.Size = new System.Drawing.Size(124, 23);
			this.buttonIconReDownload.TabIndex = 13;
			this.buttonIconReDownload.Text = "Re-Download Icon";
			this.buttonIconReDownload.UseVisualStyleBackColor = true;
			this.buttonIconReDownload.Click += new System.EventHandler(this.buttonIconReDownload_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.buttonIconTargetSwap);
			this.groupBox2.Controls.Add(this.checkTargetIndexName);
			this.groupBox2.Controls.Add(this.buttonIconTargetReDownload);
			this.groupBox2.Controls.Add(this.checkTargetOnlyFiat);
			this.groupBox2.Controls.Add(this.comboTarget);
			this.groupBox2.Location = new System.Drawing.Point(251, 9);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(233, 97);
			this.groupBox2.TabIndex = 11;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Target Coin";
			// 
			// buttonIconTargetSwap
			// 
			this.buttonIconTargetSwap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonIconTargetSwap.Location = new System.Drawing.Point(136, 68);
			this.buttonIconTargetSwap.Name = "buttonIconTargetSwap";
			this.buttonIconTargetSwap.Size = new System.Drawing.Size(91, 23);
			this.buttonIconTargetSwap.TabIndex = 14;
			this.buttonIconTargetSwap.Text = "Swap Icon";
			this.buttonIconTargetSwap.UseVisualStyleBackColor = true;
			this.buttonIconTargetSwap.Click += new System.EventHandler(this.buttonIconTargetSwap_Click);
			// 
			// buttonIconTargetReDownload
			// 
			this.buttonIconTargetReDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonIconTargetReDownload.Location = new System.Drawing.Point(6, 67);
			this.buttonIconTargetReDownload.Name = "buttonIconTargetReDownload";
			this.buttonIconTargetReDownload.Size = new System.Drawing.Size(124, 23);
			this.buttonIconTargetReDownload.TabIndex = 15;
			this.buttonIconTargetReDownload.Text = "Re-Download Icon";
			this.buttonIconTargetReDownload.UseVisualStyleBackColor = true;
			this.buttonIconTargetReDownload.Click += new System.EventHandler(this.buttonIconTargetReDownload_Click);
			// 
			// numAlertAbove
			// 
			this.numAlertAbove.Increment = new decimal(new int[] {
            1,
            0,
            0,
            524288});
			this.numAlertAbove.Location = new System.Drawing.Point(49, 46);
			this.numAlertAbove.Maximum = new decimal(new int[] {
            -727379969,
            232,
            0,
            0});
			this.numAlertAbove.Name = "numAlertAbove";
			this.numAlertAbove.Size = new System.Drawing.Size(149, 20);
			this.numAlertAbove.TabIndex = 0;
			this.numAlertAbove.ValueChanged += new System.EventHandler(this.NumericUpDownTrim);
			this.numAlertAbove.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NumericUpDownDecSeparator);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(37, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Value:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(37, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Value:";
			// 
			// groupBoxAlertAbove
			// 
			this.groupBoxAlertAbove.Controls.Add(this.buttonRemoveAlertAbove);
			this.groupBoxAlertAbove.Controls.Add(this.label3);
			this.groupBoxAlertAbove.Controls.Add(this.buttonAddAlertAbove);
			this.groupBoxAlertAbove.Controls.Add(this.comboAlertAbove);
			this.groupBoxAlertAbove.Controls.Add(this.label1);
			this.groupBoxAlertAbove.Controls.Add(this.numAlertAbove);
			this.groupBoxAlertAbove.Location = new System.Drawing.Point(12, 112);
			this.groupBoxAlertAbove.Name = "groupBoxAlertAbove";
			this.groupBoxAlertAbove.Size = new System.Drawing.Size(233, 77);
			this.groupBoxAlertAbove.TabIndex = 14;
			this.groupBoxAlertAbove.TabStop = false;
			this.groupBoxAlertAbove.Text = "Alert Above";
			// 
			// buttonRemoveAlertAbove
			// 
			this.buttonRemoveAlertAbove.Location = new System.Drawing.Point(204, 45);
			this.buttonRemoveAlertAbove.Name = "buttonRemoveAlertAbove";
			this.buttonRemoveAlertAbove.Size = new System.Drawing.Size(23, 23);
			this.buttonRemoveAlertAbove.TabIndex = 12;
			this.buttonRemoveAlertAbove.Text = "-";
			this.buttonRemoveAlertAbove.UseVisualStyleBackColor = true;
			this.buttonRemoveAlertAbove.Click += new System.EventHandler(this.buttonRemoveAlertAbove_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(5, 22);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(40, 13);
			this.label3.TabIndex = 7;
			this.label3.Text = "Select:";
			// 
			// buttonAddAlertAbove
			// 
			this.buttonAddAlertAbove.Location = new System.Drawing.Point(204, 18);
			this.buttonAddAlertAbove.Name = "buttonAddAlertAbove";
			this.buttonAddAlertAbove.Size = new System.Drawing.Size(23, 23);
			this.buttonAddAlertAbove.TabIndex = 11;
			this.buttonAddAlertAbove.Text = "+";
			this.buttonAddAlertAbove.UseVisualStyleBackColor = true;
			this.buttonAddAlertAbove.Click += new System.EventHandler(this.buttonAddAlertAbove_Click);
			// 
			// comboAlertAbove
			// 
			this.comboAlertAbove.FormattingEnabled = true;
			this.comboAlertAbove.Location = new System.Drawing.Point(49, 19);
			this.comboAlertAbove.Name = "comboAlertAbove";
			this.comboAlertAbove.Size = new System.Drawing.Size(149, 21);
			this.comboAlertAbove.Sorted = true;
			this.comboAlertAbove.TabIndex = 6;
			this.comboAlertAbove.SelectedIndexChanged += new System.EventHandler(this.comboAlertAbove_SelectedIndexChanged);
			// 
			// numAlertBelow
			// 
			this.numAlertBelow.Increment = new decimal(new int[] {
            1,
            0,
            0,
            524288});
			this.numAlertBelow.Location = new System.Drawing.Point(49, 46);
			this.numAlertBelow.Maximum = new decimal(new int[] {
            -727379969,
            232,
            0,
            0});
			this.numAlertBelow.Name = "numAlertBelow";
			this.numAlertBelow.Size = new System.Drawing.Size(149, 20);
			this.numAlertBelow.TabIndex = 4;
			this.numAlertBelow.ValueChanged += new System.EventHandler(this.NumericUpDownTrim);
			this.numAlertBelow.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NumericUpDownDecSeparator);
			// 
			// groupBoxAlertBelow
			// 
			this.groupBoxAlertBelow.Controls.Add(this.buttonRemoveAlertBelow);
			this.groupBoxAlertBelow.Controls.Add(this.buttonAddAlertBelow);
			this.groupBoxAlertBelow.Controls.Add(this.label4);
			this.groupBoxAlertBelow.Controls.Add(this.comboAlertBelow);
			this.groupBoxAlertBelow.Controls.Add(this.numAlertBelow);
			this.groupBoxAlertBelow.Controls.Add(this.label2);
			this.groupBoxAlertBelow.Location = new System.Drawing.Point(251, 112);
			this.groupBoxAlertBelow.Name = "groupBoxAlertBelow";
			this.groupBoxAlertBelow.Size = new System.Drawing.Size(233, 77);
			this.groupBoxAlertBelow.TabIndex = 15;
			this.groupBoxAlertBelow.TabStop = false;
			this.groupBoxAlertBelow.Text = "Alert Below";
			// 
			// buttonRemoveAlertBelow
			// 
			this.buttonRemoveAlertBelow.Location = new System.Drawing.Point(204, 45);
			this.buttonRemoveAlertBelow.Name = "buttonRemoveAlertBelow";
			this.buttonRemoveAlertBelow.Size = new System.Drawing.Size(23, 23);
			this.buttonRemoveAlertBelow.TabIndex = 10;
			this.buttonRemoveAlertBelow.Text = "-";
			this.buttonRemoveAlertBelow.UseVisualStyleBackColor = true;
			this.buttonRemoveAlertBelow.Click += new System.EventHandler(this.buttonRemoveAlertBelow_Click);
			// 
			// buttonAddAlertBelow
			// 
			this.buttonAddAlertBelow.Location = new System.Drawing.Point(204, 18);
			this.buttonAddAlertBelow.Name = "buttonAddAlertBelow";
			this.buttonAddAlertBelow.Size = new System.Drawing.Size(23, 23);
			this.buttonAddAlertBelow.TabIndex = 9;
			this.buttonAddAlertBelow.Text = "+";
			this.buttonAddAlertBelow.UseVisualStyleBackColor = true;
			this.buttonAddAlertBelow.Click += new System.EventHandler(this.buttonAddAlertBelow_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 22);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(40, 13);
			this.label4.TabIndex = 8;
			this.label4.Text = "Select:";
			// 
			// comboAlertBelow
			// 
			this.comboAlertBelow.FormattingEnabled = true;
			this.comboAlertBelow.Location = new System.Drawing.Point(49, 19);
			this.comboAlertBelow.Name = "comboAlertBelow";
			this.comboAlertBelow.Size = new System.Drawing.Size(149, 21);
			this.comboAlertBelow.Sorted = true;
			this.comboAlertBelow.TabIndex = 5;
			this.comboAlertBelow.SelectedIndexChanged += new System.EventHandler(this.comboAlertBelow_SelectedIndexChanged);
			// 
			// buttonSwitchAlertView
			// 
			this.buttonSwitchAlertView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonSwitchAlertView.Location = new System.Drawing.Point(12, 195);
			this.buttonSwitchAlertView.Name = "buttonSwitchAlertView";
			this.buttonSwitchAlertView.Size = new System.Drawing.Size(79, 23);
			this.buttonSwitchAlertView.TabIndex = 16;
			this.buttonSwitchAlertView.Text = "Show Alerts";
			this.buttonSwitchAlertView.UseVisualStyleBackColor = true;
			this.buttonSwitchAlertView.Click += new System.EventHandler(this.buttonSwitchAlertView_Click);
			// 
			// FormPairSettings
			// 
			this.AcceptButton = this.buttonAccept;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(495, 226);
			this.Controls.Add(this.buttonSwitchAlertView);
			this.Controls.Add(this.groupBoxAlertBelow);
			this.Controls.Add(this.groupBoxAlertAbove);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.buttonAccept);
			this.Controls.Add(this.buttonCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormPairSettings";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "CryptoGadget Settings [Pair Settings]";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numAlertAbove)).EndInit();
			this.groupBoxAlertAbove.ResumeLayout(false);
			this.groupBoxAlertAbove.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numAlertBelow)).EndInit();
			this.groupBoxAlertBelow.ResumeLayout(false);
			this.groupBoxAlertBelow.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonAccept;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ComboBox comboCoin;
        private System.Windows.Forms.ComboBox comboTarget;
        private System.Windows.Forms.CheckBox checkTargetOnlyFiat;
        private System.Windows.Forms.CheckBox checkCoinIndexName;
		private System.Windows.Forms.CheckBox checkCoinOnlyFiat;
		private System.Windows.Forms.CheckBox checkTargetIndexName;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button buttonIconSwap;
		private System.Windows.Forms.Button buttonIconReDownload;
		private System.Windows.Forms.NumericUpDown numAlertAbove;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBoxAlertAbove;
		private System.Windows.Forms.NumericUpDown numAlertBelow;
		private System.Windows.Forms.Button buttonIconTargetSwap;
		private System.Windows.Forms.Button buttonIconTargetReDownload;
		private System.Windows.Forms.Button buttonRemoveAlertAbove;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button buttonAddAlertAbove;
		private System.Windows.Forms.ComboBox comboAlertAbove;
		private System.Windows.Forms.GroupBox groupBoxAlertBelow;
		private System.Windows.Forms.Button buttonRemoveAlertBelow;
		private System.Windows.Forms.Button buttonAddAlertBelow;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox comboAlertBelow;
		private System.Windows.Forms.Button buttonSwitchAlertView;
	}
}