using Godot;
using System;

public partial class WeaponHandler : Node3D
{
	[Export]
	public Hit_Scan_Weapon Weapon_1 { get; set; }
	[Export]
	public Hit_Scan_Weapon Weapon_2 { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		Equip(Weapon_2);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);
		if(@event.IsActionPressed("weapon_1", false))
		{
			Equip(Weapon_1);
		}
		else if(@event.IsActionPressed("weapon_2", false))
		{
            Equip(Weapon_2);
        }

		if(@event.IsActionPressed("next_weapon", true))
		{
			NextWeapon();
		}
		else if (@event.IsActionPressed("prev_weapon", true))
		{
			PrevWeapon();
		}
    }

    private void Equip(Hit_Scan_Weapon active_weapon)
	{
		foreach(Hit_Scan_Weapon weapon in this.GetChildren())
		{
			if(weapon == active_weapon)
			{
				weapon.Show();
				weapon.SetProcess(true);
				weapon.AmmoHandler.UpdateAmmoLabel(weapon.ammoType);
			}
			else
			{
				weapon.Hide();
				weapon.SetProcess(false);
			}
		}
	}

	private int GetCurrentIndex()
	{
		for(int index = 0; index < this.GetChildren().Count; index++) 
		{
			if(this.GetChild<Hit_Scan_Weapon>(index).Visible)
			{
				return index;
			}
		}
		return 0;
	}

	private void NextWeapon()
	{
		int index = GetCurrentIndex();
		index = Mathf.Wrap(index + 1, 0, this.GetChildren().Count);
		Equip(this.GetChild<Hit_Scan_Weapon>(index));
	}

    private void PrevWeapon()
    {
        int index = GetCurrentIndex();
        index = Mathf.Wrap(index - 1, 0, this.GetChildren().Count);
        Equip(this.GetChild<Hit_Scan_Weapon>(index));
    }
}
