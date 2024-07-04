using Godot;
using System;

public partial class EnemyBase : Actor {
	public EStateMachine EStateMachine { get; protected set; }
	public EnemyState EnemyState { get; protected set; }
	public MoveState MoveState { get; set; }

	protected class EIdleState : EState {}
    protected class EChaseState : EState {}
    protected class EAtkState : EState {}
	protected class EAltAtkState : EState {}
	protected class EStunState : EState {}
    protected class EDeathState : EState {}
	protected class ECorpseState : EState {}

	protected EIdleState IdleState { get; set; }
    protected EChaseState ChaseState { get; set; }
    protected EAtkState AtkState { get; set; }
    protected EAltAtkState AltAtkState { get; set; }
	protected EStunState StunState { get; set; }
    protected EDeathState DeathState { get; set; }
	protected ECorpseState CorpseState { get; set; }

	public float Acceleration { get; protected set; }
	public float TurnRate { get; protected set; }

	public float PainChance { get; protected set; }

	public bool IsMoving { get; protected set; }
	public bool CanMove { get; protected set; }

    public Sprite3D VisSprite { get; protected set; }
	public int SpriteAnimFrame { get; protected set; } = 0;
	public int SpriteRotation { get; protected set; } = 0;
	public bool SpriteIsFlipped { get; protected set; }

	public RayCast3D AttackRay { get; protected set; }
	public RayCast3D SightRay { get; protected set; }
	public RayCast3D PathCast { get; protected set; }
	public Vector3 GoalPos { get; set; }
	public float GoalAngle { get; set; }
	public float SightRange { get; protected set; }

	protected bool spriteWillUpdate;
	protected bool spriteWillRotate;
	protected float spriteYOffset;

	protected bool isActive = false;

	public Actor Target { get; set; }
	public Vector3 TargetLocation { get; set; }

	
	public EnemyAnimation CurAnim { get; protected set; }
    public int CurAnimFrame { get; protected set; }
    public int CurAnimTick { get; protected set; }

	public AudioStreamPlayer3D VoiceAudio { get; protected set; }

	public Label3D DebugLabel { get; protected set; }



	public Vector3 GoalVector { get; set; }
	public float RelativeRotation { get; set; }

	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public override void _Ready() {

    }

    public override void _PhysicsProcess(double delta) {

	}

	protected void ConnectNodes() {
		ColShape = GetNode<CollisionShape3D>("CollisionShape3D");
        VisSprite = GetNode<Sprite3D>("VisSprite");
		VoiceAudio = GetNode<AudioStreamPlayer3D>("VoiceAudio");
		PathCast = GetNode<RayCast3D>("PathCast");
		AttackRay = GetNode<RayCast3D>("AttackRay");
		SightRay = GetNode<RayCast3D>("SightRay");
        DebugLabel = GetNode<Label3D>("DebugStats");

		stairUpCast = GetNode<RayCast3D>("StairUpCast");
		stairDownCast = GetNode<RayCast3D>("StairDownCast");
	}

	public virtual void ProcessGroundMovement(double deltaTime) {

	}

	public void ProcessGravity(double deltaTime) {
		Vector3 velocity = Velocity;
		if (!IsOnFloor()) {
			velocity.Y -= gravity * (float)deltaTime;
		}
		else {
			justWasOnFloor = Engine.GetPhysicsFrames();
		}
		Velocity = velocity;
	}

	public void SetEnemyState(int estate) {
        EnemyState = (EnemyState)estate;
    }

	public virtual void EnterIdleState() {
		EStateMachine.SetState(IdleState);
	}

    public virtual void EnterChaseState() {
        EStateMachine.SetState(ChaseState);
    }

	public virtual void EnterAtkState() {
		EStateMachine.SetState(AtkState);
	}

	public virtual void EnterAltAtkState() {
		EStateMachine.SetState(AltAtkState);
	}

    public virtual void EnterDeathState() {
        EStateMachine.SetState(DeathState);
    }

	public void FetchAnimation(EnemyAnimation animation) {
		CurAnim = animation;
		CurAnimFrame = 0;
		CurAnimTick = 0;
	}

	public virtual void ProcessAnimation() {
		if (CurAnim != null) {
			if (CurAnimTick == 0) {
				SpriteAnimFrame = CurAnim.AnimFrames[CurAnimFrame].TextureFrame;

				if (CurAnim.AnimFrames[CurAnimFrame].Sound != null) {
					VoiceAudio.Stream = CurAnim.AnimFrames[CurAnimFrame].Sound;
					VoiceAudio.Play();
				}

				if (CurAnim.AnimFrames[CurAnimFrame].Action != EnemyAction.None) {
					ExecuteAction(CurAnim.AnimFrames[CurAnimFrame].Action);
				}
			}

			if (CurAnim.AnimFrames[CurAnimFrame].FrameLength != 0) {
				CurAnimTick++;
			}

			if (CurAnimTick == CurAnim.AnimFrames[CurAnimFrame].FrameLength) {
				if (CurAnimFrame + 1 < CurAnim.AnimFrames.Length) {
					CurAnimFrame++;
					CurAnimTick = 0;
				}
				else {
					CurAnim = null;
				}
			}
		}
	}

	public virtual void ExecuteAction(EnemyAction action) {

	}

	public void SetSpriteRotation() {
		if (spriteWillRotate) {
			float dotProductX = AI.GetDotProdX(this, Game.Player);
			float dotProductZ = AI.GetDotProdZ(this, Game.Player);

			SpriteIsFlipped = false;
			
			if (dotProductZ > 0.80f) {
				SpriteRotation = 0;
			}
			else if (dotProductZ < -0.80f) {
				SpriteRotation = 4;
			}
			else {
				if (dotProductX > 0) {
					SpriteIsFlipped = true;
				}

				if (dotProductZ > 0.30f) {
					SpriteRotation = 1;
				}
				else if (dotProductZ > -0.30f) {
					SpriteRotation = 2;
				}
				else {
					SpriteRotation = 3;
				}
			}
		}
    }

	public void AdjustSpriteYOffset() {
		spriteYOffset = -((ColHeight * 128f) - VisSprite.Texture.GetHeight()) / 2;
		VisSprite.Offset = VisSprite.Offset with { Y = spriteYOffset };
	}

	public virtual void Activate() {
		isActive = true;
		//AI.SetTargetCharacter(this, Game.Player);
	}

	public virtual void WakeUp() {
		EnterChaseState();
	}

	public virtual void Attack() {

	}

	public override void TakeDamage(Actor? source, int damage) {
		if ((Health - damage) <= 0) {
			Health = 0;
			MoveState = MoveState.None;
			SetCorpseCollision();
			EnterDeathState();
		}
		else {
			Health -= damage;
			AI.WakeUpAndTargetSource(this, source, damage);

			if (EnemyState != EnemyState.Stun && PainChance >= Utils.RandomFloat(1f)) {
				EStateMachine.SetState(StunState);
			}
		}
	}

	public virtual void OnDeath() {

	}

	// Probably smarter to replace the enemy with a generic corpse entity that inherits its last death animation frame
	// Alternatively, alter the script to make the collision very short to avoid tanking projectiles unintentionally - Done, executed in individual death script
	public void SetCorpseCollision() {
		SetCollisionMaskValue(4, false);
		SetCollisionLayerValue(5, false);
		SetCollisionLayerValue(6, true);
	}

	public void UpdateDebugLabel() {
		DebugLabel.Text = $"Health: {Health} / {StartingHealth}\nEState: {EnemyState}\nMState: {MoveState}\nRotationY: {RotationDegrees.Y:F3}\nGRotationY: {GlobalRotationDegrees.Y}\nBasisZ: {Transform.Basis.Z:F3}\nGoal Angle: {GoalAngle:F3}";
	}

	public void AddExtraLabelInfo(string desc, float val) {
		DebugLabel.Text += $"\n{desc}: {val:F3}";
	}
}
