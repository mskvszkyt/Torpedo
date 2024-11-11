using System.Net.Sockets;
using System.Net;

public class GameServer
{
    private List<Player> players = new List<Player>();
    private const int MaxPlayers = 2;
    private TcpListener listener;

    public GameServer()
    {
        listener = new TcpListener(IPAddress.Any, 12345);
    }

    public void Start()
    {
        listener.Start();
        Console.WriteLine("Server started. Waiting for players...");

        while (players.Count < MaxPlayers)
        {
            TcpClient client = listener.AcceptTcpClient();
            Player player = new Player(client);
            players.Add(player);

            Console.WriteLine($"Player {players.Count} joined");
            new Thread(() => HandlePlayer(player)).Start();
        }
    }

    private void HandlePlayer(Player player)
    {
        // Handle communication with the player, boat placement, etc.
    }
}
