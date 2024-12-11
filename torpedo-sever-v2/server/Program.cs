using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ShipPlacementServer
{
    class Program
    {
        private static List<TcpClient> clients = new List<TcpClient>();
        private static int maxClients = 2; // Maximum of 2 clients allowed
        private static int currentPlayerIndex = 0; // 0 for Player 1, 1 for Player 2
        private static bool[] readyStatus = new bool[maxClients]; // To track if both clients are ready

        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 12345);
            server.Start();
            Console.WriteLine("Server started...");

            while (clients.Count < maxClients)
            {
                // Accept incoming client connections
                TcpClient client = server.AcceptTcpClient();
                clients.Add(client);
                Console.WriteLine("Client connected");

                // Start a new thread to handle communication with the client
                Thread clientThread = new Thread(() => HandleClient(client, clients.Count - 1));
                clientThread.Start();
            }

            // After both clients are connected, notify both clients to change the background color
            foreach (TcpClient client in clients)
            {
                SendMessage(client, "ChangeBackgroundColor");
            }

            Console.WriteLine("Both clients are connected. Background color will be changed.");

            // Wait until both clients are ready
            while (!readyStatus[0] || !readyStatus[1])
            {
                Thread.Sleep(100);
            }

            // Both clients are ready, now let them take turns
            StartGame();
        }

        private static void HandleClient(TcpClient client, int clientIndex)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];

            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;

                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Received from client " + clientIndex + ": " + message);
                //string square = message.Substring("SelectSquare".Length);
                Console.WriteLine(message.Substring("SelectSquare".Length));
                //HandlePlayerMove(clientIndex, square);

                if (message == "Ready")
                {
                    readyStatus[clientIndex] = true;
                    Console.WriteLine("Client " + clientIndex + " is ready.");

                    // If both clients are ready, start the game
                    if (readyStatus[0] && readyStatus[1])
                    {
                        SendMessageToAll("Both clients are ready. You can now select squares.");
                        StartGame();
                    }
                }

                // Handle player moves (e.g., selecting a square)
                if (message.StartsWith("SelectSquare"))
                {
                    Console.WriteLine("SelectSquare");
                    string square = message.Substring("SelectSquare".Length);
                    HandlePlayerMove(clientIndex, square);
                }
            }
        }
        
        private static void HandlePlayerMove(int clientIndex, string square)
        {
            Console.WriteLine('\n');
            Console.WriteLine(clientIndex);
            Console.WriteLine(currentPlayerIndex);
            // Check if it's the current player's turn
            if (clientIndex != currentPlayerIndex)
            {

                SendMessage(clients[clientIndex], "WaitForYourTurn");
                return;
            }

            // Process the player's move (e.g., mark the square)
            Console.WriteLine("Player " + clientIndex + " selected square: " + square);

            // Change turn to the other player
            currentPlayerIndex = (currentPlayerIndex + 1) % 2;

            // Notify both clients that it's the other player's turn
            SendMessage(clients[currentPlayerIndex], "YourTurn");
            SendMessage(clients[(currentPlayerIndex + 1) % 2], "WaitForYourTurn");
        }
        
        private static void StartGame()
        {
            // Start the game logic once both players are ready
            SendMessageToAll("Game has started! Player 1, it's your turn to select a square.");
        }

        private static void SendMessage(TcpClient client, string message)
        {
            NetworkStream stream = client.GetStream();
            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        private static void SendMessageToAll(string message)
        {
            foreach (TcpClient client in clients)
            {
                SendMessage(client, message);
            }
        }
    }
}
