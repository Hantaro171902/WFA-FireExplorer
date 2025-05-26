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
        private ImageList icons; // Declare the ImageList here

        public Form1()
        {
            InitializeComponent();

            // Initialize the ImageList in the constructor
            icons = new ImageList();
            icons.ImageSize = new Size(16, 16);
            listView1.SmallImageList = icons;
            listView1.View = View.Details;
            listView1.Columns.Add("Name", 200);
            listView1.Columns.Add("Type", 100);
            listView1.FullRowSelect = true;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string homePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); // "C:\Users\<Username>"
            string startupPath = @"C:\";

            LoadDirTree(homePath);     // TreeView shows user home
            NavigateTo(startupPath);   // ListView shows C:\
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
                    try
                    {
                        node.Nodes.Add(CreateDirNode(subDir));
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // Handle access denied exceptions
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Handle access denied exceptions
            }
            return node;
        }

        private void NavigateTo(string path)
        {

            try
            {
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

                listView1.Items.Clear();

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

                            // Add folder icon
                            var folderIcon = IconHelper.GetFileIcon(dir, true);
                            //icons.Images.Add(dir, folderIcon);
                            //item.ImageKey = dir;
                            string ext = Path.GetExtension(dir).ToLower();
                            if (!icons.Images.ContainsKey(ext))
                            {
                                icons.Images.Add(ext, folderIcon);
                            }
                            item.ImageKey = ext;


                            listView1.Items.Add(item);
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // Optionally log or ignore
                    }
                }

                foreach (string file in Directory.GetFiles(path))
                {
                    try
                    {
                        FileInfo fi = new FileInfo(file);
                        ListViewItem item = new ListViewItem(fi.Name);
                        item.SubItems.Add(fi.Extension);
                        item.Tag = file;

                        // Add file icon
                        var fileIcon = IconHelper.GetFileIcon(file, false);
                        //icons.Images.Add(file, fileIcon);
                        //item.ImageKey = file;
                        string ext = Path.GetExtension(file).ToLower();
                        if (!icons.Images.ContainsKey(ext))
                        {
                            icons.Images.Add(ext, fileIcon);
                        }
                        item.ImageKey = ext;


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

        private void btn_back_Click(object sender, EventArgs e)
        {
            if (currHistoryIdx > 0)
            {
                currHistoryIdx--;
                string path = navHistory[currHistoryIdx];
                txt_path.Text = path;
                LoadFilesInListView(path);
            }
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            if (currHistoryIdx < navHistory.Count - 1)
            {
                currHistoryIdx++;
                string path = navHistory[currHistoryIdx];
                txt_path.Text = path;
                LoadFilesInListView(path);
            }
        }




        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag is string path && Directory.Exists(path))
            {
                NavigateTo(path);
            }
        }

        private void LoadFilesInListView(string path)
        {
            listView1.Items.Clear();
            try
            {
                var dir = new DirectoryInfo(path);
                foreach (var file in dir.GetFiles())
                {
                    listView1.Items.Add(new ListViewItem(file.Name));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading folder: " + ex.Message);
            }
        }

        private string GetFullPathFromTreeNode(TreeNode node)
        {
            if (node.Parent == null)
                return node.Text;

            return Path.Combine(GetFullPathFromTreeNode(node.Parent), node.Text);
        }
    }
}
