using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataModels
{
    public class obj_INVOICE_DETAIL : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private System.Guid _invoiceDetailId;
        public System.Guid InvoiceDetail_ID 
        { 
            get { return _invoiceDetailId; }
            set 
            { 
                if (_invoiceDetailId != value)
                {
                    _invoiceDetailId = value;
                    OnPropertyChanged();
                }
            }
        }

        private Nullable<System.Guid> _invoiceId;
        public Nullable<System.Guid> InvoiceID 
        { 
            get { return _invoiceId; }
            set 
            { 
                if (_invoiceId != value)
                {
                    _invoiceId = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _productId;
        public int ProductID 
        { 
            get { return _productId; }
            set 
            { 
                if (_productId != value)
                {
                    _productId = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _barcode;
        public string BARCODE 
        { 
            get { return _barcode; }
            set 
            { 
                if (_barcode != value)
                {
                    _barcode = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _qrcode;
        public string QRCODE 
        { 
            get { return _qrcode; }
            set 
            { 
                if (_qrcode != value)
                {
                    _qrcode = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _productName;
        public string ProductName 
        { 
            get { return _productName; }
            set 
            { 
                if (_productName != value)
                {
                    _productName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _unit;
        public string Unit 
        { 
            get { return _unit; }
            set 
            { 
                if (_unit != value)
                {
                    _unit = value;
                    OnPropertyChanged();
                }
            }
        }

        private Nullable<int> _quantity;
        public Nullable<int> Quantity 
        { 
            get { return _quantity; }
            set 
            { 
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged();
                    // Auto-update total price when quantity changes
                    UpdateTotalPrice();
                }
            }
        }
        
        // Thêm thuộc tính QuantityDetail để binding với DataGridView
        public Nullable<int> QuantityDetail
        {
            get { return Quantity; }
            set { Quantity = value; }
        }

        private Nullable<double> _price;
        public Nullable<double> Price 
        { 
            get { return _price; }
            set 
            { 
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged();
                    // Auto-update total price when price changes
                    UpdateTotalPrice();
                }
            }
        }

        private Nullable<double> _discountAmount;
        public Nullable<double> DiscountAmount 
        { 
            get { return _discountAmount; }
            set 
            { 
                if (_discountAmount != value)
                {
                    _discountAmount = value;
                    OnPropertyChanged();
                }
            }
        }

        private Nullable<double> _totalPrice;
        public Nullable<double> SubTotal 
        { 
            get { return _totalPrice; }
            set 
            { 
                if (_totalPrice != value)
                {
                    _totalPrice = value;
                    OnPropertyChanged();
                }
            }
        }

        private Nullable<System.DateTime> _day;
        public Nullable<System.DateTime> Day 
        { 
            get { return _day; }
            set 
            { 
                if (_day != value)
                {
                    _day = value;
                    OnPropertyChanged();
                }
            }
        }

        private Nullable<int> _stt;
        public Nullable<int> STT 
        { 
            get { return _stt; }
            set 
            { 
                if (_stt != value)
                {
                    _stt = value;
                    OnPropertyChanged();
                }
            }
        }

        public int? TotalPrice { get; set; }

        // Method to automatically update the TotalPrice property
        private void UpdateTotalPrice()
        {
            if (Quantity.HasValue && Price.HasValue)
            {
                SubTotal = Quantity.Value * Price.Value;
            }
        }
    }
}
