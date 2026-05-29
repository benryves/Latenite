using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Latenite {

	public class SortableListView : ListView {

        

		public SortableListView() {
			this.View = View.Details;
			this.HeaderStyle = ColumnHeaderStyle.Clickable;
            base.ColumnClick += new ColumnClickEventHandler(SortableListView_ColumnClick);
         
		}

        void SortableListView_ColumnClick(object sender, ColumnClickEventArgs e) {
          
        }

        
        /// <summary>
        /// Hacky override to fix the mucky lines.
        /// </summary>
        protected override void WndProc(ref Message m) {
            if (m.Msg == 0xF) { // 0xF is WM_PAINT
                GridLines = false;
                GridLines = true;
            }
            base.WndProc(ref m);
        }

	}
}
