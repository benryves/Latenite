using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Diagnostics;
using Latenite.Properties;

namespace Latenite {
	public partial class LateniteIDE : Form {

        public TreeFileBrowser ProjectTree;

        private TabPage OldTabPageA = null;
        private TabPage OldTabPageB = null;

        public LateniteIDE() {
			InitializeComponent();

            Application.DoEvents();

            DisabledWhenNoFileIsOpen.Add(saveToolStripMenuItem);
            DisabledWhenNoFileIsOpen.Add(saveAsToolStripMenuItem);
            DisabledWhenNoFileIsOpen.Add(saveAllToolStripMenuItem);
            DisabledWhenNoFileIsOpen.Add(undoToolStripMenuItem);
            DisabledWhenNoFileIsOpen.Add(redoToolStripMenuItem);
            DisabledWhenNoFileIsOpen.Add(cutToolStripMenuItem);
            DisabledWhenNoFileIsOpen.Add(copyToolStripMenuItem);
            DisabledWhenNoFileIsOpen.Add(cutToolStripMenuItem);
            DisabledWhenNoFileIsOpen.Add(selectAllToolStripMenuItem);
            DisabledWhenNoFileIsOpen.Add(closeToolStripMenuItem);
            DisabledWhenNoFileIsOpen.Add(pasteToolStripMenuItem);
            DisabledWhenNoFileIsOpen.Add(deleteToolStripMenuItem);
            DisabledWhenNoFileIsOpen.Add(goToToolStripMenuItem);
            DisabledWhenNoFileIsOpen.Add(findToolStripMenuItem);

            DisabledWhenNoFileIsOpenButtons.Add(toolStripButtonSave);
            DisabledWhenNoFileIsOpenButtons.Add(toolStripButtonSaveAll);
            DisabledWhenNoFileIsOpenButtons.Add(toolStripButtonUndo);
            DisabledWhenNoFileIsOpenButtons.Add(toolStripButtonRedo);
            DisabledWhenNoFileIsOpenButtons.Add(toolStripButtonCut);
            DisabledWhenNoFileIsOpenButtons.Add(toolStripButtonCopy);
            DisabledWhenNoFileIsOpenButtons.Add(toolStripButtonPaste);


            //
            this.ProjectTree = new Latenite.TreeFileBrowser(this.FileIconList);
            this.ProjectTab.Controls.Add(this.ProjectTree);
            this.ProjectTree.ContextMenuStrip = this.ProjectContext;
            this.ProjectTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProjectTree.FullRowSelect = true;
            this.ProjectTree.ImageIndex = 0;
            this.ProjectTree.ItemHeight = 19;
            this.ProjectTree.LabelEdit = true;
            this.ProjectTree.Location = new System.Drawing.Point(3, 3);
            this.ProjectTree.Name = "ProjectTree";
            this.ProjectTree.Pattern = "*.*";
            this.ProjectTree.SelectedImageIndex = 0;
            this.ProjectTree.Size = new System.Drawing.Size(197, 268);
            this.ProjectTree.TabIndex = 0;
            this.ProjectTree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.ProjectTree_NodeMouseDoubleClick_1);

            try {
                this.HelpSplitContainer.SplitterDistance = this.HelpSplitContainer.Width - Settings.Default.IDE_Help_Size;
            } catch{ }
            try {
                this.ErrorSplitContainer.SplitterDistance = this.ErrorSplitContainer.Height - Settings.Default.IDE_Output_Size;
            } catch { }

            MainToolStrip.Left = 3;
            DebugAndBuild.Left = MainToolStrip.Left + MainToolStrip.Width;

		}


        public ArrayList HelpList = new ArrayList();

        private ArrayList DisabledWhenNoFileIsOpen = new ArrayList();
        private ArrayList DisabledWhenNoFileIsOpenButtons = new ArrayList();

        private void ExpandDefaultFolders(TreeNode Root) {
            foreach (TreeNode N in Root.Nodes) {
                if (N.Tag != null) {
                    foreach (XmlNode X in ProjectFile.GetElementsByTagName("expandedfolder")) {
                        if (N.Tag.ToString().ToLower() == (Path.GetDirectoryName(GetProjectFilename()) + X.InnerText).ToLower()) {
                            N.Expand();
                            break;
                        }
                    }
                }
                ExpandDefaultFolders(N);
            }
        }


		public int UntitledCount = 1;

		public XmlDocument ProjectFile = new XmlDocument();

        void PopulateToolsMenu(ref ToolStripMenuItem ToolsMenu, string Folder) {
            try {
                string[] Subfolders = Directory.GetDirectories(Folder);
                foreach (String S in Subfolders) {
                    string CaptionName = Path.GetFileName(S);
                    if (!CaptionName.ToLower().StartsWith("hidden!")) {
                        ToolStripMenuItem T = new ToolStripMenuItem(CaptionName);
                        ToolsMenu.DropDownItems.Add(T);
                        PopulateToolsMenu(ref T, S);
                    }                    
                }
            } catch { }
            try {
                string[] Tools = Directory.GetFiles(Folder);
                foreach (string S in Tools) {
                    string ToolName = Path.GetFileNameWithoutExtension(S);
                    if (ToolName.StartsWith("!")) ToolName = ToolName.Substring(1);
                    ToolsMenu.DropDownItems.Add(ToolName, null, new EventHandler(ToolClicked));
                    ToolsMenu.DropDownItems[ToolsMenu.DropDownItems.Count - 1].Tag = Path.GetFullPath(S);
                }
            } catch { }
        }

		private void LateniteIDE_Load(object sender, EventArgs e) {



            // Recall size:
            try {
                WindowState = Properties.Settings.Default.IDE_WindowState;
                ClientSize = Properties.Settings.Default.IDE_ClientSize;
            } catch { }

            ProjectTree.Pattern = Properties.Settings.Default.Project_Filter;
            this.Show();
            Application.DoEvents();
            this.ProjectTree.FixIconCache();
			

			Application.DoEvents();
			this.SearchMode.SelectedIndex = 0;
            try { this.HelpFilesCombo.SelectedIndex = 0; } catch { }
            try { this.HelpItemCombo.SelectedIndex = 0; } catch { }
            SourceFiles_ControlRemoved(null, null);
            // Tools menu:

            PopulateToolsMenu(ref toolsToolStripMenuItem, Application.StartupPath + @"\Tools");

            //this.SourceFiles.ImageList = this.ProjectTree.IconCache;

            // Set multiple lines of tabs:
            this.SourceFiles.Multiline = Properties.Settings.Default.IDE_Multiple_Tabs;

            // Expand saved expanded folders:
            ExpandDefaultFolders(ProjectTree.Nodes[0]);
            ProjectTree.Nodes[0].Expand();

            Application.DoEvents();

            // Open default files:
            try {
                SourceFile Default = null;
                foreach (XmlNode X in ProjectFile.GetElementsByTagName("openfile")) {
                    try {
                        SourceFile S = new SourceFile(this.SourceFiles, Path.Combine(Path.GetDirectoryName(GetProjectFilename()), X.InnerText));
                        foreach (XmlAttribute A in X.Attributes) {
                            if (A.Name.ToLower() == "default" && A.Value.ToLower() == "true") Default = S;
                        }
                    } catch { }
                }
                if (Default != null) SourceFiles.SelectedTab = Default;

            } catch { }

            
            

        }

        private void ToolClicked(object sender, EventArgs e) {
            Process P = new Process();

            string ToolName = ((ToolStripDropDownItem)sender).Tag.ToString();
            P.StartInfo.FileName = ToolName;

            string Ext = Path.GetExtension(ToolName).ToLower();

            if (Ext == ".exe" || Ext == ".com" || Ext == ".cmd" || Ext == ".bat") {
                P.StartInfo.UseShellExecute = false;
                SourceFile S = GetActiveSourceFile();
                string FN = (S == null) ? "" : S.Filename;
                Directory.SetCurrentDirectory(Path.GetDirectoryName(ToolName));
                SetProcessEnvironmentVariables(ref P, FN, false, "");
            }

            if (!Path.GetFileName(ToolName).StartsWith("!")) {
                P.StartInfo.CreateNoWindow = true;
            }
            
            
            try {
                P.Start();
            } catch (Exception ex) {
                MessageBox.Show("Error running tool:\n" + ex.Message, "Tools", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


		/*void UpdateChecksOnBothBuildMenus(ToolStripMenuItem sender) {
			foreach (ToolStripMenuItem T in CompileDrops) {
				if (sender.Text == T.Text) {
					T.Checked = true;
				} else {
					T.Checked = false;
				}
			}
			foreach (ToolStripMenuItem T in CompileLists) {
				if (sender.Text == T.Text) {
					T.Checked = true;
				} else {
					T.Checked = false;
				}
			}
		}*/

		/*void D_Click(object sender, EventArgs e) {
			UpdateChecksOnBothBuildMenus((ToolStripMenuItem)sender);
            defaultPlatformToolStripMenuItem.PerformClick();
		}*/

		/*void T_Click(object sender, EventArgs e) {
			string BuildFile = ((ToolStripMenuItem)sender).Tag.ToString();
			Build(BuildFile);
			UpdateChecksOnBothBuildMenus((ToolStripMenuItem)sender);
		}*/


		private void deleteToolStripMenuItem_Click(object sender, EventArgs e) {
            GetActiveSourceFile().TextEditor.Delete();
			
		}



		private void ErrorListBox_SelectedIndexChanged(object sender, EventArgs e) {
			if (ErrorListBox.SelectedItems.Count == 0) return;
			ArrayList ErrorDetails = (ArrayList)ErrorListBox.SelectedItems[0].Tag;

			if (ErrorDetails[0].ToString() == "" || ErrorDetails[1].ToString() == "0") return;

			SourceFile FileToJumpTo = null;




            if (!CheckAlreadyOpen(ErrorDetails[0].ToString())) {
                try {
                    FileToJumpTo = new SourceFile(this.SourceFiles, ErrorDetails[0].ToString());
                } catch (Exception ex) {
                    MessageBox.Show("There was an error trying to jump to the error:\n" + ex.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            } else {
                foreach (SourceFile S in this.SourceFiles.TabPages) {
                    if (Path.GetFullPath(S.Filename).ToLower() == Path.GetFullPath(ErrorDetails[0].ToString()).ToLower()) {
                        FileToJumpTo = S;
                        break;
                    }
                }
            }

			FileToJumpTo.SelectLine(Convert.ToInt32(ErrorDetails[1]));
			SourceFiles.SelectedTab = FileToJumpTo;
		}

		private void toolStripButtonUndo_Click(object sender, EventArgs e) {
			undoToolStripMenuItem.PerformClick();
		}


		private void ProjectContextRename_Click(object sender, EventArgs e) {
			if (ProjectTree.SelectedNode != null) ProjectTree.SelectedNode.BeginEdit();
		}

		private void ProjectTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e) {
			if (ProjectTree.SelectedNode != null && ProjectTree.SelectedNode.Tag != null) {
				string F = GetProjectSelFilename();
				if (F == "") return;
				if (!CheckAlreadyOpen(F)) {
					SourceFile S = new SourceFile(SourceFiles, F);
				} else {
					foreach (SourceFile S in SourceFiles.TabPages) {
						if (S.Filename.ToLower() == F.ToLower()) {
							SourceFiles.SelectedTab = (TabPage)S;
							return;
						}						
					}
				}
		
			}
		}

        private void WriteOpenFolders(TreeNode Root) {
            foreach (TreeNode T in Root.Nodes) {
                if (T.IsExpanded) {
                    XmlNode X = ProjectFile.CreateElement("expandedfolder");
                    X.InnerText = T.Tag.ToString().Replace(Path.GetDirectoryName(GetProjectFilename()), "");
                    ProjectFile.DocumentElement.AppendChild(X);
                    WriteOpenFolders(T);
                }
            }
        }

		private void LateniteIDE_FormClosing(object sender, FormClosingEventArgs e) {




            
            ArrayList OldWorkspaces = new ArrayList();
            foreach (XmlNode X in ProjectFile.GetElementsByTagName("openfile")) {
                OldWorkspaces.Add(X);                
            }
            foreach (XmlNode X in ProjectFile.GetElementsByTagName("expandedfolder")) {
                OldWorkspaces.Add(X);
            }
            foreach (XmlNode X in OldWorkspaces) {
                try {
                    X.ParentNode.RemoveChild(X);
                } catch { }
            }

            foreach (SourceFile S in SourceFiles.TabPages) {
                if (S.IsSaved) {
                    XmlNode X = ProjectFile.CreateElement("openfile");
                    X.InnerText = S.Filename.Replace(Path.GetDirectoryName(GetProjectFilename()) + "\\", "");
                    if (SourceFiles.SelectedTab == S) {
                        XmlAttribute A = ProjectFile.CreateAttribute("default");
                        A.Value = "True";
                        X.Attributes.SetNamedItem(A);
                    }
                    ProjectFile.DocumentElement.AppendChild(X);
                }
            }

            WriteOpenFolders(ProjectTree.Nodes[0]);

            foreach (SourceFile S in SourceFiles.TabPages) {
                S.Close(true);
            }
            if (SourceFiles.TabCount > 0) return;

            SaveProject();


			if (SourceFiles.TabPages.Count != 0) {
				foreach (SourceFile S in SourceFiles.TabPages) {
					if (S.IsSaved) e.Cancel = true;
				}
			}
            try {
                Properties.Settings.Default.IDE_WindowState = WindowState;
                    this.WindowState = FormWindowState.Normal;
                    Application.DoEvents();
                Properties.Settings.Default.IDE_ClientSize = ClientSize;
                
                Properties.Settings.Default.IDE_Location = this.Location;
            } catch { }
 
            
            
		}

		private void setEditorFontToolStripMenuItem_Click(object sender, EventArgs e) {
			
		}
        /// <summary>
        /// Open a colour picker.
        /// </summary>
        /// <param name="ColourToPick">Reference to a colour you want to change</param>
		private void ColourPick(ref Color ColourToPick) {
			ColourDialog.Color = ColourToPick;
			DialogResult D = ColourDialog.ShowDialog();
			if (D != DialogResult.OK) return;
			ColourToPick = ColourDialog.Color;
		}
        /// <summary>
        /// Update all the source files and set the correct colours.
        /// </summary>
        private void UpdateColours() {
            foreach (SourceFile S in SourceFiles.TabPages) {
                S.TextEditor.ForeColor = Properties.Settings.Default.Editor_Text_Colour;
                S.TextEditor.BackColor = Properties.Settings.Default.Editor_Back_Colour;
            }
        }

		private void setEditorTextColourToolStripMenuItem_Click(object sender, EventArgs e) {
            Color X = Properties.Settings.Default.Editor_Text_Colour;
			ColourPick(ref X);
            Properties.Settings.Default.Editor_Text_Colour = X;
            UpdateColours();
		}

		private void setEditorBackgroundColourToolStripMenuItem_Click(object sender, EventArgs e) {
            Color X = Properties.Settings.Default.Editor_Back_Colour;
			ColourPick(ref X);
            Properties.Settings.Default.Editor_Back_Colour = X;
            UpdateColours();
		}






		private void copyIncludeFileToolStripMenuItem_DropDownOpened(object sender, EventArgs e) {
		}

		private void fileToolStripMenuItem_DropDownOpened(object sender, EventArgs e) {
            /*
			// Now we need to populate...
            this.copyIncludeFileToolStripMenuItem.Enabled = true;
			this.copyIncludeFileToolStripMenuItem.DropDownItems.Clear();
			ToolStripDropDownItem S = (ToolStripDropDownItem)this.copyIncludeFileToolStripMenuItem;
            DisplayMoveToFolders(ProjectFile.ChildNodes[0], ref S, GetActiveSourceFile().IsSaved ? GetActiveSourceFile().Filename : "");
            */

		}

		private void MainToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {

		}

		private void propertiesToolStripMenuItem_Click(object sender, EventArgs e) {
			ProjectProperties P = new ProjectProperties(this);
			P.ShowDialog();
		}



        private void SourceFiles_SelectedIndexChanged(object sender, EventArgs e) {
            OldTabPageA = OldTabPageB;
            OldTabPageB = SourceFiles.SelectedTab;
            RightClickedTabPage = SourceFiles.SelectedTab;
            ((SourceFile)SourceFiles.SelectedTab).TextEditor_SelectionChanged(this, null);

        }

		private void SourceFiles_ControlRemoved(object sender, ControlEventArgs e) {

            if (OldTabPageA != null && SourceFiles.Contains(OldTabPageA)) {
                SourceFiles.SelectedTab = OldTabPageA;
            }
            if (SourceFiles.TabCount == 0 || (e != null && SourceFiles.TabCount == 1)) {
                //newToolStripMenuItem.PerformClick();

                foreach (ToolStripMenuItem I in DisabledWhenNoFileIsOpen) {
                    I.Enabled = false;                    
                }
                foreach (ToolStripButton B in DisabledWhenNoFileIsOpenButtons) {
                    B.Enabled = false;
                }

            }
		}

		private void ProjectTree_AfterSelect(object sender, TreeViewEventArgs e) {

		}


		private void toolStripButtonBuild_Click(object sender, EventArgs e) {
			defaultPlatformToolStripMenuItem.PerformClick();
		}


		private void saveAllToolStripMenuItem_Click(object sender, EventArgs e) {
			foreach (SourceFile S in SourceFiles.TabPages) {
				S.Save();				
			}
		}

		private void findToolStripMenuItem_Click(object sender, EventArgs e) {
			this.OutputBox.SelectedTab = this.FinderTab;
			this.SearchTextBox.SelectAll();
			this.SearchTextBox.Focus();

		}


		private void SearchTextBox_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter) {
				this.SearchButton.PerformClick();
			}
		}


		private void toolStripButtonOpen_Click(object sender, EventArgs e) {
			openToolStripMenuItem.PerformClick();
		}

		private void toolStripButtonSave_Click(object sender, EventArgs e) {
			saveToolStripMenuItem.PerformClick();
		}

		private void toolStripButtonSaveAll_Click(object sender, EventArgs e) {
			saveAllToolStripMenuItem.PerformClick();
		}

		private void toolStripButtonCut_Click(object sender, EventArgs e) {
			cutToolStripMenuItem.PerformClick();
		}

		private void toolStripButtonCopy_Click(object sender, EventArgs e) {
			copyToolStripMenuItem.PerformClick();
		}

		private void toolStripButtonPaste_Click(object sender, EventArgs e) {
			pasteToolStripMenuItem.PerformClick();
		}


		private void SearchResults_Click(object sender, EventArgs e) {
			SearchResults_SelectedIndexChanged(null, null);
		}

		private void toolStripButtonNew_ButtonClick(object sender, EventArgs e) {
            this.newToolStripMenuItem.PerformClick();
		}



		private void redoToolStripMenuItem_Click(object sender, EventArgs e) {
			try {
                if (AmInTextEditor()) GetActiveSourceFile().TextEditor.Redo();
			} catch { }
		}

		private void toolStripButtonRedo_Click(object sender, EventArgs e) {
			redoToolStripMenuItem.PerformClick();
		}

		private void redoToolStripMenuItem1_Click(object sender, EventArgs e) {
			redoToolStripMenuItem.PerformClick();
		}


		private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Close();
		}

		private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) {
			try {
				GetActiveSourceFile().TextEditor.SelectAll();
			} catch { }
		}

		private void undoToolStripMenuItem1_Click(object sender, EventArgs e) {
			undoToolStripMenuItem.PerformClick();
		}

		private void cutToolStripMenuItem1_Click(object sender, EventArgs e) {
			cutToolStripMenuItem.PerformClick();
		}

		private void copyToolStripMenuItem1_Click(object sender, EventArgs e) {
			copyToolStripMenuItem.PerformClick();
		}

		private void pasteToolStripMenuItem1_Click(object sender, EventArgs e) {
			pasteToolStripMenuItem.PerformClick();
		}

		private void deleteToolStripMenuItem1_Click(object sender, EventArgs e) {
			deleteToolStripMenuItem.PerformClick();
		}

		private void selectAllToolStripMenuItem1_Click(object sender, EventArgs e) {
			selectAllToolStripMenuItem.PerformClick();
		}

		private void ErrorListBox_Click(object sender, EventArgs e) {
            ErrorListBox_SelectedIndexChanged(sender, e);
		}


		private void lookUpToolStripMenuItem_Click(object sender, EventArgs e) {
			try {
                GetActiveSourceFile().TextEditor.Select(GetActiveSourceFile().TextEditor.GetCharIndexFromPosition(GetActiveSourceFile().TextEditor.LastMouseDown), 0);
				SearchHelp(GetActiveSourceFile().GetCurrentWord());
			} catch { }
		}

        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            DialogResult D = this.OpenFile.ShowDialog();
            if (D == DialogResult.OK) {
                foreach (string S in OpenFile.FileNames) {
                    if (!CheckAlreadyOpen(S)) {
                        SourceFile NS = new SourceFile(this.SourceFiles, S);
                    }
                }
            }
        }

		private void blankFileToolStripMenuItem_Click(object sender, EventArgs e) {
			SourceFile S = new SourceFile(this.SourceFiles);
		}

		public SourceFile GetActiveSourceFile() {
			return (SourceFile)(this.SourceFiles.SelectedTab);
		}

		private void closeToolStripMenuItem_Click(object sender, EventArgs e) {
			try {
				GetActiveSourceFile().Close(false);
			} catch { }
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
			try {
				GetActiveSourceFile().Save();
			} catch { }
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
			try {
				GetActiveSourceFile().SaveAs();
			} catch { }
		}

		private void undoToolStripMenuItem_Click(object sender, EventArgs e) {
			try {
                if (AmInTextEditor()) GetActiveSourceFile().TextEditor.Undo();
			} catch { }
		}

		private void cutToolStripMenuItem_Click(object sender, EventArgs e) {
			try {
                if (AmInTextEditor()) GetActiveSourceFile().TextEditor.Cut();
			} catch { }
		}

		private void copyToolStripMenuItem_Click(object sender, EventArgs e) {
			try {
                if (AmInTextEditor()) GetActiveSourceFile().TextEditor.Copy();
			} catch { }

		}

		private void pasteToolStripMenuItem_Click(object sender, EventArgs e) {
			try {
                if (AmInTextEditor()) GetActiveSourceFile().TextEditor.Paste();
			} catch { }
		}

        private void ProjectContextPathFull_Click(object sender, EventArgs e) {
            Directory.SetCurrentDirectory(Path.GetDirectoryName(GetProjectFilename()));
            Clipboard.SetText(Path.GetFullPath(GetProjectSelFilename()).Replace('\\','/'));            
        }

        private void ProjectContextPathRelative_Click(object sender, EventArgs e) {
            Clipboard.SetText(GetProjectSelFilename().Replace('\\','/'));
        }

        private void toolStripButtonNew_Click(object sender, EventArgs e) {
            newToolStripMenuItem.PerformClick();
        }

        private void ProjectTree_MouseDown(object sender, MouseEventArgs e) {
            TreeNode N = ProjectTree.GetNodeAt(e.X, e.Y);
            if (N==null) return;
            ProjectTree.SelectedNode = N;
        }

        private void lateniteIDEHelpToolStripMenuItem_Click(object sender, EventArgs e) {
            Process P = new Process();
            P.StartInfo.UseShellExecute = true;
            P.StartInfo.FileName = Path.Combine(Application.StartupPath, @"\Help\IDE\index.htm");
            try {
                P.Start();
            } catch (Exception ex) {
                MessageBox.Show("Could not start help system:\n" + ex.Message);
            }

        }

        /// <summary>
        /// Move an project node up/down in the project tree.
        /// </summary>
        /// <param name="MoveUp">Move the node up (rather than down).</param>
        private void SwapNodesUpDown(bool MoveUp) {
            XmlNode X = (XmlNode)(ProjectTree.SelectedNode.Tag);
            if (X == null) return;
            if (X.Name.ToLower() == "project") return;
            XmlNode Y = MoveUp ? X.PreviousSibling : X.NextSibling;
            if (Y != null) {
                XmlNode NewX = X.Clone();
                XmlNode NewY = Y.Clone();
                XmlNode P = X.ParentNode;
                if (P != null) {
                    P.ReplaceChild(NewY, X);
                    P.ReplaceChild(NewX, Y);
                }
                
                RefreshProjectView();
            }
            
        }

        private void upToolStripMenuItem_Click(object sender, EventArgs e) {
            SwapNodesUpDown(true);
        }

        private void downToolStripMenuItem_Click(object sender, EventArgs e) {
            SwapNodesUpDown(false);
        }

        private void ProjectAndHelpTabs_SelectedIndexChanged(object sender, EventArgs e) {
            if (ProjectAndHelpTabs.SelectedTab == HelpTab) {
                if (HelpBrowser.DocumentText == "") {
                    HelpFilesCombo.SelectedIndex = 0;
                    if (HelpItemCombo.Items.Count > 0) {
                        HelpItemCombo.SelectedIndex = 0;
                        HelpItemCombo_SelectedIndexChanged(null, null);
                    }
                }
            }
        }

        private void aboutLateniteToolStripMenuItem_Click(object sender, EventArgs e) {
            About X = new About();
            X.ShowDialog();
        }

        private void goToToolStripMenuItem_Click(object sender, EventArgs e) {
            GoToLine G = new GoToLine();
            G.ShowDialog(this);
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e) {
            if (ProjectTree.SelectedNode != null) ProjectTree.SelectedNode.BeginEdit();
            
        }

        private void OpenSpecialFile(TreeNode ToOpen) {
            TreeFileBrowser.SpecialSubFile S = (TreeFileBrowser.SpecialSubFile)ToOpen.Tag;
            if (S.Source != "") {
                try {
                    Process P = new Process();
                    P.StartInfo.FileName = Path.GetFullPath(S.Source);
                    P.StartInfo.Arguments = S.Args;
                    Directory.SetCurrentDirectory(Path.GetDirectoryName(S.Source));
                    P.Start();
                } catch (Exception ex) {
                    MessageBox.Show(this, "Could not open this resource:\n" + ex.Message, "Open Resource", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string FixPath(string RelativePath) {
            return Path.Combine(Path.GetDirectoryName(Application.StartupPath), RelativePath);
        }

        private bool AmInTextEditor() {
            foreach (SourceFile S in SourceFiles.TabPages) {
                if (S.TextEditor.Focused) return true;                
            }
            return false;
        }

        private void openInLateniteToolStripMenuItem_Click(object sender, EventArgs e) {

            TreeNode T = ProjectTree.SelectedNode;
            if (T == null) return;

            if (ProjectTree.IsSpecialSubFile(T)) {
                OpenSpecialFile(T);
            } else {

                string[] Extensions = Latenite.Properties.Settings.Default.Project_Filter.Split(';');
                string Extension = Path.GetExtension((string)T.Tag);
                bool ForLatenite = false;
                foreach (string Check in Extensions) {
                    if (Check.Replace("*", "").ToLower() == Extension.ToLower()) {
                        ForLatenite = true;
                        break;
                    }
                }

                if (ForLatenite) {

                    if (CheckAlreadyOpen((string)T.Tag)) {
                        foreach (SourceFile S in SourceFiles.TabPages) {
                            if (S.Filename.ToLower() == ((string)T.Tag).ToLower()) {
                                SourceFiles.SelectedTab = S;
                                break;
                            }
                        }
                    } else {
                        SourceFile S = new SourceFile(SourceFiles, (string)T.Tag);
                    }
                } else {
                    try {
                        Process P = new Process();
                        P.StartInfo.FileName = (string)T.Tag;
                        P.Start();
                    } catch {
                        MessageBox.Show(this, "Latenite could not open the selected file.", "Open File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void ProjectContext_Opening(object sender, CancelEventArgs e) {
            TreeNode T = ProjectTree.SelectedNode;
            if (T == null) {
                e.Cancel = true;
                return;
            }
            if (ProjectTree.IsSpecialSubFile(T)) {
                TreeFileBrowser.SpecialSubFile S = (TreeFileBrowser.SpecialSubFile)T.Tag;
                openInLateniteToolStripMenuItem.Text = "Open in " + S.Program;
                openInLateniteToolStripMenuItem.Enabled = S.Source != "";
                editInExternalEditorToolStripMenuItem.Enabled = false;
                deleteToolStripMenuItem2.Enabled = false;
                renameToolStripMenuItem.Enabled = false;
                newFolderToolStripMenuItem.Enabled = false;
                relativePathToClipboardToolStripMenuItem.Enabled = false;
                absolutePathToClipboardToolStripMenuItem.Enabled = false;
            } else {
                relativePathToClipboardToolStripMenuItem.Enabled = true;
                absolutePathToClipboardToolStripMenuItem.Enabled = true;
                deleteToolStripMenuItem2.Enabled = true;
                renameToolStripMenuItem.Enabled = true;
                openInLateniteToolStripMenuItem.Text = "Open";
                newFolderToolStripMenuItem.Enabled = true;
                if (T.ImageIndex == 1 || T.ImageIndex == 2) {
                    openInLateniteToolStripMenuItem.Enabled = false;
                    if (T == ProjectTree.Nodes[0]) {
                        relativePathToClipboardToolStripMenuItem.Enabled = false;
                    } else {
                        relativePathToClipboardToolStripMenuItem.Enabled = true;
                    }
                    editInExternalEditorToolStripMenuItem.Text = "Open in Explorer";
                } else {
                    openInLateniteToolStripMenuItem.Enabled = true;
                    relativePathToClipboardToolStripMenuItem.Enabled = true;
                    editInExternalEditorToolStripMenuItem.Text = "Open in External Editor";
                }
            }

        }


        private void ProjectTree_NodeMouseDoubleClick_1(object sender, TreeNodeMouseClickEventArgs e) {
            if (e.Node.Nodes.Count != 0) return;
            ProjectContext_Opening(null, null);

            openInLateniteToolStripMenuItem.PerformClick();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e) {
            Options O = new Options();
            O.ShowDialog();
        }

        private void SourceFiles_ControlAdded(object sender, ControlEventArgs e) {
            foreach (ToolStripMenuItem I in DisabledWhenNoFileIsOpen) {
                I.Enabled = true;
            }
            foreach (ToolStripButton B in DisabledWhenNoFileIsOpenButtons) {
                B.Enabled = true;
            }

        }

        private void deleteToolStripMenuItem2_Click(object sender, EventArgs e) {
            TreeNode T = ProjectTree.SelectedNode;
            if (T == null) return;
            if (MessageBox.Show("Are you sure you want to delete" + ((T.ImageIndex == 1 || T.ImageIndex == 2) ? " the folder (including all subfolders and files)" : " the file") + " \"" + T.Tag.ToString().Replace(Path.GetDirectoryName(GetProjectFilename()) + "\\", "") + "\"?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
            try {
                if (T.ImageIndex == 1 || T.ImageIndex == 2) {
                    Directory.Delete(T.Tag.ToString(), true);
                } else {
                    File.Delete(T.Tag.ToString());
                }
                TreeNode P = T.Parent;
                if (P != null) ProjectTree.PopulateFolder(ref P);
            } catch (Exception ex) {
                MessageBox.Show("There was an error deleting the file:\n" + ex.Message, "Delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void absolutePathToClipboardToolStripMenuItem_Click(object sender, EventArgs e) {
            TreeNode T = this.ProjectTree.SelectedNode;
            if (T != null) Clipboard.SetText(T.Tag.ToString().Replace('\\', '/'));
        }

        private void relativePathToClipboardToolStripMenuItem_Click(object sender, EventArgs e) {
            TreeNode T = this.ProjectTree.SelectedNode;
            if (T != null && T.Tag!=null) Clipboard.SetText(T.Tag.ToString().Replace(Path.GetDirectoryName(GetProjectFilename()) + "\\", "").Replace('\\', '/'));
        }

        private void editInExternalEditorToolStripMenuItem_Click(object sender, EventArgs e) {
            TreeNode T = this.ProjectTree.SelectedNode;
            
            if (T != null && T.Tag != null) {
                if (ProjectTree.IsSpecialSubFile(T)) {
                    OpenSpecialFile(T);
                } else {
                    Process P = new Process();
                    if (T.ImageIndex == 1 || T.ImageIndex == 2) {
                        P.StartInfo.FileName = "explorer.exe";
                        P.StartInfo.Arguments = "\"" + T.Tag.ToString() + "\"";
                    } else {
                        P.StartInfo.FileName = T.Tag.ToString();
                        P.StartInfo.Verb = "edit";
                    }

                    try {
                        P.Start();
                    } catch (Exception ex) {
                        MessageBox.Show("Could not edit file:\n" + ex.Message, "Edit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void newFolderToolStripMenuItem_Click(object sender, EventArgs e) {
            TreeNode T = ProjectTree.SelectedNode;
            if (T == null || T.Tag == null) return;
            if (!(T.ImageIndex == 1 || T.ImageIndex == 2)) {
                T = T.Parent;
            }
            if (T == null || T.Tag == null) return;

            using (NewDirectory N = new NewDirectory("Enter a Folder Name:")) {
                if (N.ShowDialog() == DialogResult.OK) {
                    try {

                        string RootPath = T.Tag.ToString();
                        Directory.CreateDirectory(Path.Combine(RootPath, N.DirectoryName.Text));
                        ProjectTree.PopulateFolder(ref T);
                    } catch (Exception ex) {
                        MessageBox.Show("There was an error creating the directory:\n" + ex.Message,"New Folder" , MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
            
        }


        private void HelpFilesCombo_SelectedIndexChanged(object sender, EventArgs e) {
            HelpItemCombo.Items.Clear();
            foreach (HelpFile H in HelpList) {
                if (H.ContainsHelp && (HelpFilesCombo.SelectedIndex == 0 || H.Name == HelpFilesCombo.SelectedItem.ToString())) {
                    H.PopulateItemsCombo(this.HelpItemCombo);
                }
            }
        }

        private void HelpItemCombo_SelectedIndexChanged(object sender, EventArgs e) {
            /*foreach (HelpFile H in HelpList) {
                if (HelpFilesCombo.SelectedIndex == 0 || H.Name == HelpFilesCombo.SelectedItem.ToString()) {
                    XmlNodeList X = H.HelpFileXML.GetElementsByTagName("item");
                    for (int i = 0; i < X.Count; ++i) {
                        XmlNode E = X.Item(i);
                        string ItemName = "";
                        try {
                            ItemName = E.Attributes.GetNamedItem("name").InnerText;
                        } catch { }
                        if (ItemName == this.HelpItemCombo.SelectedItem.ToString()) {
                            DisplayHelpFile(E);
                            break;
                        }
                    }
                }
            }*/
            HelpItemFile H = (HelpItemFile)HelpItemCombo.SelectedItem;
            DisplayHelpFile(H.HelpItem);
        }


        private void SearchButton_Click(object sender, EventArgs e) {
            if (this.SearchTextBox.Text == "") return;
            this.SearchResults.Items.Clear();
            if (this.SearchMode.SelectedIndex == 0) {
                if (SourceFiles.TabCount == 0) {
                    MessageBox.Show("You cannot use this search mode unless you have a source file open", "Search", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                } else {
                    TextSearch(this.SearchTextBox.Text, GetActiveSourceFile().TextEditor.Text, GetActiveSourceFile().Filename, this.SearchMatchCase.Checked);
                }
            } else if (this.SearchMode.SelectedIndex == 1) {
                foreach (TabPage T in this.SourceFiles.TabPages) {
                    SourceFile S = (SourceFile)T;
                    TextSearch(this.SearchTextBox.Text, S.TextEditor.Text, S.Filename, this.SearchMatchCase.Checked);
                }
            } else {

                ArrayList FilesToSearch = new ArrayList();
                string[] Patterns = Properties.Settings.Default.Project_Filter.Split(";".ToCharArray());
                foreach (string P in Patterns) {
                    FilesToSearch.AddRange(Directory.GetFiles(Path.GetDirectoryName(GetProjectFilename()), P, SearchOption.AllDirectories));
                }

                foreach (string F in FilesToSearch) {
                    Application.DoEvents(); // Time to breathe :)
                    try {
                        if (CheckAlreadyOpen(F)) {
                            foreach (SourceFile S in SourceFiles.TabPages) {
                                if (Path.GetFullPath(S.Filename).ToLower() == Path.GetFullPath(F).ToLower()) {
                                    TextSearch(SearchTextBox.Text, S.TextEditor.Text, F, SearchMatchCase.Checked);
                                    break;
                                }
                            }

                        } else {
                            using (TextReader T = new StreamReader(File.Open(F, FileMode.Open))) {
                                TextSearch(SearchTextBox.Text, T.ReadToEnd(), F, SearchMatchCase.Checked);
                            }
                        }
                    } catch { }
                }
            }
        }

        private delegate void JumpToLineInFileCallback(string Filename, int LineNumber);

        private void JumpToLineInFile(string Filename, int LineNumber) {
            if (SourceFiles.InvokeRequired) {
                JumpToLineInFileCallback J = new JumpToLineInFileCallback(JumpToLineInFile);
                SourceFiles.Invoke(J, Filename, LineNumber);
                return;
            }
            SourceFile FileToJumpTo = null;
            Filename = Path.GetFullPath(Filename).ToLower().Replace('/', '\\');
            foreach (SourceFile S in this.SourceFiles.TabPages) {
                if (Path.GetFullPath(S.Filename).ToLower().Replace('/', '\\') == Filename) {
                    FileToJumpTo = S;
                    break;
                }
            }

            if (FileToJumpTo == null) {
                try {
                    FileToJumpTo = new SourceFile(this.SourceFiles, Filename);
                } catch (Exception ex) {
                    MessageBox.Show("There was an error trying to jump to the line result:\n" + ex.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            FileToJumpTo.SelectLine(LineNumber);
            SourceFiles.SelectedTab = FileToJumpTo;
        }


        private void SearchResults_SelectedIndexChanged(object sender, EventArgs e) {
            if (SearchResults.SelectedItems.Count == 0) return;
            JumpToLineInFile((string)SearchResults.SelectedItems[0].Tag, int.Parse(SearchResults.SelectedItems[0].SubItems[2].Text));

            
        }

        /// <summary>
        /// Capture links clicked in the helpfile window - those starting with lnh_X are treated as special links.
        /// </summary>
        private void HelpBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e) {
            string Link = e.Url.OriginalString.Replace("about:blank", "");
			Link = Link.Replace("about:", "");
			if (Link.StartsWith("lnh_")) {
                e.Cancel = true;
                SearchHelp(Link.Substring(4));
            }
        }

        private void TabContext_Opening(object sender, CancelEventArgs e) {
            if (RightClickedTabPage == null || !SourceFiles.TabPages.Contains(RightClickedTabPage)) return;
            SourceFile S = (SourceFile)RightClickedTabPage;
            if (S.IsSaved) {
                this.TabCtxCopyFull.Enabled = true;
                this.TabCtxCopyRelative.Enabled  = true;
                this.TabCtxOpen.Enabled = true;
            } else {
                this.TabCtxCopyFull.Enabled = false;
                this.TabCtxCopyRelative.Enabled = false;
                this.TabCtxOpen.Enabled = false;
            }
        }

        private TabPage RightClickedTabPage = null;
        private void SourceFiles_MouseDown(object sender, MouseEventArgs e) {
            Point P = new Point(e.X,e.Y);
            for (int i = 0; i < SourceFiles.TabCount; i++) {
                Rectangle R = SourceFiles.GetTabRect(i);
                if (R.Contains(P)) {
                    RightClickedTabPage = SourceFiles.TabPages[i];
                    break;
                }
            }

        }

        private void TabCtxSave_Click(object sender, EventArgs e) {
            if (RightClickedTabPage == null || !SourceFiles.TabPages.Contains(RightClickedTabPage)) return;
            ((SourceFile)RightClickedTabPage).Save();
        }

        private void TabCtxClose_Click(object sender, EventArgs e) {
            if (RightClickedTabPage == null || !SourceFiles.TabPages.Contains(RightClickedTabPage)) return;
            ((SourceFile)RightClickedTabPage).Close(false);
        }

        private void TabCtxOpen_Click(object sender, EventArgs e) {
            if (RightClickedTabPage == null || !SourceFiles.TabPages.Contains(RightClickedTabPage) || !((SourceFile)RightClickedTabPage).IsSaved) return;
            Process P = new Process();
            P.StartInfo.FileName = "explorer";
            P.StartInfo.Arguments = Path.GetDirectoryName(((SourceFile)RightClickedTabPage).Filename);
            try {
                P.Start();
            } catch { }
        }

        private void TabCtxCloseAll_Click(object sender, EventArgs e) {
            if (RightClickedTabPage == null || !SourceFiles.TabPages.Contains(RightClickedTabPage)) return;
            foreach (SourceFile S in SourceFiles.TabPages) {
                if (S != RightClickedTabPage) S.Close(false);
                
            }
        }

        private void TabCtxCopyFull_Click(object sender, EventArgs e) {
            if (RightClickedTabPage == null || !SourceFiles.TabPages.Contains(RightClickedTabPage) || !((SourceFile)RightClickedTabPage).IsSaved) return;
            Clipboard.SetText(((SourceFile)RightClickedTabPage).Filename.Replace('\\', '/'));
        }

        private void TabCtxCopyRelative_Click(object sender, EventArgs e) {
            if (RightClickedTabPage == null || !SourceFiles.TabPages.Contains(RightClickedTabPage) || !((SourceFile)RightClickedTabPage).IsSaved) return;
            Clipboard.SetText(((SourceFile)RightClickedTabPage).Filename.Replace(Path.GetDirectoryName(GetProjectFilename()) + @"\", "").Replace('\\', '/'));

        }

        private void TerminateBuild_Click(object sender, EventArgs e) {
            try {
                KillProcessFully(ref BuildProcess);
                WasKilled = true;
            } catch (Exception ex) {
                MessageBox.Show("Could not terminate build:\n" + ex.Message, "Build", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        public bool TextFilesAreLocked = false;
        private void LockTextFiles() {
            TextFilesAreLocked = true;
            foreach (SourceFile S in SourceFiles.TabPages) {
                S.TextEditor.Locked = true;
                S.SourceFile_TextChanged(this, null);
            }            
        }
        private void UnlockTextFiles() {
            TextFilesAreLocked = false;
            foreach (SourceFile S in SourceFiles.TabPages) {
                S.TextEditor.Locked = false;
                S.SourceFile_TextChanged(this, null);
            }            
        }

        private void toggleBreakpointToolStripMenuItem_Click(object sender, EventArgs e) {
            if (!GetActiveSourceFile().TextEditor.CheckIfLineIsHighlighted(GetActiveSourceFile().TextEditor.GetCurrentLineNumber())) {
                GetActiveSourceFile().TextEditor.HighlightLine(GetActiveSourceFile().TextEditor.GetCurrentLineNumber(), Latenite.Properties.Settings.Default.Debug_Breakpoint_Back, Latenite.Properties.Settings.Default.Debug_Breakpoint_Fore);
            } else {
                GetActiveSourceFile().TextEditor.UnHighlightLine(GetActiveSourceFile().TextEditor.GetCurrentLineNumber());
            }
        }

        private void toolStripStartDebug_Click(object sender, EventArgs e) {
            startDebuggingToolStripMenuItem.PerformClick();
        }

        /// <summary>
        /// Set the new default build/debug target:
        /// </summary>
        private void BuildTarget_SelectedIndexChanged(object sender, EventArgs e) {

            // Get all the old build script details
            XmlNodeList BuildScripts = ProjectFile.GetElementsByTagName("build");

            // Go through the scripts one at a time:
            foreach (XmlNode BuildScript in BuildScripts) {

                string TestFileName = Path.Combine(Path.GetDirectoryName(GetProjectFilename()), BuildScript.InnerText).ToLower();

                // Remove the old default=? attribute
                XmlAttribute ToRemove = null;
                foreach (XmlAttribute CheckIfDefault in BuildScript.Attributes) {
                    if (CheckIfDefault.Name.ToLower() == "default") {
                        ToRemove = CheckIfDefault;
                        break;
                    }
                }

                // Can we remove it?
                if (ToRemove != null) BuildScript.Attributes.Remove(ToRemove);

                // Make a new default attribute:
                XmlAttribute NewDefaultAttribute = ProjectFile.CreateAttribute("default");

                NewDefaultAttribute.Value =
                    (((BuildMenuOption)BuildTarget.SelectedItem).Path.ToLower() == TestFileName) ?
                        "true" : "false";

                BuildScript.Attributes.Append(NewDefaultAttribute);

            }
            // Finally, save the project away.
            SaveProject();
        }

        /// <summary>
        ///  Start a build:
        /// </summary>
        private void defaultPlatformToolStripMenuItem_Click(object sender, EventArgs e) {
            BuildProject();
        }

        private void BuildProgress_VisibleChanged(object sender, EventArgs e) {
            this.IdeStatusBar.ResumeLayout(true);
        }

        private void ProjectAndHelpTabs_SelectedIndexChanged_1(object sender, EventArgs e) {
			foreach (Control C in this.IeBorder.Controls) C.Dispose();
			this.IeBorder.Controls.Clear();
            HelpBrowser = new WebBrowser();
            this.HelpBrowser.AllowWebBrowserDrop = false;
            this.HelpBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HelpBrowser.Location = new System.Drawing.Point(1, 1);
            this.HelpBrowser.Name = "HelpBrowser";
            this.HelpBrowser.ScriptErrorsSuppressed = true;
            this.HelpBrowser.Size = new System.Drawing.Size(229, 240);
            this.HelpBrowser.TabIndex = 1;
            this.HelpBrowser.WebBrowserShortcutsEnabled = false;
            this.HelpBrowser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.HelpBrowser_Navigating);
            this.IeBorder.Controls.Add(HelpBrowser);
            HelpItemCombo_SelectedIndexChanged(null, null);
        }

        private void ErrorSplitContainer_SplitterMoved(object sender, SplitterEventArgs e) {
            // Save new position
            Settings.Default.IDE_Output_Size = ErrorSplitContainer.Height - ErrorSplitContainer.SplitterDistance;
        }

        private void HelpSplitContainer_SplitterMoved(object sender, SplitterEventArgs e) {
            // Save new position
            Settings.Default.IDE_Help_Size = HelpSplitContainer.Width - HelpSplitContainer.SplitterDistance;
        }

        private void MainToolStrip_EndDrag(object sender, EventArgs e) {
            SaveMenuLayout();
        }
        private void SaveMenuLayout() {
            foreach (Control C in IdeToolstrips.TopToolStripPanel.Controls) {
                MessageBox.Show(C.Location.ToString());
            }
            
        }

        private void LateniteIDE_ResizeEnd(object sender, EventArgs e) {
            HelpSplitContainer_SplitterMoved(null, null);
            ErrorSplitContainer_SplitterMoved(null, null);
        }

	}
}