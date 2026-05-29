using IconHandler;
using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Diagnostics;
using System.Xml;
using System.Threading;

namespace Latenite {
    public class TreeFileBrowser : TreeView {

        // Notes:
        // Enable LabelEdit to allow the user to rename files.
        // 

        //public ImageList IconCache = new ImageList();

        private ArrayList ExtensionImageLink = new ArrayList();

        private FileSystemWatcher FileWatcher;

        int OldMouseX = 0;
        int OldMouseY = 0;

        /// <summary>
        /// The root directory of the control
        /// </summary>
        public string RootDirectory = Application.StartupPath;

        private string _pattern = "*.*";

        /// <summary>
        /// A TreeNode which stores the node that was selected before we let it be dragged.
        /// </summary>
        private TreeNode SelectedBeforeDrag;

        /// <summary>
        /// The search pattern specifying which files to display (specify multiple patterns by using a semicolon as a delimiter, eg *.txt;*.doc)
        /// </summary>
        public string Pattern {
            get {
                return _pattern;
            }
            set {
                _pattern = value;
                /*foreach (TreeNode Folder in this.Nodes) {
                    if (Folder.IsExpanded == true) {
                        TreeNode N = Folder;
                        PopulateFolder(ref N);
                        N.Expand();
                    }
                }*/
            }
        }

        public class SpecialSubFile {
            public string Source = "";
            public string Args = "";
            public int ImageIndex = 0;
            public string Program = "Associated Program";
        }


        private TreeNode AbsoluteRoot = new TreeNode();

        public void RefreshFolders() {

            this.Nodes.Clear();
            AbsoluteRoot = new TreeNode(Path.GetFileName(RootDirectory));
            AbsoluteRoot.ImageIndex = 1;
            AbsoluteRoot.SelectedImageIndex = 1;
            AbsoluteRoot.Nodes.Clear();
            AbsoluteRoot.Tag = RootDirectory;
            PopulateFolder(ref AbsoluteRoot);
            Nodes.Add(AbsoluteRoot);
            FileWatcher = new FileSystemWatcher(RootDirectory);
            FileWatcher.Created += new FileSystemEventHandler(FileWatcher_Modified);
            FileWatcher.Deleted += new FileSystemEventHandler(FileWatcher_Modified);
            FileWatcher.Renamed += new RenamedEventHandler(FileWatcher_Renamed);
            FileWatcher.IncludeSubdirectories = true;
            FileWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastAccess;
            FileWatcher.EnableRaisingEvents = true;
        
        }

        /// <summary>
        /// The contents of a file has been modified.
        /// </summary>
        void FileWatcher_Changed(object sender, FileSystemEventArgs e) {
            if (e.ChangeType == WatcherChangeTypes.Changed) {
                LateniteIDE L = (LateniteIDE)FindForm();
                foreach (SourceFile S in L.SourceFiles.TabPages) {
                    if (S.Filename.ToLower() == e.FullPath.ToLower()) {
                        S.Reload();
                    }
                }
            }
        }
        /// <summary>
        /// Simple renaming magic.
        /// </summary>
        void FileWatcher_Renamed(object sender, RenamedEventArgs e) {
            // Rename the folder on the tree view
            string RootDir = Path.GetDirectoryName(e.OldFullPath);
            foreach (TreeNode T in Nodes) {
                if (T.Tag.ToString().ToLower() == RootDir.ToLower()) {
                    TreeNode ToRefresh = T;
                    PopulateFolder(ref ToRefresh);
                    break;
                }
            }
            // Rename the source files
            LateniteIDE L = (LateniteIDE)FindForm();
            foreach (SourceFile S in L.SourceFiles.TabPages) {
                if (S.Filename.ToLower() == e.OldFullPath.ToLower()) {
                    S.SetNewFilename(e.FullPath);
                }
            }
        }
        /// <summary>
        /// This just brute-forces refreshes the folder
        /// </summary>
        void FileWatcher_Modified(object sender, FileSystemEventArgs e) {
            string RootDir =  Path.GetDirectoryName(e.FullPath);
            foreach (TreeNode T in Nodes) {
                if (T.Tag.ToString().ToLower() == RootDir.ToLower()) {
                    TreeNode ToRefresh = T;
                    PopulateFolder(ref ToRefresh);
                    return;
                }
            }
            
        }

        protected override void OnBeforeLabelEdit(NodeLabelEditEventArgs e) {
            if (e.Node != null && IsSpecialSubFile(e.Node)) {
                e.CancelEdit = true;
            } else {
                base.OnBeforeLabelEdit(e);
            }
        }

        bool IgnoreExpanding = false;

        /// <summary>
        /// Create a new instance of the TreeFileBrowser class.
        /// </summary>
        public TreeFileBrowser(ImageList IconCache) {
            // Add a few useful event handlers to make sure the thing works properly:
            base.MouseDown += new MouseEventHandler(TreeFileBrowser_MouseDown);
            base.BeforeExpand += new TreeViewCancelEventHandler(TreeFileBrowser_BeforeExpand);
            base.AfterExpand += new TreeViewEventHandler(TreeFileBrowser_AfterExpand);
            base.AfterCollapse += new TreeViewEventHandler(TreeFileBrowser_AfterCollapse);
            base.AfterLabelEdit += new NodeLabelEditEventHandler(TreeFileBrowser_AfterLabelEdit);
            base.BeforeCollapse += new TreeViewCancelEventHandler(TreeFileBrowser_BeforeCollapse);
            base.BeforeExpand += new TreeViewCancelEventHandler(TreeFileBrowser_BeforeCollapse);
            base.MouseUp += new MouseEventHandler(TreeFileBrowser_MouseUp);
            base.NodeMouseClick += new TreeNodeMouseClickEventHandler(TreeFileBrowser_NodeMouseClick);
            base.MouseMove += new MouseEventHandler(TreeFileBrowser_MouseMove);
            base.BeforeLabelEdit += new NodeLabelEditEventHandler(TreeFileBrowser_BeforeLabelEdit);
            base.ImageList = IconCache;
            base.ItemHeight = 19;
            // Initialise the IconCache:
            FixIconCache();
        }

        public void FixIconCache() {

            ExtensionImageLink.Clear();
            ExtensionImageLink.Add("");
            ExtensionImageLink.Add("");
            ExtensionImageLink.Add("");

            ImageList.Images.Clear();
            ImageList.Images.Add(Latenite.Properties.Resources.page_white_code);
            ImageList.Images.Add(Latenite.Properties.Resources.folder);
            ImageList.Images.Add(Latenite.Properties.Resources.folder);

            RefreshFolders();
            
        }
        private void UpdateIcon(TreeNode ToUpdate) {

        }

        void TreeFileBrowser_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e) {
            if (e.Node == AbsoluteRoot) e.CancelEdit = true;
        }

        void TreeFileBrowser_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) {
            SelectedBeforeDrag = e.Node;
        }

        void TreeFileBrowser_BeforeCollapse(object sender, TreeViewCancelEventArgs e) {
            IgnoreExpanding = true;
        }

        void TreeFileBrowser_MouseUp(object sender, MouseEventArgs e) {
            if (IgnoreExpanding) return;

            // Are we dropping (moving) a file?
            if (SelectedBeforeDrag != null && !IsSpecialSubFile(SelectedBeforeDrag)) {
                if (SelectedBeforeDrag != SelectedNode) {
                    // So, we're moving a file.
                    string OldName = (string)SelectedBeforeDrag.Tag;


                    bool MovingFromDirectory = SelectedBeforeDrag.ImageIndex == 1 || SelectedBeforeDrag.ImageIndex == 2;

                    // Are we moving it onto a directory?

                    TreeNode NewDirectoryNode;

                    if (SelectedNode.ImageIndex == 1 || SelectedNode.ImageIndex == 2) {
                        NewDirectoryNode = SelectedNode;
                    } else {
                        NewDirectoryNode = SelectedNode.Parent;
                    }

                    if (IsSpecialSubFile(NewDirectoryNode)) return;

                    string NewDirectory = (string)NewDirectoryNode.Tag;

                    string NewName = Path.Combine(NewDirectory, Path.GetFileName(OldName));

                    // Skip duplicates:
                    if (NewName != OldName) {

                        if (MessageBox.Show("Are you sure you want to move \"" + OldName.Replace(RootDirectory + "\\", "") + "\" to \"" + NewName.Replace(RootDirectory + "\\", "") + "\"?", "Move", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

                        // Here we try to move the file/folder:
                        try {
                            if (MovingFromDirectory) {
                                Directory.Move(OldName, NewName);
                            } else {
                                File.Move(OldName, NewName);
                            }
                            // Success!
                            TreeNode OldDirectoryNode = SelectedBeforeDrag.Parent;
                            // Remove the node from the current tree:
                            OldDirectoryNode.Nodes.Remove(SelectedBeforeDrag);
                            PopulateFolder(ref NewDirectoryNode);
                        } catch (Exception ex) {
                            MessageBox.Show("There was an error moving the " + (MovingFromDirectory ? "directory" : "file") + ":\n" + ex.Message, "Move", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        SelectedNode = SelectedBeforeDrag;
                    }
                }
            }
        }

        // Handle 
        void TreeFileBrowser_MouseMove(object sender, MouseEventArgs e) {
            // Check for left button:
            if ((e.Button & MouseButtons.Left) != 0 && (e.X != OldMouseX || e.Y != OldMouseY)) {
                TreeNode UnderMouse = GetNodeAt(e.X, e.Y);
                if (UnderMouse != null) {
                    SelectedNode = UnderMouse;
                }
            }
            OldMouseX = e.X;
            OldMouseY = e.Y;
        }

        void TreeFileBrowser_AfterLabelEdit(object sender, NodeLabelEditEventArgs e) {
            // Was the edit cancelled?
            if (e.Label == null || e.Label == "") {
                e.CancelEdit = true;
                return;
            }

            if (IsSpecialSubFile(e.Node)) return;

            // Get old/new paths:
            string OldPath = (string)e.Node.Tag;
            string NewPath = Path.Combine(Path.GetDirectoryName(OldPath), e.Label);

            // Is it the same?
            if (OldPath == NewPath) {
                e.CancelEdit = true;
                return;
            }

            // Try to rename (done by moving)
            try {
                // Is it a folder? (check by testing image index - 1 or 2 is the folder icon).
                if (e.Node.ImageIndex == 1 || e.Node.ImageIndex == 2) {
                    Directory.Move(OldPath, NewPath);
                } else {
                    File.Move(OldPath, NewPath);
                }
                // Update the path:
                e.Node.Tag = NewPath;
            } catch (Exception ex) {
                // Something went wrong!
                MessageBox.Show("There was an error renaming the file:\n" + ex.Message, "Rename", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.CancelEdit = true;
            }

        }
        /// <summary>
        /// Event handler to change folder icon to "collapsed" view
        /// </summary>
        void TreeFileBrowser_AfterCollapse(object sender, TreeViewEventArgs e) {
            IgnoreExpanding = false;
            if (e.Node.ImageIndex == 2) {
                e.Node.ImageIndex = 1;
                e.Node.SelectedImageIndex = 1;
            }
            if (SelectedBeforeDrag != null) SelectedNode = SelectedBeforeDrag;
        }
        /// <summary>
        /// Event handler to change folder icon to "expanded" view
        /// </summary>
        void TreeFileBrowser_AfterExpand(object sender, TreeViewEventArgs e) {
            IgnoreExpanding = false;
            if (e.Node.ImageIndex == 1) {
                e.Node.ImageIndex = 2;
                e.Node.SelectedImageIndex = 2;
            }
            if (SelectedBeforeDrag != null) SelectedNode = SelectedBeforeDrag;
        }

        /// <summary>
        /// Event handler to update the tree view should someone decide to expand a tree
        /// </summary>
        /// <param name="e">Node that is being collapsed</param>
        private void TreeFileBrowser_BeforeExpand(object sender, TreeViewCancelEventArgs e) {
            TreeNode N = e.Node;
            if (N != null && N.ImageIndex < 3) PopulateFolder(ref N);
        }

        /// <summary>
        /// Extra function to 'fix' the strange default TreeView behaviour on right-click
        /// </summary>
        /// <param name="e">Mouse coordinates</param>
        private void TreeFileBrowser_MouseDown(object sender, MouseEventArgs e) {
            if (IgnoreExpanding) return;
            TreeNode ToSelect = GetNodeAt(e.X, e.Y);
            if (ToSelect != null) {
                SelectedNode = ToSelect;
                SelectedBeforeDrag = ToSelect;
            }
        }


        public int GetIconIndex(string Filename) {
            // See if we can find the icon
            for (int i = 0; i < ExtensionImageLink.Count; ++i) {
                string S = (string)ExtensionImageLink[i];
                if (S.ToLower() == Path.GetExtension(Filename).ToLower()) {
                    return i;
                }
            }
            Image NewIcon = ExtractIcon(Filename);
            if (NewIcon == null) {
                NewIcon = ImageList.Images[0];
            }
            NewIcon.Tag = Path.GetExtension(Filename);
            ImageList.Images.Add(NewIcon);
            ExtensionImageLink.Add(Path.GetExtension(Filename));
            return ImageList.Images.Count - 1;

        }

        /// <summary>
        /// Nifty callback delegate for thread-safe modification.
        /// </summary>
        /// <param name="BaseNode">Node to repopulate</param>
        private delegate void PopulateFolderCallback(ref TreeNode BaseNode);
        /// <summary>
        /// Fill a TreeNode with the contents of that folder.
        /// </summary>
        /// <param name="BaseNode">Node to repopulate</param>
        public void PopulateFolder(ref TreeNode BaseNode) {

            if (base.InvokeRequired) {
                // Gadzooks! We're calling this from another thread.
                PopulateFolderCallback P = new PopulateFolderCallback(ref PopulateFolder);
                // Run this thing again, but invoke the method from the object itself.
                base.Invoke(P, BaseNode);
                // We're done.
                return;
            }

            //bool WasExpanded = BaseNode.IsExpanded;

            if (IsSpecialSubFile(BaseNode)) return;

            ArrayList WereExpanded = new ArrayList();

            foreach (TreeNode ON in BaseNode.Nodes) {
                if (ON.IsExpanded) WereExpanded.Add(ON.Tag);                
            }

            BaseNode.Nodes.Clear();
            string BaseFolder = (string)BaseNode.Tag;

            string[] Subfolders;
            try {
                Subfolders = Directory.GetDirectories(BaseFolder);
            } catch {
                // Could not list - denied?
                return;
            }

            foreach (string Foldername in Subfolders) {

				DirectoryInfo DI = new DirectoryInfo(Foldername);
				if ((DI.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden ||	(DI.Attributes & FileAttributes.System) == FileAttributes.System) continue;

                TreeNode NewFolder = new TreeNode(Path.GetFileName(Foldername));
                NewFolder.Tag = Path.Combine(BaseFolder, Foldername);
                bool IsEmpty = true;
                try {
                    string[] T = Directory.GetFileSystemEntries((string)NewFolder.Tag, "*");
                    if (T.Length != 0) IsEmpty = false;
                } catch { }
                if (!IsEmpty) NewFolder.Nodes.Add("");
                NewFolder.ImageIndex = 1;
                NewFolder.SelectedImageIndex = 1;
                BaseNode.Nodes.Add(NewFolder);
                if (WereExpanded.Contains(NewFolder.Tag)) {
                    NewFolder.Expand();
                }
            }
            ArrayList Files = new ArrayList();
            try {
                /*string[] Patterns = _pattern.Split(new char[] { ';' });
                foreach (string P in Patterns) {
                    string[] FileNames = Directory.GetFiles(BaseFolder, P);
                    foreach (string F in FileNames) {
                        Files.Add(F);
                    }
                }*/
                string[] FileNames = Directory.GetFiles(BaseFolder);
                foreach (string F in FileNames) {

					FileInfo FI = new FileInfo(F);
					if ((FI.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden || (FI.Attributes & FileAttributes.System) == FileAttributes.System) continue;

                    Files.Add(F);                    
                }

            } catch {
                // Could not list - denied?
                return;
            }

            Files.Sort();
            foreach (string Filename in Files) {
                TreeNode NewFile = new TreeNode(Path.GetFileName(Filename));
                NewFile.Tag = Path.Combine(BaseFolder, Filename);

                NewFile.ImageIndex = GetIconIndex(Filename);
                NewFile.SelectedImageIndex = GetIconIndex(Filename);

                string Extension = Path.GetExtension(Filename).ToUpper();
                if (Extension.Length != 0) {
                    SpecialFile(ref NewFile, Extension.Substring(1));
                }

                BaseNode.Nodes.Add(NewFile);
            }
        }



        public bool IsSpecialSubFile(TreeNode ToCheck) {
            return (ToCheck.Tag.GetType() != typeof(string));
        }

        private void SpecialFile(ref TreeNode ToEnspecialise, string Extension) {
            try {
                string PluginName = Path.Combine(Application.StartupPath, @"Plugins\Files\" + Extension + ".exe");
                if (!File.Exists(PluginName)) return;

                Process P = new Process();
                P.StartInfo.Arguments = "\"" + (string)ToEnspecialise.Tag + "\"";
                P.StartInfo.FileName = PluginName;
                P.StartInfo.UseShellExecute = false;
                P.StartInfo.RedirectStandardOutput = true;
                P.StartInfo.CreateNoWindow = true;
                P.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                P.EnableRaisingEvents = true;
                P.Start();

                DateTime EndTime = DateTime.Now + TimeSpan.FromSeconds(5);
                StringBuilder Response = new StringBuilder();
                while (P.Responding && !P.HasExited && DateTime.Now < EndTime) {
                    Response.Append(P.StandardOutput.ReadToEnd());
                    Application.DoEvents();
                    Thread.Sleep(100);
                }

                string XmlData = Response.ToString();
                if (XmlData == "") return;

                XmlDocument X = new XmlDocument();
                X.LoadXml(XmlData);

                string Program = "Associated Program";
                try {
                    Program = X.DocumentElement.Attributes.GetNamedItem("program").Value.ToString();
                } catch { }

				AddSpecialNodes(ref ToEnspecialise, X.DocumentElement, Program);
            } catch { }
        }
        private void AddSpecialNodes(ref TreeNode BaseNode, XmlNode SpecialNode, string Program) {
            foreach (XmlNode N in SpecialNode.ChildNodes) {
                string Name = "???";
                string Source = "";
                string Args = "";
                bool IsFolder = N.Name.ToUpper() == "FOLDER";

                
                foreach (XmlAttribute A in N.Attributes) {
                    switch (A.Name.ToLower()) {
                        case "name": Name = A.Value.ToString(); break;
                        case "source": Source = A.Value.ToString(); break;
                        case "args": Args = A.Value.ToString(); break;
                    }
                }
                TreeNode SubItem = new TreeNode(Name);
                SubItem.Tag = new SpecialSubFile();
                ((SpecialSubFile)SubItem.Tag).Source = Source;
                ((SpecialSubFile)SubItem.Tag).Args = Args;
                ((SpecialSubFile)SubItem.Tag).Program = Program;
                if (IsFolder) {
                    SubItem.ImageIndex = 1;
                    SubItem.SelectedImageIndex = 1;
                } else {
                    SubItem.ImageIndex = 0;
                    SubItem.SelectedImageIndex = 0;
                }
                ((SpecialSubFile)SubItem.Tag).ImageIndex = SubItem.ImageIndex;
                BaseNode.Nodes.Add(SubItem);
                if (IsFolder) {
                    AddSpecialNodes(ref SubItem, N, Program);
                }
            }
        }


        /// <summary>
        /// Set the root directory of the control and repopulate it
        /// </summary>
        /// <param name="Directory">Directory to switch to</param>
        public void SetRootDirectory(string Directory) {
            RootDirectory = Directory;
            RefreshFolders();
        }

        //IconHandler
        public Bitmap ExtractIcon(string Filename) {
            return IconHandler.IconHandler.IconFromExtension(Path.GetExtension(Filename), IconSize.Small).ToBitmap();
        }

        #region (Old) Icon extraction code:
        /*
        // Win32 voodoo
        [DllImport("gdi32.dll", EntryPoint = "GetObjectA")]
        private static extern int GetObject(IntPtr hObject, int nCount, ref BITMAPDATA lpObject);
        [DllImport("gdi32.dll")]
        private static extern int DeleteObject(IntPtr hObject);
        [DllImport("user32.dll")]
        private static extern int GetIconInfo(int hIcon, ref ICONINFO piconinfo);
        [DllImport("shell32.dll", EntryPoint = "ExtractIconExA")]
        private static extern int ExtractIconEx(string lpszFile, int nIconIndex, ref int phiconLarge, ref int phiconSmall, int nIcons);
        [DllImport("user32.dll")]
        private static extern int DestroyIcon(int hIcon);
        [StructLayout(LayoutKind.Sequential)]
        private struct ICONINFO {
            internal int fIcon;
            internal int xHotspot;
            internal int yHotspot;
            internal IntPtr hbmMask;
            internal IntPtr hbmColor;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct BITMAPDATA { // technically is a BITMAP but renamed to avoid confusion with Bitmap
            internal int bmType;
            internal int bmWidth;
            internal int bmHeight;
            internal int bmWidthBytes;
            internal Int16 bmPlanes;
            internal Int16 bmBitsPixel;
            internal int bmBits;
        }

        /// <summary>
        /// Get the icon representation of a particular file type
        /// </summary>
        /// <param name="Filename">Filename to use</param>
        /// <returns>A 16x16 small XP-compatible icon</returns>

        public Bitmap ExtractIcon(string Filename) {
            Application.DoEvents();
            Filename = Path.GetFileName(Filename);

            if (Path.GetExtension(Filename) == "") return null;
            return ExtractIconFromKeyname(Path.GetExtension(Filename));
        }


        public Bitmap ExtractIconFromKeyname(string Keyname) {

            RegistryKey searchKey = Registry.ClassesRoot.OpenSubKey(Keyname, false);

            if (searchKey == null) return null;


            RegistryKey getDefaultIcon;
            while (true) {
                getDefaultIcon = searchKey.OpenSubKey("DefaultIcon", false);
                if (getDefaultIcon != null) break;
                if (searchKey.ValueCount == 0) return null;
                try {
                    searchKey = Registry.ClassesRoot.OpenSubKey(searchKey.GetValue("").ToString());
                } catch {
                    return null;
                }
                if (searchKey == null) return null;
            }

            string fileDescription = "";
            string iconPath = "";
            try {
                fileDescription = searchKey.GetValue("").ToString();
                iconPath = getDefaultIcon.GetValue("").ToString();
            } catch {
                return null;
            }



            getDefaultIcon.Close();
            searchKey.Close();

            // Now we have that data, we need to convert a "xxxx,0" path into a "xxxx" and a "0"

            //
            return ExtractIconFromIconRefName(iconPath);
        }
        public Bitmap ExtractIconFromIconRefName(string iconPath) {
            Bitmap ReturnBmp = new Bitmap(16, 16);


            string[] getPlainIconDetails = iconPath.Replace("\"", "").Replace("%1", "explorer.exe").Split(',');
            int iconIndex = 0;
            string plainIconName = getPlainIconDetails[0];

            for (int i = 1; i < getPlainIconDetails.Length - 1; ++i) {
                plainIconName += "," + getPlainIconDetails[i];
            }

            if (iconPath.Replace("\"", "").ToUpper().EndsWith(".ICO")) {
                if (getPlainIconDetails.Length != 1) plainIconName += getPlainIconDetails[getPlainIconDetails.Length - 1];
            } else {
                try {
                    iconIndex = Convert.ToInt32(getPlainIconDetails[getPlainIconDetails.Length - 1]);
                } catch { }
            }

            // Now grab the icon:

            int iconLarge = 0;
            int iconSmall = 0;
            if (ExtractIconEx(plainIconName, iconIndex, ref iconLarge, ref iconSmall, 1) > 0) {
                IntPtr iconPtr = new IntPtr(iconSmall);
                Icon iconRes = Icon.FromHandle(iconPtr);


                ICONINFO iconInfoV = new ICONINFO();
                if (GetIconInfo((int)iconRes.Handle.ToInt32(), ref iconInfoV) == 0) return null;

                BITMAPDATA bitmapData = new BITMAPDATA();
                GetObject(iconInfoV.hbmColor, Marshal.SizeOf(bitmapData.GetType()), ref bitmapData);

                //  Is it one of those weird 32-bpp icons? If not, just leave it alone, .NET handles 'em fine


                if (bitmapData.bmBitsPixel != 32) {
                    DeleteObject(iconInfoV.hbmColor);
                    DeleteObject(iconInfoV.hbmMask);
                    ReturnBmp = iconRes.ToBitmap();
                    iconRes.Dispose();
                    if (iconLarge != 0) DestroyIcon(iconLarge);
                    if (iconSmall != 0) DestroyIcon(iconSmall);
                    return ReturnBmp;
                }

                ReturnBmp = Bitmap.FromHbitmap(iconInfoV.hbmColor);

                // Fix the bitmap:
                BitmapData bmData = ReturnBmp.LockBits(new Rectangle(0, 0, ReturnBmp.Width, ReturnBmp.Height), ImageLockMode.ReadOnly, ReturnBmp.PixelFormat);
                Bitmap dstBitmap = new Bitmap(bmData.Width, bmData.Height, bmData.Stride, PixelFormat.Format32bppArgb, bmData.Scan0);
                ReturnBmp.UnlockBits(bmData);

                DeleteObject(iconInfoV.hbmColor);
                DeleteObject(iconInfoV.hbmMask);

                // Check if bitmap has alpha blending:

                bool bitmapHasAlpha = false;
                int checkAlpha = dstBitmap.GetPixel(0, 0).A;

                for (int x = 0; x < dstBitmap.Width; ++x) {
                    for (int y = 0; y < dstBitmap.Height; ++y) {
                        if (dstBitmap.GetPixel(x, y).A != checkAlpha) {
                            bitmapHasAlpha = true;
                            break;
                        }
                    }
                }

                if (bitmapHasAlpha && (dstBitmap != null)) {
                    ReturnBmp = dstBitmap;
                } else {
                    ReturnBmp = iconRes.ToBitmap();
                }
            }
            return ReturnBmp;
        }
        */
        #endregion
    }
}
