namespace Model.Model.Entities
{
    public abstract class Entity
    {
        public int ID { get; set; }

        /// <summary>
        /// вернет новый айди = $"{overlappingNumber}000000{Старый_ID}"
        /// </summary>
        /// <param name="overlappingNumber">число, которое будет добавлено перед старым айди</param>
        public int GenerateNewID(int overlappingNumber)
        {
            return int.Parse($"{overlappingNumber}000000{ID}");
        }
    }
}
