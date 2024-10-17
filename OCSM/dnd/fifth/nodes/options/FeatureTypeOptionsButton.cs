using Godot;
using System;
using System.Linq;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class FeatureTypeOptionsButton : OptionButton
{
	[Export]
	public bool EmptyOption { get; set; }
	
	public override void _Ready()
	{
		if(EmptyOption)
			AddItem(string.Empty);
		
		foreach(var label in Enum.GetValues<FeatureTypes>().Select(ft => ft.ToString()))
			AddItem(label);
	}
}
