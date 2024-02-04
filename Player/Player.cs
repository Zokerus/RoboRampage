using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[Export]
	public float Speed = 5.0f;
	[Export]
	public float JumpHeight = 4.5f;
	[Export]
	public float mouseHSensetvity = 0.001f;
	[Export]
    public float mouseVSensetvity = 0.001f;
    [Export]
    public int maxHitPoints = 100;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	private float m_fallMultiplier = 2.0f;
	private Vector2 m_mouseMotion = Vector2.Zero;
    private int m_hitpoints = 100;
    private Node3D m_cameraPivot;
	private AnimationPlayer m_damageAnimationPlayer;
	private GameOverMenu m_gameOverMenu;

    public int HitPoints
    {
        get { return m_hitpoints; }
        set
        {
			if (m_hitpoints > value)
			{
                m_damageAnimationPlayer.Stop();
				m_damageAnimationPlayer.Play("take_damage");

            }
            m_hitpoints = value;
            if (m_hitpoints <= 0)
            {
				m_gameOverMenu.GameOver();
            }
        }
    }


    public override void _Ready()
    {
        base._Ready();
		m_cameraPivot = GetNode<Node3D>("CameraPivot");
		Input.MouseMode = Input.MouseModeEnum.Captured;
		m_hitpoints = maxHitPoints;
        m_damageAnimationPlayer = GetNode<AnimationPlayer>("CanvasLayer/DamageTexture/DamageAnimationPlayer");
		m_gameOverMenu = GetNode<GameOverMenu>("GameOverScreen");
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;
		HandleCameraRotation();

		// Add the gravity.
		if (!IsOnFloor())
			if (velocity.Y > 0)
			{
				velocity.Y -= gravity * (float)delta;
			}
			else
			{
                velocity.Y -= gravity * (float)delta * m_fallMultiplier;
            }

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = Mathf.Sqrt(JumpHeight * 2.0f * gravity);

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("left", "right", "forward", "backward");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);

		if(@event is InputEventMouseMotion mouseMotion && Input.MouseMode == Input.MouseModeEnum.Captured)
		{
			m_mouseMotion = -mouseMotion.Relative;
		}

		if(@event.IsActionPressed("escape"))
		{
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }
    }

	private void HandleCameraRotation()
	{
		this.RotateY(m_mouseMotion.X * mouseHSensetvity);
		this.m_cameraPivot.RotateX(m_mouseMotion.Y * mouseVSensetvity);
		this.m_cameraPivot.RotationDegrees = new Vector3(Math.Clamp(this.m_cameraPivot.RotationDegrees.X, -90, 90), this.m_cameraPivot.RotationDegrees.Y, this.m_cameraPivot.RotationDegrees.Z);
		m_mouseMotion = Vector2.Zero;
	}
}
