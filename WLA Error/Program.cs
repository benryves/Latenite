using System;
using System.Collections.Generic;
using System.Text;

namespace WLA_Error {
    class Program {
        static private string EscapeHTML(string ToEscape) {
            return ToEscape.Replace("&", "&amp;").Replace("\"", "&quot;").Replace("<", "&lt;").Replace(">", "&gt;").Trim();
        }
        static void Main(string[] args) {
            bool IgnoreXMLHeader = false;
            bool IsWLA = true;
            foreach (String S in args) {
                if (S.StartsWith("/")) {
                    string R = S.ToLower();
                    if (R.EndsWith("o")) {
                        IgnoreXMLHeader = true;
                    } else if (R.EndsWith("2")) {
                        IsWLA = false;
                    }
                }
            }
            if (!IgnoreXMLHeader) Console.WriteLine("<?xml version=\"1.0\"?><latenite version=\"2\">");
            while (true) {
                string ErrorLine = Console.ReadLine();
                if (ErrorLine == null) break;	// All done!
                ErrorLine = ErrorLine.Trim();
                try {
                    if (IsWLA || ErrorLine.IndexOf(':') > 0) {
                        string FullError = "";
                        string[] Errors = ErrorLine.Split(new char[] { ':' });
                        int Start = IsWLA ? 2 : 3;
                        bool First = true;
                        for (int i = Start; i < Errors.Length; ++i) {
                            if (First) {
                                First = false;
                            } else {
                                FullError += ":";
                            }
                            FullError += Errors[i];
                        }


                        if (Errors.Length == 2) {
                            string Tag = (Errors[0].Trim().ToUpper().StartsWith("WARNING")) ? "warning" : "error";
                            Console.WriteLine("<" + Tag + ">" + EscapeHTML(Errors[0]) + ":" + EscapeHTML(Errors[1]) + "</" + Tag + ">");
                        } else {
                            string Tag = (FullError.Trim().ToUpper().StartsWith("WARNING")) ? "warning" : "error";
                            Console.WriteLine("<" + Tag + " line=\"" + EscapeHTML(Errors[Start - 1]) + "\" file=\"" + EscapeHTML(Errors[Start - 2]) + "\">" + EscapeHTML(FullError) + "</" + Tag + ">");
                        }

                    } else {
                        string Tag = (ErrorLine.StartsWith("LOAD_FILE")) ? "error" : "message";
                        Console.WriteLine("<" + Tag + ">" + EscapeHTML(ErrorLine) + "</" + Tag + ">");
                    }
                } catch {
                    Console.WriteLine("<message>" + EscapeHTML(ErrorLine) + "</message>");
                }

             
            }
            if (!IgnoreXMLHeader) Console.WriteLine("</latenite>");
        }
    }
}
