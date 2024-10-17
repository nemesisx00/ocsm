using Godot;
using Ocsm.Nodes;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class HitPointsNode : Container
{
	private static class NodePaths
	{
		public static readonly NodePath CurrentHP = new("%CurrentHP");
		public static readonly NodePath MaxHP = new("%MaxHP");
		public static readonly NodePath TempHP = new("%TempHP");
	}
	
	public DynamicNumericLabel CurrentHP => currentHp;
	public DynamicNumericLabel MaxHP => maxHp;
	public DynamicNumericLabel TempHP => tempHp;
	
	private DynamicNumericLabel currentHp;
	private DynamicNumericLabel maxHp;
	private DynamicNumericLabel tempHp;
	
	public override void _Ready()
	{
		currentHp = GetNode<DynamicNumericLabel>(NodePaths.CurrentHP);
		maxHp = GetNode<DynamicNumericLabel>(NodePaths.MaxHP);
		tempHp = GetNode<DynamicNumericLabel>(NodePaths.TempHP);
	}
}
