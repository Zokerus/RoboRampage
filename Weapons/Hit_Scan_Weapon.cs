using Godot;
using System;
using System.Diagnostics;
using System.IO;

public partial class Hit_Scan_Weapon : Node3D
{
	[Export]
	public float fireRate = 14.0f;
	[Export]
	public float recoil = 0.05f;
	[Export]
	public float weaponDamage = 15;
	[Export]
	public Node3D weaponMesh = null;
	[Export]
	public GpuParticles3D muzzleFlash = null;
	//[Export]
	//public String sparksPath = null;
	[Export]
	public PackedScene sparksScene = null;
	

	private Timer m_coolDownTimer;
	private RayCast3D m_rayCast3D;
	private Vector3 m_weaponPosition;
	private GpuParticles3D m_muzzelFlash;
	private PackedScene m_sparks;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		m_coolDownTimer = GetNode<Timer>("CoolDownTimer");
		m_rayCast3D = GetNode<RayCast3D>("RayCast3D");
		m_weaponPosition = weaponMesh.Position;
        //m_sparks = ResourceLoader.Load<PackedScene>(sparksPath);
		if (muzzleFlash != null )
		{
			muzzleFlash.Lifetime = 1.0f / fireRate;
		}
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (Input.IsActionPressed("fire", true) && m_coolDownTimer.IsStopped())
		{ 
			Shoot();
        }

		weaponMesh.Position = weaponMesh.Position.Lerp(m_weaponPosition, (float)delta * 10.0f);
    }

	private void Shoot()
	{
        m_coolDownTimer.Start(1.0f / fireRate);
		weaponMesh.Position = new Vector3(weaponMesh.Position.X, weaponMesh.Position.Y, weaponMesh.Position.Z + recoil);
        muzzleFlash.Restart();
		if (m_rayCast3D.IsColliding())
		{
			if(m_rayCast3D.GetCollider() is Enemy enemy)
			{
				enemy.HitPoints = enemy.HitPoints - (int)weaponDamage;
			}
			  if (sparksScene != null)
			{
				GpuParticles3D impactSparks = sparksScene.Instantiate<GpuParticles3D>();
				AddChild(impactSparks);
				impactSparks.GlobalPosition = m_rayCast3D.GetCollisionPoint();
			}
		}
    }
}
