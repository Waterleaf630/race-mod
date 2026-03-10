using Godot;
using racemod.race_mod.save;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace racemod.race_mod.ui
{
    public partial class BattelSL : Control
    {

        Texture2D tex1;
        Texture2D tex0;

        public override void _Ready()
        {
            tex0 = GD.Load<Texture2D>("res://race-mod/image/BattleSL0.png");
            tex1 = GD.Load<Texture2D>("res://race-mod/image/BattleSL1.png");
            Scale = new Vector2(.6F, .6F);
        }

        public override void _Draw()
        {
            DrawTexture(SLManager.pf.RestBattleSL == 1? tex1 : tex0, new Vector2(20, 270));
        }
    }
}
