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
        public List<ObjectInstance> objectInstances;
        public string path;
        public string name;

        public MapInterface()
        {
            this.path = "";
            objectTemplates = new Dictionary<string, ObjectTemplate>();
            objectInstances = new List<ObjectInstance>();
        }

        public MapInterface(string path)
        {
            this.path = path;
            objectTemplates = new Dictionary<string, ObjectTemplate>();
            objectInstances = new List<ObjectInstance>();
            if (!File.Exists(path))
            {
                Save();
            }
            else
            {
                Load();
            }
        }

        public void AddObjectTemplate(ObjectTemplate objTemplate)
        {
            if (objectTemplates.ContainsKey(objTemplate.name))
                objectTemplates[objTemplate.name] = objTemplate;
            else
                objectTemplates.Add(objTemplate.name, objTemplate);
        }

        public void RemoveObjectTemplate(string key)
        {
            if (objectTemplates.ContainsKey(key))
                objectTemplates.Remove(key);
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

        public float traverseToFloat(Byte[] byteBuffer, ref int counter)
        {
            float result = BitConverter.ToSingle(byteBuffer, counter);
            counter = counter + 4;
            return result;
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

        public Byte[] getStringBuffer(String buf, int destSize=-1)
        {
            int size = destSize < 0 ? buf.Length : destSize;
            Byte[] strBuffer = new Byte[size];
            strBuffer = System.Text.Encoding.ASCII.GetBytes(buf.PadRight(size, '\0'));
            return strBuffer;
        }

        public Byte[] getIntBuffer(int buf)
        {
            Byte[] intBuffer = BitConverter.GetBytes(buf);
            return intBuffer;
        }

        public Byte[] getFloatBuffer(float buf)
        {
            return BitConverter.GetBytes(buf);
        }

        public Byte[] getDoubleBuffer(string buf)
        {
            return getStringBuffer(buf);
        }

        public Byte[] getBoolBuffer(bool buf)
        {
            Byte[] intBuffer = BitConverter.GetBytes(buf);
            return intBuffer;
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

        public void CreateOrLoad(string path)
        {
            this.path = path;
            if (!File.Exists(path))
            {
                Save();
            }
            else
            {
                Load();
            }
        }

        public void Save(string path)
        {
            this.path = path;
            Save();
        }

        public void Load(string path)
        {
            this.path = path;
            Load();
        }

        public void Clear()
        {
            objectTemplates = new Dictionary<string, ObjectTemplate>();
            objectInstances = new List<ObjectInstance>();
        }

        public void Load()
        {
            Clear();
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
                for(int j = 0; j < numProps; j++)
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
                //Number of images
                int numImages = traverseToInt(mainBuffer, ref counter);
                for(int k = 0; k < numImages; k++)
                {
                    int numSteps = traverseToInt(mainBuffer, ref counter);
                    for (int p = 0; p < numSteps; p++)
                    {
                        //Image name
                        string imageName = traverseToString(mainBuffer, ref counter, 20);
                        //Image size
                        int imageSize = traverseToInt(mainBuffer, ref counter);
                        //Image value
                        Bitmap image = traverseToBitmap(mainBuffer, ref counter, imageSize);
                        //# of Collision Vectors
                        int nVect = traverseToInt(mainBuffer, ref counter);
                        List<Line> vectors = new List<Line>();
                        for (int m = 0; m < nVect; m++)
                        {
                            int x1 = traverseToInt(mainBuffer, ref counter);
                            int y1 = traverseToInt(mainBuffer, ref counter);
                            int x2 = traverseToInt(mainBuffer, ref counter);
                            int y2 = traverseToInt(mainBuffer, ref counter);
                            Line line = new Line(x1, y1, x2, y2);
                            vectors.Add(line);
                        }
                        curTemplate.AddImage(imageName, image, vectors);
                    }
                }
                objectTemplates.Add(objName, curTemplate);
            }
            //Instance count
            int numInst = traverseToInt(mainBuffer, ref counter);
            for (int i = 0; i < numInst; i++)
            {
                //Instance name
                string instName = traverseToString(mainBuffer, ref counter, 20);
                ObjectInstance instance = new ObjectInstance(objectTemplates[instName]);
                //X
                instance.x = traverseToFloat(mainBuffer, ref counter);
                //Y
                instance.y = traverseToFloat(mainBuffer, ref counter);
                foreach (Property curProperty in instance.instance.properties.Values)
                {
                    //Size
                    int propSize = traverseToInt(mainBuffer, ref counter);
                    //Value
                    curProperty.value = traverseToByte(mainBuffer, ref counter, propSize);
                    instance.instance.AddProperty(curProperty.name, curProperty.type, curProperty.value);
                }
                objectInstances.Add(instance);
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
                    //Property value
                    bw.Write(curProperty.value);
                }
                //Number of images
                bw.Write(getIntBuffer(curTemplate.images.Count));
                foreach(string iKey in curTemplate.images.Keys)
                {
                    //Number of steps
                    bw.Write(getIntBuffer(curTemplate.images[iKey].Count));
                    foreach (ImageData imageData in curTemplate.images[iKey])
                    {
                        ImageData curImage = imageData;
                        //Bitmap name
                        bw.Write(getStringBuffer(iKey, 20));
                        //Bitmap size
                        int size = (int)getBitmapSize(curImage.image);
                        bw.Write(getIntBuffer(size));
                        //Bitmap value
                        bw.Write(getBitmapBytes(curImage.image));
                        //# of Collision Vectors
                        bw.Write(getIntBuffer(curImage.collisionVectors.Count));
                        foreach (Line curLine in curImage.collisionVectors)
                        {
                            //X1
                            bw.Write(getIntBuffer(curLine.x1));
                            //Y1
                            bw.Write(getIntBuffer(curLine.y1));
                            //X2
                            bw.Write(getIntBuffer(curLine.x2));
                            //Y2
                            bw.Write(getIntBuffer(curLine.y2));
                        }
                    }
                }
            }
            //Number of instances
            bw.Write(getIntBuffer(objectInstances.Count));
            foreach(ObjectInstance instance in objectInstances)
            {
                //Object Name
                bw.Write(getStringBuffer(instance.instance.name, 20));
                //X Value
                bw.Write(getFloatBuffer(instance.x));
                //Y Value
                bw.Write(getFloatBuffer(instance.y));
                ObjectTemplate newTemplate = objectTemplates[instance.instance.name];
                foreach (Property curProperty in instance.instance.properties.Values)
                {
                    string type = curProperty.type;
                    int size = curProperty.getSize();
                    //Property size
                    bw.Write(BitConverter.GetBytes(size));
                    //Property value
                    bw.Write(curProperty.value);
                }
            }
            bw.Close();
        }
    }

    public class Line
    {
        public int x1;
        public int y1;
        public int x2;
        public int y2;

        public Line(int x1, int y1, int x2, int y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }

    }

    public class ImageData
    {
        public List<Line> collisionVectors;
        public Bitmap image;

        public ImageData(Bitmap image)
        {
            this.image = image;
            collisionVectors = new List<Line>();
            Line top = new Line(0, 0, image.Width-1, 0);
            Line right = new Line(image.Width-1, 0, image.Width-1, image.Height-1);
            Line bottom = new Line(image.Width, image.Height-1, 0, image.Height-1);
            Line left = new Line(0, image.Height-1, 0, 0);
            collisionVectors.Add(top);
            collisionVectors.Add(right);
            collisionVectors.Add(bottom);
            collisionVectors.Add(left);
        }

        public ImageData(Bitmap image, List<Line> vects)
        {
            this.image = image;
            collisionVectors = vects; 
        }

    }

    public class ObjectInstance
    {
        public ObjectTemplate instance;
        public float x;
        public float y;

        public ObjectInstance()
        {
            instance = new ObjectTemplate();
            x = 0;
            y = 0;
        }

        public ObjectInstance(ObjectTemplate objTemp)
        {
            instance = objTemp;
            x = 0;
            y = 0;
        }

        public virtual void Draw()
        {

        }

        public virtual void Update()
        {

        }
    }

    public class ObjectTemplate
    {
        public Dictionary<string, List<ImageData>> images;
        public Dictionary<string, Property> properties;
        public string name;
        public bool visibility;

        public ObjectTemplate(string name)
        {
            this.visibility = true;
            this.name = name;
            properties = new Dictionary<string, Property>();
            images = new Dictionary<string, List<ImageData>>();
            //SetDefaultImage();
        }

        public ObjectTemplate()
        {
            properties = new Dictionary<string, Property>();
            images = new Dictionary<string, List<ImageData>>();
            SetDefaultImage();
        }

        private void SetDefaultImage()
        {
            AddImage("default", new Bitmap(32, 32));
        }

        public void SetBlankStepImage(string key)
        {
            AddImage(key, new Bitmap(32, 32));
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

        public void AddImage(string name, Bitmap bitmap, int index=-1)
        {
            if (images.ContainsKey(name))
            {
                if (index < 0)
                    images[name].Add(new ImageData(bitmap));
                else
                    images[name][index] = new ImageData(bitmap);
            }
            else
            {
                images.Add(name, new List<ImageData>());
                images[name].Add(new ImageData(bitmap));
            }
        }

        public void AddImage(string name, Bitmap bitmap, List<Line> lines, int index=-1)
        {
            if (images.ContainsKey(name))
            {
                if (index < 0)
                    images[name].Add(new ImageData(bitmap, lines));
                else
                    images[name][index] = new ImageData(bitmap, lines);
            }
            else
            {
                images.Add(name, new List<ImageData>());
                images[name].Add(new ImageData(bitmap, lines));
            }
        }

        private void RemoveImage(string name)
        {
            images.Remove(name);
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

        public float getFloat()
        {
            return float.Parse(getString());
        }

        public double getDouble()
        {
            return double.Parse(getString());
        }

        public bool getBool()
        {
            return BitConverter.ToBoolean(value, 0);
        }

        public int getSize()
        {
            return value.Length;
        }

        public dynamic GetValue()
        {
            if (type == "string")
                return getString();
            else if (type == "int")
                return getInt();
            else if (type == "double")
                return getDouble();
            else if (type == "float")
                return getFloat();
            else if (type == "image")
                return getBitmap();
            else if (type == "bool")
                return getBool();
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
