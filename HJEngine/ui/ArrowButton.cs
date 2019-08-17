using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace HJEngine.ui
{
    class ArrowButton : Component
    {
        private gfx.ColorTexture texture;
        private prim.MouseOverStateMachine mouseOverState;
        public bool activated;

        public ArrowButton(gfx.Graphics graphics, string dir, Color bgColor, prim.Point point, prim.Size size)
            : base(graphics, "arrow button", "", point, size)
        {
            mouseOverState = new prim.MouseOverStateMachine();
            activated = false;

            float[] lVertices =
            {
               point.x + size.w, point.y + size.h, 0.0f, 1.0f, 1.0f,
               point.x + size.w, point.y, 0.0f, 1.0f, 0.0f,
               point.x, point.y + (size.h/2.0f), 0.0f, 0.0f, 0.5f
            };

            float[] rVertices =
            {
                point.x, point.y, 0.0f, 1.0f, 1.0f,
                point.x, point.y + size.h, 0.0f, 1.0f, 0.0f,
                point.x + size.w, point.y + (size.h / 2.0f), 0.0f, 0.0f, 0.5f
            };

            uint[] indices =
            {
                0,1,2
            };

            if (dir == "right")
            {
                texture = new gfx.ColorTexture(graphics, bgColor, rVertices, indices);
            }
            else
            {
                texture = new gfx.ColorTexture(graphics, bgColor, lVertices, indices);
            }
        }

        public override void Draw()
        {
            base.Draw();
            texture.Draw();
        }

        public override void Update()
        {
            base.Update();
            activated = false;
            if (graphics.mousePoint.x >= this.point.x
                && graphics.mousePoint.x <= this.point.x + this.size.w
                && graphics.mousePoint.y >= this.point.y
                && graphics.mousePoint.y <= this.point.y + this.size.h)
            {
                if (graphics.leftClick.currentState == "clicked")
                {
                    activated = true;
                }
                if (mouseOverState.currentState == "not hover")
                {
                    mouseOverState.TransitionState("on");
                    texture.ChangeBrightness(1.5f);
                }
            }
            else if (mouseOverState.currentState == "hover")
            {
                mouseOverState.TransitionState("off");
                texture.ChangeBrightness(1.0f);
            }
        }
    }
}
