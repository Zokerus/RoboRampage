using Godot;
using System;

public partial class Pickup : Area3D
{
	[Export] public AmmoHandler.Ammo_Type Type { get; set; }
	[Export] public int Amount { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnBodyEntered(Node3D body)
	{ 
		if(body is Player player)
		{
			player.m_ammoHandler.AddAmmo(Type, Amount);
			this.QueueFree();
		}
	}
}
