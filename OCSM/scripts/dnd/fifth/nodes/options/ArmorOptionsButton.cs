using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Nodes;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class ArmorOptionsButton : CustomOption
{
	protected override void refreshMetadata()
	{
		if(metadataManager.Container is DndFifthContainer container)
		{
			var index = Selected;
			
			Clear();
			AddItem(string.Empty);
			container.Armors.ForEach(a => AddItem(a.Name));
			
			Selected = index;
			
			EmitSignal(CustomOption.SignalName.ItemsChanged);
		}
	}
}
