using Godot;
using System;
using System.Linq;
using Ocsm.Dnd.Fifth.Inventory;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class WeaponTypeOptions : OptionButton
{
	[Export]
	public bool EmptyOption { get; set; }
	
	public override void _Ready()
	{
		if(EmptyOption)
			AddItem(string.Empty);
		
		foreach(var label in Enum.GetValues<WeaponTypes>().Select(wt => wt.GetLabel()))
			AddItem(label);
	}
}
