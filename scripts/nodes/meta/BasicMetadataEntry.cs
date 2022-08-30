using Godot;
using System;
using OCSM;
using OCSM.Meta;

public class BasicMetadataEntry : Container
{
	private const string ClearButton = "Clear";
	private const string DescriptionInput = "Description";
	private const string DeleteButton = "Delete";
	private const string NameInput = "Name";
	private const string SaveButton = "Save";
	
	[Signal]
	public delegate void SaveClicked(string name, string description);
	[Signal]
	public delegate void DeleteConfirmed(string name);
	
	[Export]
	public string MetadataTypeLabel { get; set; } = String.Empty;
	
	public override void _Ready()
	{
		GetNode<Button>(NodePathBuilder.SceneUnique(ClearButton)).Connect(Constants.Signal.Pressed, this, nameof(clearInputs));
		GetNode<Button>(NodePathBuilder.SceneUnique(SaveButton)).Connect(Constants.Signal.Pressed, this, nameof(doSave));
		GetNode<Button>(NodePathBuilder.SceneUnique(DeleteButton)).Connect(Constants.Signal.Pressed, this, nameof(handleDelete));
	}
	
	public void loadMetadataEntry(Metadata entry)
	{
		GetNode<LineEdit>(NodePathBuilder.SceneUnique(NameInput)).Text = entry.Name;
		GetNode<TextEdit>(NodePathBuilder.SceneUnique(DescriptionInput)).Text = entry.Description;
	}
	
	private void clearInputs()
	{
		GetNode<LineEdit>(NodePathBuilder.SceneUnique(NameInput)).Text = String.Empty;
		GetNode<TextEdit>(NodePathBuilder.SceneUnique(DescriptionInput)).Text = String.Empty;
	}
	
	private void doSave()
	{
		var name = GetNode<LineEdit>(NodePathBuilder.SceneUnique(NameInput)).Text;
		var description = GetNode<TextEdit>(NodePathBuilder.SceneUnique(DescriptionInput)).Text;
		
		EmitSignal(nameof(SaveClicked), name, description);
		clearInputs();
	}
	
	private void handleDelete()
	{
		var resource = ResourceLoader.Load<PackedScene>(Constants.Scene.Meta.ConfirmDeleteEntry);
		var instance = resource.Instance<ConfirmDeleteEntry>();
		instance.EntryTypeName = MetadataTypeLabel;
		GetTree().CurrentScene.AddChild(instance);
		NodeUtilities.centerControl(instance, GetViewportRect().GetCenter());
		instance.Connect(Constants.Signal.Confirmed, this, nameof(doDelete));
		instance.Popup_();
	}
	
	private void doDelete()
	{
		var name = GetNode<LineEdit>(NodePathBuilder.SceneUnique(NameInput)).Text;
		if(!String.IsNullOrEmpty(name))
		{
			EmitSignal(nameof(DeleteConfirmed), name);
			clearInputs();
		}
		//TODO: Display error message if name is empty
	}
}
