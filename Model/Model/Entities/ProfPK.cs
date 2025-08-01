using System;
using System.Collections.Generic;

namespace Model.Model.Entities
{
    public class ProfPK : Entity
    {
        public int PKID {get; set; }
        public int MDID { get; set; }
        public int Station { get; set; }
        public bool ControlPK { get; set; }
        public int NumberRecord { get; set; }
        public int NumberChannel { get; set; }
        public int NumberADC { get; set; }
        public int NumberLoop { get; set; }
        public string NameOutFile { get; set; }
        public DateTime Date { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeFinish { get; set; }
        public string Operator { get; set; }
        public string Rejim { get; set; }
        public int GainChannel { get; set; }
        public int DeltaT { get; set; }
        public int NumberOfMeasuredPoints { get; set; }
        public int NumberOfImpulses { get; set; }
        public int TimeOfCharge { get; set; }
        public int ReceiverNumberCoins { get; set; }
        public int Current { get; set; }
        public int Shift { get; set; }
        public string Comment { get; set; }
        public int Period { get; set; }
        public int CurrentADCQuant { get; set; }
        public int ReceiverLoopSize { get; set; }
        public int LoopShunt { get; set; }
        public int MDShunt { get; set; }
        public int SensRole { get; set; }
        public int SensCoeff { get; set; }

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
