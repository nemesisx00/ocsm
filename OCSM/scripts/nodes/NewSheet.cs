using Godot;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Nodes;

public partial class NewSheet : ScrollContainer
{
	private sealed class NodePath
	{
		public const string Dnd5thPath = "%D&D5E";
		public const string CodMortal2e = "%Mortal2e";
		public const string CodChangeling2e = "%Changeling2e";
		public const string CodMage2e = "%Mage2e";
		public const string CodVampire2e = "%Vampire2e";
		public const string WodVampireV5 = "%VampireV5";
	}
	
	private SheetManager sheetManager;
	private TabContainer tabContainer;
	
	public override void _Input(InputEvent e)
	{
		if(e is InputEventKey ek && ek.IsActionReleased(Constants.Action.Cancel)
			&& tabContainer.GetChildren().Count > 0)
		{
			sheetManager.hideNewSheetUI();
		}
	}
	
	public override void _Ready()
	{
		sheetManager = GetNode<SheetManager>(Constants.NodePath.SheetManager);
		tabContainer = GetNode<TabContainer>(AppRoot.NodePath.SheetTabs);
		
		GetNode<Button>(NodePath.CodMortal2e).Pressed += newCofdMortal2e;
		GetNode<Button>(NodePath.CodChangeling2e).Pressed += newCofdChangeling2e;
		GetNode<Button>(NodePath.Dnd5thPath).Pressed += newDnd5e;
	}
	
	private void newCofdMortal2e() { addSheet(Constants.Scene.Cofd.Mortal.Sheet, Constants.Scene.Cofd.Mortal.NewSheetName); }
	private void newCofdChangeling2e() { addSheet(Constants.Scene.Cofd.Changeling.Sheet, Constants.Scene.Cofd.Changeling.NewSheetName); }
	private void newDnd5e() { addSheet(Constants.Scene.Dnd.Fifth.Sheet, Constants.Scene.Dnd.Fifth.NewSheetName); }
	
	private void addSheet(string sheetPath, string name)
	{
		sheetManager.addNewSheet(sheetPath, name);
		tabContainer.Show();
		QueueFree();
	}
}
