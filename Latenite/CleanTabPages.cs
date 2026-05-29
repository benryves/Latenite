using System;
using System.Text;
using System.Windows.Forms;

namespace Latenite {
    public class CleanTabPage : TabPage {
        /// <summary>
        /// Hacky override to fix the garbage lines.
        /// </summary>
        protected override void WndProc(ref Message m) {
            if (m.Msg == 0xF) { // 0xF is WM_PAINT
                UseVisualStyleBackColor = false;
                UseVisualStyleBackColor = true;
            }
            base.WndProc(ref m);
        }


    }
}
