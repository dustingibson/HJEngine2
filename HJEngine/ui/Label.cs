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
    class Label : Component
    {
        private gfx.ImageTexture labelTexture;

        public Label(gfx.Graphics graphics, string text, int fontSize, string fontType, Color fontColor, prim.Point point, prim.Size size) 
            : base(graphics, "label", text, point, size)
        {
            SizeF textSize = new SizeF();
            Bitmap somebitmap = new Bitmap(32, 32);
            Font font = new Font(graphics.fonts[fontType].Families[0], fontSize, FontStyle.Regular);

            using (Graphics g = Graphics.FromImage(somebitmap))
            {
                textSize = g.MeasureString(text, font);
            }

            Bitmap bitmap = new Bitmap((int)textSize.Width, (int)textSize.Height);

            GraphicsPath fontText = new GraphicsPath();
            bitmap.MakeTransparent();
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                //textSize = g.MeasureString(text, font);
                using (Brush brush = new SolidBrush(fontColor))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
                    g.DrawString(text, font, brush, new PointF(0, 0));
                }
            }

            //bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);

            size.w = bitmap.Width / (float)graphics.size.w;
            size.h = bitmap.Height / (float)graphics.size.h;

            point.x = point.cx ? getCenter(size.w) : point.x;
            point.y = point.cy ? getCenter(size.h) : point.y;

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

            labelTexture = new gfx.ImageTexture( graphics, bitmap, vertices, indices);
        }

        public void HighlightText()
        {
            labelTexture.ChangeColor(1.5f);
        }

        public override void Update()
        {
            float[] vertices = {
                 point.x + size.w,  point.y + size.h, 0.0f, 1.0f, 1.0f,  // top right
                 point.x + size.w, point.y, 0.0f, 1.0f, 0.0f,  // bottom right
                point.x, point.y, 0.0f, 0.0f, 0.0f,  // bottom left
                point.x,  point.y + size.h, 0.0f, 0.0f, 1.0f   // top left
            };
            labelTexture.Update(vertices);
            base.Update();
        }

        public override void Draw()
        {
            base.Draw();
            labelTexture.Draw();
        }
    }
}
