using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Latenite {
    public partial class GoToLine : Form {

        SourceFile BaseSourceFile;
        int MaxLines;
        public GoToLine() {
            InitializeComponent();
            BaseSourceFile = (SourceFile)Program.MainIDE.GetActiveSourceFile();
            MaxLines = BaseSourceFile.TextEditor.Lines.Length;
            if (MaxLines < 1) MaxLines = 1;
            LineCountLabel.Text = "Line number (1 - " + MaxLines + "):";
            
        }
        
        private void GoToLine_Load(object sender, EventArgs e) {
            Show();
            Application.DoEvents();
            LineToSelect.Focus();
            
        }

        private void GoToCancel_Click(object sender, EventArgs e) {
            Close();
        }

        private void GoToOk_Click(object sender, EventArgs e) {
            int LineNumber = -1;
            try {
                LineNumber = Convert.ToInt32(LineToSelect.Text, 10);
            } catch { }
            if (LineNumber < 1 || LineNumber > MaxLines) {
                MessageBox.Show("'" + LineToSelect.Text +  "' is not a valid line number.", "Go To Line", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            } else {
                BaseSourceFile.SelectLine(LineNumber);
            }
            Close();
        }
    }
}