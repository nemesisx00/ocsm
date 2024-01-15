using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Nodes;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class RaceEntry : FeaturefulMetadataEntry
{
	
	public override void RefreshMetadata()
	{
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			var optionButton = GetNode<RaceOptionsButton>(BasicMetadataEntry.NodePaths.ExistingEntryName);
			optionButton.Clear();
			optionButton.AddItem(string.Empty);
			dfc.Races.ForEach(r => optionButton.AddItem(r.Name));
		}
	}
	
	protected override void entrySelected(long index)
	{
		var optionsButton = GetNode<RaceOptionsButton>(BasicMetadataEntry.NodePaths.ExistingEntryName);
		var name = optionsButton.GetItemText((int)index);
		
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			if(dfc.Races.Find(r => r.Name.Equals(name)) is Race race)
			{
				LoadEntry(race);
				optionsButton.Deselect();
			}
		}
	}
}
