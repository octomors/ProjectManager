using ProjectManager.View;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ProjectManager.ViewModel
{
    class AddProjectDialogViewModel : BaseViewModel
    {
        public string Name { get => name; set { name = value; OnPropertyChanged(); } }
        public int MethodID { get => methodID; set { methodID = value; OnPropertyChanged(); } }
        public string Directory { get => directory; set { directory = value; OnPropertyChanged(); } }
        public string DBFolder { get => dBFolder; set { dBFolder = value; OnPropertyChanged(); } }
        public string DBFile { get => dBFile; set { dBFile = value; OnPropertyChanged(); } }
        public string Alias { get => alias; set { alias = value; OnPropertyChanged(); } }
        public bool Success { get; set; }

        public ICommand AddCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        private AddProjectDialog window;
        private string name;
        private int methodID = 1;
        private string directory;
        private string dBFolder;
        private string dBFile;
        private string alias = "ByPM";

        public AddProjectDialogViewModel(AddProjectDialog window)
        {
            this.window = window;
            AddCommand = new RelayCommand(Add);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Add()
        {
            if(ValidateData())
            {
                Success = true;
                window.Close();
            }
        }
        private void Cancel()
        {
            Success = false;
            window.Close();
        }

        private bool ValidateData()
        {
            //Name
            if(string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show(
                    "Название проекта не может быть пустым",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                    );
                return false;
            }

            //MethodID
            int[] methodIDs = {1, 2, 3, 4 };
            if(!methodIDs.Contains(MethodID))
            {
                MessageBox.Show(
                    "Доступные варианты: 1-TEM, 2-MTS, 3-VES, 4-DES",
                    "Ошибка: Указан неверный ID метода",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                    );
                return false;
            }

            //Directory
            if (string.IsNullOrWhiteSpace(Directory))
            {
                MessageBox.Show(
                    "Директория проекта не может быть пустой строкой",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                    );
                return false;
            }

            //DBFolder
            if (string.IsNullOrWhiteSpace(DBFolder))
            {
                MessageBox.Show(
                    "Название папки проекта не может быть пустой строкой",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                    );
                return false;
            }
            if(!DBFolder.StartsWith("\\"))
            {
                MessageBox.Show(
                    @"Папка проекта должна начинаться с \ . Пример: \Data",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                    );
                return false;
            }

            //DBFile
            if (string.IsNullOrWhiteSpace(DBFile))
            {
                MessageBox.Show(
                    "Файл проекта не может быть пустой строкой",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                    );
                return false;
            }

            //Alias
            if (string.IsNullOrWhiteSpace(Alias))
            {
                MessageBox.Show(
                    "Алиас не может быть пустой строкой",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                    );
                return false;
            }

            return true;
        }
    }
}
