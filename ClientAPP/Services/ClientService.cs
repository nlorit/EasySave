using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientAPP.Services
{
    public class ClientService
    {
        private TcpClient client;
        private NetworkStream stream;

        public void Connect()
        {
            client = new TcpClient();
            client.Connect("127.0.0.1", 52286);
            stream = client.GetStream();
        }

        public string SendMessage(string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);

            byte[] responseBuffer = new byte[1024];
            int bytesRead = stream.Read(responseBuffer, 0, responseBuffer.Length);
            string response = Encoding.ASCII.GetString(responseBuffer, 0, bytesRead);

            return response;
        }

        public void Disconnect()
        {
            client.Close();
        }
    }
}