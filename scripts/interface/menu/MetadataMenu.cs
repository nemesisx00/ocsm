using Godot;
using OCSM.Nodes.Autoload;
using OCSM.Nodes.CoD.CtL.Meta;
using OCSM.Nodes.DnD.Fifth.Meta;
using OCSM.Nodes.Meta;

namespace OCSM.Nodes
{
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
				case GameSystem.CoD.Changeling:
					generatePopup<CodChangelingAddEditMetadata>(GD.Load<PackedScene>(Constants.Scene.CoD.Changeling.Meta.AddEditMetadata));
					break;
				case GameSystem.DnD.Fifth:
					generatePopup<DndFifthAddEditMetadata>(GD.Load<PackedScene>(Constants.Scene.DnD.Fifth.Meta.AddEditMetadata));
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
}
