using Godot;
using System;
using OCSM.DnD.Fifth;

namespace OCSM.Nodes.DnD.Fifth
{
	public partial class Skill : Container
	{
		private sealed class Names
		{
			public const string Proficiency = "Proficiency";
			public const string Label = "Label";
			public const string Value = "Value";
		}
		
		private const string PositiveFormat = "+{0}";
		
		[Signal]
		public delegate void ProficiencyChangedEventHandler(string currentState);
		
		[Export]
		public string Label { get; set; } = String.Empty;
		[Export]
		public int AbilityModifier { get; set; } = 0;
		[Export]
		public int ProficiencyBonus { get; set; } = 2;
		
		private StatefulButton proficiency;
		
		public override void _Ready()
		{
			proficiency = GetNode<StatefulButton>(Names.Proficiency);
			proficiency.StateChanged += proficiencyUpdated;
			
			update();
		}
		
		public void setProficiency(Proficiency newProficiency)
		{
			proficiency.CurrentState = ProficiencyUtility.toStatefulButtonState(newProficiency);
			proficiency.updateTexture();
			update();
		}
		
		public void trackAbility(AbilityNode ability)
		{
			ability.AbilityChanged += scoreChanged;
		}
		
		private void proficiencyUpdated(StatefulButton button)
		{
			update();
			EmitSignal(nameof(ProficiencyChanged), button.CurrentState);
		}
		
		private void scoreChanged(Transport<Ability> transport)
		{
			AbilityModifier = transport.Value.Modifier;
			update();
		}
		
		private void update()
		{
			var value = AbilityModifier;
			switch(proficiency.CurrentState)
			{
				case StatefulButton.State.One:
					value += ProficiencyBonus / 2;
					break;
				case StatefulButton.State.Two:
					value += ProficiencyBonus;
					break;
				case StatefulButton.State.Three:
					value += ProficiencyBonus * 2;
					break;
				default:
					break;
			}
			
			var valueString = String.Format(PositiveFormat, value);
			if(value < 0)
				valueString = value.ToString();
			
			GetNode<Label>(Names.Label).Text = Label;
			GetNode<Label>(Names.Value).Text = valueString;
		}
	}
}
