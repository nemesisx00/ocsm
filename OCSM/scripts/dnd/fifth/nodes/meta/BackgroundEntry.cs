using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Nodes;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class BackgroundEntry : FeaturefulMetadataEntry
{
	public override void RefreshMetadata()
	{
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			var optionButton = GetNode<BackgroundOptionsButton>(BasicMetadataEntry.NodePaths.ExistingEntryName);
			optionButton.Clear();
			optionButton.AddItem(string.Empty);
			dfc.Backgrounds.ForEach(b => optionButton.AddItem(b.Name));
		}
	}
	
	protected override void entrySelected(long index)
	{
		var optionsButton = GetNode<BackgroundOptionsButton>(BasicMetadataEntry.NodePaths.ExistingEntryName);
		var name = optionsButton.GetItemText((int)index);
		
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			if(dfc.Backgrounds.Find(b => b.Name.Equals(name)) is Background background)
			{
				base.LoadEntry((Ocsm.Meta.Metadata)background);
				optionsButton.Deselect();
			}
		}
	}
}
