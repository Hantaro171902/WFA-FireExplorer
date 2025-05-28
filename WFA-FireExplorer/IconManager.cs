using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


public class IconManager
{
	public ImageList Icons { get; set; }

    public IconManager()
	{
		Icons = new ImageList
		{
			ImageSize = new Size(16, 16),
			ColorDepth = ColorDepth.Depth32Bit
		};
    }

	public void AddIcon(string key, string path, bool isFolder)
	{
		if (!Icons.Images.ContainsKey(key))
		{
			var icon = IconHelper.GetFileIcon(path, isFolder);
			if (icon != null)
			{
				Icons.Images.Add(key, icon);
			}
		}
	}

	public string GetKey(string path)
	{
		return Path.GetExtension(path).ToLower();
	}
}
