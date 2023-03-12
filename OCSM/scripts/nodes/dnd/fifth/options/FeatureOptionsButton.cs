using System;
using OCSM.DnD.Fifth.Meta;

namespace OCSM.Nodes.DnD.Fifth
{
	public partial class FeatureOptionsButton : CustomOption
	{
		protected override void refreshMetadata()
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				var index = Selected;
				
				Clear();
				AddItem(String.Empty);
				dfc.Features.ForEach(f => AddItem(f.Name));
				
				Selected = index;
			}
		}
	}
}
