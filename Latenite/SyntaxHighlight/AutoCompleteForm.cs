using System;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;

namespace UrielGuy.SyntaxHighlightingTextBox
{
	/// <summary>
	/// Summary description for AutoCompleteForm.
	/// </summary>
	public class AutoCompleteForm : System.Windows.Forms.Form
	{
        private StringCollection mItems = new StringCollection();
        public ListView lstCompleteItems;
		private System.Windows.Forms.ColumnHeader columnHeader1;
        private SyntaxHighlightingTextBox OwnerEditor;

		public StringCollection Items 
		{
			get 
			{
				return mItems;
			}
		}

		internal int ItemHeight 
		{
			get  
			{
				return 18;
			}
		}

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AutoCompleteForm(SyntaxHighlightingTextBox owner)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            OwnerEditor = owner;
		}

		public string SelectedItem 
		{
			get
			{
				if (lstCompleteItems.SelectedItems.Count == 0) return null;
				return (string)lstCompleteItems.SelectedItems[0].Text;
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

        private int OldHighlight = 0;

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.lstCompleteItems = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // lstCompleteItems
            // 
            this.lstCompleteItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lstCompleteItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstCompleteItems.FullRowSelect = true;
            this.lstCompleteItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstCompleteItems.LabelWrap = false;
            this.lstCompleteItems.Location = new System.Drawing.Point(0, 0);
            this.lstCompleteItems.MultiSelect = false;
            this.lstCompleteItems.Name = "lstCompleteItems";
            this.lstCompleteItems.Size = new System.Drawing.Size(200, 144);
            this.lstCompleteItems.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstCompleteItems.TabIndex = 1;
            this.lstCompleteItems.UseCompatibleStateImageBehavior = false;
            this.lstCompleteItems.View = System.Windows.Forms.View.Details;
            this.lstCompleteItems.SelectedIndexChanged += new System.EventHandler(this.lstCompleteItems_SelectedIndexChanged_1);
            this.lstCompleteItems.Click += new System.EventHandler(this.lstCompleteItems_Click);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 130;
            // 
            // AutoCompleteForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(200, 144);
            this.ControlBox = false;
            this.Controls.Add(this.lstCompleteItems);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(200, 144);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(0, 36);
            this.Name = "AutoCompleteForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "AutoCompleteForm";
            this.TopMost = true;
            this.Resize += new System.EventHandler(this.AutoCompleteForm_Resize);
            this.VisibleChanged += new System.EventHandler(this.AutoCompleteForm_VisibleChanged);
            this.ResumeLayout(false);

		}
		#endregion

		private void lstCompleteItems_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		}

		internal int SelectedIndex 
		{
			get 
			{
				if (lstCompleteItems.SelectedIndices.Count == 0)
				{
					return -1;
				}
				return lstCompleteItems.SelectedIndices[0];
			}
			set
			{
				lstCompleteItems.Items[value].Selected = true;
                Rectangle R = lstCompleteItems.Items[value].GetBounds(ItemBoundsPortion.Entire);
                if (R.Bottom > lstCompleteItems.Height) {
                    lstCompleteItems.TopItem = lstCompleteItems.Items[value - (int)Math.Floor((double)lstCompleteItems.Height / (double)ItemHeight) - 1];
                } else if (R.Top < 0) {
                    lstCompleteItems.TopItem = lstCompleteItems.Items[value];
                }
                
                if (OldHighlight < lstCompleteItems.Items.Count && OldHighlight >= 0) {
                    lstCompleteItems.Items[OldHighlight].ForeColor = Color.FromKnownColor(KnownColor.WindowText);
                    lstCompleteItems.Items[OldHighlight].BackColor = Color.FromKnownColor(KnownColor.Window);
                }
                lstCompleteItems.Items[value].BackColor = Color.FromKnownColor(KnownColor.MenuHighlight);
                lstCompleteItems.Items[value].ForeColor = Color.White;
                OldHighlight = value;
                
                
			}
		}
		private void AutoCompleteForm_Resize(object sender, System.EventArgs e)
		{
			//System.Diagnostics.Debug.WriteLine(string.Format("Size x:{0} y:{1}\r\n {2}", Size.Width , Size.Height, Environment.StackTrace));
		}

		internal void UpdateView()
		{
			lstCompleteItems.Items.Clear();
			foreach (string item in mItems)
			{
				lstCompleteItems.Items.Add(item);
			}
		}

		private void AutoCompleteForm_VisibleChanged(object sender, System.EventArgs e)
		{
			ArrayList items = new ArrayList(mItems);
			items.Sort(new CaseInsensitiveComparer());
			mItems = new StringCollection();
			mItems.AddRange((string[])items.ToArray(typeof(string)));
			columnHeader1.Width = lstCompleteItems.Width - 21;

		}

		private void lstCompleteItems_Resize(object sender, System.EventArgs e)
		{
			if (this.Size != lstCompleteItems.Size)
			{
				
			}
		}

        private void lstCompleteItems_SelectedIndexChanged_1(object sender, EventArgs e) {
            //lstCompleteItems.Select();
        }

        private void lstCompleteItems_Click(object sender, EventArgs e) {
            /*
            if (lstCompleteItems.SelectedIndices.Count != 0) {
                SelectedIndex = lstCompleteItems.SelectedIndices[0];
            }
            Message M = new Message();
            M.Msg = Win32.WM_KEYDOWN;
            M.WParam = (IntPtr)(Keys)Keys.Enter;
            OwnerEditor.RelayMessage(ref M);
             */
        }
	}
}
