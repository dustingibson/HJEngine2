using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HJEngine.editor
{
    class MapEditor
    {
        private Process process;
        private prim.InitStateMachine initState;
        private util.IPC ipc;
        private gfx.Cursor cursor;
        private gfx.Cursor defaultCursor;
        private gfx.Cursor editCursor;
        private gfx.Graphics graphics;
        private string mode;

        public MapEditor(gfx.Graphics graphics)
        {
            this.graphics = graphics;
            process = new Process();
            initState = new prim.InitStateMachine();
            process.StartInfo.FileName = Directory.GetCurrentDirectory()
                + "/res/editor/HJCompanion.exe";
            defaultCursor = new gfx.Cursor(graphics);
            editCursor = new gfx.Cursor(graphics, "edit");
            cursor = defaultCursor;
            mode = "cursor";
        }

        public void Launch()
        {
            //process.Start();
            ipc = new util.IPC();
        }

        public void Draw()
        {
            cursor.Draw();
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
                ipc.PollMessage();
                if (ipc.signal != null)
                {
                    string[] allParams = ipc.signal.Split(',');
                    if (allParams[0] == "load map")
                    {
                        //TODO: Load Map
                        Console.WriteLine("LOAD MAP");
                    }
                    if (allParams[0] == "reload map")
                    {
                        Console.WriteLine("RELOAD MAP");
                    }
                    if (allParams[0] == "place")
                    {
                        cursor = editCursor;
                        this.mode = "edit";
                    }
                    if (allParams[0] == "cursor")
                    {
                        cursor = defaultCursor;
                        this.mode = "cursor";
                    }

                }
            }
            cursor.Update();
        }

        ~MapEditor()
        {
            try
            {
                if (process != null && !process.HasExited)
                    process.CloseMainWindow();
            }
            catch
            {

            }
        }
    }
}
