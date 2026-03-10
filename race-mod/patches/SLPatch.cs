using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Screens.PauseMenu;
using MegaCrit.Sts2.Core.Runs;
using racemod.race_mod.save;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace racemod.race_mod.patches
{
    public class SLPatch
    {

        [HarmonyPatch(typeof(RunManager), "InitializeNewRun")]
        public static class Patch1
        {
            public static void Postfix()
            {
                SLManager.pf.RestBattleSL = 1;
                SLManager.pf.RestEventSL = 1;
                SLManager.save();
            }
        }
        [HarmonyPatch(typeof(NPauseMenu), "OnSaveAndQuitButtonPressed")]
        public static class Patch2
        {
            public static bool Prefix()
            {
                if (RewardScreen.is_on)
                {
                    if (SLManager.pf.RestEventSL == 1)
                    {
                        SLManager.pf.RestEventSL = 0;
                        SLManager.save();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if(NRun.Instance.CombatRoom != null && NRun.Instance.EventRoom != null)
                {
                    if (SLManager.pf.RestEventSL == 1)
                    {
                        SLManager.pf.RestEventSL = 0;
                        SLManager.save();
                        return true;
                    }
                    else if (SLManager.pf.RestBattleSL == 1)
                    {

                        SLManager.pf.RestBattleSL = 0;
                        SLManager.save();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                        
                }
                else if(NRun.Instance.CombatRoom != null)
                {
                    if (SLManager.pf.RestBattleSL == 1)
                    {

                        SLManager.pf.RestBattleSL = 0;
                        SLManager.save();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (SLManager.pf.RestEventSL == 1)
                    {
                        SLManager.pf.RestEventSL = 0;
                        SLManager.save();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
    }
}
