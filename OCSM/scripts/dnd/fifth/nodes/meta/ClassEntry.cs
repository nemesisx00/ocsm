using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Meta;
using Ocsm.Nodes;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class ClassEntry : FeaturefulMetadataEntry
{
	public override void RefreshMetadata()
	{
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			var optionButton = GetNode<ClassOptionsButton>(BasicMetadataEntry.NodePaths.ExistingEntryName);
			optionButton.Clear();
			optionButton.AddItem(string.Empty);
			dfc.Classes.ForEach(c => optionButton.AddItem(c.Name));
		}
	}
	
	protected override void entrySelected(long index)
	{
		var optionsButton = GetNode<ClassOptionsButton>(BasicMetadataEntry.NodePaths.ExistingEntryName);
		var name = optionsButton.GetItemText((int)index);
		
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			if(dfc.Classes.Find(c => c.Name.Equals(name)) is Class clazz)
			{
				base.LoadEntry((Metadata)clazz);
				optionsButton.Deselect();
			}
		}
	}
}
