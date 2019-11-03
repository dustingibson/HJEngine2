using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.IO.Pipes;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using MapInterface;

namespace HJCompanion
{
    public partial class mainForm : Form
    {
        public FileStream fileStream;
        public MemoryMappedFile controlMemFile;
        public NamedPipeClientStream client;
        public StreamReader reader;
        NamedPipeServerStream server;
        StreamWriter writer;
        public string message;
        private ControlsWindow controlsWindow;
        private MapInterface.MapInterface mapInterface;

        public mainForm()
        {
            mapInterface = new MapInterface.MapInterface();
            client = new NamedPipeClientStream("torender");
            reader = new StreamReader(client);
            client.Connect();
            server = new NamedPipeServerStream("toui");
            server.WaitForConnection();
            writer = new StreamWriter(server);
            InitializeComponent();
        }

        public void OnControlSelected(object sender, EventArgs e)
        {
            SendMessage(((ControlEventArgs)e).signal);
        }

        public void OnSave(object sender, EventArgs e)
        {
            SendMessage("reload map");
        }

        public void SendMessage(string message)
        {
            writer.WriteLine(message);
            writer.Flush();
        }

        public void ReadFromPipe()
        {
            while(true)
            {
                string line = reader.ReadLine();
                if(line == "map selection")
                {
                    OpenMap openMapDlg = new OpenMap(this.mapInterface, reader, writer);
                    controlsWindow = new ControlsWindow(mapInterface, reader, writer);
                    if (openMapDlg.ShowDialog() == DialogResult.OK)
                    {
                        SendMessage(openMapDlg.signal);
                        controlsWindow.ShowDialog();
                    }
                }
            }
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            Thread thread = new Thread(() =>
           {
               ReadFromPipe();
           });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            //Task.Factory.StartNew(() =>
            //{
                //ReadFromPipe();
                //byte[] fileBytes = Encoding.ASCII.GetBytes("blah blah");
                //fileStream = new FileStream("control.hje", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                //controlMemFile = MemoryMappedFile.CreateFromFile(fileStream, "control", fileBytes.Length,
                //    MemoryMappedFileAccess.ReadWrite, new MemoryMappedFileSecurity(), HandleInheritability.Inheritable, true);
                //var viewStream = controlMemFile.CreateViewStream();
                //viewStream.Write( fileBytes, 0, fileBytes.Length);
            //});
        }

        ~mainForm()
        {
            //fileStream.Close();
            //controlMemFile.Dispose();
        }
    }
}
