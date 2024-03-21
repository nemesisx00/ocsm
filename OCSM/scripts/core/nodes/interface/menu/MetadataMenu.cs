using Godot;
using Ocsm.Nodes.Autoload;
using Ocsm.Nodes.Cofd.Ctl.Meta;
using Ocsm.Nodes.Dnd.Fifth.Meta;
using Ocsm.Nodes.Meta;

namespace Ocsm.Nodes;

public partial class MetadataMenu : MenuButton
{
	private static class ItemNames
	{
		public static readonly StringName ManageMetadata = new("Manage Metadata");
	}
	
	public enum MenuItem
	{
		ManageMetadata,
	}
	
	private MetadataManager metadataManager;
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
		
		var popup = GetPopup();
		popup.AddItem(ItemNames.ManageMetadata, (int)MenuItem.ManageMetadata);
		popup.IdPressed += handleMenuItem;
	}
	
	private void handleMenuItem(long id)
	{
		switch((MenuItem)id)
		{
			case MenuItem.ManageMetadata:
				showAddEditMetadata();
				break;
		}
	}
	
	private void showAddEditMetadata()
	{
		switch(metadataManager.CurrentGameSystem)
		{
			case GameSystem.CofdChangeling:
				generatePopup<CofdChangelingAddEditMetadata>(GD.Load<PackedScene>(Constants.Scene.Cofd.Changeling.Meta.AddEditMetadata));
				break;
			
			case GameSystem.Dnd5e:
				generatePopup<DndFifthAddEditMetadata>(GD.Load<PackedScene>(Constants.Scene.Dnd.Fifth.Meta.AddEditMetadata));
				break;
		}
	}
	
	private T generatePopup<T>(PackedScene resource)
		where T: Window, IAddEditMetadata
	{
		var instance = resource.Instantiate<T>();
		GetTree().CurrentScene.AddChild(instance);
		instance.PopupCentered();
		
		return instance;
	}
}
