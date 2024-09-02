using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Nodes;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class FeatureOptionsButton : CustomOption
{
	protected override void refreshMetadata()
	{
		if(metadataManager.Container is DndFifthContainer container)
		{
			var index = Selected;
			
			Clear();
			
			if(EmptyOption)
				AddItem(string.Empty);
			
			container.Features.ForEach(f => AddItem(f.Name));
			
			Selected = index;
		}
	}
}
