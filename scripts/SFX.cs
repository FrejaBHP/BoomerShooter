using Godot;

public class SFX {
    public static readonly AudioStreamWav ShotgunFire = GD.Load<AudioStreamWav>("sounds/weapons/shotfire.wav");
    public static readonly AudioStreamWav ShotgunAltFire = GD.Load<AudioStreamWav>("sounds/weapons/shotfir2.wav");
    public static readonly AudioStreamWav ShotgunCock = GD.Load<AudioStreamWav>("sounds/weapons/shotcock.wav");

    public static readonly AudioStreamWav TommyFire = GD.Load<AudioStreamWav>("sounds/weapons/tomfire.wav");

    public static readonly AudioStreamWav HitMetal = GD.Load<AudioStreamWav>("sounds/misc/hitmetal.wav");
    public static readonly AudioStreamWav HitStone = GD.Load<AudioStreamWav>("sounds/misc/hitstone.wav");
    public static readonly AudioStreamWav HitWood = GD.Load<AudioStreamWav>("sounds/misc/hitwood.wav");
    public static readonly AudioStreamWav HitFlesh = GD.Load<AudioStreamWav>("sounds/misc/hitflesh.wav");

    public static readonly AudioStreamWav AxeZombieDie1 = GD.Load<AudioStreamWav>("sounds/enemies/zombie/die1.wav");
    public static readonly AudioStreamWav AxeZombieDie2 = GD.Load<AudioStreamWav>("sounds/enemies/zombie/die2.wav");
    public static readonly AudioStreamWav AxeZombieDie3 = GD.Load<AudioStreamWav>("sounds/enemies/zombie/die3.wav");
    public static readonly AudioStreamWav AxeZombiePain = GD.Load<AudioStreamWav>("sounds/enemies/zombie/pain.wav");
    public static readonly AudioStreamWav AxeZombieSwing = GD.Load<AudioStreamWav>("sounds/enemies/zombie/swing.wav");
}
