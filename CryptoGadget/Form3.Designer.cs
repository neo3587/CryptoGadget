namespace CryptoGadget {
    partial class AddCoinForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddCoinForm));
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonDone = new System.Windows.Forms.Button();
            this.boxCoin = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.boxTarget = new System.Windows.Forms.ComboBox();
            this.checkOnlyFiat = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAdd.Location = new System.Drawing.Point(249, 73);
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
            this.buttonDone.Location = new System.Drawing.Point(330, 73);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new System.Drawing.Size(75, 23);
            this.buttonDone.TabIndex = 1;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = true;
            this.buttonDone.Click += new System.EventHandler(this.buttonDone_Click);
            // 
            // boxCoin
            // 
            this.boxCoin.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.boxCoin.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.boxCoin.Location = new System.Drawing.Point(12, 25);
            this.boxCoin.Name = "boxCoin";
            this.boxCoin.Size = new System.Drawing.Size(192, 21);
            this.boxCoin.Sorted = true;
            this.boxCoin.TabIndex = 2;
            this.boxCoin.Click += new System.EventHandler(this.DropDownOnClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Coin";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(210, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "TargetCoin";
            // 
            // boxTarget
            // 
            this.boxTarget.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.boxTarget.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.boxTarget.Location = new System.Drawing.Point(213, 25);
            this.boxTarget.Name = "boxTarget";
            this.boxTarget.Size = new System.Drawing.Size(192, 21);
            this.boxTarget.Sorted = true;
            this.boxTarget.TabIndex = 4;
            this.boxTarget.Click += new System.EventHandler(this.DropDownOnClick);
            // 
            // checkOnlyFiat
            // 
            this.checkOnlyFiat.AutoSize = true;
            this.checkOnlyFiat.Location = new System.Drawing.Point(285, 52);
            this.checkOnlyFiat.Name = "checkOnlyFiat";
            this.checkOnlyFiat.Size = new System.Drawing.Size(120, 17);
            this.checkOnlyFiat.TabIndex = 6;
            this.checkOnlyFiat.Text = "Only Fiat Currencies";
            this.checkOnlyFiat.UseVisualStyleBackColor = true;
            this.checkOnlyFiat.CheckedChanged += new System.EventHandler(this.checkOnlyFiat_CheckedChanged);
            // 
            // AddCoinForm
            // 
            this.AcceptButton = this.buttonAdd;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonDone;
            this.ClientSize = new System.Drawing.Size(417, 107);
            this.Controls.Add(this.checkOnlyFiat);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.boxTarget);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.boxCoin);
            this.Controls.Add(this.buttonDone);
            this.Controls.Add(this.buttonAdd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddCoinForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CryptoGadget Settings [Add Coins]";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonDone;
        private System.Windows.Forms.ComboBox boxCoin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox boxTarget;
        private System.Windows.Forms.CheckBox checkOnlyFiat;
    }
}