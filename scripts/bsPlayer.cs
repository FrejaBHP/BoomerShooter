using Godot;
using System;
using System.Linq;
using System.Numerics;
using Vector2 = Godot.Vector2;
using Vector3 = Godot.Vector3;

public partial class bsPlayer : Actor {
	private readonly PackedScene tntbundle = GD.Load<PackedScene>("res://tnt_player.tscn");

	public const float MaxVerticalSpeed = 8.0f; // pesky tnt jumps
	public const float JumpVelocity = 4f;

	private int health;
	public override int Health {
		get => health;
		protected set {
			health = value;
			PlayerHUD.HUDUpdatePlayerHealth(health);
		}
	}

	private float cameraRotaX = 0f;
	private float cameraRotaY = 0f;

	public Camera3D PlayerCamera { get; private set; }
	private RayCast3D useRay;

	public Ammunition[] PlayerAmmo = {
		// Max ammo
		new(0), // PF
		new(100), // Shotgun
		new(500), // Tommy
		new(50) // Dynamite
	};

	public Weapon[] WeaponInventory { get; private set; } = new Weapon[(int)WeaponType.NoOfWeapons];
	public bool[] HasWeapon { get; private set; } = new bool[(int)WeaponType.NoOfWeapons];
	public Weapon ActiveWeapon { get; set; }
	public int ActiveWeaponNum { get; set; }
	public bool SwitchingWeapon { get; private set; } = false;
	public int WeaponToSwitchTo { get; private set; } = 0;

	private Label label;
	public PlayerHUD PlayerHUD { get; private set; }

	public float WeaponYOffset { get; set; } = 0f;
	public Control ActiveWeaponControl { get; private set; }
	public Control ActiveWeaponView { get; private set; }
	public Control ActiveWeaponAnchor { get; private set; }
	public Control ActiveWeaponPivot { get; private set; }
	public Godot.Collections.Array<Node> ActiveWeaponSpriteNodes { get; private set; }

	private ulong modGameTick = 0;
	private float sineSway;
	private float cosineSway;

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
	public AudioStreamPlayer3D MiscAudio { get; private set; }

	private Vector2 mouseRelative;
	private bool mouseCaptured;

	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public override void _Ready() {
		PlayerHUD = GetNode<PlayerHUD>("bsPlayerHUD");
		label = GetNode<Label>("bsPlayerHUD/Label");

		ColShape = GetNode<CollisionShape3D>("CollisionShape3D");
		ColHeight = (ColShape.Shape as CapsuleShape3D).Height;

		PlayerCamera = GetNode<Camera3D>("PlayerCamera");
		useRay = GetNode<RayCast3D>("PlayerCamera/InteractVector");
		useRay.Enabled = false;

		ActiveWeaponControl = GetNode<Control>("bsPlayerWeapon");
		ActiveWeaponView = GetNode<Control>("bsPlayerWeapon/WeaponView");
		ActiveWeaponAnchor = GetNode<Control>("bsPlayerWeapon/WeaponView/BottomAnchor");
		ActiveWeaponPivot = GetNode<Control>("bsPlayerWeapon/WeaponView/BottomAnchor/WeaponPivot");
		ActiveWeaponSpriteNodes = GetNode<Node>("bsPlayerWeapon/WeaponView/BottomAnchor/WeaponPivot").GetChildren();

		PriFireAudio = GetNode<AudioStreamPlayer3D>("PrimaryFireAudio");
		AltFireAudio = GetNode<AudioStreamPlayer3D>("AltFireAudio");
		VoiceAudio = GetNode<AudioStreamPlayer3D>("PlayerVoiceAudio");
		WeaponMiscAudio = GetNode<AudioStreamPlayer3D>("WeaponMiscAudio");
		MiscAudio = GetNode<AudioStreamPlayer3D>("MiscAudio");

		stairUpCast = GetNode<RayCast3D>("StairUpCast");
		stairDownCast = GetNode<RayCast3D>("StairDownCast");

		Input.MouseMode = Input.MouseModeEnum.Captured;
		mouseCaptured = true;

		MaxSpeed = 5.0f;
		StartingHealth = 100;
		Health = StartingHealth;
		SurfaceType = SurfaceType.Flesh;
		
		PickUpNewWeapon(WeaponType.W_Pitchfork);
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
				if (CanSwitchToWeapon(WeaponType.W_Pitchfork)) {
					SwitchingWeapon = true;
					WeaponToSwitchTo = 0;
				}
			}
			else if (Input.IsActionJustPressed("wkey2")) {
				if (CanSwitchToWeapon(WeaponType.W_Shotgun)) {
					SwitchingWeapon = true;
					WeaponToSwitchTo = 1;
				}
			}
			else if (Input.IsActionJustPressed("wkey3")) {
				if (CanSwitchToWeapon(WeaponType.W_TommyGun)) {
					SwitchingWeapon = true;
					WeaponToSwitchTo = 2;
				}
			}
			else if (Input.IsActionJustPressed("wkey4")) {
				if (CanSwitchToWeapon(WeaponType.W_DynamiteReg)) {
					SwitchingWeapon = true;
					WeaponToSwitchTo = 3;
				}
			}
		}

		if (Input.IsActionJustPressed("use")) {
			Interact();
		}
    }

	private void Interact() {
		useRay.ForceRaycastUpdate();

		if (useRay.IsColliding()) {
			if (useRay.GetCollider().IsClass("Area3D")) {
				Area3D colBody = useRay.GetCollider() as Area3D;
				if (colBody.IsInGroup("Trigger")) {
					colBody.Call("TriggerTargets");
				}
			}
			else if (useRay.GetCollider().IsClass("StaticBody3D")) {
				StaticBody3D colBody = useRay.GetCollider() as StaticBody3D;
				if (colBody.GetCollisionLayerValue(2)) {
					colBody.Call("Trigger");
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
			if (mouseCaptured) {
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
				else if (Input.IsActionJustReleased("leftclick")) {
					ActiveWeapon.PrimaryFireReleased();
				}
				else {
					WeaponReady();
				}
			}
			ActiveWeapon.WStateMachine.Process();
		}

		ProcessAnimation();
		ProcessSecondaryAnimation();

		ProcessWeaponSway();

		//TestUpdateHUDText();
	}

	public static void WeaponReady() {

	}

	private void TestCameraShake() {
		
	}

	private void ProcessWeaponSway() {
		modGameTick = (Engine.GetPhysicsFrames() % 120) * 3;

		if (Velocity != Vector3.Zero) {
			sineSway = (float)Math.Sin(modGameTick * (Math.PI / 180f));

			Vector2 groundVelocity = new(Velocity.X, Velocity.Z);
			sineSway *= groundVelocity.Length() / MaxSpeed;

			if (IsOnFloor()) {
				cosineSway = (float)Math.Cos(modGameTick * (Math.PI / 180f));
				cosineSway *= groundVelocity.Length() / MaxSpeed;

				if (cosineSway < 0) {
					cosineSway = -cosineSway;
				}
			}
			else {
				cosineSway = Math.Clamp(Velocity.Y, -2f, 2f);
			}
			ActiveWeaponControl.Position = ActiveWeaponControl.Position.Lerp(ActiveWeaponControl.Position with { X = sineSway * 33f, Y = cosineSway * 33f}, 0.25f);
		}
		else {
			ActiveWeaponControl.Position = ActiveWeaponControl.Position.Lerp(Vector2.Zero, 0.1f);
		}
	}

	private void ProcessCamera(double deltaTime) {
		cameraRotaX -= mouseRelative.X * 0.4f * (float)deltaTime;
		cameraRotaY -= mouseRelative.Y * 0.4f * (float)deltaTime;
		cameraRotaY = Mathf.Clamp(cameraRotaY, -1.3f, 1.3f);
		mouseRelative = Vector2.Zero;

		Transform3D transform = PlayerCamera.Transform;
		transform.Basis = Basis.Identity;
		PlayerCamera.Transform = transform;

		Transform3D ptransform = Transform;
		ptransform.Basis = Basis.Identity;
		Transform = ptransform;

		RotateObjectLocal(Vector3.Up, cameraRotaX);
		PlayerCamera.RotateObjectLocal(Vector3.Right, cameraRotaY);
	}

	private void ProcessMovement(double deltaTime) {
		Vector3 velocity = Velocity;

		if (!IsOnFloor()) {
			velocity.Y -= gravity * (float)deltaTime;
		}
		else {
			justWasOnFloor = Engine.GetPhysicsFrames();
		}

		if (Input.IsActionJustPressed("jump") && IsOnFloor()) {
			velocity.Y = JumpVelocity;
		}
			
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero) {
			velocity.X = direction.X * MaxSpeed;
			velocity.Z = direction.Z * MaxSpeed;
		}
		else {
			velocity.X = Mathf.MoveToward(Velocity.X, 0, MaxSpeed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, MaxSpeed);
		}

		velocity.Y = Math.Clamp(velocity.Y, -MaxVerticalSpeed, MaxVerticalSpeed);
		Velocity = velocity;

		if (!TryClimbStair(deltaTime)) {
			MoveAndSlide();
			TryDescendStair();
		}
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
				
				if (PriWepAnim.AnimFrames[priWepAnimFrame].Action != WeaponAction.None) {
					ActiveWeapon.ExecuteAction(PriWepAnim.AnimFrames[priWepAnimFrame].Action);
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

				if (SecWepAnim.AnimFrames[secWepAnimFrame].Action != WeaponAction.None) {
					ActiveWeapon.ExecuteAction(SecWepAnim.AnimFrames[secWepAnimFrame].Action);
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
				if (!HasWeapon[0]) {
					WeaponInventory[0] = new W_Pitchfork(this);
					HasWeapon[0] = true;
				}
				break;
			
			case WeaponType.W_Shotgun:
				if (!HasWeapon[1]) {
					WeaponInventory[1] = new W_Shotgun(this);
					HasWeapon[1] = true;
				}
				break;
			
			case WeaponType.W_TommyGun:
				if (!HasWeapon[2]) {
					WeaponInventory[2] = new W_TommyGun(this);
					HasWeapon[2] = true;
				}
				break;
			
			case WeaponType.W_DynamiteReg:
				if (!HasWeapon[3]) {
					WeaponInventory[3] = new W_DynamiteReg(this);
					HasWeapon[3] = true;
				}
				break;
			
			default:
				break;
		}

		if (ActiveWeapon == null) {
			BringUpNewWeapon(type);
		}
		else if (ActiveWeaponNum == 0) {
			SwitchingWeapon = true;
			WeaponToSwitchTo = (int)type;
		}
	}

	public void SwitchToWeaponWithAmmo() {
		SwitchingWeapon = true;
		if (CanSwitchToWeapon(WeaponType.W_Shotgun)) {
			WeaponToSwitchTo = (int)WeaponType.W_Shotgun;
		}
		else if (CanSwitchToWeapon(WeaponType.W_TommyGun)) {
			WeaponToSwitchTo = (int)WeaponType.W_TommyGun;
		}
		else if (CanSwitchToWeapon(WeaponType.W_DynamiteReg)) {
			WeaponToSwitchTo = (int)WeaponType.W_DynamiteReg;
		}
		// If all else fails, trusty fork to the rescue!
		else {
			WeaponToSwitchTo = (int)WeaponType.W_Pitchfork;
		}
	}

	public void BringUpNewWeapon(WeaponType type) {
		ActiveWeaponView.SetPosition(ActiveWeaponView.Position with { Y = Weapon.WeaponOffsetBottom });

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
		// Enum values and array should be decoupled, 1:1 is bad and leads to future errors. 
		// Currently forces Shotgun == 1, therefore checks for ShotgunAmmo in Ammo[1]
		PlayerAmmo[(int)ammotype].Ammo += amount; 

		if (ActiveWeapon != null && ammotype == ActiveWeapon.AmmoType) {
			PlayerHUD.HUDUpdatePlayerAmmo(PlayerAmmo[(int)ammotype].Ammo);
		}
	}

	public void ThrowObject(ThrowableType throwable, float force) {
		float angle = 4f + (float)Math.Cbrt(force);
			angle *= (cameraRotaY + 1.3f) * 0.5f;

		Vector3 impulse = new(0, angle, -force);
			impulse = impulse.Rotated(Vector3.Up, cameraRotaX);

		Vector3 origin = PlayerCamera.GlobalPosition;
			origin.Y -= 0.1f;
		
		switch (throwable) {
			case ThrowableType.TNTBundle:
				TNTBundle tnt = tntbundle.Instantiate() as TNTBundle;
				tnt.AddCollisionExceptionWith(this);
				tnt.SetOwner(this);
				Game.EntitiesNode.AddChild(tnt);
				tnt.GlobalPosition = origin;

				tnt.ApplyCentralImpulse(impulse);
				break;
			
			default:
				break;
		}
	}

	private bool CanSwitchToWeapon(WeaponType type) {
		switch (type) {
			case WeaponType.W_Pitchfork:
				if (HasWeapon[0] && ActiveWeaponNum != 0) {
					return true;
				}
				else {
					return false;
				}
			
			case WeaponType.W_Shotgun:
				if (HasWeapon[1] && ActiveWeaponNum != 1 && PlayerAmmo[(int)Ammotype.Shells].Ammo != 0) {
					return true;
				}
				else {
					return false;
				}

			case WeaponType.W_TommyGun:
				if (HasWeapon[2] && ActiveWeaponNum != 2 && PlayerAmmo[(int)Ammotype.Bullets].Ammo != 0) {
					return true;
				}
				else {
					return false;
				}
			
			case WeaponType.W_DynamiteReg:
				if (HasWeapon[3] && ActiveWeaponNum != 3 && PlayerAmmo[(int)Ammotype.DynamiteReg].Ammo != 0) {
					return true;
				}
				else {
					return false;
				}

			// How would you even get here?
			default:
				return false;
		}
	}

	public void TestUpdateHUDText() {
		//label.Text = $"Pos: {GlobalPosition.ToString("N4")}\nFrame: {priWepAnimFrame}, / Tick: {priWepAnimTick}\nSFrame: {secWepAnimFrame}, / STick: {secWepAnimTick}";
		//label.Text = $"RotaX: {cameraRotaX:N4}\nRotaY: {cameraRotaY:N4}";
		//label.Text = $"Tick: {modGameTick} / 360\nSin: {sineSway:F3}\nCos: {cosineSway:F3}";
		label.Text = $"LRotationY: {RotationDegrees.Y:F3}\nGRotationY: {GlobalRotationDegrees.Y:F3}\nBasisZ: {Transform.Basis.Z:F3}\nGBasisZ: {GlobalTransform.Basis.Z:F3}";
		/*
		if (ActiveWeaponNum == 1) {
			label.Text += $"\nLoaded shells: {(ActiveWeapon as W_Shotgun).Shells}";
		}
		*/
	}
}
