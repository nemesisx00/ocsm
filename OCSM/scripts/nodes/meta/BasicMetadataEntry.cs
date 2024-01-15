using Godot;
using System;
using Ocsm.Meta;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Nodes;

public partial class BasicMetadataEntry : Container, ICanDelete
{
	protected sealed class NodePaths
	{
		public static readonly NodePath ClearButton = new("%Clear");
		public static readonly NodePath DescriptionInput = new("%Description");
		public static readonly NodePath DeleteButton = new("%Delete");
		public static readonly NodePath ExistingEntryName = new("%ExistingEntry");
		public static readonly NodePath ExistingLabelName = new("%ExistingLabel");
		public static readonly NodePath NameInput = new("%Name");
		public static readonly NodePath SaveButton = new("%Save");
	}
	
	protected const string ExistingLabelFormat = "Existing {0}";
	
	[Signal]
	public delegate void SaveClickedEventHandler(string name, string description);
	[Signal]
	public delegate void DeleteConfirmedEventHandler(string name);
	
	[Export]
	public string MetadataTypeLabel { get; set; }
	[Export]
	public Script OptionsButtonScript { get; set; }
	
	protected MetadataManager metadataManager;
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(MetadataManager.NodePath);
		metadataManager.MetadataLoaded += RefreshMetadata;
		metadataManager.MetadataSaved += RefreshMetadata;
		
		GetNode<Button>(NodePaths.ClearButton).Pressed += clearInputs;
		GetNode<Button>(NodePaths.SaveButton).Pressed += doSave;
		GetNode<Button>(NodePaths.DeleteButton).Pressed += handleDelete;
		
		GetNode<Label>(NodePaths.ExistingLabelName).Text = string.Format(ExistingLabelFormat, MetadataTypeLabel);
		
		var optionsButton = GetNode<OptionButton>(NodePaths.ExistingEntryName);
		optionsButton.ItemSelected += entrySelected;
		
		if(OptionsButtonScript is not null)
			optionsButton.SetScript(OptionsButtonScript);
		
		RefreshMetadata();
	}
	
	public void DoDelete()
	{
		var name = GetNode<LineEdit>(NodePaths.NameInput).Text;
		if(!string.IsNullOrEmpty(name))
		{
			_ = EmitSignal(SignalName.DeleteConfirmed, name);
			clearInputs();
		}
		//TODO: Display error message if name is empty
	}
	
	public virtual void LoadEntry(Metadata entry)
	{
		GetNode<LineEdit>(NodePaths.NameInput).Text = entry.Name;
		GetNode<TextEdit>(NodePaths.DescriptionInput).Text = entry.Description;
	}
	
	public virtual void RefreshMetadata()
	{
		throw new NotImplementedException();
	}
	
	protected virtual void clearInputs()
	{
		GetNode<LineEdit>(NodePaths.NameInput).Text = string.Empty;
		GetNode<TextEdit>(NodePaths.DescriptionInput).Text = string.Empty;
	}
	
	protected virtual void doSave()
	{
		var name = GetNode<LineEdit>(NodePaths.NameInput).Text;
		var description = GetNode<TextEdit>(NodePaths.DescriptionInput).Text;
		
		EmitSignal(SignalName.SaveClicked, name, description);
		clearInputs();
	}
	
	protected void handleDelete() => NodeUtilities.DisplayDeleteConfirmation(
		MetadataTypeLabel,
		GetTree().CurrentScene,
		this
	);
	
	protected virtual void entrySelected(long index)
	{
		throw new NotImplementedException();
	}
}
