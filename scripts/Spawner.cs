using Godot;
using Godot.Collections;
using System;

public partial class Spawner : Node3D {
    public static readonly PackedScene axeZombie = GD.Load<PackedScene>("res://enemies/axe_zombie.tscn");

	[Export]
	public EnemyType Type { get; private set; }

	public override void _Ready() {
        AddToGroup("Spawner");
	}

    public void SpawnEnemy() {
        switch (Type) {
            case EnemyType.AxeZombie:
                AxeZombie zombie = axeZombie.Instantiate() as AxeZombie;
                Game.EnemiesNode.AddChild(zombie);
                zombie.GlobalPosition = GlobalPosition;
                break;
            
            default:
                break;
        }

        QueueFree();
    }
}
