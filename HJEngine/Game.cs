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

        public Game(int width, int height)
        {
            mainConfig = new util.Config("main");

            this.width = width;
            this.height = height;
        }
        
        public void LoadGL()
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            graphics = new gfx.Graphics(new prim.Size(width, height), mainConfig);

            menuFactory = new ui.MenuFactory(graphics);
            menuFactory.GotoMenu("main menu");

            graphics.size.w = this.width;
            graphics.size.h = this.height;
        }

        public void Update()
        {
            menuFactory.Update();
            if (menuFactory.signal == "reload")
            {
                //Reload();
                menuFactory.signal = "";
            }
        }

        public bool DoQuit()
        {
            return graphics.quit;
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
