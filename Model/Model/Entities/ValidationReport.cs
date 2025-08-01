using Model.Model.Entities;
using System;

namespace ProjectManager.Model.Entities
{
    /// <summary>
    /// Отчет по имеющимся ошибкам в проекте, такие как отсутствие таблиц в бд, некорректность данных и т.д.
    /// </summary>
    public class ValidationReport : Entity
    {
        public int ProjectID { get; set; }

        /// <summary>
        /// Дата и время проведенной проверки
        /// </summary>
        public DateTime CheckDateTime { get; set; }

        public string Comments { get; set; }

        /// <summary>
        /// ФИО работника, проверявшего проект.
        /// </summary>
        public string EmployeeName { get; set; }

        public ValidationReport(int id, int projectId, DateTime checkDateTime, string comments, string employeeName)
        {
            ID = id;
            ProjectID = projectId;
            CheckDateTime = checkDateTime;
            Comments = comments;
            EmployeeName = employeeName;
        }

        public ValidationReport(int projectId, DateTime checkDateTime, string comments, string employeeName)
        {
            ID = -1;
            ProjectID = projectId;
            CheckDateTime = checkDateTime;
            Comments = comments;
            EmployeeName = employeeName;
        }

        public override string ToString()
        {
            return $"{EmployeeName}, {CheckDateTime} : {Comments}";
        }
    }
}
