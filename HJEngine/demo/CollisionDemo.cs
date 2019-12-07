using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;

namespace HJEngine.demo
{
    class CollisionDemo
    {
        private prim.InitStateMachine initState;
        private gfx.Graphics graphics;
        private gfx.GameMap map;

        public CollisionDemo(gfx.Graphics graphics)
        {
            this.graphics = graphics;
            initState = new prim.InitStateMachine();
            map = new gfx.GameMap(graphics);
            map.LoadMap(graphics, "res/maps/demo.hjm");
            map.AddControlEntity(graphics, "test");
        }

        public void Launch()
        {
        }

        public void Draw()
        {
            map.Draw();
        }

        public void Update()
        {
            if (initState.currentState == "init")
            {
                Launch();
                initState.TransitionState("non init");
            }
            else
            {
                map.Update();
            }
        }

        public void CleanUp()
        {
            map.CleanUp();
        }
    }
}
