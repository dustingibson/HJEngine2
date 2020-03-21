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
    public partial class ObjectInstanceForm : Form
    {
        ObjectInstance instance;

        public ObjectInstanceForm(ObjectInstance objInst)
        {
            this.instance = objInst;
            InitializeComponent();
        }

        private void ObjectInstanceForm_Load(object sender, EventArgs e)
        {
            UpdatePropList();
        }

        private void PopulateItems(string key)
        {
            string val = Encoding.ASCII.GetString(instance.instance.properties[key].value);
            valText.Text = val; 
        }

        private void UpdatePropList()
        {
            propertyListView.Items.Clear();
            foreach(MapInterface.Property prop in instance.instance.properties.Values)
            {
                propertyListView.Items.Add( new ListViewItem (new string[] { prop.name, prop.type} ));
            }
            propertyListView.Refresh();
        }

        private void propertyListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (propertyListView.SelectedItems.Count > 0)
            {
                string key = propertyListView.SelectedItems[0].SubItems[0].Text;

                PopulateItems(key);
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (propertyListView.SelectedItems.Count > 0)
            {
                string key = propertyListView.SelectedItems[0].SubItems[0].Text;
                instance.instance.properties[key].value = Encoding.ASCII.GetBytes(valText.Text);
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (propertyListView.SelectedItems.Count > 0)
            {
                string key = propertyListView.SelectedItems[0].SubItems[0].Text;

                PopulateItems(key);
            }
            this.Close();
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            
        }
    }
}
