using Godot;
using OCSM.Nodes.Autoload;
using OCSM.Nodes.CoD.CtL.Meta;
using OCSM.Nodes.DnD.Fifth.Meta;
using OCSM.Nodes.Meta;

namespace OCSM.Nodes
{
	public partial class MetadataMenu : MenuButton
	{
		public enum MetadataItem { ManageMetadata }
		
		private MetadataManager metadataManager;
		
		public override void _Ready()
		{
			metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
			
			GetPopup().Connect(Constants.Signal.IdPressed,new Callable(this,nameof(handleMenuItem)));
		}
		
		private void handleMenuItem(int id)
		{
			switch((MetadataItem)id)
			{
				case MetadataItem.ManageMetadata:
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
					generatePopup<CodChangelingAddEditMetadata>(ResourceLoader.Load<PackedScene>(Constants.Scene.CoD.Changeling.Meta.AddEditMetadata));
					break;
				case GameSystem.DnD.Fifth:
					generatePopup<DndFifthAddEditMetadata>(ResourceLoader.Load<PackedScene>(Constants.Scene.DnD.Fifth.Meta.AddEditMetadata));
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
