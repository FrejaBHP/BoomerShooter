using Godot;

public partial class Explosion : Node3D {
    private Sprite3D explosionSprite;
    private ShapeCast3D shapeCast;
    private AudioStreamPlayer3D audio;

    private Actor owner;
    private Rid throwee;

    private int maxDamage = 80;

    private int spriteFrame = 0;
    private int framesToWait = 5;
    private int frameCounter = 0;
    private float radius;

    public override void _Ready() {
        explosionSprite = GetNode<Sprite3D>("ExplosionSprite");
        shapeCast = GetNode<ShapeCast3D>("ShapeCast3D");
        audio = GetNode<AudioStreamPlayer3D>("Boom");

        radius = (shapeCast.Shape as SphereShape3D).Radius;

        explosionSprite.Texture = Sprites.Spr_FX_Explosion[0];
        FixSpriteOffset();
        Explode();
    }
    
    public override void _PhysicsProcess(double delta) {
        frameCounter++;
        if (frameCounter == framesToWait) {
            frameCounter = 0;

            spriteFrame++;
            if (spriteFrame < 13) {
                explosionSprite.Texture = Sprites.Spr_FX_Explosion[spriteFrame];
                FixSpriteOffset();
            }
            else if (spriteFrame == 13) {
                explosionSprite.Texture = null;
            }
        }
    }

    private async void Explode() {
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
        shapeCast.ForceShapecastUpdate();
        if (shapeCast.IsColliding()) {
            for (int i = 0; i < shapeCast.GetCollisionCount(); i++) {
                if (shapeCast.GetCollider(i).IsClass("CharacterBody3D")) {
                    CharacterBody3D collider = shapeCast.GetCollider(i) as CharacterBody3D;

                    int explosionDamage = DamageFactor(shapeCast.GetCollisionPoint(i));
                    
                    if (collider.IsInGroup("Enemy")) {
                        (collider as Actor).TakeDamage(owner, explosionDamage);
                        (collider as EnemyBase).MoveState = MoveState.Knockback;
                    }
                    else if (collider.IsInGroup("Player")) {
                        if (collider.GetRid() == throwee) {
                            (collider as Actor).TakeDamage(owner, (int)(explosionDamage * 0.5f));
                        }
                        else {
                            (collider as Actor).TakeDamage(owner, explosionDamage);
                        }
                    }

                    Vector3 force = collider.GlobalPosition - GlobalPosition;
                    force.Y += 0.25f;
                    force = force.Normalized();

                    float knockback = explosionDamage * 0.125f;

                    if (knockback > 8f) {
                        knockback = 8f;
                    }

                    Vector3 appliedForce = force * knockback;
                    collider.Velocity += appliedForce;
                    
                    //GD.Print($"Force: {force:F3}, KB: {knockback:F2}\nAppl.: {appliedForce:F3}");
                }
            }
        }
        audio.Play();
    }

    public void SetThrowee(Rid rid) {
        throwee = rid;
    }

    public void SetOwner(Actor owner) {
        this.owner = owner;
    }

    private void OnAudioFinished() {
        QueueFree();
    }

    private int DamageFactor(Vector3 targetOverlap) {
        float factor = 1f - ((GlobalPosition.DistanceTo(targetOverlap) / radius));
        return (int)(maxDamage * factor);
    }

    private void FixSpriteOffset() {
        float spriteYOffset = explosionSprite.Texture.GetHeight() / 2f;
		explosionSprite.Offset = explosionSprite.Offset with { Y = spriteYOffset };
    }
}
