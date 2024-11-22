using Godot;
using Ocsm.Cofd.Ctl.Nodes.Meta;
using Ocsm.Dnd.Fifth.Nodes.Meta;
using Ocsm.Nodes.Autoload;
using Ocsm.Nodes.Meta;
using Ocsm.Wod.VtmV5.Nodes.Meta;

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
		}
	}
	
	private void showAddEditMetadata()
	{
		//TODO: Update to not require explicit use of specific namespaced types
		switch(metadataManager.CurrentGameSystem.Name)
		{
			case Cofd.Ctl.GameSystemFactory.Name:
				generatePopup<CofdChangelingAddEditMetadata>(Cofd.ResourcePaths.Changeling.Meta.AddEditMetadata);
				break;
			
			case Dnd.Fifth.GameSystemFactory.Name:
				generatePopup<DndFifthAddEditMetadata>(Dnd.ResourcePaths.Fifth.Meta.AddEditMetadata);
				break;
			
			case Wod.VtmV5.GameSystemFactory.Name:
				generatePopup<WodVtmV5AddEditMetadata>(Wod.ResourcePaths.VtmV5.Meta.AddEditMetadata);
				break;
		}
	}
	
	private void generatePopup<T>(string path)
		where T: Container, IAddEditMetadata
	{
		var resource = GD.Load<PackedScene>(path);
		if(resource.CanInstantiate())
		{
			var instance = resource.Instantiate<T>();
			GetTree().CurrentScene.AddChild(instance);
		}
	}
}
