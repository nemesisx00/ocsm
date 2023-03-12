using Godot;
using OCSM.CoD.CtL.Meta;
using OCSM.Nodes.Autoload;

namespace OCSM.Nodes.CoD.CtL
{
	public partial class ContractRegaliaOptionButton : OptionButton
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
				
				ccc.Regalias.ForEach(r => AddItem(r.Name));
				ccc.Courts.ForEach(c => AddItem(c.Name));
				AddItem("Goblin");
				
				Selected = index;
			}
		}
	}
}
