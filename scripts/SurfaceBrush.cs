using Godot;
using Godot.Collections;

public partial class SurfaceBrush : StaticBody3D {
    [Export]
	public Dictionary func_godot_properties { get; protected set; }

    public override void _Ready() {
        AddToGroup("Geometry");
    }
}