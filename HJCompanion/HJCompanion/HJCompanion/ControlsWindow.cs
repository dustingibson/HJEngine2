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
    public partial class ControlsWindow : Form
    {
        public string signal;
        public event EventHandler controlSelected;
        public MapInterface.MapInterface mapInterface;

        public ControlsWindow(MapInterface.MapInterface mapInterface)
        {
            InitializeComponent();
            signal = "";
            this.mapInterface = mapInterface;
        }

        public virtual void OnControlSelected(EventArgs e)
        {
            EventHandler handler = controlSelected;
            handler?.Invoke(this, e);
        }

        private ControlEventArgs SetSignal(string signal)
        {
            ControlEventArgs eArg = new ControlEventArgs();
            eArg.signal = signal;
            return eArg;
        }

        private void cursorButton_Click(object sender, EventArgs e)
        {
            EventHandler handler = controlSelected;
            handler?.Invoke(this, SetSignal("cursor"));
        }

        private void placeButton_Click(object sender, EventArgs e)
        {
            EventHandler handler = controlSelected;
            handler?.Invoke(this, SetSignal("place"));
        }

        private void ControlsWindow_Load(object sender, EventArgs e)
        {

        }

        private void addObjButton_Click(object sender, EventArgs e)
        {
            ObjectTemplateForm objTemplateForm = new ObjectTemplateForm(mapInterface);
            objTemplateForm.ShowDialog();
        }
    }

    public class ControlEventArgs : EventArgs
    {
        public string signal;
    }
}
