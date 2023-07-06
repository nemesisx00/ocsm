using Godot;
using System.Collections.Generic;
using Ocsm.Dnd.Fifth;

namespace Ocsm.Nodes.Dnd.Fifth
{
	public partial class AbilityScores : HBoxContainer
	{
		private sealed class NodePaths
		{
			public const string Names = "%Names";
			public const string Scores = "%Scores";
			public const string SavingThrows = "%SavingThrows";
			public const string Skills = "%Skills";
		}
		
		public List<Ability> Abilities { get; set; }
		
		public override void _Ready()
		{
		}
	}
}
