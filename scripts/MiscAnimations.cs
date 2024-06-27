using Godot;

public readonly struct MiscAnimationFrame {
    public readonly Texture2D Texture;
    public readonly int FrameLength;
    public readonly Vector2 Offset;

    public MiscAnimationFrame(Texture2D texture, int length, Vector2 offset) {
        Texture = texture;
        FrameLength = length;
        Offset = offset;
    }
}

public class MiscAnimation {
    public MiscAnimationFrame[] AnimFrames { get; set; }
    public string Name { get; set; }
    public int TotalLength { get; set; }

    public void UpdateAnimationLength() {
        for (int i = 0; i < AnimFrames.Length; i++) {
            TotalLength += AnimFrames[i].FrameLength;
        }
    }
}

public static class MiscAnimations {
    public static readonly MiscAnimation Anim_Misc_Hitspark_Metal = new();
    public static readonly MiscAnimation Anim_Misc_Hitspark_Wood = new();
    public static readonly MiscAnimation Anim_Misc_Hitspark_Flesh = new();

    public static void IndexMiscAnimations() {
        Anim_Misc_Hitspark_Metal.Name = "Misc_Hitspark_Metal";
        Anim_Misc_Hitspark_Metal.AnimFrames = new MiscAnimationFrame[6];
        Anim_Misc_Hitspark_Metal.AnimFrames[0] = new(Sprites.Spr_FX_Hitspark_Metal[0], 3, new(0, 0));
        Anim_Misc_Hitspark_Metal.AnimFrames[1] = new(Sprites.Spr_FX_Hitspark_Metal[1], 3, new(0, 0));
        Anim_Misc_Hitspark_Metal.AnimFrames[2] = new(Sprites.Spr_FX_Hitspark_Metal[2], 3, new(-2, 0));
        Anim_Misc_Hitspark_Metal.AnimFrames[3] = new(Sprites.Spr_FX_Hitspark_Metal[3], 3, new(-3.5f, 0));
        Anim_Misc_Hitspark_Metal.AnimFrames[4] = new(Sprites.Spr_FX_Hitspark_Metal[4], 3, new(-8, -4.5f));
        Anim_Misc_Hitspark_Metal.AnimFrames[5] = new(Sprites.Spr_FX_Hitspark_Metal[5], 3, new(0, 0));
        Anim_Misc_Hitspark_Metal.UpdateAnimationLength();

        Anim_Misc_Hitspark_Wood.Name = "Misc_Hitspark_Wood";
        Anim_Misc_Hitspark_Wood.AnimFrames = new MiscAnimationFrame[5];
        Anim_Misc_Hitspark_Wood.AnimFrames[0] = new(Sprites.Spr_FX_Hitspark_Wood[0], 3, new(0, 0));
        Anim_Misc_Hitspark_Wood.AnimFrames[1] = new(Sprites.Spr_FX_Hitspark_Wood[1], 3, new(0, 0));
        Anim_Misc_Hitspark_Wood.AnimFrames[2] = new(Sprites.Spr_FX_Hitspark_Wood[2], 3, new(0, -1));
        Anim_Misc_Hitspark_Wood.AnimFrames[3] = new(Sprites.Spr_FX_Hitspark_Wood[3], 3, new(0, -2));
        Anim_Misc_Hitspark_Wood.AnimFrames[4] = new(Sprites.Spr_FX_Hitspark_Wood[4], 3, new(2, -7));
        Anim_Misc_Hitspark_Wood.UpdateAnimationLength();

        Anim_Misc_Hitspark_Flesh.Name = "Misc_Hitspark_Flesh";
        Anim_Misc_Hitspark_Flesh.AnimFrames = new MiscAnimationFrame[5];
        Anim_Misc_Hitspark_Flesh.AnimFrames[0] = new(Sprites.Spr_FX_Hitspark_Flesh[0], 3, new(0, 0));
        Anim_Misc_Hitspark_Flesh.AnimFrames[1] = new(Sprites.Spr_FX_Hitspark_Flesh[1], 3, new(0, 0));
        Anim_Misc_Hitspark_Flesh.AnimFrames[2] = new(Sprites.Spr_FX_Hitspark_Flesh[2], 3, new(0, 0));
        Anim_Misc_Hitspark_Flesh.AnimFrames[3] = new(Sprites.Spr_FX_Hitspark_Flesh[3], 3, new(0, 0));
        Anim_Misc_Hitspark_Flesh.AnimFrames[4] = new(Sprites.Spr_FX_Hitspark_Flesh[4], 3, new(0, -12));
        Anim_Misc_Hitspark_Flesh.UpdateAnimationLength();
    }
}
