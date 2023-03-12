using System.Linq;
using OCSM.CoD.CtL.Meta;
using OCSM.Nodes.Autoload;

namespace OCSM.Nodes.CoD.CtL
{
	public partial class ContractTypeButton : CustomOption
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
			if(metadataManager.Container is CoDChangelingContainer ccc)
				replaceItems(ccc.ContractTypes.Select(ct => ct.Name).ToList());
		}
	}
}
