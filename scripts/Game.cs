using Godot;
using System;

public partial class Game : Node3D {
	PackedScene player = GD.Load<PackedScene>("res://bsPlayer.tscn");
	PackedScene axezombie = GD.Load<PackedScene>("res://axe_zombie.tscn");
	Marker3D marker3D;
	Marker3D enemymarker3D;
	Marker3D enemymarker3D2;

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

		enemymarker3D2 = GetNode<Marker3D>("EnemyMarker2");
		AxeZombie az1 = axezombie.Instantiate() as AxeZombie;
		AddChild(az1);
		az1.Position = enemymarker3D2.Position;
	}

	public override void _Process(double delta) {
	
	}
}
