using Godot;
using System;
using System.Linq;
using Ocsm.Nodes;

namespace Ocsm.Dnd.Fifth.Nodes;

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
