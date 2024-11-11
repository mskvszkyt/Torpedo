using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace TorpedoClient
{
    public partial class MainWindow : Window
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private const string ServerAddress = "127.0.0.1"; // Localhost (replace with actual server address if needed)
        private const int ServerPort = 12345;

        public MainWindow()
        {
            InitializeComponent();
            ConnectToServer();
        }

        private void ConnectToServer()
        {
            try
            {
                _client = new TcpClient(ServerAddress, ServerPort);
                _stream = _client.GetStream();
                Console.WriteLine("Connected to server");

                // Start receiving messages from the server
                new Thread(ReceiveMessages).Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to the server: " + ex.Message);
            }
        }

        private void ReceiveMessages()
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                try
                {
                    int bytesRead = _stream.Read(buffer, 0, buffer.Length);
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Server: " + message);

                    // Handle server messages here
                    // For example, you could update the UI to show the boat placement phase
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error receiving message: " + ex.Message);
                    break;
                }
            }
        }

        // Method to send messages to the server
        private void SendMessageToServer(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            _stream.Write(buffer, 0, buffer.Length);
        }
    }
}
