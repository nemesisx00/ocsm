using Godot;
using Ocsm.Cofd.Ctl.Meta;
using Ocsm.Nodes;

namespace Ocsm.Cofd.Ctl.Nodes;

public partial class SeemingEntry : BasicMetadataEntry
{
	protected override void entrySelected(long index)
	{
		var optionsButton = GetNode<OptionButton>(NodePaths.ExistingEntryName);
		var name = optionsButton.GetItemText((int)index);
		
		if(metadataManager.Container is CofdChangelingContainer ccc)
		{
			if(ccc.Seemings.Find(s => s.Name.Equals(name)) is Seeming seeming)
			{
				LoadEntry(seeming);
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
			ccc.Seemings.ForEach(s => optionButton.AddItem(s.Name));
		}
	}
}
