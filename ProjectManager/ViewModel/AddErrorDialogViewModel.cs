using ProjectManager.View;
using System.Windows;
using System.Windows.Input;

namespace ProjectManager.ViewModel
{
    public class AddErrorDialogViewModel : BaseViewModel
    {
        public string Text { get; set; }
        public ICommand AddCommand { get; set; }

        public bool Success { get; set; }
        private AddErrorDialog window { get; set; }

        public AddErrorDialogViewModel(AddErrorDialog window)
        {
            this.window = window;
            AddCommand = new RelayCommand(Add);
        }

        private void Add()
        {
            if (string.IsNullOrEmpty(Text))
            {
                MessageBox.Show("Описание ошиби не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Success = true;
            window.Close();
        }
    }
}
