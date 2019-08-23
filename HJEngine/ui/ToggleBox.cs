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
        private Pane selectedPane;
        public bool isChecked;
        //private gfx.Texture labelTexture;
        //private gfx.Texture boxTexture;

        public ToggleBox(gfx.Graphics graphics, string bind, string value,
            Color checkBoxColor, Color borderColor, 
            prim.Size borderSize, string text, int fontSize, string fontType, Color fontColor, 
            prim.Point point, prim.Size size)
                : base(graphics, "label", text, point, size)
        {

            this.bind = bind;
            this.value = value;

            isChecked = this.value == "true" ? true : false;
            label = new Label(graphics, text, fontSize, fontType, fontColor, point, new prim.Size(size.w, size.h));

            size.h = label.size.h;
            size.w = graphics.getWSquareSize(label.size.h);

            pane = new Pane(graphics, checkBoxColor, borderColor, borderSize, new prim.Point(point.x, point.y), new prim.Size(size.w, size.h));

            prim.Size sPaneSize = pane.size.Scale(0.8f);
            prim.Point sPanePnt = pane.point.GetTransPnt(sPaneSize.w * 0.1f, sPaneSize.h * 0.1f);
            selectedPane = new Pane(graphics, borderColor, borderColor, borderSize, sPanePnt, sPaneSize);

            label.point.x += pane.size.w;
            this.Update();
            //pane = new Pane(graphics, checkBoxColor, borderColor, borderSize, new prim.Point(point.x, point.y), new prim.Size(size.w, size.h));

        }

        public override void Update()
        {
            base.Update();
            label.Update();

            if (graphics.mousePoint.x >= this.pane.point.x
                && graphics.mousePoint.x <= this.pane.point.x + this.pane.size.w
                && graphics.mousePoint.y >= this.pane.point.y
                && graphics.mousePoint.y <= this.pane.point.y + this.pane.size.h)
            {
                if (graphics.leftClick.currentState == "clicked")
                {
                    isChecked = isChecked ? false : true;
                }
            }
            this.value = isChecked ? "true" : "false";
        }

        public override void Draw()
        {
            base.Draw();
            pane.Draw();
            if (isChecked)
                selectedPane.Draw();
            label.Draw();
        }
    }
}
