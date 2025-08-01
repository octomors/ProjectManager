using ProjectManager.Model.Entities;
using System.Collections.Generic;

namespace Model.Model.UseCases
{
    public interface IErrorManagement
    {
        void Add(ValidationError error);

        void Delete(ValidationError error);

        IEnumerable<ValidationError> GetAllByReportID(int id);

        /// <summary>
        /// Находит ошибки в проекте и сохраняет в бд
        /// </summary>
        /// <param name="project"></param>
        /// <param name="report"></param>
        void FindErrors(Project project, ValidationReport report);

        /// <summary>
        /// Находит ошибки в проекте и исправляет
        /// </summary>
        /// <param name="project"></param>
        void FixErrors(Project project);
    }
}
