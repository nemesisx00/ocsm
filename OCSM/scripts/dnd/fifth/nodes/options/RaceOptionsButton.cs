using System.Linq;
using Ocsm.Dnd.Fifth.Meta;

namespace Ocsm.Nodes.Dnd.Fifth;

public partial class RaceOptionsButton : CustomOption
{
	protected override void refreshMetadata()
	{
		if(metadataManager.Container is DndFifthContainer container)
		{
			var index = Selected;
			
			Clear();
			
			AddItem(string.Empty);
			
			container.Featurefuls
				.Where(f => f.Type == MetadataType.Dnd5eRace)
				.Select(f => f.Name)
				.ToList()
				.ForEach(label => AddItem(label));
			
			Selected = index;
		}
	}
}
