using Godot;
using System;
using Ocsm.Cofd.Meta;
using Ocsm.Nodes;
using Ocsm.Nodes.Meta;

namespace Ocsm.Cofd.Nodes.Meta;

public partial class MeritEntry : BasicMetadataEntry, ICanDelete
{
	private new static class NodePaths
	{
		public static readonly NodePath DotsName = new("%Dots");
	}
	
	[Signal]
	public new delegate void SaveClickedEventHandler(string name, string description, int value);
	
	public new void DoDelete()
	{
		var name = GetNode<LineEdit>(BasicMetadataEntry.NodePaths.NameInput).Text;
		if(!string.IsNullOrEmpty(name))
		{
			EmitSignal(SignalName.DeleteConfirmed, name);
			clearInputs();
		}
		//TODO: Display error message if name is empty
	}
	
	public void LoadMerit(Merit merit)
	{
		base.LoadEntry(merit);
		
		GetNode<TrackSimple>(NodePaths.DotsName).UpdateValue(merit.Value);
	}
	
	protected override void clearInputs()
	{
		base.clearInputs();
		
		GetNode<TrackSimple>(NodePaths.DotsName).UpdateValue(0);
	}
	
	protected override void doSave()
	{
		var name = GetNode<LineEdit>(BasicMetadataEntry.NodePaths.NameInput).Text;
		var description = GetNode<TextEdit>(BasicMetadataEntry.NodePaths.DescriptionInput).Text;
		var value = GetNode<TrackSimple>(NodePaths.DotsName).Value;
		
		EmitSignal(SignalName.SaveClicked, name, description, value);
		clearInputs();
	}
	
	protected override void entrySelected(long index)
	{
		var optionsButton = GetNode<OptionButton>(BasicMetadataEntry.NodePaths.ExistingEntryName);
		var name = optionsButton.GetItemText((int)index);
		if(metadataManager.Container is CofdCoreContainer container
			&& container.Merits.Find(m => m.Name.Equals(name)) is Merit merit)
		{
			LoadMerit(merit);
			optionsButton.Deselect();
		}
	}
	
	public override void RefreshMetadata()
	{
		if(metadataManager.Container is CofdCoreContainer container)
		{
			var optionButton = GetNode<OptionButton>(BasicMetadataEntry.NodePaths.ExistingEntryName);
			optionButton.Clear();
			optionButton.AddItem(String.Empty);
			container.Merits.ForEach(m => optionButton.AddItem(m.Name));
		}
	}
}
