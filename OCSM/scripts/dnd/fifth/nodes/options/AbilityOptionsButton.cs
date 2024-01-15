using Ocsm.Nodes;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class AbilityOptionsButton : CustomOption
{
	public override void _Ready()
	{
		refreshMetadata();
	}
	
	protected override void refreshMetadata()
	{
		var index = Selected;
		
		Clear();
		AddItem(string.Empty);
		Ability.Names.AsList().ForEach(label => AddItem(label));
		
		Selected = index;
	}
}
