using Godot;

public readonly struct WeaponAnimationLayer {
    public readonly int Layer ;
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
    public AudioStreamWav? Sound { get; set; }

    public WeaponAnimationFrame(WeaponAnimationLayer[] Layers) {
        WeaponAnimationLayers = Layers;
        //Sound = null;
    }

    public WeaponAnimationFrame(WeaponAnimationLayer[] Layers, AudioStreamWav? sound) {
        WeaponAnimationLayers = Layers;
        Sound = sound;
    }

    public WeaponAnimationFrame(WeaponAnimationLayer[] Layers, int length) {
        WeaponAnimationLayers = Layers;
        FrameLength = length;
        //Sound = null;
    }

    public WeaponAnimationFrame(WeaponAnimationLayer[] Layers, int length, AudioStreamWav? sound) {
        WeaponAnimationLayers = Layers;
        FrameLength = length;
        Sound = sound;
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
    public static readonly WeaponAnimationFrame[] Frame_Wep_PF = new WeaponAnimationFrame[3];
    public static readonly WeaponAnimationFrame[] Anim_Wep_SG = new WeaponAnimationFrame[7];

    public static readonly WeaponAnimation Anim_Wep_SG_Atk = new WeaponAnimation();
    public static readonly WeaponAnimation Anim_Wep_SG_Alt = new WeaponAnimation();
    public static readonly WeaponAnimation Anim_Wep_SG_Reload = new WeaponAnimation();
    public static readonly WeaponAnimation Anim_Wep_SG_FlashLeft = new WeaponAnimation();
    public static readonly WeaponAnimation Anim_Wep_SG_FlashRight = new WeaponAnimation();
    public static readonly WeaponAnimation Anim_Wep_SG_FlashBoth = new WeaponAnimation();


    public static void PlayAnimationFrame(bsPlayer player, WeaponAnimationFrame frame) {
        
    }

    public static void IndexWeaponAnimations() {
        // ----- Shotgun -----
        // SG Pri
        Anim_Wep_SG_Atk.AnimFrames = new WeaponAnimationFrame[6];
        Anim_Wep_SG_Atk.AnimFrames[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, Sprites.Spr_Wep_SG[0]),
            new(4, new(0, -7, 0, -52), Sprites.XSpr_Wep_SG[5])
        }, 4);
        Anim_Wep_SG_Atk.AnimFrames[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, new(0, 5, 0, 12), Sprites.Spr_Wep_SG[0]),
            new(4, new(0, -2, 0, -40), Sprites.XSpr_Wep_SG[5])
        }, 2);
        Anim_Wep_SG_Atk.AnimFrames[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, new(0, 10, 0, 18), Sprites.Spr_Wep_SG[0]),
            new(4, new(0, 3, 0, -36), Sprites.XSpr_Wep_SG[5])
        }, 2);
        Anim_Wep_SG_Atk.AnimFrames[3] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, new(0, 10, 0, 25), Sprites.Spr_Wep_SG[0]),
            new(4, new(0, 3, 0, -27), Sprites.XSpr_Wep_SG[5])
        }, 2);
        Anim_Wep_SG_Atk.AnimFrames[4] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, new(0, 5, 0, 8), Sprites.Spr_Wep_SG[0]),
            new(4, null)
        }, 4);
        Anim_Wep_SG_Atk.AnimFrames[5] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(3, Sprites.Spr_Wep_SG[0])
        }, 4);
        Anim_Wep_SG_Atk.UpdateAnimationLength();

        // SG Alt
        Anim_Wep_SG_Alt.AnimFrames = new WeaponAnimationFrame[5];
        Anim_Wep_SG_Alt.AnimFrames[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, Sprites.Spr_Wep_SG[0]),
            new(4, new(0, -7, 0, -52), Sprites.XSpr_Wep_SG[5])
        }, 3);
        Anim_Wep_SG_Alt.AnimFrames[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, new(0, 5, 0, 25), Sprites.Spr_Wep_SG[0]),
            new(4, new(0, -2, 0, -27), Sprites.XSpr_Wep_SG[5])
        }, 2);
        Anim_Wep_SG_Alt.AnimFrames[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, new(0, 5, 0, 35), Sprites.Spr_Wep_SG[0]),
            new(4, new(0, -2, 0, -17), Sprites.XSpr_Wep_SG[5])
        }, 2);
        Anim_Wep_SG_Alt.AnimFrames[3] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, new(0, 10, 0, 45), Sprites.Spr_Wep_SG[0]),
            new(4, new(0, 3, 0, -7), Sprites.XSpr_Wep_SG[5])
        }, 2);
       Anim_Wep_SG_Alt.AnimFrames[4] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, new(0, 5, 0, 15), Sprites.Spr_Wep_SG[0]),
            new(4, Sprites.Spr_NoOffset, null)
        }, 6);
        Anim_Wep_SG_Alt.UpdateAnimationLength();

        // SG Reload
        Anim_Wep_SG_Reload.AnimFrames = new WeaponAnimationFrame[5];
        Anim_Wep_SG_Reload.AnimFrames[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(3, Sprites.Spr_Wep_SG[1])
        }, 4);
        Anim_Wep_SG_Reload.AnimFrames[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, Sprites.Spr_Wep_SG[2]),
            new(5, new(0, 140, 0, 25), 20, Sprites.XSpr_Wep_SG[0], false)
        }, 8);
        Anim_Wep_SG_Reload.AnimFrames[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, Sprites.Spr_Wep_SG[2]),
            new(5, new(0, 90, 0, 0), Sprites.XSpr_Wep_SG[0])
        }, 8);
        Anim_Wep_SG_Reload.AnimFrames[3] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, Sprites.Spr_Wep_SG[3]),
            new(5, new(0, 80, 0, 0), Sprites.XSpr_Wep_SG[1])
        }, 8, SFX.ShotgunCock);
        Anim_Wep_SG_Reload.AnimFrames[4] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, Sprites.Spr_Wep_SG[4]),
            new(5, null)
        }, 8);
        Anim_Wep_SG_Reload.UpdateAnimationLength();

        // SG Muzzleflares
        Anim_Wep_SG_FlashLeft.AnimFrames = new WeaponAnimationFrame[4];
        Anim_Wep_SG_FlashLeft.AnimFrames[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(0, Sprites.Spr_SG_Flash_Offset[0], Sprites.XSpr_Wep_SG[2])
        }, 3);
        Anim_Wep_SG_FlashLeft.AnimFrames[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(0, Sprites.Spr_SG_Flash_Offset[0], Sprites.XSpr_Wep_SG[3])
        }, 3);
        Anim_Wep_SG_FlashLeft.AnimFrames[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(0, Sprites.Spr_SG_Flash_Offset[0], Sprites.XSpr_Wep_SG[4])
        }, 3);
        Anim_Wep_SG_FlashLeft.AnimFrames[3] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(0, null)
        }, 0);
        Anim_Wep_SG_FlashLeft.UpdateAnimationLength();

        Anim_Wep_SG_FlashRight.AnimFrames = new WeaponAnimationFrame[4];
        Anim_Wep_SG_FlashRight.AnimFrames[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(1, Sprites.Spr_SG_Flash_Offset[1], Sprites.XSpr_Wep_SG[2])
        }, 3);
        Anim_Wep_SG_FlashRight.AnimFrames[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(1, Sprites.Spr_SG_Flash_Offset[1], Sprites.XSpr_Wep_SG[3])
        }, 3);
        Anim_Wep_SG_FlashRight.AnimFrames[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(1, Sprites.Spr_SG_Flash_Offset[1], Sprites.XSpr_Wep_SG[4])
        }, 3);
        Anim_Wep_SG_FlashRight.AnimFrames[3] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(1, null)
        }, 0);
        Anim_Wep_SG_FlashRight.UpdateAnimationLength();

        Anim_Wep_SG_FlashBoth.AnimFrames = new WeaponAnimationFrame[4];
        Anim_Wep_SG_FlashBoth.AnimFrames[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(0, Sprites.Spr_SG_Flash_Offset[2], Sprites.XSpr_Wep_SG[2])
        }, 3);
        Anim_Wep_SG_FlashBoth.AnimFrames[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(0, Sprites.Spr_SG_Flash_Offset[2], Sprites.XSpr_Wep_SG[3])
        }, 3);
        Anim_Wep_SG_FlashBoth.AnimFrames[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(0, Sprites.Spr_SG_Flash_Offset[2], Sprites.XSpr_Wep_SG[4])
        }, 3);
        Anim_Wep_SG_FlashBoth.AnimFrames[3] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(0, null)
        }, 0);
        Anim_Wep_SG_FlashBoth.UpdateAnimationLength();



        // PF Idle
        Frame_Wep_PF[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(3, Sprites.Spr_Wep_PF[0])
        });
        // PF Stab
        Frame_Wep_PF[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(3, Sprites.Spr_Wep_PF[1])
        });
        Frame_Wep_PF[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(3, Sprites.Spr_Wep_PF[2])
        });

        // SG Idle
        Anim_Wep_SG[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, Sprites.Spr_Wep_SG[0]),
            new(4, null)
        });
        // SG Fire
        Anim_Wep_SG[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, new(0, 5, 0, 10), Sprites.Spr_Wep_SG[0]),
            new(4, new(0, -2, 0, -22), null)
        });
        Anim_Wep_SG[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, new(0, 10, 0, 20), Sprites.Spr_Wep_SG[0]),
            new(4, new(0, 3, 0, -32), Sprites.XSpr_Wep_SG[5])
        });

        //SG Reload
        Anim_Wep_SG[3] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(3, Sprites.Spr_Wep_SG[1])
        });
        Anim_Wep_SG[4] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, Sprites.Spr_Wep_SG[2]),
            new(5, new(0, 90, 0, 0), Sprites.XSpr_Wep_SG[0])
        });
        Anim_Wep_SG[5] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, Sprites.Spr_Wep_SG[3]),
            new(5, new(0, 80, 0, 0), Sprites.XSpr_Wep_SG[1])
        }, SFX.ShotgunCock);
        Anim_Wep_SG[6] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, Sprites.Spr_Wep_SG[4]),
            new(5, null)
        });
    }
}
