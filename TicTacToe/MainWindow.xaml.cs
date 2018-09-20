using System;
using System.Windows;
using System.Windows.Controls;
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
        private int BoardSize = 3;
        private int BrickSize = 75;
        private int BrickSpace = 10;
        private bool GameStart = false;
        private bool GameEnd = false;
        private Player.Name turn;
        private Shape Brick { get; set; }
        Player.State[,] boardState;
                private string BoardBackgroundColor = "#FFFFFF";
        private string BrickFillColor = "#FFFFFF";
        private string BrickBorderColor = "#000000";
        private BitmapImage empty = new BitmapImage(new Uri(@"C:\\dev\\Projects\\TicTacToe\\TicTacToe\\Resources\\empty.png"));
        private BitmapImage x = new BitmapImage(new Uri(@"C:\\dev\\Projects\\TicTacToe\\TicTacToe\\Resources\\x.png"));
        private BitmapImage o = new BitmapImage(new Uri(@"C:\\dev\\Projects\\TicTacToe\\TicTacToe\\Resources\\o.png"));
        
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
            ShowPlayerTurn();
        }

        private void Init()
        {
            boardState = new Player.State[BoardSize, BoardSize];
            CreateBoard(BoardSize);
            player1.Source = x;
            player2.Source = o;
            GameEnd = false;
            turn = Player.Name.Player1;
            ShowPlayerTurn("Click on Button and start New Game");
        }

        void OnMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            var shape = (Shape)sender;
            if (!GameStart) return;
            if (GameEnd) return;

            var pos = ParseCurrentPosition(shape.Name);
            int i = pos[0];
            int j = pos[1];

            if (HexToSolidColor(shape.Fill.ToString()).ToString() ==
                HexToSolidColor(BrickFillColor).ToString())
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
                ShowPlayerTurn($"(: {turn} Wins :)");
            }
        }

        private int[] ParseCurrentPosition(string shapeName)
        {
            if (shapeName == null) return null;
            int from = shapeName.IndexOf("_") + 1;
            int to = shapeName.LastIndexOf("_") + 1;
            int.TryParse(shapeName.Substring(from, (to - from - 1)), out int x);
            int.TryParse(shapeName.Substring(to), out int y);
            if (!(x >= 0 && x <= BoardSize && x >= 0 && y <= BoardSize)) return null;
            return new[] { x, y };
        }

        private void ShowPlayerTurn(string text = null)
        {
            playerTurn.Text = string.IsNullOrWhiteSpace(text) ? 
                $"Player turn: {turn}" : $"{text}";
        }

        private void CreateBoard(int boardSize)
        {
            StackPanel brickStackPanel = new StackPanel
            {
                Height = BrickSpace + (BrickSpace * boardSize) + (BrickSize * boardSize),
                Width = BrickSpace + (BrickSpace * boardSize) + (BrickSize * boardSize),
                Margin = new Thickness(0, 0, 0, 0),
                Background = HexToSolidColor(BoardBackgroundColor),
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
                        Fill = HexToSolidColor(BrickFillColor),
                        Height = BrickSize,
                        Width = BrickSize,
                        Margin = new Thickness(10, 10, 0, 0),
                        Stroke = HexToSolidColor(BrickBorderColor),
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

        private SolidColorBrush HexToSolidColor(string c)
        {
            return (SolidColorBrush)(new BrushConverter().ConvertFrom(c));
        }
    }
}
