using System.Linq;
using Ocsm.Cofd.Ctl.Meta;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Nodes.Cofd.Ctl;

public partial class KithOptionButton : CustomOption
{
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
		metadataManager.MetadataLoaded += refreshMetadata;
		metadataManager.MetadataSaved += refreshMetadata;
		
		refreshMetadata();
	}
	
	protected override void refreshMetadata()
	{
		if(metadataManager.Container is CofdChangelingContainer ccc)
			replaceItems(ccc.Kiths.Select(k => k.Name).ToList());
	}
}
