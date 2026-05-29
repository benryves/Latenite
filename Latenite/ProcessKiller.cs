using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Latenite {
    public partial class LateniteIDE : Form {

        #region Win32

        [DllImport("kernel32")]
        static extern bool Process32First([In]IntPtr hSnapshot, [In, Out]PROCESSENTRY32 lppe);

        [DllImport("kernel32")]
        static extern IntPtr CreateToolhelp32Snapshot([In]UInt32 dwFlags, [In]UInt32 th32ProcessID);

        [DllImport("kernel32")]
        static extern bool Process32Next([In]IntPtr hSnapshot, [In, Out]PROCESSENTRY32 lppe);

        const UInt32 TH32CS_SNAPPROCESS = 0x00000002;
        const int MAX_PATH = 260;

        [StructLayout(LayoutKind.Sequential)]
        class PROCESSENTRY32 {
            UInt32 dwSize = (UInt32)Marshal.SizeOf(typeof(PROCESSENTRY32));
            UInt32 cntUsage = 0;
            UInt32 th32ProcessID = 0;
            IntPtr th32DefaultHeapID = IntPtr.Zero;
            UInt32 th32ModuleID = 0;
            UInt32 cntThreads = 0;
            UInt32 th32ParentProcessID = 0;
            Int32 pcPriClassBase = 0;
            UInt32 dwFlags = 0;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH * sizeof(char))]
            string szExeFile;

            public UInt32 ProcID {
                get {
                    return th32ProcessID;
                }
            }

            public UInt32 ParentProcID {
                get {
                    return th32ParentProcessID;
                }
            }
        }

        #endregion

        /// <summary>
        /// Kill a process, and all child processes.
        /// </summary>
        /// <param name="ProcessToKill">The root you want to kil.</param>
        public void KillProcessFully(ref Process ProcessToKill) {

            // Take a snapshot of all the processes
            IntPtr ProcessSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);

            // Process entry details
            PROCESSENTRY32 ProcessDetails = new PROCESSENTRY32();

            // List of processes we need to kill.
            ArrayList ProcessesToKill = new ArrayList();

            bool IsFirst = true; // Looking at the FIRST process.

            bool FinishedReading = false;
            while (!FinishedReading) {

                // Get the details:
                bool SuccessfulRead = false;
                if (IsFirst) {
                    SuccessfulRead = Process32First(ProcessSnapshot, ProcessDetails);
                    IsFirst = false;
                } else {
                    SuccessfulRead = Process32Next(ProcessSnapshot, ProcessDetails);
                }

                // Check the details:
                if (SuccessfulRead) {
                    // Is the process a child of the process we want to kill?
                    if (ProcessDetails.ParentProcID == (uint)ProcessToKill.Id) {
                        ProcessesToKill.Add(ProcessDetails.ProcID);
                    }
                } else {
                    // We couldn't read, so I assume we've run off the end of the list of processes.
                    FinishedReading = true;
                }
            }                

            // Now we go and kill any listed processes that need kiling:
            foreach (uint ProcessID in ProcessesToKill) {
                try {
                    Process.GetProcessById((int)ProcessID).Kill();
                } catch (Exception ex) {
                    MessageBox.Show("Could not kill process:\n" + ex.Message, "Terminate Build", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            // Kill the root process:
            try {
                ProcessToKill.Kill();
            } catch (Exception ex) {
                MessageBox.Show("Could not kill process:\n" + ex.Message, "Terminate Build", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                
            }
        }
    }   
}
