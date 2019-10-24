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

        public GameMap()
        {
            mapInterface = new MapInterface.MapInterface();
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

        public void AddEntity(string key)
        {
            //List<MapInterface.ObjectInstance> objectInstance = new List<MapInterface.ObjectInstance>();
            //foreach( MapInterface.ObjectInstance objInst in mapInterface.objectInstances)
            //{
            //    newInstance 
            //}
        }

    }

    class ObjectEntity : MapInterface.ObjectInstance
    {
        public ImageTexture texture;
        public prim.Point point;
        public prim.Size size;

        public ObjectEntity(MapInterface.ObjectTemplate temp, Graphics graphics, prim.Point point) : base()
        {
            this.point = point;
            this.x = point.x;
            this.y = point.y;
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

        }

        public override void Draw()
        {
            texture.Draw();
        }
    }

}
