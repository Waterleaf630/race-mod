using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Runs;
using racemod.race_mod.save;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace racemod.race_mod.patches.rngfix
{
    public static class GrabSeed
    {
        [HarmonyPatch(typeof(RunRngSet), MethodType.Constructor, new Type[] { typeof(string) })]
        public static class Patch
        {
            public static void Postfix(RunRngSet __instance)
            {
                SLManager.pf.gameSeed = __instance.Seed;
                SLManager.save();
            }
        }
    }
}
