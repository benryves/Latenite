namespace Latenite {
	partial class Projects {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Projects));
            this.ProjectIcons = new System.Windows.Forms.ImageList(this.components);
            this.CreateNewProject = new System.Windows.Forms.Button();
            this.OpenExistingProject = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.ProjectFolder = new System.Windows.Forms.TextBox();
            this.ProjectName = new System.Windows.Forms.TextBox();
            this.ProjectPreview = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.FileOpenDialog = new System.Windows.Forms.OpenFileDialog();
            this.FolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProjectIcons
            // 
            this.ProjectIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.ProjectIcons.ImageSize = new System.Drawing.Size(48, 48);
            this.ProjectIcons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // CreateNewProject
            // 
            this.CreateNewProject.Enabled = false;
            this.CreateNewProject.Location = new System.Drawing.Point(321, 344);
            this.CreateNewProject.Name = "CreateNewProject";
            this.CreateNewProject.Size = new System.Drawing.Size(129, 31);
            this.CreateNewProject.TabIndex = 6;
            this.CreateNewProject.Text = "Create &New Project";
            this.CreateNewProject.Click += new System.EventHandler(this.CreateNewProject_Click);
            // 
            // OpenExistingProject
            // 
            this.OpenExistingProject.Location = new System.Drawing.Point(186, 344);
            this.OpenExistingProject.Name = "OpenExistingProject";
            this.OpenExistingProject.Size = new System.Drawing.Size(129, 31);
            this.OpenExistingProject.TabIndex = 8;
            this.OpenExistingProject.Text = "&Open Existing Project";
            this.OpenExistingProject.Click += new System.EventHandler(this.OpenExistingProject_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.ProjectFolder);
            this.groupBox1.Controls.Add(this.ProjectName);
            this.groupBox1.Controls.Add(this.ProjectPreview);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(450, 332);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "New Project";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(5, 299);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Folder:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(5, 272);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Name:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(417, 296);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(27, 20);
            this.button1.TabIndex = 9;
            this.button1.Text = "...";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ProjectFolder
            // 
            this.ProjectFolder.Location = new System.Drawing.Point(50, 296);
            this.ProjectFolder.Name = "ProjectFolder";
            this.ProjectFolder.Size = new System.Drawing.Size(361, 21);
            this.ProjectFolder.TabIndex = 8;
            // 
            // ProjectName
            // 
            this.ProjectName.Location = new System.Drawing.Point(50, 269);
            this.ProjectName.MaxLength = 255;
            this.ProjectName.Name = "ProjectName";
            this.ProjectName.Size = new System.Drawing.Size(394, 21);
            this.ProjectName.TabIndex = 7;
            this.ProjectName.Text = "New Project";
            // 
            // ProjectPreview
            // 
            this.ProjectPreview.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.ProjectPreview.HideSelection = false;
            this.ProjectPreview.LargeImageList = this.ProjectIcons;
            this.ProjectPreview.Location = new System.Drawing.Point(6, 20);
            this.ProjectPreview.MultiSelect = false;
            this.ProjectPreview.Name = "ProjectPreview";
            this.ProjectPreview.Size = new System.Drawing.Size(438, 243);
            this.ProjectPreview.SmallImageList = this.ProjectIcons;
            this.ProjectPreview.TabIndex = 6;
            this.ProjectPreview.UseCompatibleStateImageBehavior = false;
            this.ProjectPreview.View = System.Windows.Forms.View.Tile;
            this.ProjectPreview.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ProjectPreview_ItemSelectionChanged);
            this.ProjectPreview.Click += new System.EventHandler(this.ProjectPreview_Click);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Description";
            // 
            // FileOpenDialog
            // 
            this.FileOpenDialog.Filter = "Latenite Project Files (*.lnp)|*.lnp";
            // 
            // FolderBrowser
            // 
            this.FolderBrowser.Description = "The project folder will be created underneath this folder.";
            // 
            // Projects
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 387);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.OpenExistingProject);
            this.Controls.Add(this.CreateNewProject);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Projects";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Latenite Z80 Editor - Projects";
            this.Load += new System.EventHandler(this.Projects_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ImageList ProjectIcons;
		private System.Windows.Forms.Button CreateNewProject;
		private System.Windows.Forms.Button OpenExistingProject;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox ProjectFolder;
		private System.Windows.Forms.TextBox ProjectName;
		private System.Windows.Forms.ListView ProjectPreview;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.OpenFileDialog FileOpenDialog;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowser;
	}
}