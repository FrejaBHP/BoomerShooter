using Godot;
using System;

public partial class EnemyBase : CharacterBody3D {
	public virtual EStateMachine EStateMachine { get; protected set; }
	public virtual EnemyState EnemyState { get; protected set; }

	protected class EIdleState : EState {}
    protected class EChaseState : EState {}
    protected class EAtkState : EState {}
	protected class EAltAtkState : EState {}
	protected class EStunState : EState {}
    protected class EDeathState : EState {}
	protected class ECorpseState : EState {}

	protected virtual EIdleState IdleState { get; }
    protected virtual EChaseState ChaseState { get; }
    protected virtual EAtkState AtkState { get; }
    protected virtual EAltAtkState AltAtkState { get; }
	protected virtual EStunState StunState { get; }
    protected virtual EDeathState DeathState { get; }
	protected virtual ECorpseState CorpseState { get; }

	public virtual float Speed { get; protected set; }
	public virtual int StartingHealth { get; protected set; }
	public virtual int Health { get; protected set; }

	public virtual CollisionShape3D ColShape { get; protected set; }

    public virtual Sprite3D VisSprite { get; protected set; }
	public virtual int SpriteAnimFrame { get; protected set; }
	public virtual int SpriteRotation { get; protected set; }
	public virtual bool SpriteIsFlipped { get; protected set; }
	
	protected bool spriteWillUpdate;
	protected bool spriteWillRotate;
	protected float spriteYOffset;

	protected bool isActive = false;
	public virtual CharacterBody3D Target { get; set; }
	public virtual Vector3 TargetLocation { get; set; }

	
	public virtual EnemyAnimation CurAnim { get; protected set; }
    public virtual int CurAnimFrame { get; protected set; }
    public virtual int CurAnimTick { get; protected set; }

	public virtual AudioStreamPlayer3D VoiceAudio { get; protected set; }

	public virtual Label3D DebugLabel { get; protected set; }


	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public override void _Ready() {

    }

    public override void _PhysicsProcess(double delta) {

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
			if (CurAnimFrame == 0) {
				if (CurAnim.AnimFrames[CurAnimFrame].Sound != null) {
					VoiceAudio.Stream = CurAnim.AnimFrames[CurAnimFrame].Sound;
					VoiceAudio.Play();
				}
			}
			
			if (CurAnimTick == 0) {
				SpriteAnimFrame = CurAnim.AnimFrames[CurAnimFrame].TextureFrame;
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
		spriteYOffset = -(((ColShape.Shape as CapsuleShape3D).Height * 128f) - VisSprite.Texture.GetHeight()) / 2;
		VisSprite.Offset = VisSprite.Offset with { Y = spriteYOffset };
	}

	public virtual void Activate() {
		isActive = true;
		//AI.SetTargetCharacter(this, Game.Player);
	}

	public virtual void WakeUp() {
		EnterChaseState();
	}

	public void ProcessGravity(double deltaTime) {
		Vector3 velocity = Velocity;
		if (!IsOnFloor()) {
			velocity.Y -= gravity * (float)deltaTime;
		}
		Velocity = velocity;
	}

	public void TakeDamage(CharacterBody3D? source, int damage) {
		if ((Health - damage) <= 0) {
			Health = 0;
			SetCorpseCollision();
			EnterDeathState();
		}
		else {
			Health -= damage;
			AI.OnTakeDamage(this, source, damage);
		}
	}

	public virtual void OnDeath() {

	}

	// Probably smarter to replace the enemy with a generic corpse entity that inherits its last death animation frame
	// Alternatively, alter the script to make the collision very short to avoid tanking projectiles unintentionally - Done, executed in individual death script
	public void SetCorpseCollision() {
		SetCollisionLayerValue(5, false);
		SetCollisionLayerValue(6, true);
	}

	public void UpdateDebugLabel(Label3D label) {
		label.Text = $"Health: {Health} / {StartingHealth}\nState: {EnemyState}";
	}
}
