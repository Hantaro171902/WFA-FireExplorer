using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


public static class FileManager
{
    private static readonly HashSet<string> ImageExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff", ".webp"
    };

    private static readonly HashSet<string> TextExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        ".txt", ".log", ".md", ".csv", ".json", ".xml", ".ini"
    };

    public static bool IsImage(string ext)
    {
        return ImageExtensions.Contains(ext);
    }

    public static bool IsText(string ext)
    {
        return TextExtensions.Contains(ext);
    }

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

    public static List<FileInfo> GetFilesInDir(string path)
    {
        try
        {
            return Directory.GetFiles(path)
                .Select(f => new FileInfo(f))
                .Where(f => (f.Attributes & FileAttributes.System) == 0)
                .ToList();
        }
        catch (UnauthorizedAccessException)
        {
            return new List<FileInfo>(); // Return empty list if access is denied
        }
    }
}
