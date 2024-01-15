using Godot;
using Ocsm.Cofd.Ctl.Meta;
using Ocsm.Nodes;

namespace Ocsm.Cofd.Ctl.Nodes;

public partial class CourtEntry : BasicMetadataEntry
{
	protected override void entrySelected(long index)
	{
		var optionsButton = GetNode<OptionButton>(NodePaths.ExistingEntryName);
		var name = optionsButton.GetItemText((int)index);
		
		if(metadataManager.Container is CofdChangelingContainer ccc)
		{
			if(ccc.Courts.Find(c => c.Name.Equals(name)) is Court court)
			{
				LoadEntry(court);
				optionsButton.Deselect();
			}
		}
	}
	
	public override void RefreshMetadata()
	{
		if(metadataManager.Container is CofdChangelingContainer ccc)
		{
			var optionButton = GetNode<OptionButton>(NodePaths.ExistingEntryName);
			optionButton.Clear();
			optionButton.AddItem(string.Empty);
			ccc.Courts.ForEach(c => optionButton.AddItem(c.Name));
		}
	}
}
