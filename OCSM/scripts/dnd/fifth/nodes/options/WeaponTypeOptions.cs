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
		
		Enum.GetValues<ItemWeapon.WeaponTypes>()
			.Select(wt => wt.GetLabel())
			.ToList()
			.ForEach(label => AddItem(label));
	}
}
