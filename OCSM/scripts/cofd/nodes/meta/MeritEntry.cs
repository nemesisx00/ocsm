using Godot;
using Ocsm.Cofd.Meta;
using Ocsm.Nodes;

namespace Ocsm.Cofd.Nodes;

public partial class MeritEntry : BasicMetadataEntry, ICanDelete
{
	private sealed new class NodePaths
	{
		public static readonly NodePath DotsName = new("%Dots");
	}
	
	[Signal]
	public delegate void MeritSaveClickedEventHandler(string name, string description, int value);
	
	public void LoadMerit(Merit merit)
	{
		base.LoadEntry(merit);
		
		GetNode<TrackSimple>(NodePaths.DotsName).Value = merit.Value;
	}
	
	public new void DoDelete()
	{
		var name = GetNode<LineEdit>(BasicMetadataEntry.NodePaths.NameInput).Text;
		if(!string.IsNullOrEmpty(name))
		{
			_ = EmitSignal(BasicMetadataEntry.SignalName.DeleteConfirmed, name);
			clearInputs();
		}
		//TODO: Display error message if name is empty
	}
	
	public override void RefreshMetadata()
	{
		if(metadataManager.Container is CofdCoreContainer ccc)
		{
			var optionButton = GetNode<OptionButton>(BasicMetadataEntry.NodePaths.ExistingEntryName);
			optionButton.Clear();
			optionButton.AddItem(string.Empty);
			ccc.Merits.ForEach(m => optionButton.AddItem(m.Name));
		}
	}
	
	protected override void clearInputs()
	{
		base.clearInputs();
		
		GetNode<TrackSimple>(NodePaths.DotsName).Value = 0;
	}
	
	protected override void doSave()
	{
		var name = GetNode<LineEdit>(BasicMetadataEntry.NodePaths.NameInput).Text;
		var description = GetNode<TextEdit>(BasicMetadataEntry.NodePaths.DescriptionInput).Text;
		var value = GetNode<TrackSimple>(NodePaths.DotsName).Value;
		
		_ = EmitSignal(SignalName.MeritSaveClicked, name, description, value);
		clearInputs();
	}
	
	protected override void entrySelected(long index)
	{
		var optionsButton = GetNode<OptionButton>(BasicMetadataEntry.NodePaths.ExistingEntryName);
		var name = optionsButton.GetItemText((int)index);
		
		if(metadataManager.Container is CofdCoreContainer ccc)
		{
			if(ccc.Merits.Find(m => m.Name.Equals(name)) is Merit merit)
			{
				LoadMerit(merit);
				optionsButton.Deselect();
			}
		}
	}
}
