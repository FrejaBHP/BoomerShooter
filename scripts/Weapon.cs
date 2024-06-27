using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;

public abstract class Weapon {
    public abstract WStateMachine WStateMachine { get; }
    public abstract Ammotype AmmoType { get; }
    public abstract WeaponState WeaponState { get; protected set; }
    public abstract bsPlayer Player { get; protected set; }
    public abstract int AmmoReqPri { get; }
    public abstract int AmmoReqSec { get; }

    public abstract AudioStreamWav PrimaryFireAudio { get; protected set; }
    public abstract AudioStreamWav AltFireAudio { get; protected set; }

    public static float WeaponOffsetTop { get; } = 0f;
    public static float WeaponOffsetBottom { get; } = 288f;

    public void SetWeaponState(int wstate) {
        WeaponState = (WeaponState)wstate;
    }

    protected class WUpState : WState {}
    protected class WDownState : WState {}
    protected class WReadyState : WState {}
    protected class WAtkState : WState {}
    protected class WAltState : WState {}

    protected abstract WUpState UpState { get; }
    protected abstract WDownState DownState { get; }
    protected abstract WReadyState ReadyState { get; }
    protected abstract WAtkState AtkState { get; }
    protected virtual WAltState AltState { get; }

    public virtual void PrimaryFire() {
        if (WeaponState == WeaponState.ReadyState) {
            EnterAtkState();
        }
    }

    public virtual void AltFire() {
        if (WeaponState == WeaponState.ReadyState) {
            EnterAltState();
        }
    }

    public virtual void PrimaryFireReleased() {
        
    }

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
        if (player.WeaponYOffset <= WeaponOffsetTop) {
            player.ActiveWeaponView.SetPosition(player.ActiveWeaponView.Position with { Y = WeaponOffsetTop });
            player.WeaponYOffset = WeaponOffsetTop;
            weapon.EnterReadyState();
        }
        else {
            player.WeaponYOffset -= 16f;
            player.ActiveWeaponView.SetPosition(player.ActiveWeaponView.Position with { Y = player.WeaponYOffset });
        }
    }
    
    public static void A_Lower(bsPlayer player, Weapon weapon) {
        if (player.WeaponYOffset >= WeaponOffsetBottom) {
            player.ActiveWeaponView.SetPosition(player.ActiveWeaponView.Position with { Y = WeaponOffsetBottom });
            player.WeaponYOffset = WeaponOffsetBottom;
            
            player.ActiveWeapon = player.WeaponInventory[player.WeaponToSwitchTo];
            player.ActiveWeaponNum = player.WeaponToSwitchTo;
            player.BringUpNewWeapon((WeaponType)player.WeaponToSwitchTo);
        }
        else {
            player.WeaponYOffset += 16f;
            player.ActiveWeaponView.SetPosition(player.ActiveWeaponView.Position with { Y = player.WeaponYOffset });
        }
    }

    public static void A_SnapToTop(bsPlayer player) {
        player.ActiveWeaponView.SetPosition(player.ActiveWeaponView.Position with { Y = WeaponOffsetTop });
        player.WeaponYOffset = WeaponOffsetTop;
    }

    public static void A_SnapToBottom(bsPlayer player) {
        player.ActiveWeaponView.SetPosition(player.ActiveWeaponView.Position with { Y = WeaponOffsetBottom });
        player.WeaponYOffset = WeaponOffsetBottom;
    }

    public static void W_UseAmmo(bsPlayer player, Ammotype ammo, int amount) {
        if (ammo != Ammotype.None) {
            player.PlayerAmmo[(int)ammo].Ammo -= amount;

            // Dirty, probably should fix this. Replace with signal? Is that even possible?
            player.PlayerHUD.HUDUpdatePlayerAmmo(player.PlayerAmmo[(int)ammo].Ammo);
        }
    }
}


public sealed partial class W_Pitchfork : Weapon {
    public override WStateMachine WStateMachine { get; } = new();
    public override Ammotype AmmoType { get; } = Ammotype.None;
    public override int AmmoReqPri { get; } = 0;
    public override int AmmoReqSec { get; } = 0;

    public override AudioStreamWav PrimaryFireAudio { get; protected set; } = null;
    public override AudioStreamWav AltFireAudio { get; protected set; } = null;

    public override WeaponState WeaponState { get; protected set; }
    public override bsPlayer Player { get; protected set; }

    protected override WUpState UpState { get; } = new();
    protected override WDownState DownState { get; } = new();
    protected override WReadyState ReadyState { get; } = new();
    protected override WAtkState AtkState { get; } = new();

    private class WAtkState1 : WState {}
    private class WAtkState2 : WState {}
    private class WRecoveryState : WState {}
    private readonly WAtkState1 AtkState1 = new();
    private readonly WAtkState2 AtkState2 = new();
    private readonly WRecoveryState RecoveryState = new();

    public W_Pitchfork(bsPlayer player) {
        Player = player;

        // ConfigureState: Length (frames), Weapon (this), entry Animation, Action on first frame, enum WeaponState, State NextState
        // Negative length doesn't count frames, negative WeaponState keeps current, null Action does nothing, null NextState does not change state by itself
        UpState.ConfigureState(1, this, WAnimations.Frame_Wep_PF[0], () => A_Raise(Player, this), (int)WeaponState.UpState, UpState);
        DownState.ConfigureState(1, this, WAnimations.Frame_Wep_PF[0], () => A_Lower(Player, this), (int)WeaponState.DownState, DownState);
        ReadyState.ConfigureState(-1, this, WAnimations.Frame_Wep_PF[0], null, (int)WeaponState.ReadyState, null);
        AtkState.ConfigureState(3, this, WAnimations.Frame_Wep_PF[1], null, (int)WeaponState.AtkState, AtkState1);
        AtkState1.ConfigureState(13, this, WAnimations.Frame_Wep_PF[2], () => A_Stab(Player), -1, AtkState2);
        AtkState2.ConfigureState(3, this, WAnimations.Frame_Wep_PF[1], null, -1, RecoveryState);
        RecoveryState.ConfigureState(3, this, WAnimations.Frame_Wep_PF[0], null, -1, ReadyState);
    }

    public override void AltFire() {
        if (WeaponState == WeaponState.ReadyState) {
            EnterAtkState();
        }
    }

    public static void A_Stab(bsPlayer player) {
        float offsetX = -0.09f;
        for (int i = 0; i < 4; i++) {
            player.FireHitscanAttack(player.PlayerCamera, Attacks.PitchforkStab, offsetX, 0, true);
            offsetX += 0.06f;
        }
    }
}


public sealed partial class W_Shotgun : Weapon {
    public override WStateMachine WStateMachine { get; } = new();
    public override Ammotype AmmoType { get; } = Ammotype.Shells;
    public override int AmmoReqPri { get; } = 1;
    public override int AmmoReqSec { get; } = 2;

    public override AudioStreamWav PrimaryFireAudio { get; protected set; } = SFX.ShotgunFire;
    public override AudioStreamWav AltFireAudio { get; protected set; } = SFX.ShotgunAltFire;

    public override WeaponState WeaponState { get; protected set; }
    public override bsPlayer Player { get; protected set; }

    protected override WUpState UpState { get; } = new();
    protected override WDownState DownState { get; } = new();
    protected override WReadyState ReadyState { get; } = new();
    protected override WAtkState AtkState { get; } = new();
    protected override WAltState AltState { get; } = new();

    private class WRecoveryState : WState {}
    private class WReloadState : WState {}
    private class WReloadActionState : WState {}

    private readonly WRecoveryState RecoveryState = new();
    private readonly WReloadState ReloadState = new();
    private readonly WReloadActionState ReloadActionState = new();

    public int Shells = 2;

    public W_Shotgun(bsPlayer player) {
        Player = player;

        // ConfigureState: Length (frames), Weapon (this), entry Animation, Action on first frame, enum WeaponState, State NextState
        // Negative length doesn't count frames, negative WeaponState keeps current, null Action does nothing, null NextState does not change state by itself
        UpState.ConfigureState(1, this, WAnimations.Anim_Wep_SG[0], () => A_Raise(Player, this), (int)WeaponState.UpState, UpState);
        DownState.ConfigureState(1, this, WAnimations.Anim_Wep_SG[0], () => A_Lower(Player, this), (int)WeaponState.DownState, DownState);
        ReadyState.ConfigureState(-1, this, WAnimations.Anim_Wep_SG[0], null, (int)WeaponState.ReadyState, null);
        AtkState.ConfigureState(this, WAnimations.Anim_Wep_SG_Atk, () => A_FireShotgun(Player, 1, ref Shells), (int)WeaponState.AtkState, RecoveryState);
        AltState.ConfigureState(this, WAnimations.Anim_Wep_SG_Alt, () => A_FireShotgun(Player, 2, ref Shells), (int)WeaponState.AltState, RecoveryState);
        RecoveryState.ConfigureState(1, this, WAnimations.Anim_Wep_SG[0], () => A_ShotgunReloadCheck(Player, this), -1, null);
        ReloadState.ConfigureState(this, WAnimations.Anim_Wep_SG_Reload, null, -1, ReloadActionState);
        ReloadActionState.ConfigureState(1, this, null, () => A_ReloadShotgun(Player, ref Shells), -1, ReadyState);
    }

    public void EnterReloadState() {
        WStateMachine.SetState(ReloadState);
    }

    public override void PrimaryFire() {
        if (WeaponState == WeaponState.ReadyState) {
            if (Player.PlayerAmmo[(int)AmmoType].Ammo >= AmmoReqPri) {
                EnterAtkState();
            }
            else {
                EnterReadyState();
                Player.SwitchToWeaponWithAmmo();
            }
        }
    }

    public override void AltFire() {
        if (WeaponState == WeaponState.ReadyState) {
            if ((Player.PlayerAmmo[(int)AmmoType].Ammo >= AmmoReqSec) && Shells == 2) {
                EnterAltState();
            }
            else if (Shells == 1) {
                EnterAtkState();
            }
            else {
                EnterReadyState();
                Player.SwitchToWeaponWithAmmo();
            }
        }
    }

    public static void A_FireShotgun(bsPlayer player, int barrels, ref int shells) {
        float offsetX;
        float offsetY;

        if (barrels == 1) {
            if (shells == 2) {
                player.FetchAndPlaySecondaryAnimation(WAnimations.Anim_Wep_SG_FlashLeft);
            }
            else if (shells == 1) {
                player.FetchAndPlaySecondaryAnimation(WAnimations.Anim_Wep_SG_FlashRight);
            }
            shells--;
            player.PriFireAudio.Play();
        }
        else {
            player.FetchAndPlaySecondaryAnimation(WAnimations.Anim_Wep_SG_FlashBoth);
            shells = 0;
            player.AltFireAudio.Play();
        }
        
        W_UseAmmo(player, Ammotype.Shells, barrels);

        for (int pellet = 0; pellet < (16 * barrels); pellet++) {
            bool makeSound = false;
            if (pellet % 4 == 0) {
                makeSound = true;
            }
            offsetX = Utils.RandomOffset(0.13f); // 0.13f = approx. 7,4deg max
            offsetY = Utils.RandomOffset(0.03f);
            player.FireHitscanAttack(player.PlayerCamera, Attacks.BulletRegular, offsetX, offsetY, makeSound);
        }
    }

    public static void A_ShotgunReloadCheck(bsPlayer player, W_Shotgun shotgun) {
        if (shotgun.Shells == 0) {
            if (player.PlayerAmmo[(int)Ammotype.Shells].Ammo > 1) {
                shotgun.EnterReloadState();
            }
            else {
                shotgun.EnterReadyState();
                player.SwitchToWeaponWithAmmo();
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

public sealed partial class W_DynamiteReg : Weapon {
    public override WStateMachine WStateMachine { get; } = new();
    public override Ammotype AmmoType { get; } = Ammotype.DynamiteReg;
    public override int AmmoReqPri { get; } = 1;
    public override int AmmoReqSec { get; } = 1;

    public override AudioStreamWav PrimaryFireAudio { get; protected set; } = null;
    public override AudioStreamWav AltFireAudio { get; protected set; } = null;

    public override WeaponState WeaponState { get; protected set; }
    public override bsPlayer Player { get; protected set; }

    protected override WUpState UpState { get; } = new();
    protected override WDownState DownState { get; } = new();
    protected override WReadyState ReadyState { get; } = new();
    protected override WAtkState AtkState { get; } = new();
    protected override WAltState AltState { get; } = new();

    private class WSwapState : WState {}
    private class WCookState : WState {}
    private class WThrowState : WState {}
    private class WThrowActionState : WState {}
    private class WRecoveryState : WState {}
    private readonly WSwapState SwapState = new();
    private readonly WCookState CookState = new();
    private readonly WThrowState ThrowState = new();
    private readonly WThrowActionState ThrowActionState = new();
    private readonly WRecoveryState RecoveryState = new();

    private bool cooking = false;
    private int cookTime = 0;

    public W_DynamiteReg(bsPlayer player) {
        Player = player;

        // ConfigureState: Length (frames), Weapon (this), entry Animation, Action on first frame, enum WeaponState, State NextState
        // Negative length doesn't count frames, negative WeaponState keeps current, null Action does nothing, null NextState does not change state by itself
        UpState.ConfigureState(this, WAnimations.Anim_Wep_DynaReg_Up, () => RaiseLighter(), (int)WeaponState.UpState, ReadyState);
        DownState.ConfigureState(this, WAnimations.Anim_Wep_DynaReg_Down, () => LowerLighter(), (int)WeaponState.DownState, SwapState);
        ReadyState.ConfigureState(WAnimations.Anim_Wep_Light_Idle.TotalLength, this, WAnimations.Frame_Wep_DynaReg_Idle, () => LighterIdle(), (int)WeaponState.ReadyState, ReadyState);
        SwapState.ConfigureState(0, this, null, () => SwapFromDynamite(), -1, null);
        CookState.ConfigureState(this, WAnimations.Anim_Wep_DynaReg_Cook, null, (int)WeaponState.CustomState, null);
        ThrowState.ConfigureState(this, WAnimations.Anim_Wep_DynaReg_Throw, null, -1, ThrowActionState);
        ThrowActionState.ConfigureState(1, this, null, () => Throw(), -1, RecoveryState);
        RecoveryState.ConfigureState(5, this, null, null, -1, ReadyState);
    }

    public override void PrimaryFire() {
        if (WeaponState == WeaponState.ReadyState && Player.PlayerAmmo[(int)AmmoType].Ammo >= AmmoReqPri) {
            WStateMachine.SetState(CookState);
            Player.FetchAndPlaySecondaryAnimation(WAnimations.Anim_Wep_Light_Cook);
            cooking = true;
            Player.PlayerHUD.ShowChargeBar();
        }
        else if (WeaponState == WeaponState.CustomState && cooking) {
            IncreaseCookTimer();
        }
    }

    public override void AltFire() {
        if (WeaponState == WeaponState.ReadyState && Player.PlayerAmmo[(int)AmmoType].Ammo >= AmmoReqSec) {
            
        }
    }

    public override void PrimaryFireReleased() {
        if (WeaponState == WeaponState.CustomState && cooking) {
            WStateMachine.SetState(ThrowState);
            cooking = false;
            Player.PlayerHUD.HideChargeBar();
        }
    }

    private void IncreaseCookTimer() {
        cookTime++;
        if (cookTime <= 120) {
            Player.PlayerHUD.ChargeBarProgress = cookTime;
        }
        if (cookTime >= WAnimations.Anim_Wep_Light_Cook.TotalLength) {
            if ((cookTime - WAnimations.Anim_Wep_Light_Cook.TotalLength) % WAnimations.Anim_Wep_Light_Cook_Idle.TotalLength == 0) {
                Player.FetchAndPlaySecondaryAnimation(WAnimations.Anim_Wep_Light_Cook_Idle);
            }
        }
    }

    private void Throw() {
        cookTime = Math.Clamp(cookTime, 0, 120);
        float force = (cookTime * 0.1f) + 2;
        
        Player.ThrowObject(ThrowableType.TNTBundle, force);
        cookTime = 0;
        Player.PlayerHUD.ChargeBarProgress = cookTime;
        W_UseAmmo(Player, AmmoType, 1);

        if (Player.PlayerAmmo[(int)Ammotype.DynamiteReg].Ammo == 0) {
            Player.SwitchToWeaponWithAmmo();
        }
    }

    private void RaiseLighter() {
        A_SnapToTop(Player);
        Player.FetchAndPlaySecondaryAnimation(WAnimations.Anim_Wep_Light_Up);
    }

    private void LowerLighter() {
        Player.FetchAndPlaySecondaryAnimation(WAnimations.Anim_Wep_Light_Down);
    }

    private void LighterIdle() {
        Player.FetchAndPlaySecondaryAnimation(WAnimations.Anim_Wep_Light_Idle);
    }

    private void RestartLighterCookIdleAnimation() {
        Player.FetchAndPlaySecondaryAnimation(WAnimations.Anim_Wep_Light_Cook_Idle);
    }

    private void SwapFromDynamite() {
        A_SnapToBottom(Player);
        Player.ActiveWeapon = Player.WeaponInventory[Player.WeaponToSwitchTo];
        Player.ActiveWeaponNum = Player.WeaponToSwitchTo;
        Player.BringUpNewWeapon((WeaponType)Player.WeaponToSwitchTo);
    }
}
