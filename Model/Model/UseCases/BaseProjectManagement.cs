using Model.Model.Entities;
using ProjectManager.Model.Interfaces;
using System;

namespace Model.Model.UseCases
{
    /// <summary>
    /// Предоставляет доступ ко всем репозиториям базы данных определенного проекта,
    /// а также возможность поменять целевой проект для внесения изменений
    /// </summary>
    public abstract class BaseProjectManagement
    {
        protected IRepository<MD> MDRepository { get; set; }
        protected IRepository<PK> PKRepository { get; set; }
        protected IRepository<ProfMod> ProfModRepository { get; set; }
        protected IRepository<ProfPK> ProfPKRepository { get; set; }

        protected Action<string> ChangeTargetDataBase;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="changeTargetDataBase">Настраивает репозитории для работы с другим проектом</param>
        /// <param name="MDRepository"></param>
        /// <param name="PKRepository"></param>
        /// <param name="profModRepository"></param>
        /// <param name="profPKRepository"></param>
        protected BaseProjectManagement(Action<string> changeTargetDataBase, IRepository<MD> MDRepository,
            IRepository<PK> PKRepository, IRepository<ProfMod> profModRepository, 
            IRepository<ProfPK> profPKRepository)
        {
            this.MDRepository = MDRepository;
            this.PKRepository = PKRepository;
            this.ProfModRepository = profModRepository;
            this.ProfPKRepository = profPKRepository;
            ChangeTargetDataBase = changeTargetDataBase;
        }
    }
}
