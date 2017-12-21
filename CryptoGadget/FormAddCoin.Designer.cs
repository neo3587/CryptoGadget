namespace CryptoGadget {
    partial class FormAddCoin {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAddCoin));
			this.buttonAdd = new System.Windows.Forms.Button();
			this.buttonDone = new System.Windows.Forms.Button();
			this.boxCoin = new System.Windows.Forms.ComboBox();
			this.boxTarget = new System.Windows.Forms.ComboBox();
			this.checkTargetOnlyFiat = new System.Windows.Forms.CheckBox();
			this.checkCoinIndexName = new System.Windows.Forms.CheckBox();
			this.checkCoinOnlyFiat = new System.Windows.Forms.CheckBox();
			this.checkTargetIndexName = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonAdd
			// 
			this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonAdd.Location = new System.Drawing.Point(269, 112);
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.Size = new System.Drawing.Size(75, 23);
			this.buttonAdd.TabIndex = 0;
			this.buttonAdd.Text = "Add";
			this.buttonAdd.UseVisualStyleBackColor = true;
			this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
			// 
			// buttonDone
			// 
			this.buttonDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonDone.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonDone.Location = new System.Drawing.Point(350, 112);
			this.buttonDone.Name = "buttonDone";
			this.buttonDone.Size = new System.Drawing.Size(75, 23);
			this.buttonDone.TabIndex = 1;
			this.buttonDone.Text = "Done";
			this.buttonDone.UseVisualStyleBackColor = true;
			this.buttonDone.Click += new System.EventHandler(this.buttonDone_Click);
			// 
			// boxCoin
			// 
			this.boxCoin.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.boxCoin.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.boxCoin.Location = new System.Drawing.Point(6, 19);
			this.boxCoin.Name = "boxCoin";
			this.boxCoin.Size = new System.Drawing.Size(192, 21);
			this.boxCoin.Sorted = true;
			this.boxCoin.TabIndex = 2;
			this.boxCoin.Click += new System.EventHandler(this.DropDownOnClick);
			this.boxCoin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DropDownOnKeyPress);
			// 
			// boxTarget
			// 
			this.boxTarget.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
			this.boxTarget.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.boxTarget.Location = new System.Drawing.Point(6, 19);
			this.boxTarget.Name = "boxTarget";
			this.boxTarget.Size = new System.Drawing.Size(192, 21);
			this.boxTarget.Sorted = true;
			this.boxTarget.TabIndex = 4;
			this.boxTarget.Click += new System.EventHandler(this.DropDownOnClick);
			this.boxTarget.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DropDownOnKeyPress);
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
			this.groupBox1.Controls.Add(this.boxCoin);
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
			this.groupBox2.Controls.Add(this.boxTarget);
			this.groupBox2.Location = new System.Drawing.Point(222, 9);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(204, 96);
			this.groupBox2.TabIndex = 11;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Target Coin";
			// 
			// FormAddCoin
			// 
			this.AcceptButton = this.buttonAdd;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonDone;
			this.ClientSize = new System.Drawing.Size(437, 146);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.buttonDone);
			this.Controls.Add(this.buttonAdd);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormAddCoin";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "CryptoGadget Settings [Add Coins]";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonDone;
        private System.Windows.Forms.ComboBox boxCoin;
        private System.Windows.Forms.ComboBox boxTarget;
        private System.Windows.Forms.CheckBox checkTargetOnlyFiat;
        private System.Windows.Forms.CheckBox checkCoinIndexName;
		private System.Windows.Forms.CheckBox checkCoinOnlyFiat;
		private System.Windows.Forms.CheckBox checkTargetIndexName;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
	}
}