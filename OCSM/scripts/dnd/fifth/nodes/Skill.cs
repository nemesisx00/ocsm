using Godot;
using System;
using Ocsm.Nodes;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class Skill : Container
{
	private sealed class NodePaths
	{
		public static readonly NodePath Proficiency = new("Proficiency");
		public static readonly NodePath Label = new("Label");
		public static readonly NodePath Value = new("Value");
	}
	
	private const string PositiveFormat = "+{0}";
	
	[Signal]
	public delegate void ProficiencyChangedEventHandler(int currentState);
	
	[Export]
	public string Label { get; set; }
	[Export]
	public int AbilityModifier { get; set; }
	[Export]
	public int ProficiencyBonus { get; set; } = 2;
	
	private StatefulButton proficiency;
	
	public override void _Ready()
	{
		proficiency = GetNode<StatefulButton>(NodePaths.Proficiency);
		proficiency.StateChanged += proficiencyUpdated;
		
		update();
	}
	
	public void SetProficiency(Proficiency newProficiency)
	{
		proficiency.State = ProficiencyUtility.ToStatefulButtonState(newProficiency);
		proficiency.UpdateTexture();
		update();
	}
	
	public void TrackAbility(AbilityColumn ability) => ability.AbilityChanged += scoreChanged;
	public void TrackAbility(AbilityRow ability) => ability.AbilityChanged += scoreChanged;
	
	private void proficiencyUpdated(StatefulButton button)
	{
		update();
		_ = EmitSignal(SignalName.ProficiencyChanged, (int)button.State);
	}
	
	private void scoreChanged(Transport<Ability> transport)
	{
		AbilityModifier = transport.Value.Modifier;
		update();
	}
	
	private void update()
	{
		var value = AbilityModifier;
		switch(proficiency.State)
		{
			case StatefulButton.States.One:
				value += ProficiencyBonus / 2;
				break;
			
			case StatefulButton.States.Two:
				value += ProficiencyBonus;
				break;
			
			case StatefulButton.States.Three:
				value += ProficiencyBonus * 2;
				break;
		}
		
		var valueString = string.Format(PositiveFormat, value);
		
		if(value < 0)
			valueString = value.ToString();
		
		GetNode<Label>(NodePaths.Label).Text = Label;
		GetNode<Label>(NodePaths.Value).Text = valueString;
	}
}
