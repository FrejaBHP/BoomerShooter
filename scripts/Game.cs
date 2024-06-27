using Godot;
using System;
using System.Linq;

public partial class Game : Node3D {
	public static Node3D EnemiesNode { get; private set; }
	public static Node3D PlayersNode { get; private set; }
	public static Node3D EntitiesNode { get; private set; }

	public static bsPlayer Player { get; set; }

	public override void _Ready() {
		EnemiesNode = GetNode<Node3D>("Enemies");
		PlayersNode = GetNode<Node3D>("Players");
		EntitiesNode = GetNode<Node3D>("Entities");

		Sprites.IndexSpriteArrays();
		WAnimations.IndexWeaponAnimations();
		EAnimations.IndexEnemyAnimations();
		MiscAnimations.IndexMiscAnimations();

		LoadMap(); // For later
	}

	private void LoadMap() {
		IterateEntities();
	}

	private void IterateEntities() {
		var map = GetNode("Map01");
		foreach (Node child in map.GetChildren()) {
			if (child.IsInGroup("Spawner")) {
				(child as Spawner).SpawnEnemy();
			}
			else if (child.IsInGroup("PlayerSpawner")) {
				(child as PlayerSpawner).SpawnPlayer();
			}
			else if (child.IsInGroup("DoorHinge")) {
				(child as DoorHinge).GetAndReparentDoors();
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

	private void ConnectTriggers() {

	}
}
