using System;
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
        private util.Config mainConfig;
        public Game game;

        public Display(Game game) : 
            base(game.width, game.height, GraphicsMode.Default, "HJ",
                game.fullScreen ? GameWindowFlags.Fullscreen : GameWindowFlags.Default)
        {
            this.Cursor = MouseCursor.Empty;
            //this.CursorVisible = false;
            if (!game.fullScreen)
                this.Y = 100;
            this.game = game;
            mainConfig = new util.Config("main");
        }

        protected override void OnLoad(EventArgs e)
        {
            game.LoadGL();
            base.OnLoad(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            if (!game.DoReload())
                game.SetQuit();
            base.OnClosed(e);
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

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            //game.UpdateKeyBuffer(e.KeyChar.ToString(), (int)e.KeyChar);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            game.UpdateKeyBuffer(e.Key.ToString());
            //game.UpdateActionKeyBuffer(e.ScanCode);
            //Console.WriteLine(string.Format("Key {0}", e.Key.ToString()));
        }

        protected void CheckKey(KeyboardState input)
        {
            Key[] keyPool =
            {
                Key.A,
                Key.S,
                Key.D,
                Key.W
            };
            if(this.Focused)
                foreach(Key curKey in keyPool)
                    if(input.IsKeyDown(curKey))
                        game.UpdateActionKeyBuffer((uint)curKey);

        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState input = Keyboard.GetState();
            game.UpdateFPS(this.RenderFrequency);
            //Console.WriteLine(graphics.fps);
            MouseState mouseState = Mouse.GetCursorState();
            CheckKey(input);
            Point cPoint = this.PointToClient(new Point(mouseState.X, mouseState.Y));
            if (this.Focused)
                game.UpdateMousePoint(cPoint.X, cPoint.Y,
                    mouseState.IsButtonDown(MouseButton.Left),
                    mouseState.IsButtonDown(MouseButton.Middle),
                    mouseState.IsButtonDown(MouseButton.Right) );
            game.Update();
            game.CleanUp();
            if(input.IsKeyDown(Key.Escape) || game.DoQuit() || game.DoReload())
                Exit();
            base.OnUpdateFrame(e);
        }
    }
}
