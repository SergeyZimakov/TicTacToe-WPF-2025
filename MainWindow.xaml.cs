using System.Windows;
using TicTacToe.Pages;

namespace TicTacToe;
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainFrame.Navigate(new GamePage(MainFrame));
    }
}