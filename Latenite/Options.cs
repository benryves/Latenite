using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace Latenite {
    public partial class Options : Form {
        LateniteIDE BaseForm;
        public Options() {
            InitializeComponent();
            BaseForm = Program.MainIDE;
            
        }

        ArrayList XmlDefaults = new ArrayList();

        private void ButtonCancel_Click(object sender, EventArgs e) {
            if (PreviewText.Focused) return;
            this.Close();
        }

        private Color[] Colours = new Color[10];


        private void ButtonOK_Click(object sender, EventArgs e) {

            if (PreviewText.Focused) return;

            // Update the settings:
            Properties.Settings.Default.EditorFont = this.PreviewText.Font;
            Properties.Settings.Default.Editor_Back_Colour = this.PreviewText.BackColor;
            Properties.Settings.Default.Project_Filter = this.FilePattern.Text;

            Properties.Settings.Default.Editor_Text_Colour = Colours[(int)SyntaxTypes.Foreground];
            Properties.Settings.Default.Syntax_Directive = Colours[(int)SyntaxTypes.Directive];
            Properties.Settings.Default.Syntax_Z80 = Colours[(int)SyntaxTypes.Z80];
            Properties.Settings.Default.Syntax_Comment = Colours[(int)SyntaxTypes.Comment];
            Properties.Settings.Default.Syntax_Routine = Colours[(int)SyntaxTypes.Routine];
            Properties.Settings.Default.Syntax_String = Colours[(int)SyntaxTypes.String];
            Properties.Settings.Default.Syntax_Label = Colours[(int)SyntaxTypes.Label];
            Properties.Settings.Default.Syntax_Register = Colours[(int)SyntaxTypes.Register];

            Properties.Settings.Default.Editor_Syntax_Files_To_Highlight = this.FilesToHighlight.Text;

            Properties.Settings.Default.Editor_AutoIndent = this.AutoIndent.Checked;
            Properties.Settings.Default.Editor_AutoSave = this.AutoSaveOnBuild.Checked;

            Properties.Settings.Default.EditorFont = this.PreviewText.Font;

            Properties.Settings.Default.Editor_AutoIndent = this.AutoIndent.Checked;
            Properties.Settings.Default.Editor_AutoSave = this.AutoSaveOnBuild.Checked;
            Properties.Settings.Default.IDE_Multiple_Tabs = this.MultipleRowsOfTabs.Checked;
            Properties.Settings.Default.Editor_CRLF = this.NewlinesAsCRLF.Checked;

            Program.ReloadAllColouring();

            BaseForm.ProjectTree.Pattern = this.FilePattern.Text;
            BaseForm.RefreshProjectView();
            BaseForm.SourceFiles.Multiline = Properties.Settings.Default.IDE_Multiple_Tabs;

            this.Close();
        }

        private void SetFont_Click(object sender, EventArgs e) {
            SettingsFontDialog.Font = this.PreviewText.Font;
            DialogResult D = SettingsFontDialog.ShowDialog();
            if (D == DialogResult.OK) {
                this.PreviewText.Font = SettingsFontDialog.Font;
            }
        }

        private void ShowColourEditor(ref Color ColourToSet) {
            this.SettingsColourDialog.Color = ColourToSet;
            DialogResult D = SettingsColourDialog.ShowDialog();
            if (D == DialogResult.OK) {
                ColourToSet = SettingsColourDialog.Color;
            }

        }


        private enum SyntaxTypes {
            Z80 = 1, Comment = 6, String = 7, Routine = 5, Directive = 3, Label = 4, Register = 2, Foreground = 0, Background = 8
        }

        private void RejigPreview() {
            PreviewText.ClearSeperators();
            PreviewText.ClearSyntaxElements();
            string Seps = " \r\n\t,-;+*/\\(),'\"";
            foreach (char C in Seps) {
                PreviewText.AddSeperator(C);
            }

            PreviewText.AddSyntaxElement("\"", Colours[(int)SyntaxTypes.String], BenRyves.ColourfulEditor.ItemType.String, false, 0, "");
            PreviewText.AddSyntaxElement("'", Colours[(int)SyntaxTypes.String], BenRyves.ColourfulEditor.ItemType.String, false, 0, "");
            PreviewText.AddSyntaxElement(":", Colours[(int)SyntaxTypes.Label], BenRyves.ColourfulEditor.ItemType.ToStartOfWord, false, 0, "");
            PreviewText.AddSyntaxElement(";", Colours[(int)SyntaxTypes.Comment], BenRyves.ColourfulEditor.ItemType.ToEndOfLine, false, 0, "");
            PreviewText.AddSyntaxElement("_routine", Colours[(int)SyntaxTypes.Routine], BenRyves.ColourfulEditor.ItemType.Word, false, 0, "");
            PreviewText.AddSyntaxElement("call", Colours[(int)SyntaxTypes.Z80], BenRyves.ColourfulEditor.ItemType.Word, false, 0, "");
            PreviewText.AddSyntaxElement("ret", Colours[(int)SyntaxTypes.Z80], BenRyves.ColourfulEditor.ItemType.Word, false, 0, "");
            PreviewText.AddSyntaxElement("ld", Colours[(int)SyntaxTypes.Z80], BenRyves.ColourfulEditor.ItemType.Word, false, 0, "");
            PreviewText.AddSyntaxElement("ix", Colours[(int)SyntaxTypes.Register], BenRyves.ColourfulEditor.ItemType.Word, false, 0, "");
            PreviewText.AddSyntaxElement(".db", Colours[(int)SyntaxTypes.Directive], BenRyves.ColourfulEditor.ItemType.Word, false, 0, "");

            string[] Flags = { "nz", "z", "nc", "c", "po", "pe", "p", "m", "a", "f", "b", "c", "d", "e", "h", "l", "hl", "de", "bc", "ix", "iy", "af", "r", "i", "sp", "pc", "af'", "hl'", "de'", "bc'" };
            foreach (String F in Flags) {
                PreviewText.AddSyntaxElement(F, Colours[(int)SyntaxTypes.Register], BenRyves.ColourfulEditor.ItemType.Word, false, 0, "");
            }
            PreviewText.ForeColor = Colours[(int)SyntaxTypes.Foreground];
            PreviewText.BackColor = Colours[(int)SyntaxTypes.Background];
            PreviewText.ForceRefresh();
        }

        private void Options_Load(object sender, EventArgs e) {

            InstalledFontCollection IFC = new InstalledFontCollection();
            foreach (FontFamily F in IFC.Families) {
                try {
                    Font ActualFont = new Font(F, 10);
                    FontSelection.Items.Add(F.Name);
                    if (F.Name == Properties.Settings.Default.EditorFont.Name) FontSelection.SelectedIndex = FontSelection.Items.Count - 1;
                } catch { }
                
            }
            for (int i = 6; i <= 24; ++i) {
                SizeSelection.Items.Add(i.ToString());                
            }
            SizeSelection.Text = Properties.Settings.Default.EditorFont.SizeInPoints.ToString();

            LoadThemes();

            this.PreviewText.Font = Properties.Settings.Default.EditorFont;
            this.PreviewText.BackColor = Properties.Settings.Default.Editor_Back_Colour;
            this.FilePattern.Text = Properties.Settings.Default.Project_Filter;

            this.AutoIndent.Checked = Properties.Settings.Default.Editor_AutoIndent;
            this.AutoSaveOnBuild.Checked = Properties.Settings.Default.Editor_AutoSave;
            this.MultipleRowsOfTabs.Checked = Properties.Settings.Default.IDE_Multiple_Tabs;
            this.NewlinesAsCRLF.Checked = Properties.Settings.Default.Editor_CRLF;


            Colours[(int)SyntaxTypes.Foreground] = Properties.Settings.Default.Editor_Text_Colour;
            Colours[(int)SyntaxTypes.Directive] = Properties.Settings.Default.Syntax_Directive;
            Colours[(int)SyntaxTypes.Z80] = Properties.Settings.Default.Syntax_Z80;
            Colours[(int)SyntaxTypes.Comment] = Properties.Settings.Default.Syntax_Comment;
            Colours[(int)SyntaxTypes.Routine] = Properties.Settings.Default.Syntax_Routine;
            Colours[(int)SyntaxTypes.String] = Properties.Settings.Default.Syntax_String;
            Colours[(int)SyntaxTypes.Label] = Properties.Settings.Default.Syntax_Label;
            Colours[(int)SyntaxTypes.Register] = Properties.Settings.Default.Syntax_Register;
            Colours[(int)SyntaxTypes.Background] = Properties.Settings.Default.Editor_Back_Colour;

            PreviewText.Font = Properties.Settings.Default.EditorFont;

            this.FilesToHighlight.Text = Properties.Settings.Default.Editor_Syntax_Files_To_Highlight;

            RejigPreview();
            PreviewText.Text = "; This is some sample text.\n\tld ix,_text_string\n\tcall _routine\t; Routine\n\tret\n_text_string:\n.db\t\"AaBbCcXxYyZz\",0";
            PreviewText.ForceRefresh();
        }


        private void LoadThemes() {
            XmlDefaults.Clear();
            SelectDefault.Items.Clear();
            try {
                string[] Themes = Directory.GetFiles(Application.StartupPath + @"\Templates\Colours", "*.xml");
                foreach (string S in Themes) {
                    try {
                        XmlDocument X = new XmlDocument();
                        X.Load(S);
                        try {
                            string Name = X.DocumentElement.Attributes.GetNamedItem("name").Value;
                            SelectDefault.Items.Add(Name);
                            XmlDefaults.Add(X);
                        } catch { }
                    } catch (Exception) { }
                }
            } catch { }

        }


        private void SelectDefault_SelectedIndexChanged(object sender, EventArgs e) {
            XmlDocument X;
            try {
                X = (XmlDocument)XmlDefaults[SelectDefault.SelectedIndex];
            } catch {
                return;
            }
            XmlNodeList L = X.GetElementsByTagName("colour");
            foreach (XmlNode N in L) {
                try {
                    string ColourName = N.Attributes.GetNamedItem("name").Value.ToLower();
                    Color SavedColour = Color.FromName(N.InnerText);
                    if (SavedColour.ToArgb() == 0) {
                        string[] ColourComponents = N.InnerText.Split(new char[] { ',' });
                        if (ColourComponents.Length == 3) {
                            SavedColour = Color.FromArgb(
                                Convert.ToInt16(ColourComponents[0].Trim(), 10),
                                Convert.ToInt16(ColourComponents[1].Trim(), 10),
                                Convert.ToInt16(ColourComponents[2].Trim(), 10));
                        }
                    }
                    switch (ColourName) {
                        case "z80":
                            Colours[(int)SyntaxTypes.Z80] = SavedColour;
                            break;
                        case "comment":
                            Colours[(int)SyntaxTypes.Comment] = SavedColour;
                            break;
                        case "string":
                            Colours[(int)SyntaxTypes.String] = SavedColour;
                            break;
                        case "routine":
                            Colours[(int)SyntaxTypes.Routine] = SavedColour;
                            break;
                        case "directive":
                            Colours[(int)SyntaxTypes.Directive] = SavedColour;
                            break;
                        case "label":
                            Colours[(int)SyntaxTypes.Label] = SavedColour;
                            break;
                        case "register":
                            Colours[(int)SyntaxTypes.Register] = SavedColour;
                            break;
                        case "foreground":
                            Colours[(int)SyntaxTypes.Foreground] = SavedColour;
                            break;
                        case "background":
                            Colours[(int)SyntaxTypes.Background] = SavedColour;
                            break;
                    }
                    
                } catch { }
            }
            RejigPreview();
            PreviewText.ForceRefresh();
        }

        private void SetBackgroundColour_Click(object sender, EventArgs e) {
            Color C = Colours[(int)SyntaxTypes.Background];
            ShowColourEditor(ref C);
            Colours[(int)SyntaxTypes.Background] = C;
            RejigPreview();
        }

        private void SaveCurrentTheme_Click(object sender, EventArgs e) {
            using (NewDirectory N = new NewDirectory("Enter a Theme Name:")) {
                if (N.ShowDialog() == DialogResult.OK) {
                    string EscapedStringFilename = N.DirectoryName.Text;
                    string InvalidChars = "\\/:*?\"<>| ";
                    foreach (char C in InvalidChars) {
                        EscapedStringFilename = EscapedStringFilename.Replace(C, '_');
                    }

                    string ThemeFilename = Path.Combine(Application.StartupPath, @"Templates\Colours\" + EscapedStringFilename + ".xml");
                    
         
                    if (File.Exists(ThemeFilename)) {
                        if (MessageBox.Show("A theme with the name " + N.DirectoryName.Text + " already exists - would you like to overwrite it?", "Save Theme",  MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
                    }
                    // Create a new XML theme file :)
                    XmlDocument X = new XmlDocument();
                    XmlElement Theme = X.CreateElement("theme");
                    XmlAttribute Name = X.CreateAttribute("name");
                    Name.Value = N.DirectoryName.Text;
                    Theme.Attributes.SetNamedItem(Name);
                    // Now we need to add all the colours:
                    AddColourToThemeFile(ref Theme, ref X, "z80", Colours[(int)SyntaxTypes.Z80]);
                    AddColourToThemeFile(ref Theme, ref X, "comment", Colours[(int)SyntaxTypes.Comment]);
                    AddColourToThemeFile(ref Theme, ref X, "string", Colours[(int)SyntaxTypes.String]);
                    AddColourToThemeFile(ref Theme, ref X, "routine", Colours[(int)SyntaxTypes.Routine]);
                    AddColourToThemeFile(ref Theme, ref X, "directive", Colours[(int)SyntaxTypes.Directive]);
                    AddColourToThemeFile(ref Theme, ref X, "label", Colours[(int)SyntaxTypes.Label]);
                    AddColourToThemeFile(ref Theme, ref X, "register", Colours[(int)SyntaxTypes.Register]);
                    AddColourToThemeFile(ref Theme, ref X, "foreground", Colours[(int)SyntaxTypes.Foreground]);
                    AddColourToThemeFile(ref Theme, ref X, "background", Colours[(int)SyntaxTypes.Background]);
                    X.AppendChild(Theme);
                    // Save it away:
                    try {
                        X.Save(ThemeFilename);
                        LoadThemes();
                    } catch (Exception ex) {
                        MessageBox.Show("There was an error saving the theme file:\n" + ex.Message, "Save theme", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void AddColourToThemeFile(ref XmlElement RootNode, ref XmlDocument MainDocument, string ColourName, Color Colour) {
            XmlElement ColourElement = MainDocument.CreateElement("colour");
            XmlAttribute Name = MainDocument.CreateAttribute("name");
            Name.Value = ColourName;
            ColourElement.Attributes.SetNamedItem(Name);
            string ColourToText = Colour.IsKnownColor ? Colour.Name : (Colour.R + ", " + Colour.G + ", " + Colour.B);
            ColourElement.InnerText = ColourToText;
            RootNode.AppendChild(ColourElement);
        }

        private void SetColour_Click(object sender, EventArgs e) {
            if (SelectedItems.SelectedIndex < 0) SelectedItems.SelectedIndex = 0;
            Color C = Colours[SelectedItems.SelectedIndex];
            ShowColourEditor(ref C);
            Colours[SelectedItems.SelectedIndex] = C;
            RejigPreview();
        }

        private void FontSelection_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                this.PreviewText.Font = new Font(FontSelection.Text, PreviewText.Font.Size);
                RejigPreview();
            } catch { }
            
        }

        private void SizeSelection_TextUpdate(object sender, EventArgs e) {

        }

        private void SizeSelection_TextChanged(object sender, EventArgs e) {
            try {
                this.PreviewText.Font = new Font(PreviewText.Font.Name, (float)Convert.ToDouble(SizeSelection.Text));
                RejigPreview();
            } catch { }
        }

		private void Options_Shown(object sender, EventArgs e) {
			this.TabIcons.Images.Clear();
			this.TabIcons.Images.Add("color_swatch", Properties.Resources.color_swatch);
			this.TabIcons.Images.Add("cog", Properties.Resources.cog);
			this.Invalidate(true);
		}
	}
}