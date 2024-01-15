using Godot;
using Ocsm.Nodes.Autoload;
using Ocsm.Cofd.Ctl.Nodes;
using Ocsm.Dnd.Fifth.Nodes;

namespace Ocsm.Nodes;

public partial class MetadataMenu : MenuButton
{
	private sealed class ItemNames
	{
		public const string ManageMetadata = "Manage Metadata";
	}
	
	public enum MenuItem : long
	{
		ManageMetadata,
	}
	
	private MetadataManager metadataManager;
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(MetadataManager.NodePath);
		
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
			
			default:
				break;
		}
	}
	
	private void showAddEditMetadata()
	{
		switch(metadataManager.CurrentGameSystem)
		{
			case GameSystems.CofdChangeling:
				generatePopup<CofdChangelingAddEditMetadata>(GD.Load<PackedScene>(ScenePaths.Cofd.Changeling.Meta.AddEditMetadata));
				break;
			
			case GameSystems.Dnd5e:
				generatePopup<DndFifthAddEditMetadata>(GD.Load<PackedScene>(ScenePaths.Dnd.Fifth.Meta.AddEditMetadata));
				break;
		}
	}
	
	private T generatePopup<T>(PackedScene resource)
		where T: BaseAddEditMetadata
	{
		var instance = resource.Instantiate<T>();
		GetTree().CurrentScene.AddChild(instance);
		instance.PopupCentered();
		instance.MetadataChanged += metadataManager.SaveGameSystemMetadata;
		
		return instance;
	}
}
