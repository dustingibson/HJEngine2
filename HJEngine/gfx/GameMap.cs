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
            Bitmap bitmap = temp.images["default"];
            this.instance = temp;
            this.point = point;
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
