using Godot;
using System;

public partial class bsPlayer : CharacterBody3D {
	public const float Speed = 7.0f;
	public const float JumpVelocity = 4f;

	private float cameraRotaX = 0f;
	private float cameraRotaY = 0f;

	private Camera3D playerCamera;

	private int[] playerAmmo = new int[(int)Ammotype.NoOfTypes];
	private Weapon[] weaponInventory = new Weapon[1];
	private Weapon activeWeapon;
	private TextureRect activeWeaponSprite;


	private Vector2 mouseRelative;
	private bool mouseCaptured;

	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public override void _Ready() {
		playerCamera = GetNode<Camera3D>("PlayerCamera");
		activeWeaponSprite = GetNode<TextureRect>("bsPlayerWeapon/BottomAnchor/BottomPivot/WeaponSprite");
		Input.MouseMode = Input.MouseModeEnum.Captured;
		mouseCaptured = true;

		TestGiveWeapon();
	}

	private async void TestGiveWeapon() {
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		weaponInventory[0] = new W_Pitchfork(this);
		activeWeapon = weaponInventory[0];
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
		if (mouseCaptured) {
			ProcessCamera(delta);
		}
		ProcessMovement(delta);

		if (activeWeapon.WeaponState == WeaponState.ReadyState) {
			if (Input.IsActionPressed("leftclick") && (playerAmmo[(int)activeWeapon.AmmoType] > activeWeapon.AmmoReqPri || activeWeapon.AmmoType == Ammotype.A_None)) {
				activeWeapon.EnterAtkState();
			}
			else if (Input.IsActionPressed("rightclick") && (playerAmmo[(int)activeWeapon.AmmoType] > activeWeapon.AmmoReqSec || activeWeapon.AmmoType == Ammotype.A_None)) {
				activeWeapon.EnterAltState();
			}
			else {
				WeaponReady();
			}
		}
		activeWeapon.WStateMachine.Process();
	}

	public static void WeaponReady() {

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

	public void SetActiveWeaponSprite(Texture2D sprite) {
		activeWeaponSprite.Texture = sprite;
	}
}
