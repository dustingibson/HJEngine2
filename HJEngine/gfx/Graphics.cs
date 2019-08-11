using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using System.IO;
using OpenTK;
using System.Drawing.Text;

namespace HJEngine.gfx
{
    class Graphics
    {
        public prim.Size size;
        //public gfx.ShaderFactory shaders;
        public Dictionary<string,PrivateFontCollection> fonts;
        public prim.Point mousePoint;

        public Graphics(prim.Size size)
        {
            //shaders = new gfx.ShaderFactory();
            mousePoint = new prim.Point(0,0);

            this.size = size;
            fonts = new Dictionary<string, PrivateFontCollection>();

            foreach (string fname in Directory.GetFiles("res/fonts"))
            {
                PrivateFontCollection curFonts = new PrivateFontCollection();
                curFonts.AddFontFile(fname);
                fonts.Add(Path.GetFileNameWithoutExtension(fname), curFonts);
            }

        }

        public Vector2 normalizeSize(prim.Size curSize)
        {
            //float aspect = this.size.w / this.size.h;
            return new Vector2(curSize.w / this.size.w, curSize.h / this.size.h);
        }

        public Vector2 normalizeSize(prim.Size childSize, prim.Size parentSize)
        {
            Vector2 parentPixelSize = this.getPixelSize(parentSize);
            return new Vector2(  childSize.w / parentPixelSize[0], 
                                  childSize.h / parentPixelSize[1]);
        }

        public Vector2 getPixelSize(prim.Size curSize)
        {
            return new Vector2(
                curSize.w * this.size.w,
                curSize.h * this.size.h
            );
        }
        
        public float getWSquareSize(float hSize )
        {
            return (this.size.h * hSize) / this.size.w;
        }

        public prim.Point getNormalPoint(prim.Point point)
        {
            return new prim.Point(point.x / this.size.w, point.y / this.size.h);
        }

        public void updateMousePoint(int x, int y)
        {
            mousePoint = getNormalPoint(new prim.Point(x,y));
        }

        public Vector4 ColorToVec4(Color color)
        {
            float r = (float)color.R / 255f;
            float g = (float)color.G / 255f;
            float b = (float)color.B / 255f;
            float a = (float)color.A / 255f;
            return new Vector4(r, g, b, a);
        }
    }
}
