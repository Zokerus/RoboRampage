using Godot;
using System;

public partial class Lava : Area3D
{
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
			player.HitPoints = 0;
		}
		if (body is Enemy enemy) 
		{
			enemy.HitPoints = 0;
		}
	}
}
