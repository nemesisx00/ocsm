using Godot;
using System.Text.Json;
using OCSM;

public class NewSheet : ScrollContainer
{
	private const string Dnd5thPath = "D&D5E";
	private const string CodMortal2e = "Mortal2e";
	private const string CodChangeling2e = "Changeling2e";
	private const string CodMage2e = "Mage2e";
	private const string CodVampire2e = "Vampire2e";
	private const string WodVampireV5 = "VampireV5";
	
	public override void _Input(InputEvent e)
	{
		if(e is InputEventKey ek && ek.IsActionReleased(Constants.Action.Cancel)
			&& GetNode<TabContainer>(Constants.NodePath.SheetTabs).GetChildren().Count > 0)
		{
			GetNode<SheetManager>(Constants.NodePath.SheetManager).hideNewSheetUI();
		}
	}
	
	public override void _Ready()
	{
		GetNode<Button>(PathBuilder.SceneUnique(CodMortal2e)).Connect(Constants.Signal.Pressed, this, nameof(newCoDMortal2e));
		GetNode<Button>(PathBuilder.SceneUnique(CodChangeling2e)).Connect(Constants.Signal.Pressed, this, nameof(newCoDChangeling2e));
		
		GD.Print(JsonSerializer.Serialize(new Mortal()));
	}
	
	private void newCoDMortal2e() { addSheet(Constants.Scene.CoD.Mortal.Sheet, "New Mortal"); }
	private void newCoDChangeling2e() { addSheet(Constants.Scene.CoD.Changeling.Sheet, "New Changeling"); }
	
	private void addSheet(string sheetPath, string name)
	{
		GetNode<SheetManager>(Constants.NodePath.SheetManager).addNewSheet(sheetPath, name);
		GetParent().GetNode<Control>(Constants.NodePath.SheetTabs).Show();
		QueueFree();
	}
}
