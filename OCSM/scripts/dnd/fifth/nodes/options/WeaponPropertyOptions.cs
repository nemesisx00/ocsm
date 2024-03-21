using Godot;
using System;
using System.Linq;
using Ocsm.Dnd.Fifth.Inventory;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class WeaponPropertyOptions : OptionButton
{
	public override void _Ready()
	{
		Clear();
		
		Enum.GetValues<ItemWeapon.WeaponProperties>()
			.Select(wp => wp.GetLabel())
			.ToList()
			.ForEach(label => AddItem(label));
	}
}
