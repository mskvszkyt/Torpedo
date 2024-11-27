using System;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ShipPlacement
{
    public partial class MainWindow : Window
    {
        private readonly Dictionary<string, bool> placedShips = new()
        {
            { "AircraftCarrier", false },
            { "Battleship", false },
            { "Submarine", false },
            { "Cruiser", false },
            { "Destroyer", false }
        };

        private bool isVertical = true; // Tracks rotation (vertical by default)
        private TcpClient client;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Ship_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                TextBlock ship = sender as TextBlock;
                if (ship != null)
                {
                    DragDrop.DoDragDrop(ship, ship.Tag.ToString(), DragDropEffects.Move);
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.R)
            {
                isVertical = !isVertical;
            }
        }

        private void LeftGrid_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                string shipTag = e.Data.GetData(DataFormats.StringFormat) as string;
                if (shipTag == null || placedShips[shipTag])
                {
                    MessageBox.Show("This ship has already been placed!");
                    return;
                }

                int size = GetShipSize(shipTag);
                Point dropPosition = e.GetPosition(LeftGrid);
                int row = (int)(dropPosition.Y / (LeftGrid.ActualHeight / 10));
                int column = (int)(dropPosition.X / (LeftGrid.ActualWidth / 10));

                if (!IsPlacementValid(row, column, size))
                {
                    MessageBox.Show("Invalid placement. Ships cannot touch each other!");
                    return;
                }

                PlaceShip(row, column, size);
                placedShips[shipTag] = true;

                // Enable Ready button if all ships are placed
                if (AllShipsPlaced())
                {
                    ReadyButton.IsEnabled = true;
                }
            }
        }

        private int GetShipSize(string shipTag) => shipTag switch
        {
            "AircraftCarrier" => 5,
            "Battleship" => 4,
            "Submarine" => 3,
            "Cruiser" => 3,
            "Destroyer" => 2,
            _ => 0
        };

        private bool IsPlacementValid(int row, int column, int size)
        {
            if (isVertical)
            {
                if (row + size > 10) return false;
                for (int i = 0; i < size; i++)
                {
                    if (CheckAdjacentCells(row + i, column)) return false;
                }
            }
            else
            {
                if (column + size > 10) return false;
                for (int i = 0; i < size; i++)
                {
                    if (CheckAdjacentCells(row, column + i)) return false;
                }
            }

            return true;
        }

        private bool CheckAdjacentCells(int row, int column)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (row + i < 0 || row + i >= 10 || column + j < 0 || column + j >= 10) continue;
                    foreach (UIElement child in LeftGrid.Children)
                    {
                        if (Grid.GetRow(child) == row + i && Grid.GetColumn(child) == column + j)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private void PlaceShip(int row, int column, int size)
        {
            for (int i = 0; i < size; i++)
            {
                Rectangle rect = new Rectangle
                {
                    Fill = Brushes.LightBlue,
                    Stroke = Brushes.Black
                };
                if (isVertical)
                {
                    Grid.SetRow(rect, row + i);
                    Grid.SetColumn(rect, column);
                }
                else
                {
                    Grid.SetRow(rect, row);
                    Grid.SetColumn(rect, column + i);
                }
                LeftGrid.Children.Add(rect);
            }
        }

        private bool AllShipsPlaced()
        {
            foreach (var ship in placedShips.Values)
            {
                if (!ship) return false;
            }
            return true;
        }

        private void ReadyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Initialize TCP client
                client = new TcpClient("127.0.0.1", 12345);
                MessageBox.Show("Connected to server!");

                // Send "Ready" message
                string message = "Ready";
                byte[] data = Encoding.ASCII.GetBytes(message);
                client.GetStream().Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to connect to server: {ex.Message}");
            }
        }
    }
}
