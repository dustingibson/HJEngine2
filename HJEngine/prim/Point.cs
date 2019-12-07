using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HJEngine.prim
{
    class Point
    {
        public float x;
        public float y;
        public bool cx;
        public bool cy;

        public Point(float x, float y, bool cx=false, bool cy=false)
        {
            this.x = x;
            this.y = y;
            this.cx = cx;
            this.cy = cy;
        }

        public Point(Point p)
        {
            this.x = p.x;
            this.y = p.y;
            this.cx = p.cx;
            this.cy = p.cy;
        }

        public static bool operator ==(Point p1, Point p2)
        {
            if ((object)p1 == null)
                return (object)p2 == null;

            return p1.x == p2.x && p1.y == p2.y;
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return !(p1 == p2);
        }

        public prim.Point GetTransPnt(float dx, float dy)
        {
            return new prim.Point(this.x + dx, this.y + dy);
        }
    }
}
