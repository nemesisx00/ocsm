using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Nodes;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class RaceOptionsButton : CustomOption
{
	protected override void refreshMetadata()
	{
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			var index = Selected;
			
			Clear();
			
			AddItem(string.Empty);
			dfc.Races.ForEach(r => AddItem(r.Name));
			Selected = index;
		}
	}
}
