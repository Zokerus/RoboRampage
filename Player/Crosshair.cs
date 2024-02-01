using Godot;
using System;

public partial class Crosshair : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _Draw()
    {
        base._Draw();
        DrawCircle(Vector2.Zero, 4.0f, Colors.DimGray);
        DrawCircle(Vector2.Zero, 3.0f, Colors.White);

		DrawLine(new Vector2(16, 0), new Vector2(24, 0), Colors.White, 2.0f);
        DrawLine(new Vector2(0, 16), new Vector2(0, 24), Colors.White, 2.0f);
        DrawLine(new Vector2(-16, 0), new Vector2(-24, 0), Colors.White, 2.0f);
        DrawLine(new Vector2(0, -16), new Vector2(0, -24), Colors.White, 2.0f);
    }
}
