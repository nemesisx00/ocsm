using Godot;
using OCSM.Nodes.Autoload;
using OCSM.Nodes.CoD.CtL.Meta;

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
					var resource = ResourceLoader.Load<PackedScene>(Constants.Scene.CoD.Changeling.Meta.AddEditMetadata);
					var instance = resource.Instance<AddEditMetadata>();
					GetTree().CurrentScene.AddChild(instance);
					NodeUtilities.centerControl(instance, GetViewportRect().GetCenter());
					instance.Popup_();
					instance.Connect(nameof(AddEditMetadata.MetadataChanged), metadataManager, nameof(MetadataManager.saveGameSystemMetadata));
					break;
				default:
					break;
			}
		}
	}
}
