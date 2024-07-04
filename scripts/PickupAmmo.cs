using Godot;
using Godot.Collections;
using System;

public partial class PickupAmmo : Area3D {
	[Export]
	public string TextAppend { get; private set; }

	[Export]
	public Ammotype Type { get; private set; }

	[Export]
	public int Amount { get; private set; }

	[Export]
	public AudioStreamWav PickupSound { get; private set; }

	public void OnPlayerEntered(Node3D node) {
		if (node.IsInGroup("Player")) {
			bsPlayer player = node as bsPlayer;

			if (player.PlayerAmmo[(int)Type].Ammo != player.PlayerAmmo[(int)Type].Max) {
				player.GiveAmmo(Type, Amount);
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
