using System;
using Ocsm.Dnd.Fifth.Meta;

namespace Ocsm.Nodes.Dnd.Fifth;

public partial class InventoryItemOptions : CustomOption
{
	protected override void refreshMetadata()
	{
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			var index = Selected;
			
			Clear();
			AddItem(String.Empty);
			dfc.AllItems.ForEach(i => AddItem(i.Name));
			
			Selected = index;
		}
	}
}
