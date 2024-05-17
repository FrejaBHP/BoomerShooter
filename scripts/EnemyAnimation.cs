using Godot;

public partial class EnemyAnimationFrame {
    public int TextureFrame { get; private set; }
    public int FrameLength { get; set; }
    public AudioStreamWav? Sound { get; set; }

    public EnemyAnimationFrame(int textureframe, int length, AudioStreamWav? sound) {
        TextureFrame = textureframe;
        FrameLength = length;
        Sound = sound;
    }
}

public partial class EnemyAnimation {
    public EnemyAnimationFrame[] AnimFrames { get; set; }
    public int TotalLength { get; set; }

    public void UpdateAnimationLength() {
        for (int i = 0; i < AnimFrames.Length; i++) {
            TotalLength += AnimFrames[i].FrameLength;
        }
    }
}

public static class EAnimations {
    public static EnemyAnimation Anim_Enemy_AZ_Walk = new();

    public static void IndexEnemyAnimations() {
        Anim_Enemy_AZ_Walk.AnimFrames = new EnemyAnimationFrame[6];
        Anim_Enemy_AZ_Walk.AnimFrames[0] = new(0, 8, null);
        Anim_Enemy_AZ_Walk.AnimFrames[1] = new(1, 8, null);
        Anim_Enemy_AZ_Walk.AnimFrames[2] = new(2, 8, null);
        Anim_Enemy_AZ_Walk.AnimFrames[3] = new(3, 8, null);
        Anim_Enemy_AZ_Walk.AnimFrames[4] = new(4, 8, null);
        Anim_Enemy_AZ_Walk.AnimFrames[5] = new(5, 8, null);
        Anim_Enemy_AZ_Walk.UpdateAnimationLength();
    }
}