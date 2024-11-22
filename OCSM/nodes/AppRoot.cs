using System;
using System.Linq;
using System.Reflection;
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
				
				if(e.IsActionPressed(Actions.FileNew))
					menu = (int)FileMenu.MenuItem.New;
				else if(e.IsActionPressed(Actions.FileOpen))
					menu = (int)FileMenu.MenuItem.Open;
				else if(e.IsActionPressed(Actions.FileSave))
					menu = (int)FileMenu.MenuItem.Save;
				else if(e.IsActionPressed(Actions.FileCloseSheet))
					menu = (int)FileMenu.MenuItem.CloseSheet;
				
				if(menu is int menuItem)
					EmitSignal(SignalName.FileMenuTriggered, menuItem);
			}
		}
	}
	
	public override void _Ready()
	{
		appManager = GetNode<AppManager>(AppManager.NodePath);
		
		var metadataManager = GetNode<MetadataManager>(MetadataManager.NodePath);
		
		//Register all game systems that have defined a GameSystemFactory class
		AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(t => t.GetTypes())
			.Where(t => t.IsClass && t.Name == MetadataManager.TypeName_GameSystemFactory)
			.ToList()
			.ForEach(t => t.GetMethod("RegisterGameSystem", BindingFlags.Public | BindingFlags.Static)?
				.Invoke(null, [metadataManager.Registry]));
	}
}
