using Godot;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Nodes;

public partial class NewSheet : ScrollContainer
{
	private static class NodePaths
	{
		public static readonly NodePath Dnd5thPath = new("%D&D5E");
		public static readonly NodePath CofdMortal2e = new("%Mortal2e");
		public static readonly NodePath CofdChangeling2e = new("%Changeling2e");
		public static readonly NodePath CofdMage2e = new("%Mage2e");
		public static readonly NodePath CofdVampire2e = new("%Vampire2e");
		public static readonly NodePath WodVampireV5 = new("%VampireV5");
	}
	
	private SheetManager sheetManager;
	private TabContainer tabContainer;
	
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
		sheetManager = GetNode<SheetManager>(SheetManager.NodePath);
		tabContainer = GetNode<TabContainer>(AppRoot.NodePaths.SheetTabs);
		
		GetNode<Button>(NodePaths.CofdMortal2e).Pressed += newCofdMortal2e;
		GetNode<Button>(NodePaths.CofdChangeling2e).Pressed += newCofdChangeling2e;
		GetNode<Button>(NodePaths.Dnd5thPath).Pressed += newDnd5e;
	}
	
	private void newCofdMortal2e() => addSheet(ScenePaths.Cofd.Mortal.Sheet, ScenePaths.Cofd.Mortal.NewSheetName);
	private void newCofdChangeling2e() => addSheet(ScenePaths.Cofd.Changeling.Sheet, ScenePaths.Cofd.Changeling.NewSheetName);
	private void newDnd5e() => addSheet(ScenePaths.Dnd.Fifth.Sheet, ScenePaths.Dnd.Fifth.NewSheetName);
	
	private void addSheet(string sheetPath, string name)
	{
		sheetManager.AddNewSheet(sheetPath, name);
		tabContainer.Show();
		QueueFree();
	}
}
