using App.Core.Services;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Globalization;
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

        private readonly ServerService serverService;
        public Window1()
        {
            InitializeComponent();

            serverService = new ServerService();
        }


        private void ConnectToServer()
        {
            try
            {
                serverService.StartServer();
                MessageBox.Show("Connected to server.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to server: {ex.Message}");
            }
        }

        private void SendToServer(string data)
        {
            try
            {
                serverService.SendData(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending data to server: {ex.Message}");
            }
        }

        private void DisconnectFromServer()
        {
            try
            {
                serverService.StopServer();
                MessageBox.Show("Disconnected from server.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error disconnecting from server: {ex.Message}");
            }
        }

        // Event handlers for UI interactions
        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectToServer();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string data = "Some data to send to the server";
            SendToServer(data);
        }

        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            DisconnectFromServer();
        }

    }
}

