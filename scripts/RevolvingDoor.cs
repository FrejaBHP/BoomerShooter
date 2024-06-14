using Godot;
using Godot.Collections;

public partial class RevolvingDoor : StaticBody3D {
    [Export]
	Dictionary func_godot_properties;

    private Node3D hinge;
    private bool isOpening = false;
    private bool isClosing = false;
	private bool interactedWith = false;
    private bool open = false;
    private float rotationRad = 0f;
    private float degreesTurned = 0f;

    public override void _Ready() {
        if (func_godot_properties["hingename"].AsString() != "") {
            AddToGroup(func_godot_properties["hingename"].AsString());
        }
        if (func_godot_properties["targetname"].AsString() != "") {
            AddToGroup(func_godot_properties["targetname"].AsString());
        }
        AddToGroup("DoorRevolving");
        
        rotationRad = ((float)System.Math.PI / 180) * func_godot_properties["speed"].AsSingle();
        
        if ((func_godot_properties["useflags"].AsInt32() & 1 << 0) != 0) {
            SetCollisionLayerValue(2, true);
        }
    }

    public override void _PhysicsProcess(double delta) {
        if (isOpening) {
            if (!func_godot_properties["reversedir"].AsBool()) {
                hinge.RotateY(rotationRad);
            }
            else {
                hinge.RotateY(-rotationRad);
            }

            degreesTurned += func_godot_properties["speed"].AsSingle();
                
            if (degreesTurned >= func_godot_properties["angle"].AsSingle()) {
                if (!func_godot_properties["reversedir"].AsBool()) {
                    hinge.RotationDegrees = hinge.RotationDegrees with { Y = func_godot_properties["angle"].AsSingle() };
                }
                else {
                    hinge.RotationDegrees = hinge.RotationDegrees with { Y = 360f - func_godot_properties["angle"].AsSingle() };
                }
                isOpening = false;
                open = true;
            }
		}
        else if (isClosing) {
            if (!func_godot_properties["reversedir"].AsBool()) {
                hinge.RotateY(-rotationRad);
            }
            else {
                hinge.RotateY(rotationRad);
            }

            degreesTurned += func_godot_properties["speed"].AsSingle();

            if (degreesTurned >= func_godot_properties["angle"].AsSingle()) {
                hinge.RotationDegrees = hinge.RotationDegrees with { Y = 0f };
				isClosing = false;
                open = false;
			}
        }
    }

    public void Trigger() {
        if (!isOpening && !isClosing) {
            if (!open) {
                isOpening = true;
                degreesTurned = 0f;
                interactedWith = true;
            }
            else if (open && (func_godot_properties["useflags"].AsInt32() & 1 << 1) != 0) {
                isClosing = true;
                degreesTurned = 0f;
            }
        }
    }

    public void SetHinge(Node3D newHinge) {
        hinge = newHinge;
        Reparent(hinge);
    }
}
