using Godot;
using System;
using OCSM.CoD;
using OCSM.CoD.Meta;
using OCSM.Nodes.Meta;

namespace OCSM.Nodes.CoD.Meta
{
	public class MeritEntry : BasicMetadataEntry
	{
		[Signal]
		public new delegate void SaveClicked(string name, string description, int value);
		
		private const string DotsName = "Dots";
		
		public void loadMerit(Merit merit)
		{
			base.loadEntry(merit);
			
			GetNode<TrackSimple>(NodePathBuilder.SceneUnique(DotsName)).updateValue(merit.Value);
		}
		
		protected override void clearInputs()
		{
			base.clearInputs();
			
			GetNode<TrackSimple>(NodePathBuilder.SceneUnique(DotsName)).updateValue(0);
		}
		
		protected override void doSave()
		{
			var name = GetNode<LineEdit>(NodePathBuilder.SceneUnique(NameInput)).Text;
			var description = GetNode<TextEdit>(NodePathBuilder.SceneUnique(DescriptionInput)).Text;
			var value = GetNode<TrackSimple>(NodePathBuilder.SceneUnique(DotsName)).Value;
			
			EmitSignal(nameof(SaveClicked), name, description, value);
			clearInputs();
		}
		
		protected override void doDelete()
		{
			var name = GetNode<LineEdit>(NodePathBuilder.SceneUnique(NameInput)).Text;
			if(!String.IsNullOrEmpty(name))
			{
				EmitSignal(nameof(DeleteConfirmed), name);
				clearInputs();
			}
			//TODO: Display error message if name is empty
		}
		
		protected override void entrySelected(int index)
		{
			var optionsButton = GetNode<OptionButton>(NodePathBuilder.SceneUnique(ExistingEntryName));
			var name = optionsButton.GetItemText(index);
			if(metadataManager.Container is CoDCoreContainer ccc)
			{
				if(ccc.Merits.Find(m => m.Name.Equals(name)) is Merit merit)
				{
					loadMerit(merit);
					optionsButton.Selected = 0;
				}
			}
		}
		
		public override void refreshMetadata()
		{
			if(metadataManager.Container is CoDCoreContainer ccc)
			{
				var optionButton = GetNode<OptionButton>(NodePathBuilder.SceneUnique(ExistingEntryName));
				optionButton.Clear();
				optionButton.AddItem("");
				foreach(var m in ccc.Merits)
				{
					optionButton.AddItem(m.Name);
				}
			}
		}
	}
}
