using System.Windows.Controls;

namespace TicTacToe.Pages
{
    public partial class StartPage : Page
    {
        private readonly Frame _frame;
        public StartPage(Frame frame)
        {
            InitializeComponent();
            _frame = frame;
        }
    }
}
