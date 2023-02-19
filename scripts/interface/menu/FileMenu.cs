using Godot;
using OCSM.Nodes.Autoload;

namespace OCSM.Nodes
{
	public partial class FileMenu : MenuButton
	{
		public enum MenuItem { New, Open, Save, CloseSheet, Quit }
		
		private MetadataManager metadataManager;
		private SheetManager sheetManager;
		
		public override void _Ready()
		{
			metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
			sheetManager = GetNode<SheetManager>(Constants.NodePath.SheetManager);
			
			GetPopup().IdPressed += handleMenuItem;
			GetNode<AppRoot>(Constants.NodePath.AppRoot).FileMenuTriggered += handleMenuItem;
		}
		
		private void handleMenuItem(long id)
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
					GetTree().Notification((int)NotificationWMCloseRequest);
					break;
			}
		}
		
		private void doOpen()
		{
			var resource = ResourceLoader.Load<PackedScene>(Constants.Scene.OpenSheet);
			var instance = resource.Instantiate<OpenSheet>();
			GetTree().CurrentScene.AddChild(instance);
			instance.PopupCentered();
			instance.JsonLoaded += handleOpenJson;
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
				var instance = resource.Instantiate<SaveSheet>();
				instance.SheetData = data;
				GetTree().CurrentScene.AddChild(instance);
				instance.PopupCentered();
			}
		}
	}
}
