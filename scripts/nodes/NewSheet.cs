using Godot;
using System;

public class NewSheet : ScrollContainer
{
	private const string DnDPath = "Row/Column1/D&D/";
	private const string Dnd5thPath = DnDPath + "5E";
	
	private const string CoDPath = "Row/Column2/CoD/";
	private const string CodMortal2e = CoDPath + "Mortal2e";
	private const string CodChangeling2e = CoDPath + "Changeling2e";
	private const string CodMage2e = CoDPath + "Mage2e";
	private const string CodVampire2e = CoDPath + "Vampire2e";
	
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
		GetNode<Button>(CodMortal2e).Connect(Constants.Signal.Pressed, this, nameof(newCoDMortal2e));
	}
	
	private void newCoDMortal2e()
	{
		GetNode<SheetManager>(Constants.NodePath.SheetManager).addNewSheet(Constants.Scene.CoD.MortalSheet, "New Mortal");
		GetParent().GetNode<Control>(Constants.NodePath.SheetTabs).Show();
		QueueFree();
	}
}
