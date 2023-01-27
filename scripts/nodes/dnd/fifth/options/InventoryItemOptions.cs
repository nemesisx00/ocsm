using System;
using OCSM.DnD.Fifth.Meta;

namespace OCSM.Nodes.DnD.Fifth
{
	public class InventoryItemOptions : CustomOption
	{
		protected override void refreshMetadata()
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				var index = Selected;
				
				Clear();
				AddItem(String.Empty);
				dfc.AllItems.ForEach(i => AddItem(i.Name));
				
				Selected = index;
			}
		}
	}
}
