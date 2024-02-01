using Godot;
using System;

public partial class SmoothCamera : Camera3D
{
	[Export]
	public float speed = 44.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		float weight = Math.Clamp(speed * (float)delta, 0.0f, 1.0f);
	 	this.GlobalTransform = this.GlobalTransform.InterpolateWith(this.GetParent<Node3D>().GlobalTransform, weight);
		this.GlobalPosition = this.GetParent<Node3D>().GlobalPosition;
    }
}
