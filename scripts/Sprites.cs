using Godot;

public readonly struct SpriteOffset {
    public readonly float Left;
    public readonly float Right;
    public readonly float Top;
    public readonly float Bottom;

    public SpriteOffset(float left, float right, float top, float bottom) {
        Left = left;
        Right = right;
        Top = top;
        Bottom = bottom;
    }
}

public static class Sprites {
    public static readonly Texture2D[] Spr_Wep_PF = new Texture2D[3] {
        GD.Load<Texture2D>("res://textures/weapons/pitchfork/w_pf.png"),
        GD.Load<Texture2D>("res://textures/weapons/pitchfork/w_pf_atk0.png"),
        GD.Load<Texture2D>("res://textures/weapons/pitchfork/w_pf_atk1.png")
    };

    // SHOTGUN
    public static readonly Texture2D[] Spr_Wep_SG = new Texture2D[5] {
        GD.Load<Texture2D>("res://textures/weapons/shotgun/w_sg.png"),
        GD.Load<Texture2D>("res://textures/weapons/shotgun/w_sg_rl0.png"),
        GD.Load<Texture2D>("res://textures/weapons/shotgun/w_sg_rl1.png"),
        GD.Load<Texture2D>("res://textures/weapons/shotgun/w_sg_rl2.png"),
        GD.Load<Texture2D>("res://textures/weapons/shotgun/w_sg_rl3.png")
    };

    public static readonly Texture2D[] XSpr_Wep_SG = new Texture2D[6] {
        GD.Load<Texture2D>("res://textures/weapons/shotgun/xw_sg_rl0.png"),
        GD.Load<Texture2D>("res://textures/weapons/shotgun/xw_sg_rl1.png"),
        GD.Load<Texture2D>("res://textures/weapons/shotgun/xw_sg_flash0.png"),
        GD.Load<Texture2D>("res://textures/weapons/shotgun/xw_sg_flash1.png"),
        GD.Load<Texture2D>("res://textures/weapons/shotgun/xw_sg_flash2.png"),
        GD.Load<Texture2D>("res://textures/weapons/shotgun/w_sg_fire.png")
    };


    // LIGHTER
    public static readonly Texture2D Spr_Wep_Light_Idle = GD.Load<Texture2D>("res://textures/weapons/lighter/w_lighter_idle.png");
    public static readonly Texture2D Spr_Wep_Light_Strike = GD.Load<Texture2D>("res://textures/weapons/lighter/w_lighter_strike.png");

    public static readonly Texture2D[] Spr_Wep_Light_Up = new Texture2D[3] {
        GD.Load<Texture2D>("res://textures/weapons/lighter/w_lighter_up0.png"),
        GD.Load<Texture2D>("res://textures/weapons/lighter/w_lighter_up1.png"),
        GD.Load<Texture2D>("res://textures/weapons/lighter/w_lighter_up2.png")
    };

    public static readonly Texture2D[] Spr_Wep_Light_Down = new Texture2D[3] {
        GD.Load<Texture2D>("res://textures/weapons/lighter/w_lighter_down0.png"),
        GD.Load<Texture2D>("res://textures/weapons/lighter/w_lighter_down1.png"),
        GD.Load<Texture2D>("res://textures/weapons/lighter/w_lighter_down2.png")
    };

    public static readonly Texture2D[] Spr_Wep_Light_Flame = new Texture2D[5] {
        GD.Load<Texture2D>("res://textures/weapons/lighter/w_lighter_flame0.png"),
        GD.Load<Texture2D>("res://textures/weapons/lighter/w_lighter_flame1.png"),
        GD.Load<Texture2D>("res://textures/weapons/lighter/w_lighter_flame2.png"),
        GD.Load<Texture2D>("res://textures/weapons/lighter/w_lighter_flame3.png"),
        GD.Load<Texture2D>("res://textures/weapons/lighter/w_lighter_flame4.png")
    };

    // DYNAMITE
    public static readonly Texture2D Spr_Wep_DynaReg_Idle = GD.Load<Texture2D>("res://textures/weapons/dynamite/w_dyn_reg_idle.png");

    public static readonly Texture2D[] Spr_Wep_DynaReg_Throw = new Texture2D[2] {
        GD.Load<Texture2D>("res://textures/weapons/dynamite/w_dyn_throw0.png"),
        GD.Load<Texture2D>("res://textures/weapons/dynamite/w_dyn_throw1.png")
    };

    public static readonly Texture2D Spr_Wep_DynaReg_Fuse = GD.Load<Texture2D>("res://textures/weapons/dynamite/w_dyn_reg_fuse.png");

    public static readonly Texture2D[] Spr_Wep_DynaReg_Fuse_Lit = new Texture2D[3] {
        GD.Load<Texture2D>("res://textures/weapons/dynamite/w_dyn_reg_fuse_lit0.png"),
        GD.Load<Texture2D>("res://textures/weapons/dynamite/w_dyn_reg_fuse_lit1.png"),
        GD.Load<Texture2D>("res://textures/weapons/dynamite/w_dyn_reg_fuse_lit2.png")
    };

    public static readonly Texture2D[] Spr_FX_Explosion = new Texture2D[13] {
        GD.Load<Texture2D>("res://textures/effect/explosion/exflame0.png"),
        GD.Load<Texture2D>("res://textures/effect/explosion/exflame1.png"),
        GD.Load<Texture2D>("res://textures/effect/explosion/exflame2.png"),
        GD.Load<Texture2D>("res://textures/effect/explosion/exflame3.png"),
        GD.Load<Texture2D>("res://textures/effect/explosion/exflame4.png"),
        GD.Load<Texture2D>("res://textures/effect/explosion/exflame5.png"),
        GD.Load<Texture2D>("res://textures/effect/explosion/exflame6.png"),
        GD.Load<Texture2D>("res://textures/effect/explosion/exflame7.png"),
        GD.Load<Texture2D>("res://textures/effect/explosion/exflame8.png"),
        GD.Load<Texture2D>("res://textures/effect/explosion/exflame9.png"),
        GD.Load<Texture2D>("res://textures/effect/explosion/exflame10.png"),
        GD.Load<Texture2D>("res://textures/effect/explosion/exflame11.png"),
        GD.Load<Texture2D>("res://textures/effect/explosion/exflame12.png")
    };

    public static readonly Texture2D[] Spr_FX_Hitspark_Metal = new Texture2D[6] {
        GD.Load<Texture2D>("res://textures/effect/hitspark/hitspark_metal0.png"),
        GD.Load<Texture2D>("res://textures/effect/hitspark/hitspark_metal1.png"),
        GD.Load<Texture2D>("res://textures/effect/hitspark/hitspark_metal2.png"),
        GD.Load<Texture2D>("res://textures/effect/hitspark/hitspark_metal3.png"),
        GD.Load<Texture2D>("res://textures/effect/hitspark/hitspark_metal4.png"),
        GD.Load<Texture2D>("res://textures/effect/hitspark/hitspark_metal5.png")
    };

    public static readonly Texture2D[] Spr_FX_Hitspark_Wood = new Texture2D[5] {
        GD.Load<Texture2D>("res://textures/effect/hitspark/hitspark_wood0.png"),
        GD.Load<Texture2D>("res://textures/effect/hitspark/hitspark_wood1.png"),
        GD.Load<Texture2D>("res://textures/effect/hitspark/hitspark_wood2.png"),
        GD.Load<Texture2D>("res://textures/effect/hitspark/hitspark_wood3.png"),
        GD.Load<Texture2D>("res://textures/effect/hitspark/hitspark_wood4.png")
    };

    public static readonly Texture2D[] Spr_FX_Hitspark_Flesh = new Texture2D[5] {
        GD.Load<Texture2D>("res://textures/effect/hitspark/hitspark_flesh0.png"),
        GD.Load<Texture2D>("res://textures/effect/hitspark/hitspark_flesh1.png"),
        GD.Load<Texture2D>("res://textures/effect/hitspark/hitspark_flesh2.png"),
        GD.Load<Texture2D>("res://textures/effect/hitspark/hitspark_flesh3.png"),
        GD.Load<Texture2D>("res://textures/effect/hitspark/hitspark_flesh4.png")
    };

    // OTHER
    public static readonly Texture2D[,] Spr_Zombie_Walk = new Texture2D[6,5];
    public static readonly Texture2D[,] Spr_Zombie_Attack = new Texture2D[7,5];
    public static readonly Texture2D[,] Spr_Zombie_Pain = new Texture2D[1,5];
    public static readonly Texture2D[] Spr_Zombie_Death = new Texture2D[5];

    // Left, Right, Top, Bottom
    public static readonly SpriteOffset Spr_NoOffset = new(0, 0, 0, 0);

    public readonly static SpriteOffset[] Spr_Wep_Offset = new SpriteOffset[3] {//[(int)WeaponType.NoOfWeapons - 1] {
        new(-37, 37, 0, 35),
        new(0, 60, 0, 30),
        new(0, 130, 0, 40)//new(0, 65, 0, 35)
    };

    public readonly static SpriteOffset Spr_Light_Offset = new(-115, -115, 0, -5);
    public readonly static SpriteOffset Spr_Light_Flame_Offset = new(-165, 0, 0, -98);
    public readonly static SpriteOffset Spr_TNT_Idle_Offset = new(0, 50, 0, 0);

    public readonly static SpriteOffset[] Spr_SG_Flash_Offset = new SpriteOffset[3] {
        new(0, -45, 0, -60),
        new(0, -20, 0, -60),
        new(0, -35, 0, -60)
    };

    public static void IndexSpriteArrays() {
        for (int frame = 0; frame < Spr_Zombie_Walk.GetLength(0); frame++) {
            for (int dir = 0; dir < Spr_Zombie_Walk.GetLength(1); dir++) {
                Spr_Zombie_Walk[frame, dir] = GD.Load<Texture2D>($"res://textures/enemies/zombie/z_walk{frame}_rot{dir}.png");
            }
        }

        for (int frame = 0; frame < Spr_Zombie_Attack.GetLength(0); frame++) {
            for (int dir = 0; dir < Spr_Zombie_Attack.GetLength(1); dir++) {
                Spr_Zombie_Attack[frame, dir] = GD.Load<Texture2D>($"res://textures/enemies/zombie/z_atk{frame}_rot{dir}.png");
            }
        }

        for (int dir = 0; dir < Spr_Zombie_Pain.GetLength(1); dir++) {
            Spr_Zombie_Pain[0, dir] = GD.Load<Texture2D>($"res://textures/enemies/zombie/z_pain_rot{dir}.png");
        }

        for (int frame = 0; frame < Spr_Zombie_Death.Length; frame++) {
            Spr_Zombie_Death[frame] = GD.Load<Texture2D>($"res://textures/enemies/zombie/z_die{frame}.png");
        }
    }
}
