using System;
using OCSM.DnD.Fifth.Meta;

namespace OCSM.Nodes.DnD.Fifth
{
	public partial class RaceOptionsButton : CustomOption
	{
		protected override void refreshMetadata()
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				var index = Selected;
				
				Clear();
				
				AddItem(String.Empty);
				dfc.Races.ForEach(r => AddItem(r.Name));
				Selected = index;
			}
		}
	}
}
