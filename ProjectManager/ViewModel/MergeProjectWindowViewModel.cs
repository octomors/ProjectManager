using Microsoft.Win32;
using ProjectManager.View;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ProjectManager.ViewModel
{
    public class MergeProjectWindowViewModel : BaseViewModel
    {
        public string Name { get => name; set { name = value; OnPropertyChanged(); } }
        public int MethodID { get => methodID; set { methodID = value; OnPropertyChanged(); } }
        public string Directory { get => directory; set { directory = value; OnPropertyChanged(); } }
        public string Alias { get => alias; set { alias = value; OnPropertyChanged(); } }
        public bool Success { get; set; }

        public ICommand SelectProjectFolderCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        private MergeProjectsDialog window;
        private string name;
        private int methodID = 1;
        private string directory;
        private string alias = "ByPM";

        public MergeProjectWindowViewModel(MergeProjectsDialog window)
        {
            this.window = window;
            SelectProjectFolderCommand = new RelayCommand(SelectProjectFolderDialog);
            AddCommand = new RelayCommand(Add);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void SelectProjectFolderDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ValidateNames = false;
            openFileDialog.CheckFileExists = false;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Title = "Выберите папку, в которую будет добавлен сборный проект";
            openFileDialog.FileName = "Выберите папку";
            openFileDialog.ShowDialog();

            Directory = openFileDialog.FileName.Replace("\\Выберите папку", "");
        }

        private void Add()
        {
            if (ValidateData())
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
            if (string.IsNullOrWhiteSpace(Name))
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
            int[] methodIDs = { 1, 2, 3, 4 };
            if (!methodIDs.Contains(MethodID))
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
