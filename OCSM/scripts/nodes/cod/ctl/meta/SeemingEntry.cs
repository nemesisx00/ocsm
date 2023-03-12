using Godot;
using System;
using OCSM.CoD.CtL;
using OCSM.CoD.CtL.Meta;
using OCSM.Nodes.Meta;

namespace OCSM.Nodes.CoD.CtL.Meta
{
	public partial class SeemingEntry : BasicMetadataEntry
	{
		protected override void entrySelected(long index)
		{
			var optionsButton = GetNode<OptionButton>(NodePath.ExistingEntryName);
			var name = optionsButton.GetItemText((int)index);
			if(metadataManager.Container is CoDChangelingContainer ccc)
			{
				if(ccc.Seemings.Find(s => s.Name.Equals(name)) is Seeming seeming)
				{
					loadEntry(seeming);
					optionsButton.Deselect();
				}
			}
		}
		
		public override void refreshMetadata()
		{
			if(metadataManager.Container is CoDChangelingContainer ccc)
			{
				var optionButton = GetNode<OptionButton>(NodePath.ExistingEntryName);
				optionButton.Clear();
				optionButton.AddItem(String.Empty);
				ccc.Seemings.ForEach(s => optionButton.AddItem(s.Name));
			}
		}
	}
}
