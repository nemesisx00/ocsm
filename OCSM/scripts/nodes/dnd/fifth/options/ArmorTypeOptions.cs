using System;
using System.Linq;
using Godot;
using Ocsm.Dnd.Fifth.Inventory;

namespace Ocsm.Nodes.Dnd.Fifth;

public partial class ArmorTypeOptions : OptionButton
{
	public override void _Ready()
	{
		Clear();
		AddItem(string.Empty);
		
		Enum.GetValues<ItemArmor.ArmorTypes>()
			.Select(at => at.GetLabel())
			.ToList()
			.ForEach(label => AddItem(label));
	}
}
