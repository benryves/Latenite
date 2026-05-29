using System;
using System.Collections;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace Latenite {
    public class HelpItemFile : Object {
        public XmlNode HelpItem;
        public HelpItemFile(XmlNode HelpItemNode) {
            HelpItem = HelpItemNode;
        }
        public override string ToString() {
            foreach (XmlAttribute A in HelpItem.Attributes) {
                if (A.Name.ToLower() == "name") return A.Value;
            }
            return "";
        }

    }
	class HelpFile : Object {
		public XmlDocument HelpFileXML = new XmlDocument();
		public string Name = "";
        public bool ContainsHelp = false;
		public HelpFile(string Filename) {

			HelpFileXML.Load(Filename);
            foreach (XmlAttribute A in HelpFileXML.DocumentElement.Attributes) {
                if (A.Name.ToLower() == "name") {
                    ContainsHelp = true;
                    Name = A.Value;
                    break;
                }
            }
			

		}
		public void PopulateItemsCombo(ComboBox ComboToPopulate) {
			XmlNodeList X = HelpFileXML.GetElementsByTagName("item");
			for (int i = 0; i < X.Count; ++i) {
                foreach (XmlAttribute A in X.Item(i).Attributes) {
                    if (A.Name.ToLower() == "name") {
                        HelpItemFile E = new HelpItemFile(X.Item(i));
                        try {
                            ComboToPopulate.Items.Add(E);
                        } catch { }
                        break;
                    }
                }
			}
            if (ComboToPopulate.Items.Count > 0) ComboToPopulate.SelectedIndex = 0;
		}
	}
}
