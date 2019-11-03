using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;

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
        private gfx.Cursor lockedCursor;
        private gfx.Graphics graphics;
        private string mode;
        private string prevMode;
        private gfx.GameMap map;
        private MapInterface.ObjectTemplate curObj;

        public MapEditor(gfx.Graphics graphics)
        {
            this.graphics = graphics;
            process = new Process();
            initState = new prim.InitStateMachine();
            process.StartInfo.FileName = Directory.GetCurrentDirectory()
                + "/res/editor/HJCompanion.exe";
            defaultCursor = new gfx.Cursor(graphics);
            lockedCursor = new gfx.Cursor(graphics, "locked");
            editCursor = new gfx.Cursor(graphics, "edit");
            cursor = defaultCursor;
            mode = "cursor";
            prevMode = "";
            map = new gfx.GameMap();
        }

        public void Launch()
        {
            //process.Start();
            ipc = new util.IPC();
        }

        public void Draw()
        {
            //TODO: Quad Tree Data Structure
            foreach(MapInterface.ObjectInstance inst in this.map.mapInterface.objectInstances)
            {
                inst.Draw();
            }
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
                if (ipc.signal != "")
                {
                    string[] allParams = ipc.signal.Split(',');
                    if (allParams[0] == "lock")
                    {
                        prevMode = mode;
                        cursor = lockedCursor;
                        mode = "locked";
                    }
                    if (allParams[0] == "unlock")
                    {
                        cursor = defaultCursor;
                        mode = prevMode;
                    }
                    if (allParams[0] == "load map")
                    {
                        //TODO: Load Map
                        string path = Directory.GetCurrentDirectory() + "/res/maps/" + allParams[1];
                        map.LoadMap(this.graphics, path);
                        Console.WriteLine("LOAD MAP");
                    }
                    if (allParams[0] == "save instances")
                    {
                        ipc.SendMessage("lock");
                        List<MapInterface.ObjectInstance> instance = new List<MapInterface.ObjectInstance>(map.mapInterface.objectInstances);
                        //Load with latest
                        map.LoadMap(this.graphics);
                        //Save
                        map.mapInterface.objectInstances = instance;
                        map.mapInterface.Save();
                        ipc.SendMessage("unlock");
                        //Now client must have latest
                        ipc.SendMessage("reload map");
                        
                    }
                    if (allParams[0] == "remove all instances")
                    {
                        ipc.SendMessage("lock");
                        map.mapInterface.objectInstances = new List<MapInterface.ObjectInstance>();
                        map.mapInterface.Save();
                        ipc.SendMessage("unlock");
                        //Client must have the latest
                        ipc.SendMessage("reload map");
                        
                    }
                    if (allParams[0] == "reload map")
                    {
                        Console.WriteLine("RELOAD MAP");
                    }
                    if (allParams[0] == "place")
                    {
                        map.LoadMap(this.graphics);
                        string objKey = allParams[1];
                        curObj = map.mapInterface.objectTemplates[objKey];
                        Bitmap newImage = curObj.images["default"].image;
                        editCursor.ChangeTexture(newImage);
                        cursor = editCursor;
                        this.mode = "place";
                    }
                    if (allParams[0] == "cursor")
                    {
                        cursor = defaultCursor;
                        this.mode = "cursor";
                    }
                }
                if (graphics.leftClick.currentState == "clicked")
                {
                    if (this.mode == "place")
                    {
                        prim.Point pnt = graphics.mousePoint;
                        gfx.ObjectEntity objInstance = new gfx.ObjectEntity(curObj, graphics, graphics.mousePoint );
                        map.mapInterface.objectInstances.Add(objInstance);    
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
