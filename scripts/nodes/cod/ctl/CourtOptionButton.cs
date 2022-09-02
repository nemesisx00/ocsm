using Godot;
using OCSM.CoD.CtL.Meta;
using OCSM.Nodes.Autoload;

namespace OCSM.Nodes.CoD.CtL
{
	public class CourtOptionButton : OptionButton
	{
		[Export]
		public bool emptyOption = true;
		
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
			if(metadataManager.Container is CoDChangelingContainer ccc)
			{
				var index = Selected;
				
				Clear();
				
				if(emptyOption)
					AddItem("");
				
				foreach(var court in ccc.Courts)
				{
					AddItem(court.Name);
				}
				
				Selected = index;
			}
		}
	}
}
