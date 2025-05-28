using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFA_FireExplorer
{
    public class FilePreviewer
    {
        private PictureBox pictureBox;
        private RichTextBox richTextBox;

        public FilePreviewer(PictureBox pic, RichTextBox rtb)
        {
            this.pictureBox = pic;
            this.richTextBox = rtb;
        }

        public void ShowPreview(string path)
        {
            pictureBox.Visible = false;
            richTextBox.Visible = false;

            if (!File.Exists(path)) return;

            try
            {
                string ext = Path.GetExtension(path).ToLower();
                if (FileManager.IsImage(ext))
                {
                    pictureBox.Image = Image.FromFile(path);
                    pictureBox.Visible = true;
                }
                else if (FileManager.IsText(ext))
                {
                    richTextBox.Text = File.ReadAllText(path, Encoding.UTF8);
                    richTextBox.Visible = true;
                }
                else
                {
                    pictureBox.Image = null; // Clear image if not an image file
                    richTextBox.Text = $"Preview not available for {ext} files.";
                    richTextBox.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Preview failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
