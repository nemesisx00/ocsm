using Godot;
using OCSM;
using OCSM.CoD.CtL.Meta;

public class ContractTypeButton : OptionButton
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
