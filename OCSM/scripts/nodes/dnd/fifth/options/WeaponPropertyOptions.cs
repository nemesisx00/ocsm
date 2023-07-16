using System;
using Godot;
using Ocsm.Dnd.Fifth.Inventory;

namespace Ocsm.Nodes.Dnd.Fifth
{
	public partial class WeaponPropertyOptions : OptionButton
	{
		public override void _Ready()
		{
			Clear();
			AddItem(String.Empty);
			AddItem(ItemWeapon.WeaponProperty.Ammunition.GetLabel());
			AddItem(ItemWeapon.WeaponProperty.Finesse.GetLabel());
			AddItem(ItemWeapon.WeaponProperty.Heavy.GetLabel());
			AddItem(ItemWeapon.WeaponProperty.Light.GetLabel());
			AddItem(ItemWeapon.WeaponProperty.Loading.GetLabel());
			AddItem(ItemWeapon.WeaponProperty.Range.GetLabel());
			AddItem(ItemWeapon.WeaponProperty.Reach.GetLabel());
			AddItem(ItemWeapon.WeaponProperty.Silvered.GetLabel());
			AddItem(ItemWeapon.WeaponProperty.Special.GetLabel());
			AddItem(ItemWeapon.WeaponProperty.Thrown.GetLabel());
			AddItem(ItemWeapon.WeaponProperty.TwoHanded.GetLabel());
			AddItem(ItemWeapon.WeaponProperty.Versatile.GetLabel());
		}
	}
}
