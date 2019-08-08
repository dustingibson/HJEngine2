using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


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
        private gfx.Graphics graphics;

        public Component(gfx.Graphics graphics, string type, string text, prim.Point point, prim.Size size)
        {
            this.graphics = graphics;

            this.type = type;
            this.text = text;

            this.point = point;
            this.size = size;
            this.subComponents = new List<Component>();
        }

        public Component(gfx.Shader shader, XmlNode node)
        {

        }

        public float getCenter(float size)
        {
            return 0.5f - (size / 2.0f);
        }

        public virtual void Draw()
        {

        }
    }
}
