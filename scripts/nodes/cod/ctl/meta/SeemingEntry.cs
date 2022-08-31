using Godot;
using OCSM;
using OCSM.CoD.CtL;
using OCSM.CoD.CtL.Meta;

public class SeemingEntry : BasicMetadataEntry
{
	protected override void entrySelected(int index)
	{
		var optionsButton = GetNode<OptionButton>(NodePathBuilder.SceneUnique(ExistingEntryName));
		var name = optionsButton.GetItemText(index);
		if(metadataManager.Container is CoDChangelingContainer ccc)
		{
			if(ccc.Seemings.Find(c => c.Name.Equals(name)) is Seeming seeming)
			{
				loadEntry(seeming);
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
			foreach(var c in ccc.Seemings)
			{
				optionButton.AddItem(c.Name);
			}
		}
	}
}
