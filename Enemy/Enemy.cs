using Godot;
using System;

public partial class Enemy : CharacterBody3D
{
	[Export]
	public float speed = 5.0f;
	[Export]
	public float jumpHeight = 1.0f;
	[Export]
	public float aggroRange = 12.0f;
	[Export]
	public float attackRange = 1.5f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	private NavigationAgent3D m_navAgent;
	private AnimationPlayer m_animPlayer;
	private Player m_player;
	private bool m_provoked = false;

    public override void _Ready()
    {
        base._Ready();
		m_navAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
		m_animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		m_player = (Player)GetTree().GetFirstNodeInGroup("player");
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
		if (m_provoked)
		{
			m_navAgent.TargetPosition = m_player.GlobalPosition;
		}
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;
		Vector3 next_Position = m_navAgent.GetNextPathPosition();

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		float distance = this.GlobalPosition.DistanceTo(m_player.GlobalPosition);
		Vector3 direction = this.GlobalPosition.DirectionTo(next_Position);

		if (distance <= aggroRange)
		{
			m_provoked = true;
		}

		if(m_provoked) 
		{
			if (distance <= attackRange)
			{
				m_animPlayer.Play("attack");
			}
		}

		if (direction != Vector3.Zero)
		{
			LookAtTarget(direction);
			velocity.X = direction.X * speed;
			velocity.Z = direction.Z * speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public void LookAtTarget(Vector3 direction)
	{
		Vector3 adjustedDirection = direction;
		adjustedDirection.Y = 0;
		this.LookAt(this.GlobalPosition + adjustedDirection, Vector3.Up, true);
	}

	public void Attack()
	{

	}
}