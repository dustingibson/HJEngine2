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
        private Dictionary<string, gfx.Cursor> cursors;
        private gfx.Cursor defaultCursor;
        private gfx.Graphics graphics;
        private string mode;
        private string prevMode;
        private gfx.GameMap map;
        private MapInterface.ObjectTemplate curObj;
        private gfx.ObjectEntity heldObject;
        public string signal;

        public MapEditor(gfx.Graphics graphics)
        {
            this.signal = "";
            this.graphics = graphics;
            process = new Process();
            initState = new prim.InitStateMachine();
            process.StartInfo.FileName = Directory.GetCurrentDirectory()
                + "/res/editor/HJCompanion.exe";
            cursors = new Dictionary<string, gfx.Cursor>();
            cursors.Add("locked", new gfx.Cursor(graphics, "locked"));
            cursors.Add("edit", new gfx.Cursor(graphics, "edit"));
            cursors.Add("remove", new gfx.Cursor(graphics, "remove"));
            cursors.Add("move", new gfx.Cursor(graphics, "move"));
            defaultCursor = new gfx.Cursor(graphics);
            cursor = defaultCursor;
            mode = "cursor";
            prevMode = "";
            map = new gfx.GameMap(graphics);
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

        public List<gfx.ObjectEntity> getCursorAdjInstances()
        {
            List<gfx.ObjectEntity> results = new List<gfx.ObjectEntity>();
            foreach ( gfx.ObjectEntity inst in this.map.mapInterface.objectInstances)
            {
                if (graphics.mousePoint.x >= inst.point.x && graphics.mousePoint.x <= inst.point.x + inst.size.w)
                    if (graphics.mousePoint.y >= inst.point.y && graphics.mousePoint.y <= inst.point.y + inst.size.h)
                        results.Add(inst);
            }
            return results;
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
                    try
                    {
                        string[] allParams = ipc.signal.Split(',');
                        if (allParams[0] == "exit")
                        {
                            this.signal = "exit";
                            ipc.Stop();
                        }
                        if (allParams[0] == "remove instance")
                        {
                            cursor = cursors["remove"];
                            mode = "remove";
                        }
                        if (allParams[0] == "move instance")
                        {
                            cursor = cursors["move"];
                            mode = "move";
                        }
                        if (allParams[0] == "instance details")
                        {
                            cursor = cursors["details"];
                            mode = "details";
                        }
                        if (allParams[0] == "lock")
                        {
                            prevMode = mode;
                            cursor = cursors["locked"];
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
                            Bitmap newImage = curObj.images["default"][0].image;
                            cursors["edit"].ChangeTexture(newImage);
                            cursor = cursors["edit"];
                            this.mode = "place";
                        }
                        if (allParams[0] == "cursor")
                        {
                            cursor = defaultCursor;
                            this.mode = "cursor";
                        }
                    }
                    catch
                    {
                        this.signal = "exit";
                        ipc.Stop();
                    }
                }
                if (graphics.leftClick.currentState == "clicked")
                {
                    if (this.mode == "place")
                    {
                        prim.Point pnt = graphics.mousePoint;
                        gfx.ObjectEntity objInstance = new gfx.ObjectEntity(map.world, curObj, graphics, graphics.mousePoint);
                        map.mapInterface.objectInstances.Add(objInstance);
                    }
                    else if (this.mode == "remove")
                    {
                        List<gfx.ObjectEntity> insts = getCursorAdjInstances();
                        if (insts.Count > 0)
                            map.mapInterface.objectInstances.Remove(insts[0]);
                    }
                    else if (this.mode == "move progress")
                    {
                        this.mode = "move";
                    }
                }
                if (graphics.leftClick.currentState == "mouse down")
                {
                    if (this.mode == "move")
                    {
                        List<gfx.ObjectEntity> insts = getCursorAdjInstances();
                        if (insts.Count > 0)
                        {
                            heldObject = insts[0];
                            this.mode = "move progress";
                        }
                    }
                    if (this.mode == "move progress")
                    {
                        heldObject.UpdatePoint(graphics.mousePoint);
                        heldObject.Update();
                    }
                }
            }
            cursor.Update();
        }

        ~MapEditor()
        {
            try
            {
                //process.Close();
                //if (process != null && !process.HasExited)
                //    process.CloseMainWindow();
            }
            catch
            {

            }
        }
    }
}
