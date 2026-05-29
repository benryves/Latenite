using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace PindurTI_Debugger {
    public class LateniteInteraction {

        public class WatchVariable {
            public int Dereference = 0;
            public int Address = -1;
            public string Name = "";
        }

        public static byte[] MemoryDump = new byte[65536];

        public static void DebuggerInteract() {
            string[] SupportedCommands = { "QUIT", "PLAY", "PAUSE", "TOOLTIP" };
            Console.WriteLine("GET");
            int Count = -1;
            try {
                Count = Convert.ToInt32(Console.ReadLine());
            } catch {
                return;
            }
            for (int j = 0; j < Count; ++j) {
                string Command = Console.ReadLine();
                if (Command != null) {
                    switch (Command.ToLower()) {
                        case "enumerate":
                            foreach (string Supported in SupportedCommands) {
                                Console.WriteLine("ICAN " + Supported);
                            }
                            break;
                        case "quit":
                            Console.WriteLine("OK");
                            Program.MainControlPanel.Close();
                            break;
                        case "play":
                            Program.MainControlPanel.Paused = false;
                            break;
                        case "pause":
                            Program.MainControlPanel.Paused = true;
                            break;
                        default:
                            if (Command.ToLower().StartsWith("tooltip ")) {
                                string[] Details = Command.Split(' ');
                                // 0= "tooltip"
                                // 1= name
                                // 2= line #
                                // 3= sourcefile
                                // 4=     "
                                // 5= ...
                                if (Details.Length < 3) Console.WriteLine("TOOLTIP");
                                if (Program.Labels[Details[1].ToLower()] != null) {
                                    Program.LabelDetails Label = (Program.LabelDetails)Program.Labels[Details[1].ToLower()];
                                    Console.WriteLine("TOOLTIP {0}: ${1}[NL]Defined in {2}, line {3}", Label.Name, Label.Value.ToString("X4"), Path.GetFileName(Label.SourceFile), Label.Line);
                                }
                            } else if (Command.ToLower().StartsWith("watch ")) {
                                string[] Details = Command.Split(' ');
                                // 0= "watch"
                                // 1= name
                                // 2= size (in bytes)
                                // 3= dereference (0 or number of bytes!)
                                /*if (Program.Labels[Details[1].ToLower()] == null) {
                                    try {
                                        Program.PindurTI.StandardInput.WriteLine("dump-state cpu");
                                        string Confirm = Program.PindurTI.StandardOutput.ReadLine(); // OK
                                        //MessageBox.Show(Confirm);
                                        MessageBox.Show(Program.PindurTI.StandardOutput.ReadLine());
                                        int Registers = Convert.ToInt32(0);
                                        for (int i = 0; i < Registers; i++) {
                                            string Info = Program.PindurTI.StandardOutput.ReadLine();
                                            //MessageBox.Show(Registers + "," + Info);
                                            //Console.WriteLine("WATCH {0} {1}", Details[1], Info);
                                        }
                                    } catch (Exception ex) {
                                        MessageBox.Show(ex.Message);
                                        //Console.WriteLine("WATCH {0} ???", Details[1]);
                                    }
                                } else {
                                    Program.LabelDetails Label = (Program.LabelDetails)Program.Labels[Details[1].ToLower()];
                                    Console.WriteLine("WATCH {0} {1}", Details[1], Label.Value);
                                }*/
                            }
                            break;
                    }
                }
            }
        }
    }
}

