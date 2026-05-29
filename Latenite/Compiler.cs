using System;
using System.Collections;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace Latenite {
	public partial class LateniteIDE  {

        /// <summary>
        /// Write a line to the output box.
        /// </summary>
        /// <param name="Line">Line of text to write.</param>
        public delegate void AddLineToOutputCallBack(string Line);
     	void AddLineToOutput(string Line) {

            if (InvokeRequired) {
                AddLineToOutputCallBack A = new AddLineToOutputCallBack(AddLineToOutput);
                Invoke(A, Line);
                return;
            }

            if (Line == null) return;
			if (Line.Trim() != "") {
				OutputTextBox.Text += Line + "\r\n";
			}
			Application.DoEvents();
			OutputTextBox.SelectionLength = 0;
			OutputTextBox.SelectionStart = OutputTextBox.Text.Length;
			OutputTextBox.ScrollToCaret();

		}

        void AddCharToOutput(char C) {
            OutputTextBox.Text += C;
            OutputTextBox.SelectionLength = 0;
            OutputTextBox.SelectionStart = OutputTextBox.Text.Length;

        }
        /// <summary>
        /// Set up the basic environment variables for a process.
        /// </summary>
        /// <param name="ProcessToSet">Which process you want to set the environment variables for.</param>
        /// <param name="SourceFile">Currently active source file.</param>
        void SetProcessEnvironmentVariables(ref Process ProcessToSet, string SourceFile, bool BuildOnesToo, string BuildFileName) {
            try {
                SourceFile = Path.GetFullPath(SourceFile);
                ProcessToSet.StartInfo.EnvironmentVariables.Add("SOURCE_PATH", SourceFile);
                ProcessToSet.StartInfo.EnvironmentVariables.Add("SOURCE_FILE", Path.GetFileName(SourceFile));
                ProcessToSet.StartInfo.EnvironmentVariables.Add("SOURCE_DIR", Path.GetDirectoryName(SourceFile));
                ProcessToSet.StartInfo.EnvironmentVariables.Add("SOURCE_FILE_EXT", Path.GetExtension(SourceFile).Replace(".", ""));
                ProcessToSet.StartInfo.EnvironmentVariables.Add("SOURCE_FILE_NOEXT", Path.GetFileNameWithoutExtension(SourceFile));
            } catch { }
        
            ProcessToSet.StartInfo.EnvironmentVariables.Add("COMPILE_DIR", Application.StartupPath + @"\Compile");
            ProcessToSet.StartInfo.EnvironmentVariables.Add("DEBUG_DIR", Application.StartupPath + @"\Debug");

            ProcessToSet.StartInfo.EnvironmentVariables.Add("PROJECT_PATH", GetProjectFilename());
            ProcessToSet.StartInfo.EnvironmentVariables.Add("PROJECT_FILE", Path.GetFileName(GetProjectFilename()));
            ProcessToSet.StartInfo.EnvironmentVariables.Add("PROJECT_DIR", Path.GetDirectoryName(GetProjectFilename()));

            ProcessToSet.StartInfo.EnvironmentVariables.Add("PROJECT_NAME", GetProjectName());
            ProcessToSet.StartInfo.EnvironmentVariables.Add("PROJECT_BINARY", GetBinaryName());

            string ErrorLog = Application.StartupPath + @"\Compile\Errors.xml";
            string DebugLog = Application.StartupPath + @"\Compile\Debug.xml";
            if (BuildOnesToo) {
                ProcessToSet.StartInfo.EnvironmentVariables.Add("BUILD_PATH", BuildFileName);
                ProcessToSet.StartInfo.EnvironmentVariables.Add("BUILD_FILE", Path.GetFileName(BuildFileName));
                ProcessToSet.StartInfo.EnvironmentVariables.Add("BUILD_DIR", Path.GetDirectoryName(BuildFileName));
                ProcessToSet.StartInfo.EnvironmentVariables.Add("BUILD_FILE_EXT", Path.GetExtension(BuildFileName).Replace(".", ""));
                ProcessToSet.StartInfo.EnvironmentVariables.Add("BUILD_FILE_NOEXT", Path.GetFileNameWithoutExtension(BuildFileName));
                ProcessToSet.StartInfo.EnvironmentVariables.Add("ERROR_LOG", ErrorLog);
                ProcessToSet.StartInfo.EnvironmentVariables.Add("DEBUG_LOG", DebugLog);
            }
        }

        void C_OutputDataReceived(object sender, DataReceivedEventArgs e) {
            AddLineToOutput(e.Data);
        }

        bool WasKilled = false;
        public Process BuildProcess;


        /// <summary>
        /// Build the currently loaded project.
        /// </summary>
        /// <param name="BuildFileName">Which build script to use.</param>
        public bool Build(string BuildFileName) {

            if (!File.Exists(BuildFileName)) {
                MessageBox.Show(this, "The build script " + BuildFileName + " does not exist.", "Error in build", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            string S = GetProjectSourceFile();
            if (S == "") {
                MessageBox.Show("This project doesn't have any source files to build!\nPlease select a file to build from the project properties dialog.", "Build Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }



            try {
                Directory.SetCurrentDirectory(Path.GetDirectoryName(GetProjectFilename()));
                S = Path.GetFullPath(S);
            } catch (Exception ex) {
                MessageBox.Show("Something went horribly wrong.\n" + ex.Message);
                return false;
            }

            this.BuildProgress.Style = ProgressBarStyle.Marquee;

            // Autosave:
            if (Properties.Settings.Default.Editor_AutoSave) {
                foreach (SourceFile ToSave in Program.MainIDE.SourceFiles.TabPages) {
                    if (ToSave.IsSaved && ToSave.IsDirty) ToSave.Save();
                }
            }

            OutputBox.SelectedTab = OutputTab;
            OutputTextBox.Text = "------ Building: Source File: " + Path.GetFileName(S) + ", Script: " + Path.GetFileName(BuildFileName) + " ------\r\n";
            ErrorListBox.Items.Clear();

            string ErrorLog = Application.StartupPath + @"\Compile\Errors.xml";
            string DebugLog = Application.StartupPath + @"\Compile\Debug.xml";

            if (File.Exists(ErrorLog)) {
                try {
                    File.Delete(ErrorLog);
                } catch (Exception ex) {
                    OutputTextBox.Text = "Error deleting error log (" + ex.Message.ToString() + ")\r\n";
                }
            }

            if (File.Exists(DebugLog)) {
                try {
                    File.Delete(DebugLog);
                } catch (Exception ex) {
                    OutputTextBox.Text = "Error deleting debug log (" + ex.Message.ToString() + ")\r\n";
                }
            }

            

            Directory.SetCurrentDirectory(Path.GetDirectoryName(GetProjectFilename()));

            BuildProcess = new Process();
            BuildProcess.StartInfo.FileName = BuildFileName;

            SetProcessEnvironmentVariables(ref BuildProcess, S, true, BuildFileName);
            

            BuildProcess.StartInfo.CreateNoWindow = true;
            BuildProcess.StartInfo.UseShellExecute = false;
            BuildProcess.StartInfo.RedirectStandardOutput = true;
            BuildProcess.StartInfo.RedirectStandardError = true;


            BuildProcess.ErrorDataReceived += new DataReceivedEventHandler(C_OutputDataReceived);
            BuildProcess.OutputDataReceived += new DataReceivedEventHandler(C_OutputDataReceived);

            ArrayList DisabledItems = LockInterface();
            LockTextFiles();
            

            try {
                Program.MainIDE.TerminateBuild.Enabled = true;
                WasKilled = false;
                BuildProcess.Start();
                BuildProcess.BeginErrorReadLine();
                BuildProcess.BeginOutputReadLine();

                while (!BuildProcess.HasExited && BuildProcess.Responding) {
                    Application.DoEvents();
                }

                BuildProcess.Close();


            } catch (Exception ex) {
                AddLineToOutput("Fatal Error: " + ex.Message);
                UnlockTextFiles();
                UnlockInterface(DisabledItems);
                return false;
            }

            
            Program.MainIDE.TerminateBuild.Enabled = false;
            if (WasKilled) {
                AddLineToOutput("------ Build Cancelled ------");
                UnlockTextFiles();
                UnlockInterface(DisabledItems);
                SaveProject();
                this.BuildProgress.Style = ProgressBarStyle.Blocks;
                return false;
            } else {
                AddLineToOutput("------ Build Process Complete ------");
            }

            int ErrCount = 0;
            XmlDocument XmlErrorLog = new XmlDocument();
            try {
                XmlErrorLog.Load(ErrorLog);

                XmlNodeList AllErrors = XmlErrorLog.GetElementsByTagName("error");

                
                int WarnCount = 0;
                for (int j = 0; j < 2; j++) {
                    for (int i = 0; i < AllErrors.Count; ++i) {

                        if (j == 0) {
                            ErrCount++;
                        } else {
                            WarnCount++;
                        }
                        XmlNode E = AllErrors.Item(i);

                        int ErrorLine = 0;
                        string ErrorFile = "";

                        try {
                            ErrorLine = Convert.ToInt32(E.Attributes.GetNamedItem("line").InnerText);
                        } catch { }

                        try {
                            ErrorFile = E.Attributes.GetNamedItem("file").InnerText;
                        } catch { }

                        ListViewItem L = new ListViewItem(E.InnerText);
                        L.ImageIndex = (j == 0) ? 3 : 2;
                        L.SubItems.Add(ErrorFile.Replace(Path.GetDirectoryName(S), "").Trim("/\\".ToCharArray()));
                        L.SubItems.Add(ErrorLine == 0 ? "" : ErrorLine.ToString());
                        ArrayList LT = new ArrayList();

                        LT.Add(Path.GetDirectoryName(S) + "\\" + ErrorFile.Replace(Path.GetDirectoryName(S), "").Trim("/\\".ToCharArray()));
                        LT.Add(ErrorLine);

                        L.Tag = LT;

                        ErrorListBox.Items.Add(L);
                    }
                    AllErrors = XmlErrorLog.GetElementsByTagName("warning");
                }


                XmlNodeList AllMessages = XmlErrorLog.GetElementsByTagName("message");

                for (int i = 0; i < AllMessages.Count; ++i) {
                    XmlNode E = AllMessages.Item(i);
                    AddLineToOutput(E.InnerText);
                }

                AddLineToOutput("========== Build: " + ErrCount + " error" + (ErrCount == 1 ? "" : "s") + ", " + WarnCount + " warning" + (WarnCount == 1 ? "" : "s") + " ==========");
                if (ErrCount != 0 || WarnCount != 0) {
                    OutputBox.SelectedTab = ErrorList;
                    if (ErrCount != 0) {
                        MessageBox.Show("Build Failed! (" + ErrCount + " error" + (ErrCount == 1 ? "" : "s") + ")", null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }

                ToolTip T = new ToolTip();
                T.ToolTipIcon = ErrCount != 0 ? ToolTipIcon.Error : WarnCount != 0 ? ToolTipIcon.Warning : ToolTipIcon.Info;
                T.IsBalloon = true;
                T.ToolTipTitle = "Build";
                string TCap = ErrCount + " error" + (ErrCount == 1 ? "" : "s") + ", " + WarnCount + " warning" + (WarnCount == 1 ? "" : "s");
                T.ShowAlways = true;
                System.Drawing.Rectangle R = OutputBox.GetTabRect((ErrCount + WarnCount == 0) ? 1 : 0);
                System.Drawing.Point P = OutputBox.PointToScreen(new System.Drawing.Point(R.Left, R.Top));
                T.SetToolTip(this, TCap);
                T.Show(TCap, this, P.X - this.Left + 8, P.Y - this.Top + 8, 2000);
            } catch (Exception ex) {
                AddLineToOutput("Could not read error report: " + ex.Message.ToString());
                MessageBox.Show("Could not parse error report:\n'" + ex.Message.ToString() + "'\nThis is a serious error; please check that your compile scripts are correctly installed.", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // We have our build folder!
            XmlAttribute IsDefault = ProjectFile.CreateAttribute("default");
            IsDefault.Value = "true";
            XmlAttribute IsNotDefault = ProjectFile.CreateAttribute("default");
            IsNotDefault.Value = "false";
            foreach (XmlNode F in ProjectFile.GetElementsByTagName("build")) {
                Application.DoEvents();
                try {
                    string X_N = Path.Combine(Path.GetDirectoryName(GetProjectFilename()), F.InnerText);
                    string B_N = BuildFileName;
                    if (X_N.ToLower() == B_N.ToLower()) {
                        F.Attributes.SetNamedItem(IsDefault);
                    } else {
                        F.Attributes.SetNamedItem(IsNotDefault);
                    }
                } catch { }

            }


            UnlockInterface(DisabledItems);
            UnlockTextFiles();
            SaveProject();
            this.BuildProgress.Style = ProgressBarStyle.Blocks;
            return ErrCount == 0;

        }

        public ArrayList LockInterface() {

            ArrayList DisabledItems = new ArrayList();
            foreach (Control C in this.Controls) {
                if (C.GetType() == typeof(MenuStrip)) {
                    foreach (object R in ((MenuStrip)C).Items) {
                        if (R.GetType() == typeof(ToolStripMenuItem)) {
                            foreach (object T in ((ToolStripMenuItem)R).DropDownItems) {
                                if (T.GetType() == typeof(ToolStripMenuItem)) {
                                    if (((ToolStripMenuItem)T).Enabled) {
                                        ((ToolStripMenuItem)T).Enabled = false;
                                        DisabledItems.Add(T);
                                    }
                                }
                            }
                        }
                    }
                } else if (C.GetType() == typeof(ToolStrip)) {
                    if (C.Enabled) {
                        C.Enabled = false;
                        DisabledItems.Add(C);
                    }
                }
            }
            return DisabledItems;
        }

        delegate void SetControlEnabledCallback(ArrayList DisabledItems);
        void UnlockInterface(ArrayList DisabledItems) {
            if (InvokeRequired) {
                SetControlEnabledCallback S = new SetControlEnabledCallback(UnlockInterface);
                this.Invoke(S, DisabledItems);
            } else {
                foreach (object C in DisabledItems) {
                    if (C.GetType() == typeof(ToolStripMenuItem)) {
                        ((ToolStripMenuItem)C).Enabled = true;
                    } else {
                        ((Control)C).Enabled = true;
                    }
                }
            }
        }

      

        /// <summary>
        /// Build a project using the default settings
        /// </summary>
        /// <returns>Success flag</returns>
        public bool BuildProject() {
            if (this.BuildTarget.SelectedItem == null) {
                MessageBox.Show("To build a project, you must have a build script selected in the target dropdown.\nIf there are no build scripts listed, add some from the Project Properties window.", "Build Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            BuildMenuOption ToBuild = (BuildMenuOption)this.BuildTarget.SelectedItem;
            return Build(ToBuild.Path);
        }

	}
}

