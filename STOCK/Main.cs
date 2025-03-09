using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLayer;
using DataLayer;
using MaterialSkin;
using MaterialSkin.Controls;

namespace STOCK
{
    public partial class Main: MaterialForm
    {
        public Main()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue800, Primary.Blue900, Primary.Blue500, Accent.LightBlue200, TextShade.WHITE);
            flpMenu.BackColor = Color.LightGray; // Màu nền menu bên trái
            pnlMain.BackColor = Color.White; // Màu nền của panel chính

        }
        SYS_FUNC _func;
        private void Main_Load(object sender, EventArgs e)
        {
            _func = new SYS_FUNC();
            leftMenu();
        }

        private Image GetIcon(string funcCode)
        {
            string key = funcCode.ToUpper(); // Chuyển FUNC_CODE thành chữ hoa

            if (imgListMenu.Images.ContainsKey(key))
            {
                return imgListMenu.Images[key]; // Trả về icon tương ứng
            }
            return imgListMenu.Images["default"]; // Trả về icon mặc định nếu không có
        }


        void leftMenu()
        {
            flpMenu.Controls.Clear(); // Xóa menu cũ trước khi load mới

            // 🏠 **Thêm nút Dashboard lên đầu menu**
            Button btnDashboard = new Button
            {
                Text = "     Dashboard",
                Width = flpMenu.Width - 10,
                Height = 40,
                TextAlign = ContentAlignment.MiddleLeft,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.LightBlue,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Name = "Dashboard",
                Image = GetIcon("Dashboard"), // Lấy icon Dashboard nếu có
                ImageAlign = ContentAlignment.MiddleLeft
            };
            btnDashboard.Click += BtnDashboard_Click;
            flpMenu.Controls.Add(btnDashboard);

            var _IsParent = _func.getParent(); // Lấy danh mục cha từ database

            foreach (var _pr in _IsParent)
            {
                // 🔹 **Tạo nút danh mục cha**
                Button btnParent = new Button
                {
                    Text = "     " + _pr.Description, // Thêm khoảng trắng trước chữ để tạo khoảng cách
                    Width = flpMenu.Width - 10,
                    Height = 40,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.LightBlue,
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    Name = _pr.FUNC_CODE,
                    Image = GetIcon(_pr.FUNC_CODE), // Lấy icon từ ImageList
                    ImageAlign = ContentAlignment.MiddleLeft, // Cố định icon bên trái
                    TextAlign = ContentAlignment.MiddleLeft // Chữ nằm sát bên phải icon
                };



                // 🔹 **Tạo panel chứa danh mục con (ẩn ban đầu)**
                FlowLayoutPanel subMenuPanel = new FlowLayoutPanel
                {
                    Width = flpMenu.Width - 10,
                    AutoSize = true,
                    FlowDirection = FlowDirection.TopDown,
                    Visible = false
                };

                var _IsChild = _func.getChild(_pr.FUNC_CODE); // Lấy danh mục con từ database
                foreach (var _ch in _IsChild)
                {
                    Button btnChild = new Button
                    {
                        Text = "     " + _ch.Description, // Thêm khoảng trắng trước chữ
                        Width = flpMenu.Width - 20,
                        Height = 30,
                        FlatStyle = FlatStyle.Flat,
                        BackColor = Color.LightBlue,
                        ForeColor = Color.Black,
                        Font = new Font("Arial", 9, FontStyle.Regular),
                        Name = _ch.FUNC_CODE,
                        Image = GetIcon(_ch.FUNC_CODE), // Lấy icon từ ImageList
                        ImageAlign = ContentAlignment.MiddleLeft, // Icon ở bên trái
                        TextAlign = ContentAlignment.MiddleLeft // Chữ nằm cạnh icon
                    };



                    // Xử lý sự kiện click mở form tương ứng
                    btnChild.Click += (sender, e) => MessageBox.Show("Mở: " + _ch.Description);

                    subMenuPanel.Controls.Add(btnChild); // Thêm vào danh mục con
                }

                // 🎯 **Sự kiện click mở/đóng danh mục con**
                btnParent.Click += (sender, e) =>
                {
                    subMenuPanel.Visible = !subMenuPanel.Visible;
                };

                // 🔹 **Thêm danh mục cha và danh mục con vào menu**
                flpMenu.Controls.Add(btnParent);
                flpMenu.Controls.Add(subMenuPanel);
            }
        }


        private void BtnDashboard_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Dashboard Clicked!");
        }
    }
}
