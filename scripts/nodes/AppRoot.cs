using Godot;
using OCSM;

public class AppRoot : Control
{
	public const string SheetTabsName = "SheetTabs";
	private const string FileMenuName = "FileMenu";
	private const string HelpMenuName = "HelpMenu";
	
	[Signal]
	public delegate void ShortcutTriggered(int menuItem);
	
	private AppManager appManager;
	
	public override void _Input(InputEvent e)
	{
		if(!appManager.IsQuitting)
		{
			if(e is InputEventKey iek && iek.Pressed)
			{
				if(e.IsActionPressed(Constants.Action.FileNew))
					EmitSignal(nameof(ShortcutTriggered), FileMenu.MenuItem.New);
				else if(e.IsActionPressed(Constants.Action.FileOpen))
					EmitSignal(nameof(ShortcutTriggered), FileMenu.MenuItem.Open);
				else if(e.IsActionPressed(Constants.Action.FileSave))
					EmitSignal(nameof(ShortcutTriggered), FileMenu.MenuItem.Save);
				else if(e.IsActionPressed(Constants.Action.FileCloseSheet))
					EmitSignal(nameof(ShortcutTriggered), FileMenu.MenuItem.CloseSheet);
			}
		}
	}
	
	public override void _Ready()
	{
		appManager = GetNode<AppManager>(Constants.NodePath.AppManager);
	}
}
