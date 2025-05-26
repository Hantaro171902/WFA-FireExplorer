namespace WFA_FireExplorer
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.btn_back = new System.Windows.Forms.Button();
            this.btn_next = new System.Windows.Forms.Button();
            this.btn_open = new System.Windows.Forms.Button();
            this.btn_go = new System.Windows.Forms.Button();
            this.lbl_path = new System.Windows.Forms.Label();
            this.txt_path = new System.Windows.Forms.TextBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.listView1 = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // btn_back
            // 
            this.btn_back.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_back.Location = new System.Drawing.Point(39, 33);
            this.btn_back.Name = "btn_back";
            this.btn_back.Size = new System.Drawing.Size(35, 23);
            this.btn_back.TabIndex = 1;
            this.btn_back.Text = "<";
            this.btn_back.UseVisualStyleBackColor = true;
            this.btn_back.Click += new System.EventHandler(this.btn_back_Click);
            // 
            // btn_next
            // 
            this.btn_next.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_next.Location = new System.Drawing.Point(79, 33);
            this.btn_next.Name = "btn_next";
            this.btn_next.Size = new System.Drawing.Size(35, 23);
            this.btn_next.TabIndex = 2;
            this.btn_next.Text = ">";
            this.btn_next.UseVisualStyleBackColor = true;
            this.btn_next.Click += new System.EventHandler(this.btn_next_Click);
            // 
            // btn_open
            // 
            this.btn_open.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_open.Location = new System.Drawing.Point(464, 33);
            this.btn_open.Name = "btn_open";
            this.btn_open.Size = new System.Drawing.Size(113, 23);
            this.btn_open.TabIndex = 3;
            this.btn_open.Text = "Open Manually";
            this.btn_open.UseVisualStyleBackColor = true;
            // 
            // btn_go
            // 
            this.btn_go.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_go.Location = new System.Drawing.Point(416, 33);
            this.btn_go.Name = "btn_go";
            this.btn_go.Size = new System.Drawing.Size(42, 23);
            this.btn_go.TabIndex = 4;
            this.btn_go.Text = "Go";
            this.btn_go.UseVisualStyleBackColor = true;
            // 
            // lbl_path
            // 
            this.lbl_path.AutoSize = true;
            this.lbl_path.Location = new System.Drawing.Point(134, 38);
            this.lbl_path.Name = "lbl_path";
            this.lbl_path.Size = new System.Drawing.Size(32, 13);
            this.lbl_path.TabIndex = 5;
            this.lbl_path.Text = "Path:";
            // 
            // txt_path
            // 
            this.txt_path.Location = new System.Drawing.Point(172, 33);
            this.txt_path.Name = "txt_path";
            this.txt_path.Size = new System.Drawing.Size(223, 20);
            this.txt_path.TabIndex = 7;
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(39, 82);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(121, 310);
            this.treeView1.TabIndex = 8;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // listView1
            // 
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(183, 82);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(394, 310);
            this.listView1.TabIndex = 9;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 438);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.txt_path);
            this.Controls.Add(this.lbl_path);
            this.Controls.Add(this.btn_go);
            this.Controls.Add(this.btn_open);
            this.Controls.Add(this.btn_next);
            this.Controls.Add(this.btn_back);
            this.Controls.Add(this.webBrowser1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Button btn_back;
        private System.Windows.Forms.Button btn_next;
        private System.Windows.Forms.Button btn_open;
        private System.Windows.Forms.Button btn_go;
        private System.Windows.Forms.Label lbl_path;
        private System.Windows.Forms.TextBox txt_path;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ImageList imageList1;
    }
}

