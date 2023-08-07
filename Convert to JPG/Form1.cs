using System;
using System.Collections.Generic;
using ImageMagick;
using System.IO;
using System.Windows.Forms;
using System.Text;
using Spire.Doc;

namespace HEICtoJPG
{
    public partial class Form1 : Form
    {
        public List<string> fileList;

        public Form1()
        {
            InitializeComponent();
            fileList = new List<string>();
        }

        private void DropPanel_DragEnter(object sender, DragEventArgs e)
        {
            if (e != null)
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
            }
        }

        private void DropPanel_DragDrop(object sender, DragEventArgs e)
        {
            if (e != null)
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    fileList.Add(file);
                }
            }
            listBox1.Items.Clear();
            foreach (var item in fileList)
            {
                listBox1.Items.Add(item);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (radioJPG.Checked == true)
            {
                foreach (var file in fileList)
                {
                    if (File.Exists(file))
                    {
                        var fileName = Path.GetFileName(file);
                        var path = Path.GetDirectoryName(file);
                        path = path + @"\out-";

                        using (MagickImage image = new MagickImage(file))
                        {
                            string newfile = file.Replace(Path.GetExtension(file), ".jpg");
                            var newFileName = Path.GetFileName(newfile);
                            var outFile = path + newFileName;
                            image.Write(outFile);
                        }
                    }

                }
            }

            if (radioPDF.Checked == true)
            {
                foreach (var file in fileList)
                {
                    if (File.Exists(file))
                    {
                        var fileName = Path.GetFileName(file);
                        var path = Path.GetDirectoryName(file);
                        //path = path + @"\out-";

                        Document doc = new Document();
                        doc.LoadFromFile(file);

                        string newPdfFile = file.Replace(Path.GetExtension(file), ".pdf");
                        var newPdfFileName = Path.GetFileName(newPdfFile);
                        var outfile = path + @"\" + newPdfFileName;

                        doc.SaveToFile(outfile, FileFormat.PDF);

                    }

                }
            }

            if (radioJPG.Checked==true || radioPDF.Checked == true)
            {
                listBox1.Items.Clear();
                fileList.Clear();
                MessageBox.Show("All Files Processed");
            }
            else
            {
                MessageBox.Show("Please select a type of conversion.");
            }
        }
    }
}
