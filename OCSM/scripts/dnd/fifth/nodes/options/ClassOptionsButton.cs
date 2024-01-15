using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Nodes;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class ClassOptionsButton : CustomOption
{
	protected override void refreshMetadata()
	{
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			var index = Selected;
			
			Clear();
			AddItem(string.Empty);
			dfc.Classes.ForEach(c => AddItem(c.Name));
			
			Selected = index;
		}
	}
}
