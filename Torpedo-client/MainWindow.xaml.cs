using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Torpedo_client
{
    public partial class MainWindow : Window
    {
        private string _currentShip = null;  // The current selected ship
        private int _remainingShips = 5;  // Number of ships remaining to place
        private List<string> _shipsPlaced = new List<string>();  // Track placed ships
        private Dictionary<string, int> _shipSizes = new Dictionary<string, int>()
        {
            { "Aircraft Carrier", 5 },
            { "Battleship", 4 },
            { "Cruiser", 3 },
            { "Submarine", 3 },
            { "Destroyer", 2 }
        };
        private List<Button> _occupiedCells = new List<Button>();  // Cells occupied by ships
        private bool _isHorizontal = true;  // Ship orientation (true for horizontal, false for vertical)
        private TcpClient _tcpClient;
        private NetworkStream _networkStream;
        private bool _isConnected = false;

        public MainWindow()
        {
            InitializeComponent();
            CreateBoard();
            this.KeyDown += OnKeyDown;  // Listen for the "R" key press
            ConnectToServer();  // Try connecting to the server when the app starts
        }

        // Create the game board
        private void CreateBoard()
        {
            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    Button cellButton = new Button
                    {
                        Name = $"Cell_{row}_{col}",
                        Width = 40,
                        Height = 40,
                        Background = Brushes.LightGray,
                        Margin = new Thickness(1)
                    };
                    cellButton.Click += OnCellClick;
                    cellButton.ContextMenuOpening += OnCellRightClick; // Listen for right-click
                    Grid.SetRow(cellButton, row);
                    Grid.SetColumn(cellButton, col);
                    PlayerGrid.Children.Add(cellButton);
                }
            }
        }

        // Connect to the server
        private async void ConnectToServer()
        {
            try
            {
                _tcpClient = new TcpClient("127.0.0.1", 5000);
                _networkStream = _tcpClient.GetStream();
                _isConnected = true;
                Dispatcher.Invoke(() => StatusText.Text = "Connected to the server");

                // Start listening for server messages asynchronously
                await Task.Run(() => ListenForServerMessages());
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => StatusText.Text = $"Connection failed: {ex.Message}");
            }
        }

        // Listen for messages from the server
        private async Task ListenForServerMessages()
        {
            byte[] buffer = new byte[256];
            while (_isConnected)
            {
                try
                {
                    int bytesRead = await _networkStream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Dispatcher.Invoke(() =>
                        {
                            StatusText.Text = $"Server: {message}";
                        });
                    }
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        StatusText.Text = $"Error receiving data: {ex.Message}";
                    });
                    break; // Exit loop on error (connection might be lost)
                }
            }
        }

        // When the user selects a ship
        private void OnShipSelection(object sender, RoutedEventArgs e)
        {
            Button shipButton = sender as Button;
            string shipName = shipButton.Content.ToString().Split('(')[0].Trim();
            _currentShip = shipName; // Set the current ship
        }

        // Handle right-click to rotate the ship
        private void OnCellRightClick(object sender, RoutedEventArgs e)
        {
            RotateShip();
        }

        // Handle "R" key press to rotate the ship
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.R)
            {
                RotateShip();
            }
        }

        // Rotate the ship (change orientation)
        private void RotateShip()
        {
            _isHorizontal = !_isHorizontal;  // Toggle orientation (horizontal/vertical)
        }

        // Handle cell clicks for ship placement
        private void OnCellClick(object sender, RoutedEventArgs e)
        {
            if (_currentShip != null && !_shipsPlaced.Contains(_currentShip))  // Only allow placement if ship is selected and not placed
            {
                Button clickedButton = sender as Button;
                int x = Grid.GetColumn(clickedButton);
                int y = Grid.GetRow(clickedButton);
                int shipSize = _shipSizes[_currentShip];  // Get the size of the selected ship

                // Try to place the ship in the current orientation (horizontal or vertical)
                if (TryPlaceShip(x, y, shipSize, _isHorizontal))
                {
                    PlaceShip(x, y, shipSize, _isHorizontal);
                    SendShipPlacementToServer(x, y, shipSize, _isHorizontal);
                }
                else
                {
                    MessageBox.Show("Cannot place the ship here. Try another position.");
                }
            }
        }

        // Try placing the ship on the grid (horizontal or vertical)
        private bool TryPlaceShip(int x, int y, int shipSize, bool isHorizontal)
        {
            // Check if the ship fits within the grid boundaries
            if (isHorizontal)
            {
                if (x + shipSize > 10) return false;  // Out of bounds horizontally
                for (int i = 0; i < shipSize; i++)
                {
                    if (_occupiedCells.Contains(GetButtonAt(x + i, y)))  // Check if any cell is already occupied
                        return false;

                    // Check surrounding cells for adjacency
                    if (IsAdjacentOccupied(x + i, y))
                        return false;
                }
            }
            else
            {
                if (y + shipSize > 10) return false;  // Out of bounds vertically
                for (int i = 0; i < shipSize; i++)
                {
                    if (_occupiedCells.Contains(GetButtonAt(x, y + i)))  // Check if any cell is already occupied
                        return false;

                    // Check surrounding cells for adjacency
                    if (IsAdjacentOccupied(x, y + i))
                        return false;
                }
            }
            return true;
        }

        // Check if any adjacent cells are occupied (side or corner)
        private bool IsAdjacentOccupied(int x, int y)
        {
            // Check surrounding 8 cells (side and corner cells)
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    // Skip the cell itself
                    if (dx == 0 && dy == 0)
                        continue;

                    int newX = x + dx;
                    int newY = y + dy;

                    if (newX >= 0 && newX < 10 && newY >= 0 && newY < 10)
                    {
                        Button neighborButton = GetButtonAt(newX, newY);
                        if (_occupiedCells.Contains(neighborButton))
                        {
                            return true;  // Adjacent cell is occupied
                        }
                    }
                }
            }
            return false;
        }

        // Place the ship on the grid (mark cells as occupied)
        private void PlaceShip(int x, int y, int shipSize, bool isHorizontal)
        {
            for (int i = 0; i < shipSize; i++)
            {
                Button cell = isHorizontal ? GetButtonAt(x + i, y) : GetButtonAt(x, y + i);
                cell.Background = Brushes.Blue;  // Mark the cell as occupied
                _occupiedCells.Add(cell);  // Track the occupied cells
            }

            _shipsPlaced.Add(_currentShip);  // Track that the ship is placed
            _remainingShips--;  // Decrease remaining ships count

            // If all ships are placed, enable the Ready button
            if (_remainingShips == 0)
            {
                ReadyButton.IsEnabled = true;
            }
        }

        // Get the button at a specific grid position (x, y)
        private Button GetButtonAt(int x, int y)
        {
            return PlayerGrid.Children
                .OfType<Button>()
                .FirstOrDefault(btn => Grid.GetRow(btn) == y && Grid.GetColumn(btn) == x);
        }

        // Ready button clicked (Start the game)
        private void OnReadyButtonClick(object sender, RoutedEventArgs e)
        {
            if (_shipsPlaced.Count == 5)  // Check if all ships are placed
            {
                ReadyButton.IsEnabled = false;  // Disable Ready button after clicking
                StatusText.Text = "Waiting for opponent...";
                SendReadyMessageToServer(); // Send ready message to the server
            }
            else
            {
                MessageBox.Show("You must place all ships before readying up!");
            }
        }

        // Send ship placement to the server
        private void SendShipPlacementToServer(int x, int y, int shipSize, bool isHorizontal)
        {
            if (_isConnected)
            {
                string message = $"PLACE_SHIP {_currentShip} {x} {y} {isHorizontal}";
                byte[] data = Encoding.UTF8.GetBytes(message);
                _networkStream.Write(data, 0, data.Length);
                StatusText.Text = $"Placed ship: {_currentShip} at ({x}, {y}), orientation: {isHorizontal}";
            }
        }

        // Send ready message to the server
        private void SendReadyMessageToServer()
        {
            if (_isConnected)
            {
                string message = "READY";
                byte[] data = Encoding.UTF8.GetBytes(message);
                _networkStream.Write(data, 0, data.Length);
            }
        }
    }
}
