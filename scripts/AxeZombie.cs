using Godot;
using System;

public partial class AxeZombie : EnemyBase, IEnemy {
    public IEnemy InterE { get; }

    public override EStateMachine EStateMachine { get; protected set; } = new();
    public override EnemyState EnemyState { get; protected set; }
    public override CharacterBody3D Target { get; set; }
    public override Vector3 TargetLocation { get; set; }

	public override float Speed { get; protected set; } = 4.0f;
    public override int StartingHealth { get; protected set; } = 60;
    public override int Health { get; protected set; }
    public override bool IsMoving { get; protected set; }
    public override bool CanMove { get; protected set; } = true;

    public override CollisionShape3D ColShape { get; protected set; }
    public override float ColHeight { get; protected set; }

    public override Sprite3D VisSprite { get; protected set; }
    public override int SpriteAnimFrame { get; protected set; } = 0;
    public override int SpriteRotation { get; protected set; } = 0;
    public override bool SpriteIsFlipped { get; protected set; } = false;

    public override EnemyAnimation CurAnim { get; protected set; }
    public override int CurAnimFrame { get; protected set; }
    public override int CurAnimTick { get; protected set; }

    public override AudioStreamPlayer3D VoiceAudio { get; protected set; }

    public override Label3D DebugLabel { get; protected set; }

    protected override EIdleState IdleState { get; } = new();
    protected override EChaseState ChaseState { get; } = new();
    protected override EAtkState AtkState { get; } = new();
    protected override EStunState StunState { get; } = new();
    protected override EDeathState DeathState { get; } = new();
    protected override ECorpseState CorpseState { get; } = new();

    public AxeZombie() {
        InterE = this;
        Health = StartingHealth;
        spriteWillUpdate = true;
        spriteWillRotate = true;
    }

    public override void _Ready() {
        ColShape = GetNode<CollisionShape3D>("CollisionShape3D");
        ColHeight = (ColShape.Shape as CapsuleShape3D).Height;

        VisSprite = GetNode<Sprite3D>("VisSprite");
		VoiceAudio = GetNode<AudioStreamPlayer3D>("VoiceAudio");
        DebugLabel = GetNode<Label3D>("DebugStats");

        IdleState.ConfigureState(-1, this, null, null, (int)EnemyState.Idle, null);
        ChaseState.ConfigureState(this, EAnimations.Anim_Enemy_AZ_Walk, null, (int)EnemyState.Chase, null);
        AtkState.ConfigureState(-1, this, null, null, (int)EnemyState.Attack, null);
        StunState.ConfigureState(-1, this, null, null, (int)EnemyState.Stun, null);
        DeathState.ConfigureState(this, EAnimations.Anim_Enemy_AZ_Die, null, (int)EnemyState.Death, CorpseState);
        CorpseState.ConfigureState(-1, this, null, () => OnDeath(), (int)EnemyState.Corpse, null);

        EnterIdleState();
    }

    public override void _PhysicsProcess(double delta) {
        if (isActive) {
            EStateMachine.Process();
            ProcessAnimation();
            
            if (spriteWillRotate) {
                SetSpriteRotation();
            }
            if (spriteWillUpdate) {
                SetSprite();
                AdjustSpriteYOffset();
            }
        }

        if (CanMove) {
            ProcessGroundMovement(delta);
            ProcessGravity(delta);
            MoveAndSlide();
        }

        UpdateDebugLabel(DebugLabel);
    }

    public override void ProcessGroundMovement(double deltaTime) {
        if (EnemyState == EnemyState.Chase && IsOnFloor()) {
			IsMoving = true;
		}
		else {
			IsMoving = false;
		}

        if (IsMoving) {

        }
        else {
            Velocity = Velocity.Lerp(new(0, Velocity.Y, 0), 0.04f);
        }

        if (EnemyState == EnemyState.Corpse && Velocity == Vector3.Zero) {
            CanMove = false;
        }
    }

    public void SetSprite() {
        switch (EnemyState) {
            case EnemyState.Idle:
                VisSprite.Texture = Sprites.Spr_Zombie_Attack[0, SpriteRotation];
                VisSprite.FlipH = SpriteIsFlipped;
                break;
            
            case EnemyState.Chase:
                VisSprite.Texture = Sprites.Spr_Zombie_Walk[SpriteAnimFrame, SpriteRotation];
                VisSprite.FlipH = SpriteIsFlipped;
                break;
            
            case EnemyState.Attack:
                break;

            case EnemyState.Stun:
                break;

            case EnemyState.Death:
                VisSprite.Texture = Sprites.Spr_Zombie_Death[SpriteAnimFrame];
                VisSprite.FlipH = false;
                break;

            case EnemyState.Corpse:
                
                break;

            default:
                break;
        }
    }

    public override void OnDeath() {
        VisSprite.Texture = Sprites.Spr_Zombie_Death[4];
        (ColShape.Shape as CapsuleShape3D).Height = 0.25f;
        ColHeight = (ColShape.Shape as CapsuleShape3D).Height;

        float newHeight = Position.Y - 0.375f;
        Position = Position with { Y = newHeight };
        
        AdjustSpriteYOffset();
        VisSprite.FlipH = false;
        spriteWillUpdate = false;
        spriteWillRotate = false;
    }
}
