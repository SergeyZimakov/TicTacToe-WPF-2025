using TicTacToe.Classes;

namespace TicTacToe.Dto
{
    public class GameStatusDto
    {
        public bool IsGameFinished { get; set; }
        public bool IsDraw { get; set; }
        public List<int> WinLine { get; set; } = [];
    }
}
