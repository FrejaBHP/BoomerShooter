using Godot;
using System;

public partial class AxeZombie : EnemyBase, IEnemy {
    public IEnemy InterE { get; }

    public override EStateMachine EStateMachine { get; protected set; } = new();
    public override EnemyState EnemyState { get; protected set; }
    public override CharacterBody3D Target { get; set; }
    public override Vector3 TargetLocation { get; set; }

	public override float Speed { get; protected set; } = 4.0f;
    public override int Health { get; protected set; } = 60;

    public override Sprite3D VisSprite { get; protected set; }
    public override int SpriteAnimFrame { get; protected set; } = 0;
    public override int SpriteRotation { get; protected set; } = 0;
    public override bool SpriteIsFlipped { get; protected set; } = false;

    public override EnemyAnimation CurAnim { get; protected set; }
    public override int CurAnimFrame { get; protected set; }
    public override int CurAnimTick { get; protected set; }

    public override AudioStreamPlayer3D VoiceAudio { get; protected set; }

    private class EIdleState : EState {}
    private EIdleState IdleState { get; } = new();

    private class EChaseState : EState {}
    private EChaseState ChaseState { get; } = new();

    public AxeZombie() {
        InterE = this;
    }

    public override void _Ready() {
        VisSprite = GetNode<Sprite3D>("VisSprite");
		VoiceAudio = GetNode<AudioStreamPlayer3D>("VoiceAudio");

        IdleState.ConfigureState(-1, this, null, null, (int)EnemyState.Idle, null);
        ChaseState.ConfigureState(this, EAnimations.Anim_Enemy_AZ_Walk, null, (int)EnemyState.Chase, null);
        
        AI.SetTargetCharacter(this, Game.Player);
        EnterIdleState();
    }

    public override void _PhysicsProcess(double delta) {
        EStateMachine.Process();
        ProcessAnimation();
        SetSpriteRotation();
        SetSprite();
        MoveAndSlide();
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

            default:
                break;
        }
    }

    public override void WakeUp() {

    }

    public void EnterIdleState() {
        EStateMachine.SetState(IdleState);
    }

    public void EnterChaseState() {
        EStateMachine.SetState(ChaseState);
    }
}
