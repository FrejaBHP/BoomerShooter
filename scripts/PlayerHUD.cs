using Godot;
using System;

public partial class PlayerHUD : Control {
	PackedScene systemLine = GD.Load<PackedScene>("res://HUD/system_line.tscn");

	private Label labelHealth;
	private Label labelAmmo;

	private VBoxContainer pickupTextContainer;

    public override void _Ready() {
		labelHealth = GetNode<Label>("Bottom/HealthAmmoPivot/HealthAmmoTexture/HealthLabel");
        labelAmmo = GetNode<Label>("Bottom/HealthAmmoPivot/HealthAmmoTexture/AmmoLabel");
		pickupTextContainer = GetNode<VBoxContainer>("PickupTextContainer");
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

	public void PrintPickupStatusText(string append) {
		SystemLine line = systemLine.Instantiate() as SystemLine;
		
		if (pickupTextContainer.GetChildCount() >= 4) {
			pickupTextContainer.GetChild(0).QueueFree();
		}

		pickupTextContainer.AddChild(line);
		line.Visible = true;
		line.SetTextStartTimer($"Picked up {append}");
	}
}
