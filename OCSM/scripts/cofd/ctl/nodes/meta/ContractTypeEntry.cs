using Godot;
using Ocsm.Cofd.Ctl.Meta;
using Ocsm.Nodes;

namespace Ocsm.Cofd.Ctl.Nodes;

public partial class ContractTypeEntry : BasicMetadataEntry
{
	protected override void entrySelected(long index)
	{
		var optionsButton = GetNode<OptionButton>(NodePaths.ExistingEntryName);
		var name = optionsButton.GetItemText((int)index);
		
		if(metadataManager.Container is CofdChangelingContainer ccc)
		{
			if(ccc.ContractTypes.Find(ct => ct.Name.Equals(name)) is ContractType contractType)
			{
				LoadEntry(contractType);
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
			ccc.ContractTypes.ForEach(ct => optionButton.AddItem(ct.Name));
		}
	}
}
