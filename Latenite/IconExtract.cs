using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Win32;
using System.IO;
using System.Runtime.InteropServices;




namespace Latenite {

	class IconExtract {


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


        public Bitmap IconBitmap = new Bitmap(Latenite.Properties.Resources.page_white_code);

		public IconExtract(string Filename) {
            if (Path.GetExtension(Filename) == "") return;
			RegistryKey searchKey = Registry.ClassesRoot.OpenSubKey(Path.GetExtension(Filename), false);
			if (searchKey == null) return;


			RegistryKey getDefaultIcon;
			while (true) {
				getDefaultIcon = searchKey.OpenSubKey("DefaultIcon", false);
				if (getDefaultIcon != null) break;
				searchKey = Registry.ClassesRoot.OpenSubKey(searchKey.GetValue("").ToString());
				if (searchKey == null) return;
			}



			string fileDescription = searchKey.GetValue("").ToString();
			string iconPath = getDefaultIcon.GetValue("").ToString();


			getDefaultIcon.Close();
			searchKey.Close();

			// Now we have that data, we need to convert a "xxxx,0" path into a "xxxx" and a "0"

			string[] getPlainIconDetails = iconPath.Replace("\"", "").Split(',');
			int iconIndex = 0;
			string plainIconName = getPlainIconDetails[0];

			for (int i = 1; i < getPlainIconDetails.Length-1; ++i) {
				plainIconName += "," + getPlainIconDetails[i];
			}

			if (iconPath.Replace("\"", "").ToUpper().EndsWith(".ICO")) {
				if (getPlainIconDetails.Length != 1) plainIconName += getPlainIconDetails[getPlainIconDetails.Length - 1];
			} else {
				iconIndex = Convert.ToInt32(getPlainIconDetails[getPlainIconDetails.Length - 1]);
			}

			// Now grab the icon:
			
			int iconLarge = 0;
			int iconSmall = 0;
			if (ExtractIconEx(plainIconName, iconIndex, ref iconLarge, ref iconSmall, 1) > 0) {
				IntPtr iconPtr = new IntPtr(iconSmall);
				Icon iconRes = Icon.FromHandle(iconPtr);


				ICONINFO iconInfoV = new ICONINFO();
				if (GetIconInfo((int)iconRes.Handle.ToInt32(), ref iconInfoV) == 0) return;

				BITMAPDATA bitmapData = new BITMAPDATA();
				GetObject(iconInfoV.hbmColor, Marshal.SizeOf(bitmapData.GetType()), ref bitmapData);

				//  Is it one of those weird 32-bpp icons? If not, just leave it alone, .NET handles 'em fine


				if (bitmapData.bmBitsPixel != 32) {
					DeleteObject(iconInfoV.hbmColor);
					DeleteObject(iconInfoV.hbmMask);
					this.IconBitmap = iconRes.ToBitmap();
					iconRes.Dispose();
					if (iconLarge != 0) DestroyIcon(iconLarge);
					if (iconSmall != 0) DestroyIcon(iconSmall);
					return;
				} 

				this.IconBitmap = Bitmap.FromHbitmap(iconInfoV.hbmColor);
				
				// Fix the bitmap:
				BitmapData bmData = this.IconBitmap.LockBits(new Rectangle(0, 0, this.IconBitmap.Width, this.IconBitmap.Height), ImageLockMode.ReadOnly, this.IconBitmap.PixelFormat);
				Bitmap dstBitmap = new Bitmap(bmData.Width, bmData.Height, bmData.Stride, PixelFormat.Format32bppArgb, bmData.Scan0);
				this.IconBitmap.UnlockBits(bmData);

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
					this.IconBitmap = dstBitmap;
				} else {
					this.IconBitmap = iconRes.ToBitmap();
				}
			}


		}
	}
}
