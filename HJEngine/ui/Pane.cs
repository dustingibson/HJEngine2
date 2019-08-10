using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK;

namespace HJEngine.ui
{
    class Pane : Component
    {

        public gfx.ColorTexture paneTexture;
        
        public Pane(gfx.Graphics graphics, Color paneColor, Color borderColor, prim.Size borderSize, prim.Point point, prim.Size size)
            : base(graphics, "pane", "", point, size)
        {
            float[] vertices = {
                 point.x + size.w,  point.y + size.h, 0.0f, 1.0f, 1.0f,  // top right
                 point.x + size.w, point.y, 0.0f, 1.0f, 0.0f,  // bottom right
                point.x, point.y, 0.0f, 0.0f, 0.0f,  // bottom left
                point.x,  point.y + size.h, 0.0f, 0.0f, 1.0f   // top left
            };
            uint[] indices = {  // note that we start from 0!
                0, 1, 3,   // first triangle
                1, 2, 3    // second triangle
            };
            Vector2 borderVec = graphics.normalizeSize(borderSize, this.size);
            paneTexture = new gfx.ColorTexture(graphics, paneColor, borderColor, borderVec, vertices, indices);
        }

        public override void Draw()
        {
            base.Draw();
            paneTexture.Draw();
        }
    }
}
