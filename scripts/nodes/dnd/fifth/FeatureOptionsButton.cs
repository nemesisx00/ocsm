using Godot;
using OCSM.Nodes.Autoload;
using OCSM.DnD.Fifth.Meta;

namespace OCSM.Nodes.DnD.Fifth
{
	public class FeatureOptionsButton : OptionButton
	{
		private MetadataManager metadataManager;
		
		public override void _Ready()
		{
			metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
			metadataManager.Connect(nameof(MetadataManager.MetadataSaved), this, nameof(refreshMetadata));
			metadataManager.Connect(nameof(MetadataManager.MetadataLoaded), this, nameof(refreshMetadata));
			
			refreshMetadata();
		}
		
		private void refreshMetadata()
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				var index = Selected;
				
				Clear();
				
				AddItem("");
				foreach(var feature in dfc.Features)
				{
					AddItem(feature.Name);
				}
				
				Selected = index;
			}
		}
	}
}
