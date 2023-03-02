using Godot;
using OCSM.Nodes.Autoload;

namespace OCSM.Nodes
{
	public partial class NewSheet : ScrollContainer
	{
		private const string Dnd5thPath = "D&D5E";
		private const string CodMortal2e = "Mortal2e";
		private const string CodChangeling2e = "Changeling2e";
		private const string CodMage2e = "Mage2e";
		private const string CodVampire2e = "Vampire2e";
		private const string WodVampireV5 = "VampireV5";
		
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
			tabContainer = GetNode<TabContainer>(Constants.NodePath.SheetTabs);
			
			GetNode<Button>(NodePathBuilder.SceneUnique(CodMortal2e)).Pressed += newCoDMortal2e;
			GetNode<Button>(NodePathBuilder.SceneUnique(CodChangeling2e)).Pressed += newCoDChangeling2e;
			GetNode<Button>(NodePathBuilder.SceneUnique(Dnd5thPath)).Pressed += newDnd5e;
		}
		
		private void newCoDMortal2e() { addSheet(Constants.Scene.CoD.Mortal.Sheet, Constants.Scene.CoD.Mortal.NewSheetName); }
		private void newCoDChangeling2e() { addSheet(Constants.Scene.CoD.Changeling.Sheet, Constants.Scene.CoD.Changeling.NewSheetName); }
		private void newDnd5e() { addSheet(Constants.Scene.DnD.Fifth.Sheet, Constants.Scene.DnD.Fifth.NewSheetName); }
		
		private void addSheet(string sheetPath, string name)
		{
			sheetManager.addNewSheet(sheetPath, name);
			tabContainer.Show();
			QueueFree();
		}
	}
}
