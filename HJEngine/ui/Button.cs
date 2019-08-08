using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace HJEngine.ui
{
    class Button : ui.Component
    {
        private ui.Label label;

        public Button(gfx.Graphics graphics, string text, prim.Point point, prim.Size size) 
            : base(graphics, "button", text, point, size)
        {
            label = new Label(graphics, text, point, size);
        }

        public override void Draw()
        {
            base.Draw();
            label.Draw();
        }
    }
}
