using Godot;
using System;

public partial class EnemyBase : CharacterBody3D {
	public virtual EStateMachine EStateMachine { get; protected set; }
	public virtual EnemyState EnemyState { get; protected set; }

	public virtual float Speed { get; protected set; }
	public virtual int Health { get; protected set; }

    public virtual Sprite3D VisSprite { get; protected set; }
	public virtual int SpriteAnimFrame { get; protected set; }
	public virtual int SpriteRotation { get; protected set; }
	public virtual bool SpriteIsFlipped { get; protected set; }

	public virtual CharacterBody3D Target { get; set; }
	public virtual Vector3 TargetLocation { get; set; }

	
	public virtual EnemyAnimation CurAnim { get; protected set; }
    public virtual int CurAnimFrame { get; protected set; }
    public virtual int CurAnimTick { get; protected set; }

	public virtual AudioStreamPlayer3D VoiceAudio { get; protected set; }


	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public override void _Ready() {

    }

    public override void _PhysicsProcess(double delta) {

	}

	public void SetEnemyState(int estate) {
        EnemyState = (EnemyState)estate;
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
		float dotProductX = AI.GetDotProdX(this, Target);
        float dotProductZ = AI.GetDotProdZ(this, Target);

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

	public virtual void WakeUp() {

	}

	public void TakeDamage(CharacterBody3D? source, int damage) {
		if ((Health - damage) < 0) {
			Health = 0;
		}
		else {
			Health -= damage;
			AI.OnTakeDamage(this, source, damage);
		}
	}

	
}
