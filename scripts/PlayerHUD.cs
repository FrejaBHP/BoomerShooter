using Godot;
using System;

public partial class PlayerHUD : Control {
	private readonly PackedScene systemLine = GD.Load<PackedScene>("res://hud/system_line.tscn");

	private Label labelHealth;
	private Label labelAmmo;
	private TextureProgressBar chargeBar;

	private int chargeBarProgress = 0;
	public int ChargeBarProgress {
		get { return chargeBarProgress; }
		set {
			if (value > 120) {
				chargeBarProgress = 120;
			}
			else {
				chargeBarProgress = value;
				UpdateChargeBar();
			}
		}
	}

	private int myVar;
	public int MyProperty
	{
		get { return myVar; }
		set { myVar = value; }
	}
	

	private VBoxContainer pickupTextContainer;

    public override void _Ready() {
		labelHealth = GetNode<Label>("Bottom/HealthAmmoPivot/HealthAmmoTexture/HealthLabel");
        labelAmmo = GetNode<Label>("Bottom/HealthAmmoPivot/HealthAmmoTexture/AmmoLabel");
		chargeBar = GetNode<TextureProgressBar>("ChargeBar");
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

	public void ShowChargeBar() {
		chargeBar.Visible = true;
	}

	public void HideChargeBar() {
		chargeBar.Visible = false;
	}

	private void UpdateChargeBar() {
		chargeBar.Value = chargeBarProgress;
	}
}
