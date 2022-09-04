using Godot;
using OCSM.Nodes.Autoload;
using OCSM.Nodes.CoD.CtL.Meta;
using OCSM.Nodes.DnD.Fifth.Meta;

namespace OCSM.Nodes
{
	public class MetadataMenu : MenuButton
	{
		public enum MetadataItem { ManageMetadata }
		
		private MetadataManager metadataManager;
		
		public override void _Ready()
		{
			metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
			
			GetPopup().Connect(Constants.Signal.IdPressed, this, nameof(handleMenuItem));
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
					generatePopup<CodChangelingAddEditMetadata>(
						ResourceLoader.Load<PackedScene>(Constants.Scene.CoD.Changeling.Meta.AddEditMetadata),
						nameof(CodChangelingAddEditMetadata.MetadataChanged)
					);
					break;
				case GameSystem.DnD.Fifth:
					generatePopup<DndFifthAddEditMetadata>(
						ResourceLoader.Load<PackedScene>(Constants.Scene.DnD.Fifth.Meta.AddEditMetadata),
						nameof(DndFifthAddEditMetadata.MetadataChanged)
					);
					break;
				default:
					break;
			}
		}
		
		private void generatePopup<T>(PackedScene resource, string signal)
			where T: WindowDialog
		{
			var instance = resource.Instance<T>();
			GetTree().CurrentScene.AddChild(instance);
			NodeUtilities.centerControl(instance, GetViewportRect().GetCenter());
			instance.Popup_();
			instance.Connect(signal, metadataManager, nameof(MetadataManager.saveGameSystemMetadata));
		}
	}
}
