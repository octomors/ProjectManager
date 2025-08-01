using ProjectManager.Model.Entities;
using ProjectManager.Model.Interfaces;
using System.Collections.Generic;

namespace Model.Model.Interfaces
{
    public interface IValidationReportRepository : IRepository<ValidationReport>
    {
        /// <summary>
        /// Возвращает все отчеты, относящиеся к определенному проекту
        /// </summary>
        /// <param name="id">Айди проекта</param>
        /// <returns></returns>
        IEnumerable<ValidationReport> ReadAllByProjectID(int id);
    }
}
