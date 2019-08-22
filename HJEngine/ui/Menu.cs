using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Drawing;

namespace HJEngine.ui
{
    class MenuFactory
    {
        public Dictionary<string, Menu> menus;
        public string currentMenu;
        public string signal;
        private gfx.Graphics graphics;

        public MenuFactory(gfx.Graphics graphics)
        {
            signal = "";
            menus = new Dictionary<string, Menu>();
            XmlDocument xmlDoc = new XmlDocument(); 
            xmlDoc.Load("config/menu.xml");
            XmlNodeList menuNodes = xmlDoc.SelectNodes("//menu");
            foreach(XmlNode node in menuNodes)
            {
                string name = node.Attributes["name"].Value;
                menus.Add(name, new Menu(name, graphics));
            }
            this.graphics = graphics;
        }

        public void GotoMenu(string name)
        {
            this.currentMenu = name;
        }

        public void Update()
        {
            Menu menu = menus[this.currentMenu];
            menu.Update();
            foreach(Component components in menu.components)
            {
                ProcessSignal(components);
            }
        }

        public void Draw()
        {
            menus[this.currentMenu].Draw();
        }

        public void ProcessSignal(Component component)
        {
            if (component.signal != "")
            {
                string[] sigParams = component.signal.Split(',');
                if (sigParams[0] == "change menu")
                {
                    this.currentMenu = sigParams[1];
                }
                else if(sigParams[0] == "quit")
                {
                    this.graphics.quit = true;
                }
                else if (sigParams[0] == "save")
                {
                    menus[this.currentMenu].SaveConfigValues();
                    graphics.SaveConfig(menus[this.currentMenu].configValues);
                    this.currentMenu = sigParams[1];
                    signal = "reload";
                }
                component.signal = "";
            }
        }

    }

    class Menu
    {
        public List<Component> components;
        public Dictionary<string, string> configValues;
        public gfx.StarField starfield;

        public Menu(string menuName, gfx.Graphics graphics)
        {
            starfield = new gfx.StarField(graphics);

            configValues = graphics.GetConfigValues();
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
                        string action = itemNode.Attributes["action"].Value;
                        string link = itemNode.Attributes["link"].Value;
                        components.Add(new Button(graphics, text, fontSize, fontType, fontColor,
                            new prim.Point(x, y, strX == "center", strY == "center"), new prim.Size(w, h),
                            action, link));
                    }

                    else if(type == "label")
                    {
                        components.Add(new Label(graphics, text, fontSize, fontType, fontColor,
                            new prim.Point(x, y, strX == "center", strY == "center"), new prim.Size(0, 0)));
                    }

                     else if(type == "pane")
                    {
                        Color paneColor = stringToColor(itemNode.Attributes["pane_color"].Value);
                        float borderSize = float.Parse(itemNode.Attributes["border_size"].Value);
                        prim.Size borderSizeObj = new prim.Size(borderSize, borderSize);
                        components.Add(new Pane(graphics, paneColor, borderColor, borderSizeObj, 
                            new prim.Point(x, y, strX == "center", strY == "center"), new prim.Size(w, h)));
                    }

                    else if(type == "toggle box")
                    {
                        float borderSize = float.Parse(itemNode.Attributes["border_size"].Value);
                        prim.Size borderSizeObj = new prim.Size(borderSize, borderSize);
                        components.Add(new ToggleBox(graphics, bgColor, borderColor, borderSizeObj,
                            text, fontSize, fontType, fontColor,
                            new prim.Point(x, y, strX == "center", strY == "center"), new prim.Size(w, h)));
                    }

                    else if(type == "select box")
                    {
                        string bind = itemNode.Attributes["bind"].Value;
                        string value = configValues[bind];
                        float borderSize = float.Parse(itemNode.Attributes["border_size"].Value);
                        int selectFontSize = int.Parse(itemNode.Attributes["label_font_size"].Value);
                        prim.Size borderSizeObj = new prim.Size(borderSize, borderSize);
                        XmlNodeList choiceNodes = itemNode.ChildNodes;
                        components.Add(new Select(graphics, bind, value, borderColor, borderSizeObj, selectFontSize,
                            fontType, fontColor, new prim.Point(x, y), new prim.Size(w, h), choiceNodes));
                    }
                }
            }
        }

        public void SaveConfigValues()
        {
            foreach(Component component in components)
            {
                if(this.configValues.ContainsKey(component.bind))
                    this.configValues[component.bind] = component.value;
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
            starfield.Draw();
            foreach (Component curComponent in this.components)
            {
                curComponent.Draw();
            }
        }

        public void Update()
        {
            starfield.Update();
            foreach (Component curComponent in this.components)
            {
                curComponent.Update();
            }
        }

        public float getCenter(float size)
        {
            return 0.5f - (size / 2.0f);
        }
    }
}
