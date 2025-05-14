using System.Windows.Controls;
using TicTacToe.Classes;

namespace TicTacToe.Pages
{
    public partial class StartPage : Page
    {
        private readonly Frame _frame;
        private string _selectedGameModeOption;
        private string _selectedPlayAsOption;
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
            _selectedGameModeOption = selectedOption ?? string.Empty;
            switch (_selectedGameModeOption)
            {
                case "PvP":
                    Player1NameTxt.Text = "";
                    Player2NameTxt.Text = "";
                    Player1NameTxt.IsEnabled = true;
                    Player2NameTxt.IsEnabled = true;
                    PlayAsTxt.Visibility = System.Windows.Visibility.Hidden;
                    PlayAsBox.Visibility = System.Windows.Visibility.Hidden;
                    break;
                case "PvC":
                    PlayAsTxt.Visibility = System.Windows.Visibility.Visible;
                    PlayAsBox.Visibility = System.Windows.Visibility.Visible;
                    PlayAsBox.SelectedIndex = 0;
                    HandlePlayAsChanged();
                    break;
                default:
                    break;
            }
        }

        private void HandlePlayAsChanged()
        {
            var selectedOption = (PlayAsBox.SelectedItem as ComboBoxItem)?.Tag?.ToString();
            _selectedPlayAsOption = selectedOption ?? string.Empty;
            switch (_selectedPlayAsOption)
            {
                case "1":
                    Player1NameTxt.Text = "";
                    Player2NameTxt.Text = "Computer";
                    Player1NameTxt.IsEnabled = true;
                    Player2NameTxt.IsEnabled = false;
                    break;
                case "2":
                    Player1NameTxt.Text = "Computer";
                    Player2NameTxt.Text = "";
                    Player1NameTxt.IsEnabled = false;
                    Player2NameTxt.IsEnabled = true;
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
                IsComputer = _selectedGameModeOption == "PvC" && _selectedPlayAsOption == "1"
            };

            var player2 = new Player
            {
                Name = Player2NameTxt.Text,
                Symbol = Enums.SymbolTypeEnum.O,
                IsComputer = _selectedGameModeOption == "PvC" && _selectedPlayAsOption == "2"
            };
            _frame.Navigate(new GamePage(_frame, player1, player2));
        }

        private void PlayAsBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HandlePlayAsChanged();
        }
    }
}
