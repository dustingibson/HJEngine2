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
using MapInterface;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.IO.Pipes;

using System.Threading;

namespace HJCompanion
{
    public partial class OpenMap : Form
    {
        private StreamReader reader;
        private StreamWriter writer;
        public List<string> maps;
        public string mapDir;
        public string signal;
        public MapInterface.MapInterface mapInterface;

        public OpenMap(MapInterface.MapInterface mapInterface, StreamReader reader, StreamWriter writer)
        {
            this.reader = reader;
            this.writer = writer;
            this.mapInterface = mapInterface;
            signal = "";
            maps = new List<string>();
            mapDir = Directory.GetParent(Directory.GetCurrentDirectory()).FullName + "/maps";
            InitializeComponent();
        }

        private void OpenMap_Load(object sender, EventArgs e)
        {
            okButton.Enabled = false;
            UpdateMapsList();
        }

        private void UpdateMapsList()
        {
            selectionListBox.Items.Clear();
            maps.Clear();
            foreach (string files in Directory.GetFiles(mapDir))
            {
                maps.Add(files);
                selectionListBox.Items.Add(Path.GetFileName(files));
            }
            selectionListBox.Refresh();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            //Create map file
            string exist = maps.Find( curMap => nameText.Text.Equals(curMap) );
            if (exist != "")
            {
                //FileStream fs = File.Create(mapDir + "/" + nameText.Text + ".hjm");
                mapInterface.NewMap(mapDir + "/" + nameText.Text + ".hjm");
                UpdateMapsList();
                //fs.Close();
            }
            else
                MessageBox.Show("File exists");
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            signal = "load map," + selectionListBox.SelectedItem.ToString();
            mapInterface.CreateOrLoad(mapDir + "/" 
                + selectionListBox.SelectedItem.ToString());
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

        private void OpenMap_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}
