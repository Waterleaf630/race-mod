using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using racemod.race_mod.ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace racemod.race_mod.patches
{
    public static class SeedInputPatch
    {
        public static SeedButton btn;
        [HarmonyPatch(typeof(NCharacterSelectScreen), "_Ready")]
        public static class CharacterSelectPatch
        {
            public static void Postfix(NCharacterSelectScreen __instance)
            {
                btn = new SeedButton();
                __instance.AddChild(btn);
            }
        }

        [HarmonyPatch(typeof(NCharacterSelectScreen), "OnEmbarkPressed")]
        public static class NewRunPatch2
        {
            public static bool Prefix(NCharacterSelectScreen __instance)
            {
                if (btn.input.Visible)
                {
                    return false;
                }
                if (!string.IsNullOrEmpty(btn.currentSeed))
                {
                    __instance.Lobby.SetSeed(btn.currentSeed);
                }
                return true;
            }
        }
        [HarmonyPatch(typeof(NCharacterSelectScreen), "SeedChanged")]
        public static class NewRunPatch
        {
            public static bool Prefix()
            {
                return false;
            }
        }
    }
}
