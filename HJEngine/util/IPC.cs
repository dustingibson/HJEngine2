using System;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HJEngine.util
{
    class IPC
    {
        private NamedPipeServerStream server;
        private NamedPipeClientStream client;
        private StreamWriter writer;
        private StreamReader reader;
        private Task readerThread;

        public string signal;

        public IPC()
        {
            signal = "";
            StartServer();
            SendMessage("map selection");
        }

        public void SendMessage(string message)
        {
            writer.WriteLine(message);
            writer.Flush();
        }

        public void PollMessage()
        {
            if ((readerThread == null || readerThread.IsCompleted) && signal != "exit")
            {
                readerThread = Task.Factory.StartNew(() =>
                {
                    signal = reader.ReadLine();
                });
            }
            else if(signal != "exit")
            {
                signal = "";
            }
            else
            {
                Console.WriteLine("Closing for good!");
            }
        }

        public void Stop()
        {
            readerThread.Wait();
            server.Disconnect();
            server.Close();
            client.Close();
        }

        public void StartServer()
        {
            server = new NamedPipeServerStream("torender");
            server.WaitForConnection();
            writer = new StreamWriter(server);
            client = new NamedPipeClientStream("toui");
            reader = new StreamReader(client);
            client.Connect(); 
        }
    }
}
