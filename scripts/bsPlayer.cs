using Godot;
using System;
using System.Linq;
using System.Numerics;
using Vector2 = Godot.Vector2;
using Vector3 = Godot.Vector3;

public partial class bsPlayer : CharacterBody3D {
	public const float Speed = 6.0f;
	public const float JumpVelocity = 4f;

	private int health;
	public int Health {
		get => health;
		set {
			health = value;
			PlayerHUD.HUDUpdatePlayerHealth(health);
		}
	}

	private float cameraRotaX = 0f;
	private float cameraRotaY = 0f;

	private Camera3D playerCamera;
	private RayCast3D vectorRay;

	public Ammunition[] PlayerAmmo = {
		new(0), new(50)
	};

	public Weapon[] WeaponInventory = new Weapon[(int)WeaponType.NoOfWeapons];
	private bool[] hasWeapon = new bool[(int)WeaponType.NoOfWeapons];
	public Weapon ActiveWeapon { get; set; }
	public int ActiveWeaponNum { get; set; }
	public bool SwitchingWeapon { get; private set; } = false;
	public int WeaponToSwitchTo { get; private set; } = 0;

	private Label label;
	public PlayerHUD PlayerHUD { get; set; }
	public Control ActiveWeaponControl { get; set; }
	public TextureRect ActiveWeaponSprite { get; private set; }

	public Control ActiveWeaponPivot { get; private set; }
	public Godot.Collections.Array<Node> ActiveWeaponSpriteNodes { get; private set; }

	public WeaponAnimation PriWepAnim { get; private set; }
	private int priWepAnimTick = 0;
	private int priWepAnimFrame = 0;

	public WeaponAnimation SecWepAnim { get; private set; }
	private int secWepAnimTick = 0;
	private int secWepAnimFrame = 0;

	public AudioStreamPlayer3D PriFireAudio { get; private set; }
	public AudioStreamPlayer3D AltFireAudio { get; private set; }
	public AudioStreamPlayer3D VoiceAudio { get; private set; }
	public AudioStreamPlayer3D WeaponMiscAudio { get; private set; }

	private Vector2 mouseRelative;
	private bool mouseCaptured;

	private Node3D helper;

	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public override void _Ready() {
		helper = GetParent().GetNode<Node3D>("RaytraceHelper");

		PlayerHUD = GetNode<PlayerHUD>("bsPlayerHUD");
		label = GetNode<Label>("bsPlayerHUD/Label");

		playerCamera = GetNode<Camera3D>("PlayerCamera");
		vectorRay = GetNode<RayCast3D>("PlayerCamera/InteractVector");

		ActiveWeaponControl = GetNode<Control>("bsPlayerWeapon");
		ActiveWeaponSprite = GetNode<TextureRect>("bsPlayerWeapon/BottomAnchor/WeaponSprite");

		ActiveWeaponPivot = GetNode<Control>("bsPlayerWeapon/BottomAnchor/WeaponPivot");
		ActiveWeaponSpriteNodes = GetNode<Node>("bsPlayerWeapon/BottomAnchor/WeaponPivot").GetChildren();

		PriFireAudio = GetNode<AudioStreamPlayer3D>("PrimaryFireAudio");
		AltFireAudio = GetNode<AudioStreamPlayer3D>("AltFireAudio");
		VoiceAudio = GetNode<AudioStreamPlayer3D>("PlayerVoiceAudio");
		WeaponMiscAudio = GetNode<AudioStreamPlayer3D>("WeaponMiscAudio");

		Input.MouseMode = Input.MouseModeEnum.Captured;
		mouseCaptured = true;

		Health = 100;
		TestGiveWeapon();
	}

	private async void TestGiveWeapon() {
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		PickUpNewWeapon(WeaponType.W_Pitchfork);
		PickUpNewWeapon(WeaponType.W_Shotgun);
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
		if (!SwitchingWeapon) {
			if (Input.IsActionJustPressed("wkey1")) {
				if (hasWeapon[0] && ActiveWeaponNum != 0) {
					SwitchingWeapon = true;
					WeaponToSwitchTo = 0;
				}
			}
			if (Input.IsActionJustPressed("wkey2")) {
				if (hasWeapon[1] && ActiveWeaponNum != 1) {
					SwitchingWeapon = true;
					WeaponToSwitchTo = 1;
				}
			}
		}
    }

    public override void _PhysicsProcess(double delta) {
		if (mouseCaptured) {
			ProcessCamera(delta);
		}
		ProcessMovement(delta);

		if (ActiveWeapon != null) {
			if (Input.IsActionPressed("leftclick")) {
				ActiveWeapon.PrimaryFire();
			}
			else if (Input.IsActionPressed("rightclick")) {
				ActiveWeapon.AltFire();
			}
			else if (ActiveWeapon.WeaponState == WeaponState.ReadyState && SwitchingWeapon) {
				ActiveWeapon.EnterLowerState();
				SwitchingWeapon = false;
			}
			else {
				WeaponReady();
			}
		}
		ActiveWeapon.WStateMachine.Process();

		ProcessAnimation();
		ProcessSecondaryAnimation();

		TestUpdateHUDText();
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

		if (!IsOnFloor()) {
			velocity.Y -= gravity * (float)deltaTime;
		}

		if (Input.IsActionJustPressed("jump") && IsOnFloor()) {
			velocity.Y = JumpVelocity;
		}
			
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
	
	public void FetchAndPlayAnimation(WeaponAnimation animation) {
		PriWepAnim = animation;
		priWepAnimFrame = 0;
		priWepAnimTick = 0;
	}

	public void ProcessAnimation() {
		if (PriWepAnim != null) {
			if (priWepAnimTick == 0) {
				SetViewSpriteFrame(PriWepAnim.AnimFrames[priWepAnimFrame]);
				if (PriWepAnim.AnimFrames[priWepAnimFrame].Sound != null) {
					WeaponMiscAudio.Stream = PriWepAnim.AnimFrames[priWepAnimFrame].Sound;
					WeaponMiscAudio.Play();
				}
			}

			if (PriWepAnim.AnimFrames[priWepAnimFrame].FrameLength != 0) {
				priWepAnimTick++;
			}

			if (priWepAnimTick == PriWepAnim.AnimFrames[priWepAnimFrame].FrameLength) {
				if (priWepAnimFrame + 1 < PriWepAnim.AnimFrames.Length) {
					priWepAnimFrame++;
					priWepAnimTick = 0;
				}
				else {
					PriWepAnim = null;
				}
			}
		}
	}

	public void FetchAndPlaySecondaryAnimation(WeaponAnimation animation) {
		SecWepAnim = animation;
		secWepAnimFrame = 0;
		secWepAnimTick = 0;
	}

	public void ProcessSecondaryAnimation() {
		if (SecWepAnim != null) {
			if (secWepAnimTick == 0) {
				SetViewSpriteFrame(SecWepAnim.AnimFrames[secWepAnimFrame]);
				if (SecWepAnim.AnimFrames[secWepAnimFrame].Sound != null) {
					WeaponMiscAudio.Stream = SecWepAnim.AnimFrames[secWepAnimFrame].Sound;
					WeaponMiscAudio.Play();
				}
			}

			if (SecWepAnim.AnimFrames[secWepAnimFrame].FrameLength != 0) {
				secWepAnimTick++;
			}

			if (secWepAnimTick == SecWepAnim.AnimFrames[secWepAnimFrame].FrameLength) {
				if (secWepAnimFrame + 1 < SecWepAnim.AnimFrames.Length) {
					secWepAnimFrame++;
					secWepAnimTick = 0;
				}
				else {
					SecWepAnim = null;
				}
			}
		}
	}

	public void SetActiveWeaponSprite(Texture2D sprite) {
		ActiveWeaponSprite.Texture = sprite;
	}

	public void SetViewSpriteFrame(WeaponAnimationFrame frame) {
		foreach (WeaponAnimationLayer animlayer in frame.WeaponAnimationLayers) {
			TextureRect textureRect = ActiveWeaponSpriteNodes[animlayer.Layer] as TextureRect;

			textureRect.Texture = animlayer.Texture;

			textureRect.OffsetLeft = animlayer.Offset.Left;
        	textureRect.OffsetRight = animlayer.Offset.Right;
        	textureRect.OffsetTop = animlayer.Offset.Top;
			textureRect.OffsetBottom = animlayer.Offset.Bottom;

			textureRect.PivotOffset = textureRect.Size / 2;

			textureRect.RotationDegrees = animlayer.RotationDeg;
			textureRect.FlipH = animlayer.DoFlipH;
		}
	}

	public void PickUpNewWeapon(WeaponType type) {
		switch (type) {
			case WeaponType.W_Pitchfork:
				if (WeaponInventory[0] == null) {
					WeaponInventory[0] = new W_Pitchfork(this);
					hasWeapon[0] = true;
				}
				break;
			
			case WeaponType.W_Shotgun:
				if (WeaponInventory[1] == null) {
					WeaponInventory[1] = new W_Shotgun(this);
					hasWeapon[1] = true;
				}
				break;
			
			default:
				break;
		}

		GiveAmmo(WeaponInventory[(int)type].AmmoType, WeaponInventory[(int)type].AmmoOnPickup);

		if (ActiveWeapon == null) {
			BringUpNewWeapon(type);
		}
	}

	public void BringUpNewWeapon(WeaponType type) {
		ActiveWeaponControl.SetPosition(ActiveWeaponControl.Position with { Y = Weapon.WeaponOffsetBottom });

		ActiveWeapon = WeaponInventory[(int)type];
		ActiveWeaponNum = (int)type;

		ActiveWeaponPivot.OffsetLeft = Sprites.Spr_Wep_Offset[ActiveWeaponNum].Left;
        ActiveWeaponPivot.OffsetRight = Sprites.Spr_Wep_Offset[ActiveWeaponNum].Right;
        ActiveWeaponPivot.OffsetTop = Sprites.Spr_Wep_Offset[ActiveWeaponNum].Top;
		ActiveWeaponPivot.OffsetBottom = Sprites.Spr_Wep_Offset[ActiveWeaponNum].Bottom;

		ActiveWeapon.EnterRaiseState();

		if (ActiveWeapon.AmmoType == Ammotype.None) {
			PlayerHUD.HUDUpdatePlayerAmmo(-1);
		}
		else {
			PlayerHUD.HUDUpdatePlayerAmmo(PlayerAmmo[(int)ActiveWeapon.AmmoType].Ammo);
		}

		if (type == WeaponType.W_Shotgun) {
			if (PlayerAmmo[(int)Ammotype.Shells].Ammo > 1) {
				(ActiveWeapon as W_Shotgun).Shells = 2;
            }
            else if (PlayerAmmo[(int)Ammotype.Shells].Ammo == 1) {
                (ActiveWeapon as W_Shotgun).Shells = 1;
            }
		}

		PriFireAudio.Stream = ActiveWeapon.PrimaryFireAudio;
		AltFireAudio.Stream = ActiveWeapon.AltFireAudio;
	}

	public void GiveAmmo(Ammotype ammotype, int amount) {
		PlayerAmmo[(int)ammotype].Ammo += amount;
		GD.Print($"Received {WeaponInventory[(int)ammotype].AmmoOnPickup} ammo of type: {WeaponInventory[(int)ammotype].AmmoType}");
	}

	public void FireHitscanAttack(HitscanAttack atk, float offsetX, float offsetY) {
		RayCast3D newVector = new();
		newVector.SetCollisionMaskValue(1, true);
		newVector.SetCollisionMaskValue(2, true);
		newVector.SetCollisionMaskValue(5, true);
		playerCamera.AddChild(newVector);
		
		Vector3 target = new(offsetX, offsetY, -1f);
		target *= atk.Range;
		newVector.TargetPosition = target;
		newVector.ForceRaycastUpdate();

		if (newVector.IsColliding() && newVector.GetCollider().IsClass("Node3D")) {
			Node3D collider = newVector.GetCollider() as Node3D;
			if (collider.IsInGroup("Enemy")) {
				(collider as EnemyBase).TakeDamage(this, atk.Damage);
			}
		}

		newVector.SetProcess(false);
		newVector.SetPhysicsProcess(false);
		playerCamera.RemoveChild(newVector);
		helper.AddChild(newVector);
		//newVector.Free();
	}

	public void TestClearHelper() {
		foreach (var item in helper.GetChildren()) {
			item.Free();
		}
		helper.GlobalPosition = playerCamera.GlobalPosition;
		helper.Rotation = playerCamera.Rotation + Rotation;
	}

	public void TestUpdateHUDText() {
		label.Text = $"Pos: {GlobalPosition.ToString("N4")}\nFrame: {priWepAnimFrame}, / Tick: {priWepAnimTick}\nSFrame: {secWepAnimFrame}, / STick: {secWepAnimTick}";
		if (ActiveWeaponNum == 1) {
			label.Text += $"\nLoaded shells: {(ActiveWeapon as W_Shotgun).Shells}";
		}
	}
}
