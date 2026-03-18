using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace racemod.race_mod.save
{
    internal class ProfileSave
    {
        public int regularCount {  get; set; }
        public int eliteCount { get; set; }
        public int bossCount { get; set; }

        public uint rngSeed { get; set; }
        public int rngCount { get; set; }

        public uint nicheSeed { get; set; }
        public int nicheCount { get; set; }
    }
}
