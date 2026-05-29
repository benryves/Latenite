/* TASMERR.EXE
 * TASM Error Processor
 * Pipe TASM's output into this application for it to output a Latenite XML-style error log.
 * EG:
 * C:\>TASM <args> | TASMERR
 * You can then pipe the output to another program, or write it to disk using >
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace TASM_Error {
	class Program {
		static void Main(string[] args) {

			bool IgnoreXMLHeader = false;
			foreach (String S in args) {
				if (S.StartsWith("/")) {
					string R = S.ToLower();
					if (R.EndsWith("o")) {
						IgnoreXMLHeader = true;
					}
				}
			}
			if (!IgnoreXMLHeader) Console.WriteLine("<?xml version=\"1.0\"?><latenite version=\"2\">");
			while (true) {
				string ErrorLine = Console.ReadLine();
				if (ErrorLine == null) break;	// All done!
				ErrorLine = ErrorLine.Trim();
				if (ErrorLine.StartsWith("tasm: source file open error on ")) {
					// Source file open error
                    Console.WriteLine("<error>" + "S" + EscapeHTML(ErrorLine.Substring(7)) + "</error>\n");
				} else if (ErrorLine.StartsWith("tasm: Number of errors = ")) {
					// Error count
                    Console.Write("<message>" + EscapeHTML(ErrorLine) + "</message>\n");
				} else if (ErrorLine.StartsWith("tasm:")) {
					// Unknown TASM message
                    Console.Write("<message>" + EscapeHTML(ErrorLine) + "</message>\n");
				} else {
					// General error
					int LinePos = ErrorLine.IndexOf(" line ");
					if (LinePos != -1) {
						int ErrorPos = ErrorLine.IndexOf(": ", LinePos);
						if (ErrorPos != -1) {
							// Get line number/source file name:
							string SourceFile = ErrorLine.Substring(0, LinePos).Replace("/","\\");
							string LineNumber = ErrorLine.Substring(LinePos + 6, 4);
							string ErrMessage = ErrorLine.Substring(ErrorPos + 2);
                            Console.Write("<error line=\"" + LineNumber + "\" file=\"" + EscapeHTML(SourceFile) + "\">" + EscapeHTML(ErrMessage) + "</error>\n");
						} else {
                            Console.Write("<message>" + EscapeHTML(ErrorLine) + "</message>\n");
						}
					} else {
                        Console.Write("<message>" + EscapeHTML(ErrorLine) + "</message>\n");
					}
				}
				
			}
			if (!IgnoreXMLHeader) Console.WriteLine("</latenite>");
		}
        static string EscapeHTML(string Data) {
            return Data.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("&", "&amp;");
        }
	}
}