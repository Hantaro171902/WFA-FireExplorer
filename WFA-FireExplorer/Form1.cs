using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

        private TreeNode CreateDirNode(DirectoryInfo dir)
        {
            TreeNode node = new TreeNode(dir.Name)
            {
                Tag = dir.FullName // Store the full path in the tag
            };
            try
            {
                foreach (DirectoryInfo subDir in dir.GetDirectories())
                {
                    // Attempting access
                    try
                    {
                        node.Nodes.Add(CreateDirNode(subDir));
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // Handle access denied exceptions
                        //node.Nodes.Add(new TreeNode(subDir.Name + " (Access Denied)") { ForeColor = Color.Red });
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Handle access denied exceptions
                //node.Nodes.Add(new TreeNode("Access Denied") { ForeColor = Color.Red });
            }
            return node;
        }

        private void NavigateTo(string path)
        {
            // For folders
            var folderIcon = IconHelper.GetFileIcon(path, true);
            icons.Images.Add(path, folderIcon);
            item.ImageKey = dir;

            // For files
            var fileIcon = IconHelper.GetFileIcon(file, false);
            icons.Images.Add(file, fileIcon);
            item.ImageKey = file;

            try
            {
                // Check if directory exists and is accessible
                DirectoryInfo di = new DirectoryInfo(path);

                if (!di.Exists)
                {
                    MessageBox.Show("The directory does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if ((di.Attributes & FileAttributes.System) == FileAttributes.System ||
                    (di.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                {
                    MessageBox.Show("This directory is restricted or hidden.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Clear list before populating
                listView1.Items.Clear();

                // Add folders
                foreach (string dir in Directory.GetDirectories(path))
                {
                    try
                    {
                        DirectoryInfo subDir = new DirectoryInfo(dir);
                        if ((subDir.Attributes & FileAttributes.System) != FileAttributes.System)
                        {
                            ListViewItem item = new ListViewItem(subDir.Name);
                            item.SubItems.Add("Folder");
                            item.Tag = dir;
                            listView1.Items.Add(item);
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // Optionally log or ignore
                    }
                }

                // Add files
                foreach (string file in Directory.GetFiles(path))
                {
                    try
                    {
                        FileInfo fi = new FileInfo(file);
                        ListViewItem item = new ListViewItem(fi.Name);
                        item.SubItems.Add(fi.Extension);
                        item.Tag = file;
                        listView1.Items.Add(item);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // Optionally log or ignore
                    }
                }

                txt_path.Text = path; // Update the path box
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Access to this directory is denied.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
