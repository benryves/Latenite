#region Namespaces
using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Runtime;
using System.Runtime.InteropServices;
#endregion

/* ======================================================
 * COLOURFUL EDITOR -- Ben Ryves 2005 -- Part of Latenite
 * ======================================================
 * mailto:benryves@benryves.com | http://www.benryves.com
 * ======================================================
 * I needed a syntax highlighting control for my Z80 IDE.
 * The only ones I could find online were only really
 * demonstration models - good for a 5-line program -
 * far too slow to be used as a real, useful syntax
 * highlighting control. By far the best (read as: only
 * usable) one was by "UrielGuy". It didn't cope too well
 * with massive source files or syntax highlighting rule
 * lists, but in fixing the code and trying to squeeze
 * a little more performance out of it it gave me a lot
 * of good ideas.
 * I rewrote the below code from scratch, copying a few
 * of his Win32 routines (the window locking is a good
 * anti-flicker technique) and the excellent multiple
 * level undo/redo operations he wrote.
 * For average sized source files, this control is fast
 * enough. For large files (>1000 lines/>100KB) it gets
 * a little laggy - that said, so does VC#2005 with only
 * 500 lines :P [OK, VC# does a little more than mine].
 * ======================================================
 * Please bear in mind that I have only been working in
 * C# for a few weeks. If you see me doing anything odd
 * or extremely stupid, let me know.
 * ======================================================
 * Usage:
 * Just treat it like a normal RichTextBox. If you want
 * colouring, you need to add two things:
 * -- Seperators. These are the characters between words
 * such as the space, tab, full-stop, comma or semicolon.
 * -- Syntax elements. These are the tokens that we want
 * to colour - such as "private" or "void" or "xor".
 * You can add either using calls to AddSyntaxElement()
 * or AddSeperator(). Please see the demo for an example!
 * ======================================================
 */

namespace BenRyves {

    /// <summary>
    /// Create an instance of the Colourful Editor source code editing control.
    /// </summary>
    public class ColourfulEditor : RichTextBox {

        #region Property pages
        [Category("Behavior")]
        private bool _EnableHighlighting = true;
        /// <summary>
        /// Enable syntax highlighting
        /// </summary>
        public bool EnableHighlighting {
            get { return _EnableHighlighting; }
            set { _EnableHighlighting = value; ForceRefresh(); }
        }

        
        [Category("Appearance")]
        private Font _IntellisenseFont = new Font(FontFamily.GenericSansSerif, 10);
        public Font IntellisenseFont {
            get { return _IntellisenseFont; }
            set { _IntellisenseFont = value; if (IntellisenseItems != null) IntellisenseItems.Font = _IntellisenseFont; }
        }

        
        [Category("Behavior")]
        private bool _AutoIndent = true;
        public bool AutoIndent {
            get { return _AutoIndent; }
            set { _AutoIndent = value; }
        }

        [Category("Behavior")]
        
        private bool _Locked = false;
        public bool Locked {
            get { return _Locked; }
            set {
                _Locked = value;
                Lock(value);
            }
        }
        delegate void Locker(bool Locked);
        private void Lock(bool Locked) {
            if (this.InvokeRequired) {
                Locker L = new Locker(Lock);
                this.Invoke(L, Locked);
            } else {
                this.ReadOnly = Locked;
            }
        }

        #endregion

        #region Constructor


        private string LastHoverTip = "";
        public Point LastMouseDown = new Point();

        //ToolTip HoverHelp = new ToolTip();
        /// <summary>
        /// Create and set up a new instance of the ColourfulEditor
        /// </summary>
        public ColourfulEditor() {

            Latenite.Program.IntellisenseWindow.FormBorderStyle = FormBorderStyle.None;

            Latenite.Program.IntellisenseWindow.Width = 1;
            Latenite.Program.IntellisenseWindow.Height = 1;

            Latenite.Program.IntellisenseWindow.ShowInTaskbar = false;
            Latenite.Program.IntellisenseWindow.TopMost = true;
            Latenite.Program.IntellisenseWindow.Visible = false;
            Latenite.Program.IntellisenseWindow.Width = 200;
            Latenite.Program.IntellisenseWindow.Controls.Add(IntellisenseItems);     

            base.EnableAutoDragDrop = true;
            base.DragDrop += new DragEventHandler(ColourfulEditor_DragDrop);
            base.MouseMove += new MouseEventHandler(ColourfulEditor_MouseMove);
            base.MouseDown += new MouseEventHandler(ColourfulEditor_MouseDown);
            base.KeyUp += new KeyEventHandler(FixMousePosition);
            base.VScroll += new EventHandler(FixMousePosition);
            base.HScroll += new EventHandler(FixMousePosition);
        }

        void FixMousePosition(object sender, EventArgs e) {
            LastMouseDown = base.GetPositionFromCharIndex(this.SelectionStart);
        }

        void ColourfulEditor_MouseDown(object sender, MouseEventArgs e) {
            LastMouseDown.X = e.X;
            LastMouseDown.Y = e.Y;
        }

        void ColourfulEditor_MouseMove(object sender, MouseEventArgs e) {

            int FindWord = base.GetCharIndexFromPosition(new Point(e.X, e.Y));
            int WordStart = base.Text.LastIndexOfAny((char[])_Seperators.ToArray(typeof(char)), FindWord) + 1;
            int WordEnd = base.Text.IndexOfAny((char[])_Seperators.ToArray(typeof(char)), FindWord);
            if (WordEnd == -1) WordEnd = base.Text.Length;

            if (WordStart == WordEnd) {
                MainTooltip.SetToolTip(this, "");
                return;
            }

            if (WordEnd - WordStart < 0) {
                MainTooltip.SetToolTip(this, "");
                return;
            }
            if (WordStart < 0 || WordEnd < 0 || WordStart > base.Text.Length || WordEnd > base.Text.Length) return;


            string HoverSelected = base.Text.Substring(WordStart, WordEnd - WordStart);

            if (LastHoverTip == HoverSelected) return;
            LastHoverTip = HoverSelected;

            if (Latenite.Program.MainIDE.DebuggerTips) {
                string Tip = Latenite.Program.MainIDE.GetTooltip(this, HoverSelected, GetCurrentLineNumber());
                if (Tip != "") {
                    MainTooltip.SetToolTip(this, Tip);
                    InitialiseTooltip();
                    return;
                }
            }

            foreach (ItemToColour I in SyntaxElements) {
                if (I._Item.ToLower() == HoverSelected.ToLower()) {
                    MainTooltip.SetToolTip(this, I._ToolTipText);
                    InitialiseTooltip();
                    return;
                }
            }

            MainTooltip.SetToolTip(this, "");

        }

        void ColourfulEditor_GotFocus(object sender, EventArgs e) {
            base.Enabled = true;
        }

        void InitialiseTooltip() {
            MainTooltip.ShowAlways = true;
            MainTooltip.ReshowDelay = 20;
            MainTooltip.InitialDelay = 100;
            MainTooltip.AutoPopDelay = 32000;
            MainTooltip.StripAmpersands = false;
            MainTooltip.UseAnimation = true;
            MainTooltip.UseFading = true;
        }

        void ColourfulEditor_LostFocus(object sender, EventArgs e) {
            base.Enabled = false;
        }

        void ColourfulEditor_DragDrop(object sender, DragEventArgs e) {
            e.Data.SetData(e.Data.ToString());
        }


 
        #endregion

        #region Win32

        public const int WM_USER = 0x400;
        public const int WM_PAINT = 0xF;
        public const int WM_KEYDOWN = 0x100;
        public const int WM_KEYUP = 0x101;
        public const int WM_CHAR = 0x102;

        public const int VK_CONTROL = 0x11;
        public const int VK_UP = 0x26;
        public const int VK_DOWN = 0x28;
        public const int VK_SHIFT = 0x10;


        public const short KS_ON = 0x01;
        public const short KS_KEYDOWN = 0x80;

        public const int EM_GETSCROLLPOS = (WM_USER + 221);
        public const int EM_SETSCROLLPOS = (WM_USER + 222);

        [DllImport("user32")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);
        [DllImport("user32")]
        public static extern int PostMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        [DllImport("user32")]
        public static extern short GetKeyState(int nVirtKey);
        [DllImport("user32")]
        public static extern int LockWindowUpdate(IntPtr hwnd);

        #endregion

        #region Intellisense

        #region Intellisense list box

        public ToolTip MainTooltip = new ToolTip();

        private class SyntaxItemsBox : ListView {
            public SyntaxItemsBox() {
                base.Dock = DockStyle.Fill;
                base.View = View.Details;
                base.HeaderStyle = ColumnHeaderStyle.None;
                base.Scrollable = true;
                base.Columns.Add("Items", base.Width - 21);
                base.HideSelection = true;
                base.DoubleBuffered = true;
                base.SmallImageList = Latenite.Program.IntellisenseIcons;
            }

            protected override void OnResize(EventArgs e) {
                this.Columns[0].Width = this.Width - 21;
                base.OnResize(e);
            }

            private int OldHighlight = 0;

            internal int SelectedIndex {
                get {
                    if (this.SelectedIndices.Count == 0) {
                        return 0;
                    }
                    return this.SelectedIndices[0];
                }
                set {
                    if (value < Items.Count) {
                        this.Items[value].Selected = true;
                        if (OldHighlight >= 0 && OldHighlight < Items.Count) this.Items[OldHighlight].Selected = false;
                        Rectangle R = this.Items[value].GetBounds(ItemBoundsPortion.Entire);
                        if (R.Bottom > this.Height) {
                            this.TopItem = this.Items[value - (int)Math.Floor(((double)this.Height - 1) / (double)R.Height) + 1];
                        } else if (R.Top < 0) {
                            this.TopItem = this.Items[value];
                        }

                        if (OldHighlight < this.Items.Count && OldHighlight >= 0) {
                            this.Items[OldHighlight].ForeColor = Color.FromKnownColor(KnownColor.WindowText);
                            this.Items[OldHighlight].BackColor = Color.FromKnownColor(KnownColor.Window);
                        }
                        this.Items[value].BackColor = Color.FromKnownColor(KnownColor.Highlight);
                        this.Items[value].ForeColor = Color.FromKnownColor(KnownColor.HighlightText);
                        OldHighlight = value;

                        
                        

                    }
                }
            }
        }

        #endregion

        public class FlickerFreeForm : Form {
            public FlickerFreeForm() {
                this.DoubleBuffered = true;
            }
        }

        
        SyntaxItemsBox IntellisenseItems = new SyntaxItemsBox();

        private bool ShowingIntellisense = false;

        protected override void OnLostFocus(EventArgs e) {
            if (!ShowingIntellisense) {
                Latenite.Program.IntellisenseWindow.Visible = false;
            }
            base.OnLostFocus(e);
        }

        private const int MaxItemsOnDisplay = 10;

        

        private void AutoComplete(bool ForceSelection) {
            // We need to get the word before our cursor:
            string Token = "";


            int TrackBack = SelectionStart - 1;
            while (TrackBack >= 0 && (!_Seperators.Contains(Text[TrackBack]))) {
                Token = Text[TrackBack] + Token;
                --TrackBack;
            }
            TrackBack++;
            if (TrackBack < 0) TrackBack = 0;
            // We've got some sort of token, can we match it in our autocompletion list?
            //ArrayList PossibleMatches = new ArrayList();


            //IntellisenseItems.Items.Clear();
            if (Latenite.Program.AutocompleteItemsAccelerator[Token.ToLower()] == null) {
                IntellisenseItems = new SyntaxItemsBox();
                IntellisenseItems.Sorting = SortOrder.Ascending;

                foreach (ItemToColour I in SyntaxElements) {
                    if (I._CanAutoComplete && (Token == "" || I._Item.ToLower().StartsWith(Token.ToLower()))) {
                        ListViewItem It = new ListViewItem(I._Item, I._ImageIndex);
                        IntellisenseItems.Items.Add(It);
                    }
                }
                Latenite.Program.IntellisenseWindow.Controls.Add(IntellisenseItems);
                Latenite.Program.AutocompleteItemsAccelerator[Token.ToLower()] = IntellisenseItems;
            } else {
                IntellisenseItems = (SyntaxItemsBox)Latenite.Program.AutocompleteItemsAccelerator[Token.ToLower()];
            }

            for (int i = 0; i < Latenite.Program.IntellisenseWindow.Controls.Count; ++i) {
                if (Latenite.Program.IntellisenseWindow.Controls[i] == IntellisenseItems) {
                    Latenite.Program.IntellisenseWindow.Controls[i].BringToFront();
                    break;
                }
            }

            
            


            // Now, what have we matched?
            if (ForceSelection && IntellisenseItems.Items.Count == 1) {
                // We have found it!

                LockWindowUpdate(Handle);
                    int BeforeEditSelectionStart = SelectionStart;
                    SelectionStart = TrackBack;
                    SelectionLength = BeforeEditSelectionStart - TrackBack;
                    SelectedText = IntellisenseItems.Items[0].Text.Trim();
                    LockWindowUpdate((IntPtr)null);
                
            } else if (IntellisenseItems.Items.Count != 0) {
                // Pop up the listing.
                Latenite.Program.IntellisenseWindow.Height = Math.Min(MaxItemsOnDisplay, IntellisenseItems.Items.Count) * IntellisenseItems.GetItemRect(0).Height + 4;
                IntellisenseItems.Scrollable = (IntellisenseItems.Items.Count > MaxItemsOnDisplay);
                IntellisenseItems.Columns[0].Width = IntellisenseItems.Width - ((IntellisenseItems.Items.Count > MaxItemsOnDisplay) ? 21 : 0);

                IntellisenseItems.SelectedIndex = 0;
                if (!Latenite.Program.IntellisenseWindow.Visible) {
                    ShowingIntellisense = true;
                    RepositionIntellisense();
                    Latenite.Program.IntellisenseWindow.Visible = true;
                    Focus();
                    ShowingIntellisense = false;
                }
                RepositionIntellisense();


            }
        }

        private void RepositionIntellisense() {
            if (!Latenite.Program.IntellisenseWindow.Visible) return;

            Point P = GetPositionFromCharIndex(SelectionStart);
            Screen S = Screen.FromPoint(P);
            Point L = new Point(PointToScreen(P).X - 15, (int)(PointToScreen(P).Y + Font.Height));

            Latenite.Program.IntellisenseWindow.Left = Math.Max(0, Math.Min(L.X, S.Bounds.Width - Latenite.Program.IntellisenseWindow.Width));
            Latenite.Program.IntellisenseWindow.Top = L.Y;
        }

        #endregion

        #region Syntax highlighting

        /// <summary>
        /// Hashtable to speed up token matching.
        /// </summary>
        private Hashtable SyntaxLookup = new Hashtable();

        /// <summary>
        /// Rebuild the syntax hashtable for fast token matching
        /// </summary>
        private void RegenerateSyntaxHashTable() {
            SyntaxLookup.Clear();
            foreach (ItemToColour I in SyntaxElements) {
                string Key = I._Item.ToLower();
                if (SyntaxLookup[Key] == null) {
                    SyntaxLookup.Add(Key, I);
                }
            }
        }

        public void UseExternalProperties(Hashtable Colours, Hashtable Syntax, ArrayList Seps, ArrayList Elements) {
            ColourTable = Colours;
            SyntaxLookup = Syntax;
            _Seperators = Seps;
            SyntaxElements = Elements;
            RegenerateRTFHeader();
        }

        /// <summary>
        /// A class to ease construction/storage of items used when syntax highlighting.
        /// </summary>
        public class ItemToColour {
            public string _Item;
            public Color _Colour;
            public ItemType _Type;
            public bool _CanAutoComplete;
            public int _ImageIndex;
            public string _ToolTipText;
            public ItemToColour(string Item, Color Colour, ItemType Type, bool CanAutoComplete, int ImageIndex, string ToolTipText) {
                _Item = Item;
                _Colour = Colour;
                _Type = Type;
                _CanAutoComplete = CanAutoComplete;
                _ImageIndex = ImageIndex;
                _ToolTipText = ToolTipText;
            }
        }

        /// <summary>
        /// The highlight type:
        /// Word - Only highlight the word (bounded by seperators).
        /// ToEndOfLine - Keep colour until you hit the end of the line ('\n').
        /// String - Colour the elements between this character and the next instance of it, excluding any backslash-escaped ones.
        /// </summary>
        public enum ItemType { Word, ToEndOfLine, String, ToStartOfWord }

        /// <summary>
        /// Storage area for the seperators between elements.
        /// </summary>
        ArrayList _Seperators = new ArrayList();

        /// <summary>
        /// Storage area for elements used in syntax highlighting.
        /// </summary>
        private ArrayList SyntaxElements = new ArrayList();

        /// <summary>
        /// Add an element to use for syntax highlighting.
        /// </summary>
        /// <param name="Item">The token to match (eg "private")</param>
        /// <param name="Colour">The colour you wish to set the </param>
        /// <param name="Type"></param>
        /// <param name="CanAutoComplete"></param>
        public void AddSyntaxElement(string Item, Color Colour, ItemType Type, bool CanAutoComplete, int ImageIndex, string ToolTipText) {
            SyntaxElements.Add(new ItemToColour(Item, Colour, Type, CanAutoComplete, ImageIndex, ToolTipText));
            RegenerateSyntaxHashTable();
        }
        /// <summary>
        /// Add a seperator character to the control.
        /// </summary>
        /// <param name="Seperator">Seperator character (eg '\n')</param>
        public void AddSeperator(char Seperator) {
            _Seperators.Add(Seperator);
        }

        public void ClearSeperators() {
            _Seperators.Clear();
        }
        public void ClearSyntaxElements() {
            SyntaxElements.Clear();
            ColourTable.Clear();
            RTFHeader = ""; // Force an update :)
        }

        /// <summary>
        /// Binds a colour (eg Color.Red) to an index for the RTF stream.
        /// </summary>
        Hashtable ColourTable = new Hashtable();


        /// <summary>
        /// Escapes a line of plain text into text suitable to insert into an RTF stream.
        /// </summary>
        /// <param name="PlainText">Text to escape.</param>
        /// <returns>RTF-escaped text.</returns>
        private string EscapeRTF(string PlainText) {
            StringBuilder EscapingString = new StringBuilder(PlainText.Length * 4);
            for (int i = 0; i < PlainText.Length; ++i) {
                char c = PlainText[i];
                if (c == '\n') {
                    EscapingString.Append(@"\par ");
                } else  if (c >= 0 && c < 128 && c != '{' && c != '}' && c != '\\') {
                    EscapingString.Append(c);
                } else {
                    EscapingString.Append("\\u" + (int)(PlainText[i]) + "?");
                }
            }
           return EscapingString.ToString();
            //return EscapingString.ToString().Replace(@"\", @"\\").Replace("{", @"\{").Replace("}", @"\}").Replace("\n", @"\par ");
        }
        /// <summary>
        /// Escapes a single character into text suitable to insert into an RTF stream.
        /// </summary>
        /// <param name="PlainChar">The character to escape.</param>
        /// <returns>RTF-escaped text.</returns>
        private string EscapeRTF(char PlainChar) {
            return EscapeRTF(PlainChar.ToString());
        }


        /// <summary>
        /// Add a colouring control code sequence to an RTF stream.
        /// </summary>
        /// <param name="RTFToSetIn">Reference to RTF stream.</param>
        /// <param name="Colour">The colour to switch to (must be declared inside the header!)</param>
        private void SelectColouring(ref string RTFToSetIn, Color Colour) {
            if (ColourTable[Colour] == null) return;
            RTFToSetIn += @"\cf" + ((int)(ColourTable[Colour]) + 1) + " ";
        }

        /// <summary>
        /// Apply the syntax highlighting rules to a string.
        /// </summary>
        /// <param name="PlainText">The text to syntax highlight.</param>
        /// <returns>An RTF-formatted string with the required syntax highlighting applied.</returns>
        private string HighlightRTF(string PlainText) {

            if (!_EnableHighlighting || SyntaxElements.Count == 0 || _Seperators.Count == 0) {
                string NonHighlighted = "";
                SelectColouring(ref NonHighlighted, ForeColor);
                NonHighlighted += EscapeRTF(PlainText);
                return NonHighlighted;
            }

            PlainText = "\r" + PlainText.Replace("\r", "") + "\r";

            StringBuilder FinalRTF = new StringBuilder(PlainText.Length * 4);

            string ReturnedRTF = @"\highlight0 \cf0 ";

            char[] Seps = (char[])_Seperators.ToArray(typeof(char));

            int CurrentPos = PlainText.IndexOfAny(Seps);
            int LastPos = 0;

            bool ToEOL = false;
            bool ToEOS = false;
            char EOSMarker = ' ';

            bool AmInLabel = true;

            Color Label = Latenite.Properties.Settings.Default.Syntax_Label;

            int LineCounter = GetLineFromCharIndex(SelectionStart);
            bool WasHighlighted = CheckIfLineIsHighlighted(LineCounter, false) && !AmUnbreakpointing;

            if (!WasHighlighted) {

                while (CurrentPos != -1) {

                    bool JustPerformedRearrangement = false;

                    // We have hit a seperator;
                    string CurrentToken = PlainText.Substring(LastPos, CurrentPos - LastPos);
                    if (CurrentToken.Length > 0) {
                        char CurrentSep = CurrentToken[0];

                        if (CurrentSep == '\n') ++LineCounter;


                        if (CurrentSep == '\n' || CurrentSep == '\r') {
                            AmInLabel = true;
                            SelectColouring(ref ReturnedRTF, Label);
                        }

                        if (ToEOL && (CurrentSep == '\n' || CurrentSep == '\r')) {
                            ToEOL = false;
                        }

                        if ((ToEOS && (CurrentSep == EOSMarker || CurrentSep == '\n' || CurrentSep == '\r')) && (LastPos > 0 && PlainText[LastPos - 1] != '\\')) {
                            ToEOS = false;
                            JustPerformedRearrangement = true;
                        }

                        if (!JustPerformedRearrangement && !(ToEOL || ToEOS)) {

                            if (CurrentSep != '+' && CurrentSep != '-' && CurrentSep != '\n' && CurrentSep != '\r') AmInLabel = false;

                            // Now we set the colour;

                            string JustToken = ((string)((CurrentToken.Length == 1) ? CurrentToken : CurrentToken.Substring(1)));

                            ItemToColour ST = (ItemToColour)SyntaxLookup[CurrentSep.ToString()];
                            if (ST == null) {
                                ST = (ItemToColour)SyntaxLookup[JustToken];
                                if (ST == null) {
                                    ST = (ItemToColour)SyntaxLookup[JustToken.ToLower()];
                                }
                            }

                            if (ST != null) {
                                if (ST._Type == ItemType.Word && !AmInLabel) {
                                    // Go to default colour:
                                    SelectColouring(ref ReturnedRTF, ForeColor);
                                    // Chuck out the seperator:
                                    ReturnedRTF += EscapeRTF(CurrentSep);
                                    // Remove it from the actual string:
                                    CurrentToken = CurrentToken.Substring(1);
                                }

                                // Set the colouring
                                SelectColouring(ref ReturnedRTF, ST._Colour);

                                if (ST._Type == ItemType.ToEndOfLine) {
                                    ToEOL = true;
                                } else if (ST._Type == ItemType.String) {
                                    ToEOS = true;
                                    EOSMarker = CurrentSep;
                                } else if (ST._Type == ItemType.ToStartOfWord) {
                                    // Track backwards to last \par:

                                }

                            } else {
                                if (!AmInLabel) SelectColouring(ref ReturnedRTF, ForeColor);
                            }

                            if (!ToEOL && !ToEOS) {
                                ItemToColour TE = (ItemToColour)SyntaxLookup[CurrentToken[CurrentToken.Length - 1].ToString()];
                                if (TE != null && TE._Type == ItemType.ToStartOfWord) {
                                    SelectColouring(ref ReturnedRTF, TE._Colour);
                                    ReturnedRTF += EscapeRTF(CurrentToken);
                                    CurrentToken = "";
                                    if (!AmInLabel) SelectColouring(ref ReturnedRTF, ForeColor);
                                }
                            }


                        }
                        ReturnedRTF += EscapeRTF(CurrentToken);
                        if (CurrentSep == ' ' || CurrentSep == '\t') {
                            AmInLabel = false;
                        }
                    }

                    LastPos = CurrentPos;
                    CurrentPos = PlainText.IndexOfAny(Seps, CurrentPos + 1);
                    // Flush the current string if need be.
                    if (ReturnedRTF.Length > 512) {
                        FinalRTF.Append(ReturnedRTF);
                        ReturnedRTF = "";
                    }
                }
                return FinalRTF.Append(ReturnedRTF).ToString();
            } else {
                string HighlightedLine = "";
                SelectColouring(ref HighlightedLine, Latenite.Properties.Settings.Default.Debug_Breakpoint_Fore);
                object Colour = ColourTable[Latenite.Properties.Settings.Default.Debug_Breakpoint_Back];
                int C = (int)Colour; ++C;
                return HighlightedLine + @"\highlight" + C + " " + EscapeRTF(SelectedText);
            }
        }

        /// <summary>
        /// A string that contains the RTF header, including font/colour tables.
        /// You can force it to update by calling RegenerateRTFHeader()
        /// </summary>
        private string RTFHeader = "";


        /// <summary>
        /// Build up the RTF header string.
        /// </summary>
        private void RegenerateRTFHeader() {

            RTFHeader = @"{\rtf1\fbidis\ansi\ansicpg1255\deff0\deflang1037{\fonttbl{";

            // Now, the font table (we'll only allow the default font for the moment).
            RTFHeader += @"\f0\fnil\fcharset0" + Font.Name + ";}";

            RTFHeader += "}\n";    // End of font table:

            // Next, the colour table:

            ColourTable.Clear();
            int ColoursCounter = 0;

            RTFHeader += @"{\colortbl ;";

            AddColour(ForeColor, ref ColoursCounter);
            AddColour(BackColor, ref ColoursCounter);
            AddColour(Latenite.Properties.Settings.Default.Debug_Breakpoint_Back, ref ColoursCounter);
            AddColour(Latenite.Properties.Settings.Default.Debug_Breakpoint_Fore, ref ColoursCounter);
            AddColour(Latenite.Properties.Settings.Default.Debug_Current_Back, ref ColoursCounter);
            AddColour(Latenite.Properties.Settings.Default.Debug_Current_Fore, ref ColoursCounter);
            foreach (ItemToColour I in SyntaxElements) {
                AddColour(I._Colour, ref ColoursCounter);
            }
            RTFHeader += "}\n";
            RTFHeader += @"\viewkind4\uc1\pard\ltrpar\f0\fs" + Math.Round(Font.Size * 2) + " ";
        }

        /// <summary>
        /// Add a colour to the colour table.
        /// </summary>
        /// <param name="ColourToAdd"></param>
        /// <param name="ColourIndexCounter"></param>
        private void AddColour(Color ColourToAdd, ref int ColourIndexCounter) {
            if (ColourTable[ColourToAdd] == null) {
                ColourTable.Add(ColourToAdd, ColourIndexCounter++);
                RTFHeader += @"\red" + ColourToAdd.R + @"\green" + ColourToAdd.G + @"\blue" + ColourToAdd.B + ";";
            }
        }

        /// <summary>
        /// Status boolean - this registers whether the control is currently highlighting or not.
        /// </summary>
        private bool IsHighlighting = false;



        /// <summary>
        /// Perform a complete rehighlight.
        /// </summary>
        public void ForceRefresh() {
            RegenerateRTFHeader();
            this.Rtf = RTFHeader + HighlightRTF(Text);
        }


        /// <summary>
        /// What the SelectionStart was last frame.
        /// </summary>
        private int LastFrameSelectionStart = 0;
        /// <summary>
        /// What the SelectionStart is THIS frame.
        /// </summary>
        private int ThisFrameSelectionStart = 0;

        /// <summary>
        /// Override to save the position of the cursor into the LastFrameSelectionStart variable.
        /// </summary>
        protected override void OnSelectionChanged(EventArgs e) {
            if (IsHighlighting || _IsUndo) return;
            LastFrameSelectionStart = ThisFrameSelectionStart;
            ThisFrameSelectionStart = SelectionStart;
            base.OnSelectionChanged(e);
            if (Latenite.Program.IntellisenseWindow.Visible) RepositionIntellisense();
        }

        private int LastEditSelectionStart = 0;

        

        /// <summary>
        /// Override that 'intelligently' performs the syntax highlighting.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(EventArgs e) {
            if (IsDisposed) return;

            if (e != null) {

                // Give the application a little time to *breathe*
                string TextBefore = "";
                while (Text != TextBefore) {
                    TextBefore = Text;
                    Application.DoEvents();
                }


                // Update the little autocomplete box:
                if (Latenite.Program.IntellisenseWindow.Visible) AutoComplete(false);
            }

            // If we're currently highlighting, jump out.
            if (IsHighlighting || _IsUndo) return;
            IsHighlighting = true;

            if (Lines.Length != 0) {

                // Save away our current selection properties:
                int OldSelectionStart = SelectionStart;
                int OldSelectionLength = SelectionLength;

                // Calculate the new selection positions:
                int NewSelectionStart = Math.Min(LastFrameSelectionStart, SelectionStart);
                int NewSelectionEnd = Math.Max(LastFrameSelectionStart, SelectionStart);

                while (NewSelectionStart > 0 && Text[NewSelectionStart - 1] != '\n') --NewSelectionStart;
                while (NewSelectionEnd < Text.Length && Text[NewSelectionEnd] != '\n') ++NewSelectionEnd;



                // Lock window update
                LockWindowUpdate(Handle);

                // Save scrollbar positions:
                Point P = base.AutoScrollOffset;


                SelectionStart = NewSelectionStart;
                SelectionLength = NewSelectionEnd - NewSelectionStart;

                // At this point we need to perform the actual conversion!
                if (RTFHeader == "") RegenerateRTFHeader();
                string NewRTF = RTFHeader + HighlightRTF(SelectedText);
                SelectedRtf = NewRTF;

                // Set the old selection settings back:
                SelectionStart = OldSelectionStart;
                SelectionLength = OldSelectionLength;

                // Restore scrollbar positions:
                //SetScrollPos(P);
                base.AutoScrollOffset = P;
                // Unlock window update
                LockWindowUpdate((IntPtr)null);
                Invalidate(true);

            }

            IsHighlighting = false;
            LastEditSelectionStart = SelectionStart;


            // Update undo/redo stacks

            if (!_IsUndo) {
                _RedoStack.Clear();
                _UndoList.Insert(0, _LastInfo);
                this.LimitUndo();
                _LastInfo = new UndoRedoInfo(Rtf, GetScrollPos(), SelectionStart);
            }
            
            // Pass control back to the RichTextBox.
            base.OnTextChanged(e);
        }

        //bool LockKeys = false;

        #endregion

        #region Window message handler

        protected override void WndProc(ref Message m) {
            if (m.Msg == WM_KEYDOWN) {
                if (Latenite.Program.IntellisenseWindow.Visible) {
                    switch ((Keys)m.WParam) {
                        case Keys.Down:
                            if (IntellisenseItems.Items.Count != 0) {
                                IntellisenseItems.SelectedIndex = (IntellisenseItems.SelectedIndex + 1) % IntellisenseItems.Items.Count;
                            }
                            return;
                        case Keys.Up:
                            if (IntellisenseItems.Items.Count != 0) {
                                IntellisenseItems.SelectedIndex = (IntellisenseItems.SelectedIndex + IntellisenseItems.Items.Count - 1) % IntellisenseItems.Items.Count;
                            }
                            return;
                        case Keys.Left:
                        case Keys.Right:
                            return;
                        case Keys.Escape:
                            Latenite.Program.IntellisenseWindow.Visible = false;
                            return;
                        case Keys.Enter:
                        case Keys.Space:
                        case Keys.Tab:
                            Latenite.Program.IntellisenseWindow.Visible = false;
                            // Insert the currently highlighted word:
                            if (IntellisenseItems.Items.Count > 0) {
                                LockWindowUpdate(Handle);
                                JustInsertedAutocomplete = true;
                                int TrackBack = SelectionStart - 1;
                                while (TrackBack >= 0 && (!_Seperators.Contains(Text[TrackBack]))) {
                                    --TrackBack;
                                }
                                TrackBack++;

                                int OldSelectionStart = SelectionStart;


                                SelectionStart = TrackBack;
                                SelectionLength = OldSelectionStart - TrackBack;
                                SelectedText = IntellisenseItems.Items[IntellisenseItems.SelectedIndex].Text;
                                LockWindowUpdate((IntPtr)null);

                            }
                            return;
                    }
                } else if ((GetKeyState(VK_CONTROL) & KS_KEYDOWN) != 0) {
                    // Ctrl+Something is being pressed:
                    switch ((Keys)m.WParam) {
                        case Keys.Z:
                            Undo(); return;
                        case Keys.Y:
                            Redo(); return;
                        case Keys.Space:
                            if (Latenite.Program.IntellisenseWindow.Visible) break;
                            // Lock the window to stop my jiggery-pokey from being visible

                            LockWindowUpdate(Handle);
                            Point P = GetScrollPos();
                            string OldRTF = Rtf;            // Save RTF
                            int OldPos = SelectionStart;    // and positions...
                            int OldLen = SelectionLength;
                            _IsUndo = true;
                            IsHighlighting = true;
                            base.WndProc(ref m);            // Allow the control to add the space
                            Application.DoEvents();
                            Rtf = OldRTF;// Jump backwards
                            Application.DoEvents();
                            SelectionStart = OldPos;
                            SelectionLength = OldLen;
                            _IsUndo = false;
                            IsHighlighting = false;
                            LockWindowUpdate((IntPtr)null);    // Unlock all.
                            // Now we have swallowed the SPACE, run an autocomplete.
                            SetScrollPos(P);
                            AutoComplete(true);
                            return;
                        case Keys.Left:
                        case Keys.Right:
                        case Keys.Home:
                        case Keys.End:
                            break;
                        default:
                            return;
                    }
                } else if ((Keys)m.WParam == Keys.Tab && SelectionLength != 0) {
                    // Tabbing in a block
                    LockWindowUpdate(Handle);
                    Point P = GetScrollPos();
                    string OldRTF = Rtf;            // Save RTF
                    int OldPos = SelectionStart;    // and positions...
                    int OldLen = SelectionLength;
                    _IsUndo = true;
                    IsHighlighting = true;
                    base.WndProc(ref m);
                    base.ReadOnly = true;
                    Application.DoEvents();
                    base.ReadOnly = false;
                    Rtf = OldRTF;
                    // Now we need to go through...
                    SelectionStart = OldPos;
                    SelectionStart = GetFirstCharIndexOfCurrentLine();
                    SelectionLength = ((string)(Text + "\n")).IndexOf('\n', SelectionStart + Math.Max(0, OldLen - 1)) - SelectionStart;

                    string OldText = SelectedText;

                    StringBuilder NewText = new StringBuilder(OldText.Length*2);
                    string[] ToEntab = OldText.Split('\n');
                    bool First = true;
                    bool Shifted = (GetKeyState(VK_SHIFT) & KS_KEYDOWN) != 0;
                    foreach (string S in ToEntab) {
                        if (First) {
                            First = false;
                        } else {
                            NewText.Append("\n");
                        }
                        if (Shifted) {
                            if (S.Length != 0 && S[0] == '\t') {
                                NewText.Append(S.Substring(1));
                            } else {
                                NewText.Append(S);
                            }
                        } else {
                            NewText.Append("\t" + S);
                        }
                    }
                    if (RTFHeader == "") RegenerateRTFHeader();
                    SelectedRtf = RTFHeader + HighlightRTF(NewText.ToString());
                    SelectionStart = OldPos;
                    SelectionStart = GetFirstCharIndexOfCurrentLine();
                    SelectionLength = NewText.Length;
                    _IsUndo = false;
                    IsHighlighting = false;
                    LockWindowUpdate((IntPtr)null);    // Unlock all.
                    SetScrollPos(P);
                    OnTextChanged(null);
                    return;

                }
            }
            base.WndProc(ref m);
        }

        #endregion

        #region Multiple level undo and redo

        private ArrayList _UndoList = new ArrayList();
        private Stack _RedoStack = new Stack();
        private bool _IsUndo = false;
        private UndoRedoInfo _LastInfo = new UndoRedoInfo("", new Point(), 0);
        private int _MaxUndoRedoSteps = 50;


        public new bool CanUndo {
            get { return _UndoList.Count > 0; }
        }
        public new bool CanRedo {
            get { return _RedoStack.Count > 0; }
        }

        private void LimitUndo() {
            while (_UndoList.Count > _MaxUndoRedoSteps) {
                _UndoList.RemoveAt(_MaxUndoRedoSteps);
            }
        }

        /// <summary>
        /// Undo the last operation.
        /// </summary>
        public new void Undo() {
            if (!CanUndo) return;
            Point P = GetScrollPos();
            LockWindowUpdate(Handle);
            _IsUndo = true;
            _RedoStack.Push(new UndoRedoInfo(Rtf, base.AutoScrollOffset, SelectionStart));
            UndoRedoInfo info = (UndoRedoInfo)_UndoList[0];
            _UndoList.RemoveAt(0);
            Rtf = info.Rtf;
            SelectionStart = info.CursorLocation;
            //base.AutoScrollOffset = info.ScrollPos;
            _LastInfo = info;
            _IsUndo = false;
            //base.AutoScrollOffset = P;
            SetScrollPos(P);
            LockWindowUpdate((IntPtr)null);
            Invalidate();
            SafeScrollToCaret();
            base.OnTextChanged(null);
        }

        /// <summary>
        /// Redo the last undone operation.
        /// </summary>
        public new void Redo() {
            if (!CanRedo) return;
            Point P = base.AutoScrollOffset;
            LockWindowUpdate(Handle);
            _IsUndo = true;
            _UndoList.Insert(0, new UndoRedoInfo(Rtf, base.AutoScrollOffset, SelectionStart));
            LimitUndo();
            UndoRedoInfo info = (UndoRedoInfo)_RedoStack.Pop();
            Rtf = info.Rtf;
            SelectionStart = info.CursorLocation;
            _IsUndo = false;
            base.AutoScrollOffset = P;
            LockWindowUpdate((IntPtr)null);
            Invalidate();
            SafeScrollToCaret();
            base.OnTextChanged(null);
            
        }


        /// <summary>
        /// Class used as storage for old text/position information.
        /// </summary>
        private class UndoRedoInfo {
            public readonly Point ScrollPos;
            public readonly int CursorLocation;
            public readonly string Rtf;
            public UndoRedoInfo(string rtf, Point scrollPos, int cursorLoc) {
                Rtf = rtf;
                ScrollPos = scrollPos;
                CursorLocation = cursorLoc;
            }
        }

        #endregion

        #region Helpful routines

        /// <summary>
        /// Highlight a line on this control.
        /// </summary>
        /// <param name="LineToHighlight">Which line you would like to be highlighted.</param>
        public void HighlightLine(int LineToHighlight) {
            if (LineToHighlight >= 0 && LineToHighlight < Lines.Length) {
                LockWindowUpdate(Handle);
                SelectionStart = GetFirstCharIndexFromLine(LineToHighlight);
                SelectionLength = Lines[LineToHighlight].Length;
                ScrollToCaret();
                LockWindowUpdate((IntPtr)null);
                Invalidate();
            }
        }


        /// <summary>
        /// Get the current line number.
        /// </summary>
        /// <returns>The current line number.</returns>
        public int GetCurrentLineNumber() {
            return this.GetLineFromCharIndex(SelectionStart);
            //return (this.Text == "") ? 0 : ((Text + " \r\r").Remove(this.SelectionStart).Split(new char[] { '\n' }).Length - 1);
        }

        public void WaitForStaticText() {
            string T = "";
            while (T != Text) {
                T = Text;
                Application.DoEvents();
            }
        }

        public void SafeScrollToCaret() {
            int A = base.GetCharIndexFromPosition(new Point(0, 0));
            int B = base.GetCharIndexFromPosition(new Point(base.Width, base.Height));
            if (SelectionStart < A || SelectionStart > B) base.ScrollToCaret();
        }
        public void Delete() {
            if (!_Locked) {
                int SelStart = SelectionStart;
                int SelLength = SelectionLength;
                if (SelLength > 0 && SelStart + SelLength <= Text.Length) {
                    Text = Text.Remove(SelStart, SelLength);
                    SelectionLength = 0;
                    SelectionStart = SelStart;
                    ScrollToCaret();
                }
            }
        }

        #endregion

        #region AutoIndent

        private bool JustInsertedAutocomplete = false;
        protected override void OnKeyPress(KeyPressEventArgs e) {
            if (_AutoIndent && (Keys)e.KeyChar == Keys.Enter && !JustInsertedAutocomplete) {
                // Get the line above:
                int CurrentLine = GetLineFromCharIndex(GetFirstCharIndexOfCurrentLine());
                string LineAbove = Lines[Math.Max(0, CurrentLine - 1)];
                if (LineAbove != "") {
                    string Indent = "";
                    foreach (char C in LineAbove) {
                        if (C != ' ' && C != '\t') break;
                        Indent += C;
                    }
                    //if (Indent == "") Indent = "\t";
                    OnTextChanged(null);
                    SelectedText = Indent;
                }
                
            }
            JustInsertedAutocomplete = false;
            //e.Handled |= this.LockKeys;
            base.OnKeyPress(e);
        }

        

        #endregion

        #region Scrollbar position functions
        /// <summary>
        /// Sends a win32 message to get the scrollbars' position.
        /// </summary>
        /// <returns>a POINT structore containing horizontal and vertical scrollbar position.</returns>
        private unsafe Point GetScrollPos() {
            Point res = new Point();
            IntPtr ptr = new IntPtr(&res);
            SendMessage(Handle, EM_GETSCROLLPOS, 0, ptr);
            return res;
        }

        /// <summary>
        /// Sends a win32 message to set scrollbars position.
        /// </summary>
        /// <param name="point">a POINT conatining H/Vscrollbar scrollpos.</param>
        private unsafe void SetScrollPos(Point point) {
            IntPtr ptr = new IntPtr(&point);
            SendMessage(Handle, EM_SETSCROLLPOS, 0, ptr);

        }
        #endregion

        #region Breakpoints

        public bool CheckIfLineIsHighlighted(int LineNumber) {
            return CheckIfLineIsHighlighted(LineNumber, true);
        }
        
        private bool CheckIfLineIsHighlighted(int LineNumber, bool Lock) {
            Point P = new Point();
            if (Lock) {
                P = base.AutoScrollOffset;
                LockWindowUpdate(Handle);
            }

            int OldSelStart = SelectionStart;
            int OldSelLength = SelectionLength;

            SelectionStart = GetFirstCharIndexFromLine(LineNumber);
            int GetEnd = Text.IndexOf('\n', SelectionStart);
            SelectionLength = GetEnd == -1 ? Text.Length - SelectionLength : GetEnd - SelectionStart;

            bool Ret = SelectedRtf.IndexOf(@"\highlight") != -1;

            SelectionStart = OldSelStart;
            SelectionLength = OldSelLength;

            if (Lock) {
                base.AutoScrollOffset = P;
                LockWindowUpdate((IntPtr)null);
            }
            return Ret;
        }

        public void HighlightLine(int LineNumber, Color BackColour, Color ForeColour) {
            IsHighlighting = true;
            Point P = base.AutoScrollOffset;
            LockWindowUpdate(Handle);

            int OldSelStart = SelectionStart;
            int OldSelLength = SelectionLength;

            SelectionStart = GetFirstCharIndexFromLine(LineNumber);
            int GetEnd = Text.IndexOf('\n', SelectionStart);
            SelectionLength = GetEnd == -1 ? Text.Length - SelectionLength : GetEnd - SelectionStart;

            if (RTFHeader == "") RegenerateRTFHeader();

            object Colour = ColourTable[BackColour];
            if (Colour == null || BackColor == Color.Transparent && ForeColor == Color.Transparent) {
                AmUnbreakpointing = true;
                SelectedRtf = RTFHeader + HighlightRTF(SelectedText);
                AmUnbreakpointing = false;
            } else {
                string HighlightedLine = RTFHeader; 
                SelectColouring(ref HighlightedLine, ForeColour);
                int C = (int)Colour; ++C;
                HighlightedLine += @"\highlight" + C;
                SelectedRtf = HighlightedLine + " " + EscapeRTF(SelectedText) + @"\highlight0";
            }

            SelectionStart = OldSelStart;
            SelectionLength = OldSelLength;

            base.AutoScrollOffset = P;
            LockWindowUpdate((IntPtr)null);
            Invalidate();
            Application.DoEvents();
            IsHighlighting = false;
        }

        public void UnHighlightLine(int LineNumber) {
            HighlightLine(LineNumber, Color.Transparent, Color.Transparent);
        }

        bool AmUnbreakpointing = false;

        #endregion

    }
}