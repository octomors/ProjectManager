using Model.Model.Entities;
using ProjectManager.Model.Entities;
using System.Collections.Generic;

namespace ProjectManager.Model.UseCases
{
    public interface IProjectManagement
    {
        /// <summary>
        /// Добавляет запись о существующем проекте в базу данных
        /// </summary>
        /// <param name="newProject"></param>
        void Add(Project newProject);

        IEnumerable<Project> GetAll();

        /// <summary>
        /// Создает копию проекта в той же директории с новым именем
        /// </summary>
        /// <param name="project"></param>
        /// <param name="newName"></param>
        /// <returns>Директория копии проекта</returns>
        string MakeCopy(string projectDirectory, string newName);

        /// <summary>
        /// Создает новый проект и добавляет запись о нем в БД
        /// </summary>
        /// <param name="projectDirectory"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        void CreateEmptyProject(Project project);

        /// <summary>
        /// Создает новый проект в указанной директории, заполняет его данными по указанным пикетам из указанных проектов
        /// </summary>
        /// <param name="destinationDirectory">Директория, в которой будет создан проект</param>
        /// <param name="PKs">(айди проекта в общей БД, айди пикетов в БД этого проекта)</param>
        void CreateMergedProject(string destinationDirectory, Dictionary<int, List<int>> Pks, string name,
            int methodID = 1, string alias = "ByPM");
    }
}
