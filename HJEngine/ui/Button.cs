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
        public prim.MouseOverStateMachine mouseState;

        public Button(gfx.Graphics graphics, string text, int fontSize,  string fontType, Color fontColor, prim.Point point, prim.Size size) 
            : base(graphics, "button", text, point, size)
        {
            mouseState = new prim.MouseOverStateMachine();
            label = new Label(graphics, text, fontSize, fontType, fontColor, point, size);
        }

        public override void Draw()
        {
            base.Draw();
            label.Draw();
        }

        public override void Update()
        {
            if (graphics.mousePoint.x >= this.point.x
                && graphics.mousePoint.x <= this.point.x + this.size.w
                && graphics.mousePoint.y >= this.point.y
                && graphics.mousePoint.y <= this.point.y + this.size.h)
            {
                if (mouseState.currentState == "not hover")
                {
                    mouseState.TransitionState("on");
                    label.HighlightText(1.5f);
                }
            }
            else if (mouseState.currentState == "hover")
            {
                mouseState.TransitionState("off");
                label.HighlightText(1.0f);
            }
            label.Update();
        }
    }
}
