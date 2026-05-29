namespace PindurTI_Debugger {
    partial class ControlPanel {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlPanel));
            this.ControlPanelMenu = new System.Windows.Forms.MenuStrip();
            this.CalculatorsList = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UseGreyscale = new System.Windows.Forms.ToolStripMenuItem();
            this.stretchScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UseLcdColours = new System.Windows.Forms.ToolStripMenuItem();
            this.setDarkestColourToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setLightestColourToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ScaleMode = new System.Windows.Forms.ToolStripComboBox();
            this.UseLCDPixels = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.alwaysOnTopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.KeyLayoutList = new System.Windows.Forms.ToolStripMenuItem();
            this.keyboardMappingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pauseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ControlFileOpenDialog = new System.Windows.Forms.OpenFileDialog();
            this.RunSimulations = new System.Windows.Forms.Timer(this.components);
            this.SetLcdColours = new System.Windows.Forms.ColorDialog();
            this.ControlPanelMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // ControlPanelMenu
            // 
            this.ControlPanelMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CalculatorsList,
            this.optionsToolStripMenuItem,
            this.debugToolStripMenuItem});
            this.ControlPanelMenu.Location = new System.Drawing.Point(0, 0);
            this.ControlPanelMenu.Name = "ControlPanelMenu";
            this.ControlPanelMenu.Size = new System.Drawing.Size(240, 24);
            this.ControlPanelMenu.TabIndex = 0;
            // 
            // CalculatorsList
            // 
            this.CalculatorsList.Name = "CalculatorsList";
            this.CalculatorsList.Size = new System.Drawing.Size(67, 20);
            this.CalculatorsList.Text = "&Calculator";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UseGreyscale,
            this.stretchScreenToolStripMenuItem,
            this.UseLcdColours,
            this.toolStripSeparator2,
            this.ScaleMode,
            this.UseLCDPixels,
            this.toolStripSeparator1,
            this.alwaysOnTopToolStripMenuItem,
            this.toolStripMenuItem1,
            this.KeyLayoutList,
            this.keyboardMappingToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // UseGreyscale
            // 
            this.UseGreyscale.Checked = true;
            this.UseGreyscale.CheckOnClick = true;
            this.UseGreyscale.CheckState = System.Windows.Forms.CheckState.Checked;
            this.UseGreyscale.Name = "UseGreyscale";
            this.UseGreyscale.Size = new System.Drawing.Size(181, 22);
            this.UseGreyscale.Text = "Use &Greyscale";
            this.UseGreyscale.Click += new System.EventHandler(this.UseGreyscale_Click);
            // 
            // stretchScreenToolStripMenuItem
            // 
            this.stretchScreenToolStripMenuItem.Checked = true;
            this.stretchScreenToolStripMenuItem.CheckOnClick = true;
            this.stretchScreenToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.stretchScreenToolStripMenuItem.Name = "stretchScreenToolStripMenuItem";
            this.stretchScreenToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.stretchScreenToolStripMenuItem.Text = "&Stretch Screen";
            this.stretchScreenToolStripMenuItem.Click += new System.EventHandler(this.stretchScreenToolStripMenuItem_Click);
            // 
            // UseLcdColours
            // 
            this.UseLcdColours.Checked = true;
            this.UseLcdColours.CheckOnClick = true;
            this.UseLcdColours.CheckState = System.Windows.Forms.CheckState.Checked;
            this.UseLcdColours.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setDarkestColourToolStripMenuItem,
            this.setLightestColourToolStripMenuItem});
            this.UseLcdColours.Name = "UseLcdColours";
            this.UseLcdColours.Size = new System.Drawing.Size(181, 22);
            this.UseLcdColours.Text = "Use LCD &Colours";
            this.UseLcdColours.ToolTipText = "Use green-tinted LCD-style colours for the display.";
            this.UseLcdColours.Click += new System.EventHandler(this.UseLcdColours_Click);
            // 
            // setDarkestColourToolStripMenuItem
            // 
            this.setDarkestColourToolStripMenuItem.Name = "setDarkestColourToolStripMenuItem";
            this.setDarkestColourToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.setDarkestColourToolStripMenuItem.Text = "Set &Darkest Colour...";
            this.setDarkestColourToolStripMenuItem.Click += new System.EventHandler(this.setDarkestColourToolStripMenuItem_Click);
            // 
            // setLightestColourToolStripMenuItem
            // 
            this.setLightestColourToolStripMenuItem.Name = "setLightestColourToolStripMenuItem";
            this.setLightestColourToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.setLightestColourToolStripMenuItem.Text = "Set &Lightest Colour...";
            this.setLightestColourToolStripMenuItem.Click += new System.EventHandler(this.setLightestColourToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(178, 6);
            // 
            // ScaleMode
            // 
            this.ScaleMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ScaleMode.Items.AddRange(new object[] {
            "Unscaled",
            "Zoom 2x",
            "Zoom 3x",
            "Zoom 4x",
            "Smooth 2x",
            "Smooth 3x"});
            this.ScaleMode.Name = "ScaleMode";
            this.ScaleMode.Size = new System.Drawing.Size(121, 21);
            this.ScaleMode.ToolTipText = "Select the image scaling mode used.";
            this.ScaleMode.SelectedIndexChanged += new System.EventHandler(this.ScaleMode_SelectedIndexChanged);
            this.ScaleMode.Click += new System.EventHandler(this.ScaleMode_Click);
            // 
            // UseLCDPixels
            // 
            this.UseLCDPixels.Checked = true;
            this.UseLCDPixels.CheckOnClick = true;
            this.UseLCDPixels.CheckState = System.Windows.Forms.CheckState.Checked;
            this.UseLCDPixels.Name = "UseLCDPixels";
            this.UseLCDPixels.Size = new System.Drawing.Size(181, 22);
            this.UseLCDPixels.Text = "LCD &Pixels";
            this.UseLCDPixels.Click += new System.EventHandler(this.UseLCDPixels_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(178, 6);
            // 
            // alwaysOnTopToolStripMenuItem
            // 
            this.alwaysOnTopToolStripMenuItem.CheckOnClick = true;
            this.alwaysOnTopToolStripMenuItem.Name = "alwaysOnTopToolStripMenuItem";
            this.alwaysOnTopToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.alwaysOnTopToolStripMenuItem.Text = "Always on &Top";
            this.alwaysOnTopToolStripMenuItem.Click += new System.EventHandler(this.alwaysOnTopToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(178, 6);
            // 
            // KeyLayoutList
            // 
            this.KeyLayoutList.Name = "KeyLayoutList";
            this.KeyLayoutList.Size = new System.Drawing.Size(181, 22);
            this.KeyLayoutList.Text = "Keypad &Layout";
            // 
            // keyboardMappingToolStripMenuItem
            // 
            this.keyboardMappingToolStripMenuItem.Enabled = false;
            this.keyboardMappingToolStripMenuItem.Name = "keyboardMappingToolStripMenuItem";
            this.keyboardMappingToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.keyboardMappingToolStripMenuItem.Text = "Keyboard &Mapping...";
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.pauseToolStripMenuItem});
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.debugToolStripMenuItem.Text = "&Debug";
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Enabled = false;
            this.startToolStripMenuItem.Image = global::PindurTI_Debugger.Properties.Resources.debug;
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.startToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.startToolStripMenuItem.Text = "&Start";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // pauseToolStripMenuItem
            // 
            this.pauseToolStripMenuItem.Name = "pauseToolStripMenuItem";
            this.pauseToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5)));
            this.pauseToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.pauseToolStripMenuItem.Text = "Break &All";
            this.pauseToolStripMenuItem.Click += new System.EventHandler(this.pauseToolStripMenuItem_Click);
            // 
            // ControlFileOpenDialog
            // 
            this.ControlFileOpenDialog.Filter = "All Files|*.*";
            // 
            // RunSimulations
            // 
            this.RunSimulations.Interval = 10;
            this.RunSimulations.Tick += new System.EventHandler(this.RunSimulations_Tick);
            // 
            // SetLcdColours
            // 
            this.SetLcdColours.AnyColor = true;
            this.SetLcdColours.FullOpen = true;
            // 
            // ControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(240, 384);
            this.Controls.Add(this.ControlPanelMenu);
            this.DataBindings.Add(new System.Windows.Forms.Binding("TopMost", global::PindurTI_Debugger.Properties.Settings.Default, "TopMost", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.ControlPanelMenu;
            this.MaximizeBox = false;
            this.Name = "ControlPanel";
            this.Text = "PindurTI";
            this.TopMost = global::PindurTI_Debugger.Properties.Settings.Default.TopMost;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ControlPanel_FormClosing);
            this.Load += new System.EventHandler(this.ControlPanel_Load);
            this.ControlPanelMenu.ResumeLayout(false);
            this.ControlPanelMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip ControlPanelMenu;
        private System.Windows.Forms.ToolStripMenuItem CalculatorsList;
        private System.Windows.Forms.OpenFileDialog ControlFileOpenDialog;
        private System.Windows.Forms.Timer RunSimulations;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem UseGreyscale;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem KeyLayoutList;
        private System.Windows.Forms.ToolStripMenuItem keyboardMappingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stretchScreenToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem UseLcdColours;
        private System.Windows.Forms.ToolStripMenuItem setDarkestColourToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setLightestColourToolStripMenuItem;
        private System.Windows.Forms.ColorDialog SetLcdColours;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem alwaysOnTopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem pauseToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        public System.Windows.Forms.ToolStripComboBox ScaleMode;
        public System.Windows.Forms.ToolStripMenuItem UseLCDPixels;
    }
}

