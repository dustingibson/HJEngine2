using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using MapInterface;
using System.IO;

namespace HJCompanion
{
    public partial class ControlsWindow : Form
    {
        public string signal;
        public event EventHandler controlSelected;
        public event EventHandler mapSaved;
        private StreamReader reader;
        private StreamWriter writer;
        public MapInterface.MapInterface mapInterface;
        public bool save;
        public string objKey;
        private List<Button> buttonList;
        private string activeButton;
        private Thread pipeThread;

        public ControlsWindow(MapInterface.MapInterface mapInterface, StreamReader reader, StreamWriter writer)
        {
            InitializeComponent();
            signal = "";
            objKey = "";
            this.mapInterface = mapInterface;
            this.buttonList = new List<Button>();
            this.writer = writer;
            this.reader = reader;

            this.buttonList.Add(placeButton);
            this.buttonList.Add(removeButton);
            this.buttonList.Add(moveButton);

            activeButton = "";
        }

        public virtual void OnControlSelected(EventArgs e)
        {
            EventHandler handler = controlSelected;
            handler?.Invoke(this, e);
        }

        public virtual void OnMapSaved(EventArgs e)
        {
            EventHandler handler = mapSaved;
            handler?.Invoke(this, e);
        }

        private ControlEventArgs SetSignal(string signal)
        {
            ControlEventArgs eArg = new ControlEventArgs();
            eArg.signal = signal;
            return eArg;
        }

        private MapSaveEventArgs SetSave()
        {
            MapSaveEventArgs eArg = new MapSaveEventArgs();
            eArg.signal = true;
            return eArg;
        }

        private void cursorButton_Click(object sender, EventArgs e)
        {
            this.writer.WriteLine("cursor");
            this.writer.Flush();
        }

        private void placeButton_Click(object sender, EventArgs e)
        {
            if (objKey != "")
            {
                this.writer.WriteLine("place," + objKey);
                this.writer.Flush();
                resetButtons(((Button)sender).Name);
            }
        }

        private void ControlsWindow_Load(object sender, EventArgs e)
        {
            UpdateObjectTemplateList();
            pipeThread = new Thread(() =>
            {
                ReadFromPipe();
            });
            pipeThread.SetApartmentState(ApartmentState.STA);
            pipeThread.Start();
        }

        private void ReadFromPipe()
        {
            while (true)
            {
                try
                {
                    string line = reader.ReadLine();
                    if (line == "reload map")
                    {
                        ReloadMap();
                    }
                    else if (line == "lock")
                    {
                        this.Invoke(new MethodInvoker(delegate
                        {
                            this.Enabled = false;
                        }));

                    }
                    else if (line == "unlock")
                    {
                        this.Invoke(new MethodInvoker(delegate
                        {
                            this.Enabled = true;
                        }));
                    }
                }
                catch(Exception ie)
                {
                    break;
                }
            }
        }

        private void ReloadMap()
        {
            writer.WriteLine("lock");
            mapInterface.Load();
            this.Invoke(new MethodInvoker(delegate
            {
                UpdateObjectTemplateList();
            }));
            writer.WriteLine("unlock");
        }

        private void UpdateObjectTemplateList()
        {
            objListView.Items.Clear();
            foreach(MapInterface.ObjectTemplate objTemp in mapInterface.objectTemplates.Values)
            {
                objListView.Items.Add(objTemp.name);
            }
        }

        private void TriggerSave()
        {
            this.writer.WriteLine("reload map");
        }

        private void addObjButton_Click(object sender, EventArgs e)
        {
            ObjectTemplateForm objTemplateForm = new ObjectTemplateForm(mapInterface);
            if(objTemplateForm.ShowDialog() == DialogResult.OK)
            {
                mapInterface.AddObjectTemplate(objTemplateForm.objTemplate);
                UpdateObjectTemplateList();
                TriggerSave();
            }
        }

        private void objListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (objListView.SelectedItems.Count > 0)
            {
                objKey = objListView.Text;
            }
            else
            {
                objKey = "";
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if(objListView.SelectedItems.Count > 0)
            {
                objKey = objListView.Text;
                ObjectTemplateForm objTemplateForm = new ObjectTemplateForm(mapInterface, objKey);
                if (objTemplateForm.ShowDialog() == DialogResult.OK)
                {
                    mapInterface.AddObjectTemplate(objTemplateForm.objTemplate);
                    UpdateObjectTemplateList();
                    TriggerSave();
                }
            }
        }

        private void deleteObjButton_Click(object sender, EventArgs e)
        {
            if (objListView.SelectedItems.Count > 0)
            {
                objKey = objListView.Text;
                mapInterface.RemoveObjectTemplate(objKey);
                UpdateObjectTemplateList();
            }
        }

        private void resetButtons(string activeButton)
        {
            foreach (Button curButton in buttonList)
                if (curButton.Name != activeButton)
                    curButton.BackColor = Color.Transparent;
                else
                    curButton.BackColor = Color.Green;
        }

        private void moveButton_Click(object sender, EventArgs e)
        {
            resetButtons(((Button)sender).Name);
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            resetButtons(((Button)sender).Name);
        }

        private void saveInstanceButton_Click(object sender, EventArgs e)
        {
            this.writer.WriteLine("save instances");
            this.writer.Flush();
            //EventHandler handler = controlSelected;
            //handler?.Invoke(this, SetSignal("save instances"));
        }

        private void delInstanceButton_Click(object sender, EventArgs e)
        {
            this.writer.WriteLine("remove all instances");
            this.writer.Flush();
        }
    }

    public class ControlEventArgs : EventArgs
    {
        public string signal;
    }

    public class MapSaveEventArgs : EventArgs
    {
        public bool signal;
    }
}
