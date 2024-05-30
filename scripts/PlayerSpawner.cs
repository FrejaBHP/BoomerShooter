using Godot;
using Godot.Collections;
using System;

public partial class PlayerSpawner : Node3D {
    public static readonly PackedScene player = GD.Load<PackedScene>("res://bsPlayer.tscn");

	public override void _Ready() {
        AddToGroup("PlayerSpawner");
	}

    public void SpawnPlayer() {
        bsPlayer p = player.Instantiate() as bsPlayer;
        Game.PlayersNode.AddChild(p);
        p.GlobalPosition = GlobalPosition;
        Game.Player = p;

        QueueFree();
    }
}
