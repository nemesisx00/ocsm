using Godot;
using System;
using OCSM.Meta;
using OCSM.Nodes.Autoload;

namespace OCSM.Nodes.Meta
{
	public partial class BasicMetadataEntry : Container, ICanDelete
	{
		protected const string ClearButton = "Clear";
		protected const string DescriptionInput = "Description";
		protected const string DeleteButton = "Delete";
		protected const string ExistingEntryName = "ExistingEntry";
		protected const string ExistingLabelFormat = "Existing {0}";
		protected const string ExistingLabelName = "ExistingLabel";
		protected const string NameInput = "Name";
		protected const string SaveButton = "Save";
		
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
			
			GetNode<Button>(NodePathBuilder.SceneUnique(ClearButton)).Pressed += clearInputs;
			GetNode<Button>(NodePathBuilder.SceneUnique(SaveButton)).Pressed += doSave;
			GetNode<Button>(NodePathBuilder.SceneUnique(DeleteButton)).Pressed += handleDelete;
			
			GetNode<Label>(NodePathBuilder.SceneUnique(ExistingLabelName)).Text = String.Format(ExistingLabelFormat, MetadataTypeLabel);
			
			var optionsButton = GetNode<OptionButton>(NodePathBuilder.SceneUnique(ExistingEntryName));
			optionsButton.ItemSelected += entrySelected;
			if(OptionsButtonScript is Script)
				optionsButton.SetScript(OptionsButtonScript);
			
			refreshMetadata();
		}
		
		protected virtual void clearInputs()
		{
			GetNode<LineEdit>(NodePathBuilder.SceneUnique(NameInput)).Text = String.Empty;
			GetNode<AutosizeTextEdit>(NodePathBuilder.SceneUnique(DescriptionInput)).Text = String.Empty;
			NodeUtilities.autoSizeChildren(this, Constants.TextInputMinHeight);
		}
		
		protected virtual void doSave()
		{
			var name = GetNode<LineEdit>(NodePathBuilder.SceneUnique(NameInput)).Text;
			var description = GetNode<TextEdit>(NodePathBuilder.SceneUnique(DescriptionInput)).Text;
			
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
			var name = GetNode<LineEdit>(NodePathBuilder.SceneUnique(NameInput)).Text;
			if(!String.IsNullOrEmpty(name))
			{
				EmitSignal(nameof(DeleteConfirmed), name);
				clearInputs();
			}
			//TODO: Display error message if name is empty
		}
		
		public virtual void loadEntry(Metadata entry)
		{
			GetNode<LineEdit>(NodePathBuilder.SceneUnique(NameInput)).Text = entry.Name;
			GetNode<TextEdit>(NodePathBuilder.SceneUnique(DescriptionInput)).Text = entry.Description;
			NodeUtilities.autoSizeChildren(this, Constants.TextInputMinHeight);
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
}
