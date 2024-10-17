using Godot;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class ClassRow : HBoxContainer
{
	private static class NodePaths
	{
		public static readonly NodePath Label = new("%Label");
		public static readonly NodePath Level = new("%Level");
		public static readonly NodePath HitDie = new("%HitDie");
		public static readonly NodePath HitDiceCurrent = new("%HitDiceCurrent");
	}
	
	[Signal]
	public delegate void HitDiceChangedEventHandler(ClassRow node);
	[Signal]
	public delegate void LevelChangedEventHandler(ClassRow node);
	
	public string ClassName
	{
		get => label.Text;
		set => label.Text = value;
	}
	
	public int Level
	{
		get => (int)level.Value;
		
		set
		{
			level.Value = value;
			hitDiceCurrent.MaxValue = value;
		}
	}
	
	public Die HitDie
	{
		get => hitDie.SelectedDie;
		set => hitDie.SelectedDie = value;
	}
	
	public int HitDiceCurrent
	{
		get => (int)hitDiceCurrent.Value;
		set => hitDiceCurrent.Value = value;
	}
	
	private Label label;
	private SpinBox level;
	private DieOptionsButton hitDie;
	private SpinBox hitDiceCurrent;
	
	public override void _Ready()
	{
		label = GetNode<Label>(NodePaths.Label);
		level = GetNode<SpinBox>(NodePaths.Level);
		hitDie = GetNode<DieOptionsButton>(NodePaths.HitDie);
		hitDiceCurrent = GetNode<SpinBox>(NodePaths.HitDiceCurrent);
		
		level.ValueChanged += _ => EmitSignal(SignalName.LevelChanged, this);
		hitDie.ItemSelected += _ => EmitSignal(SignalName.HitDiceChanged, this);
		hitDiceCurrent.ValueChanged += _ => EmitSignal(SignalName.HitDiceChanged, this);
	}
}
