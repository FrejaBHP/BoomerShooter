using Godot;

public enum Ammotype {
    None,
    Shells,
    Bullets,
    NoOfTypes
}

public enum WeaponType {
    W_Pitchfork,
    W_Shotgun,
    W_Tommygun,
    NoOfWeapons
}

public enum DamageType {
    Melee,
    Bullet
}

public enum EnemyType {
    AxeZombie,
    NoOfEnemies
}

public enum KeyType {

}

public enum WeaponState {
    ReadyState,
    AtkState,
    AltState,
    UpState,
    DownState
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
