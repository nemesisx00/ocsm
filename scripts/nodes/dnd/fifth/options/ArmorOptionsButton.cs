using System;
using OCSM.DnD.Fifth.Meta;

namespace OCSM.Nodes.DnD.Fifth
{
	public partial class ArmorOptionsButton : CustomOption
	{
		protected override void refreshMetadata()
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
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
