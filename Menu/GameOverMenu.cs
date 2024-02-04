using Godot;
using System;

public partial class GameOverMenu : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnRestartButtonClick()
	{
		GetTree().Paused = false;
		GetTree().ReloadCurrentScene();
	}

	public void OnQuitButtonClick()
	{
		GetTree().Quit();
	}

	public void GameOver()
	{
		GetTree().Paused = true;
		this.Show();
		Input.MouseMode = Input.MouseModeEnum.Visible;
	}
}
