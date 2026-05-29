using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PindurTI_Debugger {
    public partial class CalculatorScreen : Form {



        public int SlotNumber = 0;
        public int ClockSpeed = 6000;

        private ControlPanel Controller;

        public CalculatorScreen(ControlPanel parent) {
            Controller = parent;
            InitializeComponent();
            this.ScreenArea.BackgroundImage = new Bitmap(96, 64);
        }

        public string RomFile = "";


        private void CalculatorScreen_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = true;
            this.Hide();
        }

        private int[] Registers = new int[16];
        public int RegAF { get { return Registers[00]; } }
        public int RegBC { get { return Registers[01]; } }
        public int RegDE { get { return Registers[02]; } }
        public int RegHL { get { return Registers[03]; } }
        public int RegIX { get { return Registers[04]; } }
        public int RegIY { get { return Registers[05]; } }
        public int RegPC { get { return Registers[06]; } }
        public int RegSP { get { return Registers[07]; } }
        public int RegAFs { get { return Registers[08]; } }
        public int RegBCs { get { return Registers[09]; } }
        public int RegDEs { get { return Registers[10]; } }
        public int RegHLs { get { return Registers[11]; } }
        public int RegR { get { return Registers[12]; } }
        public int RegI { get { return Registers[13]; } }
        public int RegIM { get { return Registers[14]; } }
        public int RegHALT { get { return Registers[15]; } }

        public void RefreshScreenImage() {
            try {
                Application.DoEvents();
                Program.PindurTI.StandardInput.WriteLine("activate-slot " + this.SlotNumber);
                string Response = Program.PindurTI.StandardOutput.ReadLine();
                if (Response != "OK") {
                    throw new Exception(Response);
                }

                Program.PindurTI.StandardInput.WriteLine("dump-state cpu");
                Response = Program.PindurTI.StandardOutput.ReadLine();
                if (Response != "OK") {
                    throw new Exception(Response);
                }
                for (int i = 0; i < 16; ++i) {
                    Registers[i] = Convert.ToInt16(Program.PindurTI.StandardOutput.ReadLine().Split('=')[1], 16);
                }


                Program.PindurTI.StandardInput.WriteLine("dump-state model");
                Response = Program.PindurTI.StandardOutput.ReadLine();
                if (Response != "OK") {
                    throw new Exception(Response);
                }
                int Lines = int.Parse(Program.PindurTI.StandardOutput.ReadLine());
                string Caption = "";
                for (int i = 0; i < Lines; i++) {
                    Caption += Program.PindurTI.StandardOutput.ReadLine();
                }
                this.Text = Caption;

                
                Program.PindurTI.StandardInput.WriteLine("draw-screen-" + (Controller.UseGreyscale.Checked ? "gs" : "bw"));
                byte[] Data = new Byte[96 * 64];
                BinaryReader BR = new BinaryReader(Program.PindurTI.StandardOutput.BaseStream);
                //Response = Program.PindurTI.StandardOutput.ReadLine();

                for (int i = 0; i < 4; i++) {
                    byte I = BR.ReadByte();
                }
                byte CalcStatus = BR.ReadByte();
                byte LcdStatus = BR.ReadByte();
                BR.ReadByte();
                BR.ReadByte();

                Data = BR.ReadBytes(96 * 64);

                int Width = 96;
                int Height = 64;
                switch (Program.MainControlPanel.ScaleMode.SelectedIndex) {
                    case 1:
                    case 4:
                        Width *= 2; Height *= 2; break;
                    case 2:
                    case 5:
                        Width *= 3; Height *= 3; break;
                    case 3:
                    case 6:
                        Width *= 4; Height *= 4; break;
                }
                if (this.ScreenArea.BackgroundImage.Width != Width || this.ScreenArea.BackgroundImage.Height != Height) {
                    this.ScreenArea.BackgroundImage.Dispose();
                    this.ScreenArea.BackgroundImage = new Bitmap(Width, Height, PixelFormat.Format32bppRgb);
                }

                BitmapData BD = ((Bitmap)this.ScreenArea.BackgroundImage).LockBits(new Rectangle(0, 0, this.ScreenArea.BackgroundImage.Width, this.ScreenArea.BackgroundImage.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
                int PixelBorder = Program.MainControlPanel.UseLCDPixels.Checked ? 0x20 : 0x00;
                unsafe {
                    int* p = (int*)BD.Scan0.ToPointer();
                    int Bright;
                    if (Program.MainControlPanel.ScaleMode.SelectedIndex == 0) {
                        for (int i = 0; i < 96 * 64; ++i) {
                            Bright = 0xFF - Data[i];
                            *p++ = GetPixelColour(Bright);
                        }
                    } else if (Program.MainControlPanel.ScaleMode.SelectedIndex == 1) {
                        for (int y = 0; y < 64; y++) {
                            for (int x = 0; x < 96; x++) {
                                Bright = 0xFF - Data[x + y * 96];

                                int PixelD = GetPixelColour(Bright - PixelBorder / 2);
                                int PixelB = GetPixelColour(Bright + PixelBorder / 2);

                                *(p + 192) = PixelD;
                                *p++ = PixelB;

                                *(p + 192) = PixelB;
                                *p++ = PixelB;
                            }
                            p += 192;
                        }
                    } else if (Program.MainControlPanel.ScaleMode.SelectedIndex == 2) {
                        for (int y = 0; y < 64; y++) {
                            for (int x = 0; x < 96; x++) {
                                Bright = 0xFF - Data[x + y * 96];

                                int PixelD = GetPixelColour(Bright);
                                int PixelB = GetPixelColour(Bright + PixelBorder);

                                *(p + 288) = PixelD;
                                *(p + 576) = PixelB;
                                *p++ = GetPixelColour(Bright - PixelBorder / 2);

                                *(p + 288) = PixelD;
                                *(p + 576) = PixelB;
                                *p++ = PixelD;

                                *(p + 288) = PixelB;
                                *(p + 576) = PixelB;
                                *p++ = PixelB;

                            }
                            p += 576;
                        }
                    } else if (Program.MainControlPanel.ScaleMode.SelectedIndex == 3) {
                        for (int y = 0; y < 64; y++) {
                            for (int x = 0; x < 96; x++) {
                                Bright = 0xFF - Data[x + y * 96];

                                int PixelD = GetPixelColour(Bright);
                                int PixelB = GetPixelColour(Bright + PixelBorder);

                                *(p + 384) = PixelD;
                                *(p + 768) = PixelD;
                                *(p + 1152) = PixelB;
                                *p++ = GetPixelColour(Bright - PixelBorder / 2);

                                *(p + 384) = PixelD;
                                *(p + 768) = PixelD;
                                *(p + 1152) = PixelB;
                                *p++ = PixelD;

                                *(p + 384) = PixelD;
                                *(p + 768) = PixelD;
                                *(p + 1152) = PixelB;
                                *p++ = PixelD;

                                *(p + 384) = PixelB;
                                *(p + 768) = PixelB;
                                *(p + 1152) = PixelB;
                                *p++ = PixelB;

                            }
                            p += 1152;
                        }
                    } else if (Program.MainControlPanel.ScaleMode.SelectedIndex == 4) {
                        // 2x smooth
                        for (int y = 0; y < 64; y++) {
                            for (int x = 0; x < 96; x++) {

                                int PixelA = y > 00 ? Data[x + (y - 1) * 96] : Data[x + y * 96]; // Above
                                int PixelB = y < 63 ? Data[x + (y + 1) * 96] : Data[x + y * 96]; // Below
                                int PixelL = x > 00 ? Data[(x - 1) + y * 96] : Data[x + y * 96]; // Left
                                int PixelR = x < 95 ? Data[(x + 1) + y * 96] : Data[x + y * 96]; // Right
                                int PixelC = Data[x + y * 96]; // Central

                                // Limit to 8 shades;
                                PixelA &= 0xE0;
                                PixelB &= 0xE0;
                                PixelL &= 0xE0;
                                PixelR &= 0xE0;
                                PixelC &= 0xE0;

                                int NewTL;
                                int NewTR;
                                int NewBL;
                                int NewBR;

                                if (PixelA != PixelB && PixelL != PixelR) {
                                    NewTL = PixelL == PixelA ? PixelL : PixelC;
                                    NewTR = PixelA == PixelR ? PixelR : PixelC;
                                    NewBL = PixelL == PixelB ? PixelL : PixelC;
                                    NewBR = PixelB == PixelR ? PixelR : PixelC;
                                } else {
                                    NewTL = PixelC;
                                    NewTR = PixelC;
                                    NewBL = PixelC;
                                    NewBR = PixelC;
                                }

                                *(p + 192) = GetPixelColour(0xFF - NewBL);
                                *p++ = GetPixelColour(0xFF - NewTL);
                                *(p + 192) = GetPixelColour(0xFF - NewBR);
                                *p++ = GetPixelColour(0xFF - NewTR);

                            }
                            p += 192;
                        }
                    } else if (Program.MainControlPanel.ScaleMode.SelectedIndex == 5) {
                        // 3x smooth
                        for (int y = 0; y < 64; y++) {
                            for (int x = 0; x < 96; x++) {

                                int B = y > 00 ? Data[x + (y - 1) * 96] : Data[x + y * 96]; // Above
                                int H = y < 63 ? Data[x + (y + 1) * 96] : Data[x + y * 96]; // Below
                                int D = x > 00 ? Data[(x - 1) + y * 96] : Data[x + y * 96]; // Left
                                int F = x < 95 ? Data[(x + 1) + y * 96] : Data[x + y * 96]; // Right

                                int A = x > 00
                                    ? (y > 00 ? Data[(x - 1) + (y - 1) * 96] : Data[(x - 1) + y * 96])
                                    : (y > 00 ? Data[x + (y - 1) * 96] : Data[x + y * 96]);

                                int G = x > 00
                                    ? (y < 63 ? Data[(x - 1) + (y + 1) * 96] : Data[(x - 1) + y * 96])
                                    : (y < 63 ? Data[x + (y + 1) * 96] : Data[x + y * 96]);

                                int C = x < 95
                                    ? (y > 00 ? Data[(x + 1) + (y - 1) * 96] : Data[(x + 1) + y * 96])
                                    : (y > 00 ? Data[x + (y - 1) * 96] : Data[x + y * 96]);

                                int I = x < 95
                                    ? (y < 63 ? Data[(x + 1) + (y + 1) * 96] : Data[(x + 1) + y * 96])
                                    : (y < 63 ? Data[x + (y + 1) * 96] : Data[x + y * 96]);

                                int E = Data[x + y * 96]; // Central

                                // Limit to 8 shades;
                                B &= 0xE0;
                                H &= 0xE0;
                                D &= 0xE0;
                                F &= 0xE0;
                                E &= 0xE0;

                                A &= 0xE0;
                                C &= 0xE0;
                                G &= 0xE0;
                                I &= 0xE0;

                                int E0;
                                int E1;
                                int E2;

                                int E3;
                                int E4;
                                int E5;

                                int E6;
                                int E7;
                                int E8;

                                if (B != H && D != F) {
                                    E0 = D == B ? D : E;
                                    E1 = (D == B && E != C) || (B == F && E != A) ? B : E;
                                    E2 = B == F ? F : E;
                                    E3 = (D == B && E != G) || (D == H && E != A) ? D : E;
                                    E4 = E;
                                    E5 = (B == F && E != I) || (H == F && E != C) ? F : E;
                                    E6 = D == H ? D : E;
                                    E7 = (D == H && E != I) || (H == F && E != G) ? H : E;
                                    E8 = H == F ? F : E;

                                } else {
                                    E0 = E;
                                    E1 = E;
                                    E2 = E;
                                    E3 = E;
                                    E4 = E;
                                    E5 = E;
                                    E6 = E;
                                    E7 = E;
                                    E8 = E;
                                }


                                *(p + 288) = GetPixelColour(0xFF - E3);
                                *(p + 576) = GetPixelColour(0xFF - E6);
                                *p++ = GetPixelColour(0xFF - E0);

                                *(p + 288) = GetPixelColour(0xFF - E4);
                                *(p + 576) = GetPixelColour(0xFF - E7);
                                *p++ = GetPixelColour(0xFF - E1);

                                *(p + 288) = GetPixelColour(0xFF - E5);
                                *(p + 576) = GetPixelColour(0xFF - E8);
                                *p++ = GetPixelColour(0xFF - E2);


                            }
                            p += 576;
                        }
                    }
                }

                ((Bitmap)this.ScreenArea.BackgroundImage).UnlockBits(BD);

                this.ScreenArea.Visible = (LcdStatus == (byte)'1');

                this.Refresh();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "PindurTI Graphics Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Hide();
            }
        }

        private int GetPixelColour(int Brightness) {
            if (Brightness > 0xFF) Brightness = 0xFF;
            if (Brightness < 0x00) Brightness = 0x00;
            if (Program.MainControlPanel.UseLcdColours.Checked) {
                int B = (Brightness * (((Program.MainControlPanel.PixelLightest >> 00) & 0xFF) - ((Program.MainControlPanel.PixelDarkest >> 00) & 0xFF))) / 256 + ((Program.MainControlPanel.PixelDarkest >> 00) & 0xFF);
                int G = (Brightness * (((Program.MainControlPanel.PixelLightest >> 08) & 0xFF) - ((Program.MainControlPanel.PixelDarkest >> 08) & 0xFF))) / 256 + ((Program.MainControlPanel.PixelDarkest >> 08) & 0xFF);
                int R = (Brightness * (((Program.MainControlPanel.PixelLightest >> 16) & 0xFF) - ((Program.MainControlPanel.PixelDarkest >> 16) & 0xFF))) / 256 + ((Program.MainControlPanel.PixelDarkest >> 16) & 0xFF);
                return B | ((G & 0xFF) << 08) | ((R & 0xFF) << 16);
            } else {
                return Brightness + (Brightness << 8) + (Brightness << 16);
            }
        }

        private void CalculatorScreen_Load(object sender, EventArgs e) {
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e) {
            try {
                Controller.SendCommand(this, "reset-calc");
                Controller.SendCommand(this, "run 12000000");
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "PindurTI Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void CalculatorScreen_Activated(object sender, EventArgs e) {
            Controller.LastSelectedScreen = this;
        }

        private void CalculatorScreen_KeyDown(object sender, KeyEventArgs e) {
            e.Handled = true;
            Controller.SendKeyPress(this, true, e);
        }

        private void CalculatorScreen_KeyUp(object sender, KeyEventArgs e) {
            e.Handled = true;
            Controller.SendKeyPress(this, false, e);
        }

        private void sendFileToolStripMenuItem_Click(object sender, EventArgs e) {
            Controller.PauseSimulation();
            DialogResult D = this.CalculatorScreenOpenDialog.ShowDialog();
            if (D == DialogResult.OK) {
                foreach (string F in CalculatorScreenOpenDialog.FileNames) {
                    try {
                        Controller.SendCommand(this, "send-file " + this.SlotNumber + " " + F);
                    } catch (Exception ex) {
                        D = MessageBox.Show(F + "\n" + ex.Message + "\nWould you like to continue sending files?", "File Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        if (D == DialogResult.No) break;
                    }
                }
            }
            Controller.UnpauseSimulation();
        }

        private void clockSpeedToolStripMenuItem_Click(object sender, EventArgs e) {
            TextDialog T = new TextDialog("Enter a new clock speed (in kHz):", "Clock Speed", this.ClockSpeed.ToString(), "00000");
            bool WasTopmost = this.Owner.TopMost;
            this.Owner.TopMost = false;
            DialogResult D = T.ShowDialog();
            if (D == DialogResult.OK) {
                try {
                    this.ClockSpeed = Convert.ToInt32(T.Value.Text);
                } catch (Exception ex) {
                    MessageBox.Show("Could not set clock speed:\n" + ex.Message, "Clock Speed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            this.Owner.TopMost = WasTopmost;
        }

        private void CalculatorScreen_Shown(object sender, EventArgs e) {
            this.Focus();
            this.BringToFront();
        }
        public new void Show(IWin32Window owner) {

            int ExtraWidth = this.Width - ScreenArea.Width;
            int ExtraHeight = this.Height - ScreenArea.Height;

            int Width = 96;
            int Height = 64;
            switch (Program.MainControlPanel.ScaleMode.SelectedIndex) {
                case 1:
                case 4:
                    Width *= 2; Height *= 2; break;
                case 2:
                case 5:
                    Width *= 3; Height *= 3; break;
                case 3:
                case 6:
                    Width *= 4; Height *= 4; break;
            }
            this.Width = Width + ExtraWidth;
            this.Height = Height + ExtraHeight;
            base.Show(owner);
        }
    }
}
