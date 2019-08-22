using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HJEngine.util
{
    class Config
    {
        public Dictionary<string, string> values;
        public string name;
        public Config config;
        
        public Config(string name)
        {
            this.name = name;
            this.Load();
        }

        public void Load()
        {
            values = new Dictionary<string, string>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("config/" + name + ".xml");
            XmlNodeList settingsNodes = xmlDoc.SelectNodes("//setting");
            foreach (XmlNode node in settingsNodes)
            {
                values.Add(node.Attributes["key"].Value, node.Attributes["value"].Value);
            }
        }

        public Dictionary<string,string> GetSettingCopy()
        {
            return new Dictionary<string, string>(values);
        }

        public void SaveValue(Dictionary<string,string> newValues)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode configNode = xmlDoc.CreateNode(XmlNodeType.Element, "config", xmlDoc.NamespaceURI);
            xmlDoc.AppendChild(configNode);
            foreach (KeyValuePair<string,string> keyVal in newValues)
            {
                XmlNode settingNode = xmlDoc.CreateNode(XmlNodeType.Element, "setting", xmlDoc.NamespaceURI);
                XmlAttribute keyAttribute = xmlDoc.CreateAttribute("key");
                keyAttribute.Value = keyVal.Key;
                XmlAttribute valueAttribute = xmlDoc.CreateAttribute("value");
                valueAttribute.Value = keyVal.Value;
                settingNode.Attributes.SetNamedItem(keyAttribute);
                settingNode.Attributes.SetNamedItem(valueAttribute);
                configNode.AppendChild(settingNode);
            }
            xmlDoc.Save("config/" + name + ".xml");
        }

        public string GetValue(string name)
        {
            return values[name];
        }

    }
}
