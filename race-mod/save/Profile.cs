using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace racemod.race_mod.save
{
    internal class Profile
    {
        public float LastEnterTime { get; set; }
        public int RestBattleSL { get; set; }
        public int RestEventSL { get; set; }
        public int Act3BossSlain { get; set; }
        public float saveTime { get; set; }
        
        public uint gameSeed { get; set; }
        public Profile()
        {
            LastEnterTime = 0;
            RestBattleSL = 0;
            RestEventSL = 0;
            saveTime = 0;
            Act3BossSlain = 0;
            gameSeed = 0;
        }
    }
}
