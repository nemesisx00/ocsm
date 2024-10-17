using System;
using System.Linq;
using Godot;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class AbilityOptionsButton : OptionButton
{
	[Export]
	public bool EmptyOption { get; set; }
	
	public override void _Ready()
	{
		if(EmptyOption)
			AddItem(string.Empty);
		
		foreach(var label in Enum.GetValues<Abilities>().Select(a => a.GetLabel()))
			AddItem(label);
	}
}
