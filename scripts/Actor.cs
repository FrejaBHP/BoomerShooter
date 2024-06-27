using Godot;

public partial class Actor : CharacterBody3D {
    public SurfaceType SurfaceType { get; protected set; }
    public float MaxSpeed { get; protected set; }
	private float speed;
	public float Speed {
		get => speed;
		set {
			if (value > MaxSpeed) {
				speed = MaxSpeed;
			}
			else {
				speed = value;
			}
		} 
	}

    public float MaxStepHeight { get; protected set; } = 0.25f;

    public int StartingHealth { get; protected set; }
	public virtual int Health { get; protected set; }


    public CollisionShape3D ColShape { get; protected set; }
	public float ColHeight { get; protected set; }

    protected RayCast3D stairUpCast;
	protected RayCast3D stairDownCast;
	protected bool justMovedOnStairs = false;
    protected ulong justWasOnFloor = 0;

    // Translated to C# from https://github.com/majikayogames/SimpleFPSController
    protected bool TryClimbStair(double deltaTime) {
		if (!IsOnFloor() && !justMovedOnStairs) {
			return false;
		}

		Vector3 moveVector = new(1,0,1);
		if (Velocity.Y > 0 || (Velocity * moveVector).Length() == 0) {
			return false;
		}

		Vector3 projectVector = Velocity * moveVector * (float)deltaTime;
		Transform3D stepPosition = GlobalTransform.Translated(projectVector + (Vector3)new(0, MaxStepHeight * 2, 0));

		KinematicCollision3D colCheckResult = new();
		if (TestMove(stepPosition, new(0, -MaxStepHeight * 2, 0), colCheckResult) && colCheckResult.GetCollider().IsClass("StaticBody3D")) {
            float stepHeight = (stepPosition.Origin + colCheckResult.GetTravel() - GlobalPosition).Y;
			
			if (stepHeight > MaxStepHeight || stepHeight <= 0.01f || (colCheckResult.GetPosition() - GlobalPosition).Y > MaxStepHeight) {
				return false;
			}

			stairUpCast.GlobalPosition = colCheckResult.GetPosition() + (Vector3)new(0, MaxStepHeight, 0) + projectVector.Normalized() * 0.1f;
			stairUpCast.ForceRaycastUpdate();

			if (stairUpCast.IsColliding() && !Utils.IsSurfaceTooSteep(stairUpCast.GetCollisionNormal(), this)) {
				GlobalPosition = stepPosition.Origin + colCheckResult.GetTravel();
				ApplyFloorSnap();
				justMovedOnStairs = true;
				return true;
			}
		}

		return false;
	}

    // Translated to C# from https://github.com/majikayogames/SimpleFPSController
    protected void TryDescendStair() {
		stairDownCast.ForceRaycastUpdate();
		bool isFloorBelow = stairDownCast.IsColliding() && !Utils.IsSurfaceTooSteep(stairDownCast.GetCollisionNormal(), this);
		bool wasOnFloor = Engine.GetPhysicsFrames() == justWasOnFloor;
		
		if (!IsOnFloor() && Velocity.Y <= 0 && (justMovedOnStairs || wasOnFloor) && isFloorBelow) {
			KinematicCollision3D colCheckResult = new();

			if (TestMove(GlobalTransform, new(0, -MaxStepHeight, 0), colCheckResult)) {
				float posY = Position.Y + colCheckResult.GetTravel().Y;
				Position = Position with { Y = posY };
				ApplyFloorSnap();
				justMovedOnStairs = true;
			}
		}
		else {
			justMovedOnStairs = false;
		}
	}

    public void FireHitscanAttack(Node3D parent, HitscanAttack atk, float offsetX, float offsetY, bool makesSound) {
		RayCast3D newVector = new();
		newVector.SetCollisionMaskValue(1, true);
		newVector.SetCollisionMaskValue(2, true);
        newVector.SetCollisionMaskValue(4, true);
		newVector.SetCollisionMaskValue(5, true);
		parent.AddChild(newVector);
		
		Vector3 target = new(offsetX, offsetY, -1f);
		target *= atk.Range;
		newVector.TargetPosition = target;
		newVector.ForceRaycastUpdate();

		if (newVector.IsColliding() && newVector.GetCollider().IsClass("Node3D")) {
			Node3D collider = newVector.GetCollider() as Node3D;
			if (collider.IsInGroup("Enemy") || collider.IsInGroup("Player")) {
				(collider as Actor).TakeDamage(this, atk.Damage);
                BuildEmitter(newVector.GetCollisionPoint(), atk.DamageType, (collider as Actor).SurfaceType, makesSound);
			}
			else if (collider.IsInGroup("Geometry")) {
                BuildEmitter(newVector.GetCollisionPoint(), atk.DamageType, (SurfaceType)(collider as SurfaceBrush).func_godot_properties["surface_material"].AsInt32(), makesSound);
			}
		}
        
		newVector.Free();
	}

    protected void BuildEmitter(Vector3 pos, DamageType dt, SurfaceType st, bool makesSound) {
        MiscAnimation anim = Attacks.MakeSparks(dt, st);
        AudioStreamWav noise;
        if (makesSound) {
			noise = Attacks.MakeNoise(dt, st);
		}
		else {
			noise = null;
		}

        if (!(noise == null && anim == null)) {
			int fiftyfifty = Utils.RandomInt(0, 2);
			bool flipX = false;
			
            if (fiftyfifty == 1) {
				flipX = true;
			}

			Utils.CreateEmitter(pos, noise, anim, flipX);
		}
    }

    public virtual void TakeDamage(Actor? source, int damage) {
		if ((Health - damage) <= 0) {
			Health = 0;
		}
		else {
			Health -= damage;
		}
	}
}
