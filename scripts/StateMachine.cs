using System;
using System.Reflection;
using Godot;

public interface IState {
    public void Enter();
    public void Execute();
    public void Exit();
}

public partial class StateMachine {
    State currentState;

    public void SetState(State newState) {
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

public abstract class State : IState {
    protected int Length { get; private set; }
    protected Weapon Weapon { get; private set; }
    protected Texture2D Texture { get; private set; }
    protected int WState { get; private set; }
    protected State NextState { get; private set; }
    protected Action Action { get; private set; } = null;

    protected int FrameCounter { get; private set; }

    public virtual void Enter() {
        FrameCounter = 0;
        if (WState != -1) {
            Weapon.SetWeaponState(WState);
        }
        Weapon.Player.SetActiveWeaponSprite(Texture);
        Action?.Invoke();
    }

    public virtual void Execute() {
        if (Length > 0) {
            FrameCounter++;
            if (FrameCounter >= Length && NextState != null) {
                Weapon.WStateMachine?.SetState(NextState);
            }
        }
    }

    public virtual void Exit() {

    }

    public virtual void ConfigureState(int frames, Weapon weapon, Texture2D texture, Action? action, int wState, State? next) {
        Length = frames;
        Weapon = weapon;
        Texture = texture;
        Action = action;
        WState = wState;
        NextState = next;
    }
}
