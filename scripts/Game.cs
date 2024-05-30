using Godot;
using System;
using System.Linq;

public partial class Game : Node3D {
	public static Node3D EnemiesNode { get; private set; }
	public static Node3D PlayersNode { get; private set; }
	public static Node3D RayTraceHelper { get; private set; }

	public static bsPlayer Player { get; set; }

	public override void _Ready() {
		EnemiesNode = GetNode<Node3D>("Enemies");
		PlayersNode = GetNode<Node3D>("Players");
		RayTraceHelper = GetNode<Node3D>("RaytraceHelper");

		Sprites.IndexSpriteArrays();
		WAnimations.IndexWeaponAnimations();
		EAnimations.IndexEnemyAnimations();

		LoadMap(); // For later
	}

	private void LoadMap() {
		ActivateSpawners();
	}

	private void ActivateSpawners() {
		var map = GetNode("QodotMap");
		foreach (Node child in map.GetChildren()) {
			if (child.IsInGroup("Spawner")) {
				(child as Spawner).SpawnEnemy();
			}
			else if (child.IsInGroup("PlayerSpawner")) {
				(child as PlayerSpawner).SpawnPlayer();
			}
		}

		ActivateEnemies();
	}

	private void ActivateEnemies() {
		foreach (Node enemy in EnemiesNode.GetChildren()) {
			if (enemy.IsInGroup("Enemy")) {
				(enemy as EnemyBase).Activate();
			}
		}
	}

	public override void _Process(double delta) {
	
	}
}
