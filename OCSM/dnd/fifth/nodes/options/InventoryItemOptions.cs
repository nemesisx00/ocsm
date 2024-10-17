using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Nodes;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class InventoryItemOptions : DynamicOption
{
	protected override void refreshMetadata()
	{
		if(metadataManager.Container is DndFifthContainer container)
		{
			var index = Selected;
			
			Clear();
			
			if(EmptyOption)
				AddItem(string.Empty);
			
			foreach(var i in container.Items)
				AddItem(i.Name);
			
			Selected = index;
		}
	}
}
