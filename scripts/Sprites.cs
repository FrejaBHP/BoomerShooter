using Godot;

public class SpriteOffset {
    public float Left { get; private set; }
    public float Right { get; private set; }
    public float Top { get; private set; }
    public float Bottom { get; private set; }

    public SpriteOffset(float left, float right, float top, float bottom) {
        Left = left;
        Right = right;
        Top = top;
        Bottom = bottom;
    }
}

public static class Sprites {
    public static Texture2D[] Spr_Wep_PF = new Texture2D[3];
    public static Texture2D[] Spr_Wep_SG = new Texture2D[5];
    public static Texture2D[] XSpr_Wep_SG = new Texture2D[6];

    public static Texture2D[,] Spr_Zombie_Walk = new Texture2D[6,5];
    public static Texture2D[,] Spr_Zombie_Attack = new Texture2D[7,5];
    public static Texture2D[] Spr_Zombie_Death = new Texture2D[5];

    public static SpriteOffset Spr_NoOffset = new(0, 0, 0, 0);
    public static SpriteOffset[] Spr_Wep_Offset = new SpriteOffset[(int)WeaponType.NoOfWeapons];

    public static Vector2[] fuckoff = new Vector2[2];

    public static SpriteOffset[] Spr_SG_Flash_Offset = new SpriteOffset[3];

    public static void IndexWeaponSprites() {
        Spr_Wep_PF[0] = GD.Load<Texture2D>("res://textures/weapons/pitchfork/w_pf.png");
        Spr_Wep_PF[1] = GD.Load<Texture2D>("res://textures/weapons/pitchfork/w_pf_atk0.png");
        Spr_Wep_PF[2] = GD.Load<Texture2D>("res://textures/weapons/pitchfork/w_pf_atk1.png");

        Spr_Wep_SG[0] = GD.Load<Texture2D>("res://textures/weapons/shotgun/w_sg.png");
        Spr_Wep_SG[1] = GD.Load<Texture2D>("res://textures/weapons/shotgun/w_sg_rl0.png");
        Spr_Wep_SG[2] = GD.Load<Texture2D>("res://textures/weapons/shotgun/w_sg_rl1.png");
        Spr_Wep_SG[3] = GD.Load<Texture2D>("res://textures/weapons/shotgun/w_sg_rl2.png");
        Spr_Wep_SG[4] = GD.Load<Texture2D>("res://textures/weapons/shotgun/w_sg_rl3.png");

        XSpr_Wep_SG[0] = GD.Load<Texture2D>("res://textures/weapons/shotgun/xw_sg_rl0.png");
        XSpr_Wep_SG[1] = GD.Load<Texture2D>("res://textures/weapons/shotgun/xw_sg_rl1.png");
        XSpr_Wep_SG[2] = GD.Load<Texture2D>("res://textures/weapons/shotgun/xw_sg_flash0.png");
        XSpr_Wep_SG[3] = GD.Load<Texture2D>("res://textures/weapons/shotgun/xw_sg_flash1.png");
        XSpr_Wep_SG[4] = GD.Load<Texture2D>("res://textures/weapons/shotgun/xw_sg_flash2.png");
        XSpr_Wep_SG[5] = GD.Load<Texture2D>("res://textures/weapons/shotgun/w_sg_fire.png");

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

    public static void IndexWeaponOffsets() {
        // Left, Right, Top, Bottom
        Spr_Wep_Offset[0] = new(-37, 37, 0, 35);
        Spr_Wep_Offset[1] = new(0, 60, 0, 30);

        Spr_SG_Flash_Offset[0] = new(0, -45, 0, -60);
        Spr_SG_Flash_Offset[1] = new(0, -20, 0, -60);
        Spr_SG_Flash_Offset[2] = new(0, -35, 0, -60);

        fuckoff[0] = new(0, -70);
        fuckoff[1] = new(0, 60);
    }
}
