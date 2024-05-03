using Godot;
using System;

public partial class PlayerHUD : Control {
	private Label labelHealth;
	private Label labelAmmo;

    public override void _Ready() {
		labelHealth = GetNode<Label>("Bottom/HealthAmmoPivot/HealthAmmoTexture/HealthLabel");
        labelAmmo = GetNode<Label>("Bottom/HealthAmmoPivot/HealthAmmoTexture/AmmoLabel");
    }

    public void HUDUpdatePlayerHealth(int health) {
		labelHealth.Text = health.ToString();
	}

	public void HUDUpdatePlayerAmmo(int ammo) {
		if (ammo != -1) {
			labelAmmo.Text = ammo.ToString();
		}
		else {
			labelAmmo.Text = "";
		}
	}
}
