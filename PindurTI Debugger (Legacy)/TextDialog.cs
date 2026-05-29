using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PindurTI_Debugger {
    public partial class TextDialog : Form {
        public TextDialog(string Text, string Caption, string Default, string Mask) {
            InitializeComponent();
            this.Text = Caption;
            this.Caption.Text = Text;
            this.Value.Mask = Mask;
            this.Value.Text = Default;
            
        }

        private void TextDialog_Load(object sender, EventArgs e) {
        }

        private void ButtonOK_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Value_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) {
            ToolTip T = new ToolTip();
            T.IsBalloon = true;
            T.ToolTipTitle = "Invalid Input";
            T.ToolTipIcon = ToolTipIcon.Warning;
            T.SetToolTip(this.Value, e.RejectionHint.ToString());
            T.Show("Try entering a valid value into the text box.", this.Value, 2000);
        }

        private void TextDialog_Shown(object sender, EventArgs e) {
        }
        
    }
}