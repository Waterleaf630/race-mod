using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.TopBar;
using MegaCrit.Sts2.Core.Odds;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using racemod.race_mod.save;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace racemod.race_mod.patches.rngfix
{
    public static class CardRewardFix
    {
        public static Rng myRng;

        [HarmonyPatch(typeof(RunManager), "InitializeNewRun")]
        public static class Patch0
        {
            public static void Postfix()
            {
                FollowSave.pf.regularCount = 0;
                FollowSave.pf.eliteCount = 0;
                FollowSave.pf.bossCount = 0;
                myRng = new Rng(SLManager.pf.gameSeed + 50);
            }
        }
        [HarmonyPatch(typeof(RunManager), "InitializeSavedRun")]
        public static class Patch00
        {
            public static void Postfix()
            {
                myRng = new Rng(FollowSave.pf.rngSeed, FollowSave.pf.rngCount);
            }
        }
        [HarmonyPatch(typeof(CardReward), "Populate")]
        public static class Patch1
        {
            public static void Prefix(CardReward __instance)
            {
                CardCreationOptions Options = (CardCreationOptions)AccessTools.Field(typeof(CardReward), "<Options>k__BackingField").GetValue(__instance);

                AccessTools.Field(typeof(CardCreationOptions), "<RngOverride>k__BackingField").SetValue(Options, myRng);
            }
        }
        //[HarmonyPatch(typeof(CardReward), MethodType.Constructor,
        //    new Type[] { typeof(IEnumerable<CardModel>), typeof(CardCreationSource), typeof(Player) })]
        //public static class PatchDEBUG
        //{
        //    public static void Prefix()
        //    {
        //        Log.Warn("MANNUAL FUNCTION CALLED");
        //    }
        //}
        //[HarmonyPatch(typeof(RunManager), "EnterRoom")]
        //public static class Patch2
        //{
        //    public static void Prefix(AbstractRoom room)
        //    {
        //        if(room is MerchantRoom)
        //        {
        //            ++FollowSave.pf.shopCount;
        //            myRng = new Rng(SLManager.pf.gameSeed + 400 + (uint)FollowSave.pf.shopCount);
        //        }
        //    }
        //}
        [HarmonyPatch(typeof(RewardsSet), "GenerateWithoutOffering")]
        public static class Patch2
        {
            public static void Prefix()
            {
                switch (((RunState)AccessTools.Field(typeof(RunManager), "<State>k__BackingField").
                    GetValue(RunManager.Instance)).CurrentRoom.RoomType)
                {
                    case RoomType.Monster:
                        ++FollowSave.pf.regularCount;
                        myRng = new Rng(SLManager.pf.gameSeed + 100 + (uint)FollowSave.pf.regularCount);
                        break;
                    case RoomType.Elite:
                        ++FollowSave.pf.eliteCount;
                        myRng = new Rng(SLManager.pf.gameSeed + 200 + (uint)FollowSave.pf.eliteCount);
                        break;
                    case RoomType.Boss:
                        ++FollowSave.pf.bossCount;
                        myRng = new Rng(SLManager.pf.gameSeed + 300 + (uint)FollowSave.pf.bossCount);
                        break;
                }
            }
        }

        [HarmonyPatch(typeof(CardRarityOdds), "RollWithBaseOdds")]
        public static class Patch3
        {
            public static bool Prefix(CardRarityOddsType type, ref CardRarity __result)
            {
                float num = myRng.NextFloat();
                if (num < GetBaseOdds(type, CardRarity.Rare))
                {
                    __result = CardRarity.Rare;
                    return false;
                }
                if (num < GetBaseOdds(type, CardRarity.Uncommon))
                {
                    __result = CardRarity.Uncommon;
                    return false;
                }
                __result = CardRarity.Common;
                return false;
            }
        }
        [HarmonyPatch(typeof(CardRarityOdds), "RollWithoutChangingFutureOdds",
            new Type[] { typeof(CardRarityOddsType), typeof(float) })]
        public static class Patch4
        {
            public static bool Prefix(CardRarityOddsType type, float offset, ref CardRarity __result)
            {
                float num = myRng.NextFloat();
                float num2 = GetBaseOdds(type, CardRarity.Rare) + offset;
                if (num < num2)
                {
                    __result = CardRarity.Rare;
                    return false;
                }
                if (num < GetBaseOdds(type, CardRarity.Uncommon) + num2)
                {
                    __result = CardRarity.Uncommon;
                    return false;
                }
                __result = CardRarity.Common;
                return false;
            }
        }
        [HarmonyPatch(typeof(PotionRewardOdds), "Roll")]
        public static class Patch5
        {
            public static bool Prefix(PotionRewardOdds __instance, Player player, AscensionManager ascensionManager, RoomType roomType, ref bool __result)
            {
                Log.Warn(myRng.Counter.ToString());
                float currentValue = __instance.CurrentValue;
                bool flag = Hook.ShouldForcePotionReward(player.RunState, player, roomType);
                float num = myRng.NextFloat();
                if (num < currentValue || flag)
                {
                    AccessTools.Field(typeof(PotionRewardOdds), "<CurrentValue>k__BackingField").SetValue(__instance, __instance.CurrentValue - 0.1f);
                }
                else
                {
                    AccessTools.Field(typeof(PotionRewardOdds), "<CurrentValue>k__BackingField").SetValue(__instance, __instance.CurrentValue + 0.1f);
                }
                float num2 = ((roomType != RoomType.Elite) ? 0f : 0.25f);
                float num3 = num2;
                if (!flag)
                {
                    __result = num < currentValue;// + num3 * 0.5f;
                    return false;
                }
                __result = true;
                return false;
            }
        }
        [HarmonyPatch(typeof(GoldReward), "Populate")]
        public static class Patch6
        {
            public static bool Prefix(GoldReward __instance, ref Task __result)
            {
                AccessTools.Field(typeof(GoldReward), "<Amount>k__BackingField").SetValue(__instance,
                    myRng.NextInt(
                   (int)AccessTools.Field(typeof(GoldReward), "_min").GetValue(__instance),
                   (int)AccessTools.Field(typeof(GoldReward), "_max").GetValue(__instance) + 1));
                __result = Task.CompletedTask;
                return false;
            }
        }
        public static float GetBaseOdds(CardRarityOddsType type, CardRarity rarity)
        {
            return type switch
            {
                CardRarityOddsType.EliteEncounter => rarity switch
                {
                    CardRarity.Common => CardRarityOdds.EliteCommonOdds,
                    CardRarity.Uncommon => 0.4f,
                    CardRarity.Rare => CardRarityOdds.EliteRareOdds,
                    _ => throw new ArgumentOutOfRangeException("rarity"),
                },
                CardRarityOddsType.BossEncounter => rarity switch
                {
                    CardRarity.Common => 0f,
                    CardRarity.Uncommon => 0f,
                    CardRarity.Rare => 1f,
                    _ => throw new ArgumentOutOfRangeException("rarity"),
                },
                CardRarityOddsType.Shop => rarity switch
                {
                    CardRarity.Common => CardRarityOdds.ShopCommonOdds,
                    CardRarity.Uncommon => 0.37f,
                    CardRarity.Rare => CardRarityOdds.ShopRareOdds,
                    _ => throw new ArgumentOutOfRangeException("rarity"),
                },
                CardRarityOddsType.RegularEncounter => rarity switch
                {
                    CardRarity.Common => CardRarityOdds.regularCommonOdds,
                    CardRarity.Uncommon => 0.37f,
                    CardRarity.Rare => CardRarityOdds.RegularRareOdds,
                    _ => throw new ArgumentOutOfRangeException("rarity"),
                },
                CardRarityOddsType.Uniform => rarity switch
                {
                    CardRarity.Common => 0.33f,
                    CardRarity.Uncommon => 0.33f,
                    CardRarity.Rare => 0.33f,
                    _ => throw new ArgumentOutOfRangeException("rarity"),
                },
                _ => throw new ArgumentOutOfRangeException("type"),
            };
        }
    }
}
