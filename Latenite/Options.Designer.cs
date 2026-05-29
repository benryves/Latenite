namespace Latenite {
    partial class Options {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Options));
            this.SettingsColourDialog = new System.Windows.Forms.ColorDialog();
            this.SettingsFontDialog = new System.Windows.Forms.FontDialog();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new Latenite.CleanTabPage();
            this.SetColour = new System.Windows.Forms.Button();
            this.PreviewText = new BenRyves.ColourfulEditor();
            this.SelectDefault = new System.Windows.Forms.ComboBox();
            this.SelectedItems = new System.Windows.Forms.ListBox();
            this.SizeSelection = new System.Windows.Forms.ComboBox();
            this.FontSelection = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.SaveCurrentTheme = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SetBackgroundColour = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.label10 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label11 = new System.Windows.Forms.Label();
            this.AutoSaveOnBuild = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.FilesToHighlight = new System.Windows.Forms.TextBox();
            this.FilePattern = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.MultipleRowsOfTabs = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.NewlinesAsCRLF = new System.Windows.Forms.CheckBox();
            this.AutoIndent = new System.Windows.Forms.CheckBox();
            this.TabIcons = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SettingsColourDialog
            // 
            this.SettingsColourDialog.AnyColor = true;
            this.SettingsColourDialog.FullOpen = true;
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
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.ButtonCancel, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.ButtonOK, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(123, 332);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(197, 29);
            this.tableLayoutPanel3.TabIndex = 6;
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
            this.tableLayoutPanel2.Size = new System.Drawing.Size(323, 364);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ImageList = this.TabIcons;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(323, 329);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.SetColour);
            this.tabPage1.Controls.Add(this.PreviewText);
            this.tabPage1.Controls.Add(this.SelectDefault);
            this.tabPage1.Controls.Add(this.SelectedItems);
            this.tabPage1.Controls.Add(this.SizeSelection);
            this.tabPage1.Controls.Add(this.FontSelection);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.SaveCurrentTheme);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.SetBackgroundColour);
            this.tabPage1.Controls.Add(this.listView1);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.ImageIndex = 1;
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(315, 302);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Fonts and Colours";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // SetColour
            // 
            this.SetColour.Location = new System.Drawing.Point(168, 68);
            this.SetColour.Name = "SetColour";
            this.SetColour.Size = new System.Drawing.Size(137, 24);
            this.SetColour.TabIndex = 1;
            this.SetColour.Text = "Set C&olour...";
            this.SetColour.UseVisualStyleBackColor = true;
            this.SetColour.Click += new System.EventHandler(this.SetColour_Click);
            // 
            // PreviewText
            // 
            this.PreviewText.AcceptsTab = true;
            this.PreviewText.AutoIndent = true;
            this.PreviewText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.PreviewText.DetectUrls = false;
            this.PreviewText.EnableAutoDragDrop = true;
            this.PreviewText.EnableHighlighting = true;
            this.PreviewText.IntellisenseFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PreviewText.Location = new System.Drawing.Point(9, 186);
            this.PreviewText.Name = "PreviewText";
            this.PreviewText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.PreviewText.Size = new System.Drawing.Size(296, 108);
            this.PreviewText.TabIndex = 12;
            this.PreviewText.Text = "";
            this.PreviewText.WordWrap = false;
            // 
            // SelectDefault
            // 
            this.SelectDefault.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectDefault.FormattingEnabled = true;
            this.SelectDefault.Location = new System.Drawing.Point(6, 143);
            this.SelectDefault.Name = "SelectDefault";
            this.SelectDefault.Size = new System.Drawing.Size(226, 21);
            this.SelectDefault.TabIndex = 4;
            this.SelectDefault.SelectedIndexChanged += new System.EventHandler(this.SelectDefault_SelectedIndexChanged);
            // 
            // SelectedItems
            // 
            this.SelectedItems.FormattingEnabled = true;
            this.SelectedItems.Items.AddRange(new object[] {
            "Plain Text",
            "Z80 Instructions",
            "Registers and Flags",
            "Assembler Directives",
            "Labels",
            "Routines",
            "Comments",
            "Strings"});
            this.SelectedItems.Location = new System.Drawing.Point(6, 68);
            this.SelectedItems.Name = "SelectedItems";
            this.SelectedItems.Size = new System.Drawing.Size(156, 56);
            this.SelectedItems.TabIndex = 0;
            // 
            // SizeSelection
            // 
            this.SizeSelection.FormattingEnabled = true;
            this.SizeSelection.Location = new System.Drawing.Point(238, 28);
            this.SizeSelection.Name = "SizeSelection";
            this.SizeSelection.Size = new System.Drawing.Size(67, 21);
            this.SizeSelection.TabIndex = 8;
            this.SizeSelection.TextChanged += new System.EventHandler(this.SizeSelection_TextChanged);
            this.SizeSelection.TextUpdate += new System.EventHandler(this.SizeSelection_TextUpdate);
            // 
            // FontSelection
            // 
            this.FontSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FontSelection.FormattingEnabled = true;
            this.FontSelection.Location = new System.Drawing.Point(6, 28);
            this.FontSelection.Name = "FontSelection";
            this.FontSelection.Size = new System.Drawing.Size(226, 21);
            this.FontSelection.TabIndex = 7;
            this.FontSelection.SelectedIndexChanged += new System.EventHandler(this.FontSelection_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(235, 12);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(30, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "Size:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(33, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Font:";
            // 
            // SaveCurrentTheme
            // 
            this.SaveCurrentTheme.Location = new System.Drawing.Point(238, 143);
            this.SaveCurrentTheme.Name = "SaveCurrentTheme";
            this.SaveCurrentTheme.Size = new System.Drawing.Size(67, 21);
            this.SaveCurrentTheme.TabIndex = 9;
            this.SaveCurrentTheme.Text = "&Save...";
            this.SaveCurrentTheme.UseVisualStyleBackColor = true;
            this.SaveCurrentTheme.Click += new System.EventHandler(this.SaveCurrentTheme_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(165, 52);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Colours:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 127);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Select a theme:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Display items:";
            // 
            // SetBackgroundColour
            // 
            this.SetBackgroundColour.Location = new System.Drawing.Point(168, 98);
            this.SetBackgroundColour.Name = "SetBackgroundColour";
            this.SetBackgroundColour.Size = new System.Drawing.Size(137, 24);
            this.SetBackgroundColour.TabIndex = 2;
            this.SetBackgroundColour.Text = "&Background Colour...";
            this.SetBackgroundColour.UseVisualStyleBackColor = true;
            this.SetBackgroundColour.Click += new System.EventHandler(this.SetBackgroundColour_Click);
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(6, 183);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(301, 112);
            this.listView1.TabIndex = 14;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 167);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(45, 13);
            this.label10.TabIndex = 13;
            this.label10.Text = "Sample:";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tableLayoutPanel1);
            this.tabPage3.ImageIndex = 2;
            this.tabPage3.Location = new System.Drawing.Point(4, 23);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.tabPage3.Size = new System.Drawing.Size(315, 302);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "General Settings";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label11, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.AutoSaveOnBuild, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.FilesToHighlight, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.FilePattern, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label12, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.MultipleRowsOfTabs, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.NewlinesAsCRLF, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.AutoIndent, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 8);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(309, 246);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 34);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(64, 13);
            this.label11.TabIndex = 7;
            this.label11.Text = "Autoindent:";
            // 
            // AutoSaveOnBuild
            // 
            this.AutoSaveOnBuild.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.AutoSaveOnBuild.AutoSize = true;
            this.AutoSaveOnBuild.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.AutoSaveOnBuild.Location = new System.Drawing.Point(130, 6);
            this.AutoSaveOnBuild.Name = "AutoSaveOnBuild";
            this.AutoSaveOnBuild.Size = new System.Drawing.Size(15, 14);
            this.AutoSaveOnBuild.TabIndex = 4;
            this.AutoSaveOnBuild.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Autosave on build:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Files to highlight:";
            // 
            // FilesToHighlight
            // 
            this.FilesToHighlight.Location = new System.Drawing.Point(130, 57);
            this.FilesToHighlight.Name = "FilesToHighlight";
            this.FilesToHighlight.Size = new System.Drawing.Size(174, 21);
            this.FilesToHighlight.TabIndex = 2;
            // 
            // FilePattern
            // 
            this.FilePattern.Location = new System.Drawing.Point(130, 84);
            this.FilePattern.Name = "FilePattern";
            this.FilePattern.Size = new System.Drawing.Size(174, 21);
            this.FilePattern.TabIndex = 8;
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 88);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(112, 13);
            this.label12.TabIndex = 9;
            this.label12.Text = "Project manager files:";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 115);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Multiple rows of tabs:";
            // 
            // MultipleRowsOfTabs
            // 
            this.MultipleRowsOfTabs.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.MultipleRowsOfTabs.AutoSize = true;
            this.MultipleRowsOfTabs.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.MultipleRowsOfTabs.Location = new System.Drawing.Point(130, 114);
            this.MultipleRowsOfTabs.Name = "MultipleRowsOfTabs";
            this.MultipleRowsOfTabs.Size = new System.Drawing.Size(15, 14);
            this.MultipleRowsOfTabs.TabIndex = 4;
            this.MultipleRowsOfTabs.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Save newlines as CRLF:";
            // 
            // NewlinesAsCRLF
            // 
            this.NewlinesAsCRLF.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.NewlinesAsCRLF.AutoSize = true;
            this.NewlinesAsCRLF.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.NewlinesAsCRLF.Location = new System.Drawing.Point(130, 141);
            this.NewlinesAsCRLF.Name = "NewlinesAsCRLF";
            this.NewlinesAsCRLF.Size = new System.Drawing.Size(15, 14);
            this.NewlinesAsCRLF.TabIndex = 4;
            this.NewlinesAsCRLF.UseVisualStyleBackColor = true;
            // 
            // AutoIndent
            // 
            this.AutoIndent.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.AutoIndent.AutoSize = true;
            this.AutoIndent.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.AutoIndent.Location = new System.Drawing.Point(130, 33);
            this.AutoIndent.Name = "AutoIndent";
            this.AutoIndent.Size = new System.Drawing.Size(15, 14);
            this.AutoIndent.TabIndex = 5;
            this.AutoIndent.UseVisualStyleBackColor = true;
            // 
            // TabIcons
            // 
            this.TabIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("TabIcons.ImageStream")));
            this.TabIcons.TransparentColor = System.Drawing.Color.Fuchsia;
            this.TabIcons.Images.SetKeyName(0, "folder_closed_16_h.png");
            this.TabIcons.Images.SetKeyName(1, "Color_linecolor.bmp");
            this.TabIcons.Images.SetKeyName(2, "Webcontrol_Advancededitorpart.bmp");
            // 
            // Options
            // 
            this.AcceptButton = this.ButtonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonCancel;
            this.ClientSize = new System.Drawing.Size(323, 364);
            this.Controls.Add(this.tableLayoutPanel2);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Options";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.Options_Load);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColorDialog SettingsColourDialog;
        private System.Windows.Forms.FontDialog SettingsFontDialog;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button ButtonOK;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private CleanTabPage tabPage1;
        private System.Windows.Forms.ImageList TabIcons;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox FilesToHighlight;
        private System.Windows.Forms.ListBox SelectedItems;
        private System.Windows.Forms.Button SetBackgroundColour;
        private System.Windows.Forms.Button SetColour;
        private System.Windows.Forms.ComboBox FontSelection;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox SelectDefault;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox SizeSelection;
        private System.Windows.Forms.Button SaveCurrentTheme;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private BenRyves.ColourfulEditor PreviewText;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox AutoIndent;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox FilePattern;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox AutoSaveOnBuild;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox MultipleRowsOfTabs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox NewlinesAsCRLF;
    }
}