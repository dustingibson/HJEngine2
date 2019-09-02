using System;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MapInterface
{
    public class MapInterface
    {
        public Dictionary<string, ObjectTemplate> objectTemplates;
        public string path;
        public string name;

        public MapInterface(string path)
        {
            this.path = path;
            objectTemplates = new Dictionary<string, ObjectTemplate>();
        }

        public void initBuffer(Byte[] tmpBuffer, int size)
        {
            for (int i = 0; i < size; i++)
                tmpBuffer[i] = 0;
        }

        public int traverseToInt(Byte[] byteBuffer, ref int counter, int destSize=4)
        {
            int resultingInt = 0;
            Byte[] tmpBuffer = new Byte[destSize];
            Array.Copy(byteBuffer, counter, tmpBuffer, 0, destSize);
            counter = counter + destSize;
            if (destSize == 4)
                resultingInt = BitConverter.ToInt32(tmpBuffer, 0);
            else
                resultingInt = (int)BitConverter.ToInt16(tmpBuffer, 0);
            return resultingInt;
        }

        public String traverseToString(Byte[] byteBuffer, ref int counter, int destSize)
        {
            String resultingString;
            Byte[] tmpBuffer = new Byte[destSize];
            Array.Copy(byteBuffer, counter, tmpBuffer, 0, destSize);
            resultingString = System.Text.Encoding.Default.GetString(tmpBuffer);
            counter = counter + destSize;
            String newResultingString = "";
            for (int i = 0; i < resultingString.Length; i++)
            {
                if (resultingString[i] == '\0')
                    break;
                else
                    newResultingString += resultingString[i];
            }

            return newResultingString;
        }

        public Byte[] traverseToByte(Byte[] byteBuffer, ref int counter, int destSize)
        {
            Byte[] tmpBuffer = new Byte[destSize];
            Array.Copy(byteBuffer, counter, tmpBuffer, 0, destSize);
            counter = counter + destSize;
            return tmpBuffer;
        }

        public Bitmap traverseToBitmap(Byte[] byteBuffer, ref int counter, int destSize)
        {
            Byte[] tmpBuffer = new Byte[destSize];
            TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
            Array.Copy(byteBuffer, counter, tmpBuffer, 0, destSize);
            Bitmap finalBitmap = (Bitmap)tc.ConvertFrom(tmpBuffer);
            counter = counter + destSize;
            return finalBitmap;
        }

        public long getBitmapSize(Bitmap img)
        {
            ImageConverter converter = new ImageConverter();
            Byte[] imgBytes = (byte[])converter.ConvertTo(img, typeof(byte[]));
            return imgBytes.Length;
        }

        public Byte[] getBitmapBytes(Bitmap img)
        {
            ImageConverter converter = new ImageConverter();
            Byte[] imgBytes = (byte[])converter.ConvertTo(img, typeof(byte[]));
            return imgBytes;
        }

        private Byte[] getStringBuffer(String buf, int size)
        {
            Byte[] strBuffer = new Byte[size];
            strBuffer = System.Text.Encoding.ASCII.GetBytes(buf.PadRight(20, '\0'));
            return strBuffer;
        }

        private Byte[] packNumBytes(int data, int size)
        {
            if (size == 4)
                return BitConverter.GetBytes(data);
            else
                return BitConverter.GetBytes((short)data);
        }

        private void AddToBitmap(Bitmap srcBitmap, Bitmap destBitmap, int x, int y, int w, int h)
        {
            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                {
                    Color col = srcBitmap.GetPixel(i, j);
                    destBitmap.SetPixel(x + i, y + j, col);
                }
        }

        public void Load()
        {
            Byte[] mainBuffer = File.ReadAllBytes(this.path);
            int counter = 0;
            //Number of objects
            int numObjs = traverseToInt(mainBuffer, ref counter);
            for(int i = 0; i < numObjs; i++)
            {
                //Object name
                string objName = traverseToString(mainBuffer, ref counter, 20);
                ObjectTemplate curTemplate = new ObjectTemplate(objName);
                //Number of properties
                int numProps = traverseToInt(mainBuffer, ref counter);
                for(int j = 0; j < numProps; i++)
                {
                    //Property name
                    string propertyName = traverseToString(mainBuffer, ref counter, 20);
                    //Property type
                    string propertyType = traverseToString(mainBuffer, ref counter, 20);
                    //Property size
                    int size = traverseToInt(mainBuffer, ref counter);
                    //Property value
                    byte[] value = traverseToByte(mainBuffer, ref counter, size);

                    curTemplate.AddProperty(propertyName, propertyType, value);
                }
                objectTemplates.Add(objName, curTemplate);
            }

        }

        public void Save()
        {
            //TODO: Temp backup folder
            FileStream fs = File.Create(this.path);
            BinaryWriter bw = new BinaryWriter(fs);
            //Number of Objects
            bw.Write(BitConverter.GetBytes(objectTemplates.Keys.Count));
            foreach (string key in objectTemplates.Keys)
            {
                ObjectTemplate curTemplate = objectTemplates[key];
                //Object Name
                bw.Write(getStringBuffer(curTemplate.name, 20));
                //Number of Properties
                bw.Write(BitConverter.GetBytes(curTemplate.properties.Keys.Count));
                //For each property
                foreach(string pKey in curTemplate.properties.Keys)
                {
                    Property curProperty = curTemplate.properties[pKey];
                    string type = curProperty.type;
                    int size = curProperty.getSize();
                    //Property name
                    bw.Write(getStringBuffer(curProperty.name, 20));
                    //Property type
                    bw.Write(getStringBuffer(curProperty.type, 20));
                    //Property size
                    bw.Write(BitConverter.GetBytes(size));
                    //Property 
                    bw.Write(curProperty.value);
                }
            }
            bw.Close();
        }
    }

    public class ObjectTemplate
    {
        public Dictionary<string, Property> properties;
        public string name;

        public ObjectTemplate(string name)
        {
            this.name = name;
            properties = new Dictionary<string, Property>();
        }

        public void AddProperty(string name, string type, byte[] value)
        {
            if (properties.ContainsKey(name))
            {
                properties[name].name = name;
                properties[name].type = type;
                properties[name].value = value;
            }
            else
            {
                Property property = new Property(name, type, value);
                properties.Add(name, property);
            }
        }

        public void RemoveProperty(string name)
        {
            properties.Remove(name);
        }

    }

    public class Property
    {
        public string name;
        public string type;
        public byte[] value;

        public Bitmap getBitmap()
        {
            TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
            Bitmap finalBitmap = (Bitmap)tc.ConvertFrom(value);
            return finalBitmap;
        }

        public string getString()
        {
            return System.Text.Encoding.Default.GetString(value);
        }

        public void getAudio()
        {

        }

        public int getInt()
        {
            return BitConverter.ToInt32(value, 0);
        }

        public int getSize()
        {
            return value.Length;
        }

        public dynamic GetValue()
        {
            if (type == "string")
            {
                return getString();
            }
            else if (type == "int")
            {
                return getInt();
            }
            else if (type == "image")
            {
                return getBitmap();
            }
            return "";
        }

        public Property(string name, string type, byte[] value)
        {
            this.name = name;
            this.type = type;
            this.value = value;
        }

    }
}
