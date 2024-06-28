using Godot;
using Godot.Collections;

public partial class DecorationBrush : SurfaceBrush {
    public override void _Ready() {
        AddToGroup("DecoFlat");
    }
}