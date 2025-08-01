using ProjectManager.ViewModel;
using System.Windows;

namespace ProjectManager.View
{
    /// <summary>
    /// Логика взаимодействия для AddProjectDialog.xaml
    /// </summary>
    public partial class AddProjectDialog : Window
    {
        public AddProjectDialog()
        {
            InitializeComponent();
            this.DataContext = new AddProjectDialogViewModel(this);
        }
    }
}
