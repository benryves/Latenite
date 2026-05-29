using System;
using System.Collections;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.IO;
namespace Latenite {
    partial class LateniteIDE {
        
        public void RefreshProjectView() {
        }
        public string GetProjectSelFilename() {
            return (string)ProjectTree.SelectedNode.Tag;
        }
        public bool CheckAlreadyOpen(string FileToCheck) {
            foreach (SourceFile S in this.SourceFiles.TabPages) {
                if (Path.GetFullPath(S.Filename).ToLower() == Path.GetFullPath(FileToCheck).ToLower()) {
                    return true;
                }
            }
            return false;
        }
        public string GetProjectName() {
            try {
                return ProjectFile.DocumentElement.Attributes.GetNamedItem("name").Value;
            } catch {
                return "";
            }
            
        }
        public string GetBinaryName() {
            try {
                return ProjectFile.DocumentElement.Attributes.GetNamedItem("binary").Value;
            } catch {
                return "";
            }
        }
        public string GetProjectFilename() {
            return this.ProjectFile.BaseURI.Replace("file:///", "");
        }
        public string GetProjectSourceFile() {
            try {
                string ProjectPath = Path.GetDirectoryName(GetProjectFilename());
				string SourceFile = ProjectFile.DocumentElement.Attributes.GetNamedItem("source").Value;
                return Path.Combine(ProjectPath, SourceFile);
            } catch {
                return "";
            }
        }

        public bool LoadProjectFile(string Filename) {
            try {
                ProjectFile.Load(Filename);
                ProjectTree.RootDirectory = Path.GetDirectoryName(Filename);
				if (ProjectFile.DocumentElement.Name.ToLower() != "project") throw new Exception("This is not a valid Latenite project file.");
				if (Convert.ToInt16(ProjectFile.DocumentElement.Attributes.GetNamedItem("version").Value) != 2) throw new Exception("This version of Latenite cannot open that project file. Please upgrade to the latest version.");
                ProjectTree.RefreshFolders();
                RepopulateBuildMenus();
                XmlNodeList H = ProjectFile.GetElementsByTagName("help");
                foreach (XmlNode HF in H) {
                    try {
                        AssociatedHelpFiles.Add(HF.Attributes.GetNamedItem("file").Value.ToLower());
                    } catch { }
                }
                IndexHelp();
                Program.ReloadAllColouring();
                return true;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error loading project file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ProjectFile = null;
                return false;
            }
            
        }

   

        /// <summary>
        /// Repopulate the build script menu with all the build scripts, and highlight the default one.
        /// </summary>
        public void RepopulateBuildMenus() {

            // Clear the build script list:
            this.BuildTarget.Items.Clear();
            
            // Find all the build scripts:
            XmlNodeList BuildScripts = ProjectFile.GetElementsByTagName("build");

            // Go through the scripts one at a time:
            foreach (XmlNode BuildScript in BuildScripts) {
                bool IsDefault = false; // Is this marked as the default build script?

                // Hammer through the attributes to see if default="true" is in there.
                foreach (XmlAttribute CheckDefault in BuildScript.Attributes) {
                    if (CheckDefault.Name.ToLower() == "default" && CheckDefault.Value.ToLower() == "true") {
                        IsDefault = true;
                        break;
                    }
                }
                
                // Filename of the build script:
                string BuildName = Path.Combine(Path.GetDirectoryName(GetProjectFilename()), BuildScript.InnerText);

                BuildMenuOption BuildScriptItem = new BuildMenuOption(BuildName);
                this.BuildTarget.Items.Add(BuildScriptItem);
                if (IsDefault) {
                    this.BuildTarget.SelectedItem = BuildScriptItem;
                }
            }
        }
        /// <summary>
        /// Save the project file away.
        /// </summary>
        public void SaveProject() {
            try {
                ProjectFile.Save(GetProjectFilename());
            } catch (Exception ex) {
                MessageBox.Show("Error saving project file:\n" + ex.Message);
            }
            
        }
        
    }
}
