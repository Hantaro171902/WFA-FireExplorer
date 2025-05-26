using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class IconHelper
{
	[DllImport("shell32.dll", CharSet = CharSet.Unicode)]
	private static extern int SHGetFileInfo(string pszPath, uint dwFileAttributes, out SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

	private const uint SHGFI_ICON = 0x000000100; // get icon
	private const uint SHGFI_SMALLICON = 0x000000000; // get small icon
	private const uint SHGFI_LARGEICON = 0x000000000; // get large icon
	private const uint SHGFI_USEFILEATTRIBUTES = 0x000000010; // use passed dwFileAttributes
	private const uint SHGFI_TYPENAME = 0x000000400; // get type name
	private const uint SHGFI_DISPLAYNAME = 0x000000200; // get display name


	pricate const uint FILE_ATTRIBUTE_DIRECTORY = 0x10; // directory attribute
	private const uint FILE_ATTRIBUTE_NORMAL = 0x80; // normal file attribute
	private const uint FILE_ATTRIBUTE_SYSTEM = 0x04; // system file attribute
	private const uint FILE_ATTRIBUTE_HIDDEN = 0x02; // hidden file attribute
	private const uint FILE_ATTRIBUTE_ARCHIVE = 0x20; // archive file attribute

	public static Icon GetIcon(string filePath, bool largeIcon = true)
	{
        SHFILEINFO shinfo = new SHFILEINFO();
        uint attributes = isFolder ? FILE_ATTRIBUTE_DIRECTORY : FILE_ATTRIBUTE_NORMAL;
        uint flags = SHGFI_ICON | SHGFI_SMALLICON | SHGFI_USEFILEATTRIBUTES;

  //      if (System.IO.Directory.Exists(filePath))
		//{
		//	flags |= FILE_ATTRIBUTE_DIRECTORY;
		//}
		//else
		//{
		//	flags |= FILE_ATTRIBUTE_NORMAL;
		//}

        SHGetFileInfo(path, attributes, ref shinfo, (uint)Marshal.SizeOf(shinfo), flags);
        return Icon.FromHandle(shinfo.hIcon);
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct SHFILEINFO
    {
        public IntPtr hIcon;
        public int iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)] public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)] public string szTypeName;
    }

}
