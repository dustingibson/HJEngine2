using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace HJEngine.ui
{
    class Button : ui.Component
    {
        private gfx.Texture buttonTexture;

        public Button(gfx.Graphics graphics, string text, prim.Point point, prim.Size size) 
            : base(graphics, "button", text, point, size)
        {


            SizeF textSize = new SizeF();
            Bitmap somebitmap = new Bitmap(32, 32);
            Font font = new Font("Arial", 16);

            using (Graphics g = Graphics.FromImage(somebitmap))
            {
                textSize = g.MeasureString(text, font);
            }

            Bitmap bitmap = new Bitmap((int)textSize.Width, (int)textSize.Height);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                textSize = g.MeasureString(text, font);
                using (Brush brush = new SolidBrush(Color.Red))
                {
                    //g.FillRectangle(brush, 0, 0, 32, 32);
                    g.DrawString(text, font, brush, new PointF(0, 0));
                }
            }

            bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);

            size.w = bitmap.Width / (float)graphics.size.w;
            size.h = bitmap.Height / (float)graphics.size.h;

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


            buttonTexture = new gfx.Texture(bitmap, graphics, "texture", vertices, indices);
        }

        public override void Draw()
        {
            base.Draw();
            buttonTexture.Draw();
        }
    }
}
