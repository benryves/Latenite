namespace Latenite {
	partial class ProjectProperties {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectProperties));
            this.ButtonOK = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new Latenite.CleanTabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ProjectName = new System.Windows.Forms.TextBox();
            this.BinaryName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new Latenite.CleanTabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.BuildScripts = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.label2 = new System.Windows.Forms.Label();
            this.BuildCombo = new System.Windows.Forms.ComboBox();
            this.TabIcons = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.ChooseHelpFiles = new System.Windows.Forms.ListView();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
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
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(381, 216);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ImageList = this.TabIcons;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(381, 181);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel1);
            this.tabPage1.ImageIndex = 0;
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(373, 154);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Names";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.ProjectName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.BinaryName, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(367, 54);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // ProjectName
            // 
            this.ProjectName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProjectName.Location = new System.Drawing.Point(84, 3);
            this.ProjectName.MaxLength = 255;
            this.ProjectName.Name = "ProjectName";
            this.ProjectName.Size = new System.Drawing.Size(280, 21);
            this.ProjectName.TabIndex = 6;
            this.ProjectName.Text = "Project";
            // 
            // BinaryName
            // 
            this.BinaryName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BinaryName.Location = new System.Drawing.Point(84, 30);
            this.BinaryName.MaxLength = 255;
            this.BinaryName.Name = "BinaryName";
            this.BinaryName.Size = new System.Drawing.Size(280, 21);
            this.BinaryName.TabIndex = 7;
            this.BinaryName.Text = "PROJECT";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 27);
            this.label3.TabIndex = 9;
            this.label3.Text = "Project Name:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 27);
            this.label1.TabIndex = 8;
            this.label1.Text = "Binary Name:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tableLayoutPanel4);
            this.tabPage2.ImageIndex = 1;
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(373, 154);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Build";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.BuildScripts, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.BuildCombo, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Size = new System.Drawing.Size(367, 148);
            this.tableLayoutPanel4.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 122);
            this.label4.TabIndex = 7;
            this.label4.Text = "Build Scripts:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BuildScripts
            // 
            this.BuildScripts.CheckBoxes = true;
            this.BuildScripts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.BuildScripts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BuildScripts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.BuildScripts.Location = new System.Drawing.Point(77, 30);
            this.BuildScripts.Name = "BuildScripts";
            this.BuildScripts.Size = new System.Drawing.Size(287, 116);
            this.BuildScripts.TabIndex = 6;
            this.BuildScripts.UseCompatibleStateImageBehavior = false;
            this.BuildScripts.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 250;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 27);
            this.label2.TabIndex = 5;
            this.label2.Text = "File to Build:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BuildCombo
            // 
            this.BuildCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BuildCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BuildCombo.FormattingEnabled = true;
            this.BuildCombo.Location = new System.Drawing.Point(77, 3);
            this.BuildCombo.Name = "BuildCombo";
            this.BuildCombo.Size = new System.Drawing.Size(287, 21);
            this.BuildCombo.Sorted = true;
            this.BuildCombo.TabIndex = 4;
            // 
            // TabIcons
            // 
            this.TabIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("TabIcons.ImageStream")));
            this.TabIcons.TransparentColor = System.Drawing.Color.Fuchsia;
            this.TabIcons.Images.SetKeyName(0, "");
            this.TabIcons.Images.SetKeyName(1, "");
            this.TabIcons.Images.SetKeyName(2, "Help.bmp");
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.ButtonCancel, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.ButtonOK, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(181, 184);
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
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.ChooseHelpFiles);
            this.tabPage3.ImageIndex = 2;
            this.tabPage3.Location = new System.Drawing.Point(4, 23);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(373, 154);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Help Files";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // ChooseHelpFiles
            // 
            this.ChooseHelpFiles.CheckBoxes = true;
            this.ChooseHelpFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.ChooseHelpFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChooseHelpFiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.ChooseHelpFiles.Location = new System.Drawing.Point(3, 3);
            this.ChooseHelpFiles.Name = "ChooseHelpFiles";
            this.ChooseHelpFiles.Size = new System.Drawing.Size(367, 148);
            this.ChooseHelpFiles.TabIndex = 7;
            this.ChooseHelpFiles.UseCompatibleStateImageBehavior = false;
            this.ChooseHelpFiles.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 250;
            // 
            // ProjectProperties
            // 
            this.AcceptButton = this.ButtonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonCancel;
            this.ClientSize = new System.Drawing.Size(381, 216);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ProjectProperties";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Properties";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ProjectProperties_FormClosed);
            this.Load += new System.EventHandler(this.ProjectProperties_Load);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.Button ButtonOK;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.ImageList TabIcons;
        private System.Windows.Forms.TabControl tabControl1;
        private CleanTabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox ProjectName;
        private System.Windows.Forms.TextBox BinaryName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private CleanTabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListView BuildScripts;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox BuildCombo;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListView ChooseHelpFiles;
        private System.Windows.Forms.ColumnHeader columnHeader2;
	}
}