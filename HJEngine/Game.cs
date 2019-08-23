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
    class Game
    {
        private ui.MenuFactory menuFactory;
        private gfx.Graphics graphics;
        private util.Config mainConfig;

        public int width;
        public int height;
        public bool fullScreen;
        public bool vsync;

        public Game()
        {
            mainConfig = new util.Config("main");

            string[] res = mainConfig.values["resolution"].Split(',');
            this.width = int.Parse(res[0]);
            this.height = int.Parse(res[1]);
            this.fullScreen = mainConfig.GetBoolValue("fullscreen");
            this.vsync = mainConfig.GetBoolValue("vsync");
        }
        
        public void LoadGL()
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            //if (graphics == null)
           // {
                graphics = new gfx.Graphics(new prim.Size(width, height), mainConfig);
                menuFactory = new ui.MenuFactory(graphics);
                menuFactory.GotoMenu("main menu");
           // }
            graphics.size.w = this.width;
            graphics.size.h = this.height;
        }

        protected void Reload()
        {

            Dictionary<string,string> configValues = graphics.GetConfigValues();
            string[] res = configValues["resolution"].Split(',');
            this.fullScreen = graphics.GetBoolConfigValue("fullscreen");
            this.vsync = graphics.GetBoolConfigValue("vsync");
            //this.Width = int.Parse(res[0]);
            //this.Height = int.Parse(res[1]);
            //ClientSize = new Size(this.Width, this.Height);
            //this.ClientRectangle = new Rectangle(new Point(0, 0), ClientSize);
            this.width = int.Parse(res[0]);
            this.height = int.Parse(res[1]);

            //GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
            //this.WindowState = WindowState.
        }

        public void Update()
        {
            menuFactory.Update();
            if (menuFactory.signal == "reload")
            {
                Reload();
                menuFactory.signal = "";
            }
        }

        public bool DoQuit()
        {
            return graphics.quit;
        }

        public bool DoReload()
        {
            return graphics.reload;
        }

        public void UpdateFPS(double fps)
        {
            graphics.UpdateFPS(fps);
        }

        public void UpdateMousePoint(int x, int y, bool left, bool middle, bool right)
        {
            graphics.UpdateMousePoint(x, y, left, middle, right);
        }

        public void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            menuFactory.Draw();
        }
    }
}
