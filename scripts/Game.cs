using Godot;
using System;

public partial class Game : Node3D {
	// Called when the node enters the scene tree for the first time.
	PackedScene player = GD.Load<PackedScene>("res://bsPlayer.tscn");
	Marker3D marker3D;

	public override void _Ready() {
		WeaponSprites.IndexWeaponSprites();
		WeaponSprites.IndexWeaponOffsets();
		WAnimCollection.IndexAnimations();
		
		marker3D = GetNode<Marker3D>("Marker3D");
		bsPlayer playerInstance = player.Instantiate() as bsPlayer;
		AddChild(playerInstance);
		playerInstance.Position = marker3D.Position;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
	
	}
}
