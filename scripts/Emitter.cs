using System.Threading.Tasks;
using Godot;

public partial class Emitter : Node3D {
    private Sprite3D sprite;
    public MiscAnimation Animation { get; private set; }
    public AudioStreamPlayer3D Audio { get; set; }
    public bool FlipX { get; set; }

    private int framesLived = 0;
    private int spriteFrame = 0;
    private int frameCounter = 0;
    private int duration = 5;

    public override void _Ready() {
        sprite = GetNode<Sprite3D>("Sprite");
        Audio = GetNode<AudioStreamPlayer3D>("Audio");
    }

    public void PlaySound() {
        if (Audio.Stream != null) {
            Audio.Play();
        }
    }

    public void SetAnimation(MiscAnimation anim) {
        if (anim != null) {
            Animation = anim;
            
            if (FlipX) {
                sprite.FlipH = true;
            }

            ApplyTexture();
        }
    }

    public void SetDuration() {
        int audiolength = 0;
        if (Audio.Stream != null) {
            audiolength = (int)(Audio.Stream.GetLength() * 60f);
        }
        int animlength = 0;
        if (Animation != null) {
            animlength = Animation.TotalLength;
        }

        if (audiolength > animlength) {
            duration = audiolength;
        }
        else {
            duration = animlength;
        }
    }

    public override void _PhysicsProcess(double delta) {
        if (Animation != null) {
            ProcessAnimation();
        }
        if (framesLived >= duration) {
            QueueFree();
        }
        else {
            framesLived++;
        }
    }

    private void ProcessAnimation() {
        frameCounter++;

        if (frameCounter == Animation.AnimFrames[spriteFrame].FrameLength) {
            frameCounter = 0;
            
            if (spriteFrame + 1 < Animation.AnimFrames.Length) {
                spriteFrame++;
                ApplyTexture();
            }
            else if (spriteFrame + 1 == Animation.AnimFrames.Length) {
                sprite.Texture = null;
            }
        }
    }

    private void ApplyTexture() {
        if (FlipX) {
            float mirrorX = -Animation.AnimFrames[spriteFrame].Offset.X;
            sprite.Offset = Animation.AnimFrames[spriteFrame].Offset with { X = mirrorX };
        }
        else {
            sprite.Offset = Animation.AnimFrames[spriteFrame].Offset;
        }
        sprite.Texture = Animation.AnimFrames[spriteFrame].Texture;
    }
}
