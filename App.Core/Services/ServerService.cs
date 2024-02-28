using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Services
{
    public class ServerService
    {
        public TcpListener tcpListener;
        public TcpClient tcpClient;
        public NetworkStream networkStream;

        public ServerService() 
        {
            
        }

        public void StartServer()
        {
            tcpListener = new TcpListener(IPAddress.Any, 12345); // Port à écouter
            tcpListener.Start();
            Console.WriteLine("Server started. Waiting for clients...");

            Thread clientThread = new Thread(ListenToClients);
            clientThread.Start();
        }


        private void ListenToClients()
        {
            while (true)
            {
                tcpClient = tcpListener.AcceptTcpClient();
                networkStream = tcpClient.GetStream();
                Console.WriteLine("Client connected.");

                Thread receiveThread = new Thread(ReceiveProgress);
                receiveThread.Start();
            }
        }

        private void ReceiveProgress()
        {
            while (true)
            {
                byte[] buffer = new byte[1024];
                int bytesRead = networkStream.Read(buffer, 0, buffer.Length);
                string clientProgress = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Received from client: " + clientProgress);

                // Mise à jour de la barre de progression du serveur avec la valeur de progression reçue du client
                //Dispacher.Invoke(() =>
                //{
                //    pbstatus1.Value = int.Parse(clientProgress.Replace("%", ""));
                //    lb_etat_prog_server.Content = "Progression du client : " + clientProgress;
                //});

                if (clientProgress == "disconnect")
                {
                    tcpClient.Close();
                    break;
                }
            }
        }


        public void StopServer()
        {
            // Stop the server
        }

        public void SendData(string data)
        {
            throw new NotImplementedException();
        }
    }
}
