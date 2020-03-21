using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using OpenTK.Input;

namespace HJEngine.editor
{
    class MapEditor
    {
        private Process process;
        private prim.InitStateMachine initState;
        private util.IPC ipc;
        private gfx.Cursor cursor;
        private string prevCursorName;
        private Dictionary<string, gfx.Cursor> cursors;
        private gfx.Cursor defaultCursor;
        private gfx.Graphics graphics;
        private gfx.ImageTexture shapeTexture;
        private string mode;
        private string prevMode;
        private prim.Point startShapePoint;
        private gfx.GameMap map;
        private MapInterface.ObjectTemplate curObj;
        private gfx.ObjectEntity heldObject;
        private string path;
        public string signal;
        private prim.Point scroll;
        private float scrollVelocity;

        public MapEditor(gfx.Graphics graphics)
        {
            this.prevCursorName = "cursor";
            this.signal = "";
            this.graphics = graphics;
            this.path = "";
            process = new Process();
            initState = new prim.InitStateMachine();
            process.StartInfo.FileName = Directory.GetCurrentDirectory()
                + "/res/editor/HJCompanion.exe";
            cursors = new Dictionary<string, gfx.Cursor>();
            cursors.Add("locked", new gfx.Cursor(graphics, "locked"));
            cursors.Add("edit", new gfx.Cursor(graphics, "edit"));
            cursors.Add("remove", new gfx.Cursor(graphics, "remove"));
            cursors.Add("move", new gfx.Cursor(graphics, "move"));
            cursors.Add("details", new gfx.Cursor(graphics, "details"));
            cursors.Add("shape", new gfx.Cursor(graphics, "shape"));
            defaultCursor = new gfx.Cursor(graphics);
            cursor = defaultCursor;
            mode = "cursor";
            prevMode = "";
            map = new gfx.GameMap(graphics);
            scroll = new prim.Point(0, 0);
            scrollVelocity = 0.025f;
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
            foreach (MapInterface.ObjectWall inst in this.map.mapInterface.objectWalls)
            {
                inst.Draw();
            }
            if (shapeTexture != null)
            {
                shapeTexture.Draw();
            }
            cursor.Draw();
        }

        public void Pan()
        {
            if (graphics.actionKeyBuffer.Contains((uint)Key.W)) 
                scroll.y -= scrollVelocity;
            if (graphics.actionKeyBuffer.Contains((uint)Key.S))
                scroll.y += scrollVelocity;
            if (graphics.actionKeyBuffer.Contains((uint)Key.A))
                scroll.x -= scrollVelocity;
            if (graphics.actionKeyBuffer.Contains((uint)Key.D))
                scroll.x += scrollVelocity;
            graphics.Scroll(scroll);
        }

        private prim.Point AbsMouse()
        {
            return new prim.Point(graphics.mousePoint.x + scroll.x,
                graphics.mousePoint.y + scroll.y);
        }

        public List<gfx.ObjectEntity> getCursorAdjInstances()
        {
            prim.Point absMouse = AbsMouse();
            List<gfx.ObjectEntity> results = new List<gfx.ObjectEntity>();
            foreach ( gfx.ObjectEntity inst in this.map.mapInterface.objectInstances)
            {
                if (absMouse.x >= inst.point.x && absMouse.x <= inst.point.x + inst.size.w)
                    if (absMouse.y >= inst.point.y && absMouse.y <= inst.point.y + inst.size.h)
                        results.Add(inst);
            }
            return results;
        }

        public void LockClient()
        {
            ipc.SendMessage("lock");
        }

        public void UnlockClient()
        {
            ipc.SendMessage("unlock");
        }

        public void SaveInstances()
        {
            LockClient();
            List<MapInterface.ObjectInstance> instance = new List<MapInterface.ObjectInstance>(map.mapInterface.objectInstances);
            List<MapInterface.ObjectWall> walls = new List<MapInterface.ObjectWall>(map.mapInterface.objectWalls);
            //Load with latest
            map.LoadMap(this.graphics);
            //Save
            map.mapInterface.objectInstances = instance;
            map.mapInterface.objectWalls = walls;
            map.mapInterface.Save();
            UnlockClient();
            //Now client must have latest
            ipc.SendMessage("reload map");
        }

        public void Update()
        {
            Pan();
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
                        if (allParams[0] == "shape instance")
                        {
                            cursor = cursors["shape"];
                            string objKey = allParams[1];
                            curObj = map.mapInterface.objectTemplates[objKey];
                            Bitmap curTexture = map.mapInterface.objectTemplates[objKey].images["default"][0].image;
                            shapeTexture = new gfx.ImageTexture(this.graphics, curTexture);
                            mode = "shape";
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
                            prevCursorName = cursor.name;
                            cursor = cursors["locked"];
                            mode = "locked";
                        }
                        if (allParams[0] == "unlock")
                        {
                            cursor = cursors[prevCursorName];
                            mode = prevMode;
                        }
                        if (allParams[0] == "load map")
                        {
                            //TODO: Load Map
                            this.path = Directory.GetCurrentDirectory() + "/res/maps/" + allParams[1];
                            map.LoadMap(this.graphics, this.path);
                            Console.WriteLine("LOAD MAP");
                        }
                        if (allParams[0] == "save instances")
                        {
                            SaveInstances();
                        }
                        if (allParams[0] == "remove all instances")
                        {
                            LockClient();
                            map.mapInterface.objectInstances = new List<MapInterface.ObjectInstance>();
                            map.mapInterface.Save();
                            UnlockClient();
                            //Client must have the latest
                            ipc.SendMessage("reload map");

                        }
                        if (allParams[0] == "reload map")
                        {
                            Console.WriteLine("RELOAD MAP");
                        }
                        if (allParams[0] == "load instances")
                        {
                            //ipc.SendMessage("lock");
                            map.LoadMap(this.graphics);
                            //ipc.SendMessage("unlock");
                            //ipc.SendMessage("reload map");
                        }
                        if (allParams[0] == "save instances async")
                        {
                            map.mapInterface.Save();
                        }
                        if (allParams[0] == "place")
                        {
                            SaveInstances();
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
                    catch(Exception ie)
                    {
                        this.signal = "exit";
                        ipc.Stop();
                    }
                }
                if (graphics.leftClick.currentState == "clicked")
                {
                    if (this.mode == "place")
                    {
                        prim.Point pnt = AbsMouse();
                        gfx.ObjectEntity objInstance = new gfx.ObjectEntity(map.world, curObj, graphics, AbsMouse());
                        map.mapInterface.objectInstances.Add(objInstance);
                        SaveInstances();
                    }
                    else if (this.mode == "remove")
                    {
                        List<gfx.ObjectEntity> insts = getCursorAdjInstances();
                        if (insts.Count > 0)
                            map.mapInterface.objectInstances.Remove(insts[0]);
                        SaveInstances();
                    }
                    else if (this.mode == "move progress")
                    {
                        this.mode = "move";
                        SaveInstances();
                    }
                    else if (this.mode == "shape progress")
                    {
                        this.mode = "shape";
                        prim.Size wallSize = new prim.Size(shapeTexture.size);
                        prim.Point wallPoint = new prim.Point(startShapePoint);
                        gfx.WallEntity objInstance = new gfx.WallEntity(map.world, curObj, graphics, wallPoint, wallSize);
                        map.mapInterface.objectWalls.Add(objInstance);
                        shapeTexture = null;
                    }
                    else if (this.mode == "details")
                    {
                        List<gfx.ObjectEntity> insts = getCursorAdjInstances();
                        if (insts.Count > 0)
                        {
                            SaveInstances();
                            int index = map.mapInterface.objectInstances.IndexOf(insts[0]);
                            ipc.SendMessage("launch details," + index.ToString());
                        }
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
                    if (this.mode == "shape")
                    {
                        this.mode = "shape progress";
                        startShapePoint = new prim.Point(graphics.mousePoint);
                    }
                    if (this.mode == "shape progress")
                    {
                        prim.Size newSize = new prim.Size(Math.Abs(startShapePoint.x - this.graphics.mousePoint.x),
                            Math.Abs(startShapePoint.y - this.graphics.mousePoint.y));
                        shapeTexture.Update(startShapePoint, newSize);
                    }
                    if (this.mode == "move progress")
                    {
                        heldObject.UpdatePoint(AbsMouse());
                        heldObject.Update();
                        map.mapInterface.Save();
                    }
                }
            }
            cursor.Update(AbsMouse());
        }

        public void saveInstances()
        {

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
