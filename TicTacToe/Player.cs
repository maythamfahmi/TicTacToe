using System.Collections.Generic;

namespace TicTacToe
{
    public static class Player
    {
        public static Dictionary<State, string> DefaultPlayers()
        {
            return new Dictionary<State, string>
            {
                { State.None, Name.None.ToString() },
                { State.O, Name.Player1.ToString() },
                { State.X, Name.Player2.ToString() }
            };
        }

        public enum State
        {
            None,
            X,
            O
        }

        public enum Name
        {
            None,
            Player1,
            Player2
        }
    }
}
