using System;
using System.Linq;

namespace TicTacToe
{
    public class GameLogic
    {
        public Winner Check(Player.State[,] array, Player.State player)
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

        private Winner Check(Player.State[,] array, Player.State player, string direction)
        {
            var lineSize = (int)Math.Sqrt(array.Length);
            var winnerInfo = new Winner(lineSize);
            var tmpResult = new string[lineSize, 1];
            var lineCheck = new Player.State[lineSize];
            //todo: if players has not reach 3 selection just return
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
                            lineCheck[i] = array[j, i];
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

        private bool IsFound<T>(T[] array, T find)
        {
            return array.All(s => string.Equals(
                find.ToString(), s.ToString(), StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
