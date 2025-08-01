using ProjectManager.Model.Entities;
using ProjectManager.Model.Interfaces;
using System.Collections.Generic;

namespace Model.Model.Interfaces
{
    public interface IValidationErrorRepository : IRepository<ValidationError>
    {
        /// <summary>
        /// Возвращает все ошибки относящиеся к определенному отчету
        /// </summary>
        /// <param name="id">Отчет</param>
        /// <returns></returns>
        IEnumerable<ValidationError> ReadAllByReportID(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Dictionary - ID типа, Название типа</ID></returns>
        Dictionary<int, string> ReadAllErrorTypes();
    }
}
