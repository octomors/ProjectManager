using ProjectManager.ViewModel;
using System.Windows;

namespace ProjectManager.View
{
    /// <summary>
    /// Логика взаимодействия для AddErrorDialog.xaml
    /// </summary>
    public partial class AddErrorDialog : Window
    {
        public AddErrorDialog()
        {
            InitializeComponent();
            this.DataContext = new AddErrorDialogViewModel(this);
        }
    }
}
