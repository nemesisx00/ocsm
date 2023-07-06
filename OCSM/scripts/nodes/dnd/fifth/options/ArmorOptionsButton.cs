using System;
using Ocsm.Dnd.Fifth.Meta;

namespace Ocsm.Nodes.Dnd.Fifth
{
	public partial class ArmorOptionsButton : CustomOption
	{
		protected override void refreshMetadata()
		{
			if(metadataManager.Container is DndFifthContainer dfc)
			{
				var index = Selected;
				
				Clear();
				AddItem(String.Empty);
				dfc.Armors.ForEach(a => AddItem(a.Name));
				
				Selected = index;
				
				EmitSignal(nameof(ItemsChanged));
			}
		}
	}
}
