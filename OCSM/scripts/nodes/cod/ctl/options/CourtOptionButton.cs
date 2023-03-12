using System.Linq;
using OCSM.CoD.CtL.Meta;
using OCSM.Nodes.Autoload;

namespace OCSM.Nodes.CoD.CtL
{
	public partial class CourtOptionButton : CustomOption
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
				replaceItems(ccc.Courts.Select(c => c.Name).ToList());
		}
	}
}
