using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.IO;
using System.Threading;

namespace PindurTI_Debugger {
    static class Program {

        public static Process PindurTI = null;
        public static XmlDocument Settings = null;
        public static Thread DebugInteraction = null;
        public static ControlPanel MainControlPanel;
        public static bool RunInDebugMode = false;
        public static XmlDocument DebugSource = null;
        

        public class LabelDetails {
            public int Line = 0;
            public int Value = 0;
            public string Name = "(Unknown)";
            public string SourceFile = "(Unknown)";
        }

        public static Queue<string> QueuedBreakpoints = new Queue<string>();
        public static Hashtable Labels = new Hashtable();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] Arguments) {

            try {
                Settings = new XmlDocument();
                Settings.Load("settings.xml");
            } catch (Exception ex) {
                MessageBox.Show("Error loading settings:\n" + ex.Message, "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            PindurTI = new Process();
            PindurTI.StartInfo.FileName = "pindurti.exe";
            PindurTI.StartInfo.Arguments = "-p";
            PindurTI.StartInfo.RedirectStandardInput = true;
            PindurTI.StartInfo.RedirectStandardOutput = true;
            PindurTI.StartInfo.UseShellExecute = false;

            try {
                PindurTI.Start();
            } catch (Exception ex) {
                MessageBox.Show("Error starting debugger:\n" + ex.Message, "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RunInDebugMode = false;
            foreach (string Argument in Arguments) {
                if (Argument.ToLower() == "-d") {
                    RunInDebugMode = true;

                    try {
                        DebugSource = new XmlDocument();
                        DebugSource.Load(Environment.GetEnvironmentVariable("DEBUG_LOG"));

                        foreach (XmlNode File in DebugSource.GetElementsByTagName("source")) {
                            string Filename = "(Unknown)";
                            foreach (XmlAttribute GetFilename in File.Attributes) {
                                if (GetFilename.Name.ToLower() == "file") Filename = GetFilename.Value.ToString();
                            }
                            foreach (XmlNode Label in File.ChildNodes) {
                                if (Label.Name.ToLower() == "label") {
                                    LabelDetails NewLabel = new LabelDetails();
                                    NewLabel.SourceFile = Filename;
                                    foreach (XmlAttribute GetLabelDetails in Label.Attributes) {
                                        switch (GetLabelDetails.Name.ToLower()) {
                                            case "name": NewLabel.Name = GetLabelDetails.Value.ToString(); break;
                                            case "value": NewLabel.Value = Convert.ToInt32(GetLabelDetails.Value); break;
                                            case "line": NewLabel.Line = Convert.ToInt32(GetLabelDetails.Value); break;
                                        }
                                    }
                                    
                                    Labels[NewLabel.Name.ToLower()] = NewLabel;
                                }
                            }
                        }

                        XmlNodeList Breakpoints = DebugSource.GetElementsByTagName("breakpoint");
                        foreach (XmlNode N in Breakpoints) {
                            
                            foreach (XmlAttribute A in N.Attributes) {
                                if (A.Name.ToLower() == "address") {
                                    QueuedBreakpoints.Enqueue("set-breakpoint code " + A.Value);
                                    break;
                                }
                            }
                        }
                        
                    } catch (Exception ex) {
                        Console.WriteLine("MESSAGE Could not load debug XML log: " + ex.Message);
                    }
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainControlPanel = new ControlPanel();
            Application.Run(MainControlPanel);

            if (DebugInteraction != null) {
                DebugInteraction.Abort();
            }

            PindurTI.Kill();
            try {
                PindurTI_Debugger.Properties.Settings.Default.Save();
            } catch {
                MessageBox.Show("Could not save settings.");
            }
        }

        public static void SaveSettings() {
            try {
                Settings.Save(Settings.BaseURI.Substring(8));
            } catch (Exception ex) {
                MessageBox.Show("Could not save settings:\n" + ex.Message, "Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}