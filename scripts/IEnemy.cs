using Godot;

public interface IEnemy {
    public void SetSprite();

    public void EnterIdleState();
    public void EnterChaseState();
}
