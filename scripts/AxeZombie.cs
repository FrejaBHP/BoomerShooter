using Godot;
using System;

public partial class AxeZombie : EnemyBase, IEnemy {
    public IEnemy InterE { get; }
    private float anglething;
    private bool hasAttacked = false;

    public AxeZombie() {
        InterE = this;
        MaxSpeed = 2.0f;
        Speed = 0f;
        TurnRate = 0.05f;
        Acceleration = 0.1f;

        SurfaceType = SurfaceType.Flesh;
        StartingHealth = 60;
        Health = StartingHealth;
        PainChance = 0.2f;

        GoalVector = new();
        SightRange = 25f;

        CanMove = true;
        
        spriteWillUpdate = true;
        spriteWillRotate = true;

        EStateMachine = new();
        IdleState = new();
        ChaseState = new();
        AtkState = new();
        StunState = new();
        DeathState = new();
        CorpseState = new();
    }

    public override void _Ready() {
        ConnectNodes();
        ColHeight = (ColShape.Shape as CapsuleShape3D).Height;

        IdleState.ConfigureState(-1, this, null, null, (int)EnemyState.Idle, null);
        ChaseState.ConfigureState(this, EAnimations.Anim_Enemy_AZ_Walk, null, (int)EnemyState.Chase, null);
        AtkState.ConfigureState(this, EAnimations.Anim_Enemy_AZ_Attack, null, (int)EnemyState.Attack, ChaseState);
        StunState.ConfigureState(this, EAnimations.Anim_Enemy_AZ_Stun, null, (int)EnemyState.Stun, ChaseState);
        DeathState.ConfigureState(this, EAnimations.Anim_Enemy_AZ_Die, () => PlayDeathSound(), (int)EnemyState.Death, CorpseState);
        CorpseState.ConfigureState(-1, this, null, () => OnDeath(), (int)EnemyState.Corpse, null);

        EnterIdleState();
    }

    public override void _PhysicsProcess(double delta) {
        if (isActive) {
            Think();

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
            
            if (!TryClimbStair(delta)) {
                MoveAndSlide();
                TryDescendStair();
            }
        }

        UpdateDebugLabel();
        AddExtraLabelInfo("AngleThing", anglething);
        if (Target != null) {
            AddExtraLabelInfo("DotZ Goal", GlobalTransform.Basis.Z.Dot(GoalVector));
        }
    }

    private void Think() {
        switch (EnemyState) {
            case EnemyState.Idle:
                break;
            
            case EnemyState.Chase:
                Vector3 facing = -GlobalTransform.Basis.Z;
                Vector3 targetFlat = new(Target.GlobalPosition.X, GlobalPosition.Y, Target.GlobalPosition.Z);
                Vector3 targetFlatDist = targetFlat - GlobalPosition;
                float targetAngle = facing.SignedAngleTo(targetFlatDist, Vector3.Up);

                anglething = targetAngle;
                AI.ChooseDirection(this, targetAngle);
 
                if (AI.GetDotProdZ(this, Target) < 0.9f) {
                    MoveState = MoveState.Turn;
                    AI.Turn(this);
                }
                else {
                    if (GlobalPosition.DistanceTo(targetFlat) < Attacks.AxeZombieSwing.Range * 0.75f) {
                        SightRay.TargetPosition = new(0, 0, -1);
                        SightRay.ForceRaycastUpdate();
                        if (SightRay.IsColliding()) {
                            Actor collider = SightRay.GetCollider() as Actor;
                            if (collider == Target) {
                                MoveState = MoveState.Stand;
                                EStateMachine.SetState(AtkState);
                                break;
                            }
                        }
                    }
                    MoveState = MoveState.Walk;
                    AI.Turn(this);
                    AI.MoveForward(this);
                }
                break;
            
            case EnemyState.Attack:
                break;

            case EnemyState.Stun:
                break;

            case EnemyState.Death:
                break;

            case EnemyState.Corpse:
                break;

            default:
                break;
        }
    }

    public override void ExecuteAction(EnemyAction action) {
        switch (action) {
            case EnemyAction.AZ_Swing:
                Attack();
                break;
            
            default:
                break;
        }
    }

    public override void ProcessGroundMovement(double deltaTime) {
        switch (MoveState) {
            case MoveState.None:
                Velocity = Velocity.Lerp(Vector3.Zero, 0.08f);
                break;

            case MoveState.Stand:
                Velocity = Vector3.Zero;
                break;

            case MoveState.Turn:
                Velocity = Velocity.Lerp(Vector3.Zero, 0.03f);
                break;
            
            case MoveState.Walk:
                break;
            
            case MoveState.Fly:
                break;
            
            case MoveState.Knockback:
                if (!IsOnFloor()) {
                    Velocity = Velocity.Lerp(new(0, Velocity.Y, 0), 0.04f);
                }
                else {
                    if (EnemyState == EnemyState.Death || EnemyState == EnemyState.Corpse) {
                        MoveState = MoveState.None;
                    }
                    else {
                        MoveState = MoveState.Stand;
                    }
                }
                break;
            
            default:
                break;
        }
    }

    public override void Attack() {
        FireHitscanAttack(this, Attacks.AxeZombieSwing, 0f, 0f, true);
    }

    private void RemoveAttackFlag() {
        hasAttacked = false;
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
                VisSprite.Texture = Sprites.Spr_Zombie_Attack[SpriteAnimFrame, SpriteRotation];
                VisSprite.FlipH = SpriteIsFlipped;
                break;

            case EnemyState.Stun:
                VisSprite.Texture = Sprites.Spr_Zombie_Pain[SpriteAnimFrame, SpriteRotation];
                VisSprite.FlipH = SpriteIsFlipped;
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

    private void PlayDeathSound() {
        switch (Utils.RandomInt(0, 3)) {
            case 0:
                VoiceAudio.Stream = SFX.AxeZombieDie1;
                break;
            
            case 1:
                VoiceAudio.Stream = SFX.AxeZombieDie2;
                break;
            
            case 2:
                VoiceAudio.Stream = SFX.AxeZombieDie3;
                break;
        }
        VoiceAudio.Play();
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
