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
    public static HitscanAttack PitchforkStab = new(DamageType.Melee, 0.96f, 1);
    public static HitscanAttack BulletRegular = new(DamageType.Bullet, 128f, 4);
}