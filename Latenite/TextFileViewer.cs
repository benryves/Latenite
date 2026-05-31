using System;
using System.Drawing;
using System.Windows.Forms;

namespace Latenite {
	public partial class TextFileViewer : Form {

		public string TextFileText {
			get {
				return this.textFileBox.Text;
			}
			set { this.textFileBox.Text = value; }
		}

		public Color TextFileForeColor {
			get { return this.textFileBox.ForeColor; }
			set { this.textFileBox.ForeColor = value; }
		}
		public Color TextFileBackColor {
			get { return this.textFileBox.BackColor; }
			set { this.textFileBox.BackColor = value; }
		}

		public Font TextFileFont {
			get { return this.textFileBox.Font; }
			set { this.textFileBox.Font = value; }
		}

		public bool TextFileWordWrap {
			get { return this.textFileBox.WordWrap; }
			set { this.textFileBox.WordWrap = value; }
		}

		public int TextFileWidth {
			set {
				using (Graphics g = Graphics.FromHwnd(this.Handle)) {
					this.ClientSize = new Size((int)Math.Ceiling(g.MeasureString("".PadRight(value, 'M'), this.TextFileFont).Width), this.ClientSize.Height);
				}
			}
		}

		public TextFileViewer() {
			InitializeComponent();
		}

		private void TextFileViewer_Shown(object sender, System.EventArgs e) {
			this.textFileBox.SelectionLength = this.textFileBox.SelectionStart = 0;
		}

		private void TextFileViewer_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Enter) {
				this.Close();
			}
		}
	}
}
