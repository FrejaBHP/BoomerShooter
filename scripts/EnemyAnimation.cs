using System;
using Godot;

public class EnemyAnimationFrame {
    public readonly int TextureFrame;
    public readonly int FrameLength;
    public readonly AudioStreamWav? Sound;
    public readonly EnemyAction Action;

    public EnemyAnimationFrame(int textureframe, int length, AudioStreamWav? sound, EnemyAction action) {
        TextureFrame = textureframe;
        FrameLength = length;
        Sound = sound;
        Action = action;
    }
}


public partial class EnemyAnimation {
    public EnemyAnimationFrame[] AnimFrames { get; set; }
    public int TotalLength { get; private set; }

    public void UpdateAnimationLength() {
        for (int i = 0; i < AnimFrames.Length; i++) {
            TotalLength += AnimFrames[i].FrameLength;
        }
    }
}

public static class EAnimations {
    public static readonly EnemyAnimation Anim_Enemy_AZ_Walk = new();
    public static readonly EnemyAnimation Anim_Enemy_AZ_Die = new();
    public static readonly EnemyAnimation Anim_Enemy_AZ_Attack = new();
    public static readonly EnemyAnimation Anim_Enemy_AZ_Stun = new();

    public static void IndexEnemyAnimations() {
        Anim_Enemy_AZ_Walk.AnimFrames = new EnemyAnimationFrame[6];
        Anim_Enemy_AZ_Walk.AnimFrames[0] = new(0, 8, null, EnemyAction.None);
        Anim_Enemy_AZ_Walk.AnimFrames[1] = new(1, 8, null, EnemyAction.None);
        Anim_Enemy_AZ_Walk.AnimFrames[2] = new(2, 8, null, EnemyAction.None);
        Anim_Enemy_AZ_Walk.AnimFrames[3] = new(3, 8, null, EnemyAction.None);
        Anim_Enemy_AZ_Walk.AnimFrames[4] = new(4, 8, null, EnemyAction.None);
        Anim_Enemy_AZ_Walk.AnimFrames[5] = new(5, 8, null, EnemyAction.None);
        Anim_Enemy_AZ_Walk.UpdateAnimationLength();

        Anim_Enemy_AZ_Die.AnimFrames = new EnemyAnimationFrame[5];
        Anim_Enemy_AZ_Die.AnimFrames[0] = new(0, 8, null, EnemyAction.None);
        Anim_Enemy_AZ_Die.AnimFrames[1] = new(1, 8, null, EnemyAction.None);
        Anim_Enemy_AZ_Die.AnimFrames[2] = new(2, 8, null, EnemyAction.None);
        Anim_Enemy_AZ_Die.AnimFrames[3] = new(3, 8, null, EnemyAction.None);
        Anim_Enemy_AZ_Die.AnimFrames[4] = new(4, 8, null, EnemyAction.None);
        Anim_Enemy_AZ_Die.UpdateAnimationLength();

        Anim_Enemy_AZ_Attack.AnimFrames = new EnemyAnimationFrame[7];
        Anim_Enemy_AZ_Attack.AnimFrames[0] = new(0, 8, null, EnemyAction.None);
        Anim_Enemy_AZ_Attack.AnimFrames[1] = new(1, 8, null, EnemyAction.None);
        Anim_Enemy_AZ_Attack.AnimFrames[2] = new(2, 8, null, EnemyAction.None);
        Anim_Enemy_AZ_Attack.AnimFrames[3] = new(3, 8, SFX.AxeZombieSwing, EnemyAction.None);
        Anim_Enemy_AZ_Attack.AnimFrames[4] = new(4, 8, null, EnemyAction.AZ_Swing);
        Anim_Enemy_AZ_Attack.AnimFrames[5] = new(5, 8, null, EnemyAction.None);
        Anim_Enemy_AZ_Attack.AnimFrames[6] = new(6, 8, null, EnemyAction.None);
        Anim_Enemy_AZ_Attack.UpdateAnimationLength();

        Anim_Enemy_AZ_Stun.AnimFrames = new EnemyAnimationFrame[1];
        Anim_Enemy_AZ_Stun.AnimFrames[0] = new(0, 20, SFX.AxeZombiePain, EnemyAction.None);
        Anim_Enemy_AZ_Stun.UpdateAnimationLength();
    }
}
