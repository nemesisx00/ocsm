using Godot;
using System;
using Ocsm.Meta;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Nodes.Meta;

public partial class BasicMetadataEntry : Container, ICanDelete
{
	protected static class NodePaths
	{
		public static readonly NodePath ClearButton = new("%Clear");
		public static readonly NodePath DescriptionInput = new("%Description");
		public static readonly NodePath DeleteButton = new("%Delete");
		public static readonly NodePath ExistingEntryName = new("%ExistingEntry");
		public static readonly NodePath ExistingLabelName = new("%ExistingLabel");
		public static readonly NodePath NameInput = new("%Name");
		public static readonly NodePath SaveButton = new("%Save");
	}
	
	[Signal]
	public delegate void SaveClickedEventHandler(string name, string description);
	[Signal]
	public delegate void DeleteConfirmedEventHandler(string name);
	
	[Export]
	public string MetadataTypeLabel { get; set; } = string.Empty;
	[Export]
	public Script OptionsButtonScript { get; set; } = null;
	
	protected MetadataManager metadataManager;
	
	protected LineEdit nameInput;
	protected TextEdit descriptionInput;
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(MetadataManager.NodePath);
		metadataManager.MetadataLoaded += RefreshMetadata;
		metadataManager.MetadataSaved += RefreshMetadata;
		
		GetNode<Button>(NodePaths.ClearButton).Pressed += clearInputs;
		GetNode<Button>(NodePaths.SaveButton).Pressed += doSave;
		GetNode<Button>(NodePaths.DeleteButton).Pressed += handleDelete;
		
		GetNode<Label>(NodePaths.ExistingLabelName).Text = $"Existing {MetadataTypeLabel}";
		
		var optionsButton = GetNode<OptionButton>(NodePaths.ExistingEntryName);
		optionsButton.ItemSelected += entrySelected;
		
		if(OptionsButtonScript is not null)
			optionsButton.SetScript(OptionsButtonScript);
		
		nameInput = GetNode<LineEdit>(NodePaths.NameInput);
		descriptionInput = GetNode<TextEdit>(NodePaths.DescriptionInput);
		
		RefreshMetadata();
	}
	
	public void DoDelete()
	{
		var name = GetNode<LineEdit>(NodePaths.NameInput).Text;
		if(!string.IsNullOrEmpty(name))
		{
			EmitSignal(nameof(DeleteConfirmed), name);
			clearInputs();
		}
		//TODO: Display error message if name is empty
	}
	
	public virtual void LoadEntry(Metadata entry)
	{
		nameInput.Text = entry.Name;
		descriptionInput.Text = entry.Description;
	}
	
	public virtual void RefreshMetadata()
	{
		throw new NotImplementedException();
	}
	
	protected virtual void clearInputs()
	{
		nameInput.Text = string.Empty;
		descriptionInput.Text = string.Empty;
	}
	
	protected virtual void doSave()
	{
		var name = nameInput.Text;
		var description = descriptionInput.Text;
		
		EmitSignal(SignalName.SaveClicked, name, description);
		clearInputs();
	}
	
	protected void handleDelete() => NodeUtilities.DisplayDeleteConfirmation(
		MetadataTypeLabel,
		this,
		this
	);
	
	protected virtual void entrySelected(long index)
	{
		throw new NotImplementedException();
	}
}
