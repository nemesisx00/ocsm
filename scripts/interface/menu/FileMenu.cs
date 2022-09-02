using Godot;
using OCSM.Nodes.Autoload;

namespace OCSM.Nodes
{
	public class FileMenu : MenuButton
	{
		public enum MenuItem { New, Open, Save, CloseSheet, Quit }
		
		private MetadataManager metadataManager;
		private SheetManager sheetManager;
		
		public override void _Ready()
		{
			metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
			sheetManager = GetNode<SheetManager>(Constants.NodePath.SheetManager);
			
			GetPopup().Connect(Constants.Signal.IdPressed, this, nameof(handleMenuItem));
			GetNode<AppRoot>(Constants.NodePath.AppRoot).Connect(nameof(AppRoot.FileMenuTriggered), this, nameof(handleMenuItem));
		}
		
		private void handleMenuItem(int id)
		{
			switch((MenuItem)id)
			{
				case MenuItem.New:
					sheetManager.showNewSheetUI();
					break;
				case MenuItem.Open:
					doOpen();
					break;
				case MenuItem.Save:
					doSave();
					break;
				case MenuItem.CloseSheet:
					sheetManager.closeActiveSheet();
					break;
				case MenuItem.Quit:
					GetTree().Notification(MainLoop.NotificationWmQuitRequest);
					break;
			}
		}
		
		private void doOpen()
		{
			var resource = ResourceLoader.Load<PackedScene>(Constants.Scene.OpenSheet);
			var instance = resource.Instance<OpenSheet>();
			GetTree().CurrentScene.AddChild(instance);
			instance.Popup_();
			NodeUtilities.centerControl(instance, GetViewportRect().GetCenter());
			instance.Connect(nameof(OpenSheet.JsonLoaded), this, nameof(handleOpenJson));
		}
		
		private void handleOpenJson(string json)
		{
			sheetManager.loadSheetJsonData(json);
		}
		
		private void doSave()
		{
			var data = sheetManager.getActiveSheetJsonData();
			if(data != null)
			{
				var resource = ResourceLoader.Load<PackedScene>(Constants.Scene.SaveSheet);
				var instance = resource.Instance<SaveSheet>();
				instance.SheetData = data;
				GetTree().CurrentScene.AddChild(instance);
				instance.Popup_();
				NodeUtilities.centerControl(instance, GetViewportRect().GetCenter());
			}
		}
	}
}
