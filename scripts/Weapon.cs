using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;

public abstract class Weapon {
    public abstract StateMachine WStateMachine { get; }
    public abstract Ammotype AmmoType { get; }
    public abstract WeaponState WeaponState { get; protected set; }
    public abstract bsPlayer Player { get; protected set; }
    public abstract int AmmoReqPri { get; }
    public abstract int AmmoReqSec { get; }
    public abstract int AmmoOnPickup { get; }

    public static float WeaponOffsetTop { get; } = 0f;
    public static float WeaponOffsetBottom { get; } = 288f;

    public void SetWeaponState(int wstate) {
        WeaponState = (WeaponState)wstate;
    }

    protected class WUpState : State {}
    protected class WDownState : State {}
    protected class WReadyState : State {}
    protected class WAtkState : State {}
    protected class WAltState : State {}

    protected abstract WUpState UpState { get; }
    protected abstract WDownState DownState { get; }
    protected abstract WReadyState ReadyState { get; }
    protected abstract WAtkState AtkState { get; }
    protected virtual WAltState AltState { get; }

    public virtual void EnterAtkState() {
        WStateMachine.SetState(AtkState);
    }

    public virtual void EnterAltState() {
        WStateMachine.SetState(AltState);
    }

    protected virtual void EnterReadyState() {
        WStateMachine.SetState(ReadyState);
    }

    public virtual void EnterRaiseState() {
        WStateMachine.SetState(UpState);
    }

    public virtual void EnterLowerState() {
        WStateMachine.SetState(DownState);
    }

    public static void A_Raise(bsPlayer player, Weapon weapon) {
        if (player.ActiveWeaponControl.Position.Y <= WeaponOffsetTop) {
            player.ActiveWeaponControl.SetPosition(player.ActiveWeaponControl.Position with { Y = WeaponOffsetTop });
            weapon.EnterReadyState();
        }
        else {
            float newY = player.ActiveWeaponControl.Position.Y - 16f;
            player.ActiveWeaponControl.SetPosition(player.ActiveWeaponControl.Position with { Y = newY });
        }
    }
    
    public static void A_Lower(bsPlayer player, Weapon weapon) {
        if (player.ActiveWeaponControl.Position.Y >= WeaponOffsetBottom) {
            player.ActiveWeaponControl.SetPosition(player.ActiveWeaponControl.Position with { Y = WeaponOffsetBottom });
            
            player.ActiveWeapon = player.WeaponInventory[player.WeaponToSwitchTo];
            player.ActiveWeaponNum = player.WeaponToSwitchTo;
            player.BringUpNewWeapon((WeaponType)player.WeaponToSwitchTo);
        }
        else {
            float newY = player.ActiveWeaponControl.Position.Y + 16f;
            player.ActiveWeaponControl.SetPosition(player.ActiveWeaponControl.Position with { Y = newY });
        }
    }

    public static void W_UseAmmo(bsPlayer player, Ammotype ammo, int amount) {
        if (ammo != Ammotype.None) {
            player.PlayerAmmo[(int)ammo].Ammo -= amount;

            // Dirty, probably should fix this. Replace with signal? Is that even possible?
            player.PlayerHUD.HUDUpdatePlayerAmmo(player.PlayerAmmo[(int)ammo].Ammo);
        }
    }

    public static void A_Stab(bsPlayer player) {
        float offsetX = -0.09f;
        for (int i = 0; i < 4; i++) {
            player.FireHitscanAttack(Attacks.PitchforkStab, offsetX, 0);
            offsetX += 0.06f;
        }
    }

    public static void A_FireShotgun(bsPlayer player, int barrels, ref int shells) {
        float offsetX;
        float offsetY;

        for (int pellet = 0; pellet < (16 * barrels); pellet++) {
            offsetX = Utils.RandomOffset(0.13f); // 0.13f = approx. 7,4deg
            offsetY = Utils.RandomOffset(0.13f);
            player.FireHitscanAttack(Attacks.BulletRegular, offsetX, offsetY);
        }

        if (barrels == 1) {
            shells--;
        }
        else {
            shells = 0;
        }
        
        W_UseAmmo(player, Ammotype.Shells, barrels);
    }

    public static void A_ShotgunReloadCheck(bsPlayer player, W_Shotgun shotgun) {
        if (shotgun.Shells == 0) {
            if (player.PlayerAmmo[(int)Ammotype.Shells].Ammo > 1) {
                shotgun.EnterReloadState();
            }
            else {
                shotgun.EnterReadyState();
            }
        }
        else {
            shotgun.EnterReadyState();
        }
    }

    public static void A_ReloadShotgun(bsPlayer player, ref int shells) {
        if (player.PlayerAmmo[(int)Ammotype.Shells].Ammo > 1) {
            shells = 2;
        }
        else if (player.PlayerAmmo[(int)Ammotype.Shells].Ammo == 1) {
            shells = 1;
        }
    }
}

public sealed partial class W_Pitchfork : Weapon {
    public override StateMachine WStateMachine { get; } = new();
    public override Ammotype AmmoType { get; } = Ammotype.None;
    public override int AmmoReqPri { get; } = 0;
    public override int AmmoReqSec { get; } = 0;
    public override int AmmoOnPickup { get; } = 0;
    public override WeaponState WeaponState { get; protected set; }
    public override bsPlayer Player { get; protected set; }

    protected override WUpState UpState { get; } = new();
    protected override WDownState DownState { get; } = new();
    protected override WReadyState ReadyState { get; } = new();
    protected override WAtkState AtkState { get; } = new();

    private class WAtkState1 : State {}
    private class WAtkState2 : State {}
    private class WRecoveryState : State {}
    private readonly WAtkState1 AtkState1 = new();
    private readonly WAtkState2 AtkState2 = new();
    private readonly WRecoveryState RecoveryState = new();

    public W_Pitchfork(bsPlayer player) {
        Player = player;

        // ConfigureState: Length (frames), Weapon (this), entry Texture, Action on first frame, enum WeaponState, State NextState
        // Negative length doesn't count frames, negative WeaponState keeps current, null Action does nothing, null NextState does not change state by itself
        UpState.ConfigureState(1, this, WeaponSprites.Spr_Wep_PF[0], () => A_Raise(Player, this), (int)WeaponState.UpState, UpState);
        DownState.ConfigureState(1, this, WeaponSprites.Spr_Wep_PF[0], () => A_Lower(Player, this), (int)WeaponState.DownState, DownState);
        ReadyState.ConfigureState(-1, this, WeaponSprites.Spr_Wep_PF[0], null, (int)WeaponState.ReadyState, null);
        AtkState.ConfigureState(3, this, WeaponSprites.Spr_Wep_PF[1], null, (int)WeaponState.AtkState, AtkState1);
        AtkState1.ConfigureState(13, this, WeaponSprites.Spr_Wep_PF[2], () => A_Stab(Player), -1, AtkState2);
        AtkState2.ConfigureState(3, this, WeaponSprites.Spr_Wep_PF[1], null, -1, RecoveryState);
        RecoveryState.ConfigureState(3, this, WeaponSprites.Spr_Wep_PF[0], null, -1, ReadyState);
    }

    public override void EnterAltState() {
        WStateMachine.SetState(AtkState);
    }
}

public sealed partial class W_Shotgun : Weapon {
    public override StateMachine WStateMachine { get; } = new();
    public override Ammotype AmmoType { get; } = Ammotype.Shells;
    public override int AmmoReqPri { get; } = 1;
    public override int AmmoReqSec { get; } = 2;
    public override int AmmoOnPickup { get; } = 8;
    public override WeaponState WeaponState { get; protected set; }
    public override bsPlayer Player { get; protected set; }

    //private class WAtkState1 : State {}
    //private class WAtkState2 : State {}

    protected override WUpState UpState { get; } = new();
    protected override WDownState DownState { get; } = new();
    protected override WReadyState ReadyState { get; } = new();
    protected override WAtkState AtkState { get; } = new();
    protected override WAltState AltState { get; } = new();
    //private readonly WAtkState1 AtkState1 = new();
    //private readonly WAtkState2 AtkState2 = new();

    private class WRecoveryState : State {}
    private class WReloadState : State {}
    private class WReloadState1 : State {}
    private class WReloadState2 : State {}
    private class WReloadState3 : State {}
    private class WReloadState4 : State {}
    private class WReloadState5 : State {}
    private readonly WRecoveryState RecoveryState = new();
    private readonly WReloadState ReloadState = new();
    private readonly WReloadState1 ReloadState1 = new();
    private readonly WReloadState2 ReloadState2 = new();
    private readonly WReloadState3 ReloadState3 = new();
    private readonly WReloadState4 ReloadState4 = new();
    private readonly WReloadState5 ReloadState5 = new();

    public int Shells = 2;

    public W_Shotgun(bsPlayer player) {
        Player = player;

        // ConfigureState: Length (frames), Weapon (this), entry Texture, Action on first frame, enum WeaponState, State NextState
        // Negative length doesn't count frames, negative WeaponState keeps current, null Action does nothing, null NextState does not change state by itself
        UpState.ConfigureState(1, this, WeaponSprites.Spr_Wep_SG[0], () => A_Raise(Player, this), (int)WeaponState.UpState, UpState);
        DownState.ConfigureState(1, this, WeaponSprites.Spr_Wep_SG[0], () => A_Lower(Player, this), (int)WeaponState.DownState, DownState);
        ReadyState.ConfigureState(-1, this, WeaponSprites.Spr_Wep_SG[0], null, (int)WeaponState.ReadyState, null);
        AtkState.ConfigureState(15, this, WeaponSprites.Spr_Wep_SG[0], () => A_FireShotgun(Player, 1, ref Shells), (int)WeaponState.AtkState, RecoveryState);
        AltState.ConfigureState(15, this, WeaponSprites.Spr_Wep_SG[0], () => A_FireShotgun(Player, 2, ref Shells), (int)WeaponState.AltState, RecoveryState);
        //AtkState1.ConfigureState(13, this, WeaponSprites.Spr_Wep_PF[2], null, -1, AtkState2);
        //AtkState2.ConfigureState(3, this, WeaponSprites.Spr_Wep_PF[1], null, -1, RecoveryState);
        RecoveryState.ConfigureState(1, this, WeaponSprites.Spr_Wep_SG[0], () => A_ShotgunReloadCheck(Player, this), -1, null);
        ReloadState.ConfigureState(10, this, WeaponSprites.Spr_Wep_SG[1], null, -1, ReloadState1);
        ReloadState1.ConfigureState(10, this, WeaponSprites.Spr_Wep_SG[2], null, -1, ReloadState2);
        ReloadState2.ConfigureState(10, this, WeaponSprites.Spr_Wep_SG[3], null, -1, ReloadState3);
        ReloadState3.ConfigureState(1, this, WeaponSprites.Spr_Wep_SG[3], null, -1, ReloadState4);
        ReloadState4.ConfigureState(1, this, WeaponSprites.Spr_Wep_SG[3], null, -1, ReloadState5);
        ReloadState5.ConfigureState(10, this, WeaponSprites.Spr_Wep_SG[4], () => A_ReloadShotgun(Player, ref Shells), -1, ReadyState);
    }

    public override void EnterAltState() {
        if (Shells != 2) {
            WStateMachine.SetState(AtkState);
        }
        else {
            base.EnterAltState();
        }
    }

    public void EnterReloadState() {
        WStateMachine.SetState(ReloadState);
    }
}
