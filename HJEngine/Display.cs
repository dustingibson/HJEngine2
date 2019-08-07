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
        private gfx.Texture texture2;
        private ui.Menu testMenu;
        private gfx.ShaderFactory shaders;


        public Display(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {

        }



        protected override void OnLoad(EventArgs e)
        {

            GL.ClearColor(0.3f, 0.3f, 0.3f, 1.0f);

            float[] vertices =
{
                // Position         Texture coordinates
                 0.5f,  0.5f, 0.0f, // top right
                 0.5f, -0.5f, 0.0f, // bottom right
                -0.5f, -0.5f, 0.0f
            };

            uint[] indices =
          {
                0, 2, 1
            };

            //        uint[] indices =
            //{
            //                    // Note that indices start at 0!
            //                    0, 2, 1
            //                };

            //        float[] vertices ={
            //                    0.0f, 0.0f, 0.0f, //Bottom-left vertex
            //                     1.0f, 0.0f, 0.0f, //Bottom-right vertex
            //                     0.5f,  1.0f, 0.0f  //Top vertex
            //                };


            gfx.Graphics graphics = new gfx.Graphics(new prim.Size(Width, Height));

            testMenu = new ui.Menu("main menu", graphics);
            texture = new gfx.Texture(graphics, "triangle", vertices, indices);



            //testMenu = new ui.Menu("main menu", shaders);


            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            //shader.Run();
            texture.Draw();
            testMenu.Draw();
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
