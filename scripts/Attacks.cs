public class HitscanAttack {
    public DamageType DamageType { get; }
    public float Range { get; }
    public int Damage { get; }

    public HitscanAttack(DamageType damageType, float range, int damage) {
        DamageType = damageType;
        Range = range;
        Damage = damage;
    }
}

public static class Attacks {
    public static readonly HitscanAttack PitchforkStab = new(DamageType.Melee, 1f, 1);
    public static readonly HitscanAttack BulletRegular = new(DamageType.Bullet, 128f, 4);
}