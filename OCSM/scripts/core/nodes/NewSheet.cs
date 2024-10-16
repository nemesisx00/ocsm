using System.Collections.Generic;
using System.Linq;
using Godot;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Nodes;

public partial class NewSheet : ScrollContainer
{
	private static class NodePaths
	{
		public static readonly NodePath Grid = new("%Grid");
	}
	
	private GridContainer grid;
	private PackedScene sectionScene;
	private SheetManager sheetManager;
	private TabContainer tabContainer;
	
	public override void _ExitTree()
	{
		foreach(var section in grid.GetChildren().Where(c => c is NewSheetSection).Cast<NewSheetSection>())
			section.RequestAddSheet -= addSheet;
		
		base._ExitTree();
	}
	
	public override void _Input(InputEvent e)
	{
		if(e is InputEventKey ek && ek.IsActionReleased(Actions.Cancel)
			&& tabContainer.GetChildren().Count > 0)
		{
			sheetManager.HideNewSheetUI();
		}
	}
	
	public override void _Ready()
	{
		sectionScene = GD.Load<PackedScene>(ScenePaths.NewSheetSection);
		sheetManager = GetNode<SheetManager>(SheetManager.NodePath);
		tabContainer = GetNode<TabContainer>(AppRoot.NodePaths.SheetTabs);
		grid = GetNode<GridContainer>(NodePaths.Grid);
		
		addSection("Dungeons & Dragons", Dnd.Fifth.GameButtonFactory.GenerateButtons());
		addSection("Chronicles of Darkness", Cofd.GameButtonFactory.GenerateButtons());
	}
	
	private void addSection(string label, List<AddSheet> buttons)
	{
		if(sectionScene.CanInstantiate())
		{
			var section = sectionScene.Instantiate<NewSheetSection>();
			section.GameSystem = label;
			grid.AddChild(section);
			section.RequestAddSheet += addSheet;
			
			foreach(var b in buttons)
				section.AddGameButton(b);
		}
	}
	
	private void addSheet(string path, string name)
	{
		sheetManager.AddNewSheet(path, name);
		tabContainer.Show();
		QueueFree();
	}
}
