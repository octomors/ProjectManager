using System;

namespace Model
{
    static class Geometry
    {
        /// <summary>
        /// Вычисляет расстояние между двумя точками
        /// </summary>
        /// <returns></returns>
        public static double GetDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
    }
}
