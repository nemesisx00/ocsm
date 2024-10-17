using Ocsm.Nodes.Autoload;

namespace Ocsm.Nodes;

public abstract partial class DynamicOption : FixedOption
{
	protected MetadataManager metadataManager;
	
	public override void _ExitTree()
	{
		metadataManager.MetadataLoaded -= refreshMetadata;
		metadataManager.MetadataSaved -= refreshMetadata;
		
		base._ExitTree();
	}
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(MetadataManager.NodePath);
		metadataManager.MetadataLoaded += refreshMetadata;
		metadataManager.MetadataSaved += refreshMetadata;
		
		refreshMetadata();
	}
}
