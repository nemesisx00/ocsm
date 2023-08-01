using System;
using Godot;
using Ocsm.Dnd.Fifth.Inventory;

namespace Ocsm.Nodes.Dnd.Fifth;

public partial class WeaponTypeOptions : OptionButton
{
	public override void _Ready()
	{
		Clear();
		AddItem(String.Empty);
		AddItem(ItemWeapon.WeaponType.SimpleMelee.GetLabel());
		AddItem(ItemWeapon.WeaponType.SimpleRanged.GetLabel());
		AddItem(ItemWeapon.WeaponType.MartialMelee.GetLabel());
		AddItem(ItemWeapon.WeaponType.MartialRanged.GetLabel());
	}
}
