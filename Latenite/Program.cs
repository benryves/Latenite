using System;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using BenRyves;
using System.Xml;

namespace Latenite {
	static class Program {

		public static LateniteIDE MainIDE;
        // Global syntax highlighter variables
        public static Hashtable ColourTable = new Hashtable();
        public static Hashtable SyntaxLookup = new Hashtable();
        public static ArrayList _Seperators = new ArrayList();
        public static ArrayList SyntaxElements = new ArrayList();
        public static void AddSyntaxElement(string Item, Color Colour, ColourfulEditor.ItemType Type, bool CanAutoComplete, int ImageIndex, string ToolTipText) {
            SyntaxElements.Add(new BenRyves.ColourfulEditor.ItemToColour(Item, Colour, Type, CanAutoComplete, ImageIndex, ToolTipText));
        }
        public static void AddSeperator(char Seperator) {
            _Seperators.Add(Seperator);
        }

        public static BenRyves.ColourfulEditor.FlickerFreeForm IntellisenseWindow = new BenRyves.ColourfulEditor.FlickerFreeForm();
        
        public static void RegenerateSyntaxHashTable() {
            SyntaxLookup.Clear();
            ArrayList ColourStatus = new ArrayList();
            ColourStatus.Add(Properties.Settings.Default.Syntax_Z80);
            ColourStatus.Add(Properties.Settings.Default.Syntax_Register);
            ColourStatus.Add(Properties.Settings.Default.Syntax_Directive);
            ColourStatus.Add(Properties.Settings.Default.Syntax_Routine);
            foreach (ColourfulEditor.ItemToColour I in SyntaxElements) {
                string Key = I._Item; //.ToLower();
                if (SyntaxLookup[Key] == null || (ColourStatus.IndexOf(I._Colour) < ColourStatus.IndexOf(((ColourfulEditor.ItemToColour)SyntaxLookup[Key])._Colour))) {
                    if (SyntaxLookup[Key] != null) SyntaxLookup.Remove(Key);
                    SyntaxLookup.Add(Key, I);
                }
            }
            ArrayList ToCheck = new ArrayList();

            foreach (string S in SyntaxLookup.Keys) {
                ToCheck.Add(S);
            }
            foreach (string S in ToCheck) {
                if (S.ToLower() != S && SyntaxLookup[S.ToLower()] == null) {
                    SyntaxLookup[S.ToLower()] = SyntaxLookup[S];
                }
            }
        }

        public static string StripHTML(string StringToStrip) {
            string[] Split = StringToStrip.Split('<');
            string Return = "";
            foreach (string S in Split) {
                string[] T = S.Split('>');
                if (T.Length > 1) {
                    for (int i = 1; i < T.Length; i++) {
                        Return += T[i];    
                    }
                    
                } else if (T.Length == 1) {
                    Return += T[0];
                }
            }
            return Return;
        }

        public static string ExpandHTML(string StringToExpand) {
            return StringToExpand.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"");
        }

        public static Hashtable AutocompleteItemsAccelerator = new Hashtable();
        public static ImageList IntellisenseIcons = new ImageList();
        //public static ImageList FileTypeIcons = new ImageList();

        public static void ReloadAllColouring() {

            _Seperators.Clear();
            SyntaxElements.Clear();
            ColourTable.Clear();
            SyntaxLookup.Clear();

            string Seps = " \r\n\t,+-<>%&|$~?¬^;*/\\(),'\"";
            foreach (char C in Seps) {
                AddSeperator(C);
            }

            AddSyntaxElement("\"", Properties.Settings.Default.Syntax_String, BenRyves.ColourfulEditor.ItemType.String, false, 0, "");
            AddSyntaxElement("'", Properties.Settings.Default.Syntax_String, BenRyves.ColourfulEditor.ItemType.String, false, 0, "");

            AddSyntaxElement(":", Properties.Settings.Default.Syntax_Label, BenRyves.ColourfulEditor.ItemType.Word, false, 0, "");
            AddSyntaxElement(";", Properties.Settings.Default.Syntax_Comment, BenRyves.ColourfulEditor.ItemType.ToEndOfLine, false, 0, "");

            foreach (HelpFile X in Program.MainIDE.HelpList) {
                string HelpfileName = X.HelpFileXML.DocumentElement.Attributes.GetNamedItem("name")?.Value;
                XmlNodeList L = X.HelpFileXML.GetElementsByTagName("item");
 
                foreach (XmlNode N in L) {
                    string[] ToAdd = null;
                    string ColourCode = "routine";
                    string ToolTipText = null;
                    foreach (XmlAttribute A in N.Attributes) {
                        if (A.Name.ToLower() == "highlight") {
                            ToAdd = A.Value.Split(new char[] { '/' });
                        } else if (A.Name.ToLower() == "colour") {
                            ColourCode = A.Value.ToLower();
                        } else if (A.Name.ToLower() == "syntax") {
							if (string.IsNullOrEmpty(HelpfileName)) {
								ToolTipText = "Syntax: " + ExpandHTML(StripHTML(A.Value));
							} else {
								ToolTipText+= HelpfileName + "\nSyntax: " + ExpandHTML(StripHTML(A.Value));
							}
                        }/* else if (A.Name.ToLower() == "description") {
                            ToolTipText += "\n" + ExpandHTML(StripHTML(A.Value));
                        }*/
                    }

					if (!string.IsNullOrEmpty(ToolTipText)) {
						string[] R_T = ToolTipText.Split('\n');
						ToolTipText = "";
						bool First = true;
						foreach (string S in R_T) {
							if (S.Trim() != "") {
								if (!First) {
									ToolTipText += "\r\n";
								}
								ToolTipText += S.Trim();
								First = false;
							}
						}
					}

                    if (ToAdd != null) {
                        foreach (string S in ToAdd) {
                            Color ToColour = Properties.Settings.Default.Syntax_Routine;
                            int ImageIndex = 0;
                            switch (ColourCode) {
                                case "routine": ImageIndex = 0; ToColour = Properties.Settings.Default.Syntax_Routine; break;
                                case "z80": ImageIndex = 1; ToColour = Properties.Settings.Default.Syntax_Z80; break;
                                case "directive": ImageIndex = 2; ToColour = Properties.Settings.Default.Syntax_Directive; break;
                                case "register": ImageIndex = 3; ToColour = Properties.Settings.Default.Syntax_Register; break;
                                case "value": ImageIndex = 4; ToColour = Properties.Settings.Default.Syntax_Label; break;
                                default: break;
                            }


                            AddSyntaxElement(S, ToColour, BenRyves.ColourfulEditor.ItemType.Word, true, ImageIndex, ToolTipText);
                        }
                    }
                }
            }

            IntellisenseWindow.Controls.Clear();
            AutocompleteItemsAccelerator.Clear();

            RegenerateSyntaxHashTable();
            foreach (SourceFile S in MainIDE.SourceFiles.TabPages) {
                S.TextEditor.UseExternalProperties(Program.ColourTable, Program.SyntaxLookup, Program._Seperators, Program.SyntaxElements);
                bool WasDirty = S.IsDirty;
                S.Hide();
                S.AdjustColours();
                S.IsDirty = WasDirty;
                S.Text = System.IO.Path.GetFileName(S.Filename) + (S.IsDirty ? "*" : "");
                S.TextEditor.AutoIndent = Properties.Settings.Default.Editor_AutoIndent;
                S.Show();
            }

            MainIDE.OutputTextBox.Font = Properties.Settings.Default.EditorFont;

        }

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
        static void Main(string[] args) {
			Application.EnableVisualStyles();
			Application.DoEvents();

			//MessageBox.Show("WARNING\nThis is a BETA version of Latenite.\nAs such, any bugs should be reported and any data lost is NOT my responsibility.\nI have tested it as best I can, but have released this version to try and pick out the smaller, harder to find bugs.\nBen (benryves@benryves.com)");

			Latenite.Resources.AboutBox Ab = new Latenite.Resources.AboutBox();
			Ab.Show();
			Application.DoEvents();
			MainIDE = new LateniteIDE();
			MainIDE.IndexHelp();

            // Syntax highlighting rubbish:

            if (args.Length == 0) {
                Projects P = new Projects();
                Ab.Dispose();
                Application.Run(P);
            } else {
                Ab.Dispose();
                MainIDE.LoadProjectFile(args[0]);
                if (MainIDE.ProjectFile == null) MainIDE.Close();
            }
            IntellisenseIcons.ColorDepth = ColorDepth.Depth32Bit;
            IntellisenseIcons.Images.Add(Properties.Resources.routine);
            IntellisenseIcons.Images.Add(Properties.Resources.lightning);
            IntellisenseIcons.Images.Add(Properties.Resources.directive);
            IntellisenseIcons.Images.Add(Properties.Resources.table);
            IntellisenseIcons.Images.Add(Properties.Resources.label);

            if (MainIDE.ProjectFile != null && MainIDE.ProjectFile.DocumentElement == null) {
                // We are QUITTING
				MainIDE.Dispose();
			} else {
                if (!MainIDE.IsDisposed) Application.Run(MainIDE);
			}
            try {
                Properties.Settings.Default.Save();
            } catch (Exception ex) {
                MessageBox.Show("Sorry, but I could not save your settings!\n", ex.Message);
            }
			
		}
	}
}