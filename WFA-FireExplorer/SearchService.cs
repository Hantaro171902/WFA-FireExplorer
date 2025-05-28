using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFA_FireExplorer
{
    public class SearchService
    {
        public static IEnumerable<string> SearchFiles(string root, string pattern)
        {
            return Directory.EnumerateFiles(root, $"*{pattern}*", SearchOption.AllDirectories)
                .Where(f => !new FileInfo(f).Attributes.HasFlag(FileAttributes.System | FileAttributes.Hidden));
        }

        public static IEnumerable<string> SearchFolders(string root, string pattern)
        {
            return Directory.EnumerateDirectories(root, $"*{pattern}*", SearchOption.AllDirectories)
                .Where(d => !new DirectoryInfo(d).Attributes.HasFlag(FileAttributes.System | FileAttributes.Hidden));
        }
    }
}
