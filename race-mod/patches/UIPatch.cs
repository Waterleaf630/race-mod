using Godot.NativeInterop;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.TopBar;
using racemod.race_mod.ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace racemod.race_mod.patches
{
    public static class UIPatch
    {

        [HarmonyPatch(typeof(NGlobalUi), "_Ready")]
        public static class Patch1
        {
            public static void Postfix(NGlobalUi __instance)
            {
               __instance.AddChild(new BattelSL());
               __instance.AddChild(new EventSL());
            }
        }
    }
}
