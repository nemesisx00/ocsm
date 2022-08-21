using Godot;
using OCSM;

public class FileMenu : MenuButton
{
	public enum MenuItem { New, Open, Save, CloseSheet, Quit }
	
	private SheetManager sheetManager;
	
	public override void _Ready()
	{
		sheetManager = GetNode<SheetManager>(Constants.NodePath.SheetManager);
		
		GetPopup().Connect(Constants.Signal.IdPressed, this, nameof(handleMenuItem));
		GetNode<AppRoot>(Constants.NodePath.AppRoot).Connect(nameof(AppRoot.ShortcutTriggered), this, nameof(handleMenuItem));
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
			case MenuItem.Quit:
				GetTree().Notification(MainLoop.NotificationWmQuitRequest);
				break;
			case MenuItem.CloseSheet:
				sheetManager.closeActiveSheet();
				break;
		}
	}
	
	private void doOpen()
	{
		GD.Print("Do Open!");
	}
	
	private void doSave()
	{
		GD.Print("Do Save!");
	}
}
