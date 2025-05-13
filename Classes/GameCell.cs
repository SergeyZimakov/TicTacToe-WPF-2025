using System.ComponentModel;
using System.Windows.Input;
using TicTacToe.Enums;
using TicTacToe.Helpers;

namespace TicTacToe.Classes
{
    public class GameCell : INotifyPropertyChanged
    {
        
        public int Number { get; set; }
        public SymbolTypeEnum? Symbol
        {
            get => _symbol;
            set
            {
                if (_symbol == value) return;
                _symbol = value;
                OnPropertyChanged(nameof(Symbol));
            }
        }
        
        private SymbolTypeEnum? _symbol;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public RelayCommand ClickCommand { get; set; } = null!; 
    }
}
