using Godot;
using OCSM.CoD.CtL.Meta;
using OCSM.Nodes.Autoload;

namespace OCSM.Nodes.CoD.CtL
{
	public partial class ContractTypeButton : OptionButton
	{
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
				
				AddItem("");
				foreach(var contractType in ccc.ContractTypes)
				{
					AddItem(contractType.Name);
				}
				
				Selected = index;
			}
		}
	}
}
