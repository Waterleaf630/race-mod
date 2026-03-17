using Godot;
using Godot.NativeInterop;
using HarmonyLib;
using MegaCrit.sts2.Core.Nodes.TopBar;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.GameOverScreen;
using MegaCrit.Sts2.Core.Nodes.TopBar;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using racemod.race_mod.save;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace racemod.race_mod.patches
{
    public class TimerPatch
    {
        public static float runTime = 0.0F;
        public static bool isRunning = true;
        [HarmonyPatch(typeof(RunManager), "InitializeNewRun")]
        public static class Patch0
        {
            public static void Postfix()
            {
                SLManager.pf.LastEnterTime = 0;
                SLManager.pf.saveTime = 0;
                SLManager.pf.Act3BossSlain = 0;
                SLManager.save();
                runTime = 0;
                isRunning = true;
            }
        }
        [HarmonyPatch(typeof(RunManager), "InitializeSavedRun")]
        public static class Patch1
        {
            public static void Postfix()
            {
                runTime = SLManager.pf.saveTime;
                isRunning = true;
            }
        }

        [HarmonyPatch(typeof(NRun), "_Process")]
        public static class Patch2
        {
            public static void Postfix(NRun __instance, float delta)
            {
                if(isRunning)
                    runTime += delta;
                NRunTimer timer = __instance.GlobalUi.TopBar.Timer;

                var field = AccessTools.Field(typeof(NRunTimer), "_timerLabel");
                MegaLabel label = (MegaLabel)field.GetValue(timer);
                label.SetTextAutoSize(Decode(runTime));

                timer.Show();

            }
        }
        [HarmonyPatch(typeof(RunManager), "EnterMapPointInternal")]
        public static class Patch5
        {
            public static void Prefix(RunManager __instance)
            {
                SLManager.pf.LastEnterTime = runTime;
                SLManager.save();
            }
        }
        [HarmonyPatch(typeof(RunManager), "ToSave")]
        public static class Patch6
        {
            public static void Prefix(RunManager __instance)
            {
                SLManager.pf.saveTime = runTime;
                SLManager.save();
            }
        }

        [HarmonyPatch(typeof(NRunTimer), "OnTimerTimeout")]
        public static class Patch9
        {
            public static bool Prefix(RunManager __instance)
            {
                return false;
            }
        }

        [HarmonyPatch(typeof(NGameOverScreen), "InitializeBannerAndQuote")]
        public static class Patch7
        {
            public static void Postfix(NGameOverScreen __instance)
            {
                var field = AccessTools.Field(typeof(NGameOverScreen), "_deathQuote");
                MegaRichTextLabel quote = (MegaRichTextLabel)field.GetValue(__instance);
                var field2 = AccessTools.Field(typeof(NTopBarFloorIcon), "_runState");
                IRunState state = (IRunState)field2.GetValue(NRun.Instance.GlobalUi.TopBar.FloorIcon);
                quote.Text = "进入第 " + state.TotalFloor.ToString() + " 层时间：" + Decode(SLManager.pf.LastEnterTime);
                isRunning = false;
            }
        }
        [HarmonyPatch(typeof(Hook), "AfterCombatVictory")]
        public static class Patch8
        {
            public static void Postfix(IRunState runState, CombatState? combatState, CombatRoom room)
            {
                if(runState.CurrentMapPoint.PointType == MegaCrit.Sts2.Core.Map.MapPointType.Boss &&
                    runState.Act is Glory)
                {
                    if(runState.AscensionLevel != 10)
                    {
                        isRunning = false;
                    }
                    else if(SLManager.pf.Act3BossSlain == 1)
                    {
                        isRunning = false;
                    }
                    else
                    {
                        SLManager.pf.Act3BossSlain = 1;
                        SLManager.save();
                    }
                }
            }
        }
        public static string Decode(float num)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((int)(num / 60));
            sb.Append(":");
            num -= ((int)(num / 60)) * 60;
            sb.Append((int)(num / 10));
            num -= ((int)(num / 10)) * 10;
            sb.Append((int)num);
            sb.Append(".");
            num -= (int)num;
            num *= 10;
            sb.Append((int)num);
            num -= (int)num;
            num *= 10;
            sb.Append((int)num);
            num -= (int)num;
            num *= 10;
            sb.Append((int)num);
            return sb.ToString();
        }
    }
}
