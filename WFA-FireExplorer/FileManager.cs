using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


public static class FileManager
{
	public static IEnumerable<DirectoryInfo> GetSubDirs(string path)
	{
		var dirInfo = new DirectoryInfo(path);
		return dirInfo.GetDirectories()
						.Where(d => (d.Attributes & FileAttributes.System) == 0);
    }

	public static IEnumerable<FileInfo> GetFiles(string path)
	{
			var dirInfo = new DirectoryInfo(path);
		return dirInfo.GetFiles()
						.Where(f => (f.Attributes & FileAttributes.System) == 0);
    }
}
