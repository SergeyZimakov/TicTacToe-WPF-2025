using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using TicTacToe.Classes;
using TicTacToe.Dto;
using TicTacToe.Enums;
using TicTacToe.Helpers;

namespace TicTacToe.Pages
{
    
    public partial class GamePage : Page
    {
        public ObservableCollection<GameCell> GameCells { get; set; } = [];
        public List<Player> Players { get; set; } = [];
        private int CurrPlayerIdx { get; set; } = 0;
        private bool IsGameActive { get; set; } = true;
        private readonly List<List<int>> _winLines =
        [
            [1,2,3],[4,5,6],[7,8,9],
            [1,4,7],[2,5,8],[3,6,9],
            [1,5,9],[3,5,7],
        ];
        private readonly Frame _frame;
        public GamePage(Frame frame, Player player1, Player player2)
        {
            InitializeComponent();
            _frame = frame;

            Players.Add(player1);
            Players.Add(player2);
            Loaded += GamePage_Loaded;

            DataContext = this;
        }

        private async void GamePage_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= GamePage_Loaded;
            await StartNewGame();
        }

        public async Task OnGameCellClicked(GameCell gameCell)
        {
            gameCell.Symbol = Players[CurrPlayerIdx].Symbol;

            var gameStatusDto = GetGameStatusDto();
            if (gameStatusDto.IsGameFinished)
            {
                IsGameActive = false;
                foreach (var item in GameCells)
                {
                    item.ClickCommand.RaiseCanExecuteChanged();
                    item.IsWinningCell = gameStatusDto.WinLine.Contains(item.Number);
                }

                GameMessageTxt.Text = gameStatusDto.Winner != null
                    ? $"{gameStatusDto.Winner.Name} Win"
                    : $"Draw";

                return;
            }

            SwitchPlayer();
            PrintCurrentPlayerTurn();
            await PerformComputerActionIfNeeded();
        }

        private async Task HandleOnGameCellCliked()
        {

        }

        private GameStatusDto GetGameStatusDto()
        {
            var resDto = new GameStatusDto();

            foreach (var winLine in _winLines)
            {
                var symbols = GameCells.Where(cell => winLine.Contains(cell.Number)).Select(cell => cell.Symbol);
                if (symbols.Any(symbol => !symbol.HasValue)) continue;

                if (symbols.GroupBy(symbol => symbol).Count() == 1)
                {
                    resDto.IsGameFinished = true;
                    resDto.Winner = Players.Find(player => player.Symbol == symbols.First());
                    resDto.WinLine = winLine;
                    return resDto;
                }
            }

            if (GameCells.All(cell => cell.Symbol.HasValue))
            {
                resDto.IsGameFinished = true;
                return resDto;
            }

            return resDto;
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

        private void BackButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (NavigationService?.CanGoBack == true) NavigationService.GoBack();
        }

        private async void NewGameButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            await StartNewGame();
        }

        private async Task StartNewGame()
        {
            IsGameActive = true;
            CurrPlayerIdx = 0;

            if (!GameCells.Any())
            {
                foreach (var idx in Enumerable.Range(1, 9))
                {
                    var gameCell = new GameCell { Number = idx, Symbol = null };
                    gameCell.ClickCommand = new RelayCommand(
                        async () => await OnGameCellClicked(gameCell),
                        () => IsGameActive && !gameCell.Symbol.HasValue
                    );
                    GameCells.Add(gameCell);
                }
            }
            else
            {
                foreach (var gameCell in GameCells)
                {
                    gameCell.Symbol = null;
                    gameCell.IsWinningCell = false;
                    gameCell.ClickCommand.RaiseCanExecuteChanged();
                }
            }

            PrintCurrentPlayerTurn();
            await PerformComputerActionIfNeeded();
        }

        private async Task PerformComputerActionIfNeeded()
        {
            
            if (Players[CurrPlayerIdx].IsComputer)
            {
                await Task.Delay(700);
                var cellNum = ComputerPlayerHelper.GetMove([.. GameCells], _winLines, Players[CurrPlayerIdx].Symbol);
                if (!cellNum.HasValue) return;
                await OnGameCellClicked(GameCells.First(cell => cell.Number == cellNum.Value));
            }
        }
    }
}
