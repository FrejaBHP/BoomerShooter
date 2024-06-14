using Godot;
using Godot.Collections;

public partial class DoorHinge : Node3D {
    [Export]
	Dictionary func_godot_properties;

    public override void _Ready() {
        AddToGroup("DoorHinge");
        Rotation = Rotation with { Y = 0f };
    }

    public void GetAndReparentDoors() {
        GetTree().CallGroup(func_godot_properties["target"].AsString(), "SetHinge", this);
    }
}
