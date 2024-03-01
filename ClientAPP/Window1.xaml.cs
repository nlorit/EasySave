using App;
using App.Core.Services;
using ClientAPP.Services;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Http;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace ClientAPP
{
    /// <summary>
    /// Logique d'interaction pour Window1@ .xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private ClientService client = new();

        public Window1()
        {
            InitializeComponent();
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {


            client.Connect();
            ServerResponseTextBlock.Text = "Connected to server.";
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageTextBox.Text;
            string response = client.SendMessage(message);
            ServerResponseTextBlock.Text = response;
        }

        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            client.Disconnect();
            ServerResponseTextBlock.Text = "Disconnected from server.";
        }
    }
}