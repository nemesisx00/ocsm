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
		var resource = GD.Load<PackedScene>(Constants.Scene.CoD.MortalSheet);
		var instance = resource.Instance();
		instance.Name = "New Mortal";
		
		var target = GetParent().GetParent().GetNode("SheetTabs");
		if(target is TabContainer tc)
		{
			var dupeCount = 0;
			foreach(Node c in tc.GetChildren())
			{
				if(c.Name.Contains(instance.Name))
					dupeCount++;
			}
			
			if(dupeCount > 0)
				instance.Name += String.Format(" ({0})", dupeCount);
			
			tc.AddChild(instance);
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
	
	private void doQuit()
	{
		GetTree().Notification(MainLoop.NotificationWmQuitRequest);
	}
}
