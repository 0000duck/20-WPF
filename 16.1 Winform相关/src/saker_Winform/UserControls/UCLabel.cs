using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace saker_Winform.UserControls
{
    public partial class UCLabel : Label
    {
        public int RotateAngle { get; set; }
        public string NewText { get; set; } 
        public UCLabel()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Brush b = new SolidBrush(this.ForeColor);
            pe.Graphics.TranslateTransform(25, 10);
            pe.Graphics.RotateTransform(this.RotateAngle);
            pe.Graphics.DrawString(this.NewText, this.Font, b, 0f, 0f);
            base.OnPaint(pe);
        }
    }
}
