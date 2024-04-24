using Godot;
using System;

public partial class bsPlayer : CharacterBody3D {
	public const float Speed = 7.0f;
	public const float JumpVelocity = 4f;

	private float cameraRotaX = 0f;
	private float cameraRotaY = 0f;

	private Camera3D playerCamera;
	private Vector2 mouseRelative;
	private bool mouseCaptured;

	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public override void _Ready() {
		playerCamera = GetNode<Camera3D>("PlayerCamera");
		Input.MouseMode = Input.MouseModeEnum.Captured;
		mouseCaptured = true;
	}

	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseMotion motion) {
			mouseRelative = motion.Relative;
		}
		if (Input.IsActionJustPressed("esc")) {
			if (mouseCaptured) {
				Input.MouseMode = Input.MouseModeEnum.Visible;
				mouseCaptured = false;
			}
			else {
				Input.MouseMode = Input.MouseModeEnum.Captured;
				mouseCaptured = true;
			}
		}
    }

    public override void _PhysicsProcess(double delta) {
		ProcessCamera(delta);
		ProcessMovement(delta);
	}

	private void ProcessCamera(double deltaTime) {
		cameraRotaX -= mouseRelative.X * 0.5f * (float)deltaTime;
		cameraRotaY -= mouseRelative.Y * 0.5f * (float)deltaTime;
		cameraRotaY = Mathf.Clamp(cameraRotaY, -1.3f, 1.3f);
		mouseRelative = Vector2.Zero;

		Transform3D transform = playerCamera.Transform;
		transform.Basis = Basis.Identity;
		playerCamera.Transform = transform;

		Transform3D ptransform = Transform;
		ptransform.Basis = Basis.Identity;
		Transform = ptransform;

		RotateObjectLocal(Vector3.Up, cameraRotaX);
		playerCamera.RotateObjectLocal(Vector3.Right, cameraRotaY);
	}

	private void ProcessMovement(double deltaTime) {
		Vector3 velocity = Velocity;

		if (!IsOnFloor())
			velocity.Y -= gravity * (float)deltaTime;

		if (Input.IsActionJustPressed("jump") && IsOnFloor())
			velocity.Y = JumpVelocity;

		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero) {
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else {
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
