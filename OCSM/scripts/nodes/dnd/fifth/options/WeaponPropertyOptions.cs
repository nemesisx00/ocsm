using System;
using Godot;
using Ocsm.Dnd.Fifth.Inventory;

namespace Ocsm.Nodes.Dnd.Fifth;

public partial class WeaponPropertyOptions : OptionButton
{
	public override void _Ready()
	{
		Clear();
		AddItem(String.Empty);
		AddItem(ItemWeapon.WeaponProperties.Ammunition.GetLabel());
		AddItem(ItemWeapon.WeaponProperties.Finesse.GetLabel());
		AddItem(ItemWeapon.WeaponProperties.Heavy.GetLabel());
		AddItem(ItemWeapon.WeaponProperties.Light.GetLabel());
		AddItem(ItemWeapon.WeaponProperties.Loading.GetLabel());
		AddItem(ItemWeapon.WeaponProperties.Range.GetLabel());
		AddItem(ItemWeapon.WeaponProperties.Reach.GetLabel());
		AddItem(ItemWeapon.WeaponProperties.Silvered.GetLabel());
		AddItem(ItemWeapon.WeaponProperties.Special.GetLabel());
		AddItem(ItemWeapon.WeaponProperties.Thrown.GetLabel());
		AddItem(ItemWeapon.WeaponProperties.TwoHanded.GetLabel());
		AddItem(ItemWeapon.WeaponProperties.Versatile.GetLabel());
	}
}
