using System.Collections.Generic;

namespace Model.Model.Entities
{
    /// <summary>
    /// Пикет
    /// </summary>
    public class PK : Entity
    {
        /// <summary>
        /// Координаты петли
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// Координаты петли
        /// </summary>
        public int Y { get; set; }
        /// <summary>
        /// Высота петли
        /// </summary>
        public int H { get; set; }
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int H1 { get; set; }
        public int Az { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
        public int H2 { get; set; }
        public string PKType { get; set; }
        /// <summary>
        /// ID генераторной петли
        /// </summary>
        public int MDID { get; set; }
        /// <summary>
        /// Длина одной стороны петли
        /// </summary>
        public int ReceiverLoopSize { get; set; }
        /// <summary>
        /// Расстояние от центра генераторной петли
        /// </summary>
        public int Offset { get; set; }
        public string Profile { get; set; }
        public bool ControlPK { get; set; }
        public string Remarks { get; set; }
        public string FReserved { get; set; }
        public int Lon { get; set; }
        public int Lat { get; set; }

        public static Dictionary<string, int> DefaultValues { get; set; } = new Dictionary<string, int>()
        {
            { "ID", 1 },
            { "X", -1 },
            { "Y", -1 },
            { "H", 0 },
            { "X1", -1 },
            { "Y1", -1 },
            { "H1", -1 },
            { "Az", 0 },
            { "X2", -1 },
            { "Y2", -1 },
            { "H2", -1 },
            { "ReceiverLoopSize", 200 },
            { "Offset", 0 },
            { "Lon", 0 },
            { "Lat", 0 },
        };
    }
}
