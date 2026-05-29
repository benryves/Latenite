using System;
using System.Collections;
using System.Text;
using System.Xml;
using Microsoft.Win32;

namespace EarlyMorning_Resource_Plugin {
    class Program {
        static string Filename = "";
        static string EmPath = "";

        static XmlDocument X;
        static void Main(string[] args) {

            if (args.Length != 1) {
                Console.WriteLine();
                return;
            }


            RegistryKey EarlyMorning = Registry.ClassesRoot.OpenSubKey(".emr");
            if (EarlyMorning != null) {
                try {
                    EarlyMorning = Registry.ClassesRoot.OpenSubKey(EarlyMorning.GetValue("").ToString() + @"\shell\open\command");
                    EmPath = EarlyMorning.GetValue(EarlyMorning.GetValueNames()[0]).ToString().Split('"')[1];
                } catch { }
            }

            try {
                X = new XmlDocument();
                Filename = args[0];
                if (args[0].IndexOf(' ') != -1 && args[0][0] != '"') Filename = "\"" + Filename + "\"";
                X.Load(args[0].Replace("\"", ""));
                
                StringBuilder Out = new StringBuilder("<x program=\"EarlyMorning\">");


                GetResourceInfo(ref Out, X.GetElementsByTagName("spritesheet"), "sprite");
                GetResourceInfo(ref Out, X.GetElementsByTagName("mapcollection"), "map");

                Out.Append("</x>");
                Console.WriteLine(Out.ToString());
            } catch {
                Console.WriteLine();
            }

        }
        static int FolderCount = 0;
        static private void GetResourceInfo(ref StringBuilder XmlOut, XmlNodeList ResourceList, string Subresource) {
            
            foreach (XmlNode N in ResourceList) {
                string Title = "???";
                ArrayList Sprites = new ArrayList();
                foreach (XmlNode SN in N.ChildNodes) {
                    if (SN.Name.ToLower() == "title") {
                        Title = SN.InnerText;
                    } else if (SN.Name.ToLower() == Subresource) {
                        Sprites.Add(SN);
                    }
                }
                XmlOut.Append("<folder name=\"" + Title + "\">");

                int ResourceCount = 0;
                foreach (XmlNode SN in Sprites) {
                    foreach (XmlNode SNSN in SN.ChildNodes) {
                        if (SNSN.Name.ToLower() == "title") {
                            string Event = "";
                            if (EmPath != "") {
                                Event = "source=\"" + EmPath + "\" args=\"" + Filename.Replace("\"", "&quot;") + " " + Subresource + " " + FolderCount + " " + ResourceCount + "\" ";
                            }
                            XmlOut.Append("<file name=\"" + SNSN.InnerText + "\" " + Event + "/>");
                            break;
                        }
                    }
                    ++ResourceCount;
                }
                XmlOut.Append("</folder>");
                ++FolderCount;

            }
        }
    }
}
