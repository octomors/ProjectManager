using Model.Model.Entities;
using Model.Model.Interfaces;
using ProjectManager.Model.Entities;
using ProjectManager.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace Model.Model.UseCases
{
    public class ReportManagement : IReportManagement
    {
        private IValidationReportRepository ReportRepository { get; set; }
        private IErrorManagement ErrorManager { get; set; }
        private string EmployeeName = File.ReadAllText("EMPLOYEENAME.txt");


        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportRepository"></param>
        /// <param name="changeTargetDataBase">Меняет connectionString у репозиториев проекта</param>
        public ReportManagement(IValidationReportRepository reportRepository, IErrorManagement errorManager)
        {
            ReportRepository = reportRepository;
            ErrorManager = errorManager;
        }

        public void Generate(Project targetProject)
        {
            var report = new ValidationReport(targetProject.ID, DateTime.Now, "", EmployeeName);
            ReportRepository.Create(report);
            ErrorManager.FindErrors(targetProject, report);
        }

        public void Delete(ValidationReport report)
        {
            ReportRepository.Delete(report.ID);
        }

        public IEnumerable<ValidationReport> GetAllByProjectID(int id)
        {
            return ReportRepository.ReadAllByProjectID(id);
        }
    }
}
