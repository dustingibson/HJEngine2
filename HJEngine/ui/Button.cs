using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HJEngine.ui
{
    class Button : ui.Component
    {
        private gfx.Texture buttonTexture;

        public Button(string type, string text, prim.Point point, prim.Size size) 
            : base(type, text, point, size)
        {
            float[] vertices = {
                 point.x + size.w,  point.y + size.h, 0.0f,  // top right
                 point.x + size.w, point.y + size.h, 0.0f,  // bottom right
                point.x, point.y + size.h, 0.0f,  // bottom left
                point.x,  point.y, 0.0f   // top left
            };

            uint[] indices = {  // note that we start from 0!
                0, 1, 3,   // first triangle
                1, 2, 3    // second triangle
            };

            buttonTexture = new gfx.Texture(vertices, indices);
        }

        public override void Draw()
        {
            base.Draw();
            buttonTexture.Draw();
        }
    }
}
