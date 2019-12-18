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
        protected Shader shader;

        public Texture(gfx.Graphics graphics, string shaderName, float[] vertices, uint[] indices)
        {
            this.vertices = vertices;
            this.indices = indices;
            this.graphics = graphics;
            this.shader = new Shader(shaderName, graphics.shaders);
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

        protected void SetProjections()
        {
            Matrix4 model = Matrix4.Identity;
            Matrix4 proj = Ortho(0f, 1f, 1f, 0f, 0f, 1f);

            this.shader.SetMatrix4("projection", proj);
            this.shader.SetMatrix4("model", model);
        }

        protected void SetProjections(prim.Point s)
        {
            //Matrix4 model = Matrix4.Identity;
            Matrix4 proj = Ortho(0f + s.x, 1f + s.x, 1f + s.y, 0f + s.y, 0f, 1f);

            this.shader.SetMatrix4("projection", proj);
        }

        protected virtual void ToVAO()
        {

            vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer,
                vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            elementBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer,
                indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

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
            //SetProjections();
        }

        public virtual void Update(float[] vertices)
        {
            this.vertices = vertices;
        }

        public virtual void Update()
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
        private float brightness;

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
            ToVAO();
        }

        protected override void ToVAO()
        {
            base.ToVAO();
            int stride = 5;
            brightness = 1f;
            this.GenVertexArray();
            var vertexLocation = this.shader.GetAttributeLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, stride * sizeof(float), 0);
            var texCoordLocation = this.shader.GetAttributeLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, stride * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(0);
            SetProjections();
        }

        public void ChangeColor(float m)
        {
            this.brightness = m;
        }

        public override void Draw()
        {
            base.Draw();
            this.shader.SetFloat("brightness", this.brightness);
            this.Use();
            this.shader.Use();
            GL.BindVertexArray(arrayObj);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer,
                vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer,
                indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            
            GL.DrawElements(PrimitiveType.Triangles, this.indices.Length,
                DrawElementsType.UnsignedInt, 0);
            //GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        public override void Update(float[] vertices)
        {
            base.Update(vertices);
        }

    }

    class ColorTexture : Texture
    {
        public Color fillColor;
        public Color borderColor;
        public Vector2 borderSize;
        private float brightness;

        public ColorTexture(gfx.Graphics graphics, Color fillColor, float[] vertices, uint[] indices)
            : base(graphics, "triangle", vertices, indices)
        {
            this.texHandle = GL.GenTexture();
            this.fillColor = fillColor;
            borderSize = new Vector2(0, 0);
            borderColor = Color.FromArgb(0);
            brightness = 1f;
            ToVAO();
        }

        public ColorTexture(gfx.Graphics graphics, Color fillColor, Color borderColor, Vector2 borderSize, float[] vertices, uint[] indices)
    : base(graphics, "triangle", vertices, indices)
        {
            this.texHandle = GL.GenTexture();
            this.fillColor = fillColor;
            this.borderSize = borderSize;
            this.borderColor = borderColor;
            brightness = 1f;
            ToVAO();
        }

        protected override void ToVAO()
        {
            base.ToVAO();
            int stride = 5;
            this.GenVertexArray();
            var vertexLocation = this.shader.GetAttributeLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, stride * sizeof(float), 0);
            var texCoordLocation = this.shader.GetAttributeLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, stride * sizeof(float), 3 * sizeof(float));
            SetProjections();
        }

        public void ChangeBrightness(float m)
        {
            this.brightness = m;
        }

        public override void Draw()
        {
            base.Draw();
            this.shader.SetVec4("fillColor", graphics.ColorToVec4(this.fillColor));
            this.shader.SetVec4("borderColor", this.graphics.ColorToVec4(this.borderColor));
            this.shader.SetVec2("borderSize", borderSize);
            this.shader.SetFloat("brightness", this.brightness);
            this.shader.Use();
            GL.BindVertexArray(arrayObj);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer,
                vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer,
                indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            GL.DrawElements(PrimitiveType.Triangles, this.indices.Length,
                DrawElementsType.UnsignedInt, 0);
            //GL.BindVertexArray(0);
        }

        public override void Update(float[] vertices)
        {
            base.Update(vertices);
        }
    }
}
