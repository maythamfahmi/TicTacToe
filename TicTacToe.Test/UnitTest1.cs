using System;
using Shouldly;
using NUnit.Framework;
using System.Collections.Generic;
using TicTacToe.Algorithm;
using TicTacToe.Model;

namespace TicTacToe.Test
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void WarmUp()
        {
            Console.WriteLine("The is test is warming");
        }

        [TestCaseSource("GameLogicTestCases")]
        public void TestGameLogic(GameLogicTestCase g)
        {
            var result = new GameLogic().CheckWinner(g.BoardSize, g.ExpectedWinner);
            Console.WriteLine(g.TestName);
            result.Name.ShouldBe(Player.DefaultPlayers()[g.ExpectedWinner]);
        }

        private static IEnumerable<GameLogicTestCase> GameLogicTestCases()
        {
            return new List<GameLogicTestCase>()
            {
                new GameLogicTestCase("01- Column test", new Player.State[3, 3] {
                    { Player.State.O, Player.State.X, Player.State.O },
                    { Player.State.O, Player.State.X, Player.State.X },
                    { Player.State.O, Player.State.None, Player.State.None }
                }, Player.State.O),
                new GameLogicTestCase("02- Row test", new Player.State[3, 3] {
                    { Player.State.O, Player.State.O, Player.State.O },
                    { Player.State.X, Player.State.X, Player.State.None },
                    { Player.State.None, Player.State.None, Player.State.X }
                }, Player.State.O),
                new GameLogicTestCase("03- Diagnol Top Down test", new Player.State[3, 3] {
                    { Player.State.O, Player.State.X, Player.State.X },
                    { Player.State.X, Player.State.O, Player.State.None },
                    { Player.State.None, Player.State.None, Player.State.O }
                }, Player.State.O),
                new GameLogicTestCase("04- Diagnol Down Top test", new Player.State[3, 3] {
                    { Player.State.None, Player.State.X, Player.State.O },
                    { Player.State.X, Player.State.O, Player.State.None },
                    { Player.State.O, Player.State.None, Player.State.None }
                }, Player.State.O),
                new GameLogicTestCase("05- None winner test", new Player.State[3, 3] {
                    { Player.State.O, Player.State.X, Player.State.O },
                    { Player.State.X, Player.State.O, Player.State.X },
                    { Player.State.O, Player.State.X, Player.State.O }
                }, Player.State.None),
                new GameLogicTestCase("06- Empty test", new Player.State[3, 3] {
                    { Player.State.None, Player.State.None, Player.State.None },
                    { Player.State.None, Player.State.None, Player.State.None },
                    { Player.State.None, Player.State.None, Player.State.None }
                }, Player.State.None),
                new GameLogicTestCase("07- Null test", null, Player.State.None),
                new GameLogicTestCase("08- Full test", new Player.State[3, 3] {
                    { Player.State.X, Player.State.X, Player.State.X },
                    { Player.State.X, Player.State.X, Player.State.X },
                    { Player.State.X, Player.State.X, Player.State.X }
                }, Player.State.X),
                new GameLogicTestCase("09- Few selected test", new Player.State[3, 3] {
                    { Player.State.X, Player.State.X, Player.State.None },
                    { Player.State.None, Player.State.None, Player.State.None },
                    { Player.State.None, Player.State.None, Player.State.None }
                }, Player.State.None),
                new GameLogicTestCase("10- X winner test", new Player.State[3, 3] {
                    { Player.State.X, Player.State.O, Player.State.O },
                    { Player.State.O, Player.State.X, Player.State.X },
                    { Player.State.O, Player.State.None, Player.State.X }
                }, Player.State.X),
            };
        }
    }

    public class GameLogicTestCase
    {
        public string TestName { get; set; }
        public Player.State[,] BoardSize { get; set; }
        public Player.State ExpectedWinner { get; set; }

        public GameLogicTestCase(string testName, Player.State[,] boardSize, Player.State expectedWinner)
        {
            TestName = testName;
            BoardSize = boardSize;
            ExpectedWinner = expectedWinner;
        }
    }
}
