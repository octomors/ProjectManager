using Model.Model.Entities;
using Model.Model.UseCases;
using ProjectManager.Model.Entities;
using ProjectManager.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace ProjectManager.Model.UseCases
{
    public class ProjectManagement : BaseProjectManagement, IProjectManagement
    {
        private IRepository<Project> ProjectRepository { get; set; }

        public ProjectManagement(IRepository<Project> projectRepository, Action<string> changeTargetDataBase,
            IRepository<MD> MDRepository, IRepository<PK> PKRepository, IRepository<ProfMod> profModRepository,
            IRepository<ProfPK> profPKRepository)
            : base (changeTargetDataBase, MDRepository, PKRepository, profModRepository, profPKRepository)
        {
            ProjectRepository = projectRepository;
        }

        public void Add(Project newProject)
        {
            ProjectRepository.Create(newProject);
        }

        public IEnumerable<Project> GetAll()
        {
            return ProjectRepository.ReadAll();
        }
        
        public string MakeCopy(string projectDirectory, string newName)
        {
            var sourceDir = new DirectoryInfo(projectDirectory);
            string destinationDir = Path.Combine(sourceDir.Parent.FullName, newName);
            CopyDirectory(projectDirectory, destinationDir, true);

            return destinationDir;
        }

        public void CreateMergedProject(string destinationDirectory, Dictionary<int, List<int>> Pks, string name,
            int methodID = 1, string alias = "ByPM")
        {
            var mergedProject = new Project(name, methodID, $"{destinationDirectory}\\{name}", "\\Data", $"{name}.mdb", alias);
            //Инициализаия данных обьединенного проекта
            List<PK> mergedPKs = new List<PK>();
            List<MD> mergedMDs = new List<MD>();
            List<ProfMod> mergedProfMods = new List<ProfMod>();
            List<ProfPK> mergedProfPKs = new List<ProfPK>();

            CreateEmptyProject(mergedProject);
            Add(mergedProject);

            //Перебор необходимых проектов
            foreach (var pks in Pks)
            {
                //Соединение всех репозиториев с бд нужного проекта
                Project targetProject = ProjectRepository.Read(pks.Key);
                ChangeTargetDataBase(targetProject.DBFileFullPath);

                //Пикеты с нужным айди из текущего проекта
                //var requiredPKs = PKRepository.ReadAll().Where(pk => pks.Value.Contains(pk.ID)).ToList();
                var requiredPKs = new List<PK>(pks.Value.Count);
                foreach(var PKID in pks.Value)
                {
                    requiredPKs.Add(PKRepository.Read(PKID));
                }

                //Айди генераторных петель, на которые ссылаются выбранные пикеты
                var requiredMDIDs = requiredPKs.Select(pk => pk.MDID).Distinct();
                //Генераторные петли, на которые ссылаются выбранные пикеты
                var requiredMDs = MDRepository.ReadAll().Where(md => requiredMDIDs.Contains(md.ID));

                //Все записи таблицы ProfMod, которые ссылаются на выбранные ген. петли и пикеты
                var requiredProfMods = ProfModRepository.ReadAll()
                    .Where(profMod => pks.Value.Contains(profMod.PKID) && requiredMDIDs.Contains(profMod.MDID));

                //Все записи таблицы ProfPK, которые ссылаются на выбранные ген. петли и пикеты
                var requiredProfPKs = ProfPKRepository.ReadAll()
                    .Where(profPK => pks.Value.Contains(profPK.PKID) && requiredMDIDs.Contains(profPK.MDID)).ToList();

                //Исправление дубликатов текущего проекта
                //[коллекция [списков]] дубликатов с одинаковым айди (во вложенном списке все сущности имеют одинаковый айди)
                var duplicatePKs = requiredPKs.GroupBy(entity => entity.ID).Where(g => g.Count() > 1).Select(g => g.ToList());
                var duplicateMDs = requiredMDs.GroupBy(entity => entity.ID).Where(g => g.Count() > 1).Select(g => g.ToList());
                var duplicateProfMods = requiredProfMods.GroupBy(entity => entity.ID).Where(g => g.Count() > 1).Select(g => g.ToList());
                var duplicateProfPKs = requiredProfPKs.GroupBy(entity => entity.ID).Where(g => g.Count() > 1).Select(g => g.ToList());

                foreach(var list in duplicatePKs)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].ID = list[i].GenerateNewID(i);
                    }
                }
                foreach (var list in duplicateMDs)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].ID = list[i].GenerateNewID(i);
                    }
                }
                foreach (var list in duplicateProfMods)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].ID = list[i].GenerateNewID(i);
                    }
                }
                foreach (var list in duplicateProfPKs)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].ID = list[i].GenerateNewID(i);
                    }
                }

                //Исправление дубликатов сборного и текущего проекта
                foreach(var pk in requiredPKs)
                {
                    for(int i = 0; i < mergedPKs.Count; i++)
                    {
                        if(mergedPKs[i].ID == pk.ID)
                        {
                            RegeneratePKID(pk, requiredPKs, requiredProfMods, requiredProfPKs);
                            break;
                        }
                    }
                }

                foreach (var md in requiredMDs)
                {
                    for (int i = 0; i < mergedMDs.Count; i++)
                    {
                        if (mergedMDs[i].ID == md.ID)
                        {
                            RegenerateMDID(md, requiredPKs, requiredProfMods, requiredProfPKs);
                            break;
                        }
                    }
                }

                foreach (var profMod in requiredProfMods)
                {
                    for (int i = 0; i < mergedProfMods.Count; i++)
                    {
                        if (mergedProfMods[i].ID == profMod.ID)
                        {
                            profMod.ID = profMod.GenerateNewID(1);
                            break;
                        }
                    }
                }

                foreach (var profPK in requiredProfPKs)
                {
                    for (int i = 0; i < mergedProfPKs.Count; i++)
                    {
                        if (mergedProfPKs[i].ID == profPK.ID)
                        {
                            profPK.ID = profPK.GenerateNewID(1);
                            break;
                        }
                    }
                }
                //Добавление данных текущего проекта к данным сборного проекта
                mergedPKs.AddRange(requiredPKs);
                mergedMDs.AddRange(requiredMDs);
                mergedProfMods.AddRange(requiredProfMods);
                mergedProfPKs.AddRange(requiredProfPKs);

                //Копирование кривых текущего проекта в сборный проект
                foreach(var profMod in requiredProfMods)
                {
                    //Добавляет расширение файла, если оно отсутствует
                    string tqqFileName = profMod.TqqFileName.EndsWith(".tqq") ?
                        profMod.TqqFileName : $"{profMod.TqqFileName}.tqq";
                    string txtFileName = profMod.TxtFileName.EndsWith(".txt") ?
                        profMod.TxtFileName : $"{profMod.TxtFileName}.txt";

                    //Куда копировать
                    string destTqqFilePath = $"{mergedProject.Directory}\\Curves\\{tqqFileName}";
                    string destTxtFilePath = $"{mergedProject.Directory}\\TXT\\{txtFileName}";

                    string sourceTqqFilePath = $"{targetProject.Directory}\\Curves\\{tqqFileName}";
                    if (!File.Exists(sourceTqqFilePath))
                    {
                        sourceTqqFilePath = $"{targetProject.Directory}\\curves\\{tqqFileName}";
                    }
                    if(File.Exists(sourceTqqFilePath))
                    {
                        File.Copy(sourceTqqFilePath, destTqqFilePath);
                    }

                    string sourceTxtFilePath = $"{targetProject.Directory}\\TXT\\{txtFileName}";
                    if (!File.Exists(sourceTxtFilePath))
                    {
                        sourceTxtFilePath = $"{targetProject.Directory}\\txt\\{txtFileName}";
                    }
                    if (File.Exists(sourceTxtFilePath))
                    {
                        File.Copy(sourceTxtFilePath, destTxtFilePath);
                    }
                }
            }
;
            //Сохранение сборного проекта
            ChangeTargetDataBase(mergedProject.DBFileFullPath);

            foreach (var md in mergedMDs)
            {
                MDRepository.CreateKeepID(md);
            }
            foreach (var pk in mergedPKs)
            {
                PKRepository.CreateKeepID(pk);
            }
            foreach (var profMod in mergedProfMods)
            {
                ProfModRepository.CreateKeepID(profMod);
            }
            foreach (var profPK in mergedProfPKs)
            {
                ProfPKRepository.CreateKeepID(profPK);
            }

            ChangeTargetDataBase("");
        }

        /// <summary>
        /// Создает пустой проект в указанной директории
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public void CreateEmptyProject(Project project)
        {
            CopyDirectory(".\\ProjectTemplate", project.Directory, true);
            File.Move($"{project.Directory}\\Data\\Template.mdb", project.DBFileFullPath);
        }

        /// <summary>
        /// Перегенерирует айди ГП и обновляет все ссылающиеся на нее сущности
        /// </summary>
        /// <param name="md"></param>
        /// <param name="PKs"></param>
        /// <param name="ProfMods"></param>
        /// <param name="ProfPKs"></param>
        private void RegenerateMDID(MD md, IEnumerable<PK> PKs,
            IEnumerable<ProfMod> ProfMods, IEnumerable<ProfPK> ProfPKs)
        {
            //Все пикеты, Prof_Pk, Prof_Mod, ссылающиеся на текущую ГП
            var relatedPKs = PKs.Where(pk => pk.MDID == md.ID).ToList();
            var relatedProfMods = ProfMods.Where(profMod => profMod.MDID == md.ID).ToList();
            var relatedProfPks = ProfPKs.Where(profPk => profPk.MDID == md.ID).ToList();

            md.ID = md.GenerateNewID(1);

            //Исправление внешних ключей
            foreach (var entity in relatedPKs)
            {
                entity.MDID = md.ID;
            }
            foreach (var entity in relatedProfMods)
            {
                entity.MDID = md.ID;
            }
            foreach (var entity in relatedProfPks)
            {
                entity.MDID = md.ID;
            }
        }

        /// <summary>
        /// Перегенерирует айди пикета и обновляет все ссылающиеся на него сущности
        /// </summary>
        /// <param name="md"></param>
        /// <param name="PKs"></param>
        /// <param name="ProfMods"></param>
        /// <param name="ProfPKs"></param>
        private void RegeneratePKID(PK pk, IEnumerable<PK> PKs,
            IEnumerable<ProfMod> ProfMods, IEnumerable<ProfPK> ProfPKs)
        {
            //Все Prof_Pk, Prof_Mod, ссылающиеся на текущий пикет
            var relatedProfMods = ProfMods.Where(profMod => profMod.PKID == pk.ID);
            var relatedProfPks = ProfPKs.Where(profPk => profPk.PKID == pk.ID);

            pk.ID = pk.GenerateNewID(1);

            //Исправление внешних ключей
            foreach (var entity in relatedProfMods)
            {
                entity.PKID = pk.ID;
            }
            foreach (var entity in relatedProfPks)
            {
                entity.PKID = pk.ID;
            }
        }

        private void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            var dir = new DirectoryInfo(sourceDir);

            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            DirectoryInfo[] dirs = dir.GetDirectories();

            Directory.CreateDirectory(destinationDir);

            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }
    }
}
