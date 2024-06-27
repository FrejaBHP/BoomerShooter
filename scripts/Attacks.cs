using Godot;

public readonly struct HitscanAttack {
    public readonly DamageType DamageType;
    public readonly float Range;
    public readonly int Damage;

    public HitscanAttack(DamageType damageType, float range, int damage) {
        DamageType = damageType;
        Range = range;
        Damage = damage;
    }
}

// Implement later for AoE attacks?
public class RadialAttack {
    public DamageType DamageType { get; }
    public Vector3 Origin { get; }
    public float Radius { get; }
    public int Damage { get; }

    public RadialAttack() {

    }
}

public static class Attacks {
    // Player Weapons
    public static readonly HitscanAttack PitchforkStab = new(DamageType.Melee, 1f, 4);
    public static readonly HitscanAttack BulletRegular = new(DamageType.Bullet, 128f, 4);

    // Enemy Attacks
    public static readonly HitscanAttack AxeZombieSwing = new(DamageType.Melee, 1f, 15);

    public static AudioStreamWav MakeNoise(DamageType dt, SurfaceType st) {
        switch (dt) {
            case DamageType.Melee:
                return st switch {
                    SurfaceType.Metal => SFX.HitMetal,
                    SurfaceType.Stone => SFX.HitStone,
                    SurfaceType.Wood => SFX.HitWood,
                    SurfaceType.Flesh => SFX.HitFlesh,
                    _ => null,
                };
            
            case DamageType.Bullet:
                return st switch {
                    SurfaceType.Metal => SFX.HitMetal,
                    SurfaceType.Stone => null,
                    SurfaceType.Wood => null,
                    SurfaceType.Flesh => null,
                    _ => null,
                };
            
            default:
                return null;
        }
    }

    public static MiscAnimation MakeSparks(DamageType dt, SurfaceType st) {
        switch (dt) {
            case DamageType.Melee:
                return st switch {
                    SurfaceType.Metal => MiscAnimations.Anim_Misc_Hitspark_Metal,
                    SurfaceType.Stone => MiscAnimations.Anim_Misc_Hitspark_Metal,
                    SurfaceType.Wood => MiscAnimations.Anim_Misc_Hitspark_Wood,
                    SurfaceType.Flesh => MiscAnimations.Anim_Misc_Hitspark_Flesh,
                    _ => null,
                };
            
            case DamageType.Bullet:
                return st switch {
                    SurfaceType.Metal => MiscAnimations.Anim_Misc_Hitspark_Metal,
                    SurfaceType.Stone => MiscAnimations.Anim_Misc_Hitspark_Metal,
                    SurfaceType.Wood => MiscAnimations.Anim_Misc_Hitspark_Wood,
                    SurfaceType.Flesh => MiscAnimations.Anim_Misc_Hitspark_Flesh,
                    _ => null,
                };
            
            default:
                return null;
        }
    }
}
