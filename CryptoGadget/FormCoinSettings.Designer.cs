﻿namespace CryptoGadget {
    partial class FormCoinSettings {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCoinSettings));
			this.buttonAccept = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.comboCoin = new System.Windows.Forms.ComboBox();
			this.comboTarget = new System.Windows.Forms.ComboBox();
			this.checkTargetOnlyFiat = new System.Windows.Forms.CheckBox();
			this.checkCoinIndexName = new System.Windows.Forms.CheckBox();
			this.checkCoinOnlyFiat = new System.Windows.Forms.CheckBox();
			this.checkTargetIndexName = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.buttonIconSwap = new System.Windows.Forms.Button();
			this.buttonIconReDownload = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonAccept
			// 
			this.buttonAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonAccept.Location = new System.Drawing.Point(270, 111);
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
			this.buttonCancel.Location = new System.Drawing.Point(351, 111);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 1;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// comboCoin
			// 
			this.comboCoin.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.comboCoin.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.comboCoin.Location = new System.Drawing.Point(6, 19);
			this.comboCoin.Name = "comboCoin";
			this.comboCoin.Size = new System.Drawing.Size(192, 21);
			this.comboCoin.Sorted = true;
			this.comboCoin.TabIndex = 2;
			this.comboCoin.Click += new System.EventHandler(this.DropDownOnClick);
			this.comboCoin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DropDownOnKeyPress);
			// 
			// comboTarget
			// 
			this.comboTarget.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
			this.comboTarget.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.comboTarget.Location = new System.Drawing.Point(6, 19);
			this.comboTarget.Name = "comboTarget";
			this.comboTarget.Size = new System.Drawing.Size(192, 21);
			this.comboTarget.Sorted = true;
			this.comboTarget.TabIndex = 4;
			this.comboTarget.Click += new System.EventHandler(this.DropDownOnClick);
			this.comboTarget.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DropDownOnKeyPress);
			// 
			// checkTargetOnlyFiat
			// 
			this.checkTargetOnlyFiat.AutoSize = true;
			this.checkTargetOnlyFiat.Location = new System.Drawing.Point(6, 73);
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
			this.checkCoinIndexName.Location = new System.Drawing.Point(6, 46);
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
			this.checkCoinOnlyFiat.Location = new System.Drawing.Point(6, 72);
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
			this.checkTargetIndexName.Location = new System.Drawing.Point(6, 46);
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
			this.groupBox1.Controls.Add(this.checkCoinIndexName);
			this.groupBox1.Controls.Add(this.checkCoinOnlyFiat);
			this.groupBox1.Location = new System.Drawing.Point(12, 9);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(204, 96);
			this.groupBox1.TabIndex = 10;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Coin";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.checkTargetIndexName);
			this.groupBox2.Controls.Add(this.checkTargetOnlyFiat);
			this.groupBox2.Controls.Add(this.comboTarget);
			this.groupBox2.Location = new System.Drawing.Point(222, 9);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(204, 96);
			this.groupBox2.TabIndex = 11;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Target Coin";
			// 
			// buttonIconSwap
			// 
			this.buttonIconSwap.Location = new System.Drawing.Point(141, 111);
			this.buttonIconSwap.Name = "buttonIconSwap";
			this.buttonIconSwap.Size = new System.Drawing.Size(75, 23);
			this.buttonIconSwap.TabIndex = 12;
			this.buttonIconSwap.Text = "Swap Icon";
			this.buttonIconSwap.UseVisualStyleBackColor = true;
			this.buttonIconSwap.Click += new System.EventHandler(this.buttonIconSwap_Click);
			// 
			// buttonIconReDownload
			// 
			this.buttonIconReDownload.Location = new System.Drawing.Point(12, 111);
			this.buttonIconReDownload.Name = "buttonIconReDownload";
			this.buttonIconReDownload.Size = new System.Drawing.Size(126, 23);
			this.buttonIconReDownload.TabIndex = 13;
			this.buttonIconReDownload.Text = "Re-Download Icon";
			this.buttonIconReDownload.UseVisualStyleBackColor = true;
			this.buttonIconReDownload.Click += new System.EventHandler(this.buttonIconReDownload_Click);
			// 
			// FormCoinSettings
			// 
			this.AcceptButton = this.buttonAccept;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(437, 142);
			this.Controls.Add(this.buttonIconReDownload);
			this.Controls.Add(this.buttonIconSwap);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.buttonAccept);
			this.Controls.Add(this.buttonCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormCoinSettings";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "CryptoGadget Settings [Coin Settings]";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
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
	}
}