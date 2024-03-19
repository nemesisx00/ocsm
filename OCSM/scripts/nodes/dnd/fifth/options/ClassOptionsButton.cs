using Ocsm.Dnd.Fifth.Meta;

namespace Ocsm.Nodes.Dnd.Fifth;

public partial class ClassOptionsButton : CustomOption
{
	protected override void refreshMetadata()
	{
		if(metadataManager.Container is DndFifthContainer container)
		{
			var index = Selected;
			
			Clear();
			AddItem(string.Empty);
			container.Classes.ForEach(c => AddItem(c.Name));
			
			Selected = index;
		}
	}
}
