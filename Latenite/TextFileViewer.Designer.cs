namespace Latenite {
	partial class TextFileViewer {
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
			this.textFileBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// textFileBox
			// 
			this.textFileBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textFileBox.Location = new System.Drawing.Point(0, 0);
			this.textFileBox.Multiline = true;
			this.textFileBox.Name = "textFileBox";
			this.textFileBox.ReadOnly = true;
			this.textFileBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textFileBox.Size = new System.Drawing.Size(800, 450);
			this.textFileBox.TabIndex = 0;
			this.textFileBox.WordWrap = false;
			// 
			// TextFileViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.textFileBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.Name = "TextFileViewer";
			this.ShowInTaskbar = false;
			this.Text = "TextFileReader";
			this.Shown += new System.EventHandler(this.TextFileViewer_Shown);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextFileViewer_KeyDown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textFileBox;
	}
}