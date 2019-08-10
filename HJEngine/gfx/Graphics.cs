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
        public Shader shader;
        public prim.Size size;
        public gfx.ShaderFactory shaders;
        public Dictionary<string,PrivateFontCollection> fonts;


        public Graphics(prim.Size size)
        {
            shaders = new gfx.ShaderFactory();

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
