using System;
using System.Collections;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using BenRyves;

namespace Latenite {
	public partial class LateniteIDE  {
        public static Process Debugger;


        private void startDebuggingToolStripMenuItem_Click(object sender, EventArgs e) {
            if (Debugger == null || Debugger.HasExited) {
                if (!BuildProject()) {
                    return;
                } else {
                    string DebuggerPath = "";
                    string DebuggerArgs = "";
                    XmlDocument DebugInfo = new XmlDocument();
                    try {
                        DebugInfo.Load(Application.StartupPath + @"\Compile\Debug.xml");
                        XmlNodeList GetInfo = DebugInfo.GetElementsByTagName("debug");
                        foreach (XmlNode DebugInfoNode in GetInfo) {
                            foreach (XmlAttribute GetAttribute in DebugInfoNode.Attributes) {
                                if (GetAttribute.Name.ToLower() == "debugger") {
                                    DebuggerPath = GetAttribute.Value;
                                } else if (GetAttribute.Name.ToLower() == "debugger_args") {
                                    DebuggerArgs = GetAttribute.Value;
                                }

                            }
                        }
                    } catch (Exception ex) {
                        MessageBox.Show("Could not get debugger start information:\n" + ex.Message, "Debug", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    Debugger = new Process();
                    CommandQueue = new Queue();
                    try {
                        SetProcessEnvironmentVariables(ref Debugger, GetProjectSourceFile(), true, ((BuildMenuOption)this.BuildTarget.SelectedItem).Path);
                        Debugger.StartInfo.FileName = DebuggerPath;
                        Debugger.StartInfo.Arguments = DebuggerArgs;
                        Debugger.StartInfo.UseShellExecute = false;
                        Debugger.StartInfo.RedirectStandardInput = true;
                        Debugger.StartInfo.RedirectStandardOutput = true;
                        Debugger.EnableRaisingEvents = true;
                        Debugger.Exited += new EventHandler(Debugger_Exited);
                        Debugger.OutputDataReceived += new DataReceivedEventHandler(Debugger_OutputDataReceived);
                        Directory.SetCurrentDirectory(Path.GetDirectoryName(DebuggerPath));
                        Debugger.Start();
                        Debugger.BeginOutputReadLine();
                    } catch (Exception ex) {
                        MessageBox.Show("Could not start debugger:\n" + ex.Message, "Debug", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    LockTextFiles();
                    DebugLockedInterface = LockInterface();
                    CommandQueue.Enqueue("ENUMERATE");
                    WatchRecords.Clear();
                    WatchRecords.Add(new WatchRecord("hl"));
                    WatchRecords.Add(new WatchRecord("noname.plotsscreen"));
                    foreach (WatchRecord WR in WatchRecords) {
                        CommandQueue.Enqueue("WATCH " + WR.Name + " 2 2");
                    }

                }
            } else {
                CommandQueue.Enqueue("PLAY");
            }
        }

        private Queue CommandQueue;

        delegate void Enabler(ref object ToEnable);
        private void Enable(ref object ToEnable) {
            if (InvokeRequired){
                Enabler E = new Enabler(Enable);
                this.Invoke(E, ToEnable);
            } else {
                if (ToEnable.GetType() == typeof(ToolStripMenuItem)) {
                    ((ToolStripMenuItem)ToEnable).Enabled = true;
                } else {
                    ((ToolStripButton)ToEnable).Enabled = true;
                }
            }
        }

        public bool DebuggerTips = false;

        void Debugger_OutputDataReceived(object sender, DataReceivedEventArgs e) {
            if (e.Data == null) return;
            string Message = e.Data.ToString();
            string[] Commands = Message.ToUpper().Split(' ');


            switch (Commands[0]) {
                case "MESSAGE":
                    MessageBox.Show(Message.Substring(8), "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case "ICAN":
                    object ToEnable;
                    switch (Message.Substring(5).ToUpper()) {
                        case "PLAY":
                            ToEnable = startDebuggingToolStripMenuItem;
                            Enable(ref ToEnable);
                            ToEnable = toolStripStartDebug;
                            Enable(ref ToEnable);
                            break;
                        case "PAUSE":
                            ToEnable = pauseDebuggingToolStripMenuItem;
                            Enable(ref ToEnable);
                            break;
                        case "QUIT":
                            ToEnable = stopDebuggingToolStripMenuItem;
                            Enable(ref ToEnable);
                            break;
                        case "BREAKPOINT":
                            ToEnable = startDebuggingToolStripMenuItem;
                            Enable(ref ToEnable);
                            break;
                        case "TOOLTIP":
                            DebuggerTips = true;
                            break;
                    }
                    break;
                case "REQUEST":
                    CommandQueue.Enqueue(Message.Substring(8));
                    break;
                case "TOOLTIP":
                    if (Message.Length == 7) {
                        AwaitTooltip = " ";
                    } else {
                        AwaitTooltip = Message.Substring(8).Replace("[NL]", "\n");
                        if (AwaitTooltip == "") AwaitTooltip = " ";
                    }
                    break;
                case "GET":
                    Debugger.StandardInput.WriteLine(CommandQueue.Count);
                    while (CommandQueue.Count > 0) {
                        Debugger.StandardInput.WriteLine((string)CommandQueue.Dequeue());
                    }
                    break;
                case "HIGHLIGHT":
                    if (Commands.Length < 4) break;
                    int LineNumber;
                    
                    if (int.TryParse(Commands[2], out LineNumber)) {
                        string Filename = "";
                        for (int i = 3; i < Commands.Length; ++i) {
                            Filename += Commands[i] + " ";                            
                        }
                        Filename = Filename.Trim();
                        JumpToLineInFile(Filename, LineNumber);
                    }
                    
                    break;
            }

        }

        ArrayList DebugLockedInterface;

        void Debugger_Exited(object sender, EventArgs e) {
            Debugger.Dispose();
            Debugger = null;
            UnlockInterface(DebugLockedInterface);
            DebuggerTips = false;
            UnlockTextFiles();
        }
        private void stopDebuggingToolStripMenuItem_Click(object sender, EventArgs e) {
            if (Debugger != null && !Debugger.HasExited) {
                CommandQueue.Enqueue("QUIT");
            }
        }

        private void pauseDebuggingToolStripMenuItem_Click(object sender, EventArgs e) {
            if (Debugger != null && !Debugger.HasExited) {
                CommandQueue.Enqueue("PAUSE");
            }
        }
        private string AwaitTooltip = "";

        public string GetTooltip(ColourfulEditor RequestedFrom, string Text, int LineNumber) {
            AwaitTooltip = "";
            SourceFile SourcefileSource = null;
            foreach (SourceFile S in SourceFiles.TabPages) {
                if (S.TextEditor.Equals(RequestedFrom)) {
                    SourcefileSource = S;
                    break;
                }
            }
            if (SourcefileSource == null) return "[Tooltip Error]";

            CommandQueue.Enqueue("TOOLTIP " + Text + " " + LineNumber.ToString() + " " + SourcefileSource.Filename);
            int Start = Environment.TickCount;
            while (AwaitTooltip == "" && Environment.TickCount < Start + 1000) {
                Application.DoEvents();
            }
            if (AwaitTooltip == "") return "";
            return AwaitTooltip == " " ? "" : AwaitTooltip;
            
        }

    }
}

