using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace STOCK.Controls
{
    public class RoundedButton : Button
    {
        private int borderRadius = 10;
        private Color borderColor = Color.FromArgb(0, 99, 177); // Blue color matching topbar
        private int borderSize = 1;
        private bool isParentMenu = false;
        private bool isExpanded = false;

        public int BorderRadius
        {
            get { return borderRadius; }
            set { borderRadius = value; Invalidate(); }
        }

        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; Invalidate(); }
        }

        public int BorderSize
        {
            get { return borderSize; }
            set { borderSize = value; Invalidate(); }
        }

        public bool IsParentMenu
        {
            get { return isParentMenu; }
            set { isParentMenu = value; Invalidate(); }
        }

        public bool IsExpanded
        {
            get { return isExpanded; }
            set { isExpanded = value; Invalidate(); }
        }

        public RoundedButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Size = new Size(150, 40);
            BackColor = Color.FromArgb(240, 240, 240); // Light gray
            ForeColor = Color.Black;
            Font = new Font("Arial", 10F, FontStyle.Regular);
            Cursor = Cursors.Hand;
            TextAlign = ContentAlignment.MiddleLeft;
            TextImageRelation = TextImageRelation.ImageBeforeText;
            ImageAlign = ContentAlignment.MiddleLeft;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rectSurface = new Rectangle(0, 0, this.Width, this.Height);
            Rectangle rectBorder = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

            // Rounded button surface
            using (GraphicsPath pathSurface = GetFigurePath(rectSurface, borderRadius))
            using (GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius))
            using (Pen penSurface = new Pen(this.Parent.BackColor, 2))
            using (Pen penBorder = new Pen(borderColor, borderSize))
            {
                // Button surface
                this.Region = new Region(pathSurface);

                // Draw surface border for HD result
                g.DrawPath(penSurface, pathSurface);

                // Button border
                if (borderSize > 0)
                    g.DrawPath(penBorder, pathBorder);

                // Draw arrow for parent menu
                if (isParentMenu)
                {
                    int arrowSize = 8;
                    int arrowX = this.Width - arrowSize - 10;
                    int arrowY = (this.Height - arrowSize) / 2;

                    Point[] arrowPoints;
                    if (isExpanded)
                    {
                        // Down arrow
                        arrowPoints = new Point[]
                        {
                            new Point(arrowX, arrowY),
                            new Point(arrowX + arrowSize, arrowY),
                            new Point(arrowX + arrowSize/2, arrowY + arrowSize)
                        };
                    }
                    else
                    {
                        // Right arrow
                        arrowPoints = new Point[]
                        {
                            new Point(arrowX, arrowY),
                            new Point(arrowX, arrowY + arrowSize),
                            new Point(arrowX + arrowSize, arrowY + arrowSize/2)
                        };
                    }

                    g.FillPolygon(new SolidBrush(ForeColor), arrowPoints);
                }
            }
        }

        private GraphicsPath GetFigurePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            float curveSize = radius * 2F;

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            BackColor = Color.FromArgb(0, 120, 215); // Highlight color
            ForeColor = Color.White;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (IsParentMenu)
            {
                BackColor = IsExpanded ? Color.FromArgb(0, 99, 177) : Color.FromArgb(240, 240, 240);
                ForeColor = IsExpanded ? Color.White : Color.Black;
            }
            else
            {
                BackColor = Color.FromArgb(240, 240, 240); // Light gray
                ForeColor = Color.Black;
            }
        }
    }
}
