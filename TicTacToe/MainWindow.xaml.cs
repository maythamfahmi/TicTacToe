using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int BoardSize = 4;
        private int BrickSize = 75;
        private int BrickSpace = 10;
        private Player.Name turn;
        private bool GameStart = true;
        private bool GameEnd = false;

        private BitmapImage empty = new BitmapImage(new Uri(@"C:\\dev\\Projects\\TicTacToe\\TicTacToe\\Resources\\empty.png"));
        private BitmapImage x = new BitmapImage(new Uri(@"C:\\dev\\Projects\\TicTacToe\\TicTacToe\\Resources\\x.png"));
        private BitmapImage o = new BitmapImage(new Uri(@"C:\\dev\\Projects\\TicTacToe\\TicTacToe\\Resources\\o.png"));
        private BitmapImage bg = new BitmapImage(new Uri(@"C:\\dev\\Projects\\TicTacToe\\TicTacToe\\Resources\\bg.png"));

        private Shape Brick { get; set; }
        Player.State[,] boardState;

        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Title = "Tic Tac Toe";
            CreateBoard(BoardSize);
            Init();
        }

        private void OnCreateNewGame(object sender, RoutedEventArgs e)
        {
            GameStart = true;
            Init();
        }

        private void Init()
        {
            boardState = new Player.State[BoardSize, BoardSize];
            CreateBoard(BoardSize);
            player1.Source = x;
            player2.Source = o;
            GameEnd = false;
            turn = Player.Name.Player1;
            ShowPlayerTurn();
        }

        void OnMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            var shape = (Shape)sender;
            if (!GameStart) return;
            if (GameEnd) return;

            var pos = ParseCurrentPosition(shape.Name);
            int i = pos[0];
            int j = pos[1];

            //todo: Make color global
            if (shape.Fill.ToString() == "#FF0000FF")
            {
                if (turn == Player.Name.Player1)
                {
                    shape.Fill = new ImageBrush(o);
                    turn = Player.Name.Player2;
                    boardState[i, j] = Player.State.O;
                }
                else if (turn == Player.Name.Player2)
                {
                    shape.Fill = new ImageBrush(x);
                    turn = Player.Name.Player1;
                    boardState[i, j] = Player.State.X;
                }
            }

            var result = new GameLogic().Check(boardState, boardState[i, j]);

            ShowPlayerTurn();

            if (result.Name != Player.Name.None.ToString())
            {
                GameEnd = true;
                GameStart = false;
                ShowPlayerTurn($"The Winner is {result.Name}");
            }

        }

        public Player.Name GetPlayerTurn()
        {
            return turn;
        }

        private int[] ParseCurrentPosition(string shapeName)
        {
            if (shapeName == null) return null;
            int from = shapeName.IndexOf("_") + 1;
            int to = shapeName.LastIndexOf("_") + 1;
            int x, y = 0;
            int.TryParse(shapeName.Substring(from, (to - from - 1)), out x);
            int.TryParse(shapeName.Substring(to), out y);
            if (!(x >= 0 && x <= BoardSize && x >= 0 && y <= BoardSize)) return null;
            return new[] { x, y };
        }

        private void ShowPlayerTurn(string winner = "")
        {
            playerTurn.Content = $"{turn} {winner}";
        }

        private void CreateBoard(int boardSize)
        {
            StackPanel brickStackPanel = new StackPanel
            {
                Height = BrickSpace + (BrickSpace * boardSize) + (BrickSize * boardSize),
                Width = BrickSpace + (BrickSpace * boardSize) + (BrickSize * boardSize),
                Margin = new Thickness(0, 0, 0, 0),
                Background = new SolidColorBrush(Colors.White),
            };

            for (int bx = 0; bx < boardSize; bx++)
            {
                StackPanel rowStackPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                for (int by = 0; by < boardSize; by++)
                {
                    Brick = new Rectangle
                    {
                        Fill = new SolidColorBrush(Colors.Blue),
                        Height = BrickSize,
                        Width = BrickSize,
                        Margin = new Thickness(10, 10, 0, 0),
                        Stroke = new SolidColorBrush(Colors.Black),
                        StrokeThickness = 3,
                        Name = $"Box_{bx}_{by}",
                    };
                    Brick.MouseUp += OnMouseButtonUp;

                    rowStackPanel.Children.Add(Brick);
                }
                brickStackPanel.Children.Add(rowStackPanel);
            }

            boardStackPanel.Children.Clear();
            boardStackPanel.Children.Add(brickStackPanel);
        }
    }
}
