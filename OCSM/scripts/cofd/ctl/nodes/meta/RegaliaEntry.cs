using Godot;
using Ocsm.Cofd.Ctl.Meta;
using Ocsm.Nodes;

namespace Ocsm.Cofd.Ctl.Nodes;

public partial class RegaliaEntry : BasicMetadataEntry
{
	protected override void entrySelected(long index)
	{
		var optionsButton = GetNode<OptionButton>(NodePaths.ExistingEntryName);
		var name = optionsButton.GetItemText((int)index);
		
		if(metadataManager.Container is CofdChangelingContainer ccc)
		{
			if(ccc.Regalias.Find(r => r.Name.Equals(name)) is Regalia regalia)
			{
				LoadEntry(regalia);
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
			ccc.Regalias.ForEach(r => optionButton.AddItem(r.Name));
		}
	}
}
