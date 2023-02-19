using System;
using OCSM.DnD.Fifth.Meta;

namespace OCSM.Nodes.DnD.Fifth
{
	public partial class WeaponOptions : CustomOption
	{
		protected override void refreshMetadata()
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
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
}
