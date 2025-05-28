using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFA_FireExplorer
{
    public class ImageListManager
    {
        public static void EnsureIconForFile(ImageList imageList, string filePath)
        {
            string ext = Path.GetExtension(filePath).ToLower();
            if (!imageList.Images.ContainsKey(ext))
            {
                Icon icon = IconHelper.GetFileIcon(filePath, false);
                imageList.Images.Add(ext, icon);
            }
        }

        public static void EnsureIconForExtension(ImageList imageList, string extension)
        {
            if (!imageList.Images.ContainsKey(extension))
            {
                Icon icon = IconHelper.GetFileIcon("dummy" + extension, false);
                imageList.Images.Add(extension, icon);
            }
        }
    }
}
