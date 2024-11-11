using System.Net.Sockets;
using System.Text;

public class Player
{
    public TcpClient Client { get; }
    public NetworkStream Stream { get; }
    public GameBoard Board { get; }
    public bool IsReady { get; private set; }

    public Player(TcpClient client)
    {
        Client = client;
        Stream = client.GetStream();
        Board = new GameBoard();
    }

    public string ReceiveMessage()
    {
        byte[] buffer = new byte[1024];
        int bytesRead = Stream.Read(buffer, 0, buffer.Length);
        return Encoding.UTF8.GetString(buffer, 0, bytesRead);
    }

    public void SendMessage(string message)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        Stream.Write(buffer, 0, buffer.Length);
    }

    public bool AllBoatsPlaced()
    {
        return Board.AllBoatsPlaced();
    }
}

public class Boat
{
    public string Type { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public string Orientation { get; set; } // "Horizontal" or "Vertical"
}

public class GameBoard
{
    private List<Boat> boats = new List<Boat>();

    public bool CanPlaceBoat(Boat boat)
    {
        // Add logic for boat placement validation: inside the grid, no overlap, etc.
        return true;
    }

    public void PlaceBoat(Boat boat)
    {
        boats.Add(boat);
    }

    public bool AllBoatsPlaced()
    {
        return boats.Count == 5;  // All 5 boats placed
    }
}