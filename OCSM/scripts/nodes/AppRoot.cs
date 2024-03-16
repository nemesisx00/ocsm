using Godot;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Nodes;

public partial class AppRoot : Control
{
	public static class NodePaths
	{
		public static readonly NodePath Self = new("/root/AppRoot");
		public static readonly NodePath SheetTabs = new(Self + "/%SheetTabs");
		public static readonly NodePath NewSheet = new(Self + "/%NewSheet");
	}
	
	[Signal]
	public delegate void FileMenuTriggeredEventHandler(int menuItem);
	[Signal]
	public delegate void HelpMenuTriggeredEventHandler(int menuItem);
	
	private AppManager appManager;
	
	public override void _Input(InputEvent e)
	{
		if(!appManager.IsQuitting)
		{
			if(e is InputEventKey iek && iek.Pressed)
			{
				int? menu = null;
				
				if(e.IsActionPressed(Constants.Action.FileNew))
					menu = (int)FileMenu.MenuItem.New;
				else if(e.IsActionPressed(Constants.Action.FileOpen))
					menu = (int)FileMenu.MenuItem.Open;
				else if(e.IsActionPressed(Constants.Action.FileSave))
					menu = (int)FileMenu.MenuItem.Save;
				else if(e.IsActionPressed(Constants.Action.FileCloseSheet))
					menu = (int)FileMenu.MenuItem.CloseSheet;
				
				if(menu is int menuItem)
					EmitSignal(SignalName.FileMenuTriggered, menuItem);
			}
		}
	}
	
	public override void _Ready()
	{
		appManager = GetNode<AppManager>(Constants.NodePath.AppManager);
		GetNode<MetadataManager>(Constants.NodePath.MetadataManager).InitializeGameSystems();
	}
}
