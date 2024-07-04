using System;
using System.Reflection;
using Godot;

public interface IState {
    public void Enter();
    public void Execute();
    public void Exit();
}

// ------ ENEMIES FINITE STATE MACHINE AND STATES

public partial class EStateMachine {
    EState currentState;

    public void SetState(EState newState) {
        if (currentState != null) {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter();
    }

    public void Process() {
        if (currentState != null) {
            currentState.Execute();
        }
    }
}

public abstract class EState : IState {
    protected int Length { get; private set; }
    protected EnemyBase Enemy { get; private set; }
    protected EnemyAnimationFrame Frame { get; private set; }
    protected EnemyAnimation Animation { get; private set; }
    protected int EEState { get; private set; }
    protected EState NextState { get; private set; }
    protected Action Action { get; private set; } = null;

    protected int FrameCounter { get; private set; }

    public virtual void Enter() {
        FrameCounter = 0;
        if (EEState != -1) {
            Enemy.SetEnemyState(EEState);
        }
        
        if (Frame != null) { // For single-frame stuff
            
        }
        if (Animation != null) {
            Enemy.FetchAnimation(Animation);
        }
        Action?.Invoke();
    }

    public virtual void Execute() {
        if (Length > 0) {
            FrameCounter++;
            if (FrameCounter >= Length && NextState != null) {
                Enemy.EStateMachine.SetState(NextState);
            }
            else if (FrameCounter >= Length && NextState == null) {
                Enemy.EStateMachine.SetState(this);
            }
        }
    }

    public virtual void Exit() {

    }

    public virtual void ConfigureState(int frames, EnemyBase enemy, EnemyAnimationFrame frame, Action? action, int eState, EState? next) {
        Length = frames;
        Enemy = enemy;
        Frame = frame;
        Animation = null;
        Action = action;
        EEState = eState;
        NextState = next;
    }

    public virtual void ConfigureState(EnemyBase enemy, EnemyAnimation animation, Action? action, int eState, EState? next) {
        Enemy = enemy;
        Animation = animation;
        Length = Animation.TotalLength;
        Frame = null;
        Action = action;
        EEState = eState;
        NextState = next;
    }
}


// ------ WEAPONS FINITE STATE MACHINE AND STATES ------

public partial class WStateMachine {
    WState currentState;

    public void SetState(WState newState) {
        if (currentState != null) {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter();
    }

    public void Process() {
        if (currentState != null) {
            currentState.Execute();
        }
    }
}

public abstract class WState : IState {
    protected int Length { get; private set; }
    protected Weapon Weapon { get; private set; }
    protected Texture2D Texture { get; private set; }
    protected WeaponAnimationFrame Frame { get; private set; }
    protected WeaponAnimation Animation { get; private set; }
    protected int EWState { get; private set; }
    protected WState NextState { get; private set; }
    protected Action Action { get; private set; } = null;

    protected int FrameCounter { get; private set; }

    public virtual void Enter() {
        FrameCounter = 0;
        if (EWState != -1) {
            Weapon.SetWeaponState(EWState);
        }
        
        if (Frame != null) { // For single-frame stuff
            Weapon.Player.SetViewSpriteFrame(Frame);
            if (Frame.Sound != null) {
                Weapon.Player.WeaponMiscAudio.Stream = Frame.Sound;
                Weapon.Player.WeaponMiscAudio.Play();
            }
        }
        if (Animation != null) {
            Weapon.Player.FetchAndPlayAnimation(Animation);
        }
        Action?.Invoke();
    }

    public virtual void Execute() {
        if (Length > 0) {
            if (FrameCounter >= Length && NextState != null) {
                Weapon.WStateMachine?.SetState(NextState);
            }
            FrameCounter++;
        }
    }

    public virtual void Exit() {

    }

    public virtual void ConfigureState(int frames, Weapon weapon, WeaponAnimationFrame frame, Action? action, int wState, WState? next) {
        Length = frames;
        Weapon = weapon;
        Frame = frame;
        Animation = null;
        Action = action;
        EWState = wState;
        NextState = next;
    }

    public virtual void ConfigureState(Weapon weapon, WeaponAnimation animation, Action? action, int wState, WState? next) {
        Weapon = weapon;
        Animation = animation;
        Length = Animation.TotalLength;
        Frame = null;
        Action = action;
        EWState = wState;
        NextState = next;
    }
}
