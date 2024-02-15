using Godot;
using System;

public partial class Enemy : CharacterBody3D
{
	[Export]
	public float speed = 5.0f;
	[Export]
	public float aggroRange = 12.0f;
	[Export]
	public float attackRange = 1.5f;
	[Export]
	public int maxHitPoints = 100;
	[Export]
	public int damage = 20;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	private NavigationAgent3D m_navAgent;
	private AnimationTree m_animTree;
	private AnimationNodeStateMachinePlayback m_playback;
	private Player m_player;
	private bool m_provoked = false;
	private int m_hitpoints = 100;

	public int HitPoints
	{
		get { return m_hitpoints; }
		set 
		{ 
			m_hitpoints = value; 
			if (m_hitpoints <= 0)
			{
				this.QueueFree();
			}
			else
			{
				this.m_provoked = true;
			}
		}
	}

    public override void _Ready()
    {
        base._Ready();
		m_navAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
		m_animTree = GetNode<AnimationTree>("AnimationTree");
		m_player = (Player)GetTree().GetFirstNodeInGroup("player");
		m_hitpoints = maxHitPoints;
		m_provoked = false;
		m_playback = m_animTree.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
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
				m_playback.Travel("attack");
				//m_animPlayer.Play("attack");
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
		m_player.HitPoints = m_player.HitPoints - damage;
	}
}
