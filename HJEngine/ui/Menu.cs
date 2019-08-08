using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HJEngine.ui
{
    class Menu
    {
        public List<Component> components;

        public Menu(string menuName, gfx.Graphics graphics)
        {
            components = new List<Component>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("config/menu.xml");
            XmlNode menuNode = xmlDoc.SelectSingleNode(string.Format("//menu[@name='{0}']", menuName));
            foreach (XmlNode itemNode in menuNode.ChildNodes)
            {
                if (itemNode.Name == "item")
                {
                    string type = itemNode.Attributes["type"].Value;
                    string name = itemNode.Attributes["name"].Value;
                    string text = itemNode.Attributes["text"].Value;
                    float w = float.Parse(itemNode.Attributes["w"].Value);
                    float h = float.Parse(itemNode.Attributes["h"].Value);
                    string strX = itemNode.Attributes["x"].Value;
                    string strY = itemNode.Attributes["y"].Value;
                    float x = strX == "center" ? getCenter(w) : float.Parse(strX);
                    float y = strY == "center" ? getCenter(h) : float.Parse(strY);

                    if(type == "button")
                    {
                        components.Add(new Button(graphics, text, 
                            new prim.Point(x, y, strX == "center", strY == "center"), new prim.Size(w, h)));
                    }

                }
            }
        }

        public void Draw()
        {
            foreach(Component curComponent in this.components)
            {
                curComponent.Draw();
            }
        }

        public float getCenter(float size)
        {
            return 0.5f - (size / 2.0f);
        }
    }
}
