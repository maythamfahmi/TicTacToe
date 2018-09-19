using System;
using System.Linq;

namespace TicTacToe.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Player.State[,] BoardSize = new Player.State[3, 3];
            BoardSize[0, 0] = Player.State.O;
            BoardSize[0, 1] = Player.State.X;
            BoardSize[0, 2] = Player.State.X;
            BoardSize[1, 0] = Player.State.O;
            BoardSize[1, 1] = Player.State.X;
            BoardSize[1, 2] = Player.State.X;
            BoardSize[2, 0] = Player.State.O;
            BoardSize[2, 1] = Player.State.X;
            BoardSize[2, 2] = Player.State.X;

            var result = Check(BoardSize, Player.State.O);

            Console.WriteLine(result.Name);
            foreach (var item in result.Result)
            {
                Console.WriteLine(item);
            }
        }

        static Winner Check(Player.State[,] array, Player.State player)
        {
            var directions = new[] { "column", "row", "diagTopDown", "diagDownTop" };
            Winner result = null;
            foreach (var direction in directions)
            {
                result = Check(array, player, direction);
                if (result != null && result.Name != Player.Name.None.ToString())
                    break;
            }
            return result;
        }

        private static Winner Check(Player.State[,] array, Player.State player, string direction)
        {
            var lineSize = (int)Math.Sqrt(array.Length);
            var winnerInfo = new Winner(lineSize);
            var tmpResult = new string[lineSize, 1];
            var lineCheck = new Player.State[lineSize];
            for (int j = 0; j < lineSize; j++)
            {
                if (direction == "diagTopDown" || direction == "diagDownTop")
                {
                    if (direction == "diagTopDown")
                    {
                        lineCheck[j] = array[j, j];
                        tmpResult[j, 0] = $"{j}{j}";
                    }
                    if (direction == "diagDownTop")
                    {
                        lineCheck[j] = array[j, (lineSize - 1) - j];
                        tmpResult[j, 0] = $"{j}{(lineSize - 1) - j}";
                    }
                }
                if (direction == "column" || direction == "row")
                {
                    for (int i = 0; i < lineSize; i++)
                    {
                        if (direction == "column")
                        {
                            lineCheck[i] = array[i, j];
                            tmpResult[i, 0] = $"{i}{j}";
                        }
                        if (direction == "row")
                        {
                            lineCheck[j] = array[j, i];
                            tmpResult[j, 0] = $"{i}{j}";
                        }
                    }
                }
                if (IsFound(lineCheck, player))
                {
                    winnerInfo.Name = Player.DefaultPlayers()[player];
                    winnerInfo.Result = tmpResult;
                    break;
                }
            }
            return winnerInfo;
        }

        private static bool IsFound<T>(T[] array, T find)
        {
            return array.All(s => string.Equals(
                find.ToString(), s.ToString(), StringComparison.InvariantCultureIgnoreCase));
        }

    }
}
