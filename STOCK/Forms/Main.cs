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
using MATERIAL.MyFunctions;
using MaterialSkin;
using MaterialSkin.Controls;
using POS.PosControls;
using STOCK.Controls;
using UserManagement; // Added using directive

namespace STOCK.Forms
{
    public partial class Main : MaterialForm
    {
        private tb_SYS_USER _currentUser;
        private int collapsedSidebarWidth = 200; // Default collapsed width
        private int expandedSidebarWidth = 250;  // Default expanded width
        private bool isSidebarExpanded = false;
        private FlowLayoutPanel currentExpandedPanel = null; // Track currently expanded panel
        private RoundedButton currentExpandedButton = null; // Track currently expanded button

        public Main()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue800, Primary.Blue900, Primary.Blue500, Accent.LightBlue200, TextShade.WHITE);
            flpMenu.BackColor = Color.FromArgb(245, 245, 245);
            pnlMain.BackColor = Color.White;

            // Set initial sidebar width
            flpMenu.Width = collapsedSidebarWidth;

            // Subscribe to the form's Resize event
            this.Resize += new System.EventHandler(this.Main_Resize);
        }

        public Main(tb_SYS_USER user) : this()
        {
            _currentUser = user;
        }

        SYS_FUNC _func;

        private void Main_Load(object sender, EventArgs e)
        {
            _func = new SYS_FUNC();
            LeftMenu();
            btnCountStock.BackColor = Color.FromArgb(21, 101, 192); // tương đương #3f51b5
            btnCountStock.ForeColor = Color.White;


        }

        private int GetUserRight(string funcCode)
        {
            if (_currentUser == null) return 0;

            SYS_RIGHT sysRight = new SYS_RIGHT();
            var right = sysRight.getRight(_currentUser.UserID, funcCode);

            if (right == null) return 0;
            return right.UserRight ?? 0;
        }

        private void ShowUserControl(UserControl uc)
        {
            pnlMain.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnlMain.Controls.Add(uc);
        }

        // Overload to handle Forms as well
        private void ShowFormDialog(Form frm)
        {
             // Optionally clear the main panel or handle background forms
             // pnlMain.Controls.Clear();
             frm.ShowDialog(); // Show as a modal dialog
        }


        private Image GetIcon(string funcCode)
        {
            string key = funcCode.ToUpper();
            if (imgListMenu.Images.ContainsKey(key))
            {
                return imgListMenu.Images[key];
            }
            // Return a default icon if the key is not found
             if (imgListMenu.Images.ContainsKey("DEFAULT")) // Assuming you have a "DEFAULT" key
             {
                 return imgListMenu.Images["DEFAULT"];
             }
             return null; // Or return a default Bitmap object
        }


        private int CalculateButtonHeight(Button btn, string text, Font font, int width)
        {
            using (Graphics g = btn.CreateGraphics())
            {
                SizeF textSize = g.MeasureString(text, font, width);
                return Math.Max(30, (int)textSize.Height + 10);
            }
        }

        // Method to adjust sidebar width
        private void AdjustSidebarWidth(bool expand)
        {
            isSidebarExpanded = expand;

            // Animate the width change
            Timer resizeTimer = new Timer();
            resizeTimer.Interval = 10;

            int targetWidth = expand ? expandedSidebarWidth : collapsedSidebarWidth;
            int step = expand ? 5 : -5;

            resizeTimer.Tick += (s, e) =>
            {
                if ((expand && flpMenu.Width >= targetWidth) ||
                    (!expand && flpMenu.Width <= targetWidth))
                {
                    flpMenu.Width = targetWidth;
                    resizeTimer.Stop();
                    resizeTimer.Dispose();

                    // Update all buttons width after resize is complete
                    foreach (Control ctrl in flpMenu.Controls)
                    {
                        if (ctrl is RoundedButton)
                        {
                            ctrl.Width = flpMenu.Width - 20;
                        }
                        else if (ctrl is FlowLayoutPanel subPanel)
                        {
                            subPanel.Width = flpMenu.Width - 20;
                            foreach (Control subCtrl in subPanel.Controls)
                            {
                                if (subCtrl is RoundedButton)
                                {
                                    subCtrl.Width = subPanel.Width - 15;
                                }
                            }
                        }
                    }
                }
                else
                {
                    flpMenu.Width += step;
                }
            };

            resizeTimer.Start();
        }

        // Helper method to update widths of controls inside the sidebar
        private void UpdateSidebarControlWidths()
        {
            // Use Invoke to ensure thread safety if called from a different thread (though unlikely here)
            if (flpMenu.InvokeRequired)
            {
                flpMenu.Invoke(new MethodInvoker(UpdateSidebarControlWidths));
                return;
            }

            int availableWidth = flpMenu.ClientSize.Width - flpMenu.Padding.Horizontal; // Use ClientSize for accurate inner width

            foreach (Control ctrl in flpMenu.Controls)
            {
                int controlMarginHorizontal = ctrl.Margin.Horizontal;
                int targetControlWidth = Math.Max(10, availableWidth - controlMarginHorizontal); // Ensure minimum width

                if (ctrl is RoundedButton btn) // Handles Dashboard and Parent buttons
                {
                    btn.Width = targetControlWidth;
                }
                else if (ctrl is FlowLayoutPanel subPanel) // Handles sub-menu panels
                {
                    // Adjust subPanel width considering its own margin within flpMenu
                    subPanel.Width = targetControlWidth;

                    int subPanelAvailableWidth = subPanel.ClientSize.Width - subPanel.Padding.Horizontal;

                    foreach (Control subCtrl in subPanel.Controls)
                    {
                        if (subCtrl is RoundedButton subBtn) // Handles child buttons
                        {
                            int subControlMarginHorizontal = subCtrl.Margin.Horizontal;
                            int targetSubControlWidth = Math.Max(10, subPanelAvailableWidth - subControlMarginHorizontal);

                            // Set max width first if needed, then width
                            subBtn.MaximumSize = new Size(targetSubControlWidth, 0);
                            subBtn.Width = targetSubControlWidth;

                            // Recalculate height based on new width if AutoSize is true for height
                            if (subBtn.AutoSize)
                            {
                                // Ensure text is not empty before measuring
                                string buttonText = subBtn.Text?.TrimStart() ?? string.Empty;
                                if (!string.IsNullOrEmpty(buttonText))
                                {
                                    subBtn.Height = Math.Max(35, CalculateButtonHeight(subBtn, buttonText, subBtn.Font, subBtn.Width - subBtn.Padding.Horizontal - (subBtn.Image != null ? subBtn.Image.Width + 5 : 0))); // Adjust for padding and image
                                }
                                else {
                                    subBtn.Height = 35; // Default height if no text
                                }
                            }
                        }
                    }
                }
            }
        }


        // Method to calculate optimal sidebar width based on menu item text
        private void CalculateOptimalSidebarWidth()
        {
            int maxTextWidth = 0;
            Font parentFont = new Font("Arial", 10, FontStyle.Bold);
            Font childFont = new Font("Arial", 9, FontStyle.Regular);

            // Calculate width needed for parent menu items
            var parentItems = _func.getParent();
            foreach (var item in parentItems)
            {
                using (Graphics g = CreateGraphics())
                {
                    SizeF size = g.MeasureString("     " + item.Description, parentFont);
                    maxTextWidth = Math.Max(maxTextWidth, (int)size.Width + 50); // Add padding for icon and margins
                }
            }

            // Calculate width needed for child menu items
            foreach (var parent in parentItems)
            {
                var childItems = _func.getChild(parent.FUNC_CODE);
                foreach (var child in childItems)
                {
                    using (Graphics g = CreateGraphics())
                    {
                        SizeF size = g.MeasureString("     " + child.Description, childFont);
                        maxTextWidth = Math.Max(maxTextWidth, (int)size.Width + 65); // Add padding for indentation
                    }
                }
            }

            // Set widths based on calculations
            collapsedSidebarWidth = Math.Max(200, maxTextWidth);
            expandedSidebarWidth = Math.Max(250, maxTextWidth + 20);

            // Set initial width
            flpMenu.Width = collapsedSidebarWidth;
        }

        // Method to collapse all submenus except the one being expanded
        private void CollapseOtherMenus(FlowLayoutPanel panelToKeepOpen, RoundedButton buttonToKeepSelected)
        {
            foreach (Control ctrl in flpMenu.Controls)
            {
                // Skip the dashboard button and the panel we want to keep open
                if (ctrl == panelToKeepOpen || ctrl.Name == "Dashboard")
                    continue;

                // If it's a panel, collapse it
                if (ctrl is FlowLayoutPanel panel)
                {
                    panel.Visible = false;
                }

                // If it's a parent button (not the one we're keeping selected), reset its style
                if (ctrl is RoundedButton btn && btn != buttonToKeepSelected && btn.IsParentMenu)
                {
                    btn.BackColor = Color.FromArgb(240, 240, 240);
                    btn.ForeColor = Color.Black;
                    btn.IsExpanded = false;
                }
            }

            // Update tracking variables
            currentExpandedPanel = panelToKeepOpen;
            currentExpandedButton = buttonToKeepSelected;
        }

        void LeftMenu()
        {
            flpMenu.Controls.Clear();

            // Calculate optimal sidebar width based on menu items
            CalculateOptimalSidebarWidth();

            // Set padding for the flowLayoutPanel to create space around buttons
            flpMenu.Padding = new Padding(5);

            // Dashboard button
            RoundedButton btnDashboard = new RoundedButton
            {
                Text = "     Dashboard",
                Width = flpMenu.Width - 20,
                Height = 45,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.FromArgb(0, 99, 177), // Match topbar color
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Name = "Dashboard",
                Image = GetIcon("DASHBOARD"), // Use GetIcon
                ImageAlign = ContentAlignment.MiddleLeft,
                BorderRadius = 10,
                Margin = new Padding(5)
            };
            btnDashboard.Click += BtnDashboard_Click;
            flpMenu.Controls.Add(btnDashboard);

            var _IsParent = _func.getParent();

            foreach (var _pr in _IsParent)
            {
                // Create parent menu button with rounded corners
                RoundedButton btnParent = new RoundedButton
                {
                    Text = "     " + _pr.Description,
                    Width = flpMenu.Width - 20,
                    Height = 45,
                    BackColor = Color.FromArgb(240, 240, 240),
                    ForeColor = Color.Black,
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    Name = _pr.FUNC_CODE,
                    Image = GetIcon(_pr.FUNC_CODE), // Use GetIcon
                    ImageAlign = ContentAlignment.MiddleLeft,
                    TextAlign = ContentAlignment.MiddleLeft,
                    BorderRadius = 10,
                    IsParentMenu = true,
                    Margin = new Padding(5, 5, 5, 0) // Spacing between buttons
                };

                // Create panel for submenu items with proper styling
                FlowLayoutPanel subMenuPanel = new FlowLayoutPanel
                {
                    Width = flpMenu.Width - 20,
                    AutoSize = true,
                    FlowDirection = FlowDirection.TopDown,
                    BackColor = Color.FromArgb(245, 245, 245),
                    Margin = new Padding(10, 0, 5, 5),
                    Padding = new Padding(5, 0, 0, 5),
                    Visible = false, // Initially collapsed
                    Tag = btnParent // Store reference to parent button
                };

                // Track if this parent menu has any visible child menus
                bool hasVisibleChildMenus = false;

                // Get child menu items
                var _IsChild = _func.getChild(_pr.FUNC_CODE);

                foreach (var _ch in _IsChild)
                {
                    int currentUserRight = GetUserRight(_ch.FUNC_CODE);

                    if (currentUserRight > 0)
                    {
                        hasVisibleChildMenus = true;

                        // Create child menu button with rounded corners
                        RoundedButton btnChild = new RoundedButton
                        {
                            Text = "     " + _ch.Description,
                            Width = subMenuPanel.Width - 15,
                            Height = 35,
                            BackColor = Color.FromArgb(240, 240, 240),
                            ForeColor = Color.Black,
                            Font = new Font("Arial", 9, FontStyle.Regular),
                            Name = _ch.FUNC_CODE,
                            Image = GetIcon(_ch.FUNC_CODE), // Use GetIcon
                            ImageAlign = ContentAlignment.MiddleLeft,
                            TextAlign = ContentAlignment.MiddleLeft,
                            AutoSize = false,
                            TextImageRelation = TextImageRelation.ImageBeforeText,
                            BorderRadius = 8,
                            Margin = new Padding(5, 3, 5, 3)
                        };

                        btnChild.MaximumSize = new Size(subMenuPanel.Width - 15, 0);
                        btnChild.AutoSize = true;
                        btnChild.Padding = new Padding(5);
                        btnChild.Height = Math.Max(35, CalculateButtonHeight(btnChild, _ch.Description, btnChild.Font, btnChild.Width));

                        btnChild.Click += (sender, e) =>
                        {
                            // Reset style for all child buttons in this panel
                            foreach (Control ctrl in subMenuPanel.Controls)
                            {
                                if (ctrl is RoundedButton)
                                {
                                    RoundedButton btn = (RoundedButton)ctrl;
                                    btn.BackColor = Color.FromArgb(240, 240, 240);
                                    btn.ForeColor = Color.Black;
                                }
                            }
                             // Highlight the clicked child button
                            RoundedButton clickedChildBtn = (RoundedButton)sender;
                            clickedChildBtn.BackColor = Color.FromArgb(0, 120, 215);
                            clickedChildBtn.ForeColor = Color.White;

                            // Handle specific button clicks
                            if (_ch.FUNC_CODE == "COMPANY")
                            {
                                ShowUserControl(new CompanyControl(_currentUser, currentUserRight));
                            }
                            else if (_ch.FUNC_CODE == "DEPARTMENT")
                            {
                                ShowUserControl(new DepartmentControl(_currentUser, currentUserRight));
                            }
                            else if (_ch.FUNC_CODE == "SUPPLIER")
                            {
                                ShowUserControl(new SupplierControl(_currentUser, currentUserRight));
                            }
                            else if (_ch.FUNC_CODE == "ORIGIN")
                            {
                                ShowUserControl(new OriginControl(_currentUser, currentUserRight));
                            }
                            else if (_ch.FUNC_CODE == "UNIT")
                            {
                                ShowUserControl(new UnitControl(_currentUser, currentUserRight));
                            }
                            else if (_ch.FUNC_CODE == "CATEGORY")
                            {
                                ShowUserControl(new Product_CategoryControl(_currentUser, currentUserRight));
                            }
                            else if (_ch.FUNC_CODE == "PRODUCT")
                            {
                                ShowUserControl(new ProductControl(_currentUser, currentUserRight));
                            }
                            else if (_ch.FUNC_CODE == "PURCHASE INVOICE")
                            {
                                ShowUserControl(new PurchaseInvoiceControl(_currentUser, currentUserRight));
                            }
                            else if (_ch.FUNC_CODE == "INTERNAL EXPORT INVOICE")
                            {
                                ShowUserControl(new InternalDeliveryControl(_currentUser, currentUserRight));
                            }
                            else if (_ch.FUNC_CODE == "INTERNAL RECEIPT INVOICE")
                            {
                                ShowUserControl(new InternalReceipt(_currentUser, currentUserRight));
                            }
                            else if (_ch.FUNC_CODE == "WHOLESALE ORDER")
                            {
                                ShowUserControl(new WholeSale(_currentUser, currentUserRight));
                            }
                            else if (_ch.FUNC_CODE == "PERMISSION")
                            {
                                UserManagement.formMain frmUserManagement = new UserManagement.formMain();
                                frmUserManagement.ShowDialog();
                            }
                            // *** END MODIFIED ***
                            else
                            {
                                MessageBox.Show("Function not available yet!");
                            }
                        };

                        subMenuPanel.Controls.Add(btnChild);
                    }
                }

                if (hasVisibleChildMenus)
                {
                    // Add click handler for parent menu to expand/collapse submenu
                    btnParent.Click += (sender, e) =>
                    {
                        RoundedButton btn = (RoundedButton)sender;
                        bool isExpanded = !subMenuPanel.Visible;

                        if (isExpanded)
                        {
                            // Collapse all other menus first
                            CollapseOtherMenus(subMenuPanel, btn);

                            // Then expand this one
                            btn.IsExpanded = true;
                            subMenuPanel.Visible = true;
                            btn.BackColor = Color.FromArgb(0, 99, 177);
                            btn.ForeColor = Color.White;

                            // Expand sidebar width if needed
                            AdjustSidebarWidth(true);
                        }
                        else
                        {
                            // Collapse this menu
                            btn.IsExpanded = false;
                            subMenuPanel.Visible = false;
                            btn.BackColor = Color.FromArgb(240, 240, 240);
                            btn.ForeColor = Color.Black;

                            // Reset tracking variables
                            currentExpandedPanel = null;
                            currentExpandedButton = null;

                            // Check if any other submenu is expanded
                            bool anyExpanded = false;
                            foreach (Control ctrl in flpMenu.Controls)
                            {
                                if (ctrl is FlowLayoutPanel panel && panel.Visible)
                                {
                                    anyExpanded = true;
                                    break;
                                }
                            }

                            // Collapse sidebar width if no submenu is expanded
                            if (!anyExpanded)
                            {
                                AdjustSidebarWidth(false);
                            }
                        }
                    };

                    flpMenu.Controls.Add(btnParent);
                    flpMenu.Controls.Add(subMenuPanel);
                }
            }
        }

        private void BtnDashboard_Click(object sender, EventArgs e)
        {
            // Collapse all submenus
            foreach (Control ctrl in flpMenu.Controls)
            {
                if (ctrl is FlowLayoutPanel panel)
                {
                    panel.Visible = false;
                }

                if (ctrl is RoundedButton btn && btn.IsParentMenu)
                {
                    btn.BackColor = Color.FromArgb(240, 240, 240);
                    btn.ForeColor = Color.Black;
                    btn.IsExpanded = false;
                }
            }

            // Reset tracking variables
            currentExpandedPanel = null;
            currentExpandedButton = null;

            // Collapse sidebar width
            AdjustSidebarWidth(false);

            // Highlight dashboard button
            RoundedButton dashBtn = (RoundedButton)sender;
            dashBtn.BackColor = Color.FromArgb(0, 120, 215); // Highlight color
            dashBtn.ForeColor = Color.White;

            // Show Dashboard UserControl (Assuming you have one or want to clear pnlMain)
             pnlMain.Controls.Clear(); // Or show a Dashboard UserControl
             // ShowUserControl(new DashBoard()); // Example if you have a Dashboard control
            MessageBox.Show("Dashboard Clicked!"); // Keep or remove this
        }

        private void btnCountStock_Click(object sender, EventArgs e)
        {
            string dpid = "";
            INVENTORY _invent = new INVENTORY();
            if (myFunctions._dpid == "~")
            {
                dpid = "DW";
            }
            else
            {
                dpid = myFunctions._dpid;
            }
            if (_invent.CountStock(dpid, DateTime.Now))
            {
                MessageBox.Show("Update stock successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Update stock failed!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBarcode_Click(object sender, EventArgs e)
        {
            formPrintBarcode frm = new formPrintBarcode();
            frm.ShowDialog();
        }

        // Event handler for the main form's Resize event
        private void Main_Resize(object sender, EventArgs e)
        {
            // Avoid adjustments during minimization or maximization transitions if causing issues,
            // but generally, updating on Normal state is key.
            if (this.WindowState == FormWindowState.Normal || this.WindowState == FormWindowState.Maximized) // Update in both states
            {
                // Determine the target width based on the current state
                int targetWidth = isSidebarExpanded ? expandedSidebarWidth : collapsedSidebarWidth;

                // Check if the form is fully loaded and widths are calculated
                if (expandedSidebarWidth > 0 && collapsedSidebarWidth > 0)
                {
                     // Only update if the width needs changing
                     if (flpMenu.Width != targetWidth)
                     {
                         flpMenu.Width = targetWidth;
                     }

                     // Update the widths of controls inside the sidebar immediately
                     UpdateSidebarControlWidths();
                }
            }
        }
    }
}
