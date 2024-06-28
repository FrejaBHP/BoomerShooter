using Godot;

public readonly struct WeaponAnimationLayer {
    public readonly int Layer;
    public readonly SpriteOffset Offset;
    public readonly int RotationDeg;
    public readonly Texture2D? Texture;
    public readonly bool DoFlipH;

    public WeaponAnimationLayer(int layer, Texture2D? texture) {
        Layer = layer;
        Offset = Sprites.Spr_NoOffset;
        RotationDeg = 0;
        Texture = texture;
        DoFlipH = false;
    }

    public WeaponAnimationLayer(int layer, SpriteOffset offset, Texture2D? texture) {
        Layer = layer;
        Offset = offset;
        RotationDeg = 0;
        Texture = texture;
        DoFlipH = false;
    }

    public WeaponAnimationLayer(int layer, SpriteOffset offset, int rotation, Texture2D? texture, bool flipH) {
        Layer = layer;
        Offset = offset;
        RotationDeg = rotation;
        Texture = texture;
        DoFlipH = flipH;
    }
}

public partial class WeaponAnimationFrame {
    public WeaponAnimationLayer[] WeaponAnimationLayers { get; set; }
    public int FrameLength { get; set; }
    public readonly AudioStreamWav? Sound;
    public readonly WeaponAction Action;

    public WeaponAnimationFrame(WeaponAnimationLayer[] Layers) {
        WeaponAnimationLayers = Layers;
    }

    public WeaponAnimationFrame(WeaponAnimationLayer[] Layers, int length, AudioStreamWav? sound, WeaponAction action) {
        WeaponAnimationLayers = Layers;
        FrameLength = length;
        Sound = sound;
        Action = action;
    }
}

public class WeaponAnimation {
    public WeaponAnimationFrame[] AnimFrames { get; set; }
    public int TotalLength { get; set; }

    public void UpdateAnimationLength() {
        for (int i = 0; i < AnimFrames.Length; i++) {
            TotalLength += AnimFrames[i].FrameLength;
        }
    }
}

public static class WAnimations {
    public static readonly WeaponAnimationFrame Frame_Wep_PF_Idle = new WeaponAnimationFrame(
        new WeaponAnimationLayer[1] {
            new(3, Sprites.Spr_Wep_PF[0]),
        });
    public static readonly WeaponAnimation Anim_Wep_PF_Atk = new();

    public static readonly WeaponAnimationFrame Frame_Wep_SG_Idle = new WeaponAnimationFrame(
        new WeaponAnimationLayer[1] {
            new(3, Sprites.Spr_Wep_SG[0]),
        });
    public static readonly WeaponAnimation Anim_Wep_SG_Atk = new();
    public static readonly WeaponAnimation Anim_Wep_SG_Alt = new();
    public static readonly WeaponAnimation Anim_Wep_SG_Reload = new();
    public static readonly WeaponAnimation Anim_Wep_SG_FlashLeft = new();
    public static readonly WeaponAnimation Anim_Wep_SG_FlashRight = new();
    public static readonly WeaponAnimation Anim_Wep_SG_FlashBoth = new();

    public static readonly WeaponAnimation Anim_Wep_Light_Up = new();
    public static readonly WeaponAnimation Anim_Wep_Light_Down = new();
    public static readonly WeaponAnimation Anim_Wep_Light_Idle = new();
    public static readonly WeaponAnimation Anim_Wep_Light_Cook = new();
    public static readonly WeaponAnimation Anim_Wep_Light_Cook_Idle = new();
    public static readonly WeaponAnimation Anim_Wep_Light_Recover = new();

    public static readonly WeaponAnimation Anim_Wep_DynaReg_Up = new();
    public static readonly WeaponAnimation Anim_Wep_DynaReg_Down = new();

    public static readonly WeaponAnimationFrame Frame_Wep_DynaReg_Idle = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(0, new(-103, 0, 0, -90), Sprites.Spr_Wep_DynaReg_Fuse),
            new(3, Sprites.Spr_Wep_DynaReg_Idle)
        });
    public static readonly WeaponAnimation Anim_Wep_DynaReg_Cook = new();
    public static readonly WeaponAnimation Anim_Wep_DynaReg_Throw = new();
    public static readonly WeaponAnimation Anim_Wep_DynaReg_Recover = new();

    public static void IndexWeaponAnimations() {
        // ----- Pitchfork -----
        // PF Atk
        Anim_Wep_PF_Atk.AnimFrames = new WeaponAnimationFrame[3];
        Anim_Wep_PF_Atk.AnimFrames[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(3, Sprites.Spr_Wep_PF[1])
        }, 
        3, null, WeaponAction.None);
        Anim_Wep_PF_Atk.AnimFrames[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(3, Sprites.Spr_Wep_PF[2])
        }, 
        13, null, WeaponAction.PF_Stab);
        Anim_Wep_PF_Atk.AnimFrames[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(3, Sprites.Spr_Wep_PF[1])
        }, 
        3, null, WeaponAction.None);
        Anim_Wep_PF_Atk.UpdateAnimationLength();

        // ----- Shotgun -----
        // SG Pri
        Anim_Wep_SG_Atk.AnimFrames = new WeaponAnimationFrame[6];
        Anim_Wep_SG_Atk.AnimFrames[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, Sprites.Spr_Wep_SG[0]),
            new(4, new(0, -7, 0, -52), Sprites.XSpr_Wep_SG[5])
        }, 
        4, null, WeaponAction.None);
        Anim_Wep_SG_Atk.AnimFrames[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, new(0, 5, 0, 12), Sprites.Spr_Wep_SG[0]),
            new(4, new(0, -2, 0, -40), Sprites.XSpr_Wep_SG[5])
        }, 
        2, null, WeaponAction.None);
        Anim_Wep_SG_Atk.AnimFrames[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, new(0, 10, 0, 18), Sprites.Spr_Wep_SG[0]),
            new(4, new(0, 3, 0, -36), Sprites.XSpr_Wep_SG[5])
        }, 
        2, null, WeaponAction.None);
        Anim_Wep_SG_Atk.AnimFrames[3] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, new(0, 10, 0, 25), Sprites.Spr_Wep_SG[0]),
            new(4, new(0, 3, 0, -27), Sprites.XSpr_Wep_SG[5])
        }, 
        2, null, WeaponAction.None);
        Anim_Wep_SG_Atk.AnimFrames[4] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, new(0, 5, 0, 8), Sprites.Spr_Wep_SG[0]),
            new(4, null)
        }, 
        4, null, WeaponAction.None);
        Anim_Wep_SG_Atk.AnimFrames[5] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(3, Sprites.Spr_Wep_SG[0])
        }, 
        4, null, WeaponAction.None);
        Anim_Wep_SG_Atk.UpdateAnimationLength();

        // SG Alt
        Anim_Wep_SG_Alt.AnimFrames = new WeaponAnimationFrame[5];
        Anim_Wep_SG_Alt.AnimFrames[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, Sprites.Spr_Wep_SG[0]),
            new(4, new(0, -7, 0, -52), Sprites.XSpr_Wep_SG[5])
        }, 
        3, null, WeaponAction.None);
        Anim_Wep_SG_Alt.AnimFrames[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, new(0, 5, 0, 25), Sprites.Spr_Wep_SG[0]),
            new(4, new(0, -2, 0, -27), Sprites.XSpr_Wep_SG[5])
        }, 
        2, null, WeaponAction.None);
        Anim_Wep_SG_Alt.AnimFrames[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, new(0, 5, 0, 35), Sprites.Spr_Wep_SG[0]),
            new(4, new(0, -2, 0, -17), Sprites.XSpr_Wep_SG[5])
        }, 
        2, null, WeaponAction.None);
        Anim_Wep_SG_Alt.AnimFrames[3] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, new(0, 10, 0, 45), Sprites.Spr_Wep_SG[0]),
            new(4, new(0, 3, 0, -7), Sprites.XSpr_Wep_SG[5])
        }, 
        2, null, WeaponAction.None);
       Anim_Wep_SG_Alt.AnimFrames[4] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, new(0, 5, 0, 15), Sprites.Spr_Wep_SG[0]),
            new(4, Sprites.Spr_NoOffset, null)
        }, 
        6, null, WeaponAction.None);
        Anim_Wep_SG_Alt.UpdateAnimationLength();

        // SG Reload
        Anim_Wep_SG_Reload.AnimFrames = new WeaponAnimationFrame[6];
        Anim_Wep_SG_Reload.AnimFrames[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(3, Sprites.Spr_Wep_SG[1])
        }, 
        4, null, WeaponAction.None);
        Anim_Wep_SG_Reload.AnimFrames[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, Sprites.Spr_Wep_SG[2]),
            new(5, new(0, 140, 0, 25), 20, Sprites.XSpr_Wep_SG[0], false)
        }, 
        8, null, WeaponAction.None);
        Anim_Wep_SG_Reload.AnimFrames[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, Sprites.Spr_Wep_SG[2]),
            new(5, new(0, 90, 0, 0), Sprites.XSpr_Wep_SG[0])
        }, 
        8, null, WeaponAction.None);
        Anim_Wep_SG_Reload.AnimFrames[3] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, Sprites.Spr_Wep_SG[3]),
            new(5, new(0, 80, 0, 0), Sprites.XSpr_Wep_SG[1])
        }, 
        8, SFX.ShotgunCock, WeaponAction.None);
        Anim_Wep_SG_Reload.AnimFrames[4] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, Sprites.Spr_Wep_SG[4]),
            new(5, Sprites.Spr_NoOffset, null)
        }, 
        8, null, WeaponAction.None);
        Anim_Wep_SG_Reload.AnimFrames[5] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(3, Sprites.Spr_Wep_SG[4])
        }, 
        1, null, WeaponAction.SG_Reload);
        Anim_Wep_SG_Reload.UpdateAnimationLength();

        // SG Muzzleflares
        Anim_Wep_SG_FlashLeft.AnimFrames = new WeaponAnimationFrame[4];
        Anim_Wep_SG_FlashLeft.AnimFrames[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(0, Sprites.Spr_SG_Flash_Offset[0], Sprites.XSpr_Wep_SG[2])
        }, 
        3, null, WeaponAction.None);
        Anim_Wep_SG_FlashLeft.AnimFrames[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(0, Sprites.Spr_SG_Flash_Offset[0], Sprites.XSpr_Wep_SG[3])
        }, 
        3, null, WeaponAction.None);
        Anim_Wep_SG_FlashLeft.AnimFrames[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(0, Sprites.Spr_SG_Flash_Offset[0], Sprites.XSpr_Wep_SG[4])
        }, 
        3, null, WeaponAction.None);
        Anim_Wep_SG_FlashLeft.AnimFrames[3] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(0, null)
        }, 
        0, null, WeaponAction.None);
        Anim_Wep_SG_FlashLeft.UpdateAnimationLength();

        Anim_Wep_SG_FlashRight.AnimFrames = new WeaponAnimationFrame[4];
        Anim_Wep_SG_FlashRight.AnimFrames[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(1, Sprites.Spr_SG_Flash_Offset[1], Sprites.XSpr_Wep_SG[2])
        }, 
        3, null, WeaponAction.None);
        Anim_Wep_SG_FlashRight.AnimFrames[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(1, Sprites.Spr_SG_Flash_Offset[1], Sprites.XSpr_Wep_SG[3])
        }, 
        3, null, WeaponAction.None);
        Anim_Wep_SG_FlashRight.AnimFrames[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(1, Sprites.Spr_SG_Flash_Offset[1], Sprites.XSpr_Wep_SG[4])
        }, 
        3, null, WeaponAction.None);
        Anim_Wep_SG_FlashRight.AnimFrames[3] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(1, Sprites.Spr_NoOffset, null)
        }, 
        0, null, WeaponAction.None);
        Anim_Wep_SG_FlashRight.UpdateAnimationLength();

        Anim_Wep_SG_FlashBoth.AnimFrames = new WeaponAnimationFrame[4];
        Anim_Wep_SG_FlashBoth.AnimFrames[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(0, Sprites.Spr_SG_Flash_Offset[2], Sprites.XSpr_Wep_SG[2])
        }, 
        3, null, WeaponAction.None);
        Anim_Wep_SG_FlashBoth.AnimFrames[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(0, Sprites.Spr_SG_Flash_Offset[2], Sprites.XSpr_Wep_SG[3])
        }, 
        3, null, WeaponAction.None);
        Anim_Wep_SG_FlashBoth.AnimFrames[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(0, Sprites.Spr_SG_Flash_Offset[2], Sprites.XSpr_Wep_SG[4])
        }, 
        3, null, WeaponAction.None);
        Anim_Wep_SG_FlashBoth.AnimFrames[3] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(0, Sprites.Spr_NoOffset, null)
        }, 
        0, null, WeaponAction.None);
        Anim_Wep_SG_FlashBoth.UpdateAnimationLength();

        // CUSTOM UP AND DOWN ANIMS SHOULD LAST 18 FRAMES
        // Lighter
        Anim_Wep_Light_Up.AnimFrames = new WeaponAnimationFrame[5];
        Anim_Wep_Light_Up.AnimFrames[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(2, Sprites.Spr_Light_Offset, Sprites.Spr_Wep_Light_Up[0])
        }, 
        3, null, WeaponAction.None);
        Anim_Wep_Light_Up.AnimFrames[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(2, Sprites.Spr_Light_Offset, Sprites.Spr_Wep_Light_Up[1])
        }, 
        6, null, WeaponAction.None);
        Anim_Wep_Light_Up.AnimFrames[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(2, Sprites.Spr_Light_Offset, Sprites.Spr_Wep_Light_Up[2]),
            new(5, new(-146, 0, 0, -96), Sprites.Spr_Wep_Light_Strike)
        }, 
        5, null, WeaponAction.None);
        Anim_Wep_Light_Up.AnimFrames[3] = new WeaponAnimationFrame(new WeaponAnimationLayer[3] {
            new(1, Sprites.Spr_Light_Flame_Offset, Sprites.Spr_Wep_Light_Flame[0]),
            new(2, Sprites.Spr_Light_Offset, Sprites.Spr_Wep_Light_Idle),
            new(5, Sprites.Spr_NoOffset, null)
        }, 
        2, null, WeaponAction.None);
        Anim_Wep_Light_Up.AnimFrames[4] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(1, Sprites.Spr_Light_Flame_Offset, Sprites.Spr_Wep_Light_Flame[1])
        }, 
        2, null, WeaponAction.None);
        Anim_Wep_Light_Up.UpdateAnimationLength();

        Anim_Wep_Light_Down.AnimFrames = new WeaponAnimationFrame[4];
        Anim_Wep_Light_Down.AnimFrames[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(1, null),
            new(2, Sprites.Spr_Light_Offset, Sprites.Spr_Wep_Light_Down[0])
        }, 
        6, null, WeaponAction.None);
        Anim_Wep_Light_Down.AnimFrames[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(1, null),
            new(2, Sprites.Spr_Light_Offset, Sprites.Spr_Wep_Light_Down[1])
        }, 
        6, null, WeaponAction.None);
        Anim_Wep_Light_Down.AnimFrames[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(1, null),
            new(2, Sprites.Spr_Light_Offset, Sprites.Spr_Wep_Light_Down[2])
        }, 
        6, null, WeaponAction.None);
        Anim_Wep_Light_Down.AnimFrames[3] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(1, Sprites.Spr_NoOffset, null),
            new(2, Sprites.Spr_NoOffset, null)
        }, 
        0, null, WeaponAction.None);
        Anim_Wep_Light_Down.UpdateAnimationLength();

        Anim_Wep_Light_Idle.AnimFrames = new WeaponAnimationFrame[3];
        Anim_Wep_Light_Idle.AnimFrames[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(1, Sprites.Spr_Light_Flame_Offset, Sprites.Spr_Wep_Light_Flame[2]),
            new(2, Sprites.Spr_Light_Offset, Sprites.Spr_Wep_Light_Idle)
        }, 
        5, null, WeaponAction.None);
        Anim_Wep_Light_Idle.AnimFrames[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(1, Sprites.Spr_Light_Flame_Offset, Sprites.Spr_Wep_Light_Flame[3])
        }, 
        5, null, WeaponAction.None);
        Anim_Wep_Light_Idle.AnimFrames[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(1, Sprites.Spr_Light_Flame_Offset, Sprites.Spr_Wep_Light_Flame[4])
        }, 
        5, null, WeaponAction.None);
        Anim_Wep_Light_Idle.UpdateAnimationLength();

        Anim_Wep_Light_Cook.AnimFrames = new WeaponAnimationFrame[5];
        Anim_Wep_Light_Cook.AnimFrames[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[0] {
        }, 
        7, null, WeaponAction.None);
        Anim_Wep_Light_Cook.AnimFrames[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(1, new(-185, 0, 0, -88), Sprites.Spr_Wep_Light_Flame[2]),
            new(2, new(-125, -125, 0, 5), Sprites.Spr_Wep_Light_Idle)
        }, 
        4, null, WeaponAction.None);
        Anim_Wep_Light_Cook.AnimFrames[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(1, new(-205, 0, 0, -78), Sprites.Spr_Wep_Light_Flame[3]),
            new(2, new(-135, -135, 0, 15), Sprites.Spr_Wep_Light_Idle)
        }, 
        4, null, WeaponAction.None);
        Anim_Wep_Light_Cook.AnimFrames[3] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(1, new(-225, 0, 0, -68), Sprites.Spr_Wep_Light_Flame[4]),
            new(2, new(-145, -145, 0, 25), Sprites.Spr_Wep_Light_Idle)
        }, 
        4, null, WeaponAction.None);
        Anim_Wep_Light_Cook.AnimFrames[4] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(1, new(-245, 0, 0, -58), Sprites.Spr_Wep_Light_Flame[4]),
            new(2, new(-155, -155, 0, 35), Sprites.Spr_Wep_Light_Idle)
        }, 
        0, null, WeaponAction.None);
        Anim_Wep_Light_Cook.UpdateAnimationLength();

        Anim_Wep_Light_Cook_Idle.AnimFrames = new WeaponAnimationFrame[3];
        Anim_Wep_Light_Cook_Idle.AnimFrames[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(1, new(-245, 0, 0, -58), Sprites.Spr_Wep_Light_Flame[2]),
        }, 
        5, null, WeaponAction.None);
        Anim_Wep_Light_Cook_Idle.AnimFrames[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(1, new(-245, 0, 0, -58), Sprites.Spr_Wep_Light_Flame[3])
        }, 
        5, null, WeaponAction.None);
        Anim_Wep_Light_Cook_Idle.AnimFrames[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(1, new(-245, 0, 0, -58), Sprites.Spr_Wep_Light_Flame[4])
        }, 
        5, null, WeaponAction.None);
        Anim_Wep_Light_Cook_Idle.UpdateAnimationLength();

        // Dynamite Regular
        Anim_Wep_DynaReg_Up.AnimFrames = new WeaponAnimationFrame[3];
        Anim_Wep_DynaReg_Up.AnimFrames[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(0, new(-103, 0, 0, -90), Sprites.Spr_Wep_DynaReg_Fuse),
            new(3, Sprites.Spr_Wep_DynaReg_Idle)
        }, 
        6, null, WeaponAction.None);
        Anim_Wep_DynaReg_Up.AnimFrames[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(0, new(-103, 0, 0, -90), Sprites.Spr_Wep_DynaReg_Fuse),
            new(3, Sprites.Spr_Wep_DynaReg_Idle)
        }, 
        6, null, WeaponAction.None);
        Anim_Wep_DynaReg_Up.AnimFrames[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(0, new(-103, 0, 0, -90), Sprites.Spr_Wep_DynaReg_Fuse),
            new(3, Sprites.Spr_Wep_DynaReg_Idle)
        }, 
        6, null, WeaponAction.None);
        Anim_Wep_DynaReg_Up.UpdateAnimationLength();

        Anim_Wep_DynaReg_Down.AnimFrames = new WeaponAnimationFrame[4];
        Anim_Wep_DynaReg_Down.AnimFrames[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(0, new(-103, 0, 0, -90), Sprites.Spr_Wep_DynaReg_Fuse),
            new(3, Sprites.Spr_Wep_DynaReg_Idle)
        }, 
        6, null, WeaponAction.None);
        Anim_Wep_DynaReg_Down.AnimFrames[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(0, new(-103, 0, 0, -90), Sprites.Spr_Wep_DynaReg_Fuse),
            new(3, Sprites.Spr_Wep_DynaReg_Idle)
        }, 
        6, null, WeaponAction.None);
        Anim_Wep_DynaReg_Down.AnimFrames[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(0, new(-103, 0, 0, -90), Sprites.Spr_Wep_DynaReg_Fuse),
            new(3, Sprites.Spr_Wep_DynaReg_Idle)
        }, 
        6, null, WeaponAction.None);
        Anim_Wep_DynaReg_Down.AnimFrames[3] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(0, Sprites.Spr_NoOffset, null),
            new(3, Sprites.Spr_NoOffset, null)
        }, 
        0, null, WeaponAction.None);
        Anim_Wep_DynaReg_Down.UpdateAnimationLength();

        Anim_Wep_DynaReg_Cook.AnimFrames = new WeaponAnimationFrame[5];
        Anim_Wep_DynaReg_Cook.AnimFrames[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(0, new(-133, 0, 0, -90), Sprites.Spr_Wep_DynaReg_Fuse),
            new(3, new(-30, 0, 0, 0), Sprites.Spr_Wep_DynaReg_Idle)
        }, 
        7, null, WeaponAction.None);
        Anim_Wep_DynaReg_Cook.AnimFrames[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(0, new(-113, 0, 0, -80), Sprites.Spr_Wep_DynaReg_Fuse),
            new(3, new(-10, 0, 0, 10), Sprites.Spr_Wep_DynaReg_Idle)
        }, 
        4, null, WeaponAction.None);
        Anim_Wep_DynaReg_Cook.AnimFrames[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(0, new(-93, 0, 0, -70), Sprites.Spr_Wep_DynaReg_Fuse),
            new(3, new(10, 0, 0, 20), Sprites.Spr_Wep_DynaReg_Idle)
        }, 
        4, null, WeaponAction.None);
        Anim_Wep_DynaReg_Cook.AnimFrames[3] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(0, new(-73, 0, 0, -60), Sprites.Spr_Wep_DynaReg_Fuse),
            new(3, new(30, 0, 0, 30), Sprites.Spr_Wep_DynaReg_Idle)
        }, 
        4, null, WeaponAction.None);
        Anim_Wep_DynaReg_Cook.AnimFrames[4] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(0, new(-53, 0, 0, -50), Sprites.Spr_Wep_DynaReg_Fuse),
            new(3, new(50, 0, 0, 40), Sprites.Spr_Wep_DynaReg_Idle)
        }, 
        231, null, WeaponAction.None);
        Anim_Wep_DynaReg_Cook.UpdateAnimationLength();

        Anim_Wep_DynaReg_Throw.AnimFrames = new WeaponAnimationFrame[5];
        Anim_Wep_DynaReg_Throw.AnimFrames[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(0, Sprites.Spr_NoOffset, null),
            new(3, new(50, 0, 0, 40), Sprites.Spr_Wep_DynaReg_Throw[0])
        }, 
        4, null, WeaponAction.None);
        Anim_Wep_DynaReg_Throw.AnimFrames[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(3, new(45, 0, 0, 20), Sprites.Spr_Wep_DynaReg_Throw[0])
        }, 
        3, null, WeaponAction.None);
        Anim_Wep_DynaReg_Throw.AnimFrames[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(3, new(41, 0, 0, 10), Sprites.Spr_Wep_DynaReg_Throw[1])
        }, 
        2, null, WeaponAction.None);
        Anim_Wep_DynaReg_Throw.AnimFrames[3] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(3, new(38, 0, 0, -5), Sprites.Spr_Wep_DynaReg_Throw[1])
        }, 
        1, null, WeaponAction.None);
        Anim_Wep_DynaReg_Throw.AnimFrames[4] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(3, new(35, 0, 0, -15), Sprites.Spr_Wep_DynaReg_Throw[1])
        }, 
        0, null, WeaponAction.Dyn_Throw);
        Anim_Wep_DynaReg_Throw.UpdateAnimationLength();
    }
}
