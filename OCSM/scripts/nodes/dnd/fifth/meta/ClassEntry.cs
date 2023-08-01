using System;
using Ocsm.Dnd.Fifth;
using Ocsm.Dnd.Fifth.Meta;

namespace Ocsm.Nodes.Dnd.Fifth.Meta;

public partial class ClassEntry : FeaturefulMetadataEntry
{
	protected override void entrySelected(long index)
	{
		var optionsButton = GetNode<ClassOptionsButton>(NodePath.ExistingEntryName);
		var name = optionsButton.GetItemText((int)index);
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			if(dfc.Classes.Find(c => c.Name.Equals(name)) is Class clazz)
			{
				loadEntry(clazz);
				optionsButton.Deselect();
			}
		}
	}
	
	public override void refreshMetadata()
	{
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			var optionButton = GetNode<ClassOptionsButton>(NodePath.ExistingEntryName);
			optionButton.Clear();
			optionButton.AddItem(String.Empty);
			dfc.Classes.ForEach(c => optionButton.AddItem(c.Name));
		}
	}
}
