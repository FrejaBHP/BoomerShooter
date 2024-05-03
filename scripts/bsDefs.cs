public enum Ammotype {
    None,
    Shells,
    NoOfTypes
}

public enum WeaponType {
    W_Pitchfork,
    W_Shotgun,
    NoOfWeapons
}

public enum DamageType {
    Melee,
    Bullet
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

public class Ammunition {
    public int Max { get; }
    private int ammo = 0;
    public int Ammo { 
        get => ammo; 
        set {
            if (ammo + value > Max) {
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
