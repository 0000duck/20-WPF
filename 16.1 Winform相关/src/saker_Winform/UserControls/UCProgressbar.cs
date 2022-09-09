
using System.Drawing;
using System.Windows.Forms;

namespace saker_Winform.UserControls
{
    public class UCProgressBar : ProgressBar//继承ProgressBar所有功能
    {
        public UCProgressBar()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rect = ClientRectangle;
            Graphics g = e.Graphics;

            ProgressBarRenderer.DrawHorizontalBar(g, rect);
            rect.Inflate(-3, -3);
            if (Value > 0)
            {
                var clip = new Rectangle(rect.X, rect.Y, (int)((float)Value / Maximum * rect.Width), rect.Height);
                ProgressBarRenderer.DrawHorizontalChunks(g, clip);
            }

            string text = string.Format("{0}%", Value * 100 / Maximum); ;
            using (var font = new Font(FontFamily.GenericSerif, 9))
            {
                SizeF sz = g.MeasureString(text, font);
                var location = new PointF(rect.Width / 2 - sz.Width / 2, rect.Height / 2 - sz.Height / 2 + 2);
                g.DrawString(text, font, Brushes.Black, location);
            }
        }
    }
}
