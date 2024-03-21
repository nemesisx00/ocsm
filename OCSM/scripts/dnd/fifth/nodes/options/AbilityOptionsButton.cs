using System;
using System.Linq;
using Ocsm.Dnd.Fifth;

namespace Ocsm.Nodes.Dnd.Fifth;

public partial class AbilityOptionsButton : CustomOption
{
	public override void _Ready() => refreshMetadata();
	
	protected override void refreshMetadata()
	{
		var index = Selected;
		
		Clear();
		AddItem(string.Empty);
		Enum.GetValues<Abilities>()
			.Select(a => a.GetLabel())
			.ToList()
			.ForEach(label => AddItem(label));
		
		Selected = index;
	}
}
