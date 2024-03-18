using System;
using Ocsm.Dnd.Fifth;

namespace Ocsm.Nodes.Dnd.Fifth;

public partial class AbilityOptionsButton : CustomOption
{
	public override void _Ready()
	{
		refreshMetadata();
	}
	
	protected override void refreshMetadata()
	{
		var index = Selected;
		
		Clear();
		AddItem(String.Empty);
		AbilityInfo.Abilities.asList().ForEach(label => AddItem(label));
		
		Selected = index;
	}
}
