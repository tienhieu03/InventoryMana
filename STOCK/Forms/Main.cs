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

namespace STOCK.Forms
{
    public partial class Main : MaterialForm
    {
        public Main()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue800, Primary.Blue900, Primary.Blue500, Accent.LightBlue200, TextShade.WHITE);
            flpMenu.BackColor = Color.LightGray;
            pnlMain.BackColor = Color.White;
        }

        SYS_FUNC _func;

        private void Main_Load(object sender, EventArgs e)
        {
            _func = new SYS_FUNC();
            leftMenu();
        }

        private void ShowUserControl(UserControl uc)
        {
            pnlMain.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnlMain.Controls.Add(uc);
        }

        private Image GetIcon(string funcCode)
        {
            string key = funcCode.ToUpper();
            if (imgListMenu.Images.ContainsKey(key))
            {
                return imgListMenu.Images[key];
            }
            return imgListMenu.Images["default"];
        }

        private int CalculateButtonHeight(Button btn, string text, Font font, int width)
        {
            using (Graphics g = btn.CreateGraphics())
            {
                SizeF textSize = g.MeasureString(text, font, width);
                return Math.Max(30, (int)textSize.Height + 10);
            }
        }


        void leftMenu()
        {
            flpMenu.Controls.Clear();

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
                Image = GetIcon("Dashboard"),
                ImageAlign = ContentAlignment.MiddleLeft
            };
            btnDashboard.Click += BtnDashboard_Click;
            flpMenu.Controls.Add(btnDashboard);

            var _IsParent = _func.getParent();

            foreach (var _pr in _IsParent)
            {
                Button btnParent = new Button
                {
                    Text = "     " + _pr.Description,
                    Width = flpMenu.Width - 10,
                    Height = 40,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.LightBlue,
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    Name = _pr.FUNC_CODE,
                    Image = GetIcon(_pr.FUNC_CODE),
                    ImageAlign = ContentAlignment.MiddleLeft,
                    TextAlign = ContentAlignment.MiddleLeft
                };

                FlowLayoutPanel subMenuPanel = new FlowLayoutPanel
                {
                    Width = flpMenu.Width - 10,
                    AutoSize = true,
                    FlowDirection = FlowDirection.TopDown,
                    Visible = false
                };

                var _IsChild = _func.getChild(_pr.FUNC_CODE);
                foreach (var _ch in _IsChild)
                {
                    Button btnChild = new Button
                    {
                        Text = _ch.Description,
                        Width = flpMenu.Width - 20,
                        Height = 30,
                        FlatStyle = FlatStyle.Flat,
                        BackColor = Color.LightBlue,
                        ForeColor = Color.Black,
                        Font = new Font("Arial", 9, FontStyle.Regular),
                        Name = _ch.FUNC_CODE,
                        Image = GetIcon(_ch.FUNC_CODE),
                        ImageAlign = ContentAlignment.MiddleLeft,
                        TextAlign = ContentAlignment.MiddleLeft,
                        AutoSize = false,
                        TextImageRelation = TextImageRelation.ImageBeforeText
                    };

                    btnChild.MaximumSize = new Size(flpMenu.Width - 20, 0);
                    btnChild.AutoSize = true;
                    btnChild.Padding = new Padding(5);
                    btnChild.Height = CalculateButtonHeight(btnChild, _ch.Description, btnChild.Font, btnChild.Width);

                    btnChild.Click += (sender, e) =>
                    {
                        if (_ch.FUNC_CODE == "COMPANY")
                        {
                            //ShowUserControl(new CompanyManagementControl());
                        }
                        else if (_ch.FUNC_CODE == "DEPARTMENT")
                        {
                            ShowUserControl(new DepartmentControl());
                        }
                        else
                        {
                            MessageBox.Show("Function not available yet!");
                        }
                    };

                    subMenuPanel.Controls.Add(btnChild);
                }

                btnParent.Click += (sender, e) =>
                {
                    subMenuPanel.Visible = !subMenuPanel.Visible;
                };

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