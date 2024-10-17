using Godot;
using System;
using System.Linq;
using Ocsm.Dnd.Fifth.Inventory;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class WeaponPropertyOptions : OptionButton
{
	[Export]
	public bool EmptyOption { get; set; }
	
	public override void _Ready()
	{
		if(EmptyOption)
			AddItem(string.Empty);
		
		foreach(var label in Enum.GetValues<WeaponProperties>().Select(wp => wp.GetLabel()))
			AddItem(label);
	}
}
