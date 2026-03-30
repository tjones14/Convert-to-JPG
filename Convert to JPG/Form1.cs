using System;
using System.Collections.Generic;
using ImageMagick;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.ComponentModel;


namespace HEICtoJPG
{
    public partial class Form1 : Form
    {
        public List<string> fileList;

        public Form1()
        {
            InitializeComponent();
            fileList = [];
            radioPDF.CheckedChanged += RadioPDF_CheckedChanged;
        }

        private void RadioPDF_CheckedChanged(object? sender, EventArgs e)
        {
            checkBoxSingleFile.Enabled = radioPDF.Checked;
        }

        private void DropPanel_DragEnter(object? sender, DragEventArgs e)
        {
            if (e != null && e.Data != null)
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effect = DragDropEffects.Copy;
                }
            }
        }

        private void DropPanel_DragDrop(object? sender, DragEventArgs e)
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
        }

        private void NormalizeImageSize(List<MagickImage> images)
        {
            // Maximum physical dimensions in inches
            const double maxWidthInches = 8.5;
            const double maxHeightInches = 11.0;

            // Standard DPI for screen display
            const double dpi = 96.0;

            // Convert physical dimensions to pixels
            uint maxWidthPixels = (uint)(maxWidthInches * dpi);
            uint maxHeightPixels = (uint)(maxHeightInches * dpi);

            foreach (var image in images)
            {
                uint currentWidth = image.Width;
                uint currentHeight = image.Height;

                // Check if image exceeds either dimension limit
                if (currentWidth > maxWidthPixels || currentHeight > maxHeightPixels)
                {
                    // Calculate scale factors for each dimension
                    double widthScale = (double)maxWidthPixels / currentWidth;
                    double heightScale = (double)maxHeightPixels / currentHeight;

                    // Use the smaller scale factor to maintain aspect ratio
                    double scale = Math.Min(widthScale, heightScale);

                    // Calculate new dimensions
                    uint newWidth = (uint)(currentWidth * scale);
                    uint newHeight = (uint)(currentHeight * scale);

                    // Resize the image maintaining aspect ratio
                    image.Resize(newWidth, newHeight);
                }
            }
        }

        private void Button1_Click(object? sender, EventArgs e)
        {
            if (!radioJPG.Checked && !radioPDF.Checked)
            {
                MessageBox.Show("Please select a type of conversion.");
                return;
            }

            button1.Enabled = false;
            progressSpinner.Visible = true;
            progressSpinner.Value = 0;
            progressSpinner.Maximum = fileList.Count;

            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;

            worker.DoWork += (s, args) =>
            {
                try
                {
                    if (radioJPG.Checked)
                    {
                        ProcessJpgConversion(worker);
                    }

                    if (radioPDF.Checked)
                    {
                        ProcessPdfConversion(worker);
                    }
                }
                catch (Exception ex)
                {
                    args.Result = ex;
                }
            };

            worker.ProgressChanged += (s, args) =>
            {
                progressSpinner.Value = args.ProgressPercentage;
            };

            worker.RunWorkerCompleted += (s, args) =>
            {
                progressSpinner.Visible = false;
                button1.Enabled = true;

                if (args.Result is Exception ex)
                {
                    MessageBox.Show($"Error processing files: {ex.Message}");
                }
                else
                {
                    listBox1.Items.Clear();
                    fileList.Clear();
                    MessageBox.Show("All Files Processed");
                }
            };

            worker.RunWorkerAsync();
        }

        private void ProcessJpgConversion(BackgroundWorker worker)
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

            // Normalize image sizes
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
                    worker.ReportProgress(imageIndex);
                }
            }
        }

        private void ProcessPdfConversion(BackgroundWorker worker)
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

            // Normalize image sizes
            NormalizeImageSize(images);

            if (images.Count > 0)
            {
                var firstFile = fileList[0];
                var outputPath = Path.GetDirectoryName(firstFile) ?? Environment.CurrentDirectory;
                var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                Directory.CreateDirectory(tempDir);

                try
                {
                    if (checkBoxSingleFile.Checked)
                    {
                        // Create single PDF with all images as pages
                        var pdfFileName = Path.Combine(outputPath, "out-combined.pdf");

                        // Create document with letter size (8.5 x 11 inches)
                        Document doc = new Document(PageSize.LETTER);
                        PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(pdfFileName, FileMode.Create));
                        doc.Open();

                        int pageIndex = 0;
                        foreach (var image in images)
                        {
                            // Save image to temporary file
                            var tempImagePath = Path.Combine(tempDir, $"temp_{pageIndex}.png");
                            image.Format = MagickFormat.Png;
                            image.Write(tempImagePath);

                            // Add image to PDF
                            using (var imgStream = File.OpenRead(tempImagePath))
                            {
                                iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(imgStream);

                                // Scale image to fit page with margins
                                float pageWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
                                float pageHeight = doc.PageSize.Height - doc.TopMargin - doc.BottomMargin;

                                pdfImage.ScaleToFit(pageWidth, pageHeight);
                                pdfImage.Alignment = Element.ALIGN_CENTER;

                                doc.Add(pdfImage);
                                doc.NewPage();
                            }

                            pageIndex++;
                            worker.ReportProgress(pageIndex);
                        }

                        doc.Close();
                    }
                    else
                    {
                        // Create individual PDF for each image
                        int fileIndex = 0;
                        foreach (var image in images)
                        {
                            var tempImagePath = Path.Combine(tempDir, $"temp_{fileIndex}.png");
                            image.Format = MagickFormat.Png;
                            image.Write(tempImagePath);

                            // Create document with letter size (8.5 x 11 inches)
                            Document doc = new Document(PageSize.LETTER);
                            var originalFileName = Path.GetFileNameWithoutExtension(fileList[fileIndex]);
                            var pdfFileName = Path.Combine(outputPath, $"out-{originalFileName}.pdf");
                            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(pdfFileName, FileMode.Create));
                            doc.Open();

                            using (var imgStream = File.OpenRead(tempImagePath))
                            {
                                iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(imgStream);

                                // Scale image to fit page with margins
                                float pageWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
                                float pageHeight = doc.PageSize.Height - doc.TopMargin - doc.BottomMargin;

                                pdfImage.ScaleToFit(pageWidth, pageHeight);
                                pdfImage.Alignment = Element.ALIGN_CENTER;

                                doc.Add(pdfImage);
                            }

                            doc.Close();

                            fileIndex++;
                            worker.ReportProgress(fileIndex);
                        }
                    }
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
    }
}

