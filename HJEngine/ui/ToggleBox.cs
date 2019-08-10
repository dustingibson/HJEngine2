using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace HJEngine.ui
{
    class ToggleBox : Component
    {

        private Label label;
        private Pane pane;
        //private gfx.Texture labelTexture;
        //private gfx.Texture boxTexture;

        public ToggleBox(gfx.Graphics graphics, Color checkBoxColor, Color borderColor, 
            prim.Size borderSize, string text, int fontSize, string fontType, Color fontColor, 
            prim.Point point, prim.Size size)
                : base(graphics, "label", text, point, size)
        {

            //label = new Label(graphics, text, fontSize, fontType, fontColor, point, new prim.Size(size.w, size.h));

            //size.h = label.size.h;
            //size.w = label.size.h;

            //pane = new Pane(graphics, checkBoxColor, borderColor, borderSize, new prim.Point(point.x, point.y), new prim.Size(label.size.w, label.size.h));
            
            //pane = new Pane(graphics, checkBoxColor, borderColor, borderSize, new prim.Point(point.x, point.y), new prim.Size(size.w, size.h));

        }

        public override void Draw()
        {
            base.Draw();
            pane.Draw();
            //label.Draw();
        }
    }
}
