using System.Collections.ObjectModel;
using System.Windows;
using TicTacToe.Classes;
using TicTacToe.Enums;
using TicTacToe.Helpers;

namespace TicTacToe;
public partial class MainWindow : Window
{
    public ObservableCollection<GameCell> GameCells { get; set; } = new ObservableCollection<GameCell>();
    public SymbolTypeEnum currTurnSymbol = SymbolTypeEnum.X;
    private readonly List<List<int>> _winLines =
    [
        [1,2,3],[4,5,6],[7,8,9],
        [1,4,7],[2,5,8],[3,6,9],
        [1,5,9],[3,5,7],
    ];
    public MainWindow()
    {
        InitializeComponent();
        foreach (var idx in Enumerable.Range(1, 9))
        {
            var gameCell = new GameCell { Number = idx, Symbol = Enums.SymbolTypeEnum.NotFilled };
            gameCell.ClickCommand = new RelayCommand(() => OnGameCellClicked(gameCell));
            GameCells.Add(gameCell);
        }
        DataContext = this;
    }

    public void OnGameCellClicked(GameCell gameCell)
    {
        if (gameCell.Symbol == currTurnSymbol) return;

        gameCell.Symbol = currTurnSymbol;
        currTurnSymbol = currTurnSymbol.GetNextTurn();

        var winner = CheckWinner();
        if (winner == SymbolTypeEnum.NotFilled) return;

        WinnerTxt.Text = $"{winner.GetAsString()} Win";
    }

    private SymbolTypeEnum CheckWinner()
    {
        var xList = GameCells.Where(c => c.Symbol == SymbolTypeEnum.X).Select(c => c.Number).ToHashSet();
        var oList = GameCells.Where(c => c.Symbol == SymbolTypeEnum.O).Select(c => c.Number).ToHashSet();

        foreach (var winLine in _winLines)
        {
            if (winLine.All(xList.Contains)) return SymbolTypeEnum.X;
            if (winLine.All(oList.Contains)) return SymbolTypeEnum.O;
        }

        return SymbolTypeEnum.NotFilled;
    }
}