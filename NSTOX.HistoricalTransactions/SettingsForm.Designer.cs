namespace NSTOX.HistoricalTransactions
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.retailerName = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.retailerId = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.transactionsPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnItemsBrowse = new System.Windows.Forms.Button();
            this.txtItemsPath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.retailerId)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Retailer ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Retailer Name";
            // 
            // retailerName
            // 
            this.retailerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.retailerName.Location = new System.Drawing.Point(101, 38);
            this.retailerName.Name = "retailerName";
            this.retailerName.Size = new System.Drawing.Size(183, 20);
            this.retailerName.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(212, 198);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // retailerId
            // 
            this.retailerId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.retailerId.Location = new System.Drawing.Point(102, 12);
            this.retailerId.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.retailerId.Name = "retailerId";
            this.retailerId.Size = new System.Drawing.Size(182, 20);
            this.retailerId.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Transactions Path";
            // 
            // transactionsPath
            // 
            this.transactionsPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.transactionsPath.Location = new System.Drawing.Point(101, 64);
            this.transactionsPath.Name = "transactionsPath";
            this.transactionsPath.ReadOnly = true;
            this.transactionsPath.Size = new System.Drawing.Size(183, 20);
            this.transactionsPath.TabIndex = 6;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(209, 90);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 7;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnItemsBrowse
            // 
            this.btnItemsBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnItemsBrowse.Location = new System.Drawing.Point(210, 145);
            this.btnItemsBrowse.Name = "btnItemsBrowse";
            this.btnItemsBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnItemsBrowse.TabIndex = 10;
            this.btnItemsBrowse.Text = "Browse...";
            this.btnItemsBrowse.UseVisualStyleBackColor = true;
            this.btnItemsBrowse.Click += new System.EventHandler(this.btnItemsBrowse_Click);
            // 
            // txtItemsPath
            // 
            this.txtItemsPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtItemsPath.Location = new System.Drawing.Point(102, 119);
            this.txtItemsPath.Name = "txtItemsPath";
            this.txtItemsPath.ReadOnly = true;
            this.txtItemsPath.Size = new System.Drawing.Size(183, 20);
            this.txtItemsPath.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Items Path";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(299, 233);
            this.Controls.Add(this.btnItemsBrowse);
            this.Controls.Add(this.txtItemsPath);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.transactionsPath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.retailerId);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.retailerName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.retailerId)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox retailerName;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown retailerId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox transactionsPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnItemsBrowse;
        private System.Windows.Forms.TextBox txtItemsPath;
        private System.Windows.Forms.Label label4;
    }
}