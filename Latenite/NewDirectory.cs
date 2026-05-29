using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Latenite {
    public partial class NewDirectory : Form {
        public NewDirectory(string BoxCaption) {
            InitializeComponent();
            this.Text = BoxCaption;
        }

        private void NewDirectory_Load(object sender, EventArgs e) {
            this.Show();
            Application.DoEvents();
            DirectoryName.Focus();
        }

        private void ButtonOK_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}