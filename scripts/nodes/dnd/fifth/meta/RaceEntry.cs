using Godot;
using OCSM.DnD.Fifth;
using OCSM.DnD.Fifth.Meta;

namespace OCSM.Nodes.DnD.Fifth.Meta
{
	public partial class RaceEntry : FeaturefulMetadataEntry
	{
		protected override void entrySelected(long index)
		{
			var optionsButton = GetNode<RaceOptionsButton>(NodePathBuilder.SceneUnique(ExistingEntryName));
			var name = optionsButton.GetItemText((int)index);
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				if(dfc.Races.Find(r => r.Name.Equals(name)) is Race race)
				{
					loadEntry(race);
					optionsButton.Selected = 0;
				}
			}
		}
		
		public override void refreshMetadata()
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				var optionButton = GetNode<RaceOptionsButton>(NodePathBuilder.SceneUnique(ExistingEntryName));
				optionButton.Clear();
				optionButton.AddItem("");
				foreach(var r in dfc.Races)
				{
					optionButton.AddItem(r.Name);
				}
			}
		}
	}
}
