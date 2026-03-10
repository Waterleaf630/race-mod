using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using racemod.race_mod.save;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace racemod.race_mod.patches.rngfix
{
    public static class TransformationPatch
    {
        public static Rng MyNiche;
        [HarmonyPatch(typeof(CardFactory), "GetFilteredTransformationOptions")]
        public static class Patch1
        {
            public static bool Prefix(CardModel original, IEnumerable<CardModel> originalOptions, bool isInCombat, ref CardModel[] __result)
            {
                IEnumerable<CardModel> source = originalOptions;
                CardRarity rarity = original.Rarity;
                if ((uint)(rarity - 8) > 1u)
                {
                    source = source.Where(delegate (CardModel c)
                    {
                        CardRarity rarity2 = c.Rarity;
                        return (uint)(rarity2 - 2) <= 2u;
                    });
                }
                if (isInCombat)
                {
                    source = source.Where((CardModel c) => c.CanBeGeneratedInCombat);
                }
                //source = source.Where((CardModel c) => c.Id != original.Id).ToList();
                __result = source.Where((CardModel c) => c.MultiplayerConstraint != CardMultiplayerConstraint.MultiplayerOnly).ToArray();
                return false;
            }
        }
        [HarmonyPatch(typeof(RunRngSet), "GetRng")]
        public static class Patch2
        {
            public static bool Prefix(RunRngSet __instance, RunRngType rngType, ref Rng __result)
            {
                if (rngType == RunRngType.Niche)
                {
                    __result = MyNiche;
                    return false;
                }
                return true;
            }
        }
        [HarmonyPatch(typeof(PandorasBox), "AfterObtained")]
        public static class Patch3
        {
            public static void Prefix()
            {
                MyNiche = new Rng(SLManager.pf.gameSeed + 100003);
            }
            [HarmonyPatch(typeof(CombatState), "CreateCreature")]
            public static class Patch4
            {
                public static void Prefix(MonsterModel monster)
                {
                    MyNiche = new Rng(SLManager.pf.gameSeed + (uint)StringHelper.GetDeterministicHashCode(monster.ToString()));
                }
            }
        }
        [HarmonyPatch(typeof(Astrolabe), "AfterObtained")]
        public static class Patch5
        {
            public static void Prefix()
            {
                MyNiche = new Rng(SLManager.pf.gameSeed + 100005);
            }
        }
        [HarmonyPatch(typeof(NewLeaf), "AfterObtained")]
        public static class Patch6
        {
            public static void Prefix()
            {
                MyNiche = new Rng(SLManager.pf.gameSeed + 100006);
            }
        }
        [HarmonyPatch(typeof(SereTalon), "AfterObtained")]
        public static class Patch7
        {
            public static void Prefix()
            {
                MyNiche = new Rng(SLManager.pf.gameSeed + 100007);
            }
        }
        [HarmonyPatch(typeof(DollRoom), "ChooseRandom")]
        public static class Patch8
        {
            public static void Prefix()
            {
                MyNiche = new Rng(SLManager.pf.gameSeed + 100008);
            }
        }
        [HarmonyPatch(typeof(MorphicGrove), "Group")]
        public static class Patch9
        {
            public static void Prefix()
            {
                MyNiche = new Rng(SLManager.pf.gameSeed + 100009);
            }
        }
        [HarmonyPatch(typeof(Trial), "NondescriptInnocent")]
        public static class Patch10
        {
            public static void Prefix()
            {
                MyNiche = new Rng(SLManager.pf.gameSeed + 100010);
            }
        }
    }
}
