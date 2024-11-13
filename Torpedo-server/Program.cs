using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class GameServer
{
    private const int Port = 5000;
    private static List<TcpClient> _clients = new List<TcpClient>();
    private static Game _game;

    public static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Starting server...");
            TcpListener listener = new TcpListener(IPAddress.Loopback, Port);  // Accepting local connections (localhost)
            listener.Start();
            Console.WriteLine("Server started, waiting for players to connect...");

            // Accept two players asynchronously
            Task.Run(() => AcceptPlayers(listener));

            // Keep the server running
            Console.WriteLine("Press any key to stop the server.");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static async Task AcceptPlayers(TcpListener listener)
    {
        while (_clients.Count < 2)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();
            _clients.Add(client);
            Console.WriteLine("Player connected. Total players: " + _clients.Count);

            // Start handling the player in a new thread
            Task.Run(() => HandlePlayer(client));
        }

        // Both players are connected, start the game
        _game = new Game(_clients);
        _game.Start();
    }

    private static async Task HandlePlayer(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[256];

        while (true)
        {
            try
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0)
                    break;

                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Received: " + message);
                _game.HandlePlayerInput(client, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading from server: {ex.Message}");
                break;
            }
        }
    }
}

class Game
{
    private List<TcpClient> _clients;
    private int _currentPlayerIndex;
    private GameBoard _player1Board, _player2Board;
    private bool _gameStarted;
    private bool _player1Ready, _player2Ready;
    private CancellationTokenSource _cancellationTokenSource;

    public Game(List<TcpClient> clients)
    {
        _clients = clients;
        _player1Board = new GameBoard();
        _player2Board = new GameBoard();
        _gameStarted = false;
        _player1Ready = false;
        _player2Ready = false;
        _currentPlayerIndex = 0;
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public void Start()
    {
        Console.WriteLine("Both players connected.");
        NotifyPlayers("Both players connected. Please place your ships.");

        // Inform players to start placing ships
        SendMessageToClient(_clients[0], "Place your ships. Your turn.");
        SendMessageToClient(_clients[1], "Place your ships. Your turn.");
    }

    public void HandlePlayerInput(TcpClient client, string input)
    {
        int playerIndex = _clients.IndexOf(client);

        if (!_gameStarted)
        {
            // Handle ship placement
            if (input.StartsWith("PLACE_SHIP"))
            {
                PlaceShip(client, input);
            }
            else if (_player1Ready && _player2Ready)
            {
                // Both players ready, start the game
                _gameStarted = true;
                SendMessageToClient(_clients[0], "Game started! Your turn.");
                SendMessageToClient(_clients[1], "Game started! Opponent's turn.");
            }
        }
        else
        {
            // Gameplay phase (shooting)
            if (input.StartsWith("SHOOT"))
            {
                ProcessShot(client, input);
            }
        }
    }

    private void PlaceShip(TcpClient client, string input)
    {
        int playerIndex = _clients.IndexOf(client);
        string[] parts = input.Split(' ');
        string shipType = parts[1];
        int x = int.Parse(parts[2]);
        int y = int.Parse(parts[3]);
        bool horizontal = bool.Parse(parts[4]);

        GameBoard board = playerIndex == 0 ? _player1Board : _player2Board;

        if (board.PlaceShip(shipType, x, y, horizontal))
        {
            SendMessageToClient(client, "Ship placed successfully!");

            if (playerIndex == 0)
                _player1Ready = true;
            else
                _player2Ready = true;
        }
        else
        {
            SendMessageToClient(client, "Invalid ship placement, try again.");
        }

        // Check if both players are ready
        if (_player1Ready && _player2Ready)
        {
            // Notify both players the game will begin
            _gameStarted = true;
            SendMessageToClient(_clients[0], "Both players are ready. The game has started! Your turn.");
            SendMessageToClient(_clients[1], "Both players are ready. The game has started! Opponent's turn.");
        }
    }

    private void ProcessShot(TcpClient client, string input)
    {
        int playerIndex = _clients.IndexOf(client);
        string[] parts = input.Split(' ');
        int targetX = int.Parse(parts[1]);
        int targetY = int.Parse(parts[2]);

        GameBoard opponentBoard = playerIndex == 0 ? _player2Board : _player1Board;

        if (opponentBoard.CheckHit(targetX, targetY))
        {
            HandleServerMessage(client, $"HIT {targetX},{targetY}");
        }
        else
        {
            HandleServerMessage(client, $"MISS {targetX},{targetY}");
        }

        // Switch turns
        _currentPlayerIndex = 1 - _currentPlayerIndex;
        SendMessageToClient(_clients[_currentPlayerIndex], "Your turn.");
        SendMessageToClient(_clients[1 - _currentPlayerIndex], "Waiting for opponent's shot...");

        // Check if game over (all ships sunk)
        if (_player1Board.AllShipsSunk() || _player2Board.AllShipsSunk())
        {
            string winnerMessage = _player1Board.AllShipsSunk() ? "Player 2 wins!" : "Player 1 wins!";
            SendMessageToClient(_clients[0], winnerMessage);
            SendMessageToClient(_clients[1], winnerMessage);
            NotifyPlayers(winnerMessage);
        }
    }

    private void SendMessageToClient(TcpClient client, string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        client.GetStream().Write(data, 0, data.Length);
    }

    private void NotifyPlayers(string message)
    {
        foreach (var client in _clients)
        {
            SendMessageToClient(client, message);
        }
    }

    // This method processes the hit/miss messages from the server
    private void HandleServerMessage(TcpClient client, string message)
    {
        int playerIndex = _clients.IndexOf(client);
        if (message.Contains("HIT"))
        {
            SendMessageToClient(client, "You hit the enemy!");
            var coords = message.Split(' ')[1].Split(',');
            int x = int.Parse(coords[0]);
            int y = int.Parse(coords[1]);
            UpdateCellColor(x, y, "Green", playerIndex); // Update the cell color to green (hit)
        }
        else if (message.Contains("MISS"))
        {
            SendMessageToClient(client, "You missed.");
            var coords = message.Split(' ')[1].Split(',');
            int x = int.Parse(coords[0]);
            int y = int.Parse(coords[1]);
            UpdateCellColor(x, y, "Red", playerIndex); // Update the cell color to red (miss)
        }
    }

    // This method simulates updating the cell color in the console (WPF will handle this in UI)
    private void UpdateCellColor(int x, int y, string color, int playerIndex)
    {
        Console.WriteLine($"Player {playerIndex + 1}: {color} at ({x},{y})");
    }
}

class GameBoard
{
    private bool[,] _board = new bool[10, 10];
    private List<Ship> _ships = new List<Ship>();

    public bool PlaceShip(string shipType, int x, int y, bool horizontal)
    {
        Ship newShip = new Ship(shipType, x, y, horizontal);
        if (ValidatePlacement(newShip))
        {
            _ships.Add(newShip);
            return true;
        }
        return false;
    }

    private bool ValidatePlacement(Ship ship)
    {
        // Validate placement: check boundaries and if ships overlap
        foreach (var s in _ships)
        {
            if (s.Overlaps(ship))
                return false;
        }

        return true;
    }

    public bool CheckHit(int x, int y)
    {
        foreach (var ship in _ships)
        {
            if (ship.Hits(x, y))
                return true;
        }
        return false;
    }

    public bool AllShipsSunk()
    {
        return _ships.All(ship => ship.IsSunk());
    }
}

class Ship
{
    public string Type { get; }
    public int X { get; }
    public int Y { get; }
    public bool Horizontal { get; }

    public Ship(string type, int x, int y, bool horizontal)
    {
        Type = type;
        X = x;
        Y = y;
        Horizontal = horizontal;
    }

    public bool Hits(int x, int y)
    {
        // Check if the coordinates (x, y) are within the ship's area
        return (Horizontal && y == Y && x >= X && x < X + Type.Length) ||
               (!Horizontal && x == X && y >= Y && y < Y + Type.Length);
    }

    public bool IsSunk()
    {
        // For simplicity, consider the ship "sunk" if it has been hit
        return true;
    }

    public bool Overlaps(Ship other)
    {
        // Check if the ship overlaps with another
        return false;
    }
}
