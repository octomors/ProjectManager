using System.Collections.Generic;

namespace Model.Model.Entities
{
    /// <summary>
    /// Генераторная петля (источник)
    /// </summary>
    public class MD : Entity
    {
        /// <summary>
        /// Левый верхний угол петли
        /// </summary>
        public int Ax { get; set; }
        /// <summary>
        /// Левый верхний угол петли
        /// </summary>
        public int Ay { get; set; }
        /// <summary>
        /// Правый верхний угол петли
        /// </summary>
        public int Bx { get; set; }
        /// <summary>
        /// Правый верхний угол петли
        /// </summary>
        public int By { get; set; }
        /// <summary>
        /// Правый нижний угол петли
        /// </summary>
        public int Cx { get; set; }
        /// <summary>
        /// Правый нижний угол петли
        /// </summary>
        public int Cy { get; set; }
        /// <summary>
        /// Левый нижний угол петли
        /// </summary>
        public int Dx { get; set; }
        /// <summary>
        /// Левый нижний угол петли
        /// </summary>
        public int Dy { get; set; }
        /// <summary>
        /// Центр петли
        /// </summary>
        public int Qx { get; set; }
        /// <summary>
        /// Центр петли
        /// </summary>
        public int Qy { get; set; }
        /// <summary>
        /// Высота
        /// </summary>
        public int H { get; set; }
        public string DType { get; set; }
        public bool ControlMD { get; set; }
        /// <summary>
        /// Длина 1 стороны петли
        /// </summary>
        public int TransmitterLoopSize { get; set; }
        public int TransmitterLoopSize2 { get; set; }
        public string Remarks { get; set; }
        public int TransmitterNumberCoins { get; set; }

        public static Dictionary<string, int> DefaultValues { get; set; } = new Dictionary<string, int>()
        {
            { "ID", 1 },
            { "Ax", -1 },
            { "Ay", -1 },
            { "Bx", -1 },
            { "By", -1 },
            { "Cx", -1 },
            { "Cy", -1 },
            { "Dx", -1 },
            { "Dy", -1 },
            { "Qx", -1 },
            { "Qy", -1 },
            { "TransmitterLoopSize", 600 },
            { "TransmitterLoopSize2", 600 },
            { "TransmitterNumberCoins", 1 }
        };
    }
}