using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Text;
using System.Runtime.InteropServices;

namespace UrielGuy.SyntaxHighlightingTextBox
{
	/// <summary>
	/// A textbox the does syntax highlighting.
	/// </summary>
	public class SyntaxHighlightingTextBox :	System.Windows.Forms.RichTextBox 
	{
		#region Members

		//Members exposed via properties
		private SeperaratorCollection mSeperators = new SeperaratorCollection();  
		private HighLightDescriptorCollection mHighlightDescriptors = new HighLightDescriptorCollection();
		private bool mCaseSesitive = false;
		private bool mFilterAutoComplete = false;

		//Internal use members
		private bool mAutoCompleteShown = false;
		private bool mParsing = false;
		private bool mIgnoreLostFocus = false;

		private AutoCompleteForm mAutoCompleteForm;

		//Undo/Redo members
		private ArrayList mUndoList = new ArrayList();
		private Stack mRedoStack = new Stack();
		private bool mIsUndo = false;
		private UndoRedoInfo mLastInfo = new UndoRedoInfo("", new Win32.POINT(), 0);
		private int mMaxUndoRedoSteps = 50;
       

		#endregion

		#region Properties
		/// <summary>
		/// Determines if token recognition is case sensitive.
		/// </summary>
		[Category("Behavior")]
		public bool CaseSensitive 
		{ 
			get 
			{ 
				return mCaseSesitive; 
			}
			set 
			{ 
				mCaseSesitive = value;
			}
		}


		/// <summary>
		/// Sets whether or not to remove items from the Autocomplete window as the user types...
		/// </summary>
		[Category("Behavior")]
		public bool FilterAutoComplete 
		{
			get 
			{
				return mFilterAutoComplete;
			}
			set 
			{
				mFilterAutoComplete = value;
			}
		}

		/// <summary>
		/// Set the maximum amount of Undo/Redo steps.
		/// </summary>
		[Category("Behavior")]
		public int MaxUndoRedoSteps 
		{
			get 
			{
				return mMaxUndoRedoSteps;
			}
			set
			{
				mMaxUndoRedoSteps = value;
			}
		}

			/// <summary>
			/// A collection of charecters. a token is every string between two seperators.
			/// </summary>
			/// 
			public SeperaratorCollection Seperators 
		{
			get 
			{
				return mSeperators;
			}
		}
		
		/// <summary>
		/// The collection of highlight descriptors.
		/// </summary>
		/// 
		public HighLightDescriptorCollection HighlightDescriptors 
		{
			get 
			{
				return mHighlightDescriptors;
			}
		}

		#endregion

		#region Overriden methods

		/// <summary>
		/// The on text changed overrided. Here we parse the text into RTF for the highlighting.
		/// </summary>
		/// <param name="e"></param>
        /// 

        string OldText = "";
		protected override void OnTextChanged(EventArgs e) {
            if (Text != OldText) { // Only capture real text changes
                if (mParsing) return;              
                // Now, do we really want to update the colouring?
                if (LineHighlightedWhenLastColoured != LineCurrentlyEditing) {
                    UpdateColouring(e);
                }
                //
                base.OnTextChanged(e);
            }
            OldText = Text;
            
		}


        private ArrayList RTFCodes = new ArrayList();

        /// <summary>
        /// Added constructor to set up the autocomplete form properly (and allow 2-way comms between them).
        /// </summary>
        public SyntaxHighlightingTextBox() {
            mAutoCompleteForm = new AutoCompleteForm(this);
        }


        private class RTFCode : object {
            public string RTF = "";
            public string Text = "";
            public RTFCode(string _Text, string _RTF) {
                RTF = _RTF;
                Text = _Text;
            }
        }

        
        private string EscapeRTF(string Text) {
            return Text.Replace("\\", "\\\\").Replace("{", "\\{").Replace("}", "\\}");
        }
        private string EscapeRTF(char Character) {
            return EscapeRTF(Character.ToString());
        }

        private int GetCurrentLineNumber() {
            return (this.Text == "") ? 0 : (this.Text + "\r\r").Remove(this.SelectionStart).Split(new char[] { '\n' }).Length;
            
        }

        int LineCurrentlyEditing = -1;
        protected override void OnSelectionChanged(EventArgs e) {

            if (mParsing) return;

            int LineNumber = GetCurrentLineNumber();

            if (LineCurrentlyEditing != LineNumber) {
                UpdateColouring(null);
            }
            LineCurrentlyEditing = LineNumber;
            base.OnSelectionChanged(e);
        }



        int LineHighlightedWhenLastColoured = -1;
        public void UpdateColouring(EventArgs e) {

            if (mParsing) return;
            mParsing = true;

            if (!mIsUndo) {
                mRedoStack.Clear();
                mUndoList.Insert(0, mLastInfo);
                this.LimitUndo();
                mLastInfo = new UndoRedoInfo(Text, GetScrollPos(), SelectionStart);
            }

            int LineNumber = GetCurrentLineNumber();
            //Save scroll bar and cursor position, changeing the RTF moves the cursor and scrollbars to top positin
            Win32.POINT scrollPos = GetScrollPos();
            int cursorLoc = SelectionStart;

            //Created with an estimate of how big the stringbuilder has to be...
            StringBuilder sb = new
                StringBuilder((int)(Text.Length * 1.5 + 150));

            //Adding RTF header
            sb.Append(@"{\rtf1\fbidis\ansi\ansicpg1255\deff0\deflang1037{\fonttbl{");

            //Font table creation
            int fontCounter = 0;
            Hashtable fonts = new Hashtable();
            AddFontToTable(sb, Font, ref fontCounter, fonts);
            foreach (HighlightDescriptor hd in mHighlightDescriptors) {
                if ((hd.Font != null) && !fonts.ContainsKey(hd.Font.Name)) {
                    AddFontToTable(sb, hd.Font, ref fontCounter, fonts);
                }
            }
            sb.Append("}\n");

            //ColorTable

            sb.Append(@"{\colortbl ;");
            Hashtable colors = new Hashtable();
            int colorCounter = 1;
            AddColorToTable(sb, ForeColor, ref colorCounter, colors);
            AddColorToTable(sb, BackColor, ref colorCounter, colors);

            foreach (HighlightDescriptor hd in mHighlightDescriptors) {
                if (!colors.ContainsKey(hd.Color)) {
                    AddColorToTable(sb, hd.Color, ref colorCounter, colors);
                }
            }

            //Parsing text

            sb.Append("}\n").Append(@"\viewkind4\uc1\pard\ltrpar");
            SetDefaultSettings(sb, colors, fonts);

            ArrayList NewRTFCodes = new ArrayList();
            NewRTFCodes.Add(new RTFCode("", ""));


            int CachedRTF = 0;

            bool FirstLine = true;
            int LineCt = 0;
            foreach (string L in Lines) {
                LineCt++;
                if (FirstLine) {
                    FirstLine = false;
                } else {
                    sb.Append("\\par\n");
                }

                // Have we already created this code?

                string RTFCodeToAdd = "";
                bool IsValidTranslation = true;
                if (e!=null && Latenite.Properties.Settings.Default.Editor_Syntax_Ignore_Current_Line && LineNumber == LineCt) {
                    RTFCodeToAdd = EscapeRTF(L);
                    IsValidTranslation = false;
                } else {

                    int CachedRTFA = CachedRTF;
                    int CachedRTFB = CachedRTF;

                    int MaxCounts = 0;

                    while (CachedRTFA < RTFCodes.Count && ((RTFCode)RTFCodes[CachedRTFA]).Text != L && MaxCounts++ != 32) {
                        ++CachedRTFA;
                    }

                    MaxCounts = 0;
                    if (!(CachedRTFA < RTFCodes.Count && L == ((RTFCode)RTFCodes[CachedRTFA]).Text) && MaxCounts++ != 32) {
                        // We only need to search backwards in odd cases
                        while (CachedRTFB >= 0 && ((RTFCode)RTFCodes[CachedRTFB]).Text != L) {
                            --CachedRTFB;
                        }
                    }





                    if (CachedRTFA < RTFCodes.Count && L == ((RTFCode)RTFCodes[CachedRTFA]).Text) {
                        CachedRTF = CachedRTFA;
                        RTFCodeToAdd = ((RTFCode)RTFCodes[CachedRTF]).RTF;
                    } else if (CachedRTFB >= 0 && L == ((RTFCode)RTFCodes[CachedRTFB]).Text) {
                        CachedRTF = CachedRTFB;
                        RTFCodeToAdd = ((RTFCode)RTFCodes[CachedRTF]).RTF;
                    } else {


                        //if (NumberOfTranslations++ < 200) {

                            char[] LineAsChar = (L + "\n").ToCharArray();

                            // Now we handle the conversion:

                            RTFCodeToAdd = "";
                            string CurToken = "";

                            bool TokenIsAtStart = true;


                            for (int i = 0; i < LineAsChar.Length; ++i) {
                                char S = LineAsChar[i];
                                if (mSeperators.Contains(S)) {
                                    // It is a sep, so ignore it. Just spit it out and clear the current token.

                                    if (TokenIsAtStart && CurToken.EndsWith(":")) {
                                        SetColor(ref RTFCodeToAdd, Latenite.Properties.Settings.Default.Syntax_Label, colors);
                                    }

                                    foreach (HighlightDescriptor H in mHighlightDescriptors) {
                                        if (H.Token.ToLower() == CurToken.ToLower() && H.DescriptorType == DescriptorType.Word) {
                                            SetColor(ref RTFCodeToAdd, H.Color, colors);
                                            break;
                                        }
                                    }



                                    RTFCodeToAdd += EscapeRTF(CurToken);
                                    SetColor(ref RTFCodeToAdd, ForeColor, colors);
                                    RTFCodeToAdd += EscapeRTF(S);
                                    CurToken = "";
                                    TokenIsAtStart = false;
                                } else {
                                    bool HandledToken = false;
                                    foreach (HighlightDescriptor H in mHighlightDescriptors) {
                                        if (H.Token.StartsWith(S.ToString())) {
                                            if (H.DescriptorType == DescriptorType.ToEOL) {
                                                // Flush current token
                                                RTFCodeToAdd += EscapeRTF(CurToken); CurToken = "";
                                                // Set colour
                                                SetColor(ref RTFCodeToAdd, H.Color, colors);
                                                RTFCodeToAdd += EscapeRTF(L.Substring(i));
                                                HandledToken = true;
                                                i = LineAsChar.Length; // Exit the entire for loop system
                                                TokenIsAtStart = false;
                                                break;
                                            } else if (H.DescriptorType == DescriptorType.ToCloseToken) {
                                                // Flush current token
                                                RTFCodeToAdd += EscapeRTF(CurToken); CurToken = "";
                                                // Set colour
                                                SetColor(ref RTFCodeToAdd, H.Color, colors);
                                                // Add open token
                                                RTFCodeToAdd += EscapeRTF(S);
                                                // Now we need to rattle on through until we hit the close marker.
                                                while (i < LineAsChar.Length - 1 && (LineAsChar[++i].ToString() != H.CloseToken || (i == 0 || LineAsChar[i - 1] == '\\'))) {
                                                    RTFCodeToAdd += EscapeRTF(LineAsChar[i]);
                                                }
                                                if (i < LineAsChar.Length) {
                                                    RTFCodeToAdd += EscapeRTF(LineAsChar[i]);
                                                }
                                                // Back to the boring colour:
                                                SetColor(ref RTFCodeToAdd, ForeColor, colors);
                                                HandledToken = true;
                                                TokenIsAtStart = false;
                                                break;
                                            }
                                        }
                                    }
                                    if (!HandledToken) CurToken += S;
                                }
                            }


                            SetColor(ref RTFCodeToAdd, ForeColor, colors);

                            RTFCodes.Add(new RTFCode(L, RTFCodeToAdd));

                        /*} else {
                            IsValidTranslation = false;
                            HasCompletedTranslation = false;
                            RTFCodeToAdd = EscapeRTF(L);
                        }*/


                    }
                }
                sb.Append(RTFCodeToAdd);
                if (IsValidTranslation) NewRTFCodes.Add(new RTFCode(L, RTFCodeToAdd));
            }
            RTFCodes = NewRTFCodes;


            Win32.LockWindowUpdate(Handle);

            Rtf = sb.ToString();
            SelectionStart = cursorLoc;
            SetScrollPos(scrollPos);

            Win32.LockWindowUpdate((IntPtr)0);

            Invalidate();

            if (mAutoCompleteShown) {
                if (mFilterAutoComplete) {
                    SetAutoCompleteItems();
                    SetAutoCompleteSize();
                    SetAutoCompleteLocation(false);
                }
                SetBestSelectedAutoCompleteItem();
            }

            LineHighlightedWhenLastColoured = LineNumber;

            mParsing = false;
            /*if (HasCompletedTranslation == false) {
                Timer T = new Timer();
                T.Tick += new EventHandler(T_Tick);
                T.Enabled = true;
            }*/

        }

        void T_Tick(object sender, EventArgs e) {
            ((Timer)sender).Enabled = false;
            UpdateColouring(null);
        }


		protected override void OnVScroll(EventArgs e)
		{
			if (mParsing) return;
			base.OnVScroll (e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			HideAutoCompleteForm();
			base.OnMouseDown (e);
		}


        /*public void RelayMessage(ref Message m) {
            WndProc(ref m);
        }*/

		/// <summary>
		/// Taking care of Keyboard events
		/// </summary>
		/// <param name="m"></param>
		/// <remarks>
		/// Since even when overriding the OnKeyDown methoed and not calling the base function 
		/// you don't have full control of the input, I've decided to catch windows messages to handle them.
		/// </remarks>
		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
				case Win32.WM_PAINT:
				{
					//Don't draw the control while parsing to avoid flicker.
					if (mParsing)
					{
						return;
					}
					break;
				}
				case Win32.WM_KEYDOWN:
				{
					if (mAutoCompleteShown)
					{
						switch ((Keys)(int)m.WParam)
						{
							case Keys.Down:
							{
								if (mAutoCompleteForm.Items.Count != 0)
								{
									mAutoCompleteForm.SelectedIndex = (mAutoCompleteForm.SelectedIndex + 1) % mAutoCompleteForm.Items.Count;
								}
								return;
							}
							case Keys.Up:
							{
								if (mAutoCompleteForm.Items.Count != 0)
								{
									if (mAutoCompleteForm.SelectedIndex < 1)
									{
										mAutoCompleteForm.SelectedIndex = mAutoCompleteForm.Items.Count - 1;
									}
									else
									{
										mAutoCompleteForm.SelectedIndex--;
									}
								}
								return;
							}
							case Keys.Enter:
							case Keys.Space:
                            case Keys.Tab:
							{
								AcceptAutoCompleteItem();
								return;
							}
							case Keys.Escape:
							{
								HideAutoCompleteForm();
								return;
							}
								
						}
					}
					else
					{
						if (((Keys)(int)m.WParam == Keys.Space) && 
							((Win32.GetKeyState(Win32.VK_CONTROL) & Win32.KS_KEYDOWN) != 0))
						{
							CompleteWord();
						} 
						else if (((Keys)(int)m.WParam == Keys.Z) && 
							((Win32.GetKeyState(Win32.VK_CONTROL) & Win32.KS_KEYDOWN) != 0))
						{
							Undo();
							return;
						}
						else if (((Keys)(int)m.WParam == Keys.Y) && 
							((Win32.GetKeyState(Win32.VK_CONTROL) & Win32.KS_KEYDOWN) != 0))
						{
							Redo();
							return;
						}
					}
					break;
				}
				case Win32.WM_CHAR:
				{
					switch ((Keys)(int)m.WParam)
					{
						case Keys.Space:
							if ((Win32.GetKeyState(Win32.VK_CONTROL) & Win32.KS_KEYDOWN )!= 0)
							{
								return;
							}
							break;
						case Keys.Enter:
							if (mAutoCompleteShown) return;
							break;
					}
				}
				break;

			}
			base.WndProc (ref m);
		}


		/// <summary>
		/// Hides the AutoComplete form when losing focus on textbox.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLostFocus(EventArgs e)
		{
            //Application.DoEvents();
            //if (!mIgnoreLostFocus && !mAutoCompleteForm.lstCompleteItems.Focused)
            if (!mIgnoreLostFocus)
			{
				HideAutoCompleteForm();
			}
			base.OnLostFocus (e);
		}
        
        
		#endregion

		#region Undo/Redo Code
		public new bool CanUndo 
		{
			get 
			{
				return mUndoList.Count > 0;
			}
		}
		public new bool CanRedo
		{
			get 
			{
				return mRedoStack.Count > 0;
			}
		}

		private void LimitUndo()
		{
			while (mUndoList.Count > mMaxUndoRedoSteps)
			{
				mUndoList.RemoveAt(mMaxUndoRedoSteps);
			}
		}

		public new void Undo()
		{
			if (!CanUndo)
				return;
			mIsUndo = true;
			mRedoStack.Push(new UndoRedoInfo(Text, GetScrollPos(), SelectionStart));
			UndoRedoInfo info = (UndoRedoInfo)mUndoList[0];
			mUndoList.RemoveAt(0);
			Text = info.Text;
			SelectionStart = info.CursorLocation;
			SetScrollPos(info.ScrollPos);
			mLastInfo = info;
			mIsUndo = false;
		}
		public new void Redo()
		{
			if (!CanRedo)
				return;
			mIsUndo = true;
			mUndoList.Insert(0,new UndoRedoInfo(Text, GetScrollPos(), SelectionStart));
			LimitUndo();
			UndoRedoInfo info = (UndoRedoInfo)mRedoStack.Pop();
			Text = info.Text;
			SelectionStart = info.CursorLocation;
			SetScrollPos(info.ScrollPos);
			mIsUndo = false;
		}

		private class UndoRedoInfo
		{
			public UndoRedoInfo(string text, Win32.POINT scrollPos, int cursorLoc)
			{
				Text = text;
				ScrollPos = scrollPos;
				CursorLocation = cursorLoc;
			}
			public readonly Win32.POINT ScrollPos;
			public readonly int CursorLocation;
			public readonly string Text;
		}
		#endregion

		#region AutoComplete functions

		/// <summary>
		/// Entry point to autocomplete mechanism.
		/// Tries to complete the current word. if it fails it shows the AutoComplete form.
		/// </summary>
		private void CompleteWord() {

            #region Broken code
            /*
			int curTokenStartIndex = Text.LastIndexOfAny(mSeperators.GetAsCharArray(), Math.Min(SelectionStart, Text.Length - 1))+1;
			int curTokenEndIndex= Text.IndexOfAny(mSeperators.GetAsCharArray(), SelectionStart);
			if (curTokenEndIndex == -1) 
			{
				curTokenEndIndex = Text.Length;
			}
            
             */
            #endregion


            // Below is Ben's fix
            string TextToSearchIn = Text;
            if (SelectionStart < Text.Length && SelectionStart > 0) {
                TextToSearchIn = Text.Substring(0, SelectionStart);
            }
            int curTokenStartIndex = TextToSearchIn.LastIndexOfAny(mSeperators.GetAsCharArray(), Math.Min(SelectionStart, TextToSearchIn.Length - 1)) + 1;
            int curTokenEndIndex = TextToSearchIn.IndexOfAny(mSeperators.GetAsCharArray(), SelectionStart);
            if (curTokenEndIndex == -1) {
                curTokenEndIndex = TextToSearchIn.Length;
            }
            // End of fix

            string curTokenString = Text.Substring(curTokenStartIndex, Math.Max(curTokenEndIndex - curTokenStartIndex, 0)).ToUpper();

			string token = null;
			foreach (HighlightDescriptor hd in mHighlightDescriptors)
			{
				if (hd.UseForAutoComplete && hd.Token.ToUpper().StartsWith(curTokenString))
				{
					if (token == null)
					{
						token = hd.Token;
					}
					else
					{
						token = null;
						break;
					}
				}
			}
			if (token == null)
			{
				ShowAutoComplete();
			}
			else
			{
				SelectionStart = curTokenStartIndex;
				SelectionLength = curTokenEndIndex - curTokenStartIndex;
				SelectedText = token;
				SelectionStart = SelectionStart + SelectionLength;
				SelectionLength = 0;
			}
		}

		/// <summary>
		/// replace the current word of the cursor with the one from the AutoComplete form and closes it.
		/// </summary>
		/// <returns>If the operation was succesful</returns>
		private bool AcceptAutoCompleteItem()
		{
			
			if (mAutoCompleteForm.SelectedItem == null)
			{
				return false;
            }

            #region Buggy original code [REMOVED FOR GREAT JUSTICE]
            /*

			int curTokenStartIndex = Text.LastIndexOfAny(mSeperators.GetAsCharArray(), Math.Min(SelectionStart, Text.Length - 1)) + 1;
			int curTokenEndIndex= Text.IndexOfAny(mSeperators.GetAsCharArray(), SelectionStart);
			if (curTokenEndIndex == -1) 
			{
				curTokenEndIndex = Text.Length;
			}
 */
            #endregion


            // Below is Ben's fix
            string TextToSearchIn = Text;
            if (SelectionStart < Text.Length && SelectionStart > 0) {
                TextToSearchIn = Text.Substring(0, SelectionStart);
            }
            int curTokenStartIndex = TextToSearchIn.LastIndexOfAny(mSeperators.GetAsCharArray(), Math.Min(SelectionStart, TextToSearchIn.Length - 1)) + 1;
            int curTokenEndIndex = TextToSearchIn.IndexOfAny(mSeperators.GetAsCharArray(), SelectionStart);
            if (curTokenEndIndex == -1) {
                curTokenEndIndex = TextToSearchIn.Length;
            }
            // End of fix

			SelectionStart = Math.Max(curTokenStartIndex, 0);
			SelectionLength = Math.Max(0,curTokenEndIndex - curTokenStartIndex);
			SelectedText = mAutoCompleteForm.SelectedItem;
			SelectionStart = SelectionStart + SelectionLength;
			SelectionLength = 0;
			
			HideAutoCompleteForm();
			return true;
		}



		/// <summary>
		/// Finds the and sets the best matching token as the selected item in the AutoCompleteForm.
		/// </summary>
		private void SetBestSelectedAutoCompleteItem()
		{
			int curTokenStartIndex = Text.LastIndexOfAny(mSeperators.GetAsCharArray(), Math.Min(SelectionStart, Text.Length - 1))+1;
			int curTokenEndIndex= Text.IndexOfAny(mSeperators.GetAsCharArray(), SelectionStart);
			if (curTokenEndIndex == -1) 
			{
				curTokenEndIndex = Text.Length;
			}
			string curTokenString = Text.Substring(curTokenStartIndex, Math.Max(curTokenEndIndex - curTokenStartIndex,0)).ToUpper();
			
			if ((mAutoCompleteForm.SelectedItem != null) && 
				mAutoCompleteForm.SelectedItem.ToUpper().StartsWith(curTokenString))
			{
				return;
			}

			int matchingChars = -1;
			string bestMatchingToken = null;

			foreach (string item in mAutoCompleteForm.Items)
			{
				bool isWholeItemMatching = true;
				for (int i = 0 ; i < Math.Min(item.Length, curTokenString.Length); i++)
				{
					if (char.ToUpper(item[i]) != char.ToUpper(curTokenString[i]))
					{
						isWholeItemMatching = false;
						if (i-1 > matchingChars)
						{
							matchingChars = i;
							bestMatchingToken = item;
							break;
						}
					}
				}
				if (isWholeItemMatching &&
					(Math.Min(item.Length, curTokenString.Length) > matchingChars))
				{
					matchingChars = Math.Min(item.Length, curTokenString.Length);
					bestMatchingToken = item;
				}
			}
			
			if (bestMatchingToken != null)
			{
				mAutoCompleteForm.SelectedIndex = mAutoCompleteForm.Items.IndexOf(bestMatchingToken);
			}


		}

		/// <summary>
		/// Sets the items for the AutoComplete form.
		/// </summary>
		private void SetAutoCompleteItems()
		{
			mAutoCompleteForm.Items.Clear();
			string filterString = "";
			if (mFilterAutoComplete) {
                string TextToSearchIn = Text;
                if (SelectionStart < Text.Length && SelectionStart > 0) {
                    TextToSearchIn = Text.Substring(0, SelectionStart);
                }
                int filterTokenStartIndex = TextToSearchIn.LastIndexOfAny(mSeperators.GetAsCharArray(), Math.Min(SelectionStart, TextToSearchIn.Length - 1)) + 1;
                int filterTokenEndIndex = TextToSearchIn.IndexOfAny(mSeperators.GetAsCharArray(), SelectionStart);
				if (filterTokenEndIndex == -1) {
                    filterTokenEndIndex = TextToSearchIn.Length;
				}
                int stringLength = Math.Max(1, filterTokenEndIndex - filterTokenStartIndex);
                try {
                    filterString = TextToSearchIn.Substring(filterTokenStartIndex, stringLength).ToUpper();
                } catch {
                    Console.WriteLine("Oh shit.");
                }

			}

            Console.WriteLine(filterString);
		
			foreach (HighlightDescriptor hd in mHighlightDescriptors)
			{
				if (hd.Token.ToUpper().StartsWith(filterString) && hd.UseForAutoComplete)
				{
					mAutoCompleteForm.Items.Add(hd.Token);
				}
			}
			mAutoCompleteForm.UpdateView();
		}
		
		/// <summary>
		/// Sets the size. the size is limited by the MaxSize property in the form itself.
		/// </summary>
		private void SetAutoCompleteSize()
		{
			mAutoCompleteForm.Height = Math.Max(Math.Min(
				Math.Max(mAutoCompleteForm.Items.Count, 1) * mAutoCompleteForm.ItemHeight + 4, 
				mAutoCompleteForm.MaximumSize.Height), mAutoCompleteForm.MinimumSize.Height);
		}

		/// <summary>
		/// closes the AutoCompleteForm.
		/// </summary>
		private void HideAutoCompleteForm()
		{
			mAutoCompleteForm.Visible = false;
			mAutoCompleteShown = false;
		}
		

		/// <summary>
		/// Sets the location of the AutoComplete form, maiking sure it's on the screen where the cursor is.
		/// </summary>
		/// <param name="moveHorizontly">determines wheather or not to move the form horizontly.</param>
		private void SetAutoCompleteLocation(bool moveHorizontly)
		{
			Point cursorLocation = GetPositionFromCharIndex(SelectionStart);
			Screen screen = Screen.FromPoint(cursorLocation);
			Point optimalLocation = new Point(PointToScreen(cursorLocation).X-15, (int)(PointToScreen(cursorLocation).Y + Font.Size*2 + 2));
			Rectangle desiredPlace = new Rectangle(optimalLocation , mAutoCompleteForm.Size);
			desiredPlace.Width = 200;
			if (desiredPlace.Left < screen.Bounds.Left) 
			{
				desiredPlace.X = screen.Bounds.Left;
			}
			if (desiredPlace.Right > screen.Bounds.Right)
			{
				desiredPlace.X -= (desiredPlace.Right - screen.Bounds.Right);
			}
			if (desiredPlace.Bottom > screen.Bounds.Bottom)
			{
				desiredPlace.Y = cursorLocation.Y - 2 - desiredPlace.Height;
			}
			if (!moveHorizontly)
			{
				desiredPlace.X = mAutoCompleteForm.Left;
			}

			mAutoCompleteForm.Bounds = desiredPlace;
		}

		/// <summary>
		/// Shows the Autocomplete form.
		/// </summary>
		public void ShowAutoComplete()
		{
			SetAutoCompleteItems();
			SetAutoCompleteSize();
			SetAutoCompleteLocation(true);
			mIgnoreLostFocus = true;
			mAutoCompleteForm.Visible = true;
			SetBestSelectedAutoCompleteItem();
			mAutoCompleteShown = true;
			Focus();
			mIgnoreLostFocus = false;
		}

		#endregion 

		#region Rtf building helper functions

		/// <summary>
		/// Set color and font to default control settings.
		/// </summary>
		/// <param name="sb">the string builder building the RTF</param>
		/// <param name="colors">colors hashtable</param>
		/// <param name="fonts">fonts hashtable</param>
		private void SetDefaultSettings(StringBuilder sb, Hashtable colors, Hashtable fonts)
		{
			SetColor(sb, ForeColor, colors);
			SetFont(sb, Font, fonts);
			SetFontSize(sb, (int)Font.Size);
			EndTags(sb);
		}


		/// <summary>
		/// Set Color and font to a highlight descriptor settings.
		/// </summary>
		/// <param name="sb">the string builder building the RTF</param>
		/// <param name="hd">the HighlightDescriptor with the font and color settings to apply.</param>
		/// <param name="colors">colors hashtable</param>
		/// <param name="fonts">fonts hashtable</param>
		private void SetDescriptorSettings(StringBuilder sb, HighlightDescriptor hd, Hashtable colors, Hashtable fonts)
		{
			SetColor(sb, hd.Color, colors);
			if (hd.Font != null)
			{
				SetFont(sb, hd.Font, fonts);
				SetFontSize(sb, (int)hd.Font.Size);
			}
			EndTags(sb);

		}
		/// <summary>
		/// Sets the color to the specified color
		/// </summary>
		private void SetColor(StringBuilder sb, Color color, Hashtable colors)
		{
			sb.Append(@"\cf").Append(colors[color]);
		}

        private void SetColor(ref string sb, Color color, Hashtable colors) {
            sb += (@"\cf") + (colors[color]) + " ";
        }
		/// <summary>
		/// Sets the backgroung color to the specified color.
		/// </summary>
		private void SetBackColor(StringBuilder sb, Color color, Hashtable colors)
		{
			sb.Append(@"\cb").Append(colors[color]);
		}
		/// <summary>
		/// Sets the font to the specified font.
		/// </summary>
		private void SetFont(StringBuilder sb, Font font, Hashtable fonts)
		{
			if (font == null) return;
			sb.Append(@"\f").Append(fonts[font.Name]);
		}
		/// <summary>
		/// Sets the font size to the specified font size.
		/// </summary>
		private void SetFontSize(StringBuilder sb, int size)
		{
			sb.Append(@"\fs").Append(size*2);
		}
		/// <summary>
		/// Adds a newLine mark to the RTF.
		/// </summary>
		private void AddNewLine(StringBuilder sb)
		{
			sb.Append("\\par\n");
		}

		/// <summary>
		/// Ends a RTF tags section.
		/// </summary>
		private void EndTags(StringBuilder sb)
		{
			sb.Append(' ');
		}

		/// <summary>
		/// Adds a font to the RTF's font table and to the fonts hashtable.
		/// </summary>
		/// <param name="sb">The RTF's string builder</param>
		/// <param name="font">the Font to add</param>
		/// <param name="counter">a counter, containing the amount of fonts in the table</param>
		/// <param name="fonts">an hashtable. the key is the font's name. the value is it's index in the table</param>
		private void AddFontToTable(StringBuilder sb, Font font, ref int counter, Hashtable fonts)
		{
	
			sb.Append(@"\f").Append(counter).Append(@"\fnil\fcharset0").Append(font.Name).Append(";}");
			fonts.Add(font.Name, counter++);
		}

		/// <summary>
		/// Adds a color to the RTF's color table and to the colors hashtable.
		/// </summary>
		/// <param name="sb">The RTF's string builder</param>
		/// <param name="color">the color to add</param>
		/// <param name="counter">a counter, containing the amount of colors in the table</param>
		/// <param name="colors">an hashtable. the key is the color. the value is it's index in the table</param>
		private void AddColorToTable(StringBuilder sb, Color color, ref int counter, Hashtable colors)
		{
	
			sb.Append(@"\red").Append(color.R).Append(@"\green").Append(color.G).Append(@"\blue")
				.Append(color.B).Append(";");
			colors.Add(color, counter++);
		}

		#endregion

		#region Scrollbar positions functions
		/// <summary>
		/// Sends a win32 message to get the scrollbars' position.
		/// </summary>
		/// <returns>a POINT structore containing horizontal and vertical scrollbar position.</returns>
		private unsafe Win32.POINT GetScrollPos()
		{
			Win32.POINT res = new Win32.POINT();
			IntPtr ptr = new IntPtr(&res);
			Win32.SendMessage(Handle, Win32.EM_GETSCROLLPOS, 0, ptr);
			return res;

		}

		/// <summary>
		/// Sends a win32 message to set scrollbars position.
		/// </summary>
		/// <param name="point">a POINT conatining H/Vscrollbar scrollpos.</param>
		private unsafe void SetScrollPos(Win32.POINT point)
		{
			IntPtr ptr = new IntPtr(&point);
			Win32.SendMessage(Handle, Win32.EM_SETSCROLLPOS, 0, ptr);

		}
		#endregion
	}

}