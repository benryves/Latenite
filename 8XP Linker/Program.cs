using System;
using System.Collections;
using System.Text;
using System.IO;

namespace _8XP_Linker {
	class Program {
		private class RawBinary {
			public string Filename = "";
			public string Name = "";
			public int Type = 6;
		}

		static bool UseXML = false;
		static bool Is83P = false;
		static bool IgnoreXMLHeader = false;

		public static ArrayList Data = new ArrayList();
		public static string Comment = "Created by Latenite";

		/// <summary>
		/// Add a variable to the data block
		/// </summary>
		/// <param name="VariableHeader">Variable header (data type followed by 8 NUL padded characters for a name)</param>
		/// <param name="VariableData">ArrayList of the data</param>
		public static void AddVariable(ArrayList VariableHeader, ArrayList VariableData) {
			// Note that this adds extra gubbins to the binary.
			// There is a difference between the plain binary program and the binary on the calculator;
			// the calculator binary has one extra u16bit value in it denoting the program's size.
			// Hence we add the VariableData.Count+2 (2 extra bytes for the extra size info);
            // Also note a few minor differences with the TI83 and the TI83+ versions.
       
            Data.Add(Is83P ? 0x0B : 0x0D);
			Data.Add(0x00);

			Data.Add((VariableData.Count + 2) & 0xFF);
			Data.Add((VariableData.Count + 2) >> 8);
			for (int i = 0; i < 9; i++) {
				Data.Add(VariableHeader[i]);
			}
            if (!Is83P) {
                Data.Add(0x00);
                Data.Add(0x00);
            }

			Data.Add((VariableData.Count + 2) & 0xFF);
			Data.Add((VariableData.Count + 2) >> 8);
			Data.Add(VariableData.Count & 0xFF);
			Data.Add(VariableData.Count >> 8);
			for (int i = 0; i < VariableData.Count; ++i) {
				Data.Add(VariableData[i]);
			}
		}

		/// <summary>
		/// Save the data block to disk (exceptions should be handled externally!)
		/// </summary>
		/// <param name="Filename">Name of the file to save in</param>
		public static void Save(string Filename) {

			if (File.Exists(Filename)) {
				File.Delete(Filename);
			}
			using (BinaryWriter W = new BinaryWriter(File.OpenWrite(Filename), Encoding.ASCII)) {
				if (Is83P) {
					W.Write("**TI83**".ToCharArray());
				} else {
					W.Write("**TI83F*".ToCharArray());
				}
				W.Write((byte)0x1A);
				W.Write((byte)0x0A);
				W.Write((byte)0x00);

				string FinalComment = Comment.PadRight(42, (char)0x00);
				W.Write(FinalComment.ToCharArray());



				W.Write((byte)(Data.Count & 0xFF));
				W.Write((byte)(Data.Count >> 8));

				ushort CheckSum = 0;
				for (int i = 0; i < Data.Count; i++) {
					byte b = (byte)(Convert.ToByte(Data[i]));
					CheckSum += b;
					W.Write(b);
				}

				W.Write((byte)(CheckSum & 0xFF));
				W.Write((byte)(CheckSum >> 8));
			}

		}

        /// <summary>
        /// Write an error to the error log.
        /// </summary>
        /// <param name="Error">The text for the error.</param>
        /// <param name="Type">Set as a string for the tag/description (use error, warning or message)</param>
		private static void LogError(string Error, string Type) {
			if (UseXML) {
				Console.WriteLine("<" + Type + ">" + Error + "</" + Type + ">");
			} else {
				Console.WriteLine(Type + ": " + Error);
			}
		}

        /// <summary>
        /// Main entry point
        /// </summary>
        /// <param name="args">Command-line arguments</param>
		static void Main(string[] args) {
			if (args.Length == 0) {
				Console.WriteLine("8XP Linker by Ben Ryves - Syntax:\n8XPLINK (Binary[,Name[,Type=6]]) <Output> [/x]\neg: 8XPLINK (MYFILE.BIN,MyFile) (MYFILE2.BIN,MyFile2,5) MYFILE.8XP /x\nUse the /x switch to echo error/warning messages in Latenite-friendly XML.");
				return;
			}
			ArrayList FilesToLink = new ArrayList();
			string OutFileName = "";
			foreach (string S in args) {
				if (S.StartsWith("(") && S.EndsWith(")")) {
					string RS = S.TrimStart("(".ToCharArray()).TrimEnd(")".ToCharArray());

					// Add a new binary;
					RawBinary R = new RawBinary();
					string[] Params = RS.Split(",".ToCharArray());
					if (Params.Length >= 1) {
						R.Filename = Params[0].Replace("\"", "");
						if (Params.Length >= 2) {
							R.Name = Params[1].Replace("\"", "");
							if (Params.Length >= 3) {
								R.Type = Convert.ToInt32(Params[2].Replace("\"", "").ToString(), 10);
							}
						} else {
							R.Name = Path.GetFileNameWithoutExtension(R.Filename);
						}
					}
					FilesToLink.Add(R);
				} else if (S.StartsWith("/")) {
					if (S.ToLower().EndsWith("x")) {
						UseXML = true;
					} else if (S.EndsWith("3")) {
						Is83P = true;
					} else if (S.ToLower().EndsWith("o")) {
						IgnoreXMLHeader = true;
					}
				} else {
					OutFileName = S;
				}
			}

			if (!IgnoreXMLHeader && UseXML) Console.WriteLine("<?xml version=\"1.0\"?><latenite version=\"2\">");

			// Now link 'em:
			int SuccessLinks = 0;
			foreach (RawBinary R in FilesToLink) {
				ArrayList H = new ArrayList();

				H.Add(R.Type);
				foreach (char C in R.Name.ToString()) {
					H.Add(C);
				}
				while (H.Count < 9) {
					H.Add(0);
				}
				if (H.Count > 9) {
					LogError("Variable name exceeds 8 character and has been truncated.", "warning");
				}
				if (R.Name.ToUpper() != R.Name) {
					LogError("Lowercase variable names (" + R.Name + ") can cause issues with the TI-OS.", "warning");
				}
				ArrayList D = new ArrayList();
				try {
					using (BinaryReader B = new BinaryReader(File.OpenRead(R.Filename))) {
						for (int i = 0; i < B.BaseStream.Length; ++i) {
							D.Add(B.ReadByte());						
						}
					}
					AddVariable(H, D);
					SuccessLinks++;
				} catch (Exception ex) {
					LogError(ex.Message, "error");					
				}

				
			}
			try {
				Save(OutFileName);
				LogError("Linked " + SuccessLinks + " file" + (SuccessLinks == 1 ? "" : "s") + " successfully!", "message");
			} catch (Exception ex) {
				LogError(ex.Message, "error");
			}
			
			if (!IgnoreXMLHeader && UseXML) Console.WriteLine("</latenite>");
		}

	}
}
