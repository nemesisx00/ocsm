using Ocsm.Dnd.Fifth.Meta;

namespace Ocsm.Nodes.Dnd.Fifth;

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
