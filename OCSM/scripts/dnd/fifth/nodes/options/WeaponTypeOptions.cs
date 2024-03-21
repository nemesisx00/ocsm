using System;
using System.Linq;
using Godot;
using Ocsm.Dnd.Fifth.Inventory;

namespace Ocsm.Nodes.Dnd.Fifth;

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
