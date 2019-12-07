using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using System.IO;
using OpenTK;
using System.Drawing.Text;

namespace HJEngine.gfx
{
    class Graphics
    {
        public prim.Size size;
        //public gfx.ShaderFactory shaders;
        public Dictionary<string,PrivateFontCollection> fonts;
        public prim.Point mousePoint;
        public double fps;
        public double t_fps;
        public double sec;
        public bool quit;
        public bool reload;
        public string keyBuffer;
        public KEYCODE keyCode;
        public prim.ClickStateMachine leftClick;
        public prim.ClickStateMachine rightClick;
        public prim.ClickStateMachine middleClick;
        private util.Config config;
        private Dictionary<string, string> configValues;

        public enum KEYCODE
        {
            NONE,
            BACKSPACE,
            SHIFT,
            ENTER
        }

        public Graphics(prim.Size size, util.Config config)
        {
            quit = false;
            reload = false;
            fps = 0;
            t_fps = 60;
            this.config = config;
            configValues = config.GetSettingCopy();
            leftClick = new prim.ClickStateMachine();
            rightClick = new prim.ClickStateMachine();
            middleClick = new prim.ClickStateMachine();
            mousePoint = new prim.Point(0,0);

            this.size = size;
            fonts = new Dictionary<string, PrivateFontCollection>();


            foreach (string fname in Directory.GetFiles("res/fonts"))
            {
                PrivateFontCollection curFonts = new PrivateFontCollection();
                curFonts.AddFontFile(fname);
                fonts.Add(Path.GetFileNameWithoutExtension(fname), curFonts);
            }
        }

        public Dictionary<string, string> GetConfigValues()
        {
            config.Load();
            return config.GetSettingCopy();
        }
        
        public bool GetBoolConfigValue(string name)
        {
            return config.GetBoolValue(name);
        }

        public void SaveConfig()
        {
            config.SaveValue(configValues);
        }

        public void SaveConfig(Dictionary<string,string> extConfigValues)
        {
            config.SaveValue(extConfigValues);
        }

        public void UpdateFPS(double displayFPS)
        {
            this.fps = displayFPS;
            this.sec = 1.0 / displayFPS;
        }

        public Vector2 normalizeSize(prim.Size curSize)
        {
            //float aspect = this.size.w / this.size.h;
            return new Vector2(curSize.w / this.size.w, curSize.h / this.size.h);
        }

        public Vector2 normalizeSize(prim.Size childSize, prim.Size parentSize)
        {
            Vector2 parentPixelSize = this.getPixelSize(parentSize);
            return new Vector2(  childSize.w / parentPixelSize[0], 
                                  childSize.h / parentPixelSize[1]);
        }

        public Vector2 getPixelSize(prim.Size curSize)
        {
            return new Vector2(
                curSize.w * this.size.w,
                curSize.h * this.size.h
            );
        }

        public float getFPSDiff()
        {
            return fps >= t_fps ? 1f : ((float)t_fps - (float)fps) / (float)t_fps;
        }
        
        public float getWSquareSize(float hSize )
        {
            return (this.size.h * hSize) / this.size.w;
        }

        public prim.Point getNormalPoint(prim.Point point)
        {
            return new prim.Point(point.x / this.size.w, point.y / this.size.h);
        }

        private void HandleClickState(prim.ClickStateMachine clickState, bool clicked)
        {
            if(!clicked && clickState.currentState == "clicked")
                clickState.TransitionState("reset");
            if(clicked && clickState.currentState == "mouse up")
                clickState.TransitionState("click");
            if(!clicked && clickState.currentState == "mouse down")
                clickState.TransitionState("release");
        }

        public void UpdateMousePoint(int x, int y, bool leftClick, bool middleClick, bool rightClick)
        {
            mousePoint = getNormalPoint(new prim.Point(x,y));
            HandleClickState(this.leftClick, leftClick);
            HandleClickState(this.middleClick, middleClick);
            HandleClickState(this.rightClick, rightClick);
        }

        public Vector4 ColorToVec4(Color color)
        {
            float r = (float)color.R / 255f;
            float g = (float)color.G / 255f;
            float b = (float)color.B / 255f;
            float a = (float)color.A / 255f;
            return new Vector4(r, g, b, a);
        }

        public void UpdateKeyBuffer(string buffer)
        {
            if (buffer == "BackSpace")
                this.keyCode = KEYCODE.BACKSPACE;
            if (buffer == "Space")
                this.keyBuffer = " ";
            else
                this.keyBuffer += buffer;
        }

        public void CleanUp()
        {
            keyBuffer = "";
            this.keyCode = KEYCODE.NONE;
        }
    }
}
