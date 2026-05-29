namespace Latenite {
    partial class MultipleResults {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
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
            this.SearchResults = new System.Windows.Forms.ListBox();
            this.SelectCancel = new System.Windows.Forms.Button();
            this.SelectOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SearchResults
            // 
            this.SearchResults.FormattingEnabled = true;
            this.SearchResults.HorizontalScrollbar = true;
            this.SearchResults.Location = new System.Drawing.Point(12, 12);
            this.SearchResults.Name = "SearchResults";
            this.SearchResults.Size = new System.Drawing.Size(286, 95);
            this.SearchResults.TabIndex = 0;
            this.SearchResults.DoubleClick += new System.EventHandler(this.SearchResults_DoubleClick);
            // 
            // SelectCancel
            // 
            this.SelectCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.SelectCancel.Location = new System.Drawing.Point(221, 113);
            this.SelectCancel.Name = "SelectCancel";
            this.SelectCancel.Size = new System.Drawing.Size(77, 27);
            this.SelectCancel.TabIndex = 2;
            this.SelectCancel.Text = "&Cancel";
            this.SelectCancel.UseVisualStyleBackColor = true;
            this.SelectCancel.Click += new System.EventHandler(this.SelectCancel_Click);
            // 
            // SelectOK
            // 
            this.SelectOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.SelectOK.Location = new System.Drawing.Point(138, 113);
            this.SelectOK.Name = "SelectOK";
            this.SelectOK.Size = new System.Drawing.Size(77, 27);
            this.SelectOK.TabIndex = 3;
            this.SelectOK.Text = "&OK";
            this.SelectOK.UseVisualStyleBackColor = true;
            this.SelectOK.Click += new System.EventHandler(this.SelectOK_Click);
            // 
            // MultipleResults
            // 
            this.AcceptButton = this.SelectOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.SelectCancel;
            this.ClientSize = new System.Drawing.Size(310, 149);
            this.Controls.Add(this.SelectOK);
            this.Controls.Add(this.SelectCancel);
            this.Controls.Add(this.SearchResults);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MultipleResults";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select a Help Item";
            this.Load += new System.EventHandler(this.MultipleResults_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button SelectCancel;
        private System.Windows.Forms.Button SelectOK;
        public System.Windows.Forms.ListBox SearchResults;
    }
}