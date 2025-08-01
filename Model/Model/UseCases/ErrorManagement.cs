using Model.Model.Entities;
using Model.Model.Interfaces;
using ProjectManager.Model.Entities;
using ProjectManager.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Model.UseCases
{
    public class ErrorManagement : BaseProjectManagement, IErrorManagement
    {
        private IValidationErrorRepository ErrorRepository { get; set; }

        public ErrorManagement(IValidationErrorRepository validationErrorRepository, Action<string> changeTargetDataBase,
            IRepository<MD> MDRepository, IRepository<PK> PKRepository, IRepository<ProfMod> profModRepository,
            IRepository<ProfPK> profPKRepository)
            : base(changeTargetDataBase, MDRepository, PKRepository, profModRepository, profPKRepository)
        {
            ErrorRepository = validationErrorRepository;
            ValidationError.ErrorTypes = ErrorRepository.ReadAllErrorTypes();
        }

        /// <summary>
        /// Сохраняет ошибку в бд
        /// </summary>
        /// <param name="error"></param>
        public void Add(ValidationError error)
        {
            ErrorRepository.Create(error);
        }

        /// <summary>
        /// Удаляет ошибку из бд
        /// </summary>
        /// <param name="error"></param>
        public void Delete(ValidationError error)
        {
            ErrorRepository.Delete(error.ID);
        }

        public IEnumerable<ValidationError> GetAllByReportID(int id)
        {
            return ErrorRepository.ReadAllByReportID(id);
        }

        /// <summary>
        /// Находит ошибки в проекте, добавляет к отчету и сохраняет в бд
        /// </summary>
        /// <param name="project"></param>
        /// <param name="report"></param>
        public void FindErrors(Project project, ValidationReport report)
        {
            ChangeTargetDataBase($"{project.Directory}{project.DBFolder}\\{project.DBFile}");

            //Генераторные петли
            var allMds = MDRepository.ReadAll();
            foreach (var md in GetDefaultCoordinatesLoop(allMds))
            {
                Add(new ValidationError(report.ID, $"Координаты ген. петли, ID : {md.ID}", 5));
            }
            foreach (var md in GetDefaultSizeLoop(allMds))
            {
                Add(new ValidationError(report.ID, $"размер ген. петли, ID : {md.ID}", 5));
            }
            foreach (var md in GetOverlappingIdLoop(allMds))
            {
                Add(new ValidationError(report.ID, $"Более 1 генераторной петли с ID : {md.ID}", 6));
            }

            //Пикеты
            var allPKs = PKRepository.ReadAll();
            foreach (var pk in GetDefaultCoordinatesLoop(allPKs))
            {
                Add(new ValidationError(report.ID, $"Координаты пикета, ID : {pk.ID}", 5));
            }
            foreach (var pk in GetDefaultSizeLoop(allPKs))
            {
                Add(new ValidationError(report.ID, $"ReceiverLoopSize пикета, ID : {pk.ID}", 5));
            }
            foreach (var pk in GetOverlappingIdLoop(allPKs))
            {
                Add(new ValidationError(report.ID, $"Более 1 пикета с ID : {pk.ID}", 6));
            }

            //Разносы
            foreach (var pk in GetUniqueOffsetPK(allPKs))
            {
                Add(new ValidationError(report.ID, $"Уникальный разнос ({pk.Offset}), ID пикета: {pk.ID}", 2));
            }
            foreach ((PK pk, double distance) in GetRealOffset(allMds, allPKs))
            {
                // Процент, который составляет реальный разнос от указанного
                double percent = distance / pk.Offset * 100;

                //Если разница более 10 процентов
                if (Math.Abs(100 - percent) > 10)
                {
                    Add(new ValidationError(report.ID, $"Реальный разнос ({distance:f2}) отличается >10% от указанного ({pk.Offset}), " +
                        $"ID пикета: {pk.ID}", 2));
                }
            }

            //Для предотвращения возможных ошибок
            ChangeTargetDataBase("");
        }

        /// <summary>
        /// Пересчитывает и обновляет координаты ген. петель на основе пикетов, потом пикеты на основе ген. петель
        /// </summary>
        /// <param name="project"></param>
        public void FixErrors(Project project)
        {
            ChangeTargetDataBase($"{project.Directory}{project.DBFolder}\\{project.DBFile}");

            var allMds = MDRepository.ReadAll();
            var invalidCoordinatesMDs = GetDefaultCoordinatesLoop(allMds);
            var allPKs = PKRepository.ReadAll();
            var invalidCoordinatesPKs = GetDefaultCoordinatesLoop(allPKs);

            //Исправление координат генераторных петель
            for (int i = 0; i < invalidCoordinatesMDs.Count; i++)
            {
                ///все соосные пикеты с правильными координатами, относящиеся к текущей генераторной петле
                var relatedPKs = allPKs
                    .Where(pk => pk.MDID == invalidCoordinatesMDs[i].ID && pk.Offset == 0)
                    .Where(pk => !invalidCoordinatesPKs.Contains(pk)).ToList();

                if (relatedPKs.Count() != 0)
                {
                    PK overlappingPK = relatedPKs[0];

                    //Если у ГП дефолтный центр, то устанавливает значение центра соосного пикета
                    if (invalidCoordinatesMDs[i].Qx == MD.DefaultValues["Qx"])
                        invalidCoordinatesMDs[i].Qx = overlappingPK.X;
                    if (invalidCoordinatesMDs[i].Qy == MD.DefaultValues["Qy"])
                        invalidCoordinatesMDs[i].Qy = overlappingPK.Y;

                    //Исправление координат всех углов ГП на основе центра ГП 
                    invalidCoordinatesMDs[i].Ax = invalidCoordinatesMDs[i].Qx - invalidCoordinatesMDs[i].TransmitterLoopSize / 2;
                    invalidCoordinatesMDs[i].Ay = invalidCoordinatesMDs[i].Qy + invalidCoordinatesMDs[i].TransmitterLoopSize / 2;
                    invalidCoordinatesMDs[i].Bx = invalidCoordinatesMDs[i].Qx + invalidCoordinatesMDs[i].TransmitterLoopSize / 2;
                    invalidCoordinatesMDs[i].By = invalidCoordinatesMDs[i].Qy + invalidCoordinatesMDs[i].TransmitterLoopSize / 2;
                    invalidCoordinatesMDs[i].Cx = invalidCoordinatesMDs[i].Qx + invalidCoordinatesMDs[i].TransmitterLoopSize / 2;
                    invalidCoordinatesMDs[i].Cy = invalidCoordinatesMDs[i].Qy - invalidCoordinatesMDs[i].TransmitterLoopSize / 2;
                    invalidCoordinatesMDs[i].Dx = invalidCoordinatesMDs[i].Qx - invalidCoordinatesMDs[i].TransmitterLoopSize / 2;
                    invalidCoordinatesMDs[i].Dy = invalidCoordinatesMDs[i].Qy - invalidCoordinatesMDs[i].TransmitterLoopSize / 2;

                    MDRepository.Update(invalidCoordinatesMDs[i]);
                }
            }

            //Исправление координат пикетов

            foreach (var invalidPK in invalidCoordinatesPKs)
            {
                var relatedMD = MDRepository.Read(invalidPK.MDID);
                if (relatedMD != null && !invalidCoordinatesMDs.Contains(relatedMD))
                {
                    invalidPK.X = relatedMD.Qx + invalidPK.Offset;
                    invalidPK.Y = relatedMD.Qy;

                    PKRepository.Update(invalidPK);
                }
            }

            //Для предотвращения возможных ошибок
            ChangeTargetDataBase("");
        }

        /// <summary>
        /// Возвращает все петли с одинаковыми ID
        /// </summary>
        /// <param name="allMds"></param>
        /// <returns></returns>
        private List<MD> GetOverlappingIdLoop(IEnumerable<MD> allMds)
        {
            var overlappingMDs = new List<MD>();
            var IDSet = new HashSet<int>();

            foreach(var md in allMds)
            {
                if(IDSet.Contains(md.ID))
                {
                    overlappingMDs.Add(md);
                }
                else
                {
                    IDSet.Add(md.ID);
                }
            }

            return overlappingMDs;
        }

        /// <summary>
        /// Возвращает все петли с одинаковыми ID
        /// </summary>
        /// <param name="allMds"></param>
        /// <returns></returns>
        private List<PK> GetOverlappingIdLoop(IEnumerable<PK> allPks)
        {
            var overlappingPKs = new List<PK>();
            var IDSet = new HashSet<int>();

            foreach (var md in allPks)
            {
                if (IDSet.Contains(md.ID))
                {
                    overlappingPKs.Add(md);
                }
                else
                {
                    IDSet.Add(md.ID);
                }
            }

            return overlappingPKs;
        }

        /// <summary>
        /// Возвращает все петли с координатами по умолчанию
        /// </summary>
        /// <param name="MDs"></param>
        /// <returns></returns>
        private List<MD> GetDefaultCoordinatesLoop(IEnumerable<MD> MDs)
        {
            var invalidMDs = new List<MD>();

            foreach (var md in MDs)
            {
                //Координаты
                if (md.Ax == MD.DefaultValues["Ax"] ||
                    md.Ay == MD.DefaultValues["Ay"] ||
                    md.Bx == MD.DefaultValues["Bx"] ||
                    md.By == MD.DefaultValues["By"] ||
                    md.Cx == MD.DefaultValues["Cx"] ||
                    md.Cy == MD.DefaultValues["Cy"] ||
                    md.Dx == MD.DefaultValues["Dx"] ||
                    md.Dy == MD.DefaultValues["Dy"] ||
                    md.Dy == MD.DefaultValues["Qx"] ||
                    md.Dy == MD.DefaultValues["Qy"])
                {
                    invalidMDs.Add(md);
                }
            }
            return invalidMDs;
        }

        /// <summary>
        /// Возвращает все петли с координатами по умолчанию
        /// </summary>
        /// <param name="MDs"></param>
        /// <returns></returns>
        private List<PK> GetDefaultCoordinatesLoop(IEnumerable<PK> PKs)
        {
            var invalidPKs = new List<PK>();

            foreach (var pk in PKs)
            {
                if (pk.X == PK.DefaultValues["X"] ||
                    pk.X == PK.DefaultValues["Y"])
                {
                    invalidPKs.Add(pk);
                }
            }

            return invalidPKs;
        }

        /// <summary>
        /// Возвращает все петли с размером петли по умолчанию
        /// </summary>
        /// <param name="MDs"></param>
        /// <returns></returns>
        private List<MD> GetDefaultSizeLoop(IEnumerable<MD> MDs)
        {
            var defaultLoopSizeMDs = new List<MD>();
            foreach (var md in MDs)
            {
                //Размер петли
                if (md.TransmitterLoopSize == MD.DefaultValues["TransmitterLoopSize"] ||
                    md.TransmitterLoopSize2 == MD.DefaultValues["TransmitterLoopSize2"])
                {
                    defaultLoopSizeMDs.Add(md);
                }
            }

            return defaultLoopSizeMDs;
        }

        /// <summary>
        /// Возвращает все петли с размером петли по умолчанию
        /// </summary>
        /// <param name="MDs"></param>
        /// <returns></returns>
        private List<PK> GetDefaultSizeLoop(IEnumerable<PK> PKs)
        {
            var defaultLoopSizePKs = new List<PK>();

            foreach (var pk in PKs)
            {
                if (pk.ReceiverLoopSize == PK.DefaultValues["ReceiverLoopSize"])
                {
                    defaultLoopSizePKs.Add(pk);
                }
            }

            return defaultLoopSizePKs;
        }

        /// <summary>
        /// Вычисляет реальные разносы на основе координат пикетов и ген. петель
        /// </summary>
        /// <param name="PKs"></param>
        /// <returns></returns>
        private List<(PK, double)> GetRealOffset(IEnumerable<MD> MDs, IEnumerable<PK> PKs)
        {
            var offsets = new List<(PK, double)>();

            foreach(var pk in PKs)
            {
                var relatedMD = MDs.Where(md => md.ID == pk.MDID).First();
                if (relatedMD == null)
                    continue;
                double distance = Geometry.GetDistance(pk.X, pk.Y, relatedMD.Qx, relatedMD.Qy);

                offsets.Add((pk,distance));
            }
            return offsets;
        }

        /// <summary>
        /// Возвращает пикеты с уникальным значением разноса
        /// </summary>
        /// <param name="PKs"></param>
        /// <returns></returns>
        private List<PK> GetUniqueOffsetPK(IEnumerable<PK> PKs)
        {
            return PKs
                .GroupBy(pk => pk.Offset)
                .Where(g => g.Count() == 1)
                .Select(g => g.First())
                .ToList();
        }
    }
}
