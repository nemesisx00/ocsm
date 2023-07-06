using Godot;
using System;
using Ocsm.Cofd.Ctl;
using Ocsm.Cofd.Ctl.Meta;
using Ocsm.Nodes.Meta;

namespace Ocsm.Nodes.Cofd.Ctl.Meta
{
	public partial class ContractTypeEntry : BasicMetadataEntry
	{
		protected override void entrySelected(long index)
		{
			var optionsButton = GetNode<OptionButton>(NodePath.ExistingEntryName);
			var name = optionsButton.GetItemText((int)index);
			if(metadataManager.Container is CofdChangelingContainer ccc)
			{
				if(ccc.ContractTypes.Find(ct => ct.Name.Equals(name)) is ContractType contractType)
				{
					loadEntry(contractType);
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
				ccc.ContractTypes.ForEach(ct => optionButton.AddItem(ct.Name));
			}
		}
	}
}
