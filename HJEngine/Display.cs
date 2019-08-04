using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL4;

namespace HJEngine
{
    class Display : GameWindow
    {
        private gfx.Shader shader;
        private gfx.Texture texture;

        public Display(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {

        }

        private Matrix4 Ortho(float left, float right, float bottom, float top, float zNear, float zFar)
        {
            Matrix4 result = Matrix4.Identity;
            result[0,0] = 2f / (right - left);
            result[1,1] = 2f / (top - bottom);
            result[2,2] = -2f / (zFar - zNear);
            result[0,3] = -(right + left) / (right - left);
            result[1,3] = -(top + bottom) / (top - bottom);
            result[2,3] = -(zFar + zNear) / (zFar - zNear);
            return result;
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.3f, 0.3f, 0.3f, 1.0f);

            uint[] indices =
    {
                    // Note that indices start at 0!
                    0, 2, 1
                };chrome://vivaldi-webui/startpage?section=Speed-dials&activeSpeedDialIndex=0&background-color=#f6f6f6

            float[] vertices ={
                    0.0f, 0.0f, 0.0f, //Bottom-left vertex
                     1.0f, 0.0f, 0.0f, //Bottom-right vertex
                     0.5f,  1.0f, 0.0f  //Top vertex
                };

            shader = new gfx.Shader("triangle");
            Matrix4 model = Matrix4.Identity;
            Matrix4 proj = Ortho(0f, 1f, 0f, 1f, 0f, 1f);
            shader.SetMatrix4("projection", proj);
            shader.SetMatrix4("model", model);
            texture = new gfx.Texture(vertices, indices);

            //base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            shader.Run();
            texture.Draw();
            SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState input = Keyboard.GetState();
            if(input.IsKeyDown(Key.Escape))
            {
                Exit();
            }
            base.OnUpdateFrame(e);
        }
    }
}
