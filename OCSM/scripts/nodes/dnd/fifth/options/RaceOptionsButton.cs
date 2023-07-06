using System;
using Ocsm.Dnd.Fifth.Meta;

namespace Ocsm.Nodes.Dnd.Fifth
{
	public partial class RaceOptionsButton : CustomOption
	{
		protected override void refreshMetadata()
		{
			if(metadataManager.Container is DndFifthContainer dfc)
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
