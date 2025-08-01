using ProjectManager.Model.Entities;
using System.Collections.Generic;

namespace Model.Model.UseCases
{
    public interface IReportManagement
    {
        /// <summary>
        /// Находит всевозможные ошибки в проекте, обьединяет их в отчет и добавляет отчет к проекту.  
        /// </summary>
        /// <param name="targetProject">проект для которого сгенерируется отчет</param>
        void Generate(Project targetProject);

        void Delete(ValidationReport report);

        IEnumerable<ValidationReport> GetAllByProjectID(int id);
    }
}
