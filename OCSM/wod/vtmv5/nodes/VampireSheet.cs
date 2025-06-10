using Ocsm.Nodes;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Wod.VtmV5.Nodes;

public partial class VampireSheet : CharacterSheet<Vampire>, ICharacterSheet
{
	private MetadataManager metadataManager;
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(MetadataManager.NodePath);
		SheetData ??= new();
	}
}
