using Godot;
using Ocsm.Cofd.Ctl.Meta;
using Ocsm.Nodes;

namespace Ocsm.Cofd.Ctl.Nodes;

public partial class KithEntry : BasicMetadataEntry
{
	protected override void entrySelected(long index)
	{
		var optionsButton = GetNode<OptionButton>(NodePaths.ExistingEntryName);
		var name = optionsButton.GetItemText((int)index);
		
		if(metadataManager.Container is CofdChangelingContainer ccc)
		{
			if(ccc.Kiths.Find(k => k.Name.Equals(name)) is Kith kith)
			{
				LoadEntry(kith);
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
			ccc.Kiths.ForEach(k => optionButton.AddItem(k.Name));
		}
	}
}
