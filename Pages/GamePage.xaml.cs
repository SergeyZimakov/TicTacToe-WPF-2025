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
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public Player CurrentPlayer { get; set; }
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

            Player1 = player1;
            Player2 = player2;
            CurrentPlayer = Player1;
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
            gameCell.Symbol = CurrentPlayer.Symbol;

            var gameStatusDto = GetGameStatusDto();
            if (gameStatusDto.IsGameFinished)
            {
                IsGameActive = false;
                foreach (var item in GameCells)
                {
                    item.ClickCommand.RaiseCanExecuteChanged();
                    item.IsWinningCell = gameStatusDto.WinLine.Contains(item.Number);
                }

                GameMessageTxt.Text = gameStatusDto.IsDraw
                    ? $"Draw"
                    : $"{CurrentPlayer.Name} Win";

                return;
            }

            SwitchPlayer();
            PrintCurrentPlayerTurn();
            await PerformComputerActionIfNeeded();
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
                    resDto.WinLine = winLine;
                    return resDto;
                }
            }

            if (GameCells.All(cell => cell.Symbol.HasValue))
            {
                resDto.IsGameFinished = true;
                resDto.IsDraw = true;
                return resDto;
            }

            return resDto;
        }

        private void PrintCurrentPlayerTurn() => GameMessageTxt.Text = $"{CurrentPlayer.Name} turn({CurrentPlayer.Symbol.GetAsString()})";
        

        private void SwitchPlayer() => CurrentPlayer = CurrentPlayer != Player1 ? Player1 : Player2;
        

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
            CurrentPlayer = Player1;

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
            
            if (CurrentPlayer.IsComputer)
            {
                await Task.Delay(700);
                var cellNum = ComputerPlayerHelper.GetMove([.. GameCells], _winLines, CurrentPlayer.Symbol);
                if (!cellNum.HasValue) return;
                await OnGameCellClicked(GameCells.First(cell => cell.Number == cellNum.Value));
            }
        }
    }
}
