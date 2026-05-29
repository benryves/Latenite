namespace Latenite {
    partial class GoToLine {
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
            this.LineCountLabel = new System.Windows.Forms.Label();
            this.GoToOk = new System.Windows.Forms.Button();
            this.GoToCancel = new System.Windows.Forms.Button();
            this.LineToSelect = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // LineCountLabel
            // 
            this.LineCountLabel.AutoSize = true;
            this.LineCountLabel.Location = new System.Drawing.Point(9, 9);
            this.LineCountLabel.Name = "LineCountLabel";
            this.LineCountLabel.Size = new System.Drawing.Size(0, 13);
            this.LineCountLabel.TabIndex = 0;
            // 
            // GoToOk
            // 
            this.GoToOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.GoToOk.Location = new System.Drawing.Point(157, 52);
            this.GoToOk.Name = "GoToOk";
            this.GoToOk.Size = new System.Drawing.Size(73, 25);
            this.GoToOk.TabIndex = 2;
            this.GoToOk.Text = "&OK";
            this.GoToOk.UseVisualStyleBackColor = true;
            this.GoToOk.Click += new System.EventHandler(this.GoToOk_Click);
            // 
            // GoToCancel
            // 
            this.GoToCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.GoToCancel.Location = new System.Drawing.Point(78, 52);
            this.GoToCancel.Name = "GoToCancel";
            this.GoToCancel.Size = new System.Drawing.Size(73, 25);
            this.GoToCancel.TabIndex = 3;
            this.GoToCancel.Text = "&Cancel";
            this.GoToCancel.UseVisualStyleBackColor = true;
            this.GoToCancel.Click += new System.EventHandler(this.GoToCancel_Click);
            // 
            // LineToSelect
            // 
            this.LineToSelect.Location = new System.Drawing.Point(12, 25);
            this.LineToSelect.Name = "LineToSelect";
            this.LineToSelect.Size = new System.Drawing.Size(218, 21);
            this.LineToSelect.TabIndex = 4;
            // 
            // GoToLine
            // 
            this.AcceptButton = this.GoToOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.GoToCancel;
            this.ClientSize = new System.Drawing.Size(242, 89);
            this.Controls.Add(this.LineToSelect);
            this.Controls.Add(this.GoToCancel);
            this.Controls.Add(this.GoToOk);
            this.Controls.Add(this.LineCountLabel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GoToLine";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Go To Line";
            this.Load += new System.EventHandler(this.GoToLine_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LineCountLabel;
        private System.Windows.Forms.Button GoToOk;
        private System.Windows.Forms.Button GoToCancel;
        private System.Windows.Forms.TextBox LineToSelect;
    }
}