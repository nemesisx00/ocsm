using System;
using Ocsm.Dnd.Fifth.Meta;

namespace Ocsm.Nodes.Dnd.Fifth;

public partial class WeaponOptions : CustomOption
{
	protected override void refreshMetadata()
	{
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			var index = Selected;
			
			Clear();
			AddItem(String.Empty);
			dfc.Weapons.ForEach(w => AddItem(w.Name));
			
			Selected = index;
			
			EmitSignal(nameof(ItemsChanged));
		}
	}
}
