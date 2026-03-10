using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace racemod.race_mod.save
{
    internal class SLManager
    {
        public static Profile pf = new Profile();
        public static void save()
        {
            string json = System.Text.Json.JsonSerializer.Serialize(pf);
            var file = FileAccess.Open("user://racemod.json", FileAccess.ModeFlags.Write);
            file.StoreString(json);
            file.Close();
        }
        public static void load()
        {
            var file = FileAccess.Open("user://racemod.json", FileAccess.ModeFlags.Read);
            string json = file.GetAsText();
            file.Close();
            try {
                pf = System.Text.Json.JsonSerializer.Deserialize<Profile>(json);
            }
            catch (Exception e)
            {
                pf = new Profile();
            }
        }

        [HarmonyPatch(typeof(NMainMenu), "_Ready")]
        public static class Patch1
        {
            public static void Postfix()
            {
                load();
            }
        }
    }
}
