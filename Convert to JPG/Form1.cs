using System;
using System.Collections.Generic;
using ImageMagick;
using System.IO;
using System.Windows.Forms;
using Spire.Pdf;


namespace HEICtoJPG
{
    public partial class Form1 : Form
    {
        public List<string> fileList;

        public Form1()
        {
            InitializeComponent();
            fileList = [];
        }

        private void DropPanel_DragEnter(object sender, DragEventArgs e)
        {
            if (e != null && e.Data != null)
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effect = DragDropEffects.Copy;
                }
            }
        }

        private void DropPanel_DragDrop(object sender, DragEventArgs e)
        {
            if (e != null && e.Data != null)
            {
                var data = e.Data.GetData(DataFormats.FileDrop);
                if (data is string[] files)
                {
                    foreach (string file in files)
                    {
                        fileList.Add(file);
                    }
                }
            }
            listBox1.Items.Clear();
            foreach (var item in fileList)
            {
                listBox1.Items.Add(item);
            }
            UpdateNormalizeSizeCheckboxState();
        }

        private void UpdateNormalizeSizeCheckboxState()
        {
            chkNormalizeSize.Enabled = fileList.Count > 1;
        }

        private void NormalizeImageSize(List<MagickImage> images)
        {
            if (!chkNormalizeSize.Checked)
            {
                return;
            }

            // Use the size of the first image as the normalized size
            uint normalizedWidth = images[0].Width;
            uint normalizedHeight = images[0].Height;

            for (int i = 1; i < images.Count; i++)
            {
                images[i].Resize(normalizedWidth, normalizedHeight);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (radioJPG.Checked == true)
            {
                List<MagickImage> images = [];
                
                // Load all images first to determine normalization size if needed
                foreach (var file in fileList)
                {
                    if (File.Exists(file))
                    {
                        images.Add(new MagickImage(file));
                    }
                }

                // Normalize image sizes if checkbox is checked
                NormalizeImageSize(images);

                int imageIndex = 0;
                foreach (var file in fileList)
                {
                    if (File.Exists(file) && imageIndex < images.Count)
                    {
                        var fileName = Path.GetFileName(file);
                        var path = Path.GetDirectoryName(file);
                        path += @"\out-";

                        using (MagickImage image = images[imageIndex])
                        {
                            string newfile = file.Replace(Path.GetExtension(file), ".jpg");
                            var newFileName = Path.GetFileName(newfile);
                            var outFile = path + newFileName;
                            image.Write(outFile);
                        }

                        imageIndex++;
                    }
                }
            }

            if (radioPDF.Checked == true)
            {
                List<MagickImage> images = [];
                
                // Load all images first
                foreach (var file in fileList)
                {
                    if (File.Exists(file))
                    {
                        images.Add(new MagickImage(file));
                    }
                }

                // Normalize image sizes if checkbox is checked
                NormalizeImageSize(images);

                // Create PDF from images
                if (images.Count > 0)
                {
                    var firstFile = fileList[0];
                    var outputPath = Path.GetDirectoryName(firstFile) ?? Environment.CurrentDirectory;
                    var pdfFileName = Path.Combine(outputPath, "out-combined.pdf");
                    var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                    Directory.CreateDirectory(tempDir);

                    try
                    {
                        using PdfDocument doc = new();
                        int pageIndex = 0;
                        foreach (var image in images)
                        {
                            // Save image to temporary file
                            var tempImagePath = Path.Combine(tempDir, $"temp_{pageIndex}.png");
                            image.Format = MagickFormat.Png;
                            image.Write(tempImagePath);

                            // Add image to PDF
                            PdfPageBase page = doc.Pages.Add();
                            using (var imgStream = File.OpenRead(tempImagePath))
                            {
                                var pdfImage = Spire.Pdf.Graphics.PdfImage.FromStream(imgStream);
                                page.Canvas.DrawImage(pdfImage, 0, 0, page.GetClientSize().Width, page.GetClientSize().Height);
                            }

                            pageIndex++;
                        }

                        doc.SaveToFile(pdfFileName);
                    }
                    finally
                    {
                        // Clean up temporary files
                        try
                        {
                            Directory.Delete(tempDir, true);
                        }
                        catch
                        {
                            // Ignore cleanup errors
                        }
                    }
                }
            }

            if (radioJPG.Checked==true || radioPDF.Checked == true)
            {
                listBox1.Items.Clear();
                fileList.Clear();
                chkNormalizeSize.Checked = false;
                UpdateNormalizeSizeCheckboxState();
                MessageBox.Show("All Files Processed");
            }
            else
            {
                MessageBox.Show("Please select a type of conversion.");
            }
        }
    }
}

