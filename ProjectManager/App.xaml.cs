using AccessDataBase;
using Model.Model.UseCases;
using ProjectManager.Model.UseCases;
using ProjectManager.ViewModel;
using System;
using System.Windows;
using System.Xml;

namespace ProjectManager
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //Настройка Зависимостей

            //DataAccessLayer
            string mainConnectionString = GetMainConnectionString();
            string ProjectDBConnectionStringFirstPart = null;
            string ProjectDBConnectionStringSecondPart = null;
            GetProjectConnectionString(out ProjectDBConnectionStringFirstPart, out ProjectDBConnectionStringSecondPart);

            var projectRepository = new ProjectRepository() { ConnectionString = mainConnectionString };
            var reportRepository = new ValidationReportRepository() { ConnectionString = mainConnectionString };
            var errorRepository = new ValidationErrorRepository() { ConnectionString = mainConnectionString };

            var mdRepository = new MDRepository();
            var pkRepository = new PKRepository();
            var profPKRepository = new ProfPKRepository();
            var profModRepository = new ProfModRepository();

            //Model
            var errorManagementUseCase = new ErrorManagement(errorRepository, ChangeConnectionString, mdRepository,
                pkRepository, profModRepository, profPKRepository);
            var reportManagementUseCase = new ReportManagement(reportRepository, errorManagementUseCase);
            var projectManagementUseCase = new ProjectManagement(projectRepository, ChangeConnectionString, mdRepository,
                pkRepository, profModRepository, profPKRepository);

            //ProjectManager
            var mainWindowVM = new MainWindowViewModel(projectManagementUseCase, reportManagementUseCase, errorManagementUseCase);
            var mainWindow = new MainWindow();
            mainWindow.DataContext = mainWindowVM;


            mainWindow.Show();

            //Меняет строку подключения для всех репозиториев базы данных проекта
            void ChangeConnectionString(string obj)
            {
                string currentConnectionString = $"{ProjectDBConnectionStringFirstPart}{obj}{ProjectDBConnectionStringSecondPart}";

                mdRepository.ConnectionString = currentConnectionString;
                pkRepository.ConnectionString = currentConnectionString;
                profModRepository.ConnectionString = currentConnectionString;
                profPKRepository.ConnectionString = currentConnectionString;
            }
        }

        private string GetMainConnectionString()
        {
            string appConfigPath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            XmlDocument doc = new XmlDocument();
            doc.Load(appConfigPath);

            XmlNode connectionStringsNode = doc.SelectSingleNode($"//connectionStrings/add[@name='MainDataBase']");
            if(connectionStringsNode != null)
            {
                return connectionStringsNode.Attributes["connectionString"]?.Value;
            }

            return null;
        }

        private void GetProjectConnectionString(out string FirstPart, out string SecondPart)
        {
            FirstPart = " ";
            SecondPart = " ";
            string appConfigPath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            XmlDocument doc = new XmlDocument();
            doc.Load(appConfigPath);

            XmlNode connectionStringsNode = doc.SelectSingleNode($"//connectionStrings/add[@name='ProjectDataBaseFirstPart']");
            if (connectionStringsNode != null)
            {
                FirstPart = connectionStringsNode.Attributes["connectionString"]?.Value;
            }

            XmlNode connectionStringsNode2 = doc.SelectSingleNode($"//connectionStrings/add[@name='ProjectDataBaseSecondPart']");
            if (connectionStringsNode != null)
            {
                FirstPart = connectionStringsNode.Attributes["connectionString"]?.Value;
            }

        }
    }
}
