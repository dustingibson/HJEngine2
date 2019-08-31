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

        public MapEditor()
        {
            process = new Process();
            initState = new prim.InitStateMachine();
            process.StartInfo.FileName = Directory.GetCurrentDirectory()
                + "/res/editor/HJCompanion.exe";
        }

        public void Launch()
        {
            //process.Start();
            ipc = new util.IPC();
        }

        public void Draw()
        {

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
                }
            }

        }

        ~MapEditor()
        {
            if (!process.HasExited)
                process.CloseMainWindow();
        }
    }
}
