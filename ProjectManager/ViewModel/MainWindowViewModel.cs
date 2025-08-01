using Microsoft.Win32;
using Model.Model.UseCases;
using ProjectManager.Model.Entities;
using ProjectManager.Model.UseCases;
using ProjectManager.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace ProjectManager.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        private ValidationError selectedError;
        private ValidationReport selectedReport;
        private Project selectedProject;
        private ObservableCollection<ValidationReport> reports;
        private ObservableCollection<Project> projects;
        private ObservableCollection<ValidationError> errors;

        #region Bindings
        public ObservableCollection<Project> Projects
        {
            get => projects;
            set
            {
                projects = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ValidationReport> Reports
        {
            get => reports;
            set
            {
                reports = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ValidationError> Errors
        {
            get => errors;
            set
            {
                errors = value;
                OnPropertyChanged();
            }
        }

        public Project SelectedProject
        {
            get => selectedProject;
            set
            {
                selectedProject = value;
                if (value != null)
                    ReloadReports(value.ID);
                OnPropertyChanged();
            }
        }
        public ValidationReport SelectedReport
        {
            get => selectedReport;
            set
            {
                selectedReport = value;
                if (value != null)
                    ReloadErrors(value.ID);
                OnPropertyChanged();
            }
        }
        public ValidationError SelectedError
        {
            get => selectedError;
            set
            {
                selectedError = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddProjectCommand { get; set; }
        public ICommand CreateMergedProjectCommand { get; set; }
        public ICommand GenerateReportCommand { get; set; }
        public ICommand DeleteReportCommand { get; set; }
        public ICommand FixProjectCommand { get; set; }
        public ICommand AddErrorCommand { get; set; }
        public ICommand DeleteErrorCommand { get; set; }

        #endregion

        private IProjectManagement ProjectManager { get; set; }
        private IReportManagement ReportManager { get; set; }
        private IErrorManagement ErrorManager { get; set; }

        public MainWindowViewModel(IProjectManagement projectManager, IReportManagement reportManager, IErrorManagement errorManager)
        {
            AddProjectCommand = new RelayCommand(AddProject);
            CreateMergedProjectCommand = new RelayCommand(CreateMergedProject);
            GenerateReportCommand = new RelayCommand(GenerateReport, () => SelectedProject != null);
            FixProjectCommand = new RelayCommand(FixErrors, () => SelectedProject != null);
            DeleteReportCommand = new RelayCommand(DeleteReport, () => SelectedReport != null);
            AddErrorCommand = new RelayCommand(AddError, () => SelectedReport != null);
            DeleteErrorCommand = new RelayCommand(DeleteError, () => SelectedError != null);

            ProjectManager = projectManager;
            ReportManager = reportManager;
            ErrorManager = errorManager;

            ReloadProjects();
        }

        private void DeleteError()
        {
            ErrorManager.Delete(SelectedError);

            ReloadErrors(SelectedReport.ID);
        }

        private void AddError()
        {
            var addErrorWindow = new AddErrorDialog();

            addErrorWindow.ShowDialog();
            var vm = (AddErrorDialogViewModel)addErrorWindow.DataContext;

            if (vm.Success != true)
                return;

            ErrorManager.Add(new ValidationError(SelectedReport.ID, vm.Text, 7));

            ReloadErrors(SelectedReport.ID);
        }

        private void FixErrors()
        {
            string CopyProjectName = $"{SelectedProject.Name}_Испр";
            string CopyProjectDirectory = ProjectManager.MakeCopy(selectedProject.Directory, CopyProjectName);

            Project CopyProject = new Project(
                CopyProjectName,
                SelectedProject.MethodID,
                CopyProjectDirectory,
                SelectedProject.DBFolder,
                SelectedProject.DBFile,
                SelectedProject.ProjectAlias);

            ProjectManager.Add(CopyProject);
            try
            {
                ErrorManager.FixErrors(CopyProject);
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop);
            }

            ReloadProjects();
        }

        private void DeleteReport()
        {
            ReportManager.Delete(SelectedReport);

            ReloadReports(SelectedProject.ID);
        }

        private void GenerateReport()
        { 
            try
            {
                ReportManager.Generate(SelectedProject);
            }
            catch (System.Data.OleDb.OleDbException e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            ReloadReports(SelectedProject.ID);
        }

        private void AddProject()
        {
            var addProjectWindow = new AddProjectDialog();
            var vm = (AddProjectDialogViewModel)addProjectWindow.DataContext;

            ShowInputProjectFileDialog(out string name, out string directory,
                out string DBFolder, out string DBFile);

            vm.Name = name;
            vm.Directory = directory;
            vm.DBFolder = DBFolder;
            vm.DBFile = DBFile;

            addProjectWindow.ShowDialog();


            if (vm.Success != true)
            {
                return;
            }

            Project project = new Project(
                vm.Name,
                vm.MethodID,
                vm.Directory,
                vm.DBFolder,
                vm.DBFile,
                vm.Alias);

            ProjectManager.Add(project);

            MessageBox.Show("Запись о проекте добавлена в базу данных", "", MessageBoxButton.OK, MessageBoxImage.Information);
            Projects.Add(project);
            SelectedProject = project;
        }

        private void CreateMergedProject()
        {
            if (!ShowInputRequiredPKFileDialog(out string filePath))
            {
                return;
            }

            Dictionary<int, List<int>> requiredPKs = new Dictionary<int, List<int>>();
            if(!ParseRequiredPKFile(filePath, out requiredPKs))
            {
                MessageBox.Show("Не удалось извлечь данные из файла. Необходимый формат: " +
                    "айди проекта, пробел, айди пикета. Пример 1 строки:12 256", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var mergeProjectsWindow = new MergeProjectsDialog();
            var vm = (MergeProjectWindowViewModel)mergeProjectsWindow.DataContext;


            mergeProjectsWindow.ShowDialog();


            if (vm.Success != true)
            {
                return;
            }

            ProjectManager.CreateMergedProject(vm.Directory, requiredPKs, vm.Name, vm.MethodID, vm.Alias);

            ReloadProjects();
        }

        private bool ParseRequiredPKFile(string filePath, out Dictionary<int, List<int>> requiredPKs)
        {
            requiredPKs = new Dictionary<int, List<int>>();
            
            foreach(var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if(parts.Length != 2)
                    return false;
                if (int.TryParse(parts[0], out int projectID) &&  int.TryParse(parts[1], out int PKID))
                {
                    if(!requiredPKs.ContainsKey(projectID))
                    {
                        requiredPKs[projectID] = new List<int>();
                    }
                    requiredPKs[projectID].Add(PKID);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath">путь к файлу с перечислением пикетов</param>
        /// <returns>успех</returns>
        private bool ShowInputRequiredPKFileDialog(out string filePath)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Выберите .txt файл с перечислением пикетов для сборного проекта";
            openFileDialog.InitialDirectory = @"C:\";
            openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt";
            bool? success = openFileDialog.ShowDialog();
            filePath = openFileDialog.FileName;

            return success == true;
        }

        /// <summary>
        /// Просит пользователя выбрать Access бд файл проекта, парсит из пути атрибуты проекта
        /// </summary>
        /// <param name="name"></param>
        /// <param name="directory"></param>
        /// <param name="DBFolder"></param>
        /// <param name="DBFile"></param>
        private void ShowInputProjectFileDialog(out string name, out string directory,
            out string DBFolder, out string DBFile)
        {
            name = string.Empty;
            directory = string.Empty;
            DBFolder = string.Empty;
            DBFile = string.Empty;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Выберите .mdb файл проекта, которого желаете добавить в базу данных...";
            openFileDialog.InitialDirectory = @"S:\FDB";
            bool? success = openFileDialog.ShowDialog();
            if (success == true)
            {
                FileInfo fileInfo = new FileInfo(openFileDialog.FileName);

                name = fileInfo.Directory.Parent?.Name;
                directory = fileInfo.Directory.Parent?.FullName;
                DBFolder = $"\\{fileInfo.Directory?.Name}";
                DBFile = openFileDialog.SafeFileName;
            }
        }

        private void ReloadProjects()
        {
            Projects = new ObservableCollection<Project>(ProjectManager.GetAll());
        }

        private void ReloadReports(int projectId)
        {
            Reports = new ObservableCollection<ValidationReport>(ReportManager.GetAllByProjectID(projectId));
            if (Reports.Count == 1)
            {
                SelectedReport = Reports[0];
                ReloadErrors(Reports[0].ID);
            }
            else
            {
                Errors = null;
            }
        }

        private void ReloadErrors(int reportID)
        {
            Errors = new ObservableCollection<ValidationError>(ErrorManager.GetAllByReportID(reportID));
        }
    }
}
