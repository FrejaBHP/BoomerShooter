using Godot;
using Godot.Collections;
using System;

public partial class SlidingDoor : StaticBody3D {
	[Export]
	Dictionary properties;

	private bool isOpening = false;
	private bool alreadyOpened = false;
	private float moveDist = 0f;
	private Vector3 endPos;

	public override void _Ready() {
		AddToGroup("DoorSliding");
		GetChild<MeshInstance3D>(0).IgnoreOcclusionCulling = true;
	}

    public override void _PhysicsProcess(double delta) {
		if (isOpening) {
			GlobalPosition = GlobalPosition.MoveToward(endPos, moveDist);

			if (GlobalPosition == endPos) {
				isOpening = false;
				GD.Print("Door finished opening!");
			}
		}
    }

    public void Open() {
		if (!alreadyOpened) {
			moveDist = properties["speed"].AsSingle() / 32f;

			if (properties["angle"].AsSingle() >= 0) {
				Vector3 angleVector = new(properties["distance"].AsSingle() / 32, 0, 0);
				endPos = GlobalPosition + angleVector.Rotated(Vector3.Up, ((float)Math.PI / 180) * properties["angle"].AsSingle());
			}
			else if (properties["angle"].AsSingle() == -1) {
				Vector3 angleVector = new(0, properties["distance"].AsSingle() / 32, 0);
				endPos = GlobalPosition + angleVector;
			}
			else if (properties["angle"].AsSingle() == -2) {
				Vector3 angleVector = new(0, -properties["distance"].AsSingle() / 32, 0);
				endPos = GlobalPosition + angleVector;
			}

			GD.Print($"StartPos: {GlobalPosition}, EndPos: {endPos}");
			isOpening = true;
			alreadyOpened = true;
		}
	}
}
