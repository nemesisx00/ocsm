using System;
using System.Linq;
using Ocsm.Nodes;

namespace Ocsm.Dnd.Fifth.Nodes;

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
