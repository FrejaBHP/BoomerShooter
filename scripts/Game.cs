using Godot;
using System;

public partial class Game : Node3D {
	// Called when the node enters the scene tree for the first time.
	PackedScene player = GD.Load<PackedScene>("res://bsPlayer.tscn");
	PackedScene axezombie = GD.Load<PackedScene>("res://axe_zombie.tscn");
	Marker3D marker3D;
	Marker3D enemymarker3D;

	public static bsPlayer Player { get; private set; }

	public override void _Ready() {
		Sprites.IndexWeaponSprites();
		Sprites.IndexWeaponOffsets();
		WAnimations.IndexWeaponAnimations();
		EAnimations.IndexEnemyAnimations();
		
		marker3D = GetNode<Marker3D>("Marker3D");
		bsPlayer playerInstance = player.Instantiate() as bsPlayer;
		AddChild(playerInstance);
		playerInstance.Position = marker3D.Position;
		
		Player = playerInstance;

		enemymarker3D = GetNode<Marker3D>("EnemyMarker");
		AxeZombie az = axezombie.Instantiate() as AxeZombie;
		AddChild(az);
		az.Position = enemymarker3D.Position;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
	
	}
}
