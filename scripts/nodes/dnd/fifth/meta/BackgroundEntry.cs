using Godot;
using OCSM.DnD.Fifth;
using OCSM.DnD.Fifth.Meta;

namespace OCSM.Nodes.DnD.Fifth.Meta
{
	public partial class BackgroundEntry : FeaturefulMetadataEntry
	{
		protected override void entrySelected(long index)
		{
			var optionsButton = GetNode<BackgroundOptionsButton>(NodePath.ExistingEntryName);
			var name = optionsButton.GetItemText((int)index);
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				if(dfc.Backgrounds.Find(b => b.Name.Equals(name)) is Background background)
				{
					loadEntry(background);
					optionsButton.Selected = 0;
				}
			}
		}
		
		public override void refreshMetadata()
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				var optionButton = GetNode<BackgroundOptionsButton>(NodePath.ExistingEntryName);
				optionButton.Clear();
				optionButton.AddItem("");
				foreach(var b in dfc.Backgrounds)
				{
					optionButton.AddItem(b.Name);
				}
			}
		}
	}
}
