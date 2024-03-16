using System;
using Ocsm.Dnd.Fifth;
using Ocsm.Dnd.Fifth.Meta;

namespace Ocsm.Nodes.Dnd.Fifth.Meta;

public partial class BackgroundEntry : FeaturefulMetadataEntry
{
	protected override void entrySelected(long index)
	{
		var optionsButton = GetNode<BackgroundOptionsButton>(Nodes.Meta.BasicMetadataEntry.NodePaths.ExistingEntryName);
		var name = optionsButton.GetItemText((int)index);
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			if(dfc.Backgrounds.Find(b => b.Name.Equals(name)) is Background background)
			{
				LoadEntry(background);
				optionsButton.Deselect();
			}
		}
	}
	
	public override void RefreshMetadata()
	{
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			var optionButton = GetNode<BackgroundOptionsButton>(Nodes.Meta.BasicMetadataEntry.NodePaths.ExistingEntryName);
			optionButton.Clear();
			optionButton.AddItem(String.Empty);
			dfc.Backgrounds.ForEach(b => optionButton.AddItem(b.Name));
		}
	}
}
