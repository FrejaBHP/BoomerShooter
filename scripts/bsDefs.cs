using Godot;

public enum Ammotype {
    None,
    Shells,
    Bullets,
    DynamiteReg,
    NoOfTypes
}

public enum WeaponType {
    W_Pitchfork,
    W_Shotgun,
    W_TommyGun,
    W_DynamiteReg,
    NoOfWeapons
}

public enum DamageType {
    Melee,
    Bullet,
    Explosive
}

public enum SurfaceType {
    None,
    Metal,
    Stone,
    Wood,
    Flesh
}

public enum ThrowableType {
    TNTBundle
}

public enum EnemyType {
    AxeZombie,
    NoOfEnemies
}

public enum MoveState {
    None,
    Stand,
    Turn,
    Walk,
    Fly,
    Knockback
}

public enum KeyType {

}

public enum WeaponState {
    ReadyState,
    AtkState,
    AltState,
    UpState,
    DownState,
    CustomState
}

public enum EnemyState {
    Idle,
    Chase,
    Attack,
    AltAttack,
    Stun,
    Death,
    Corpse
}

public enum EnemyAction {
    None,
    AZ_Swing
}

public enum WeaponAction {
    None,
    PF_Stab,
    SG_Reload,
    Tommy_Fire,
    Tommy_Alt,
    Dyn_Throw
}

public class Ammunition {
    public int Max { get; }
    private int ammo = 0;
    public int Ammo { 
        get => ammo; 
        set {
            if (value > Max) {
                ammo = Max;
            }
            else {
                ammo = value;
            }
        }
    }

    public Ammunition(int max) {
        Max = max;
    }
}
