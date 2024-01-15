using Godot;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Nodes;

public partial class FileMenu : MenuButton
{
	private sealed class ItemNames
	{
		public const string CloseSheet = "Close Sheet";
		public const string New = "New";
		public const string Open = "Open";
		public const string Quit = "Quit";
		public const string Save = "Save";
	}
	
	public enum MenuItem : long
	{
		New,
		Open,
		Save,
		CloseSheet,
		Quit,
	}
	
	private MetadataManager metadataManager;
	private SheetManager sheetManager;
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(MetadataManager.NodePath);
		sheetManager = GetNode<SheetManager>(SheetManager.NodePath);
		
		var popup = GetPopup();
		popup.AddItem(ItemNames.New, (int)MenuItem.New);
		popup.AddItem(ItemNames.Open, (int)MenuItem.Open);
		popup.AddItem(ItemNames.Save, (int)MenuItem.Save);
		popup.AddItem(ItemNames.CloseSheet, (int)MenuItem.CloseSheet);
		popup.AddSeparator();
		popup.AddItem(ItemNames.Quit, (int)MenuItem.Quit);
		popup.IdPressed += handleMenuItem;
		
		GetNode<AppRoot>(AppRoot.NodePaths.Self).FileMenuTriggered += handleMenuItem;
	}
	
	private void handleMenuItem(long id)
	{
		switch((MenuItem)id)
		{
			case MenuItem.New:
				sheetManager.ShowNewSheetUI();
				break;
			
			case MenuItem.Open:
				doOpen();
				break;
			
			case MenuItem.Save:
				doSave();
				break;
			
			case MenuItem.CloseSheet:
				sheetManager.CloseActiveSheet();
				break;
			
			case MenuItem.Quit:
				//FIXME: GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest);
				//Determine why firing the notification doesn't work here.
				//The window is created but then its Canceled signal is triggered before it's even finished drawing.
				//Calling AppManager's showQuitConfirm manually is good enough for now.
				GetNode<AppManager>(AppManager.NodePath).ShowQuitConfirm();
				break;
		}
	}
	
	private void doOpen()
	{
		var resource = GD.Load<PackedScene>(ScenePaths.OpenSheet);
		var instance = resource.Instantiate<OpenSheet>();
		GetTree().CurrentScene.AddChild(instance);
		instance.PopupCentered();
		instance.JsonLoaded += handleOpenJson;
	}
	
	private void handleOpenJson(string json) => sheetManager.LoadSheetJsonData(json);
	
	private void doSave()
	{
		var data = sheetManager.GetActiveSheetJsonData();
		
		if(data != null)
		{
			var resource = GD.Load<PackedScene>(ScenePaths.SaveSheet);
			var instance = resource.Instantiate<SaveSheet>();
			instance.SheetData = data;
			GetTree().CurrentScene.AddChild(instance);
			instance.PopupCentered();
		}
	}
}
