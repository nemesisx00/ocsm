using System.Linq;
using Ocsm.Cofd.Ctl.Meta;
using Ocsm.Nodes;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Cofd.Ctl.Nodes;

public partial class SeemingOptionButton : CustomOption
{
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(MetadataManager.NodePath);
		metadataManager.MetadataLoaded += refreshMetadata;
		metadataManager.MetadataSaved += refreshMetadata;
		
		refreshMetadata();
	}
	
	protected override void refreshMetadata()
	{
		if(metadataManager.Container is CofdChangelingContainer ccc)
			replaceItems(ccc.Seemings.Select(s => s.Name).ToList());
	}
}
