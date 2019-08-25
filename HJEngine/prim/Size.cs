using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HJEngine.prim
{
    class Size
    {
        public float w;
        public float h;

        public Size(float w, float h)
        {
            this.w = w;
            this.h = h;
        }

        public Size()
        {
            this.w = 0;
            this.h = 0;
        }

        public Size(Size sizeCopy)
        {
            this.w = sizeCopy.w;
            this.h = sizeCopy.h;
        }

        public Size Scale(float p)
        {
            return new prim.Size(w * p, h * p);
        }
    }
}
