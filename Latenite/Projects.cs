using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Latenite {
	public partial class Projects : Form {
		public Projects() {
			InitializeComponent();
		}



		private void Projects_Load(object sender, EventArgs e) {
			this.ProjectFolder.Text = Path.GetFullPath("Projects");
			// Populate:
			string[] ProjectFolders = { "" };
			try {
				ProjectFolders = Directory.GetDirectories("Templates\\Projects\\");
			} catch (Exception ex) {
				MessageBox.Show("Fatal error: Projects folder not found!\n" + ex.Message);
				Application.Exit();
			}
			foreach (string P in ProjectFolders) {
				ListViewItem L = new ListViewItem(Path.GetFileName(P));
                if (File.Exists(P + "\\Icon.png")) {
					try {
                        Bitmap B = new Bitmap(P + "\\Icon.png");
                        decimal WinVersion = (decimal)Environment.OSVersion.Version.Major + (decimal)Environment.OSVersion.Version.Minor / 10;
                        if (Environment.OSVersion.Platform == PlatformID.Win32NT && WinVersion >= 5.1m) {
                            this.ProjectIcons.Images.Add(B);
                        } else {
                            Image I = new Bitmap((Image)B, new Size(32, 32));
                            B = new Bitmap(32, 32, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                            Graphics G = Graphics.FromImage(B);
                            G.Clear(ProjectPreview.BackColor);
                            G.DrawImage(I, 0, 0);
                            this.ProjectIcons.Images.Add(B);
                        }
						
						L.ImageIndex = ProjectIcons.Images.Count - 1;
					} catch { }
										
				}
				if (File.Exists(P + "\\Description.txt")) {
					try {
						using (TextReader R = new StreamReader(P + "\\Description.txt")) {
							L.SubItems.Add(R.ReadToEnd());
						}
						
					} catch { }

				}
				this.ProjectPreview.Items.Add(L);
												
			}
			

		}

		private void ProjectPreview_Click(object sender, EventArgs e) {

		}

		private void ProjectPreview_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
			if (ProjectPreview.SelectedItems.Count == 0) {
				CreateNewProject.Enabled = false;
			} else {
				CreateNewProject.Enabled = true;
			}
		}

		private void OpenExistingProject_Click(object sender, EventArgs e) {
			DialogResult D = FileOpenDialog.ShowDialog();
			if (D != DialogResult.OK) return;
			if (Program.MainIDE.LoadProjectFile(FileOpenDialog.FileName)) {
				this.Dispose();
			}
		}

        /// <summary>
        /// Copy a folder's files and all subfolders recursively.
        /// </summary>
        /// <param name="RootFolder">Path to the root folder.</param>
        private bool CopyAllFilesAndFolders(string SourceFolder, string DestFolder) {
            try {
                Directory.CreateDirectory(DestFolder);
            } catch (Exception ex) {
                MessageBox.Show("The folder '" + DestFolder + "' could not be created:\n" + ex.Message, "New Project", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            // Now copy over all the files:
            Directory.SetCurrentDirectory(Application.StartupPath);
            string[] FilesToCopy = { "" };
            try {
                FilesToCopy = Directory.GetFiles(SourceFolder);
            } catch (Exception ex) {
                MessageBox.Show("Error getting list of files to copy:\n" + ex.Message, "New Project", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            foreach (string F in FilesToCopy) {
                try {
                    string Src = F;
                    string Dst = Path.Combine(DestFolder,Path.GetFileName(F).Replace("$PROJECT_NAME$",this.ProjectName.Text));

                    bool JustCopy = Path.GetFileName(Src)[0] == '!';
                    if (JustCopy) {
                        Dst = Path.Combine(Path.GetDirectoryName(Dst), Path.GetFileName(Dst).Substring(1));
                    }
                    
                    if (File.Exists(Dst)) {
                        if (MessageBox.Show("The file '" + Dst + "' already exists - would you like to overwrite it?", "New Project", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return false;
                    }
                    
                    if (Path.GetFileName(Src)[0] == '!') {
                        File.Copy(Src, Dst);
                    } else {

                        using (TextReader T = new StreamReader(Src)) {
                            using (TextWriter W = new StreamWriter(Dst)) {
                                W.Write(T.ReadToEnd().Replace("$PROJECT_NAME$", this.ProjectName.Text));
                            }
                        }
                    }
                } catch (Exception ex) {
                    MessageBox.Show("Error copying file '" + F + "':\n" + ex.Message, "New Project", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            // Repeat for all subdirectories:
            string[] FoldersToCopy = Directory.GetDirectories(SourceFolder);
            foreach (String F in FoldersToCopy) {
                string Folder = F.Replace(SourceFolder + "\\", "");
                if (!CopyAllFilesAndFolders(Path.Combine(SourceFolder, Folder), Path.Combine(DestFolder, Folder))) {
                    return false;    
                }
            }
            return true;
        }

		private void CreateNewProject_Click(object sender, EventArgs e) {

            // Check for invalid characters:
            if (this.ProjectName.Text.IndexOfAny(("\\/:*?\"<>|").ToCharArray()) != -1) {
                MessageBox.Show("The project name cannot contain any characters not allowed as part of a standard Windows filename.", "New", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (this.ProjectFolder.Text.IndexOfAny(("*?\"<>|").ToCharArray()) != -1) {
                MessageBox.Show("The project folder cannot contain any characters not allowed as part of a standard Windows path.", "New", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


			// First up: Check to see if the required path is valid:
			string ProjectFolder = this.ProjectFolder.Text + "\\" + this.ProjectName.Text;
			if (Directory.Exists(ProjectFolder)) {
				DialogResult D = MessageBox.Show("A folder already exists at '" + ProjectFolder + "'\nWould you like to delete the folder?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
				if (D != DialogResult.Yes) return;
				try {
					Directory.Delete(ProjectFolder, true);
				} catch (Exception ex) {
					MessageBox.Show("Could not delete folder '" + ProjectFolder + "'\n" + ex.Message, "New Project", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				
			}
		
            // Copy!
            if (!CopyAllFilesAndFolders(Application.StartupPath + "\\Templates\\Projects\\" + ProjectPreview.SelectedItems[0].Text + "\\Files", ProjectFolder)) {
                MessageBox.Show("There was an error creating the new project.", "New Project", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

			// Try to load the shiny, new project!
			if (Program.MainIDE.LoadProjectFile(ProjectFolder + "\\" + this.ProjectName.Text + ".lnp")) {
				this.Dispose();
			}

		}

		private void button1_Click(object sender, EventArgs e) {
			FolderBrowser.SelectedPath = this.ProjectFolder.Text;
			DialogResult D = FolderBrowser.ShowDialog();
			if (D == DialogResult.OK) this.ProjectFolder.Text = FolderBrowser.SelectedPath;
		}
	}
}