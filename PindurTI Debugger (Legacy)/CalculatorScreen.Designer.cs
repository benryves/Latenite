namespace PindurTI_Debugger {
    partial class CalculatorScreen {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.CalculatorMenuStrip = new System.Windows.Forms.MenuStrip();
            this.actionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clockSpeedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ScreenArea = new System.Windows.Forms.PictureBox();
            this.CalculatorScreenOpenDialog = new System.Windows.Forms.OpenFileDialog();
            this.CalculatorMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenArea)).BeginInit();
            this.SuspendLayout();
            // 
            // CalculatorMenuStrip
            // 
            this.CalculatorMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.actionsToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.CalculatorMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.CalculatorMenuStrip.Name = "CalculatorMenuStrip";
            this.CalculatorMenuStrip.Size = new System.Drawing.Size(88, 24);
            this.CalculatorMenuStrip.TabIndex = 0;
            this.CalculatorMenuStrip.Text = "menuStrip1";
            // 
            // actionsToolStripMenuItem
            // 
            this.actionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetToolStripMenuItem,
            this.sendFileToolStripMenuItem});
            this.actionsToolStripMenuItem.Name = "actionsToolStripMenuItem";
            this.actionsToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.actionsToolStripMenuItem.Text = "&Actions";
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.resetToolStripMenuItem.Text = "&Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // sendFileToolStripMenuItem
            // 
            this.sendFileToolStripMenuItem.Image = global::PindurTI_Debugger.Properties.Resources.folder;
            this.sendFileToolStripMenuItem.Name = "sendFileToolStripMenuItem";
            this.sendFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.sendFileToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.sendFileToolStripMenuItem.Text = "Send &File";
            this.sendFileToolStripMenuItem.Click += new System.EventHandler(this.sendFileToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clockSpeedToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // clockSpeedToolStripMenuItem
            // 
            this.clockSpeedToolStripMenuItem.Name = "clockSpeedToolStripMenuItem";
            this.clockSpeedToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.clockSpeedToolStripMenuItem.Text = "&Clock Speed...";
            this.clockSpeedToolStripMenuItem.Click += new System.EventHandler(this.clockSpeedToolStripMenuItem_Click);
            // 
            // ScreenArea
            // 
            this.ScreenArea.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ScreenArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScreenArea.Location = new System.Drawing.Point(0, 24);
            this.ScreenArea.Name = "ScreenArea";
            this.ScreenArea.Padding = new System.Windows.Forms.Padding(4);
            this.ScreenArea.Size = new System.Drawing.Size(88, 46);
            this.ScreenArea.TabIndex = 1;
            this.ScreenArea.TabStop = false;
            // 
            // CalculatorScreenOpenDialog
            // 
            this.CalculatorScreenOpenDialog.Filter = "All TI82/83/83+ Files (*.82?;*.83?;*.8x?)|*.82?;*.83?;*.8x?|All Files (*.*)|*.*";
            this.CalculatorScreenOpenDialog.Multiselect = true;
            // 
            // CalculatorScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(88, 70);
            this.Controls.Add(this.ScreenArea);
            this.Controls.Add(this.CalculatorMenuStrip);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MainMenuStrip = this.CalculatorMenuStrip;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CalculatorScreen";
            this.ShowInTaskbar = false;
            this.Text = "CalculatorScreen";
            this.Shown += new System.EventHandler(this.CalculatorScreen_Shown);
            this.Activated += new System.EventHandler(this.CalculatorScreen_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CalculatorScreen_FormClosing);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CalculatorScreen_KeyUp);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CalculatorScreen_KeyDown);
            this.Load += new System.EventHandler(this.CalculatorScreen_Load);
            this.CalculatorMenuStrip.ResumeLayout(false);
            this.CalculatorMenuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenArea)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip CalculatorMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem actionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendFileToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog CalculatorScreenOpenDialog;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clockSpeedToolStripMenuItem;
        public System.Windows.Forms.PictureBox ScreenArea;

    }
}