using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HJCompanion
{
    public partial class OpenMap : Form
    {
        public List<string> maps;
        public string mapDir;
        public string signal;

        public OpenMap()
        {
            signal = "";
            maps = new List<string>();
            mapDir = Directory.GetParent(Directory.GetCurrentDirectory()).FullName + "/maps";
            InitializeComponent();
        }

        private void OpenMap_Load(object sender, EventArgs e)
        {
            okButton.Enabled = false;
            foreach (string files in Directory.GetFiles(mapDir))
            {
                maps.Add(files);
                selectionListBox.Items.Add(Path.GetFileName(files));
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            //Create map file
            string exist = maps.Find( curMap => nameText.Text.Equals(curMap) );
            if (exist != "")
            {
                File.Create(mapDir + "/" + nameText.Text + ".hjm");
                maps.Add(nameText.Text + ".hjm");
                selectionListBox.Refresh();
            }
            else
                MessageBox.Show("File exists");
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            signal = "load map," + selectionListBox.SelectedItem.ToString();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void selectionListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectionListBox.SelectedIndex > -1)
                okButton.Enabled = true;
            else
                okButton.Enabled = false;
        }
    }
}
