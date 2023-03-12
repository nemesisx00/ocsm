using Godot;
using System;
using OCSM.CoD;
using OCSM.CoD.Meta;
using OCSM.Nodes.Meta;

namespace OCSM.Nodes.CoD.Meta
{
	public partial class MeritEntry : BasicMetadataEntry, ICanDelete
	{
		private sealed new class NodePath : BasicMetadataEntry.NodePath
		{
			public const string DotsName = "%Dots";
		}
		
		[Signal]
		public new delegate void SaveClickedEventHandler(string name, string description, int value);
		
		public void loadMerit(Merit merit)
		{
			base.loadEntry(merit);
			
			GetNode<TrackSimple>(NodePath.DotsName).updateValue(merit.Value);
		}
		
		protected override void clearInputs()
		{
			base.clearInputs();
			
			GetNode<TrackSimple>(NodePath.DotsName).updateValue(0);
		}
		
		protected override void doSave()
		{
			var name = GetNode<LineEdit>(NodePath.NameInput).Text;
			var description = GetNode<TextEdit>(NodePath.DescriptionInput).Text;
			var value = GetNode<TrackSimple>(NodePath.DotsName).Value;
			
			EmitSignal(nameof(SaveClicked), name, description, value);
			clearInputs();
		}
		
		public new void doDelete()
		{
			var name = GetNode<LineEdit>(NodePath.NameInput).Text;
			if(!String.IsNullOrEmpty(name))
			{
				EmitSignal(nameof(DeleteConfirmed), name);
				clearInputs();
			}
			//TODO: Display error message if name is empty
		}
		
		protected override void entrySelected(long index)
		{
			var optionsButton = GetNode<OptionButton>(NodePath.ExistingEntryName);
			var name = optionsButton.GetItemText((int)index);
			if(metadataManager.Container is CoDCoreContainer ccc)
			{
				if(ccc.Merits.Find(m => m.Name.Equals(name)) is Merit merit)
				{
					loadMerit(merit);
					optionsButton.Deselect();
				}
			}
		}
		
		public override void refreshMetadata()
		{
			if(metadataManager.Container is CoDCoreContainer ccc)
			{
				var optionButton = GetNode<OptionButton>(NodePath.ExistingEntryName);
				optionButton.Clear();
				optionButton.AddItem(String.Empty);
				ccc.Merits.ForEach(m => optionButton.AddItem(m.Name));
			}
		}
	}
}
