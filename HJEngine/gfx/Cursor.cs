using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL4;
using OpenTK;

namespace HJEngine.gfx
{
    class Cursor
    {
        public prim.Point point;
        public prim.Size size;
        private gfx.ImageTexture cursorTexture;
        private gfx.Graphics graphics;

        public Cursor(gfx.Graphics graphics, string name="cursor")
        {
            this.graphics = graphics;
            string cursorPath = Directory.GetCurrentDirectory() + "/res/img/" + name + ".png";
            Bitmap cursorBmp = new Bitmap(cursorPath);
            NewTexture(cursorBmp);
        }

        public void NewTexture(Bitmap cursorBmp)
        {
            size = new prim.Size(cursorBmp.Width / (float)graphics.size.w,
                cursorBmp.Height / (float)graphics.size.h);
            point = graphics.mousePoint;

            float[] vertices =
            {
                 point.x + size.w,  point.y + size.h, 0.0f, 1.0f, 1.0f,  // top right
                 point.x + size.w, point.y, 0.0f, 1.0f, 0.0f,  // bottom right
                point.x, point.y, 0.0f, 0.0f, 0.0f,  // bottom left
                point.x,  point.y + size.h, 0.0f, 0.0f, 1.0f   // top left
            };

            uint[] indices = {  // note that we start from 0!
                0, 1, 3,   // first triangle
                1, 2, 3    // second triangle
            };

            cursorTexture = new gfx.ImageTexture(graphics, cursorBmp, vertices, indices);
        }

        public void ChangeTexture(Bitmap cursorBmp)
        {
            NewTexture(cursorBmp);
        }

        public void Draw()
        {
            cursorTexture.Draw();
        }

        public void Update()
        {
            point = graphics.mousePoint;

            float[] vertices =
            {
                 point.x + size.w,  point.y + size.h, 0.0f, 1.0f, 1.0f,  // top right
                 point.x + size.w, point.y, 0.0f, 1.0f, 0.0f,  // bottom right
                point.x, point.y, 0.0f, 0.0f, 0.0f,  // bottom left
                point.x,  point.y + size.h, 0.0f, 0.0f, 1.0f   // top left
            };

            uint[] indices = {  // note that we start from 0!
                0, 1, 3,   // first triangle
                1, 2, 3    // second triangle
            };

            cursorTexture.Update(vertices);
        }
    }
}
