using Godot;

public static class AI {
    public static void SetTargetLocation(EnemyBase enemy, Vector3 vector3) {
        enemy.Target = null;
        enemy.TargetLocation = vector3;
    }

    public static void SetTargetCharacter(EnemyBase enemy, CharacterBody3D character) {
        enemy.Target = character;
        enemy.TargetLocation = character.GlobalPosition;
    }
    
    public static float GetDotProdX(EnemyBase enemy, CharacterBody3D character) {
        Vector3 facing = enemy.GlobalTransform.Basis.X;
        Vector3 target = enemy.GlobalPosition.DirectionTo(character.GlobalPosition);
        return facing.Dot(target);
    }

    public static float GetDotProdZ(EnemyBase enemy, CharacterBody3D character) {
        Vector3 facing = enemy.GlobalTransform.Basis.Z;
        Vector3 target = enemy.GlobalPosition.DirectionTo(character.GlobalPosition);
        return facing.Dot(target);
    }
    
    public static void WakeUpAndTargetSource(EnemyBase enemy, CharacterBody3D? source, int damage) {
        if (source != null) {
            if (enemy.EnemyState == EnemyState.Idle) {
                enemy.WakeUp();
            }
            SetTargetCharacter(enemy, source);
        }
    }
}
