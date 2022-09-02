using Godot;
using System;

namespace OCSM.Nodes.DnD.Fifth
{
	public class Ability : Container
	{
		private sealed class Names
		{
			public const string Name = "Name";
			public const string Score = "Score";
			public const string Modifier = "Modifier";
		}
		
		[Export]
		private string AbilityName { get; set; } = String.Empty;
		
		public int Value
		{
			get
			{
				return (int)score.Value;
			}
			
			set
			{
				score.Value = value;
				
			}
		}
		
		private Label label;
		private SpinBox score;
		private SpinBox modifier;
		
		public override void _Ready()
		{
			label = GetNode<Label>(NodePathBuilder.SceneUnique(Names.Name));
			score = GetNode<SpinBox>(NodePathBuilder.SceneUnique(Names.Score));
			modifier = GetNode<SpinBox>(NodePathBuilder.SceneUnique(Names.Modifier));
			
			label.Text = AbilityName;
			score.Connect(Constants.Signal.ValueChanged, this, nameof(scoreChanged));
		}
		
		private void calculateModifier()
		{
			modifier.Value = (int)(Value / 2) - 5;
			if(modifier.Value >= 0)
				modifier.Prefix = "+";
			else
				modifier.Prefix = String.Empty;
		}
		
		private void scoreChanged(int value)
		{
			calculateModifier();
		}
	}
}
