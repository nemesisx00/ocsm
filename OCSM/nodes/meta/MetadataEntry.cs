using Godot;
using Godot.Collections;
using System.Linq;
using Ocsm.Meta;
using Ocsm.Nodes.Autoload;
using System.Collections.Generic;

namespace Ocsm.Nodes.Meta;

public partial class MetadataEntry : Container, ICanDelete
{
	private static class NodePaths
	{
		public static readonly NodePath ClearButton = new("%Clear");
		public static readonly NodePath DescriptionInput = new("%Description");
		public static readonly NodePath DeleteButton = new("%Delete");
		public static readonly NodePath ExistingEntryName = new("%ExistingEntry");
		public static readonly NodePath ExistingLabelName = new("%ExistingLabel");
		public static readonly NodePath NameInput = new("%Name");
		public static readonly NodePath SaveButton = new("%Save");
	}
	
	private const string ExistingLabelFormat = "Existing {0}";
	
	[Signal]
	public delegate void SaveClickedEventHandler(string name, string description, Array<string> type);
	[Signal]
	public delegate void DeleteConfirmedEventHandler(string name, Array<string> type);
	
	[Export]
	public Array<string> MetadataTypes { get; set; }
	[Export]
	public string MetadataTypeLabel { get; set; } = string.Empty;
	
	private MetadataManager metadataManager;
	
	private MetadataOption optionsButton;
	private LineEdit name;
	private TextEdit description;
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(MetadataManager.NodePath);
		
		GetNode<Button>(NodePaths.ClearButton).Pressed += clearInputs;
		GetNode<Button>(NodePaths.SaveButton).Pressed += doSave;
		GetNode<Button>(NodePaths.DeleteButton).Pressed += handleDelete;
		
		GetNode<Label>(NodePaths.ExistingLabelName).Text = string.Format(ExistingLabelFormat, MetadataTypeLabel);
		
		name = GetNode<LineEdit>(NodePaths.NameInput);
		description = GetNode<TextEdit>(NodePaths.DescriptionInput);
		
		optionsButton = GetNode<MetadataOption>(NodePaths.ExistingEntryName);
		optionsButton.MetadataTypes = MetadataTypes;
		optionsButton.ItemSelected += entrySelected;
		
		RefreshMetadata();
	}
	
	public void DoDelete()
	{
		var name = GetNode<LineEdit>(NodePaths.NameInput).Text;
		if(!string.IsNullOrEmpty(name))
		{
			EmitSignal(SignalName.DeleteConfirmed, name, MetadataTypes);
			clearInputs();
		}
		//TODO: Display error message if name is empty
	}
	
	public void LoadEntry(Metadata entry)
	{
		name.Text = entry.Name;
		description.Text = entry.Description;
	}
	
	public void RefreshMetadata() => optionsButton.RefreshMetadata();
	
	private void clearInputs()
	{
		name.Text = string.Empty;
		description.Text = string.Empty;
	}
	
	private void doSave()
	{
		var name = GetNode<LineEdit>(NodePaths.NameInput).Text;
		var description = GetNode<TextEdit>(NodePaths.DescriptionInput).Text;
		
		EmitSignal(SignalName.SaveClicked, name, description, MetadataTypes);
		clearInputs();
	}
	
	private void entrySelected(long index)
	{
		var optionsButton = GetNode<OptionButton>(NodePaths.ExistingEntryName);
		var name = optionsButton.GetItemText((int)index);
		
		if(metadataManager.Container is BaseContainer container
			&& container.Metadata.Where(m => m.Types == MetadataTypes.ToList() && m.Name == name).FirstOrDefault() is Metadata metadata)
		{
			LoadEntry(metadata);
			optionsButton.Deselect();
		}
	}
	
	private void handleDelete() => NodeUtilities.DisplayDeleteConfirmation(
		MetadataTypeLabel,
		this,
		this
	);
}
