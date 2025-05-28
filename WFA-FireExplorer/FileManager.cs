using System;
using System.Collections.Generic;
using System.IO;


public class FileManager
{
	public static IEnumerable<DirInfo> GetSubDirs(string path)
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
