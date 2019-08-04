using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HJEngine.ui
{
    class Component
    {
        public string type;
        public string text;
        public string value;
        public prim.Point point;
        public prim.Size size;
        private List<Component> subComponents;

        public Component(string type, string text, prim.Point point, prim.Size size)
        {
            this.type = type;
            this.text = text;

            this.point = point;
            this.size = size;
            this.subComponents = new List<Component>();
        }

        public virtual void Draw()
        {

        }
    }
}
