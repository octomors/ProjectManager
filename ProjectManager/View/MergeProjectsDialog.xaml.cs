using ProjectManager.ViewModel;
using System.Windows;

namespace ProjectManager.View
{
    /// <summary>
    /// Логика взаимодействия для MergeProjectsDialog.xaml
    /// </summary>
    public partial class MergeProjectsDialog : Window
    {
        public MergeProjectsDialog()
        {
            InitializeComponent();
            this.DataContext = new MergeProjectWindowViewModel(this);
        }
    }
}
