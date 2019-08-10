using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Drawing;

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
            string fontType = menuNode.Attributes["font"].Value;
            Color fontColor = stringToColor(menuNode.Attributes["font_color"].Value);
            Color bgColor = stringToColor(menuNode.Attributes["bg_color"].Value);
            Color borderColor = stringToColor(menuNode.Attributes["border_color"].Value);
            int fontSize = int.Parse(menuNode.Attributes["font_size"].Value);
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
                        components.Add(new Button(graphics, text, fontSize, fontType, fontColor,
                            new prim.Point(x, y, strX == "center", strY == "center"), new prim.Size(w, h)));
                    }

                    if(type == "pane")
                    {
                        Color paneColor = stringToColor(itemNode.Attributes["pane_color"].Value);
                        float borderSize = float.Parse(itemNode.Attributes["border_size"].Value);
                        prim.Size borderSizeObj = new prim.Size(borderSize, borderSize);
                        components.Add(new Pane(graphics, paneColor, borderColor, borderSizeObj, 
                            new prim.Point(x, y, strX == "center", strY == "center"), new prim.Size(w, h)));
                    }

                }
            }
        }

        private Color stringToColor(string colorStr)
        {
            string[] rgba = colorStr.Split(',');
            int r = int.Parse(rgba[0]);
            int g = int.Parse(rgba[1]);
            int b = int.Parse(rgba[2]);
            int a = int.Parse(rgba[3]);
            return Color.FromArgb(a, r, g, b);
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
