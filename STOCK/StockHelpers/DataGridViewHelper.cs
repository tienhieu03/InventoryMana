using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace STOCK.StockHelpers
{
    public static class DataGridViewHelper
    {
        /// <summary>
        /// Updates all STT (serial number) cells in a DataGridView
        /// </summary>
        /// <param name="dataGridView">DataGridView to update</param>
        /// <param name="columnName">Name of the STT column</param>
        public static void UpdateRowNumbers(DataGridView dataGridView, string columnName = "STT")
        {
            if (dataGridView == null) return;

            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                if (dataGridView.Rows[i].IsNewRow) continue;
                dataGridView.Rows[i].Cells[columnName].Value = i + 1;
            }
        }

        /// <summary>
        /// Ensures there is always an empty row at the bottom of the DataGridView
        /// </summary>
        /// <param name="dataGridView">DataGridView to check</param>
        /// <param name="emptyRowCheckColumn">Column name to check for emptiness</param>
        public static void EnsureEmptyRowAtBottom(DataGridView dataGridView, string emptyRowCheckColumn = "BARCODE")
        {
            try
            {
                // For data-bound grids
                if (dataGridView.DataSource is BindingSource bs)
                {
                    if (bs.DataSource is List<dynamic> detailList)
                    {
                        // Check if last row is empty or if no rows exist
                        bool needsEmptyRow = true;
                        if (detailList.Count > 0)
                        {
                            var lastItem = detailList[detailList.Count - 1];
                            var value = GetPropertyValue(lastItem, emptyRowCheckColumn);
                            needsEmptyRow = !string.IsNullOrEmpty(value?.ToString());
                        }
                        
                        if (needsEmptyRow)
                        {
                            // Add new empty row - this depends on your object structure
                            // This is a simplified example; adapt to your actual class
                            dynamic emptyItem = Activator.CreateInstance(detailList[0].GetType());
                            SetPropertyValue(emptyItem, "STT", detailList.Count + 1);
                            detailList.Add(emptyItem);
                            bs.ResetBindings(false);
                        }
                    }
                }
                else
                {
                    // For unbound grids
                    bool needsEmptyRow = true;
                    
                    // Check if the grid already has a blank row at the end
                    if (dataGridView.Rows.Count > 0)
                    {
                        int lastIndex = dataGridView.Rows.Count - 1;
                        if (dataGridView.Rows[lastIndex].IsNewRow)
                        {
                            needsEmptyRow = false; // DataGridView already has a new row
                        }
                        else
                        {
                            // Check if the last row has an empty specified column
                            var lastRow = dataGridView.Rows[lastIndex];
                            needsEmptyRow = lastRow.Cells[emptyRowCheckColumn].Value != null && 
                                            !string.IsNullOrEmpty(lastRow.Cells[emptyRowCheckColumn].Value.ToString());
                        }
                    }
                    
                    if (needsEmptyRow && !dataGridView.AllowUserToAddRows)
                    {
                        dataGridView.Rows.Add();
                        UpdateRowNumbers(dataGridView);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error ensuring empty bottom row: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the SubTotal column based on Price and QuantityDetail columns
        /// </summary>
        /// <param name="dataGridView">The DataGridView to update</param>
        public static void UpdateAllTotalPrices(DataGridView dataGridView)
        {
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                if (dataGridView.Rows[i].IsNewRow) continue;
                
                try
                {
                    // Try to find the quantity column - it could be Quantity or QuantityDetail
                    object quantityValue = null;
                    if (dataGridView.Columns.Contains("QuantityDetail"))
                        quantityValue = dataGridView.Rows[i].Cells["QuantityDetail"]?.Value;
                    else if (dataGridView.Columns.Contains("Quantity"))
                        quantityValue = dataGridView.Rows[i].Cells["Quantity"]?.Value;
                    
                    object priceValue = dataGridView.Rows[i].Cells["Price"]?.Value;
                    
                    if (quantityValue != null && priceValue != null &&
                        double.TryParse(quantityValue.ToString(), out double quantity) &&
                        double.TryParse(priceValue.ToString(), out double price))
                    {
                        double totalPrice = quantity * price;
                        
                        // Only update if different (to avoid infinite loops with data binding)
                        // Try to find the total price column - it could be TotalPrice or SubTotal
                        object currentTotalPriceValue = null;
                        if (dataGridView.Columns.Contains("SubTotal"))
                            currentTotalPriceValue = dataGridView.Rows[i].Cells["SubTotal"]?.Value;
                        else if (dataGridView.Columns.Contains("TotalPrice"))
                            currentTotalPriceValue = dataGridView.Rows[i].Cells["TotalPrice"]?.Value;
                        
                        double currentTotalPrice = 0;
                        if (currentTotalPriceValue != null)
                        {
                            double.TryParse(currentTotalPriceValue.ToString(), out currentTotalPrice);
                        }
                        
                        if (Math.Abs(currentTotalPrice - totalPrice) > 0.001) // Small tolerance for floating point comparison
                        {
                            // Update the appropriate column
                            if (dataGridView.Columns.Contains("SubTotal"))
                                dataGridView.Rows[i].Cells["SubTotal"].Value = totalPrice;
                            else if (dataGridView.Columns.Contains("TotalPrice"))
                                dataGridView.Rows[i].Cells["TotalPrice"].Value = totalPrice;
                            
                            Console.WriteLine($"Updated total price for row {i}: {totalPrice} = {quantity} * {price}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating total price for row {i}: {ex.Message}");
                }
            }
        }

        // Helper methods for reflecting properties on dynamic objects
        private static object GetPropertyValue(dynamic obj, string propName)
        {
            try
            {
                var type = obj.GetType();
                var prop = type.GetProperty(propName);
                return prop?.GetValue(obj);
            }
            catch
            {
                return null;
            }
        }

        private static void SetPropertyValue(dynamic obj, string propName, object value)
        {
            try
            {
                var type = obj.GetType();
                var prop = type.GetProperty(propName);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(obj, value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting property {propName}: {ex.Message}");
            }
        }
    }
}
