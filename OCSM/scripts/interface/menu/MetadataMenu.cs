using Godot;
using Ocsm.Nodes.Autoload;
using Ocsm.Nodes.Cofd.Ctl.Meta;
using Ocsm.Nodes.Dnd.Fifth.Meta;
using Ocsm.Nodes.Meta;

namespace Ocsm.Nodes;

public partial class MetadataMenu : MenuButton
{
	private sealed class ItemNames
	{
		public const string ManageMetadata = "Manage Metadata";
	}
	
	public enum MenuItem : long { ManageMetadata }
	
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
			default:
				break;
		}
	}
	
	private void showAddEditMetadata()
	{
		switch(metadataManager.CurrentGameSystem)
		{
			case Constants.GameSystem.Cofd.Changeling:
				generatePopup<CodChangelingAddEditMetadata>(GD.Load<PackedScene>(Constants.Scene.Cofd.Changeling.Meta.AddEditMetadata));
				break;
			case Constants.GameSystem.Dnd.Fifth:
				generatePopup<DndFifthAddEditMetadata>(GD.Load<PackedScene>(Constants.Scene.Dnd.Fifth.Meta.AddEditMetadata));
				break;
			default:
				break;
		}
	}
	
	private T generatePopup<T>(PackedScene resource)
		where T: BaseAddEditMetadata
	{
		var instance = resource.Instantiate<T>();
		GetTree().CurrentScene.AddChild(instance);
		instance.PopupCentered();
		instance.MetadataChanged += metadataManager.saveGameSystemMetadata;
		
		return instance;
	}
}
