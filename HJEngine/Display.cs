﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;

namespace HJEngine
{
    class Display : GameWindow
    {
        private gfx.Texture texture;
        private ui.Menu testMenu;
        private gfx.Graphics graphics;

        public Display(int width, int height, string title) : base(width, height, GraphicsMode.Default, title, GameWindowFlags.Default)
        {

        }

        protected override void OnLoad(EventArgs e)
        {
            
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);


            graphics = new gfx.Graphics(new prim.Size(Width, Height));

            testMenu = new ui.Menu("options", graphics);

            base.OnLoad(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
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
            MouseState mouseState = Mouse.GetCursorState();
            Point cPoint = this.PointToClient(new Point(mouseState.X, mouseState.Y));
            graphics.updateMousePoint(cPoint.X, cPoint.Y);

            testMenu.Update();

            if(input.IsKeyDown(Key.Escape))
            {
                Exit();
            }
            base.OnUpdateFrame(e);
        }
    }
}
