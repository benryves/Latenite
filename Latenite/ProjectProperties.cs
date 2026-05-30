using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace Latenite {
	public partial class ProjectProperties : Form {
		LateniteIDE BaseForm;

        private string OldDefault = "";

		public ProjectProperties(LateniteIDE Owner) {
			BaseForm = Owner;
            if (BaseForm.BuildTarget.SelectedItem != null) {
                OldDefault = ((BuildMenuOption)BaseForm.BuildTarget.SelectedItem).Path;
            }
			InitializeComponent();
		}


        private class ListItem : Object {
            public object Tag;
            private string Text;
            public ListItem(string DisplayText, object Filename) {
                Text = DisplayText;
                Tag = Filename;
            }
            public override string ToString() {
                return Text;
            }
        }
        
		private void ProjectProperties_Load(object sender, EventArgs e) {
			// Populate the boxes:
            string SourceFile = "";
			try {
				foreach (XmlAttribute A in BaseForm.ProjectFile.DocumentElement.Attributes) {
					switch (A.Name.ToLower()) {
						case "name":
							this.ProjectName.Text = A.Value;
							break;
						case "binary":
							this.BinaryName.Text = A.Value;
							break;
                        case "source":
                            SourceFile = Path.Combine(Path.GetDirectoryName(BaseForm.GetProjectFilename()),A.Value);
                            break;
						default:
							break;
					}
				}
			} catch (Exception ex) {
				MessageBox.Show("There appears to be a problem with the project file:\n" + ex.Message);		
			}

            // Now we need to populate the BUILD SOURCE box and the BUILD SCRIPTS boxes:
            string[] AllFiles = Directory.GetFiles(Path.GetDirectoryName(BaseForm.GetProjectFilename()), "*.*", SearchOption.AllDirectories);
            foreach (string S in AllFiles) {
                ListItem X = new ListItem(S.Replace(Path.GetDirectoryName(BaseForm.GetProjectFilename()) + "\\", ""), S);
                this.BuildCombo.Items.Add(X);
            }
            for (int i = 0; i < BuildCombo.Items.Count; ++i) {
                ListItem X = (ListItem)BuildCombo.Items[i];
                if (SourceFile.ToLower() == X.Tag.ToString().ToLower()) {
                    this.BuildCombo.SelectedIndex = i;
                    break;
                }
            }           


            XmlNodeList ValidBuildScripts = Program.MainIDE.ProjectFile.GetElementsByTagName("build");
            ArrayList BuildScriptNames = new ArrayList();
            foreach (XmlNode X in ValidBuildScripts) {
                BuildScriptNames.Add(Path.Combine(Path.GetDirectoryName(BaseForm.GetProjectFilename()), X.InnerText));
            }
            string[] BuildFiles = Directory.GetFiles(Path.GetDirectoryName(BaseForm.GetProjectFilename()), "*.cmd", SearchOption.AllDirectories);
            foreach (string S in BuildFiles) {
                ListViewItem X = new ListViewItem(S.Replace(Path.GetDirectoryName(BaseForm.GetProjectFilename()) + "\\", ""));
                X.Tag = S;
                X.Checked = BuildScriptNames.Contains(S);
                this.BuildScripts.Items.Add(X);

            }

            // Populate the XML help file source box:

            string[] XmlHelpFileList = Directory.GetFiles(Path.Combine(Application.StartupPath, "Help"), "*.xml", SearchOption.AllDirectories);
            foreach (string S in XmlHelpFileList) {
				HelpFile H;
				try {
					H = new HelpFile(S);
					ListViewItem X = new ListViewItem(H.Name);
					if (H.Name != Path.GetFileName(S)) X.Text += " (" + Path.GetFileName(S) +")";
					X.Tag = S;
					X.Checked = Program.MainIDE.AssociatedHelpFiles.Contains(Path.GetFileName(S).ToLower());
					this.ChooseHelpFiles.Items.Add(X);
				} catch { }
            }

		}

		private void ProjectProperties_FormClosed(object sender, FormClosedEventArgs e) {
		}

		private void ButtonCancel_Click(object sender, EventArgs e) {
			this.Close();
		}

		private void ButtonOK_Click(object sender, EventArgs e) {
			if (this.ProjectName.Text == "" || this.BinaryName.Text == "" || this.BuildCombo.Text == "") {
				MessageBox.Show("You must select a build file and provide both a project and a binary name!", "Project Properties", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

           
			// Now update the project;
			XmlAttribute APrj = BaseForm.ProjectFile.CreateAttribute("name");
			APrj.Value = this.ProjectName.Text;
			XmlAttribute ABin = BaseForm.ProjectFile.CreateAttribute("binary");
			ABin.Value = this.BinaryName.Text;
            XmlAttribute ASrc = BaseForm.ProjectFile.CreateAttribute("source");
			ASrc.Value = this.BuildCombo.Text;

			try {
				BaseForm.ProjectFile.DocumentElement.Attributes.SetNamedItem(APrj);
				BaseForm.ProjectFile.DocumentElement.Attributes.SetNamedItem(ABin);
				BaseForm.ProjectFile.DocumentElement.Attributes.SetNamedItem(ASrc);
			} catch (Exception ex) {
				MessageBox.Show("Could not update project properties:\n" + ex.Message);
				return;				
			}

            ArrayList ToRemove = new ArrayList();
            foreach (XmlNode X in BaseForm.ProjectFile.GetElementsByTagName("build")) {
                ToRemove.Add(X);
            }
            foreach (XmlNode X in ToRemove) {
                X.ParentNode.RemoveChild(X);
            }

            XmlAttribute DefTrue = BaseForm.ProjectFile.CreateAttribute("default");
            DefTrue.Value = "true";
            XmlAttribute DefFalse = BaseForm.ProjectFile.CreateAttribute("default");
            DefFalse.Value = "false";

            foreach (ListViewItem L in this.BuildScripts.Items) {
                if (L.Checked == true) {
                    XmlNode X = BaseForm.ProjectFile.CreateElement("build");
                    X.InnerText = ((string)L.Tag).Replace(Path.GetDirectoryName(BaseForm.GetProjectFilename()) + "\\", "");
                    if (L.Tag.ToString().ToLower() == OldDefault.ToLower()) {
                        X.Attributes.SetNamedItem(DefTrue);
                    } else {
                        X.Attributes.SetNamedItem(DefFalse);
                    }
					BaseForm.ProjectFile.DocumentElement.AppendChild(X);
                }
            }

            Program.MainIDE.HelpList.Clear();
            Program.MainIDE.AssociatedHelpFiles.Clear();
            for (int i = 0; i < ChooseHelpFiles.Items.Count; ++i) {
                if (ChooseHelpFiles.Items[i].Checked) Program.MainIDE.AssociatedHelpFiles.Add(Path.GetFileName(ChooseHelpFiles.Items[i].Tag.ToString()).ToLower());
            }
            Program.MainIDE.IndexHelp();

            ToRemove.Clear();
            foreach (XmlNode X in (XmlNodeList)Program.MainIDE.ProjectFile.GetElementsByTagName("help")) {
                ToRemove.Add(X);
            }
            foreach (XmlNode N in ToRemove) {
                N.ParentNode.RemoveChild(N);                
            }

            Program.ReloadAllColouring();

            foreach (HelpFile H in Program.MainIDE.HelpList) {
                XmlNode N = Program.MainIDE.ProjectFile.CreateElement("help");
                XmlAttribute A = Program.MainIDE.ProjectFile.CreateAttribute("file");
                A.Value = Path.GetFileName(H.HelpFileXML.BaseURI).ToLower();
                N.Attributes.Append(A);
				Program.MainIDE.ProjectFile.DocumentElement.AppendChild(N);
            }



            

            
            BaseForm.SaveProject();
			BaseForm.RepopulateBuildMenus();
			this.Close();

		}

		private void ProjectProperties_Shown(object sender, EventArgs e) {
			this.TabIcons.Images.Clear();
			this.TabIcons.Images.Add("textfield_rename", Properties.Resources.textfield_rename);
			this.TabIcons.Images.Add("brick", Properties.Resources.brick);
			this.TabIcons.Images.Add("help", Properties.Resources.help);
			this.Invalidate(true);
		}
	}
}