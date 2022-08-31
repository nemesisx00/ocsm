using Godot;
using OCSM;
using OCSM.CoD.CtL;
using OCSM.CoD.CtL.Meta;

public class ContractTypeEntry : BasicMetadataEntry
{
	protected override void entrySelected(int index)
	{
		var optionsButton = GetNode<OptionButton>(NodePathBuilder.SceneUnique(ExistingEntryName));
		var name = optionsButton.GetItemText(index);
		if(metadataManager.Container is CoDChangelingContainer ccc)
		{
			if(ccc.ContractTypes.Find(ct => ct.Name.Equals(name)) is ContractType contractType)
			{
				loadEntry(contractType);
				optionsButton.Selected = 0;
			}
		}
	}
	
	public override void refreshMetadata()
	{
		if(metadataManager.Container is CoDChangelingContainer ccc)
		{
			var optionButton = GetNode<OptionButton>(NodePathBuilder.SceneUnique(ExistingEntryName));
			optionButton.Clear();
			optionButton.AddItem("");
			foreach(var ct in ccc.ContractTypes)
			{
				optionButton.AddItem(ct.Name);
			}
		}
	}
}
