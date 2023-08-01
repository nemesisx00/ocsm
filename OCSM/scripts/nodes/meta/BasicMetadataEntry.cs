using Godot;
using System;
using Ocsm.Meta;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Nodes.Meta;

public partial class BasicMetadataEntry : Container, ICanDelete
{
	protected class NodePath
	{
		public const string ClearButton = "%Clear";
		public const string DescriptionInput = "%Description";
		public const string DeleteButton = "%Delete";
		public const string ExistingEntryName = "%ExistingEntry";
		public const string ExistingLabelName = "%ExistingLabel";
		public const string NameInput = "%Name";
		public const string SaveButton = "%Save";
	}
	
	protected const string ExistingLabelFormat = "Existing {0}";
	
	[Signal]
	public delegate void SaveClickedEventHandler(string name, string description);
	[Signal]
	public delegate void DeleteConfirmedEventHandler(string name);
	
	[Export]
	public string MetadataTypeLabel { get; set; } = String.Empty;
	[Export]
	public Script OptionsButtonScript { get; set; } = null;
	
	protected MetadataManager metadataManager;
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
		metadataManager.MetadataLoaded += refreshMetadata;
		metadataManager.MetadataSaved += refreshMetadata;
		
		GetNode<Button>(NodePath.ClearButton).Pressed += clearInputs;
		GetNode<Button>(NodePath.SaveButton).Pressed += doSave;
		GetNode<Button>(NodePath.DeleteButton).Pressed += handleDelete;
		
		GetNode<Label>(NodePath.ExistingLabelName).Text = String.Format(ExistingLabelFormat, MetadataTypeLabel);
		
		var optionsButton = GetNode<OptionButton>(NodePath.ExistingEntryName);
		optionsButton.ItemSelected += entrySelected;
		if(OptionsButtonScript is Script)
			optionsButton.SetScript(OptionsButtonScript);
		
		refreshMetadata();
	}
	
	protected virtual void clearInputs()
	{
		GetNode<LineEdit>(NodePath.NameInput).Text = String.Empty;
		GetNode<TextEdit>(NodePath.DescriptionInput).Text = String.Empty;
	}
	
	protected virtual void doSave()
	{
		var name = GetNode<LineEdit>(NodePath.NameInput).Text;
		var description = GetNode<TextEdit>(NodePath.DescriptionInput).Text;
		
		EmitSignal(nameof(SaveClicked), name, description);
		clearInputs();
	}
	
	protected void handleDelete()
	{
		NodeUtilities.displayDeleteConfirmation(
			MetadataTypeLabel,
			GetTree().CurrentScene,
			GetViewportRect().GetCenter(),
			this,
			nameof(doDelete)
		);
	}
	
	public void doDelete()
	{
		var name = GetNode<LineEdit>(NodePath.NameInput).Text;
		if(!String.IsNullOrEmpty(name))
		{
			EmitSignal(nameof(DeleteConfirmed), name);
			clearInputs();
		}
		//TODO: Display error message if name is empty
	}
	
	public virtual void loadEntry(Metadata entry)
	{
		GetNode<LineEdit>(NodePath.NameInput).Text = entry.Name;
		GetNode<TextEdit>(NodePath.DescriptionInput).Text = entry.Description;
	}
	
	protected virtual void entrySelected(long index)
	{
		throw new NotImplementedException();
	}
	
	public virtual void refreshMetadata()
	{
		throw new NotImplementedException();
	}
}
