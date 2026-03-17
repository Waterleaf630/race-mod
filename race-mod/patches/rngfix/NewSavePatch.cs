using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace racemod.race_mod.patches.rngfix
{
    public static class NewSavePatch
    {
        [HarmonyPatch(typeof(ActModel), "ApplyDiscoveryOrderModifications")]
        public static class Patch0
        {
            public static bool Prefix()
            {
                return false;
            }
        }
        [HarmonyPatch(typeof(ActModel), "GetRandomList")]
        public static class Patch1
        {
            public static bool Prefix(string seed, ref IEnumerable<ActModel> __result)
            {
                List<ActModel> res = ActModel.GetDefaultList().ToList();
                Rng rng = new Rng((uint)StringHelper.GetDeterministicHashCode(seed));
                if(rng.NextBool()) res[0] = ModelDb.Act<Underdocks>();
                __result = res;
                return false;
            }
        }

    }
}
