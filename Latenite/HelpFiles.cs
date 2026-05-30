using System;
using System.Collections;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace Latenite {


	public partial class LateniteIDE {

        public ArrayList AssociatedHelpFiles = new ArrayList();

        /// <summary>
        /// Load and index all the external help files.
        /// </summary>
		public void IndexHelp() {
			Directory.SetCurrentDirectory(Application.StartupPath);
            HelpFilesCombo.Items.Clear();
            HelpFilesCombo.Items.Add("(All)");
			// Index the help files:
			try {
				string[] HelpFiles = Directory.GetFileSystemEntries("Help", "*.xml");
				foreach (string H in HelpFiles) {
                    if (AssociatedHelpFiles.Contains(Path.GetFileName(H).ToLower())) {
                        HelpFile F = new HelpFile(H);
                        if (F.ContainsHelp && !string.IsNullOrEmpty(F.Name)) {
                            HelpFilesCombo.Items.Add(F.Name);
                        }
                        HelpList.Add(F);
                    }
				}
			} catch (Exception ex) {
				MessageBox.Show("There was an error loading the help files:\n" + ex.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
            HelpFilesCombo.SelectedIndex = 0;
            
		}

		/// <summary>
		/// Display a help file item (decode the XML to HTML)
		/// </summary>
		/// <param name="E">XmlNode of the item to display</param>
		public void DisplayHelpFile(XmlNode E) {

            if (HelpBrowser == null) return;

			string StartHTML = "<html><head><link rel=\"Stylesheet\" href=\"file:///" + Path.GetFullPath(Application.StartupPath + "\\" + "Help").Replace("\\", "/").Replace(" ", "%20") + "/style.css\" /></head><body>";
			string EndHTML = "</body></html>";

			try {

				string Name = "";
				string Description = "";
				string Syntax = "";
				string Comments = "";

				string CurrentItemName = "";

                string RootName = "";

                try {
                    RootName = "<i>" + E.OwnerDocument.DocumentElement.Attributes.GetNamedItem("name").Value + "</i><br />";
                } catch { }

				for (int j = 0; j < E.Attributes.Count; ++j) {
					switch (E.Attributes[j].Name.ToLower()) {
						case "name":
                            CurrentItemName = E.Attributes[j].Value;
							Name = "<div class=\"name\">" + RootName + CurrentItemName + "</div>"; break;
						case "description":
							Description = "<p class=\"region\">Description</p><div class=\"description\">" + E.Attributes[j].InnerText + "</div>"; break;
						case "syntax":
							Syntax = "<p class=\"region\">Syntax</p><div class=\"syntax\">" + E.Attributes[j].InnerText + "</div>"; break;
						case "comments":
							Comments = "<p class=\"region\">Comments</p><div class=\"comments\">" + E.Attributes[j].InnerText + "</div>"; break;
						default:
							break;
					}
				}

				// Now we need to handle any input/outputs:

				string InputTable = "";
				string OutputTable = "";
				string DestroyedTable = "";
				string Notes = "";
				string Examples = "";
				

				foreach (XmlNode SI in E.ChildNodes) {
					string IOName = "";
					string IORegister = "";
					string IOComments = "";
					string NoteHTML = "";
					string CodeHTML = "";
					foreach (XmlAttribute SA in SI.Attributes) {
						switch (SA.Name.ToLower()) {
							case "name":
								IOName = SA.InnerText; break;
							case "register":
								IORegister = SA.InnerText; break;
							case "comments":
								IOComments = SA.InnerText; break;
							case "description":
								NoteHTML = SA.InnerText; break;
							case "code":
								CodeHTML = SA.InnerText; break;
							default:
								break;
						}
					}
					string TableRow = "<tr><td width=\"20px\">" + IORegister + "</td><td>" + IOName + "</td><td>" + IOComments + "</td></tr>";
					switch (SI.Name.ToLower()) {
						case "input":
							InputTable += TableRow; break;
						case "output":
							OutputTable += TableRow; break;
						case "destroyed":
							DestroyedTable += TableRow; break;
						case "note":
							Notes += "<p class=\"region\">Note</p><div class=\"note\">" + NoteHTML + "</div>"; break;
						case "example":
							Examples += "<p class=\"region\">Example</p><div class=\"example\">";
							if (IOName != "") Examples += "<p class=\"examplenotes\">" + IOName + "</p>";
							Examples+="<div class=\"code\"><pre>" + CodeHTML + "</pre></div>";
							if (IOComments != "") Examples += "<p class=\"examplenotes\">" + IOComments + "</p>";
							Examples += "</div>"; break;
						default:
							break;
					}
				}


				if (InputTable != "") {
					InputTable = "<p class=\"region\">Input</p><table class=\"io\">" + InputTable + "</table>";
				}

				if (OutputTable != "") {
					OutputTable = "<p class=\"region\">Output</p><table class=\"io\">" + OutputTable + "</table>";
				}

				if (DestroyedTable != "") {
					DestroyedTable = "<p class=\"region\">Destroyed</p><table class=\"io\">" + DestroyedTable + "</table>";
				}

				string TableOfLinks = "";
				// Now we need to build up a table of links!

				ArrayList SeeA = new ArrayList();
				if (E.ParentNode != null) {
					XmlNode G = E.ParentNode;
					foreach (XmlNode SG in G.ChildNodes) {
						string OtherName = SG.Attributes.GetNamedItem("name").InnerText;
						if (OtherName != CurrentItemName) {
							if (TableOfLinks == "") {
								TableOfLinks = "<b>" + G.Attributes.GetNamedItem("name").InnerText + "</b>";
							}
							SeeA.Add(OtherName);
						}
					}
				}
				SeeA.Sort();
				foreach (string S in SeeA) {
					TableOfLinks += "<br /><a href=\"lnh_" + S + "\">" + S + "</a>";
				}

				if (TableOfLinks != "") {
					TableOfLinks = "<p class=\"region\">See also</p><div class=\"seealso\">" + TableOfLinks + "</div>";
				}

                this.HelpBrowser.DocumentText = StartHTML + Name + Syntax + Description + Comments + InputTable + OutputTable + Examples + Notes + TableOfLinks + EndHTML;
			} catch (Exception ex) {
				try {
					this.HelpBrowser.DocumentText = StartHTML + "<div class=\"name\">Error</div><div class=\"description\">" + ex.Message + "</div>" + EndHTML;
				} catch { }
			}



		}



		/// <summary>
		/// Search all the help files for an item
		/// </summary>
		/// <param name="StringToSearch">String of the item to search for</param>
		private void SearchHelp(string StringToSearch) {
            ProjectAndHelpTabs.SelectedTab = HelpTab;
            Application.DoEvents();
            ArrayList Results = new ArrayList();
			foreach (HelpFile H in HelpList) {
                if (!H.ContainsHelp) continue;
				XmlNodeList X = H.HelpFileXML.GetElementsByTagName("item");
				foreach (XmlNode N in X) {
					try {
						string TopicName = N.Attributes.GetNamedItem("name")?.InnerText;
						if (string.IsNullOrEmpty(TopicName)) {
							// Nothing to do
						} else if (TopicName.ToLower() == StringToSearch.ToLower()) {
                            Results.Add(N);
                        } else {
                            string[] Topics = TopicName.Split(new char[] { '/' });
                            foreach (string S in Topics) {
                                if (S.ToLower() == StringToSearch.ToLower()) {
                                    Results.Add(N);
                                }
                            }
                        }
                    } catch { }
				}
			}
            if (Results.Count == 1) {
                DisplayHelpFile((XmlNode)Results[0]);
            } else if (Results.Count > 0) {
                // Multiple results
                MultipleResults M = new MultipleResults(Results);
                M.ShowDialog(this);
            }
			
		}

	}
}
