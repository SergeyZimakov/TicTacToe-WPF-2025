using TicTacToe.Enums;

namespace TicTacToe.Classes
{
    public class Player
    {
        public required string Name { get; set; }
        public SymbolTypeEnum Symbol { get; set; }
        public bool IsComputer { get; set; }
    }
}
