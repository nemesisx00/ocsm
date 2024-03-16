using Godot;
using System;
using Ocsm.Dnd.Fifth;

namespace Ocsm.Nodes.Dnd.Fifth;

public partial class Skill : Container
{
	private sealed class Names
	{
		public const string Proficiency = "Proficiency";
		public const string Label = "Label";
		public const string Value = "Value";
	}
	
	[Signal]
	public delegate void ProficiencyChangedEventHandler(StatefulButton.States currentState);
	
	[Export]
	public string Label { get; set; } = string.Empty;
	[Export]
	public int AbilityModifier { get; set; }
	[Export]
	public int ProficiencyBonus { get; set; } = 2;
	
	private Label label;
	private StatefulButton proficiency;
	private Label value;
	
	public override void _Ready()
	{
		proficiency = GetNode<StatefulButton>(Names.Proficiency);
		proficiency.StateChanged += proficiencyUpdated;
		
		label = GetNode<Label>(Names.Label);
		value = GetNode<Label>(Names.Value);
		
		update();
	}
	
	public void SetProficiency(Proficiency newProficiency)
	{
		proficiency.CurrentState = newProficiency.ToStatefulButtonState();
		proficiency.UpdateTexture();
		update();
	}
	
	public void TrackAbility(AbilityColumn ability) => ability.AbilityChanged += scoreChanged;
	public void TrackAbility(AbilityRow ability) => ability.AbilityChanged += scoreChanged;
	
	private void proficiencyUpdated(StatefulButton button)
	{
		update();
		EmitSignal(SignalName.ProficiencyChanged, (int)button.CurrentState);
	}
	
	private void scoreChanged(Transport<Ability> transport)
	{
		AbilityModifier = transport.Value.Modifier;
		update();
	}
	
	private void update()
	{
		var modifier = AbilityModifier;
		switch(proficiency.CurrentState)
		{
			case StatefulButton.States.One:
				modifier += ProficiencyBonus / 2;
				break;
			
			case StatefulButton.States.Two:
				modifier += ProficiencyBonus;
				break;
			
			case StatefulButton.States.Three:
				modifier += ProficiencyBonus * 2;
				break;
		}
		
		label.Text = Label;
		value.Text = modifier < 0 ? modifier.ToString() : $"+{modifier}";
	}
}
