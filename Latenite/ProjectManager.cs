using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.ComponentModel;

namespace Latenite {
	public partial class LateniteIDE {

		/// <summary>
		/// Refresh the project tree view and build menus
		/// </summary>
		public void RefreshProjectView() {
			this.ProjectTree.Nodes.Clear();
			TreeNode N = new TreeNode("Project");
			PopulateProjectFolder(ProjectFile.ChildNodes[0], ref N);
			if (N.Nodes.Count >= 1) {
				this.ProjectTree.Nodes.Add(N.Nodes[0]);
			}
            RepopulateBuildMenus();
		}

        private void RepopulateBuildMenus() {
            // Repopulate the "build" menu(s).

            // Remove the old ones:
            ArrayList ToRemove = new ArrayList();
            for (int i = 0; i < buildToolStripMenuItem.DropDownItems.Count; ++i) {
                if (buildToolStripMenuItem.DropDownItems[i].Tag != null) {
                    ToRemove.Add(buildToolStripMenuItem.DropDownItems[i]);
                }
            }
            CompileDrops.Clear();
            CompileLists.Clear();
            toolStripButtonBuild.DropDownItems.Clear();
            foreach (ToolStripDropDownItem T in ToRemove) {
                buildToolStripMenuItem.DropDownItems.Remove(T);
            }

            if (ProjectFile.ChildNodes.Count != 0) {
                foreach (XmlNode X in ProjectFile.ChildNodes[0].ChildNodes) {
                    if (X.Name.ToLower() == "folder") {
                        try {
                            if (X.Attributes.GetNamedItem("name").Value.ToString().ToLower() == "build") {
                                foreach (XmlNode Y in X.ChildNodes) {
                                    if (Y.Name.ToLower() == "file") {
                                        bool IsDefault = false;
                                        try {
                                            if (Y.Attributes.GetNamedItem("default").Value.ToLower() == "true") {
                                                IsDefault = true;
                                            }
                                        } catch { }
                                        // We have found them!
                                        string BuildName = Y.Attributes.GetNamedItem("name").Value.ToString();
                                        buildToolStripMenuItem.DropDownItems.Add(Path.GetFileNameWithoutExtension(BuildName), null, new EventHandler(T_Click));

                                        buildToolStripMenuItem.DropDownItems[buildToolStripMenuItem.DropDownItems.Count - 1].Tag = BuildName;
                                        ((ToolStripMenuItem)buildToolStripMenuItem.DropDownItems[buildToolStripMenuItem.DropDownItems.Count - 1]).Checked = IsDefault;
                                        CompileDrops.Add(buildToolStripMenuItem.DropDownItems[buildToolStripMenuItem.DropDownItems.Count - 1]);

                                        toolStripButtonBuild.DropDownItems.Add(Path.GetFileNameWithoutExtension(BuildName), null, new EventHandler(D_Click));
                                        toolStripButtonBuild.DropDownItems[toolStripButtonBuild.DropDownItems.Count - 1].Tag = BuildName;
                                        ((ToolStripMenuItem)toolStripButtonBuild.DropDownItems[toolStripButtonBuild.DropDownItems.Count - 1]).Checked = IsDefault;
                                        CompileLists.Add(toolStripButtonBuild.DropDownItems[toolStripButtonBuild.DropDownItems.Count - 1]);

                                    }
                                }
                                return;
                            }
                        } catch { }
                    }
                }
            }
        }

		/// <summary>
		/// Populate a tree node from a folder (an XML node on the ProjectFile)
		/// </summary>
		/// <param name="Folder">The XML Node representing the folder</param>
		/// <param name="Node">The TreeNode to populate</param>
		void PopulateProjectFolder(XmlNode Folder, ref TreeNode Node) {
			TreeNode F = new TreeNode(Folder.Attributes.GetNamedItem("name").InnerText);
			F.Tag = Folder;

			bool FExpanded = true;
			foreach (XmlAttribute XA in Folder.Attributes) {
				if (XA.Name.ToLower() == "expanded") {
					if (XA.Value.ToLower() == "true") {
						FExpanded = true;
					} else {
						FExpanded = false;
					}
				}
			}
			F.ImageIndex = 0;
			F.StateImageIndex = 1;
			F.SelectedImageIndex = 1;

			ArrayList FilesToAdd = new ArrayList();

			foreach (XmlNode X in Folder.ChildNodes) {
				if (X.Name == "folder") {
					PopulateProjectFolder(X, ref F);
				} else if (X.Name == "file") {
					FilesToAdd.Add(X);
				}
			}

			foreach (XmlNode X in FilesToAdd) {
				try {
					string FN = Path.GetFileName(X.Attributes.GetNamedItem("name").InnerText);
					TreeNode N = new TreeNode(FN);
					N.Tag = X;

					// Icon:

					int IX = 0;
					bool FoundIcon = false;
					foreach (System.Drawing.Image I in FileIconsList.Images) {

						if (I.Tag != null && (I.Tag.ToString() == Path.GetExtension(FN).ToLower())) {
							N.ImageIndex = IX;
							N.SelectedImageIndex = IX;
							FoundIcon = true;
							break;
						}
						++IX;
					}

					if (!FoundIcon) {
						IconExtract IE = new IconExtract(FN);
						IE.IconBitmap.Tag = Path.GetExtension(FN).ToLower();
						FileIconsList.Images.Add(IE.IconBitmap);
						N.ImageIndex = IX;
						N.SelectedImageIndex = IX;
					}
					F.Nodes.Add(N);

                    foreach (XmlAttribute A in X.Attributes) {
                        if (A.Name.ToLower() == "open") {
                            if (A.Value.ToLower() == "true") {
                                try {
                                    string Filename = X.Attributes.GetNamedItem("name").Value;
                                    if (!CheckAlreadyOpen(Filename)) {
                                        SourceFile S = new SourceFile(this.SourceFiles, Filename);
                                    }
                                } catch { }
                            }
                        }
                    }


				} catch { }

			}

			if (FExpanded) F.Expand();
			Node.Nodes.Add(F);

		}

		public string GetProjectSourceFile() {
			XmlNodeList X = ProjectFile.GetElementsByTagName("file");
			string Filename = "";
			foreach (XmlNode Y in X) {
				try {
					if (Y.Attributes.GetNamedItem("build").Value.ToString().ToLower() == "true") {
						Filename = Path.GetDirectoryName(GetProjectFilename()) + "\\" + Y.Attributes.GetNamedItem("name").Value.ToString();
						return Filename;
					}
				} catch { }
			}
			return Filename;
		}

		private void ProjectTree_AfterExpand(object sender, TreeViewEventArgs e) {
			XmlAttribute Ex = ProjectFile.CreateAttribute("expanded");
			Ex.Value = e.Node.IsExpanded.ToString();
			((XmlNode)e.Node.Tag).Attributes.SetNamedItem(Ex);
			if (((XmlNode)e.Node.Tag).Name.ToLower() == "folder") {
				e.Node.ImageIndex = e.Node.IsExpanded ? 1 : 0;
				e.Node.SelectedImageIndex = e.Node.IsExpanded ? 1 : 0;
			}
			SaveProject();
		}

		public bool LoadProjectFile(string Filename) {
			try {
				ProjectFile.Load(Filename);
			} catch (Exception ex) {
				MessageBox.Show("There was an error trying to load to the project file:\n" + ex.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			return true;
		}


		public string GetProjectName() {
			try {
				return ProjectFile.ChildNodes[0].Attributes.GetNamedItem("name").Value.ToString();
			} catch {
				return "";			
			}
		}

		public string GetBinaryName() {
			try {
				return ProjectFile.ChildNodes[0].Attributes.GetNamedItem("binary").Value.ToString();
			} catch {
				return "";
			}
		}


		public void SaveProject() {
			try {
				ProjectFile.Save(GetProjectFilename());
			} catch (Exception ex) {
				MessageBox.Show("There was an error saving the project file:\n" + ex.Message,"Saving Project",MessageBoxButtons.OK,MessageBoxIcon.Error);			
			}
		}

		private void ProjectTree_AfterLabelEdit(object sender, NodeLabelEditEventArgs e) {
			if (e.Label == null) return;
			XmlNode X = (XmlNode)e.Node.Tag;
			XmlAttribute Ex = ProjectFile.CreateAttribute("name");
			Ex.Value = e.Label;

			if (X.Name.ToLower() == "folder" || X.Name.ToLower() == "project") {
				X.Attributes.SetNamedItem(Ex);
			} else {
				string CurrentName = Path.GetDirectoryName(GetProjectFilename()) + "\\" + X.Attributes.GetNamedItem("name").Value;
				string NewName = Path.GetDirectoryName(CurrentName) + "\\" + e.Label;
				try {

					File.Move(CurrentName, NewName);
				} catch (Exception ex) {
					MessageBox.Show("There was an error renaming the file:\n" + ex.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
					e.CancelEdit = true;
					return;
				}

				XmlAttribute ExName = ProjectFile.CreateAttribute("name");
                string Relative = NewName.Replace(Path.GetDirectoryName(GetProjectFilename()) + "\\", "");
				ExName.Value = Relative;
				((XmlNode)e.Node.Tag).Attributes.SetNamedItem(ExName);

                Directory.SetCurrentDirectory(Path.GetDirectoryName(GetProjectFilename()));
				foreach (SourceFile S in SourceFiles.TabPages) {
					if (Path.GetFullPath(S.Filename).ToLower() == Path.GetFullPath(CurrentName).ToLower()) {
						S.Filename = NewName;
						S.Text = Path.GetFileName(S.Filename) + (S.IsDirty ? "*" : "");
                        Console.WriteLine("Updated");
					}
				}
			}
			SaveProject();
            RepopulateBuildMenus();
		}

		private void ProjectContext_Opening(object sender, CancelEventArgs e) {
			if (ProjectTree.SelectedNode != null && ProjectTree.SelectedNode.Tag != null) {

				XmlNode X = (XmlNode)ProjectTree.SelectedNode.Tag;

                upToolStripMenuItem.Enabled = (X.PreviousSibling == null) ? false : true;
                downToolStripMenuItem.Enabled = (X.NextSibling == null) ? false : true;

				if (X.Name.ToLower() == "file") {
					ProjectContextOpenExternal.Enabled = true;
					ProjectContextOpenLatenite.Enabled = true;
					ProjectContextPathFull.Enabled = true;
					ProjectContextPathRelative.Enabled = true;
					fromComputerToolStripMenuItem.Enabled = true;
					fromProjectToolStripMenuItem.Enabled = true;
					addFileToolStripMenuItem.Enabled = false;
					addFolderToolStripMenuItem.Enabled = false;
				} else {
					ProjectContextOpenExternal.Enabled = false;
					ProjectContextOpenLatenite.Enabled = false;
					ProjectContextPathFull.Enabled = false;
					ProjectContextPathRelative.Enabled = false;
					fromComputerToolStripMenuItem.Enabled = false;
					if (X.Name.ToLower() == "project") {
						fromProjectToolStripMenuItem.Enabled = false;
                        upToolStripMenuItem.Enabled = false;
                        downToolStripMenuItem.Enabled = false;
					} else {
						fromProjectToolStripMenuItem.Enabled = true;
					}
					addFileToolStripMenuItem.Enabled = true;
					addFolderToolStripMenuItem.Enabled = true;
				}

			}
		}


		private string GetProjectSelFilename() {
			string F = "";
			XmlNode X = (XmlNode)ProjectTree.SelectedNode.Tag;
			if (X.Name.ToLower() != "file") return "";

			try {
				F = X.Attributes.GetNamedItem("name").Value;
			} catch (Exception ex) {
				MessageBox.Show("Error getting filename from project:\n" + ex.Message);
				return "";
			}

			return F;
		}

		private void AddFileToProjectFolder(string Filename, XmlNode X) {
            if (Filename == "") return;

			if (X.Name.ToLower() != "folder" && X.Name.ToLower() != "project") return;

			XmlNode NewChild = ProjectFile.CreateNode(XmlNodeType.Element, "file", null);

			XmlAttribute ExName = ProjectFile.CreateAttribute("name");
			string PF = GetProjectFilename();
			string SF = Filename;

			string Relative = SF.Replace(Path.GetDirectoryName(PF) + "\\", "");
			if (Path.IsPathRooted(Relative)) {
				DialogResult D = MessageBox.Show("The file you are trying to add does not fall under the root directory of the project - this means that if you move the project folder, the link to the file will be lost.\nWould you like to continue adding this file to the project?", null, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
				if (D == DialogResult.No) return;
			}

			ExName.Value = Relative;
			NewChild.Attributes.SetNamedItem(ExName);

			X.AppendChild(NewChild);
			RefreshProjectView();
			SaveProject();
		}

		private void addFileToolStripMenuItem_Click(object sender, EventArgs e) {
			if (ProjectTree.SelectedNode == null || ProjectTree.SelectedNode.Tag == null) return;
			DialogResult D = OpenFile.ShowDialog();
			if (D != DialogResult.OK) return;
			XmlNode X = (XmlNode)ProjectTree.SelectedNode.Tag;
			AddFileToProjectFolder(OpenFile.FileName, X);

		}

        /// <summary>
        /// Delete an item from the project (optionally from PC as well)
        /// </summary>
        /// <param name="DeleteFromComputer">Delete from PC as well?</param>
        /// <param name="SuppressDialogs">Stop showing the Are You Sure dialog?</param>
		private void DeleteItem(bool DeleteFromComputer, bool SuppressDialogs) {
			XmlNode X = (XmlNode)ProjectTree.SelectedNode.Tag;
            if (!SuppressDialogs) {
                DialogResult D = MessageBox.Show("Are you sure you want to delete this " + X.Name + " from the project" + (DeleteFromComputer ? " and your computer" : "") + "?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (D == DialogResult.No) return;
            }
			string F = GetProjectSelFilename();
            Directory.SetCurrentDirectory(Path.GetDirectoryName(GetProjectFilename()));
			if (DeleteFromComputer) {
				try {
					if (F == "") return;
					File.Delete(F);
				} catch (Exception ex) {
					MessageBox.Show("Error deleting the file:\n" + ex.Message, "Delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			X.RemoveAll();
			X.ParentNode.RemoveChild(X);
			RefreshProjectView();
			foreach (SourceFile S in SourceFiles.TabPages) {
				if (S.Filename.ToLower() == F.ToLower()) {
					S.IsDirty = false;
					S.Close(false);
				}
			}
		}

		private void ProjectContextOpenLatenite_Click(object sender, EventArgs e) {
			ProjectTree_NodeMouseDoubleClick(null, null);
		}

		private void ProjectContextOpenExternal_Click(object sender, EventArgs e) {
			if (ProjectTree.SelectedNode != null && ProjectTree.SelectedNode.Tag != null) {
				string F = GetProjectSelFilename();
                Directory.SetCurrentDirectory(Path.GetDirectoryName(GetProjectFilename()));
				if (F == "") return;
				System.Diagnostics.Process P = new System.Diagnostics.Process();
				P.StartInfo.FileName = F;
				P.StartInfo.Verb = "Edit";
				try {
					P.Start();
				} catch (Exception ex) {
					MessageBox.Show("There was an error opening the file:\n" + ex.Message, "External Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

			}
		}

		private void fromComputerToolStripMenuItem_Click(object sender, EventArgs e) {
			DeleteItem(true, false);
		}

		private void addFolderToolStripMenuItem_Click(object sender, EventArgs e) {
			if (ProjectTree.SelectedNode == null) return;

			TreeNode NewFolder = new TreeNode("New Folder");

			XmlNode NewChild = ProjectFile.CreateNode(XmlNodeType.Element, "folder", null);
			XmlAttribute ExName = ProjectFile.CreateAttribute("name");
			ExName.Value = "New Folder";
			NewChild.Attributes.SetNamedItem(ExName);
			NewFolder.Tag = NewChild;

			((XmlNode)ProjectTree.SelectedNode.Tag).AppendChild(NewChild);

			ProjectTree.SelectedNode.Nodes.Add(NewFolder);
			SaveProject();
			NewFolder.BeginEdit();

		}

		private void MoveToFolderClick(object sender, EventArgs e) {
            try {
                ArrayList A = (ArrayList)((ToolStripMenuItem)sender).Tag;
                if (((XmlNode)A[0]).Name.ToLower() == "file") {
                    DeleteItem(false, true);
                    AddFileToProjectFolder(A[1].ToString(), (XmlNode)(A[0]));
                } else {
                    // Now we need to move it.
                    ((XmlNode)(A[0])).AppendChild((XmlNode)ProjectTree.SelectedNode.Tag);
                    RefreshProjectView();
                    SaveProject();
                }

            } catch { }
		}

		private void DisplayMoveToFolders(XmlNode Folder, ref ToolStripDropDownItem Menu, string Filename) {
            if (Filename == "") {
                Menu.Enabled = false;
                return;
            }
			Menu.DropDownItems.Add(Folder.Attributes.GetNamedItem("name").InnerText, Latenite.Properties.Resources.folder_open_16_h, new EventHandler(MoveToFolderClick));
            
            ArrayList A = new ArrayList();
            A.Add(Folder);
            A.Add(Filename);
			Menu.DropDownItems[Menu.DropDownItems.Count - 1].Tag = A;
            
			foreach (XmlNode X in Folder.ChildNodes) {
				if (X.Name.ToLower() == "folder") {
					ToolStripDropDownItem S = (ToolStripDropDownItem)Menu.DropDownItems[Menu.DropDownItems.Count - 1];
					DisplayMoveToFolders(X, ref S, Filename);
				}

			}
		}


		private void ProjectTree_MouseDoubleClick(object sender, MouseEventArgs e) {
			ProjectContextOpenLatenite.PerformClick();
		}
        /// <summary>
        /// Get the path of the currently loaded project.
        /// </summary>
        /// <returns>A full path to the project file's XML document.</returns>
		public string GetProjectFilename() {
			if (ProjectFile == null) {
				return "";
			} else {
				return ProjectFile.BaseURI.Replace("file:///", "");
			}
		}

        /// <summary>
        /// Check to see if a file is already opened in the tab control
        /// </summary>
        /// <param name="Filename">Filename to check (can be relative to project file or absolute)</param>
        /// <returns>True if open, false if not.</returns>
		private bool CheckAlreadyOpen(string Filename) {
            Directory.SetCurrentDirectory(Path.GetDirectoryName(GetProjectFilename()));
			foreach (SourceFile S in SourceFiles.TabPages) {
				if (Path.GetFullPath(S.Filename).ToLower() == Path.GetFullPath(Filename).ToLower()) {
					return true;
				}
			}
			return false;
		}

	}
}
