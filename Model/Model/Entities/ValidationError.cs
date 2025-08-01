using Model.Model.Entities;
using System.Collections.Generic;

namespace ProjectManager.Model.Entities
{
    /// <summary>
    /// представляет ошибку проекта, такую как отсутствие таблицы в бд, некорректность данных и т.д.
    /// </summary>
    public class ValidationError : Entity
    {
        public int ReportID { get; set; }

        public string Message { get; set; }

        public int TypeID { get; set; }

        public string TypeTitle
        { 
            get => ErrorTypes[TypeID];
        }

        public static Dictionary<int, string> ErrorTypes { get; set; }

        public ValidationError(int id, int reportId, string message, int typeID)
        {
            ID = id;
            ReportID = reportId;
            Message = message;
            TypeID = typeID;
        }
        public ValidationError(int reportId, string message, int typeID)
        {
            ID = -1;
            ReportID = reportId;
            Message = message;
            TypeID = typeID;
        }

        public override string ToString()
        {
            return $"{TypeTitle} : {Message}";
        }
    }
}
