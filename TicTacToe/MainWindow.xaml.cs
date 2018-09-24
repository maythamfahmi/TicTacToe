using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TicTacToe.Algorithm;
using TicTacToe.Model;

namespace TicTacToe
{
    /// <inheritdoc />
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    internal partial class MainWindow : Window
    {
        private const int BoardSize = 3;
        private const int BrickSize = 75;
        private const int BrickSpace = 10;
        private bool _gameStart;
        private bool _gameEnd;
        private Player.Name _turn;
        private Player.State[,] _boardState;
        private Shape Brick { get; set; }
        private const string BoardBackgroundColor = "#FFFFFF";
        private const string BrickFillColor = "#FFFFFF";
        private const string BrickBorderColor = "#000000";
        private readonly BitmapImage _x = new BitmapImage(new Uri(@"C:\\dev\\Projects\\TicTacToe\\TicTacToe\\Resources\\x.png"));
        private readonly BitmapImage _o = new BitmapImage(new Uri(@"C:\\dev\\Projects\\TicTacToe\\TicTacToe\\Resources\\o.png"));

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
            _gameStart = true;
            Init();
            ShowPlayerTurn();
        }

        private void Init()
        {
            _boardState = new Player.State[BoardSize, BoardSize];
            CreateBoard(BoardSize);
            player1.Source = _x;
            player2.Source = _o;
            _gameEnd = false;
            _turn = Player.Name.Player1;
            ShowPlayerTurn("Click on Button and start New Game");
        }

        private void OnMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            var shape = (Shape)sender;
            if (!_gameStart) return;
            if (_gameEnd) return;

            var pos = ParseCurrentPosition(shape.Name);
            var i = pos[0];
            var j = pos[1];

            if (HexToSolidColor(shape.Fill.ToString()).ToString() ==
                HexToSolidColor(BrickFillColor).ToString())
            {
                switch (_turn)
                {
                    case Player.Name.Player1:
                        shape.Fill = new ImageBrush(_o);
                        _turn = Player.Name.Player2;
                        _boardState[i, j] = Player.State.O;
                        break;
                    case Player.Name.Player2:
                        shape.Fill = new ImageBrush(_x);
                        _turn = Player.Name.Player1;
                        _boardState[i, j] = Player.State.X;
                        break;
                    case Player.Name.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            var result = new GameLogic().CheckWinner(_boardState, _boardState[i, j]);

            ShowPlayerTurn();

            if (result.Name == Player.Name.None.ToString()) return;
            _gameEnd = true;
            _gameStart = false;
            ShowPlayerTurn($"(: {_turn} Wins :)");
        }

        private static int[] ParseCurrentPosition(string shapeName)
        {
            if (shapeName == null) return null;
            var from = shapeName.IndexOf("_", StringComparison.Ordinal) + 1;
            var to = shapeName.LastIndexOf("_", StringComparison.Ordinal) + 1;
            int.TryParse(shapeName.Substring(from, (to - from - 1)), out var i);
            int.TryParse(shapeName.Substring(to), out var j);
            if (!(i >= 0 && i <= BoardSize && i >= 0 && j <= BoardSize)) return null;
            return new[] { i, j };
        }

        private void ShowPlayerTurn(string text = null)
        {
            playerTurn.Text = string.IsNullOrWhiteSpace(text) ?
                $"Player turn: {_turn}" : $"{text}";
        }

        private void CreateBoard(int boardSize)
        {
            var brickStackPanel = new StackPanel
            {
                Height = BrickSpace + (BrickSpace * boardSize) + (BrickSize * boardSize),
                Width = BrickSpace + (BrickSpace * boardSize) + (BrickSize * boardSize),
                Margin = new Thickness(0, 0, 0, 0),
                Background = HexToSolidColor(BoardBackgroundColor),
            };

            for (var bx = 0; bx < boardSize; bx++)
            {
                var rowStackPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                for (var by = 0; by < boardSize; by++)
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

        private static SolidColorBrush HexToSolidColor(string c)
        {
            return (SolidColorBrush)(new BrushConverter().ConvertFrom(c));
        }
    }
}
