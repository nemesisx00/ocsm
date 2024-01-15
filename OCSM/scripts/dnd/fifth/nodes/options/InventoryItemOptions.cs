using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Nodes;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class InventoryItemOptions : CustomOption
{
	protected override void refreshMetadata()
	{
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			var index = Selected;
			
			Clear();
			AddItem(string.Empty);
			dfc.AllItems.ForEach(i => AddItem(i.Name));
			
			Selected = index;
		}
	}
}
