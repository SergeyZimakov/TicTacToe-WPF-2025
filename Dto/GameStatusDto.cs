using TicTacToe.Classes;

namespace TicTacToe.Dto
{
    public class GameStatusDto
    {
        public bool IsGameFinished { get; set; }
        public Player? Winner { get; set; } = null;
        public List<int> WinLine { get; set; } = [];
    }
}
