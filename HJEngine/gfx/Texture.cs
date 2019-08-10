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
    class Texture
    {
        public float[] vertices;
        public uint[] indices;
        public int vertexBuffer;
        public int arrayObj;
        public int elementBuffer;
        public int texHandle;
        public string shaderName;
        protected gfx.Graphics graphics;

        public Texture(gfx.Graphics graphics, string shaderName, float[] vertices, uint[] indices)
        {
            this.vertices = vertices;
            this.indices = indices;
            this.graphics = graphics;
            this.shaderName = shaderName;
        }

        private Matrix4 Ortho(float left, float right, float bottom, float top, float zNear, float zFar)
        {
            Matrix4 result = Matrix4.Identity;
            result[0, 0] = 2f / (right - left);
            result[1, 1] = 2f / (top - bottom);
            result[2, 2] = -2f / (zFar - zNear);
            result[0, 3] = -(right + left) / (right - left);
            result[1, 3] = -(top + bottom) / (top - bottom);
            result[2, 3] = -(zFar + zNear) / (zFar - zNear);
            return result;
        }

        protected void SetProjections(Shader shader)
        {
            Matrix4 model = Matrix4.Identity;
            Matrix4 proj = Ortho(0f, 1f, 1f, 0f, 0f, 1f);

            shader.SetMatrix4("projection", proj);
            shader.SetMatrix4("model", model);
        }

        protected virtual void ToVAO(Shader shader)
        {

            vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer,
                vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            elementBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer,
                indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);


            int stride = 5;
            this.GenVertexArray();
            var vertexLocation = shader.GetAttributeLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, stride * sizeof(float), 0);
            var texCoordLocation = shader.GetAttributeLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, stride * sizeof(float), 3 * sizeof(float));
            SetProjections(shader);
        }

        public void GenVertexArray()
        {
            arrayObj = GL.GenVertexArray();
            GL.BindVertexArray(arrayObj);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBuffer);
        }


        public void UpdateVertices()
        {
            for(int i = 0; i < 3; i++)
            {
                //float x = this.vertices[i * 3] * (float)Math.Cos(t*0.01745);
                //float y = this.vertices[(i * 3) + 1] + (float)Math.Cos(t * 0.01745);

                //this.vertices[i * 3] = x;
                //this.vertices[i * 3 + 1] = y;
            }
        }

        public virtual void Draw()
        {

        }

        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, texHandle);
        }
    }

    class ImageTexture : Texture
    {
        public ImageTexture(gfx.Graphics graphics, Bitmap bitmap, float[] vertices, uint[] indices) 
            : base(graphics, "texture", vertices, indices)
        {
            this.texHandle = GL.GenTexture();
            this.Use();
            GL.BindTexture(TextureTarget.Texture2D, texHandle);
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0,
                bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width,
                bitmapData.Height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
            bitmap.UnlockBits(bitmapData);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            Shader shader = graphics.shaders.GetShader(shaderName);
            ToVAO(shader);
        }

        protected override void ToVAO(Shader shader)
        {
            base.ToVAO(shader);
        }

        public override void Draw()
        {
            base.Draw();
            Shader shader = graphics.shaders.GetShader(shaderName);
            this.Use();
            shader.Use();
            GL.BindVertexArray(arrayObj);
            GL.BufferData(BufferTarget.ArrayBuffer,
                vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BufferData(BufferTarget.ElementArrayBuffer,
                indices.Length * sizeof(float), indices, BufferUsageHint.StaticDraw);
            GL.DrawElements(PrimitiveType.Triangles, this.indices.Length,
                DrawElementsType.UnsignedInt, 0);
            //GL.BindVertexArray(0);
        }

    }

    class ColorTexture : Texture
    {
        public Color fillColor;
        public Color borderColor;
        public Vector2 borderSize;

        public ColorTexture(gfx.Graphics graphics, Color fillColor, float[] vertices, uint[] indices)
            : base(graphics, "triangle", vertices, indices)
        {
            this.texHandle = GL.GenTexture();
            this.fillColor = fillColor;
            borderSize = new Vector2(0, 0);
            borderColor = Color.FromArgb(0);
            Shader shader = graphics.shaders.GetShader(shaderName);
            ToVAO(shader);
        }

        public ColorTexture(gfx.Graphics graphics, Color fillColor, Color borderColor, prim.Size borderSize, float[] vertices, uint[] indices)
    : base(graphics, "triangle", vertices, indices)
        {
            this.texHandle = GL.GenTexture();
            this.fillColor = fillColor;
            this.borderSize = graphics.normalizeSize(borderSize);
            this.borderColor = borderColor;
            Shader shader = graphics.shaders.GetShader(shaderName);
            ToVAO(shader);
        }

        protected override void ToVAO(Shader shader)
        {
            base.ToVAO(shader);
            shader.SetVec4("fillColor", new Vector4(1.0f, 0.0f, 0.0f, 1.0f));
            shader.SetVec4("borderColor", this.graphics.ColorToVec4(this.borderColor));
            shader.SetVec2("borderSize", borderSize);
        }

        public override void Draw()
        {
            base.Draw();
            Shader shader = graphics.shaders.GetShader(shaderName);
            shader.Use();
            GL.BindVertexArray(arrayObj);
            GL.BufferData(BufferTarget.ArrayBuffer,
                vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BufferData(BufferTarget.ElementArrayBuffer,
                indices.Length * sizeof(float), indices, BufferUsageHint.StaticDraw);
            GL.DrawElements(PrimitiveType.Triangles, this.indices.Length,
                DrawElementsType.UnsignedInt, 0);
            //GL.BindVertexArray(0);
        }
    }
}
