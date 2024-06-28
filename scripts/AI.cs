using System;
using Godot;

public static class AI {
    public static void SetTargetLocation(EnemyBase enemy, Vector3 vector3) {
        enemy.Target = null;
        enemy.TargetLocation = vector3;
    }

    public static void SetTargetActor(EnemyBase enemy, Actor target) {
        enemy.Target = target;
        enemy.TargetLocation = target.GlobalPosition;
    }
    
    public static float GetDotProdX(EnemyBase enemy, CharacterBody3D character) {
        Vector3 facing = -enemy.GlobalTransform.Basis.X;
        Vector3 flat = new(character.GlobalPosition.X, enemy.GlobalPosition.Y, character.GlobalPosition.Z);
        Vector3 target = enemy.GlobalPosition.DirectionTo(flat);
        return facing.Dot(target);
    }

    public static float GetDotProdZ(EnemyBase enemy, CharacterBody3D character) {
        Vector3 facing = -enemy.GlobalTransform.Basis.Z;
        Vector3 flat = new(character.GlobalPosition.X, enemy.GlobalPosition.Y, character.GlobalPosition.Z);
        Vector3 target = enemy.GlobalPosition.DirectionTo(flat);
        return facing.Dot(target);
    }
    
    public static void WakeUpAndTargetSource(EnemyBase enemy, Actor? source, int damage) {
        if (source != null) {
            if (enemy.EnemyState == EnemyState.Idle) {
                enemy.WakeUp();
            }
            SetTargetActor(enemy, source);
        }
    }

    public static bool CanMove(EnemyBase enemy, float angle, float dist) {
        Vector3 targetPos = new(0, 0, -dist);
        targetPos = targetPos.Rotated(Vector3.Up, angle);
        enemy.PathCast.TargetPosition = targetPos;
        enemy.PathCast.ForceRaycastUpdate();

        if (enemy.PathCast.IsColliding()) {
            Node3D collider = (Node3D)enemy.PathCast.GetCollider();
            if (collider.IsInGroup("Player")) {
                return true;
            }
            else if (collider.IsInGroup("Enemy")) {
                return false;
            }
            else if (collider.IsClass("StaticBody3D")) {
                return false;
            }
            else {
                return true;
            }
        }
        else {
            return true;
        }
    }

    public static void ChooseDirection(EnemyBase enemy, float angle) {
        // equal to projecting 0,5 secs ahead
        float movedPerStep = enemy.MaxSpeed / 2f;

        // Try moving straight to target
        if (CanMove(enemy, angle, movedPerStep)) {
            enemy.GoalAngle = angle;
        }
        else if (CanMove(enemy, angle + 0.33f, movedPerStep)) {
            enemy.GoalAngle = angle + 0.33f;
        }
        else if (CanMove(enemy, angle - 0.33f, movedPerStep)) {
            enemy.GoalAngle = angle - 0.33f;
        }
        // Try turning around
        else {
            if (angle > 0) {
                enemy.GoalAngle = angle + (float)Math.PI * 0.5f;
            }
            else {
                enemy.GoalAngle = angle - (float)Math.PI * 0.5f;
            }
        }
    }
    
    public static void MoveForward(EnemyBase enemy) {
        enemy.Speed += enemy.Acceleration;
        Vector3 vel = new(0, 0, -1);
        vel = vel.Rotated(Vector3.Up, enemy.Rotation.Y);
        enemy.Velocity = vel * enemy.Speed;
    }

    public static void Turn(EnemyBase enemy) {
        if (enemy.GoalAngle > enemy.TurnRate * 0.5f) {
            enemy.RotateObjectLocal(Vector3.Up, enemy.TurnRate);
            enemy.Velocity = enemy.Velocity.Rotated(Vector3.Up, enemy.TurnRate);
        }
        else if (enemy.GoalAngle < -enemy.TurnRate * 0.5f) {
            enemy.RotateObjectLocal(Vector3.Up, -enemy.TurnRate);
            enemy.Velocity = enemy.Velocity.Rotated(Vector3.Up, -enemy.TurnRate);
        }
    }

    public static bool CanSee(EnemyBase enemy, CharacterBody3D target) {
        Vector3 angleVector = new(target.GlobalPosition.X - enemy.GlobalPosition.X, 0f, target.GlobalPosition.Z - enemy.GlobalPosition.Z);
        float angle = -enemy.GlobalTransform.Basis.Z.SignedAngleTo(angleVector, Vector3.Up);
        Vector3 targetPos = new(0, 0, -enemy.SightRange);
        targetPos = targetPos.Rotated(Vector3.Up, angle);
        enemy.SightRay.TargetPosition = targetPos;

        enemy.SightRay.ForceRaycastUpdate();
        if (enemy.SightRay.IsColliding()) {
            Node3D collider = (Node3D)enemy.SightRay.GetCollider();
            if (collider.IsInGroup("Player")) {
                return true;
            }
            else if (collider.IsClass("StaticBody3D")) {
                return false;
            }
            else {
                return false;
            }
        }
        else {
            return false;
        }
    }
}
