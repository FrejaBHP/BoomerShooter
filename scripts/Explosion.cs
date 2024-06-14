using Godot;

public partial class Explosion : Node3D {
    private Sprite3D explosionSprite;
    private ShapeCast3D shapeCast;
    private AudioStreamPlayer3D audio;

    private int damage = 80;

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
                    
                    float yOffset = 0.10f;
                    if (collider.IsInGroup("Enemy")) {
                        (collider as EnemyBase).TakeDamage(null, DamageFactor(shapeCast.GetCollisionPoint(i)));
                        //yOffset = (collider as EnemyBase).ColHeight / 2;
                        //GD.Print(DamageFactor(shapeCast.GetCollisionPoint(i)));
                    }
                    else if (collider.IsInGroup("Player")) {
                        (collider as bsPlayer).TakeDamage(null, DamageFactor(shapeCast.GetCollisionPoint(i)));
                        //yOffset = (collider as bsPlayer).ColHeight / 2;
                        //GD.Print(DamageFactor(shapeCast.GetCollisionPoint(i)));
                    }
                    
                    Vector3 velocity = collider.GlobalPosition - GlobalPosition;
                    velocity.Y += yOffset;
                    velocity *= 8f;
                    collider.Velocity += velocity;
                }
            }
        }
        audio.Play();
    }

    private void OnAudioFinished() {
        QueueFree();
    }

    private int DamageFactor(Vector3 victimpos) {
        float factor = radius - (GlobalPosition.DistanceTo(victimpos) / 2f);
        return (int)(damage * factor);
    }

    private void FixSpriteOffset() {
        float spriteYOffset = explosionSprite.Texture.GetHeight() / 2f;
		explosionSprite.Offset = explosionSprite.Offset with { Y = spriteYOffset };
    }
}
