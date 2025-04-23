using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing; // Added
using System.IO; // Added
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// using Microsoft.Reporting.WinForms; // Removed
using STOCK;

namespace STOCK.PopUpForm
{
    public partial class FormBarcodeRDLC : Form
    {
        private DataTable _barcodeDataToPrint;
        private int _currentRowIndex = 0;
        private Bitmap _previewBitmap; // To hold the preview image

        public FormBarcodeRDLC(DataTable dt)
        {
            InitializeComponent();
            _barcodeDataToPrint = dt; // Store the data
        }

        private void FormBarcodeRDLC_Load(object sender, EventArgs e)
        {
            GeneratePreview(); // Generate and display the preview on load
        }

        private void GeneratePreview()
        {
            if (_barcodeDataToPrint == null || _barcodeDataToPrint.Rows.Count == 0)
            {
                pictureBoxPreview.Image = null;
                return;
            }

            // Use default page settings for preview dimensions
            PageSettings pageSettings = printDocument1.DefaultPageSettings;
            // Create a bitmap with page dimensions minus margins
            int previewWidth = pageSettings.Bounds.Width - pageSettings.Margins.Left - pageSettings.Margins.Right;
            int previewHeight = pageSettings.Bounds.Height - pageSettings.Margins.Top - pageSettings.Margins.Bottom;

            // Ensure minimum dimensions
            previewWidth = Math.Max(previewWidth, 100);
            previewHeight = Math.Max(previewHeight, 100);

            _previewBitmap = new Bitmap(previewWidth, previewHeight);


            using (Graphics graphics = Graphics.FromImage(_previewBitmap))
            {
                graphics.Clear(Color.White); // White background for preview
                // Simulate PrintPageEventArgs for the preview drawing
                Rectangle marginBounds = new Rectangle(0, 0, previewWidth, previewHeight); // Use 0,0 origin for bitmap drawing
                PrintPageEventArgs previewArgs = new PrintPageEventArgs(graphics, marginBounds, pageSettings.Bounds, pageSettings);

                // Draw the first page onto the bitmap
                _currentRowIndex = 0; // Reset index for drawing preview
                DrawBarcodePage(previewArgs); // Use the same drawing logic
                // Note: We ignore e.HasMorePages for the preview bitmap generation
            }

            pictureBoxPreview.Image = _previewBitmap;
        }


        // Reusable drawing logic for both preview and printing
        private void DrawBarcodePage(PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;
            float yPos = e.MarginBounds.Top;
            float xPos = e.MarginBounds.Left;
            float pageBottom = e.MarginBounds.Bottom;
            float pageRight = e.MarginBounds.Right;
            float maxItemHeightInRow = 0;
            bool fontCreatedLocally = false; // Flag to track if we created the font

            // Determine the font to use *before* the loop
            Font drawFont = this.Font;
            if (drawFont == null)
            {
                drawFont = new Font("Arial", 8); // Use a default font
                fontCreatedLocally = true;
            }

            int barcodesPerRow = 3; // Adjust as needed

            try // Use try/finally to ensure font disposal
            {
                while (_currentRowIndex < _barcodeDataToPrint.Rows.Count)
                {
                    DataRow row = _barcodeDataToPrint.Rows[_currentRowIndex];
                    byte[] barcodeImageBytes = (byte[])row["BarcodeImage"];
                    string productName = row["ProductName"].ToString();
                    string price = Convert.ToDecimal(row["Price"]).ToString("N0") + " VNĐ"; // Format price with VNĐ

                    using (MemoryStream ms = new MemoryStream(barcodeImageBytes))
                    using (Bitmap barcodeBitmap = new Bitmap(ms))
                    {
                        float barcodeWidth = barcodeBitmap.Width;
                        float barcodeHeight = barcodeBitmap.Height;
                        // Use the drawFont declared outside the loop
                        float textHeight = graphics.MeasureString(productName + "\n" + price, drawFont).Height;
                        float totalItemHeight = barcodeHeight + textHeight + 5; // Padding

                        // Check horizontal fit
                        if (xPos + barcodeWidth > pageRight)
                        {
                            xPos = e.MarginBounds.Left;
                            yPos += maxItemHeightInRow + 10; // Move down by max height + padding
                            maxItemHeightInRow = 0;
                        }

                        // Check vertical fit
                        if (yPos + totalItemHeight > pageBottom)
                        {
                            e.HasMorePages = true; // More data exists, need another page
                            return; // Stop drawing on this page
                        }

                        // Draw Barcode
                        graphics.DrawImage(barcodeBitmap, xPos, yPos, barcodeWidth, barcodeHeight);

                        // Draw Text
                        float textYPos = yPos + barcodeHeight + 2;
                        RectangleF textRect = new RectangleF(xPos, textYPos, barcodeWidth, textHeight);
                        StringFormat stringFormat = new StringFormat
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Near
                        };
                        graphics.DrawString(productName + "\n" + price, drawFont, Brushes.Black, textRect, stringFormat);

                        // Update position and max height for the row
                        xPos += barcodeWidth + 10; // Add padding
                        maxItemHeightInRow = Math.Max(maxItemHeightInRow, totalItemHeight);
                        _currentRowIndex++; // Move to next data row
                    }
                } // End of using MemoryStream/Bitmap

                // No more data rows left for this page or in total
                if (_currentRowIndex >= _barcodeDataToPrint.Rows.Count)
                {
                     e.HasMorePages = false;
                }
                // If HasMorePages was set to true inside the loop due to vertical overflow,
                // it remains true here, and the loop terminates for this page.

            } // End of while loop
            finally
            {
                // Clean up font only if created locally, after the loop finishes or exits
                if (fontCreatedLocally && drawFont != null)
                {
                    drawFont.Dispose();
                }
            }
        }


        private void btnPrint_Click(object sender, EventArgs e)
        {
             if (_barcodeDataToPrint == null || _barcodeDataToPrint.Rows.Count == 0)
             {
                 MessageBox.Show("No barcode data to print.", "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                 return;
             }
            _currentRowIndex = 0; // Reset index before printing starts
            try
            {
                printDocument1.Print(); // Start the printing process
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Printing failed: {ex.Message}", "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); // Close the form
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            // This event handler is called by the PrintDocument for each page
            DrawBarcodePage(e); // Use the common drawing logic
        }

        // Clean up the preview bitmap when the form is disposed
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            if (_previewBitmap != null)
            {
                _previewBitmap.Dispose();
                _previewBitmap = null;
            }
        }
    }
}
