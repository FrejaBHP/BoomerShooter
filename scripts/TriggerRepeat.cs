using Godot;
using Godot.Collections;

public partial class TriggerRepeat : Area3D {
    [Export]
	Dictionary func_godot_properties;

    public override void _Ready() {
        AddToGroup("Trigger");
        
        if (func_godot_properties["walk_over"].AsBool()) {
            BodyEntered += OnBodyEntered;
            SetCollisionMaskValue(4, true);
            if (!func_godot_properties["player_only"].AsBool()) {
                SetCollisionMaskValue(5, true);
            }
        }

        if (func_godot_properties["interact_with"].AsBool()) {
            SetCollisionLayerValue(2, true);
        }
    }

    private void OnBodyEntered(Node3D body) {
        TriggerTargets();
    }

    public void TriggerTargets() {
        GetTree().CallGroup(func_godot_properties["target"].AsString(), MethodName.Trigger);
    }

    private void Trigger() {

    }
}
