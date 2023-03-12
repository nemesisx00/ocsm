using Godot;
using OCSM.CoD.CtL.Meta;
using OCSM.Nodes.Autoload;

namespace OCSM.Nodes.CoD.CtL
{
	public partial class KithOptionButton : OptionButton
	{
		[Export]
		public bool emptyOption = true;
		
		private MetadataManager metadataManager;
		
		public override void _Ready()
		{
			metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
			metadataManager.MetadataLoaded += refreshMetadata;
			metadataManager.MetadataSaved += refreshMetadata;
			
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
				
				foreach(var kith in ccc.Kiths)
				{
					AddItem(kith.Name);
				}
				
				Selected = index;
			}
		}
	}
}
