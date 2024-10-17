using System;
using System.Linq;
using Godot;
using Ocsm.Dnd.Fifth.Inventory;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class ArmorTypeOptions : OptionButton
{
	[Export]
	public bool EmptyOption { get; set; }
	
	public override void _Ready()
	{
		if(EmptyOption)
			AddItem(string.Empty);
		
		foreach(var label in Enum.GetValues<ArmorTypes>().Select(at => at.GetLabel()))
			AddItem(label);
	}
}
