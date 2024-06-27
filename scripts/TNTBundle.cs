using Godot;

public partial class TNTBundle : RigidBody3D {
    private readonly PackedScene explosion = GD.Load<PackedScene>("res://explosion.tscn");

    private Actor owner;
    private Area3D targetArea;

    public override void _Ready() {
        targetArea = GetNode<Area3D>("Area3D");
    }

    public override void _PhysicsProcess(double delta) {
        //GD.Print($"TNT TF: {GlobalPosition}");
    }

    public void SetOwner(Actor owner) {
        this.owner = owner;
    }

    public void OnBodyEntered(Node3D body) {
        if (!GetCollisionExceptions().Contains(body as PhysicsBody3D)) {
            Explosion boom = explosion.Instantiate() as Explosion;
            Game.EntitiesNode.AddChild(boom);
            boom.GlobalPosition = GlobalPosition;
            boom.SetOwner(owner);
            boom.SetThrowee(owner.GetRid());
            QueueFree();
        }
    }
}
