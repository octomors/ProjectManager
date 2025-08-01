using System.Collections.Generic;

namespace Model.Model.Entities
{
    public class ProfMod : Entity
    {
        public int PKID { get; set; }
        public int MDID { get; set; }
        public string TqqFileName { get; set; }
        public string TxtFileName { get; set; }
        public string PodFileName { get; set; }
        public string AvrFileName { get; set; }
        public int Current { get; set; }
        public int Angle { get; set; }
        public int Station { get; set; }
        public string PKType { get; set; }
        public string Remarks { get; set; }
        public bool Sync { get; set; }

        public static Dictionary<string, int> DefaultValues { get; set; } = new Dictionary<string, int>()
        {
            { "ID", 1 },
            { "PK", 1 },
            { "Station", 1 },
            { "Current", 1 },
            { "Angle", -1 },
        };
    }
}
