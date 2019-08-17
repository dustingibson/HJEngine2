using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL4;
using OpenTK;
using OpenTK.Graphics;

namespace HJEngine.gfx
{
    class Star
    {
        public Star()
        {

        }
    }

    class StarField : Texture 
    {
        private double counter;
        private prim.VertexStateMachine vertexState;

        public StarField(gfx.Graphics graphics)
            : base(graphics, "starfield", new float[] { }, new uint[] { })
        {
            vertexState = new prim.VertexStateMachine();
            this.counter = 0;
            this.texHandle = GL.GenTexture();
            prim.Size size = new prim.Size(graphics.getWSquareSize(0.05f), 0.05f);
            prim.Point pnt = new prim.Point(0.0f, 0.0f-size.h);

            LoadStarfield();

            //this.vertices = new float[] {
            //    pnt.x + size.w, pnt.y + size.h, 0.0f, /*tCoord*/ 1.0f, 1.0f, /*color*/ 1.0f, 0.0f, 0.0f, 1.0f,/*velocity*/0.15f,
            //    pnt.x + size.w, pnt.y, 0.0f, /*tCoord*/ 1.0f, 0.0f, /*color*/ 1.0f, 0.0f, 0.0f, 1.0f,/*velocity*/0.15f,
            //    pnt.x, pnt.y, 0.0f,/*tCoord*/ 0.0f, 0.0f, /*color*/ 1.0f, 0.0f, 0.0f, 1.0f,/*velocity*/0.15f,
            //    pnt.x, pnt.y + size.h, 0.0f, /*tCoord*/ 0.0f, 1.0f, /*color*/ 1.0f, 0.0f, 0.0f, 1.0f,/*velocity*/0.15f,
            //};
            //this.indices = new uint[] {  // note that we start from 0!
            //    0, 1, 3,   // first triangle
            //    1, 2, 3    // second triangle
            //};
            ToVAO();
        }

        protected void LoadStarfield()
        {
            int numStars = 2000;

            List<Vector4> colors = new List<Vector4>();
            colors.Add(graphics.ColorToVec4(Color.FromArgb(255, 255, 61, 61)));
            colors.Add(graphics.ColorToVec4(Color.FromArgb(255, 249, 255, 84)));
            colors.Add(graphics.ColorToVec4(Color.FromArgb(255, 84, 92, 255)));
            for (int i = 0; i < 7; i++)
                colors.Add(graphics.ColorToVec4(Color.White));

            List<float> data = new List<float>();
            List<uint> indices = new List<uint>();
            GaussianList sizeList = new GaussianList(0.005, 0.01, 1000);
            GaussianList velList = new GaussianList(0.12, 0.03, 1000);
            Random rand = new Random();

            this.counter = rand.NextDouble() * 5000;

            for(int i = 0; i < numStars; i++)
            {
                float gSize = (float)sizeList.GetValue(rand);
                float x = (float)rand.NextDouble();
                float v = (float)velList.GetValue(rand);

                Vector4 color = colors[rand.Next() % colors.Count];
                prim.Size size = new prim.Size(graphics.getWSquareSize(gSize), gSize);

                //Vertices
                data.Add(x + size.w);
                data.Add(0.0f);
                data.Add(0.0f);
                //Tex Coord
                data.Add(1.0f);
                data.Add(1.0f);
                //Color
                data.Add(color[0]);
                data.Add(color[1]);
                data.Add(color[2]);
                data.Add(1.0f);
                //Velocity
                data.Add(v);


                //Vertices
                data.Add(x + size.w);
                data.Add(0.0f - size.h);
                data.Add(0.0f);
                //Tex Coord
                data.Add(1.0f);
                data.Add(0.0f);
                //Color
                data.Add(color[0]);
                data.Add(color[1]);
                data.Add(color[2]);
                data.Add(1.0f);
                //Velocity
                data.Add(v);


                //Vertices
                data.Add(x);
                data.Add(0.0f - size.h);
                data.Add(0.0f);
                //Tex Coord
                data.Add(0.0f);
                data.Add(0.0f);
                //Color
                data.Add(color[0]);
                data.Add(color[1]);
                data.Add(color[2]);
                data.Add(1.0f);
                //Velocity
                data.Add(v);


                //Vertices
                data.Add(x);
                data.Add(0.0f);
                data.Add(0.0f);
                //Tex Coord
                data.Add(0.0f);
                data.Add(1.0f);
                //Color
                data.Add(color[0]);
                data.Add(color[1]);
                data.Add(color[2]);
                data.Add(1.0f);
                //Velocity
                data.Add(v);

                indices.Add((uint)(4*i));
                indices.Add((uint)(4*i+1));
                indices.Add((uint)(4*i+3));
                indices.Add((uint)(4*i+1));
                indices.Add((uint)(4*i+2));
                indices.Add((uint)(4*i+3));
            }
            this.vertices = data.ToArray();
            this.indices = indices.ToArray();
        }

        protected override void ToVAO()
        {
            base.ToVAO();
            int stride = 10;
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
            var velocityLocation = this.shader.GetAttributeLocation("aVelocity");
            GL.EnableVertexAttribArray(velocityLocation);
            GL.VertexAttribPointer(velocityLocation, 1, VertexAttribPointerType.Float, false, stride * sizeof(float), 9 * sizeof(float));
            GL.EnableVertexAttribArray(0);
            SetProjections();
            this.shader.SetFloat("sec", (float)this.graphics.sec);

            //this.shader.SetVec4(("fillColor"), graphics.ColorToVec4(Color.Red));
            //this.shader.SetFloat("brightness", 1.0f);

        }

        public override void Draw()
        {
            base.Draw();
            this.shader.Use();
            GL.BindVertexArray(arrayObj);
            if (this.vertexState.currentState == "change")
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
                GL.BufferData(BufferTarget.ArrayBuffer,
                    vertices.Length * sizeof(float), vertices, BufferUsageHint.DynamicDraw);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBuffer);
                GL.BufferData(BufferTarget.ElementArrayBuffer,
                    indices.Length * sizeof(uint), indices, BufferUsageHint.DynamicDraw);
               this.vertexState.TransitionState("set");
            }
            GL.DrawElements(PrimitiveType.Triangles, this.indices.Length,
                DrawElementsType.UnsignedInt, 0);
        }

        public override void Update()
        {
            counter += graphics.sec;
            this.shader.SetFloat("sec", (float)counter);
        }
    }
}
