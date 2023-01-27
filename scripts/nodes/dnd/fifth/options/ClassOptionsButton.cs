using System;
using OCSM.DnD.Fifth.Meta;

namespace OCSM.Nodes.DnD.Fifth
{
	public class ClassOptionsButton : CustomOption
	{
		protected override void refreshMetadata()
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				var index = Selected;
				
				Clear();
				AddItem(String.Empty);
				dfc.Classes.ForEach(c => AddItem(c.Name));
				
				Selected = index;
			}
		}
	}
}
