using Godot;
using OCSM.CoD.CtL;
using OCSM.CoD.CtL.Meta;
using OCSM.Nodes.Meta;

namespace OCSM.Nodes.CoD.CtL.Meta
{
	public partial class CourtEntry : BasicMetadataEntry
	{
		protected override void entrySelected(long index)
		{
			var optionsButton = GetNode<OptionButton>(NodePath.ExistingEntryName);
			var name = optionsButton.GetItemText((int)index);
			if(metadataManager.Container is CoDChangelingContainer ccc)
			{
				if(ccc.Courts.Find(c => c.Name.Equals(name)) is Court court)
				{
					loadEntry(court);
					optionsButton.Selected = 0;
				}
			}
		}
		
		public override void refreshMetadata()
		{
			if(metadataManager.Container is CoDChangelingContainer ccc)
			{
				var optionButton = GetNode<OptionButton>(NodePath.ExistingEntryName);
				optionButton.Clear();
				optionButton.AddItem("");
				foreach(var c in ccc.Courts)
				{
					optionButton.AddItem(c.Name);
				}
			}
		}
	}
}
