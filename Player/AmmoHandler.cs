using Godot;
using Godot.Collections;
using System;

public partial class AmmoHandler : Node
{
	public enum Ammo_Type
	{
		BULLET,
		SMALL_BULLET
	}

	[Export] public Label ammoLabel;

	private Dictionary<Ammo_Type, int> m_ammoStorage = new Dictionary<Ammo_Type, int>() { {Ammo_Type.BULLET, 10}, { Ammo_Type.SMALL_BULLET, 60 } };

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		//m_ammoStorage.Add(Ammo_Type.BULLET, 10);
		//m_ammoStorage.Add(Ammo_Type.SMALL_BULLET, 60);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public bool HasAmmo(Ammo_Type type)
	{
		return (m_ammoStorage[type] > 0);
	}

	public void UseAmmo(Ammo_Type type)
	{
		if (HasAmmo(type))
		{
			m_ammoStorage[type] -= 1;
			UpdateAmmoLabel(type);
		}
	}

	public void UpdateAmmoLabel(Ammo_Type type)
	{
		ammoLabel.Text = m_ammoStorage[type].ToString();

    }
}
