namespace TicTacToe
{
    public class Winner
    {
        public string Name { get; set; }
        public string[,] Result { get; set; }

        public Winner()
        {
            Name = Player.Name.None.ToString();
        }

        public Winner(int tileSize)
        {
            Name = Player.Name.None.ToString();
            Result = new string[tileSize, 1];
        }
    }
}
