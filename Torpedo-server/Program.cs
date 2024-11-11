using System;

namespace TorpedoServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Create a new instance of the GameServer
                GameServer gameServer = new GameServer();

                // Start the server
                gameServer.Start();

                Console.WriteLine("Server is running. Press any key to exit...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
