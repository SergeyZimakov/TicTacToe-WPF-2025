namespace TicTacToe.Enums
{
    public enum SymbolTypeEnum
    {
        X,
        O,
    }

    public static class SymbolTypeEnumExtensions
    {
        public static string GetAsString(this SymbolTypeEnum symbolTypeEnum) => symbolTypeEnum switch
        {
            SymbolTypeEnum.X => "X",
            SymbolTypeEnum.O => "O",
            _ => throw new NotImplementedException()
        };

        public static SymbolTypeEnum GetNextTurn(this SymbolTypeEnum symbolTypeEnum)
        {
            return symbolTypeEnum == SymbolTypeEnum.X ? SymbolTypeEnum.O : SymbolTypeEnum.X;
        }
    }
}
