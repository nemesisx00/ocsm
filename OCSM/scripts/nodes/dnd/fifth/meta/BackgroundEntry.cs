using System;
using Ocsm.Dnd.Fifth;
using Ocsm.Dnd.Fifth.Meta;

namespace Ocsm.Nodes.Dnd.Fifth.Meta
{
	public partial class BackgroundEntry : FeaturefulMetadataEntry
	{
		protected override void entrySelected(long index)
		{
			var optionsButton = GetNode<BackgroundOptionsButton>(NodePath.ExistingEntryName);
			var name = optionsButton.GetItemText((int)index);
			if(metadataManager.Container is DndFifthContainer dfc)
			{
				if(dfc.Backgrounds.Find(b => b.Name.Equals(name)) is Background background)
				{
					loadEntry(background);
					optionsButton.Deselect();
				}
			}
		}
		
		public override void refreshMetadata()
		{
			if(metadataManager.Container is DndFifthContainer dfc)
			{
				var optionButton = GetNode<BackgroundOptionsButton>(NodePath.ExistingEntryName);
				optionButton.Clear();
				optionButton.AddItem(String.Empty);
				dfc.Backgrounds.ForEach(b => optionButton.AddItem(b.Name));
			}
		}
	}
}
