using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MapInterface;

namespace HJCompanion
{
    public partial class ObjectTemplateForm : Form
    {
        public MapInterface.ObjectTemplate objTemplate;
        public MapInterface.MapInterface mapInterface;
        public Bitmap uploadedImage;

        public ObjectTemplateForm(MapInterface.MapInterface mapInterface)
        {
            InitializeComponent();
            this.mapInterface = mapInterface;
            objTemplate = new MapInterface.ObjectTemplate();
            updatePropertyList();
        }

        private void valueText_Click(object sender, EventArgs e)
        {

        }

        public void updatePropertyList()
        {
            propListView.Items.Clear();
            foreach(string propertyKey in objTemplate.properties.Keys)
            {
                MapInterface.Property property = objTemplate.properties[propertyKey];
                propListView.Items.Add( new ListViewItem( new string[]{ property.name, " " }));
            }
            propListView.Refresh();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                string name = nameText.Text;
                string type = typeCombo.Text;
                byte[] value = { };

                if (type == "string")
                    value = mapInterface.getStringBuffer(valueText.Text);
                else if (type == "image")
                    value = mapInterface.getBitmapBytes(uploadedImage);
                else if (type == "int")
                    value = mapInterface.getIntBuffer(Int32.Parse(valueText.Text));
                else if (type == "double")
                    value = mapInterface.getDoubleBuffer(valueText.Text);
                else if (type == "float")
                    value = mapInterface.getFloatBuffer(valueText.Text);
                else if (type == "boolean")
                    value = mapInterface.getBoolBuffer(bool.Parse(valueText.Text));

                else
                {
                    MessageBox.Show("Invalid type");
                    return;
                }

                if (name == "")
                {
                    MessageBox.Show("Enter a name");
                    return;
                }
                if (value.Count() <= 0)
                {
                    MessageBox.Show("No value");
                    return;
                }

                objTemplate.AddProperty(name, type, value);
                updatePropertyList();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void propListView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
