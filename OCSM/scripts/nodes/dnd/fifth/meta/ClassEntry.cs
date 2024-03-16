using System;
using Ocsm.Dnd.Fifth;
using Ocsm.Dnd.Fifth.Meta;

namespace Ocsm.Nodes.Dnd.Fifth.Meta;

public partial class ClassEntry : FeaturefulMetadataEntry
{
	protected override void entrySelected(long index)
	{
		var optionsButton = GetNode<ClassOptionsButton>(Nodes.Meta.BasicMetadataEntry.NodePaths.ExistingEntryName);
		var name = optionsButton.GetItemText((int)index);
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			if(dfc.Classes.Find(c => c.Name.Equals(name)) is Class clazz)
			{
				LoadEntry(clazz);
				optionsButton.Deselect();
			}
		}
	}
	
	public override void RefreshMetadata()
	{
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			var optionButton = GetNode<ClassOptionsButton>(Nodes.Meta.BasicMetadataEntry.NodePaths.ExistingEntryName);
			optionButton.Clear();
			optionButton.AddItem(String.Empty);
			dfc.Classes.ForEach(c => optionButton.AddItem(c.Name));
		}
	}
}
