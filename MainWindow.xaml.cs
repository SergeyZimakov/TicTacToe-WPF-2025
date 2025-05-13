using System.Collections.ObjectModel;
using System.Windows;
using TicTacToe.Classes;
using TicTacToe.Enums;
using TicTacToe.Helpers;

namespace TicTacToe;
public partial class MainWindow : Window
{
    public ObservableCollection<GameCell> GameCells { get; set; } = [];
    public List<Player> Players { get; set; } =
    [
        new() { Name = "Player 1", Symbol = SymbolTypeEnum.X},
        new() { Name = "Player 2", Symbol = SymbolTypeEnum.O},
    ];
    private int CurrPlayerIdx { get; set; } = 0;
    private bool IsGameActive { get; set; } = true;
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
            var gameCell = new GameCell { Number = idx, Symbol = null };
            gameCell.ClickCommand = new RelayCommand(
                () => OnGameCellClicked(gameCell),
                () => IsGameActive && !gameCell.Symbol.HasValue
            );
            GameCells.Add(gameCell);
        }
        PrintCurrentPlayerTurn();
        DataContext = this;
    }

    public void OnGameCellClicked(GameCell gameCell)
    {
        gameCell.Symbol = Players[CurrPlayerIdx].Symbol;

        var winner = GetWinner();
        if (winner.HasValue)
        {
            IsGameActive = false;
            foreach (var item in GameCells) item.ClickCommand.RaiseCanExecuteChanged();
            GameMessageTxt.Text = $"{Players[CurrPlayerIdx].Name} Win";
            return;
        }

        SwitchPlayer();
        PrintCurrentPlayerTurn();
    }

    private SymbolTypeEnum? GetWinner()
    {
        var xList = GameCells.Where(c => c.Symbol == SymbolTypeEnum.X).Select(c => c.Number).ToHashSet();
        var oList = GameCells.Where(c => c.Symbol == SymbolTypeEnum.O).Select(c => c.Number).ToHashSet();

        foreach (var winLine in _winLines)
        {
            if (winLine.All(xList.Contains)) return SymbolTypeEnum.X;
            if (winLine.All(oList.Contains)) return SymbolTypeEnum.O;
        }

        return null;
    }

    private void PrintCurrentPlayerTurn()
    {
        var currPlayer = Players[CurrPlayerIdx];
        GameMessageTxt.Text = $"{currPlayer.Name} turn({currPlayer.Symbol.GetAsString()})";
    }

    private void SwitchPlayer()
    {
        CurrPlayerIdx = CurrPlayerIdx == 0 ? 1 : 0;
    }
}