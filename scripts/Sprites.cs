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

    public static readonly Texture2D[,] Spr_Zombie_Walk = new Texture2D[6,5];
    public static readonly Texture2D[,] Spr_Zombie_Attack = new Texture2D[7,5];
    public static readonly Texture2D[] Spr_Zombie_Death = new Texture2D[5];

    // Left, Right, Top, Bottom
    public static readonly SpriteOffset Spr_NoOffset = new(0, 0, 0, 0);

    public readonly static SpriteOffset[] Spr_Wep_Offset = new SpriteOffset[(int)WeaponType.NoOfWeapons - 1] {
        new(-37, 37, 0, 35),
        new(0, 60, 0, 30)
    };

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

        for (int frame = 0; frame < Spr_Zombie_Death.Length; frame++) {
            Spr_Zombie_Death[frame] = GD.Load<Texture2D>($"res://textures/enemies/zombie/z_die{frame}.png");
        }
    }
}
