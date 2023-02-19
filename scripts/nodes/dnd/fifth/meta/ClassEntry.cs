using Godot;
using OCSM.DnD.Fifth;
using OCSM.DnD.Fifth.Meta;

namespace OCSM.Nodes.DnD.Fifth.Meta
{
	public partial class ClassEntry : FeaturefulMetadataEntry
	{
		protected override void entrySelected(int index)
		{
			var optionsButton = GetNode<ClassOptionsButton>(NodePathBuilder.SceneUnique(ExistingEntryName));
			var name = optionsButton.GetItemText(index);
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
				var optionButton = GetNode<ClassOptionsButton>(NodePathBuilder.SceneUnique(ExistingEntryName));
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
