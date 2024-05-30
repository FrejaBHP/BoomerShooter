using Godot;
using Godot.Collections;
using System;

public partial class PickupWeapon : Area3D {
	[Export]
	public string TextAppend { get; private set; }

	[Export]
	public WeaponType Type { get; private set; }
    
    [Export]
	public int Amount { get; private set; }

	[Export]
	public AudioStreamWav PickupSound { get; private set; }

	public override void _Ready() {
	
	}

	public void OnPlayerEntered(Node3D node) {
		if (node.IsInGroup("Player")) {
			bsPlayer player = node as bsPlayer;

			if (!player.HasWeapon[(int)Type]) {
                player.PickUpNewWeapon(Type);
                player.GiveAmmo(player.WeaponInventory[(int)Type].AmmoType, Amount);
		        player.PlayerHUD.PrintPickupStatusText(TextAppend);
				if (PickupSound != null) {
					player.MiscAudio.Stream = PickupSound;
					player.MiscAudio.Play();
				}
				QueueFree();
			}
		}
	}
}
