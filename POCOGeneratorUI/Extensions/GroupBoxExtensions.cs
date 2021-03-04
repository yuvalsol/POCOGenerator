using System;
using System.Drawing;

namespace System.Windows.Forms
{
    public static partial class GroupBoxExtensions
    {
        public static void DrawGroupBox(this GroupBox groupBox, Graphics g, FontStyle fontStyle = FontStyle.Regular)
        {
            DrawGroupBox(groupBox, g, SystemColors.Control, SystemColors.ControlText, SystemColors.ActiveBorder, fontStyle);
        }

        public static void DrawGroupBox(this GroupBox groupBox, Graphics g, Color backgroundColor, Color textColor, Color borderColor, FontStyle fontStyle = FontStyle.Regular)
        {
            if (groupBox != null)
            {
                Brush textBrush = new SolidBrush(textColor);
                Brush borderBrush = new SolidBrush(borderColor);
                Pen borderPen = new Pen(borderBrush);

                Font textStyle = new Font(groupBox.Font, fontStyle);
                SizeF textSize = g.MeasureString(groupBox.Text, textStyle);

                Rectangle rect = new Rectangle(
                    groupBox.ClientRectangle.X,
                    groupBox.ClientRectangle.Y + (int)(textSize.Height / 2),
                    groupBox.ClientRectangle.Width - 1,
                    groupBox.ClientRectangle.Height - (int)(textSize.Height / 2) - 1
                );

                // clear text and border
                g.Clear(backgroundColor);

                // text
                int textLeft = (groupBox.ClientRectangle.Width - (int)(textSize.Width)) / 2;
                g.DrawString(groupBox.Text, textStyle, textBrush, textLeft, 0);

                int textPadding = 3;

                // border left
                g.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));

                // border right
                g.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));

                // border bottom
                g.DrawLine(borderPen, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));

                // border top left
                g.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + textLeft - textPadding, rect.Y));

                // border top right
                g.DrawLine(borderPen, new Point(rect.X + textLeft + (textPadding + 1) + (int)(textSize.Width), rect.Y), new Point(rect.X + rect.Width, rect.Y));
            }
        }
    }
}
