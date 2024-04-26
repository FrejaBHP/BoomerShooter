using System.Runtime.CompilerServices;
using Godot;

public abstract class Weapon {
    public abstract StateMachine WStateMachine { get; }
    public abstract Ammotype AmmoType { get; }
    public abstract WeaponState WeaponState { get; protected set; }
    public abstract bsPlayer Player { get; protected set; }
    public abstract int AmmoReqPri { get; }
    public abstract int AmmoReqSec { get; }

    public void SetWeaponState(int wstate) {
        WeaponState = (WeaponState)wstate;
    }

    public abstract void EnterAtkState();
    public abstract void EnterAltState();
}

public partial class W_Pitchfork : Weapon {
    public override StateMachine WStateMachine { get; } = new();
    public override Ammotype AmmoType { get; } = Ammotype.A_None;
    public override int AmmoReqPri { get; } = 0;
    public override int AmmoReqSec { get; } = 0;
    public override WeaponState WeaponState { get; protected set; }
    public override bsPlayer Player { get; protected set; }

    private class WReadyState : State {}
    private class WAtkState : State {}
    private class WAtkState1 : State {}
    private class WAtkState2 : State {}
    private class WRecoveryState : State {}

    private readonly WReadyState ReadyState = new();
    private readonly WAtkState AtkState = new();
    private readonly WAtkState1 AtkState1 = new();
    private readonly WAtkState2 AtkState2 = new();
    private readonly WRecoveryState RecoveryState = new();

    public W_Pitchfork(bsPlayer player) {
        Player = player;

        // ConfigureState: Length (frames), Weapon (this), Texture, enum WeaponState, State NextState
        // Negative length doesn't count frames, negative WeaponState keeps current, null NextState does not change state by itself
        ReadyState.ConfigureState(-1, this, WeaponSprites.S_W_PF[0], (int)WeaponState.ReadyState, null);
        AtkState.ConfigureState(3, this, WeaponSprites.S_W_PF[1], (int)WeaponState.AtkState, AtkState1);
        AtkState1.ConfigureState(13, this, WeaponSprites.S_W_PF[2], -1, AtkState2);
        AtkState2.ConfigureState(3, this, WeaponSprites.S_W_PF[1], -1, RecoveryState);
        RecoveryState.ConfigureState(3, this, WeaponSprites.S_W_PF[0], -1, ReadyState);

        WStateMachine.SetState(ReadyState);
    }

    public override void EnterAtkState() {
        WStateMachine.SetState(AtkState);
    }

    public override void EnterAltState() {
        WStateMachine.SetState(AtkState);
    }
}
