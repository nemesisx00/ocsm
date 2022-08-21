using Godot;
using OCSM;

public class FileMenu : MenuButton
{
	private enum MenuItem { New, Open, Save, CloseSheet, Quit }
	
	public override void _Ready()
	{
		GetPopup().Connect(Constants.Signal.IdPressed, this, nameof(handleMenuItem));
	}
	
	private void handleMenuItem(int id)
	{
		switch((MenuItem)id)
		{
			case MenuItem.New:
				GetNode<SheetManager>(Constants.NodePath.SheetManager).showNewSheetUI();
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
				GetNode<SheetManager>(Constants.NodePath.SheetManager).closeActiveSheet();
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
