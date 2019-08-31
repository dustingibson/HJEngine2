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
        private editor.MapEditor mapEditor;

        public prim.GameStateMachine state;
        public int width;
        public int height;
        public bool fullScreen;
        public bool vsync;

        public Game()
        {
            state = new prim.GameStateMachine();
            mainConfig = new util.Config("main");
            mapEditor = new editor.MapEditor();

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

            graphics = new gfx.Graphics(new prim.Size(width, height), mainConfig);
            menuFactory = new ui.MenuFactory(graphics);
            menuFactory.GotoMenu("main menu");

            graphics.size.w = this.width;
            graphics.size.h = this.height;
        }

        protected void Reload()
        {

            Dictionary<string,string> configValues = graphics.GetConfigValues();
            string[] res = configValues["resolution"].Split(',');
            this.fullScreen = graphics.GetBoolConfigValue("fullscreen");
            this.vsync = graphics.GetBoolConfigValue("vsync");
            this.width = int.Parse(res[0]);
            this.height = int.Parse(res[1]);
        }

        public void Update()
        {
            string[] signalParams = menuFactory.signal.Split(',');
            if (signalParams[0] == "reload")
            {
                Reload();
                menuFactory.signal = "";
            }
            if (signalParams[0] == "change state")
                state.TransitionState(signalParams[1]);

            if (state.currentState == "main menu")
                menuFactory.Update();
            else if (state.currentState == "editor")
                mapEditor.Update();
        }

        public bool DoQuit()
        {
            return graphics.quit;
        }

        public void SetQuit()
        {
            graphics.quit = true;
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

        public void UpdateKeyBuffer(string buffer)
        {
            graphics.UpdateKeyBuffer(buffer);
        }

        public void CleanUp()
        {
            graphics.CleanUp();
        }

        public void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            if(state.currentState == "main menu")
                menuFactory.Draw();
            else if (state.currentState == "editor")
                mapEditor.Draw();
        }
    }
}
