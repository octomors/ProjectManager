using System.Collections.Generic;

namespace ProjectManager.Model.Interfaces
{
    public interface IRepository<TEntity>
    {
        /// <summary>
        /// Создает новую запись в бд, присваивает сущности вычисленный ID
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="keepID">false - сгенерировать айди в бд, true - записать в бд текущий айди</param>
        void Create(TEntity entity);

        void CreateKeepID(TEntity entity);

        void Delete(int ID);

        void Update(TEntity entity);

        TEntity Read(int ID);

        IEnumerable<TEntity> ReadAll();
    }
}
