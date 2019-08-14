using System;
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
    class StarField : Texture 
    {
        public StarField(gfx.Graphics graphics)
            : base(graphics, "starfield", new float[] { }, new uint[] { })
        {
            this.texHandle = GL.GenTexture();
            prim.Point pnt = new prim.Point(0.2f, 0.2f);
            prim.Size size = new prim.Size(0.3f, 0.3f);
            //this.vertices = new float[]
            //{
            //                     point.x + size.w,  point.y + size.h, 0.0f, 1.0f, 1.0f,  // top right
            //     point.x + size.w, point.y, 0.0f, 1.0f, 0.0f,  // bottom right
            //    point.x, point.y, 0.0f, 0.0f, 0.0f,  // bottom left
            //    point.x,  point.y + size.h, 0.0f, 0.0f, 1.0f   // top left
            //};
            //this.vertices = new float[] {
            //    pnt.x + size.w, pnt.y + size.h, 0.0f, /*tCoord*/ 1.0f, 1.0f,
            //    pnt.x + size.w, pnt.y, 0.0f, /*tCoord*/ 1.0f, 0.0f,
            //    pnt.x, pnt.y, 0.0f,/*tCoord*/ 0.0f, 0.0f, 
            //    pnt.x, pnt.y + size.h, 0.0f, /*tCoord*/ 0.0f, 1.0f
            //};
            this.vertices = new float[] {
                pnt.x + size.w, pnt.y + size.h, 0.0f, /*tCoord*/ 1.0f, 1.0f, /*color*/ 1.0f, 0.0f, 0.0f, 1.0f,
                pnt.x + size.w, pnt.y, 0.0f, /*tCoord*/ 1.0f, 0.0f, /*color*/ 1.0f, 0.0f, 0.0f, 1.0f,
                pnt.x, pnt.y, 0.0f,/*tCoord*/ 0.0f, 0.0f, /*color*/ 1.0f, 0.0f, 0.0f, 1.0f,
                pnt.x, pnt.y + size.h, 0.0f, /*tCoord*/ 0.0f, 1.0f, /*color*/ 1.0f, 0.0f, 0.0f, 1.0f,
            };
            this.indices = new uint[] {  // note that we start from 0!
                0, 1, 3,   // first triangle
                1, 2, 3    // second triangle
            };
            ToVAO();
        }

        protected override void ToVAO()
        {
            base.ToVAO();
            int stride = 9;
            this.GenVertexArray();
            var vertexLocation = this.shader.GetAttributeLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, stride * sizeof(float), 0);
            var texCoordLocation = this.shader.GetAttributeLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, stride * sizeof(float), 3 * sizeof(float));
            var texColorLocation = this.shader.GetAttributeLocation("aTexColor");
            GL.EnableVertexAttribArray(texColorLocation);
            GL.VertexAttribPointer(texColorLocation, 4, VertexAttribPointerType.Float, false, stride * sizeof(float), 5 * sizeof(float));
            GL.EnableVertexAttribArray(0);
            SetProjections();
            //this.shader.SetVec4(("fillColor"), graphics.ColorToVec4(Color.Red));
            //this.shader.SetFloat("brightness", 1.0f);

        }

        public override void Draw()
        {
            base.Draw();
            this.shader.Use();
            GL.BindVertexArray(arrayObj);
            GL.BufferData(BufferTarget.ArrayBuffer,
                vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BufferData(BufferTarget.ElementArrayBuffer,
                indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            GL.DrawElements(PrimitiveType.Triangles, this.indices.Length,
                DrawElementsType.UnsignedInt, 0);
        }
    }
}
