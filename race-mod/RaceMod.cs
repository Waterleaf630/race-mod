using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using System;


[ModInitializer(nameof(Initialize))]
public partial class RaceMod : Node
{
    public static void Initialize()
    {
        Harmony harmony = new Harmony("race-mod");
        harmony.PatchAll();
        Console.WriteLine("HELLO");
    }
}


// 需求列表
// 精确计时
// SL 限制