using Godot;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Nodes;

public partial class FileMenu : MenuButton
{
	private static class ItemNames
	{
		public static readonly StringName CloseSheet = new("Close Sheet");
		public static readonly StringName New = new("New");
		public static readonly StringName Open = new("Open");
		public static readonly StringName Quit = new("Quit");
		public static readonly StringName Save = new("Save");
	}
	
	public enum MenuItem
	{
		New,
		Open,
		Save,
		CloseSheet,
		Quit,
	}
	
	private AppManager appManager;
	private MetadataManager metadataManager;
	private SheetManager sheetManager;
	
	public override void _Ready()
	{
		appManager = GetNode<AppManager>(AppManager.NodePath);
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
	
	private void handleMenuItem(long id) => handleMenuItem((int)id);
	private void handleMenuItem(int id)
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
				appManager.ShowQuitConfirm();
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
