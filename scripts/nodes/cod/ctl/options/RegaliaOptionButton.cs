using Godot;
using OCSM.CoD.CtL.Meta;
using OCSM.Nodes.Autoload;

namespace OCSM.Nodes.CoD.CtL
{
	public partial class RegaliaOptionButton : OptionButton
	{
		[Export]
		public bool emptyOption = true;
		[Export]
		private bool includeNonRegalia = false;
		
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
				
				foreach(var regalia in ccc.Regalias)
				{
					AddItem(regalia.Name);
				}
				
				Selected = index;
			}
		}
	}
}
