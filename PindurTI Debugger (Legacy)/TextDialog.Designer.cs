namespace PindurTI_Debugger {
    partial class TextDialog {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextDialog));
            this.Caption = new System.Windows.Forms.Label();
            this.Value = new System.Windows.Forms.MaskedTextBox();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Caption
            // 
            this.Caption.AutoSize = true;
            this.Caption.Location = new System.Drawing.Point(12, 9);
            this.Caption.Name = "Caption";
            this.Caption.Size = new System.Drawing.Size(19, 13);
            this.Caption.TabIndex = 0;
            this.Caption.Text = "...";
            // 
            // Value
            // 
            this.Value.Location = new System.Drawing.Point(12, 25);
            this.Value.Name = "Value";
            this.Value.PromptChar = ' ';
            this.Value.Size = new System.Drawing.Size(268, 21);
            this.Value.TabIndex = 1;
            this.Value.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.Value_MaskInputRejected);
            // 
            // ButtonOK
            // 
            this.ButtonOK.Location = new System.Drawing.Point(124, 52);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(75, 22);
            this.ButtonOK.TabIndex = 2;
            this.ButtonOK.Text = "&OK";
            this.ButtonOK.UseVisualStyleBackColor = true;
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(205, 52);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 22);
            this.ButtonCancel.TabIndex = 3;
            this.ButtonCancel.Text = "&Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // TextDialog
            // 
            this.AcceptButton = this.ButtonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonCancel;
            this.ClientSize = new System.Drawing.Size(292, 86);
            this.ControlBox = false;
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOK);
            this.Controls.Add(this.Value);
            this.Controls.Add(this.Caption);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TextDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TextDialog";
            this.Shown += new System.EventHandler(this.TextDialog_Shown);
            this.Load += new System.EventHandler(this.TextDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Caption;
        private System.Windows.Forms.Button ButtonOK;
        private System.Windows.Forms.Button ButtonCancel;
        public System.Windows.Forms.MaskedTextBox Value;
    }
}