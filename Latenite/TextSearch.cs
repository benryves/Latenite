using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace Latenite {
	public partial class LateniteIDE {

		private void TextSearch(string Needle, string Haystack, string Filename, bool MatchCase) {
			int SearchPos = 0;
			while (SearchPos<Haystack.Length) {
				if (MatchCase) {
					SearchPos = Haystack.IndexOf(Needle, SearchPos);
				} else {
					SearchPos = Haystack.ToLower().IndexOf(Needle.ToLower(), SearchPos);
				}
				if (SearchPos == -1) break;
				ListViewItem L = new ListViewItem(Path.GetFileName(Filename));
				int EndOfLine = Haystack.IndexOf('\n', SearchPos);
				if (EndOfLine == -1) EndOfLine = Haystack.Length - 1;
				int StartOfLine = SearchPos;
				char[] C = Haystack.ToCharArray();

				while (StartOfLine > 1) {
					if (C[StartOfLine - 1] == '\n') break;
					--StartOfLine;
				}
				L.SubItems.Add(Haystack.Substring(StartOfLine, EndOfLine - StartOfLine).Replace('\t',' ').Trim());
				// Calculate the line number:
				string[] Lines = Haystack.Substring(0,SearchPos).Split('\n');
				L.SubItems.Add(Convert.ToString(Lines.Length));
				L.Tag = Filename;
				this.SearchResults.Items.Add(L);
				SearchPos += Needle.Length;
			}
		}

	

		
	}
}
