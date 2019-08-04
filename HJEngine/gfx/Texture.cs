using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace HJEngine.gfx
{
    class Texture
    {
        public float[] vertices;
        public uint[] indices;
        public int vertexBuffer;
        public int arrayObj;
        public int elementBuffer;

        private int t;

        public Texture(float[] vertices, uint[] indices)
        {
            this.vertices = vertices;
            this.indices = indices;
            this.t = 0;

            vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer,
                vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            elementBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer,
                indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);


            //shader.Run();
            arrayObj = GL.GenVertexArray();
            GL.BindVertexArray(arrayObj);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBuffer);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            GL.EnableVertexArrayAttrib(arrayObj, 0);

        }

        public void UpdateVertices()
        {
            if (this.t > 365)
                this.t = 0;
            for(int i = 0; i < 3; i++)
            {
                float x = this.vertices[i * 3] * (float)Math.Cos(t*0.01745);
                float y = this.vertices[(i * 3) + 1] + (float)Math.Cos(t * 0.01745);

                this.vertices[i * 3] = x;
                this.vertices[i * 3 + 1] = y;
            }
            this.t++;
        }

        public void Draw()
        {
            //Console.WriteLine(string.Join(",",this.vertices));
            //UpdateVertices();
            GL.BindVertexArray(vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer,
                vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.DrawElements(PrimitiveType.Triangles, this.indices.Length,
                DrawElementsType.UnsignedInt, 0);
        }
    }
}
