using Godot;

namespace Ocsm.Nodes;

public partial class AddSheet : VBoxContainer
{
	private static class NodePaths
	{
		public static readonly NodePath Button = new("%TextureButton");
		public static readonly NodePath Game = new("%Game");
		public static readonly NodePath Edition = new("%Edition");
	}
	
	[Signal]
	public delegate void RequestAddSheetEventHandler(string sheetPath, string name);
	
	[Export]
	public string GameLabel { get; set; }
	
	[Export]
	public string Edition { get; set; }
	
	[Export]
	public string SheetPath { get; set; }
	
	[Export]
	public string SheetName { get; set; }
	
	[Export]
	public Texture2D TextureNormal { get; set; }
	
	[Export]
	public Texture2D TextureDisabled { get; set; }
	
	private TextureButton button;
	private Label edition;
	private Label game;
	
	public override void _Ready()
	{
		button = GetNode<TextureButton>(NodePaths.Button);
		edition = GetNode<Label>(NodePaths.Edition);
		game = GetNode<Label>(NodePaths.Game);
		
		button.TextureNormal = TextureNormal;
		button.TextureDisabled = TextureDisabled;
		edition.Text = Edition;
		game.Text = GameLabel;
		
		if(string.IsNullOrEmpty(SheetPath))
		{
			button.Disabled = true;
			button.MouseDefaultCursorShape = CursorShape.Arrow;
		}
		else
			button.Pressed += () => EmitSignal(SignalName.RequestAddSheet, SheetPath, SheetName);
	}
}
