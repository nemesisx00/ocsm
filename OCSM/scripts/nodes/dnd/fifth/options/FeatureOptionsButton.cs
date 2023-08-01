using System;
using Ocsm.Dnd.Fifth.Meta;

namespace Ocsm.Nodes.Dnd.Fifth;

public partial class FeatureOptionsButton : CustomOption
{
	protected override void refreshMetadata()
	{
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			var index = Selected;
			
			Clear();
			AddItem(String.Empty);
			dfc.Features.ForEach(f => AddItem(f.Name));
			
			Selected = index;
		}
	}
}
