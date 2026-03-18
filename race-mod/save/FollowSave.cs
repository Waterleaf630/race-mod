using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Runs;
using racemod.race_mod.patches.rngfix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace racemod.race_mod.save
{
    internal class FollowSave
    {
        public static ProfileSave pf = new ProfileSave();
        public static void save()
        {
            pf.rngSeed = CardRewardFix.myRng.Seed;
            pf.rngCount = CardRewardFix.myRng.Counter;
            pf.nicheSeed = TransformationPatch.MyNiche.Seed;
            pf.nicheCount = TransformationPatch.MyNiche.Counter;
            string json = System.Text.Json.JsonSerializer.Serialize(pf);
            var file = FileAccess.Open("user://racemod2.json", FileAccess.ModeFlags.Write);
            file.StoreString(json);
            file.Close();
        }
        public static void load()
        {
            var file = FileAccess.Open("user://racemod2.json", FileAccess.ModeFlags.Read);
            string json = file.GetAsText();
            file.Close();
            try
            {
                pf = System.Text.Json.JsonSerializer.Deserialize<ProfileSave>(json);
            }
            catch (Exception e)
            {
                pf = new ProfileSave();
            }
        }

        [HarmonyPatch(typeof(RunManager), "InitializeSavedRun")]
        public static class Patch1
        {
            public static void Postfix()
            {
                load();
            }
        }

        [HarmonyPatch(typeof(RunManager), "ToSave")]
        public static class Patch2
        {
            public static void Postfix()
            {
                save();
            }
        }
    }
}
