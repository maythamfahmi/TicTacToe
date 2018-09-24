using System;
using System.Linq;
using TicTacToe.Model;
using System.Collections.Generic;

namespace TicTacToe.Algorithm
{
    public class GameLogic
    {
        private readonly string[] _directionsCheck = { "column", "row", "diagTopDown", "diagDownTop" };

        public Winner CheckWinner(Player.State[,] array, Player.State player)
        {
            var result = new Winner();
            if (array == null) return result;
            foreach (var direction in _directionsCheck)
            {
                result = CheckWinner(array, player, direction);
                if (result != null && result.Name != Player.Name.None.ToString())
                    break;
            }
            return result;
        }

        private static Winner CheckWinner(Player.State[,] array, Player.State player, string direction)
        {
            var lineSize = (int)Math.Sqrt(array.Length);
            var result = new Winner(lineSize);
            var tmpResult = new string[lineSize, 1];
            var lineCheck = new Player.State[lineSize];
            if (array.Length <= 2) return result;
            for (var j = 0; j < lineSize; j++)
            {
                switch (direction)
                {
                    case "diagTopDown":
                    case "diagDownTop":
                    {
                        if (direction == "diagTopDown")
                        {
                            lineCheck[j] = array[j, j];
                            tmpResult[j, 0] = $"{j}{j}";
                        }
                        else if (direction == "diagDownTop")
                        {
                            lineCheck[j] = array[j, (lineSize - 1) - j];
                            tmpResult[j, 0] = $"{j}{(lineSize - 1) - j}";
                        }

                        break;
                    }
                    case "column":
                    case "row":
                    {
                        for (var i = 0; i < lineSize; i++)
                        {
                            if (direction == "column")
                            {
                                lineCheck[i] = array[i, j];
                                tmpResult[i, 0] = $"{i}{j}";
                            }
                            else if (direction == "row")
                            {
                                lineCheck[i] = array[j, i];
                                tmpResult[j, 0] = $"{i}{j}";
                            }
                        }

                        break;
                    }
                }

                if (!IsFound(lineCheck, player)) continue;
                result.Name = Player.DefaultPlayers()[player];
                result.Result = tmpResult;
                break;
            }
            return result;
        }

        private static bool IsFound<T>(IEnumerable<T> data, T find)
        {
            return data.All(s => string.Equals(
                find.ToString(), s.ToString(), StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
