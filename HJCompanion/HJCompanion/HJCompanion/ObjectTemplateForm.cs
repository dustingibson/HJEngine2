using System;
using System.IO;
using System.Threading;
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
        public Bitmap currentImage;
        public string name;

        public ObjectTemplateForm(MapInterface.MapInterface mapInterface)
        {
            InitializeComponent();
            this.mapInterface = mapInterface;
            objTemplate = new MapInterface.ObjectTemplate();
            updatePropertyList();
            UpdateImageListView();
        }

        public ObjectTemplateForm(MapInterface.MapInterface mapInterface, string key)
        {
            InitializeComponent();
            this.mapInterface = mapInterface;
            objTemplate = this.mapInterface.objectTemplates[key];
            updatePropertyList();
            UpdateImageListView();
            this.objNameText.Text = key;
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
                propListView.Items.Add( new ListViewItem( new string[]{ property.name, property.type }));
            }
            propListView.Refresh();
        }

        public void UpdateStepListView()
        {
            string key = imageListView.SelectedItems[0].Text;
            if (imageListView.SelectedItems.Count > 0)
            {
                stepListView.Items.Clear();
                for( int i = 0; i < objTemplate.images[key].Count; i++)
                {
                    stepListView.Items.Add(i.ToString());
                }
                stepListView.Refresh();
            }
        }

        public void ClearPropertiesUI()
        {
            nameText.Text = "";
            typeCombo.SelectedItem = "";
            valText.Text = "";
        }

        public void ClearImageUI()
        {
            imageNameText.Text = "";
            previewImagePicture.Image = null;
        }

        public void updateUI()
        {
            if (propListView.SelectedItems.Count > 0)
            {
                string selectedItem = propListView.SelectedItems[0].Text;
                Property property = objTemplate.properties[selectedItem];
                nameText.Text = property.name;
                typeCombo.SelectedItem = property.type;
                if (property.type == "string")
                    valText.Text = property.getString();
                else if (property.type == "int")
                    valText.Text = property.getInt().ToString();
                else if (property.type == "float")
                    valText.Text = property.getFloat().ToString();
                else if (property.type == "double")
                    valText.Text = property.getDouble().ToString();
                else if (property.type == "bool")
                    valText.Text = property.getBool().ToString();
                else if (property.type == "image")
                    uploadedImage = property.getBitmap();
            }
            else
            {
                ClearPropertiesUI();
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                string name = nameText.Text;
                string type = typeCombo.Text;
                byte[] value = { };

                if (type == "string")
                    value = mapInterface.getStringBuffer(valText.Text);
                else if (type == "image")
                    value = mapInterface.getBitmapBytes(uploadedImage);
                else if (type == "int")
                    value = mapInterface.getIntBuffer(Int32.Parse(valText.Text));
                else if (type == "double")
                    value = mapInterface.getDoubleBuffer(valText.Text);
                else if (type == "float")
                    value = mapInterface.getFloatBuffer(float.Parse(valText.Text));
                else if (type == "boolean")
                    value = mapInterface.getBoolBuffer(bool.Parse(valText.Text));

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
                ClearPropertiesUI();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void propListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateUI();
        }

        private void objNameText_TextChanged(object sender, EventArgs e)
        {

        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            objTemplate.name = objNameText.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public void UpdateImageListView()
        {
            imageListView.Items.Clear();
            foreach(string key in objTemplate.images.Keys)
            {
                imageListView.Items.Add(key);
            }
            imageListView.Refresh();
        }


        private void addImage(int step=-1)
        {
            try
            {
                string name = imageNameText.Text;
                string imgPath = imageFileText.Text;
                if (!File.Exists(imgPath))
                {
                    MessageBox.Show("File does not exist");
                    return;
                }
                if (name == "")
                {
                    MessageBox.Show("Invalid name");
                    return;
                }
                Bitmap image = new Bitmap(imgPath);
                objTemplate.AddImage(name, image, step);
                if (step < 0)
                {
                    UpdateImageListView();
                    ClearImageUI();
                }
            }
            catch (Exception ie)
            {
                MessageBox.Show(ie.ToString());
            }
        }

        private void imageAddButton_Click(object sender, EventArgs e)
        {
        }

        private void imageBrowseButton_Click(object sender, EventArgs e)
        {

            try
            {
                if (imgFileOpenDlg.ShowDialog() == DialogResult.OK)
                {
                    imageFileText.Text = imgFileOpenDlg.FileName;
                }
            }
            catch (Exception ie)
            {
                MessageBox.Show(ie.ToString());
            }
        }

        private void imageFileText_TextChanged(object sender, EventArgs e)
        {
            string imgPath = imageFileText.Text;
            try
            {
                if (File.Exists(imgPath))
                {
                    previewImagePicture.Image = new Bitmap(imgPath);
                }
            }
            catch { }
        }

        private void ObjectTemplateForm_Load(object sender, EventArgs e)
        {
           
        }

        private void imageListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (imageListView.SelectedItems.Count > 0)
            {
                string iKey = imageListView.SelectedItems[0].Text;
                imageNameText.Text = iKey;
                UpdateStepListView();
                //previewImagePicture.Image = new Bitmap(objTemplate.images[iKey][0].image);
            }
        }

        private void previewImagePicture_Click(object sender, EventArgs e)
        {
            string iKey = imageListView.SelectedItems[0].Text;
            CollisionForm colForm = new CollisionForm(objTemplate.images[iKey][0]);
            if(colForm.ShowDialog() == DialogResult.OK)
            {
                objTemplate.images[iKey][0].collisionVectors = colForm.lines;
            }
        }

        private void deletePropButton_Click(object sender, EventArgs e)
        {
            if (propListView.SelectedItems.Count > 0)
            {
                string selectedItem = propListView.SelectedItems[0].Text;
                objTemplate.properties.Remove(selectedItem);
                updatePropertyList();
                ClearPropertiesUI();
                //updateUI();
            }
        }

        private void deleteImageButton_Click(object sender, EventArgs e)
        {
            if (imageListView.SelectedItems.Count > 0)
            {
                string iKey = imageListView.SelectedItems[0].Text;
                objTemplate.images.Remove(iKey);
                UpdateImageListView();
                ClearImageUI();
            }
        }

        private void stepListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (stepListView.SelectedItems.Count > 0)
            {
                string iKey = imageListView.SelectedItems[0].Text;
                int index = stepListView.SelectedIndices[0];
                //imageNameText.Text = iKey;
                previewImagePicture.Image = new Bitmap(objTemplate.images[iKey][index].image);
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void addStepButton_Click(object sender, EventArgs e)
        {
            //addImage();
            if (imageListView.SelectedItems.Count > 0 && stepListView.SelectedItems.Count > 0)
            {
                string iKey = imageListView.SelectedItems[0].Text;
                objTemplate.SetBlankStepImage(iKey);
                UpdateStepListView();
            }
        }

        private void saveStepImage_Click(object sender, EventArgs e)
        {
            if (stepListView.SelectedItems.Count > 0)
            {
                int step = stepListView.SelectedIndices[0];
                addImage(step);
            }
        }
    }
}
