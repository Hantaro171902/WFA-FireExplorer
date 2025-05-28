using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFA_FireExplorer
{
    public class HistoryManager
    {
        private List<string> history = new();
        private int index = -1;

        private bool CanGoBack => index > 0;
        private bool CanGoForward => index < history.Count - 1;
        public string Current => index >= 0 ? history[index] : null;

        public void Add(string path)
        {
            if (index == -1 || history[index] != path)
            {
                if (index < history.Count - 1)
                {
                    history = history.Take(index + 1).ToList(); // Remove forward history
                }
            }
            history.Add(path);
            index++;
        }

        public string Back()
        {
            if (CanGoBack)
            {
                index--;
                return Current;
            }
            return null; // No back history
        }

        public string Forward()
        {
            if (CanGoForward)
            {
                index++;
                return Current;
            }
            return null; // No forward history
        }
    }
}
