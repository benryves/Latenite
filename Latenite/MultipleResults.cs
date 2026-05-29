using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Latenite {
    public partial class MultipleResults : Form {
        private ArrayList ResultSet;
        public MultipleResults(ArrayList Results) {
            InitializeComponent();
            ResultSet = Results;
        }
        private void MultipleResults_Load(object sender, EventArgs e) {
            foreach (XmlNode N in ResultSet) {
                string ToAdd = "";
                try {
					ToAdd = N.OwnerDocument.DocumentElement.Attributes.GetNamedItem("name").Value + ": " + N.Attributes.GetNamedItem("name").Value;
                } catch { }
                this.SearchResults.Items.Add(ToAdd);
            }
            this.SearchResults.SelectedIndex = 0;

        }

        private void SelectCancel_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void SelectOK_Click(object sender, EventArgs e) {
            try {
                Program.MainIDE.DisplayHelpFile((XmlNode)ResultSet[SearchResults.SelectedIndex]);
            } catch { }
            this.Close();
        }

        private void SearchResults_DoubleClick(object sender, EventArgs e) {
            SelectOK.PerformClick();
        }
    }
}