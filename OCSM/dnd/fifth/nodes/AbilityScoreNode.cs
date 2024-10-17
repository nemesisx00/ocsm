using Godot;
using Ocsm.Nodes;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class AbilityScoreNode : Control
{
	private static class NodePaths
	{
		public static readonly NodePath AbilityName = new("%AbilityName");
		public static readonly NodePath Modifier = new("%Modifier");
		public static readonly NodePath Score = new("%Score");
	}
	
	public const int DefaultValue = 10;
	
	[Export]
	public string AbilityName { get; set; }
	
	private Label abilityName;
	private Label modifier;
	private DynamicNumericLabel score;
	
	public override void _ExitTree()
	{
		score.ValueChanged -= handleScoreChanged;
		
		base._ExitTree();
	}
	
	public override void _Ready()
	{
		abilityName = GetNode<Label>(NodePaths.AbilityName);
		modifier = GetNode<Label>(NodePaths.Modifier);
		score = GetNode<DynamicNumericLabel>(NodePaths.Score);
		score.ValueChanged += handleScoreChanged;
		
		abilityName.Text = AbilityName;
		score.Value = DefaultValue;
	}
	
	private void handleScoreChanged(double value) => modifier.Text = StringUtilities.FormatModifier((int)(value / 2) - 5);
}
