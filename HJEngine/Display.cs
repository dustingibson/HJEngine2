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
        private ui.MenuFactory menuFactory;
        private gfx.Graphics graphics;
        private util.Config mainConfig;
        public Game game;

        public Display(Game game) : base(game.width, game.height)
        {
            this.game = game;
            mainConfig = new util.Config("main");
        }

        protected override void OnLoad(EventArgs e)
        {
            game.LoadGL();
            base.OnLoad(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            game.Render();
            SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected void Reload()
        {
            //Dictionary<string,string> configValues = graphics.GetConfigValues();
            //string[] res = configValues["resolution"].Split(',');
            //this.Width = int.Parse(res[0]);
            //this.Height = int.Parse(res[1]);
            //ClientSize = new Size(this.Width, this.Height);
            //this.ClientRectangle = new Rectangle(new Point(0, 0), ClientSize);
            //graphics.size.w = this.Width;
            //graphics.size.h = this.Height;

            //GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
            //this.WindowState = WindowState.
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            //this.RenderFrequency
            game.UpdateFPS(this.RenderFrequency);
            //Console.WriteLine(graphics.fps);
            KeyboardState input = Keyboard.GetState();
            MouseState mouseState = Mouse.GetCursorState();
            Point cPoint = this.PointToClient(new Point(mouseState.X, mouseState.Y));
            game.UpdateMousePoint(cPoint.X, cPoint.Y,
                mouseState.IsButtonDown(MouseButton.Left),
                mouseState.IsButtonDown(MouseButton.Middle),
                mouseState.IsButtonDown(MouseButton.Right) );
            game.Update();
            if(input.IsKeyDown(Key.Escape) || game.DoQuit())
                Exit();
            base.OnUpdateFrame(e);
        }
    }
}
