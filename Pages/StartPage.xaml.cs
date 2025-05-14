using System.Windows.Controls;
using TicTacToe.Classes;

namespace TicTacToe.Pages
{
    public partial class StartPage : Page
    {
        private readonly Frame _frame;
        private string _selectedGameOption;
        public StartPage(Frame frame)
        {
            InitializeComponent();
            _frame = frame;
            Loaded += StartPage_Loaded;
        }

        private void GameModeBox_SelectionChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            HandleSelectedGameMode();
        }

        private void StartPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            GameModeBox.SelectedIndex = 0;
            HandleSelectedGameMode();
        }

        private void HandleSelectedGameMode()
        {
            var selectedOption = (GameModeBox.SelectedItem as ComboBoxItem)?.Tag?.ToString();
            _selectedGameOption = selectedOption ?? string.Empty;
            switch (selectedOption)
            {
                case "PvP":
                    Player1NameTxt.Text = "";
                    Player2NameTxt.Text = "";
                    Player1NameTxt.IsEnabled = true;
                    Player2NameTxt.IsEnabled = true;
                    break;
                case "PvC":
                    Player1NameTxt.Text = "";
                    Player2NameTxt.Text = "Computer";
                    Player1NameTxt.IsEnabled = true;
                    Player2NameTxt.IsEnabled = false;
                    break;
                default:
                    break;
            }
        }

        private void StartGame_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Player1NameTxt.Text))
            {
                ErrorMessageTxt.Text = "Please enter the names of all player 1";
                return;
            }

            if (string.IsNullOrWhiteSpace(Player1NameTxt.Text))
            {
                ErrorMessageTxt.Text = "Please enter the names of all player 2";
                return;
            }

            ErrorMessageTxt.Text = string.Empty;

            var player1 = new Player
            {
                Name = Player1NameTxt.Text,
                Symbol = Enums.SymbolTypeEnum.X,
                IsComputer = false
            };

            var player2 = new Player
            {
                Name = Player2NameTxt.Text,
                Symbol = Enums.SymbolTypeEnum.O,
                IsComputer = _selectedGameOption == "PvC"
            };
            _frame.Navigate(new GamePage(_frame, player1, player2));
        }
    }
}
