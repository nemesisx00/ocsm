using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Nodes;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class WeaponOptions : CustomOption
{
	protected override void refreshMetadata()
	{
		if(metadataManager.Container is DndFifthContainer container)
		{
			var index = Selected;
			
			Clear();
			AddItem(string.Empty);
			container.Weapons.ForEach(w => AddItem(w.Name));
			
			Selected = index;
			
			EmitSignal(CustomOption.SignalName.ItemsChanged);
		}
	}
}
