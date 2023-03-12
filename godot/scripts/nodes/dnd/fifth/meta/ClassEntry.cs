using Godot;
using OCSM.DnD.Fifth;
using OCSM.DnD.Fifth.Meta;

namespace OCSM.Nodes.DnD.Fifth.Meta
{
	public partial class ClassEntry : FeaturefulMetadataEntry
	{
		protected override void entrySelected(long index)
		{
			var optionsButton = GetNode<ClassOptionsButton>(NodePath.ExistingEntryName);
			var name = optionsButton.GetItemText((int)index);
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				if(dfc.Classes.Find(c => c.Name.Equals(name)) is Class clazz)
				{
					loadEntry(clazz);
					optionsButton.Selected = 0;
				}
			}
		}
		
		public override void refreshMetadata()
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				var optionButton = GetNode<ClassOptionsButton>(NodePath.ExistingEntryName);
				optionButton.Clear();
				optionButton.AddItem("");
				foreach(var c in dfc.Classes)
				{
					optionButton.AddItem(c.Name);
				}
			}
		}
	}
}
