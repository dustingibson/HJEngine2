using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL4;
using OpenTK;

namespace HJEngine.gfx
{
    class GameMap
    {
        public MapInterface.MapInterface mapInterface;
        public ControlEntity controlEntity;

        public GameMap()
        {
            mapInterface = new MapInterface.MapInterface();
            controlEntity = new ControlEntity();
        }

        public void AddControlEntity(Graphics graphics, string templateKey)
        {
            MapInterface.ObjectTemplate template = mapInterface.objectTemplates[templateKey];
            controlEntity = new ControlEntity(template, graphics);
        }

        public void LoadMap(Graphics graphics, string path="")
        {
            if (path == "")
                mapInterface.Load();
            else
                mapInterface = new MapInterface.MapInterface(path);
            //Offload Interface
            List<MapInterface.ObjectInstance> tempInstance = new List<MapInterface.ObjectInstance>(mapInterface.objectInstances);
            mapInterface.objectInstances.Clear();
            //Convert to entities
            foreach (MapInterface.ObjectInstance curInstance in tempInstance)
            {
                prim.Point pnt = new prim.Point(curInstance.x, curInstance.y);
                mapInterface.objectInstances.Add(new ObjectEntity(curInstance.instance, graphics, pnt));
            }

        }

        public void Draw()
        {
            //TODO: Quad Tree Data Structure
            foreach (ObjectEntity curInstance in mapInterface.objectInstances)
            {
                curInstance.Draw();
            }
            controlEntity.Draw();
        }

        public void Update()
        {
            //TODO: Quad Tree Data Structure
            foreach (ObjectEntity curInstance in mapInterface.objectInstances)
            {
                curInstance.Update();
            }
            controlEntity.Update();
        }

        public void AddEntity(string key)
        {
            //List<MapInterface.ObjectInstance> objectInstance = new List<MapInterface.ObjectInstance>();
            //foreach( MapInterface.ObjectInstance objInst in mapInterface.objectInstances)
            //{
            //    newInstance 
            //}
        }

    }

    class ControlEntity
    {
        private gfx.Graphics graphics;
        public bool active;
        public float vx;
        public float vy;
        public ObjectEntity controlObject;

        public ControlEntity(MapInterface.ObjectTemplate template, gfx.Graphics graphics)
        {
            this.active = true;
            this.graphics = graphics;
            controlObject = new ObjectEntity(template, graphics, new prim.Point(0f, 0f));
            vx = 0.005f;
            vy = 0.005f;
        }

        public ControlEntity()
        {
            this.active = false;
        }

        public void Update()
        {
            if (this.active)
            {
                if (graphics.keyBuffer.Contains("W"))
                    controlObject.dy = vy * -1;
                if (graphics.keyBuffer.Contains("S"))
                    controlObject.dy = vy;
                if (graphics.keyBuffer.Contains("A"))
                    controlObject.dx = vx * -1;
                if (graphics.keyBuffer.Contains("D"))
                    controlObject.dx = vx;
                controlObject.Update();
            }
        }

        public void Draw()
        {
            if(this.active)
            {
                controlObject.Draw();
            }
        }
    }

    class CollisionEntity
    {
        public prim.Point pntA;
        public prim.Point pntB;

        public CollisionEntity(prim.Point pntA, prim.Point pntB)
        {
            this.pntA = pntA;
            this.pntB = pntB;
        }
    }

    class ObjectEntity : MapInterface.ObjectInstance
    {
        public ImageTexture texture;
        public prim.Point point;
        public prim.Size size;
        public List<CollisionEntity> colEntities;
        public float dx;
        public float dy;

        public ObjectEntity(MapInterface.ObjectTemplate temp, Graphics graphics, prim.Point point) : base()
        {
            this.point = point;
            this.x = point.x;
            this.y = point.y;
            this.dx = 0f;
            this.dy = 0f;
            this.colEntities = new List<CollisionEntity>();
            InitTexture(temp, graphics);
        }

        private void InitTexture(MapInterface.ObjectTemplate temp, Graphics graphics)
        {
            Bitmap bitmap = temp.images["default"].image;
            this.instance = temp;
            this.size = new prim.Size(bitmap.Width / graphics.size.w, bitmap.Height / graphics.size.h);
            float[] vertices = {
                 point.x + size.w,  point.y + size.h, 0.0f, 1.0f, 1.0f,  // top right
                 point.x + size.w, point.y, 0.0f, 1.0f, 0.0f,  // bottom right
                point.x, point.y, 0.0f, 0.0f, 0.0f,  // bottom left
                point.x,  point.y + size.h, 0.0f, 0.0f, 1.0f   // top left
            };

            uint[] indices = {  // note that we start from 0!
                0, 1, 3,   // first triangle
                1, 2, 3    // second triangle
            };

            texture = new ImageTexture(graphics, bitmap, vertices, indices);

            foreach(MapInterface.Line line in temp.images["default"].collisionVectors)
            {
                float nx1 = ((float)line.x1 / bitmap.Width) * (float)size.w;
                float nx2 = ((float)line.x2 / bitmap.Width) * (float)size.w;
                float ny1 = ((float)line.y1 / bitmap.Height) * (float)size.h;
                float ny2 = ((float)line.y2 / bitmap.Height) * (float)size.h;
                colEntities.Add(new CollisionEntity(new prim.Point(nx1, ny1), new prim.Point(nx2, ny2)));
            }
        }

        public override void Update()
        {
            point.x += dx;
            point.y += dy;
            float[] vertices = {
                 point.x + size.w,  point.y + size.h, 0.0f, 1.0f, 1.0f,  // top right
                 point.x + size.w, point.y, 0.0f, 1.0f, 0.0f,  // bottom right
                point.x, point.y, 0.0f, 0.0f, 0.0f,  // bottom left
                point.x,  point.y + size.h, 0.0f, 0.0f, 1.0f   // top left
            };
            texture.Update(vertices);
            this.dx = 0f;
            this.dy = 0f;
        }

        public override void Draw()
        {
            texture.Draw();
        }
    }

}
