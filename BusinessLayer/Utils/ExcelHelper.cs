using System;
using System.Collections.Generic;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace STOCK.StockHelpers
{
    public class errExport
    {
        public string _barcode { get; set; }
        public int _quantity { get; set; }
        public string _errcode { get; set; }
    }

    public static class ExcelHelper
    {
        /// <summary>
        /// Releases COM objects to prevent memory leaks
        /// </summary>
        /// <param name="obj">The COM object to release</param>
        public static void ReleaseObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                obj = null;
                Console.WriteLine("Error releasing object: " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        /// <summary>
        /// Exports errors to Excel file
        /// </summary>
        /// <param name="errors">List of errors to export</param>
        /// <param name="originalFilename">Original filename for reference</param>
        public static void ExportErrorsToExcel(List<errExport> errors, string originalFilename)
        {
            if (errors == null || errors.Count == 0)
                return;

            string folder = Path.GetDirectoryName(originalFilename);
            string errorFileName = Path.Combine(folder, "Errors_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx");

            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                workbook = excelApp.Workbooks.Add();
                worksheet = workbook.Sheets[1];

                // Add headers
                worksheet.Cells[1, 1] = "Barcode";
                worksheet.Cells[1, 2] = "Quantity";
                worksheet.Cells[1, 3] = "Error";

                // Format headers
                Excel.Range headerRange = worksheet.Range["A1:C1"];
                headerRange.Font.Bold = true;
                headerRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                // Add data rows
                for (int i = 0; i < errors.Count; i++)
                {
                    worksheet.Cells[i + 2, 1] = errors[i]._barcode;
                    worksheet.Cells[i + 2, 2] = errors[i]._quantity;
                    worksheet.Cells[i + 2, 3] = errors[i]._errcode;
                }

                // Auto-fit columns
                worksheet.Columns.AutoFit();

                // Save the workbook
                workbook.SaveAs(errorFileName);

                System.Windows.Forms.MessageBox.Show(
                    $"Import completed with {errors.Count} errors.\nError details saved to: {errorFileName}", 
                    "Import Results", 
                    System.Windows.Forms.MessageBoxButtons.OK, 
                    System.Windows.Forms.MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    $"Failed to export errors: {ex.Message}", 
                    "Error", 
                    System.Windows.Forms.MessageBoxButtons.OK, 
                    System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                if (workbook != null)
                {
                    workbook.Close(true);
                    ReleaseObject(workbook);
                }
                excelApp.Quit();
                ReleaseObject(excelApp);
            }
        }
    }
}
