using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Xml;
using BenRyves;

namespace Latenite {
	public class SourceFile : TabPage {
		TabControl MyBaseTab;

		public bool IsDirty = false;
		public bool IsSaved = false;
		public string Filename = "";

		public BenRyves.ColourfulEditor TextEditor;

        public void AdjustColours() {
            TextEditor.Font = Properties.Settings.Default.EditorFont;
            TextEditor.UseExternalProperties(Program.ColourTable, Program.SyntaxLookup, Program._Seperators, Program.SyntaxElements);
            TextEditor.ForeColor = Properties.Settings.Default.Editor_Text_Colour;
            TextEditor.BackColor = Properties.Settings.Default.Editor_Back_Colour;
            TextEditor.ForceRefresh();
        }


		private void InitialiseSourceFile(TabControl BaseTabControl) {
			MyBaseTab = BaseTabControl;

            LateniteIDE B = ((LateniteIDE)BaseTabControl.FindForm());

            this.TextEditor = new ColourfulEditor();

            Panel P = new Panel();
            P.BackColor = Color.FromKnownColor(KnownColor.ControlDark);
            P.Padding = new Padding(1);
            P.Controls.Add(this.TextEditor);
            P.Dock = DockStyle.Fill;

            this.TextEditor.Dock = DockStyle.Fill;
            this.Controls.Add(P);
            P.BringToFront();

            this.TextEditor.AcceptsTab = true;
            this.TextEditor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextEditor.EnableHighlighting = true;
            this.TextEditor.HideSelection = false;
            this.TextEditor.IntellisenseFont = (Font)Program.MainIDE.Font.Clone();
            this.TextEditor.Location = new System.Drawing.Point(0, 0);
            this.TextEditor.Name = "MainTextEditor";
            this.TextEditor.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.TextEditor.Size = new System.Drawing.Size(240, 193);
            this.TextEditor.Text = "";
            this.TextEditor.WordWrap = false;
            this.TextEditor.Font = Properties.Settings.Default.EditorFont;
            this.TextEditor.EnableHighlighting = true;
            this.TextEditor.DetectUrls = false;
            this.TextEditor.ContextMenuStrip = Program.MainIDE.TextEditorContextMenu;
            this.TextEditor.AutoIndent = Properties.Settings.Default.Editor_AutoIndent;
            TextEditor.TextChanged += new EventHandler(TextEditor_TextChanged);
            AdjustColours();
			this.Filename = this.Text;
            this.TextChanged += new EventHandler(SourceFile_TextChanged);
            Application.DoEvents();
            this.TextEditor.Dock = DockStyle.Fill;
            this.TextEditor.Refresh();
            this.TextEditor.AutoWordSelection = false;

            this.TextEditor.SelectionChanged += new EventHandler(TextEditor_SelectionChanged);

        }

        public void TextEditor_SelectionChanged(object sender, EventArgs e) {
            Program.MainIDE.StatusLineNumber.Text = "Ln " + ((int)this.TextEditor.GetCurrentLineNumber() + 1).ToString();
            Program.MainIDE.StatusColNumber.Text = "Col " + ((int)(this.TextEditor.SelectionStart - this.TextEditor.GetFirstCharIndexOfCurrentLine()) + 1).ToString();
        }


        delegate void Safe_TextChanged(object sender, EventArgs e);
        public void SourceFile_TextChanged(object sender, EventArgs e) {
            if (InvokeRequired) {
                Safe_TextChanged S = new Safe_TextChanged(SourceFile_TextChanged);
                this.Invoke(S, sender, e);
            } else {
                if (Program.MainIDE.TextFilesAreLocked) {
                    this.MyBaseTab.ImageList = Program.MainIDE.LockedIcon;
                    this.ImageIndex = 0;                  
                } else {
                    this.MyBaseTab.ImageList = Program.MainIDE.FileIconList;
                    this.ImageIndex = ((LateniteIDE)MyBaseTab.FindForm()).ProjectTree.GetIconIndex(Filename);
                }
                string[] AllowedHighlights = Properties.Settings.Default.Editor_Syntax_Files_To_Highlight.Replace("*", "").Split(new char[] { ';' });
                bool CanHighlight = false;
                foreach (string S in AllowedHighlights) {
                    if (S.ToLower() == Path.GetExtension(Filename).ToLower()) {
                        CanHighlight = true;
                        break;
                    }
                }
                if (CanHighlight != TextEditor.EnableHighlighting) {
                    TextEditor.EnableHighlighting = CanHighlight;
                }
            }
            
        }

        string OldText = "";
		void TextEditor_TextChanged(object sender, EventArgs e) {
            if (TextEditor.Text != OldText) {
                this.IsDirty = true;
                this.Text = Path.GetFileName(this.Filename) + "*";
            }
            OldText = TextEditor.Text;
        }

		public SourceFile(TabControl BaseTabControl) {
			InitialiseSourceFile(BaseTabControl);
            this.Filename = Path.Combine(Path.GetDirectoryName(Program.MainIDE.GetProjectFilename()), "Untitled" + ((LateniteIDE)MyBaseTab.FindForm()).UntitledCount++);
            MyBaseTab.TabPages.Add(this);
            MyBaseTab.SelectedTab = this;
            Application.DoEvents();
            TextEditor_TextChanged(null, null);
            Application.DoEvents();
            TextEditor_TextChanged(null, null);
            Application.DoEvents();            
            SourceFile_TextChanged(null, null);
            this.IsSaved = false;
            this.IsDirty = false;
            this.Text = Path.GetFileName(this.Filename);
            this.TextEditor.Locked = Program.MainIDE.TextFilesAreLocked;

		}


        public delegate void SetNewFilenameCallback(string NewFileName);
        public void SetNewFilename(string NewFileName) {
            if (InvokeRequired) {
                SetNewFilenameCallback S = new SetNewFilenameCallback(SetNewFilename);
                Invoke(S, NewFileName);
                return;
            }
            // Update the filename:
            this.Filename = NewFileName;
            this.Text = Path.GetFileName(NewFileName) + (IsDirty ? "*" : "");
        }

        public delegate void ReloadCallback();
        public void Reload() {
            if (InvokeRequired) {
                ReloadCallback R = new ReloadCallback(Reload);
                Invoke(R);
                return;
            }
            if (this.IsDirty) {
                DialogResult D = MessageBox.Show("The file " + Filename + " has been modified by an external editor.\nWould you like to revert to the newly saved version?", "File Changed", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (D == DialogResult.No) return;
            }

            string FileContents = "";
            try {
                using (TextReader T = new StreamReader(this.Filename)) {
                    FileContents = T.ReadToEnd();
                }
            } catch (Exception ex) {
                MessageBox.Show("There was an error opening the file '" + this.Filename + "'\n" + ex.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.TextEditor.Text = FileContents;
            TextEditor.ForceRefresh();
            this.IsDirty = false;
            this.Text = Path.GetFileName(Filename);
        }

		public SourceFile(TabControl BaseTabControl, string OpenFilename) {

            InitialiseSourceFile(BaseTabControl);
            this.Filename = OpenFilename;
            this.Text = Path.GetFileName(OpenFilename);

            this.TextEditor.Visible = false;

            TextEditor.Text = "";
            string NewText = "";
			try {
                using (TextReader T = new StreamReader(OpenFilename)) {
                    NewText = T.ReadToEnd();
                }
			} catch (Exception ex) {
				MessageBox.Show("There was an error opening the file '" + OpenFilename.ToString() + "'\n" + ex.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
				IsDirty = false;
				Close(false);
				return;
			}

            TextEditor.Text = NewText;
            TextEditor.ForceRefresh();

            this.TextEditor.Visible = true;

            this.Text = Path.GetFileName(OpenFilename);

			
			this.IsDirty = false;
			this.IsSaved = true;
			this.TextEditor.DeselectAll();

            this.TextEditor.SelectionLength = 0;
            this.TextEditor.SelectionStart = 0;
            this.TextEditor.DeselectAll();
            this.TextEditor.Locked = Program.MainIDE.TextFilesAreLocked;

            MyBaseTab.TabPages.Add(this);
            MyBaseTab.SelectedTab = this;
		}


        
        /// <summary>
        /// Highlight a line in the source file
        /// </summary>
        /// <param name="LineNumber">Index of the line to highlight</param>
		public void SelectLine(int LineNumber) {
            TextEditor.HighlightLine(LineNumber - 1);
		}

        /// <summary>
        /// Pop up a save as dialog for the file.
        /// </summary>
		public void SaveAs() {
			LateniteIDE L = (LateniteIDE)this.FindForm();
            L.SaveFile.FileName = this.Filename;
			DialogResult R =  L.SaveFile.ShowDialog();
			if (R == DialogResult.OK) {
				try {
                    this.Filename = L.SaveFile.FileName.ToString();
                    using (TextWriter T = new StreamWriter(this.Filename)) {
                        string TextToSave = Properties.Settings.Default.Editor_CRLF ? this.TextEditor.Text.Replace("\r", "").Replace("\n", "\r\n") : this.TextEditor.Text;
                        T.Write(TextToSave);
                    }
					this.IsSaved = true;
					this.IsDirty = false;
                    SaveBreakpoints();
					this.Text = Path.GetFileName(this.Filename);
				} catch (Exception ex) {
					MessageBox.Show("Error saving file '" + L.SaveFile.FileName.ToString() + "'\n" + ex.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

        private void SaveBreakpoints() {
            /*int NumLines = this.TextEditor.Lines.Length;
            // Get rid of old breakpoints:
            XmlNodeList FileNodes = Program.MainIDE.ProjectFile.GetElementsByTagName("file");
            XmlNode FileXml = null;
            string ProjectFolder = Path.GetDirectoryName(Program.MainIDE.GetProjectFilename());
            foreach (XmlNode X in FileNodes) {
                foreach (XmlAttribute A in X.Attributes) {
                    if (A.Name.ToLower() == "name" && Path.Combine(ProjectFolder, A.Value.ToString()).ToLower() == this.Filename.ToLower()) {
                        FileXml = X;
                        break;
                    }
                }                
            }

            if (FileXml == null) ;

            ArrayList BreakpointedLines = new ArrayList();
            for (int i = 0; i < NumLines; ++i) {
                if (TextEditor.CheckIfLineIsHighlighted(i)) {
                }
            }*/
        }

		public void Save() {
			if (this.IsSaved) {
				try {
                    using (TextWriter T = new StreamWriter(this.Filename)) {
                        string TextToSave = Properties.Settings.Default.Editor_CRLF ? this.TextEditor.Text.Replace("\r", "").Replace("\n", "\r\n") : this.TextEditor.Text;
                        T.Write(TextToSave);
                    }
					this.IsSaved = true;
					this.IsDirty = false;
                    SaveBreakpoints();
					this.Text = Path.GetFileName(this.Filename);
				} catch (Exception ex) {
					MessageBox.Show("Error saving file '" + this.Filename + "'\n" + ex.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			} else {
				SaveAs();
			}
		}

        /// <summary>
        /// Close a source file and remove it from the tab control.
        /// </summary>
        /// <param name="AppIsClosing">Set to True if the IDE is being closed to stop the file from being marked as completely closed.</param>
		public void Close(bool AppIsClosing) {
			if (IsDirty) {
				DialogResult D = MessageBox.Show("The file '" + Path.GetFileName(this.Filename) + "' has not been saved since it was last edited - would you like to save it now?", "Closing", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
				if (D == DialogResult.Cancel) {
					return;
				} else if (D == DialogResult.Yes) {
					Save();
					Close(AppIsClosing);
					return;
				}
			}

			// Close!
            //if (!AppIsClosing) SetOpenStatus(false);
			if (this.TextEditor != null) this.TextEditor.Dispose();
			try {
				this.MyBaseTab.TabPages.Remove(this);
			} catch { }
			
		}

        /// <summary>
        /// Get the word at the current cursor location
        /// </summary>
        /// <returns>The word at the current cursor location</returns>
		public string GetCurrentWord() {
			int CurrentStringA = this.TextEditor.SelectionStart;
            int CurrentStringB = this.TextEditor.SelectionStart+1; // +this.TextEditor.SelectionLength;
			string Terminators = "()# .,:;\t\n\r\"";
			char[] T = TextEditor.Text.ToCharArray();
			while (CurrentStringA > 1) {
				if (Terminators.IndexOf(T[CurrentStringA-1]) != -1) break;
				--CurrentStringA;
			}

			while (CurrentStringB < T.LongLength) {
				if (Terminators.IndexOf(T[CurrentStringB]) != -1) break;
				++CurrentStringB;
			}
			return this.TextEditor.Text.Substring(CurrentStringA, CurrentStringB - CurrentStringA);

		}
	}
}
