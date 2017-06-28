using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary
{
    public class Training
    {
        public int TID { get; set; }
        public string className { get; set; }
        public int? numberOfSets { get; set; }
        public int? numberOfReps { get; set; }
        public int exercise_EID { get; set; }
        public int? durationMin { get; set; }
        public int? restBetweenMin { get; set; }
        public bool sunday { get; set; }
        public bool monday { get; set; }
        public bool tuesday { get; set; }
        public bool wednesday { get; set; }
        public bool thursday { get; set; }
        public bool friday { get; set; }
        public bool saturday { get; set; }
        public string timeOfDay { get; set; }
        public string name { get; set; }
    }
}
