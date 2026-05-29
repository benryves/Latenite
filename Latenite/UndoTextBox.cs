using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;
using BenRyves;

namespace Latenite {
	/// <summary>
	/// A class that provides a text box that has slightly less crappy undo/redo operations!
	/// </summary>
    public class UndoTextBox : ColourfulEditor {
        public bool WasUpdatingColours = false;
        public void AdjustColours() {


            this.Font = Properties.Settings.Default.EditorFont;
            this.ForeColor = Properties.Settings.Default.Editor_Text_Colour;
            this.BackColor = Properties.Settings.Default.Editor_Back_Colour;

            this.AddSeperator(' ');
            this.AddSeperator('\r');
            this.AddSeperator('\n');
            this.AddSeperator('\t');
            this.AddSeperator(',');
            this.AddSeperator('-');
            this.AddSeperator(';');
            this.AddSeperator('+');
            this.AddSeperator('*');
            this.AddSeperator('(');
            this.AddSeperator(')');
            this.AddSeperator(',');
            this.AddSeperator('\'');
            this.AddSeperator('"');
            
            AddSyntaxElement("\"", Properties.Settings.Default.Syntax_String, ItemType.String, false);
            AddSyntaxElement("'", Properties.Settings.Default.Syntax_String, ItemType.String, false);
            
            AddSyntaxElement(":", Properties.Settings.Default.Syntax_Label, ItemType.ToStartOfWord, false);
            AddSyntaxElement(";", Properties.Settings.Default.Syntax_Comment, ItemType.ToEndOfLine, false);

            string[] Flags = { "nz", "z", "nc", "c", "po", "pe", "p", "m", "a", "f", "b", "c", "d", "e", "h", "l", "hl", "de", "bc", "ix", "iy", "af", "r", "i", "sp", "pc", "af'", "hl'", "de'", "bc'" };
            foreach (String F in Flags) {
                AddSyntaxElement(F, Properties.Settings.Default.Syntax_Register, ItemType.Word, false);
            }

            foreach (HelpFile X in Program.MainIDE.HelpList) {
                XmlNodeList L = X.HelpFileXML.GetElementsByTagName("item");
                foreach (XmlNode N in L) {
                    string[] ToAdd = null;
                    string ColourCode = "routine";
                    foreach (XmlAttribute A in N.Attributes) {
                        if (A.Name.ToLower() == "highlight") {
                            ToAdd = A.Value.Split(new char[] { '/' });
                        } else if (A.Name.ToLower() == "colour") {
                            ColourCode = A.Value.ToLower();
                        }
                    }
                    if (ToAdd != null) {
                        foreach (string S in ToAdd) {
                            Color ToColour = Properties.Settings.Default.Syntax_Routine;
                            switch (ColourCode) {
                                case "routine": ToColour = Properties.Settings.Default.Syntax_Routine; break;
                                case "z80": ToColour = Properties.Settings.Default.Syntax_Z80; break;
                                case "directive": ToColour = Properties.Settings.Default.Syntax_Directive; break;
                                default: break;
                            }
                            AddSyntaxElement(S, ToColour, ItemType.Word, true);
                        }
                    }
                }
            }
            ForceRefresh();
            WasUpdatingColours = true;
        }

        public UndoTextBox() {
            this.AutoWordSelection = false;
            AdjustColours();
            this.BorderStyle = BorderStyle.None;

        }

        /*
		private class UndoStatus {
			public int SelectionStart = 0;
			public int SelectionLength = 0;
			public string Text = "";
			public UndoStatus(int SelectionStart, int SelectionLength, string Text) {
				this.Text = Text;
				this.SelectionLength = SelectionLength;
				this.SelectionStart = SelectionStart;

			}
		};

		ArrayList UndoStack = new ArrayList();

		public UndoTextBox() {
			ResetUndoStack();
			this.TextChanged += new EventHandler(UndoTextBox_TextChanged);
		}

		private bool IsUndoing = false;

		private int StackPointer = 0;

		private UndoStatus OldText = new UndoStatus(0, 0, "");

		void UndoTextBox_TextChanged(object sender, EventArgs e) {
			if (!IsUndoing) {
				StackPointer++;
				UndoStack.RemoveRange(StackPointer, UndoStack.Count - StackPointer);
				UndoStack.Add(new UndoStatus(this.SelectionStart, this.SelectionLength, this.Text));
			}
		}

		public void ResetUndoStack() {
			OldText = new UndoStatus(this.SelectionStart, this.SelectionLength, this.Text);
			UndoStack.Clear();
			StackPointer = 0;
			UndoStack.Add(new UndoStatus(this.SelectionStart, this.SelectionLength, this.Text));
		}

		private void ChangeState() {
			UndoStatus State = (UndoStatus)UndoStack[StackPointer];
			IsUndoing = true;
			this.Text = State.Text;
			IsUndoing = false;
			this.SelectionStart = State.SelectionStart;
			this.SelectionLength = State.SelectionLength;
			this.ScrollToCaret();
		}

		new public void Undo() {
			if (StackPointer < 1) return;
			StackPointer--;
			ChangeState();

		}

		public void Redo() {
			if (StackPointer >= UndoStack.Count-1) return;
			StackPointer++;
			ChangeState();
		}

        */




	}
}
