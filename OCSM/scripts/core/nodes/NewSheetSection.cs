using System.Linq;
using Godot;

namespace Ocsm.Nodes;

public partial class NewSheetSection : HBoxContainer
{
	private static class NodePaths
	{
		public static readonly NodePath GameSystem = new("%GameSystem");
		public static readonly NodePath ButtonGrid = new("%ButtonGrid");
	}
	
	[Signal]
	public delegate void RequestAddSheetEventHandler(string path, string name);
	
	[Export]
	public string GameSystem { get; set; }
	
	private GridContainer buttonGrid;
	private Label gameSystem;
	
	public override void _ExitTree()
	{
		foreach(var gb in buttonGrid.GetChildren().Cast<AddSheet>())
			gb.RequestAddSheet -= handleRequestAddSheet;
		
		base._ExitTree();
	}
	
	public override void _Ready()
	{
		buttonGrid = GetNode<GridContainer>(NodePaths.ButtonGrid);
		gameSystem = GetNode<Label>(NodePaths.GameSystem);
		
		gameSystem.Text = GameSystem;
	}
	
	public void AddGameButton(AddSheet button)
	{
		buttonGrid.AddChild(button);
		button.RequestAddSheet += handleRequestAddSheet;
	}
	
	private void handleRequestAddSheet(string path, string name)
		=> EmitSignal(SignalName.RequestAddSheet, path, name);
}
