using Godot;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Nodes;

public partial class AppRoot : Control
{
	public sealed class NodePaths
	{
		public static readonly NodePath Self = new("/root/AppRoot");
		public static readonly NodePath SheetTabs = new($"{Self}/%SheetTabs");
		public static readonly NodePath NewSheet = new($"{Self}/%NewSheet");
	}
	
	[Signal]
	public delegate void FileMenuTriggeredEventHandler(long menuItem);
	[Signal]
	public delegate void HelpMenuTriggeredEventHandler(long menuItem);
	
	private AppManager appManager;
	
	public override void _Input(InputEvent e)
	{
		if(!appManager.IsQuitting)
		{
			if(e is InputEventKey iek && iek.Pressed)
			{
				if(e.IsActionPressed(Constants.Action.FileNew))
					_ = EmitSignal(SignalName.FileMenuTriggered, (long)FileMenu.MenuItem.New);
				else if(e.IsActionPressed(Constants.Action.FileOpen))
					_ = EmitSignal(SignalName.FileMenuTriggered, (long)FileMenu.MenuItem.Open);
				else if(e.IsActionPressed(Constants.Action.FileSave))
					_ = EmitSignal(SignalName.FileMenuTriggered, (long)FileMenu.MenuItem.Save);
				else if(e.IsActionPressed(Constants.Action.FileCloseSheet))
					_ = EmitSignal(SignalName.FileMenuTriggered, (long)FileMenu.MenuItem.CloseSheet);
			}
		}
	}
	
	public override void _Ready()
	{
		appManager = GetNode<AppManager>(AppManager.NodePath);
		GetNode<MetadataManager>(MetadataManager.NodePath).InitializeGameSystems();
	}
}
