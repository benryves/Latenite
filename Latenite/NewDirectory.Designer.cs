namespace Latenite {
    partial class NewDirectory {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewDirectory));
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.TabIcons = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.DirectoryName = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.ButtonCancel, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.ButtonOK, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(88, 28);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(197, 29);
            this.tableLayoutPanel3.TabIndex = 6;
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonCancel.Location = new System.Drawing.Point(101, 3);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(93, 26);
            this.ButtonCancel.TabIndex = 4;
            this.ButtonCancel.Text = "&Cancel";
            // 
            // ButtonOK
            // 
            this.ButtonOK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ButtonOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonOK.Location = new System.Drawing.Point(3, 3);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(92, 26);
            this.ButtonOK.TabIndex = 2;
            this.ButtonOK.Text = "&OK";
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // TabIcons
            // 
            this.TabIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("TabIcons.ImageStream")));
            this.TabIcons.TransparentColor = System.Drawing.Color.Fuchsia;
            this.TabIcons.Images.SetKeyName(0, "Control_TextBox.bmp");
            this.TabIcons.Images.SetKeyName(1, "build.png");
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.DirectoryName, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(288, 60);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // DirectoryName
            // 
            this.DirectoryName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DirectoryName.Location = new System.Drawing.Point(3, 3);
            this.DirectoryName.MaxLength = 255;
            this.DirectoryName.Name = "DirectoryName";
            this.DirectoryName.Size = new System.Drawing.Size(282, 21);
            this.DirectoryName.TabIndex = 7;
            // 
            // NewDirectory
            // 
            this.AcceptButton = this.ButtonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonCancel;
            this.ClientSize = new System.Drawing.Size(288, 60);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "NewDirectory";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Enter a Folder Name:";
            this.Load += new System.EventHandler(this.NewDirectory_Load);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.Button ButtonOK;
        private System.Windows.Forms.ImageList TabIcons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        public System.Windows.Forms.TextBox DirectoryName;
    }
}