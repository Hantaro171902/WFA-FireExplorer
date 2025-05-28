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
        private IconManager iconManager;
        private HistoryManager historyManager;
        private FilePreviewer previewer;

        public Form1()
        {
            InitializeComponent();

            iconManager = new IconManager();
            historyManager = new HistoryManager();
            previewer = new FilePreviewer(pictureBox1, richTextBox1);

            // Initialize the ImageList in the constructor
            icons = new ImageList();
            icons.ImageSize = new Size(16, 16);

            imageList1.Images.Add("folder", SystemIcons.WinLogo); // or your own icon
            imageList1.Images.Add("file", SystemIcons.Application);

            treeView1.ImageList = imageList1;
            listView1.SmallImageList = imageList1;


            listView1.SmallImageList = icons;
            listView1.View = View.Details;
            listView1.Columns.Add("Name", 200);
            listView1.Columns.Add("Type", 100);
            listView1.Columns.Add("Size", 100);
            listView1.Columns.Add("Date Modified", 150);
            listView1.Columns.Add("Path", 200);

            listView1.Columns[4].Width = 0; // Hide the Path column
            listView1.Columns[4].Tag = "Path"; // Store the column name in the Tag property

            listView1.FullRowSelect = true;
            listView1.HideSelection = false;
            listView1.MultiSelect = false;
            listView1.AllowColumnReorder = true;
            listView1.GridLines = true;
            listView1.SmallImageList = icons; // Set the ImageList for the ListView
            listView1.ItemSelectionChanged += (s, e) => { if (e.IsSelected) txt_path.Text = e.Item.Tag as string; };
            listView1.DoubleClick += (s, e) =>
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    string selectedPath = listView1.SelectedItems[0].Tag as string;
                    if (Directory.Exists(selectedPath))
                    {
                        NavigateTo(selectedPath);
                    }
                    else if (File.Exists(selectedPath))
                    {
                        System.Diagnostics.Process.Start(selectedPath); // Open the file
                    }
                }
            };
            
            treeView1.ImageList = icons; // Set the ImageList for the TreeView
            treeView1.ShowLines = true;
            treeView1.ShowPlusMinus = true;
            treeView1.ShowRootLines = true;
            //treeView1.BeforeExpand += treeView1_BeforeExpand;

            var folderIcon = IconHelper.GetFileIcon(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), true);
            icons.Images.Add("folder", folderIcon); // Add a default folder icon

            TreeNode node = new TreeNode("MyFolder");
            node.ImageKey = "folder"; // Use the folder icon
            node.SelectedImageKey = "folder"; // Use the folder icon when selected

            ListViewItem item = new ListViewItem("MyFile.txt");
            item.ImageKey = "file"; // Use the file icon

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string homePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); // "C:\Users\<Username>"
            string startupPath = @"C:\";

            LoadDirTree(homePath);     // TreeView shows user home
            NavigateTo(startupPath, addToHistory: true);  // Navigate ListView to C:\ (and save it in history)   // ListView shows C:\
        
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
                Tag = dir.FullName, // Store the full path in the tag
                ImageKey = "folder", // Use the folder icon
                SelectedImageKey = "folder"
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

        private void NavigateTo(string path, bool addToHistory = true)
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
                //txt_path.Text = path;
                NavigateTo(path, addToHistory: false);
            }
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            if (currHistoryIdx < navHistory.Count - 1)
            {
                currHistoryIdx++;
                string path = navHistory[currHistoryIdx];
                //txt_path.Text = path;
                NavigateTo(path, addToHistory: false);
            }
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node?.Tag is string path && Directory.Exists(path))
            {
                e.Node.Nodes.Clear(); // Clear existing nodes
                try
                {
                    var dirInfo = new DirectoryInfo(path);
                    foreach (var subDir in dirInfo.GetDirectories())
                    {
                        var subNode = CreateDirNode(subDir);
                        e.Node.Nodes.Add(subNode);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    // Handle access denied exceptions
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag is string path && Directory.Exists(path))
            {
                NavigateTo(path);   // This automatically adds to history now
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;

            var item = listView1.SelectedItems[0];
            string path = item.Tag.ToString();

            pictureBox1.Visible = false; // Hide the picture box initially
            richTextBox1.Visible = false; // Hide the rich text box initially

            if (File.Exists(path))
            {
                // If it's a file, show its content in the rich text box
                try
                {
                    string ext = Path.GetExtension(path).ToLower();
                    if (ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".bmp")
                    {
                        pictureBox1.Image = Image.FromFile(path);
                        pictureBox1.Visible = true;
                    }
                    else if (ext == ".txt" || ext == ".log" || ext == ".md")
                    {
                        richTextBox1.Text = File.ReadAllText(path);
                        richTextBox1.Visible = true;
                    }
                    else
                    {
                        richTextBox1.Text = "Preview not available";
                        richTextBox1.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //private void LoadFilesInListView(string path)
        //{
        //    listView1.Items.Clear();
        //    try
        //    {
        //        var dir = new DirectoryInfo(path);
        //        foreach (var file in dir.GetFiles())
        //        {
        //            listView1.Items.Add(new ListViewItem(file.Name));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error reading folder: " + ex.Message);
        //    }
        //}

        private string GetFullPathFromTreeNode(TreeNode node)
        {
            if (node.Parent == null)
                return node.Text;

            return Path.Combine(GetFullPathFromTreeNode(node.Parent), node.Text);
        }

        // Find button click event handler
        private void btn_find_Click(object sender, EventArgs e)
        {
            string searchTerm = txt_find.Text.Trim();
            string currentPath = txt_path.Text;

            if (string.IsNullOrEmpty(searchTerm))
            {
                MessageBox.Show("Please enter a something to search .", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Directory.Exists(currentPath))
            {
                MessageBox.Show("The current path does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            listView1.Items.Clear();

            try
            {
                var matchedFiles = Directory.GetFiles(currentPath, "*" + searchTerm + "*", SearchOption.AllDirectories)
                    .Where(file => !new FileInfo(file).Attributes.HasFlag(FileAttributes.System | FileAttributes.Hidden));

                var matchedDirs = Directory.GetDirectories(currentPath, "*" + searchTerm + "*", SearchOption.AllDirectories)
                    .Where(dir => !new DirectoryInfo(dir).Attributes.HasFlag(FileAttributes.System | FileAttributes.Hidden));

                foreach (var file in matchedFiles)
                {
                    FileInfo fi = new FileInfo(file);
                    var item = new ListViewItem(fi.Name);
                    item.SubItems.Add(fi.Extension);
                    item.Tag = file;

                    var fileIcon = IconHelper.GetFileIcon(file, false);
                    if (!icons.Images.ContainsKey(file))
                        icons.Images.Add(file, fileIcon);
                    item.ImageKey = file;

                    listView1.Items.Add(item);
                }

                if (listView1.Items.Count == 0)
                {
                    MessageBox.Show("No files found matching the search term.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while searching: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
    }
}
