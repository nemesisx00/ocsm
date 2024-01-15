using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Nodes;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class WeaponOptions : CustomOption
{
	protected override void refreshMetadata()
	{
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			var index = Selected;
			
			Clear();
			AddItem(string.Empty);
			dfc.Weapons.ForEach(w => AddItem(w.Name));
			
			Selected = index;
			_ = EmitSignal(CustomOption.SignalName.ItemsChanged);
		}
	}
}
