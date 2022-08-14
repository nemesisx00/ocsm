using Godot;
using System;

public class FileMenu : MenuButton
{
	private enum MenuItem { New, Open, Save, Quit }
	
	public override void _Ready()
	{
		GetPopup().Connect(Constants.Signal.IdPressed, this, nameof(handleMenuItem));
	}
	
	private void handleMenuItem(int id)
	{
		switch((MenuItem)id)
		{
			case MenuItem.New:
				doNew();
				break;
			case MenuItem.Open:
				doOpen();
				break;
			case MenuItem.Save:
				doSave();
				break;
			case MenuItem.Quit:
				doQuit();
				break;
		}
	}
	
	private void doNew()
	{
		GD.Print("Do New!");
	}
	
	private void doOpen()
	{
		GD.Print("Do Open!");
	}
	
	private void doSave()
	{
		GD.Print("Do Save!");
	}
	
	private void doQuit()
	{
		GetTree().Notification(MainLoop.NotificationWmQuitRequest);
	}
}
