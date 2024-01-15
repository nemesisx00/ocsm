using System.Linq;
using Ocsm.Cofd.Ctl.Meta;
using Ocsm.Nodes;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Cofd.Ctl.Nodes;

public partial class ContractRegaliaOptionButton : CustomOption
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
		{
			var list = ccc.Regalias.Select(r => r.Name)
				.Union(ccc.Courts.Select(c => c.Name))
				.ToList();
			
			list.Add(ContractRegalia.Goblin.Name);
			replaceItems(list);
		}
	}
}
