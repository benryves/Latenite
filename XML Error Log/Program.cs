using System;
using System.Collections.Generic;
using System.Text;

namespace XML_Error_Log {
	class Program {
		static void Main(string[] args) {
			if (args.Length == 0) return;
			switch (args[0].ToLower()) {
				case "start":
					Console.WriteLine("<?xml version=\"1.0\"?><latenite version=\"2\">");
					break;
				case "end":
					Console.WriteLine("</latenite>");
					break;
				default:
					break;
			}
		}
	}
}
