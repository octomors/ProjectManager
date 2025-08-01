using Model.Model.Entities;

namespace ProjectManager.Model.Entities
{
    public class Project : Entity
    {
        public string Name { get; set; }

        public int MethodID { get; set; }

        /// <summary>
        /// Полный путь к папке проекта
        /// </summary>
        public string Directory { get; set; }

        /// <summary>
        /// Папка (обычно \Data), внутри которой лежит Access база данных определенного проекта.
        /// </summary>
        public string DBFolder { get; set; }

        /// <summary>
        /// Имя файла базы данных проекта с расширением
        /// </summary>
        public string DBFile { get; set; }

        public string ProjectAlias { get; set; }

        /// <summary>
        /// Полный путь к базе данных проекта
        /// </summary>
        public string DBFileFullPath
        {
            get
            {
                return $"{Directory}{DBFolder}\\{DBFile}";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="methodID"></param>
        /// <param name="directory"></param>
        /// <param name="dbFolder">Папка (обычно \Data), внутри которой лежит Access база данных конкретного проекта</param>
        /// <param name="dbFileName"></param>
        /// <param name="projectAlias"></param>
        public Project(int id, string name, int methodId, string directory, string dbFolder, string dbFile, string projectAlias)
        {
            ID = id;
            Name = name;
            MethodID = methodId;
            Directory = directory;
            DBFolder = dbFolder;
            DBFile = dbFile;
            ProjectAlias = projectAlias;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="methodId"></param>
        /// <param name="directory"></param>
        /// <param name="dbFolder">Папка (обычно \Data), внутри которой лежит Access база данных конкретного проекта.</param>
        /// <param name="dbFile"></param>
        /// <param name="projectAlias"></param>
        public Project(string name, int methodId, string directory, string dbFolder, string dbFile, string projectAlias)
        {
            ID = -1;
            Name = name;
            MethodID = methodId;
            Directory = directory;
            DBFolder = dbFolder;
            DBFile = dbFile;
            ProjectAlias = projectAlias;
        }

        public override string ToString()
        {
            return $"{ID}, {Name}, {Directory}";
        }
    }
}
