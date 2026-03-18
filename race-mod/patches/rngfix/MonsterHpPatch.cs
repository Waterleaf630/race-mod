using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace racemod.race_mod.patches.rngfix
{
    public static class MonsterHpPatch
    {
        [HarmonyPatch(typeof(CombatState), "CreateCreature")]
        public static class Patch0
        {
            public static bool Prefix(CombatState __instance, MonsterModel monster, CombatSide side, string? slot, ref Creature __result)
            {
                monster.AssertMutable();
                monster.RunRng = __instance.RunState.Rng;
                Creature creature = new Creature(monster, side, slot);
                var field = AccessTools.Field(typeof(CombatState), "_nextCreatureId");
                creature.CombatId = (uint)field.GetValue(__instance);
                field.SetValue(__instance, creature.CombatId + 1);
                creature.CombatState = __instance;
                monster.Rng = new Rng((uint)((__instance.RunState.Rng.Seed + StringHelper.GetDeterministicHashCode(__instance.Encounter.Id.Entry) + creature.CombatId)));
                if (side == CombatSide.Enemy)
                {
                    int res = monster.Rng.NextInt(monster.MinInitialHp, monster.MaxInitialHp + 1);
                    AccessTools.Field(typeof(Creature), "_maxHp").SetValue(creature, res);
                    AccessTools.Field(typeof(Creature), "_currentHp").SetValue(creature, res);
                    AccessTools.Field(typeof(Creature), "<MonsterMaxHpBeforeModification>k__BackingField").SetValue(creature, res);
                    creature.ScaleMonsterHpForMultiplayer(__instance.Encounter, __instance.Players.Count, __instance.RunState.CurrentActIndex);
                }
                __result = creature;
                return false;
            }
        }
    }
}
