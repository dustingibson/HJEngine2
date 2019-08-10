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
        private gfx.Texture texture;
        private ui.Menu testMenu;

        public Display(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {

        }

        protected override void OnLoad(EventArgs e)
        {
            
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            float[] vertices = {
                // Position         Texture coordinates
                 1.0f,  1.0f, 0.0f, 1.0f, 1.0f, // top right
                 1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
                 0.0f, 0.0f, 0.0f, 0.0f, 0.0f,// bottom right
                0.0f, 1.0f, 0.0f, 0.0f, 1.0f
            };

            uint[] indices =
            {
                0, 1, 3,
                1, 2, 3
            };

            gfx.Graphics graphics = new gfx.Graphics(new prim.Size(Width, Height));

            testMenu = new ui.Menu("main menu", graphics);
            //texture = new gfx.ColorTexture(graphics, System.Drawing.Color.Red, System.Drawing.Color.Blue, new prim.Size(10,10), vertices, indices);

            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            //texture.Draw();
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
