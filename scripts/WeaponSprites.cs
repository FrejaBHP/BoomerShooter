using Godot;
public static class WeaponSprites {
    public static Texture2D[] S_W_PF = new Texture2D[3];
    public static void IndexWeaponSprites() {
        S_W_PF[0] = GD.Load<Texture2D>("res://textures/weapons/pitchfork/w_pf.png");
        S_W_PF[1] = GD.Load<Texture2D>("res://textures/weapons/pitchfork/w_pf_atk0.png");
        S_W_PF[2] = GD.Load<Texture2D>("res://textures/weapons/pitchfork/w_pf_atk1.png");
    }
}
