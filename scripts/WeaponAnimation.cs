using Godot;

public partial class WeaponAnimationLayer {
    public int Layer { get; private set; }
    public SpriteOffset Offset { get; private set; }
    public int RotationDeg { get; private set; }
    public Texture2D? Texture { get; private set; }

    public WeaponAnimationLayer(int layer, Texture2D? texture) {
        Layer = layer;
        Offset = WeaponSprites.Spr_NoOffset;
        RotationDeg = 0;
        Texture = texture;
    }

    public WeaponAnimationLayer(int layer, SpriteOffset offset, Texture2D? texture) {
        Layer = layer;
        Offset = offset;
        RotationDeg = 0;
        Texture = texture;
    }

    public WeaponAnimationLayer(int layer, SpriteOffset offset, int rotation, Texture2D? texture) {
        Layer = layer;
        Offset = offset;
        RotationDeg = rotation;
        Texture = texture;
    }
}

public partial class WeaponAnimationFrame {
    public WeaponAnimationLayer[] WeaponAnimationLayers { get; set; }
    public int FrameLength { get; set; }
    public WeaponAnimationFrame(WeaponAnimationLayer[] Layers) {
        WeaponAnimationLayers = Layers;
    }

    public WeaponAnimationFrame(WeaponAnimationLayer[] Layers, int length) {
        WeaponAnimationLayers = Layers;
        FrameLength = length;
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

public static class WAnimCollection {
    public static WeaponAnimationFrame[] Frame_Wep_PF = new WeaponAnimationFrame[3];
    public static WeaponAnimationFrame[] Anim_Wep_SG = new WeaponAnimationFrame[7];

    public static WeaponAnimation Anim_Wep_SG_Atk = new WeaponAnimation();
    public static WeaponAnimation Anim_Wep_SG_Alt = new WeaponAnimation();
    public static WeaponAnimation Anim_Wep_SG_FlashLeft = new WeaponAnimation();
    public static WeaponAnimation Anim_Wep_SG_FlashRight = new WeaponAnimation();
    public static WeaponAnimation Anim_Wep_SG_FlashBoth = new WeaponAnimation();


    public static void PlayAnimationFrame(bsPlayer player, WeaponAnimationFrame frame) {
        
    }

    public static void IndexAnimations() {
        // SG Atk
        Anim_Wep_SG_Atk.AnimFrames = new WeaponAnimationFrame[4];
        Anim_Wep_SG_Atk.AnimFrames[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, WeaponSprites.Spr_NoOffset, WeaponSprites.Spr_Wep_SG[0]),
            new(4, new(0, -7, 0, -52), WeaponSprites.XSpr_Wep_SG[5])
        }, 3);
        Anim_Wep_SG_Atk.AnimFrames[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, new(0, 5, 0, 12), WeaponSprites.Spr_Wep_SG[0]),
            new(4, new(0, -2, 0, -40), WeaponSprites.XSpr_Wep_SG[5])
        }, 3);
        Anim_Wep_SG_Atk.AnimFrames[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, new(0, 10, 0, 20), WeaponSprites.Spr_Wep_SG[0]),
            new(4, new(0, 3, 0, -32), WeaponSprites.XSpr_Wep_SG[5])
        }, 3);
       Anim_Wep_SG_Atk.AnimFrames[3] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, new(0, 5, 0, 8), WeaponSprites.Spr_Wep_SG[0]),
            new(4, WeaponSprites.Spr_NoOffset, null)
        }, 3);
        Anim_Wep_SG_Atk.UpdateAnimationLength();

        // SG Alt
        Anim_Wep_SG_Alt.AnimFrames = new WeaponAnimationFrame[4];
        Anim_Wep_SG_Alt.AnimFrames[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, WeaponSprites.Spr_Wep_SG[0]),
            new(4, new(0, -7, 0, -52), WeaponSprites.XSpr_Wep_SG[5])
        }, 3);
        Anim_Wep_SG_Alt.AnimFrames[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, new(0, 5, 0, 20), WeaponSprites.Spr_Wep_SG[0]),
            new(4, new(0, -2, 0, -32), WeaponSprites.XSpr_Wep_SG[5])
        }, 3);
        Anim_Wep_SG_Alt.AnimFrames[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, new(0, 10, 0, 35), WeaponSprites.Spr_Wep_SG[0]),
            new(4, new(0, 3, 0, -17), WeaponSprites.XSpr_Wep_SG[5])
        }, 3);
       Anim_Wep_SG_Alt.AnimFrames[3] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, new(0, 5, 0, 15), WeaponSprites.Spr_Wep_SG[0]),
            new(4, WeaponSprites.Spr_NoOffset, null)
        }, 3);
        Anim_Wep_SG_Alt.UpdateAnimationLength();

        Anim_Wep_SG_FlashLeft.AnimFrames = new WeaponAnimationFrame[4];
        Anim_Wep_SG_FlashLeft.AnimFrames[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(0, WeaponSprites.Spr_SG_Flash_Offset[0], WeaponSprites.XSpr_Wep_SG[2])
        }, 3);
        Anim_Wep_SG_FlashLeft.AnimFrames[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(0, WeaponSprites.Spr_SG_Flash_Offset[0], WeaponSprites.XSpr_Wep_SG[3])
        }, 3);
        Anim_Wep_SG_FlashLeft.AnimFrames[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(0, WeaponSprites.Spr_SG_Flash_Offset[0], WeaponSprites.XSpr_Wep_SG[4])
        }, 3);
        Anim_Wep_SG_FlashLeft.AnimFrames[3] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(0, null)
        }, 0);
        Anim_Wep_SG_FlashLeft.UpdateAnimationLength();

        Anim_Wep_SG_FlashRight.AnimFrames = new WeaponAnimationFrame[4];
        Anim_Wep_SG_FlashRight.AnimFrames[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(1, WeaponSprites.Spr_SG_Flash_Offset[1], WeaponSprites.XSpr_Wep_SG[2])
        }, 3);
        Anim_Wep_SG_FlashRight.AnimFrames[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(1, WeaponSprites.Spr_SG_Flash_Offset[1], WeaponSprites.XSpr_Wep_SG[3])
        }, 3);
        Anim_Wep_SG_FlashRight.AnimFrames[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(1, WeaponSprites.Spr_SG_Flash_Offset[1], WeaponSprites.XSpr_Wep_SG[4])
        }, 3);
        Anim_Wep_SG_FlashRight.AnimFrames[3] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(1, null)
        }, 0);
        Anim_Wep_SG_FlashRight.UpdateAnimationLength();

        Anim_Wep_SG_FlashBoth.AnimFrames = new WeaponAnimationFrame[4];
        Anim_Wep_SG_FlashBoth.AnimFrames[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(0, WeaponSprites.Spr_SG_Flash_Offset[2], WeaponSprites.XSpr_Wep_SG[2])
        }, 3);
        Anim_Wep_SG_FlashBoth.AnimFrames[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(0, WeaponSprites.Spr_SG_Flash_Offset[2], WeaponSprites.XSpr_Wep_SG[3])
        }, 3);
        Anim_Wep_SG_FlashBoth.AnimFrames[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(0, WeaponSprites.Spr_SG_Flash_Offset[2], WeaponSprites.XSpr_Wep_SG[4])
        }, 3);
        Anim_Wep_SG_FlashBoth.AnimFrames[3] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(0, null)
        }, 0);
        Anim_Wep_SG_FlashBoth.UpdateAnimationLength();


        // PF Idle
        Frame_Wep_PF[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(3, new(0, 0, 0, 0), WeaponSprites.Spr_Wep_PF[0])
        });
        // PF Stab
        Frame_Wep_PF[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(3, new(0, 0, 0, 0), WeaponSprites.Spr_Wep_PF[1])
        });
        Frame_Wep_PF[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(3, new(0, 0, 0, 0), WeaponSprites.Spr_Wep_PF[2])
        });

        // SG Idle
        Anim_Wep_SG[0] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, new(0, 0, 0, 0), WeaponSprites.Spr_Wep_SG[0]),
            new(4, new(0, 0, 0, 0), null)
        });
        // SG Fire
        Anim_Wep_SG[1] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, new(0, 5, 0, 10), WeaponSprites.Spr_Wep_SG[0]),
            new(4, new(0, -2, 0, -22), null)
        });
        Anim_Wep_SG[2] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, new(0, 10, 0, 20), WeaponSprites.Spr_Wep_SG[0]),
            new(4, new(0, 3, 0, -32), WeaponSprites.XSpr_Wep_SG[5])
        });
        //SG Reload
        Anim_Wep_SG[3] = new WeaponAnimationFrame(new WeaponAnimationLayer[1] {
            new(3, new(0, 0, 0, 0), WeaponSprites.Spr_Wep_SG[1])
        });
        Anim_Wep_SG[4] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, new(0, 0, 0, 0), WeaponSprites.Spr_Wep_SG[2]),
            new(5, new(0, 90, 0, 0), WeaponSprites.XSpr_Wep_SG[0])
        });
        Anim_Wep_SG[5] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, new(0, 0, 0, 0), WeaponSprites.Spr_Wep_SG[3]),
            new(5, new(0, 80, 0, 0), WeaponSprites.XSpr_Wep_SG[1])
        });
        Anim_Wep_SG[6] = new WeaponAnimationFrame(new WeaponAnimationLayer[2] {
            new(3, new(0, 0, 0, 0), WeaponSprites.Spr_Wep_SG[4]),
            new(5, new(0, 0, 0, 0), null)
        });
    }
}
