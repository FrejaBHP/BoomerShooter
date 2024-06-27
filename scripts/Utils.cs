using System;
using Godot;

public static class Utils {
    private static readonly PackedScene emitter = GD.Load<PackedScene>("res://emitter.tscn");

    static readonly Random random = new Random();

    public static int RandomInt(int min, int max) {
        return random.Next(min, max);
    }
    public static float RandomFloat(float scale) {
        return random.NextSingle() * scale;
    }

    public static float RandomOffset(float ratio) {
        // Ratio guide table:
        // 0.0  = 0 deg
        // 0.05 = 2,86 deg
        // 0.1  = 5,71 deg
        // 0.15 = 8,53 deg
        // 0.2  = 11,31 deg
        // 0.25 = 14 deg

        // 0.5  = 26,57 deg
        // 0.75 = 36,87 deg
        // 1.0  = 45 deg

        return (random.NextSingle() - random.NextSingle()) * ratio;
    }

    public static bool IsSurfaceTooSteep(Vector3 normal, Actor body) {
        return normal.AngleTo(Vector3.Up) > body.FloorMaxAngle;
    }

    public static void CreateEmitter(Vector3 pos, AudioStream sound, MiscAnimation animation, bool flipX) {
        Emitter e = emitter.Instantiate() as Emitter;
		Game.EntitiesNode.AddChild(e);

        //await e.ChildrenSet();
        //e.Reparent(Game.EntitiesNode);
        e.GlobalPosition = pos;
        e.FlipX = flipX;
        e.Audio.Stream = sound;
        e.SetAnimation(animation);
        e.SetDuration();
        e.PlaySound();
	}
}
