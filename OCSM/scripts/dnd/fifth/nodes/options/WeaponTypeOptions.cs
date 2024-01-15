using Godot;
using System;
using System.Linq;
using Ocsm.Dnd.Fifth.Inventory;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class WeaponTypeOptions : OptionButton
{
	public override void _Ready()
	{
		Clear();
		Enum.GetValues<ItemWeapon.WeaponType>()
			.ToList()
			.ForEach(wt => AddItem(wt.GetLabel()));
	}
}
