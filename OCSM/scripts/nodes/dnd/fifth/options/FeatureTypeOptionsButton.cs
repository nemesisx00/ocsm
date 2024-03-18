using Godot;
using System;
using Ocsm.Dnd.Fifth;
using System.Linq;

namespace Ocsm.Nodes.Dnd.Fifth;

public partial class FeatureTypeOptionsButton : CustomOption
{
	[Export]
	public bool emptyOption = true;
	
	public override void _Ready()
	{
		if(emptyOption)
			AddItem(string.Empty);
		
		Enum.GetValues<FeatureTypes>()
			.Select(ft => ft.ToString())
			.ToList()
			.ForEach(label => AddItem(label));
	}
}
