using Godot;
using System;
using Ocsm.Cofd.Ctl;
using Ocsm.Cofd.Ctl.Meta;
using Ocsm.Nodes.Meta;

namespace Ocsm.Nodes.Cofd.Ctl.Meta;

public partial class RegaliaEntry : BasicMetadataEntry
{
	protected override void entrySelected(long index)
	{
		var optionsButton = GetNode<OptionButton>(NodePath.ExistingEntryName);
		var name = optionsButton.GetItemText((int)index);
		if(metadataManager.Container is CofdChangelingContainer ccc)
		{
			if(ccc.Regalias.Find(r => r.Name.Equals(name)) is Regalia regalia)
			{
				loadEntry(regalia);
				optionsButton.Deselect();
			}
		}
	}
	
	public override void refreshMetadata()
	{
		if(metadataManager.Container is CofdChangelingContainer ccc)
		{
			var optionButton = GetNode<OptionButton>(NodePath.ExistingEntryName);
			optionButton.Clear();
			optionButton.AddItem(String.Empty);
			ccc.Regalias.ForEach(r => optionButton.AddItem(r.Name));
		}
	}
}
