using System;
using System.Xml;
using System.Windows.Forms;
using System.IO;

namespace Latenite {
    public class HelpItemFile : Object {
        public XmlNode HelpItem;
        public HelpItemFile(XmlNode HelpItemNode) {
            HelpItem = HelpItemNode;
        }
        public override string ToString() {
			return HelpItem.Attributes["name"]?.Value;
        }
    }
	class HelpFile : Object {
		public XmlDocument HelpFileXML = new XmlDocument();
		public string Name = "";
        public bool ContainsHelp = false;
		public HelpFile(string Filename) {
			Name = Path.GetFileName(Filename);
			HelpFileXML.Load(Filename);
			Name = HelpFileXML.DocumentElement.Attributes.GetNamedItem("name")?.Value;
			ContainsHelp = HelpFileXML.GetElementsByTagName("item").Count > 0;
		}
		public void PopulateItemsCombo(ComboBox ComboToPopulate) {
			foreach (XmlNode X in HelpFileXML.GetElementsByTagName("item")) {
				if (X.Attributes["name"] != null) {
					HelpItemFile E = new HelpItemFile(X);
					ComboToPopulate.Items.Add(E);
				}
			}
            if (ComboToPopulate.Items.Count > 0) ComboToPopulate.SelectedIndex = 0;
		}
		public override string ToString() {
			return this.Name;
		}
	}
}
