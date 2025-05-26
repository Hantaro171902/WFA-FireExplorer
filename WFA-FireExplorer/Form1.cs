using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFA_FireExplorer
{
    public partial class Form1 : Form
    {
        private List<string> navHistory = new List<string>();
        private int currHistoryIdx = -1;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string homePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); // "C:\Users\<Username>"
            string startupPath = @"C:\";

            LoadDirTree(homePath);     // TreeView shows user home
            NavigateTo(startupPath);         // ListView shows C:\
        }


        private void LoadDirTree(string rootPath)
        {
            treeView1.Nodes.Clear();
            try
            {
                var rootDirInfo = new DirectoryInfo(rootPath);
                treeView1.Nodes.Add(CreateDirNode(rootDirInfo));
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading directory tree: {ex.Message}");
            }
        }

        private TreeNode CreateDirNode(DirectoryInfo dirInfo)
        {
            var node = new TreeNode(dirInfo.Name) { Tag = dirInfo.FullName };
            try
            {
                foreach (var dir in dirInfo.GetDirectories())
                {
                    node.Nodes.Add(CreateDirNode(dir));
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Handle access denied exceptions
                node.Nodes.Add(new TreeNode("Access Denied") { ForeColor = Color.Red });
            }
            return node;
        }

        private void NavigateTo(string path)
        {
            if (currHistoryIdx >= 0 && currHistoryIdx < navHistory.Count - 1 && 
                navHistory[currHistoryIdx] == path)  { 
                
                return; // Already at the current path
            }

            // Trim forward history if navigating to a new path
            if (currHistoryIdx < navHistory.Count - 1)
            {
                navHistory = navHistory.Take(currHistoryIdx + 1).ToList();
            }

            navHistory.Add(path);
            currHistoryIdx = navHistory.Count - 1;

            txt_path.Text = path;
            LoadFileList(path);
        }

        //private void btn_open_Click(object sender, EventArgs e)
        //{
        //    using (FolderBrowserDialog fbd = new FolderBrowserDialog())
        //    {
        //        fbd.Description = "Select a folder to explore";
        //        if (fbd.ShowDialog() == DialogResult.OK)
        //        {
        //            webBrowser1.Url = new Uri(fbd.SelectedPath);
        //            txt_path.Text = fbd.SelectedPath;
        //            // Here you can add code to handle the selected folder
        //            MessageBox.Show($"Selected folder: {fbd.SelectedPath}");
        //        }
        //    }
        //}

        private void btn_back_Click(object sender, EventArgs e)
        {
            if (currHistoryIdx > 0)
            {
                currHistoryIdx--;
                string path = navHistory[currHistoryIdx];
                txt_path.Text = path;
                LoadFileList(path);
            }
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            if (currHistoryIdx < navHistory.Count - 1)
            {
                currHistoryIdx++;
                string path = navHistory[currHistoryIdx];
                txt_path.Text = path;
                LoadFileList(path);
            }
        }

        //private void btn_go_Click(object sender, EventArgs e)
        //{
        //    string path = txt_path.Text;
        //    if (Directory.Exists(path))
        //    {
        //        NavigateTo(path);
        //    }
        //    else
        //    {
        //        MessageBox.Show("The folder doesn't exist.");
        //    }
        //}

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag is string path && Directory.Exists(path))
            {
                NavigateTo(path);
            }
        }

        private void LoadFileList(string path)
        {
            try
            {
                listView1.Items.Clear();
                var dirInfo = new DirectoryInfo(path);
                foreach (var file in dirInfo.GetFiles())
                {
                    listView1.Items.Add(new ListViewItem(file.Name) { Tag = file.FullName });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading file list: {ex.Message}");
            }
        }

        private string GetFullPath(TreeNode node)
        {
            if (node.Parent == null)
            {
                return node.Tag as string;
            }
            else
            {
                return Path.Combine(GetFullPath(node.Parent), node.Tag as string);
            }
        }
    }
}
