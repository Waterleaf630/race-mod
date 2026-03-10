using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace racemod.race_mod.patches
{
    public class RewardScreen
    {
        public static bool is_on;

        [HarmonyPatch(typeof(NRewardsScreen), "ShowScreen")]
        public static class Patch1
        {
            public static void Prefix()
            {
                is_on = true;
            }
        }

        [HarmonyPatch(typeof(RunManager), "EnterRoom")]
        public static class Patch2
        {
            public static void Prefix()
            {
                is_on = false;
            }
        }
    }
}
