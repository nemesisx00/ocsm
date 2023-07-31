using System;
using Ocsm.Dnd.Fifth;
using Ocsm.Dnd.Fifth.Meta;

namespace Ocsm.Nodes.Dnd.Fifth.Meta;

public partial class RaceEntry : FeaturefulMetadataEntry
{
	protected override void entrySelected(long index)
	{
		var optionsButton = GetNode<RaceOptionsButton>(NodePath.ExistingEntryName);
		var name = optionsButton.GetItemText((int)index);
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			if(dfc.Races.Find(r => r.Name.Equals(name)) is Race race)
			{
				loadEntry(race);
				optionsButton.Deselect();
			}
		}
	}
	
	public override void refreshMetadata()
	{
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			var optionButton = GetNode<RaceOptionsButton>(NodePath.ExistingEntryName);
			optionButton.Clear();
			optionButton.AddItem(String.Empty);
			dfc.Races.ForEach(r => optionButton.AddItem(r.Name));
		}
	}
}
