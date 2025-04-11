using System;
using System.Windows.Forms;

namespace STOCK.StockHelpers
{
    public static class FormValidationHelper
    {
        /// <summary>
        /// Validates date range between two date pickers
        /// </summary>
        /// <param name="fromDate">Start date picker control</param>
        /// <param name="toDate">End date picker control</param>
        /// <returns>True if the date range is valid</returns>
        public static bool ValidateDateRange(DateTimePicker fromDate, DateTimePicker toDate)
        {
            if (fromDate.Value > toDate.Value)
            {
                MessageBox.Show("Invalid date range.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates that a date is not in the future
        /// </summary>
        /// <param name="dateControl">The date control to check</param>
        /// <returns>True if the date is valid (not in the future)</returns>
        public static bool ValidateDateNotFuture(DateTimePicker dateControl)
        {
            if (dateControl.Value > DateTime.Now)
            {
                MessageBox.Show("Date cannot be in the future.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dateControl.Value = DateTime.Now;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates that a DataGridView has at least one valid row
        /// </summary>
        /// <param name="dataGridView">DataGridView to validate</param>
        /// <param name="keyColumnName">Column to check for valid data</param>
        /// <returns>True if at least one valid row exists</returns>
        public static bool ValidateGridHasData(DataGridView dataGridView, string keyColumnName = "BARCODE")
        {
            // Check if at least one row has data
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (!row.IsNewRow && 
                    row.Cells[keyColumnName].Value != null && 
                    !string.IsNullOrWhiteSpace(row.Cells[keyColumnName].Value.ToString()))
                {
                    return true;
                }
            }
            
            // No valid rows found
            MessageBox.Show("Please enter invoice details.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
    }
}
