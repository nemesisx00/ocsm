using Godot;
using Ocsm.Nodes;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class SkillNode : Container
{
	private static class NodePaths
	{
		public static readonly NodePath Proficiency = new("Proficiency");
		public static readonly NodePath Label = new("Label");
		public static readonly NodePath Value = new("Value");
	}
	
	[Signal]
	public delegate void ProficiencyChangedEventHandler(StatefulButton.States currentState);
	
	[Export]
	public string Label { get; set; } = string.Empty;
	[Export]
	public int AbilityModifier { get; set; }
	[Export]
	public int ProficiencyBonus { get; set; } = 2;
	
	private AbilityColumn abilityColumn;
	private AbilityRow abilityRow;
	private Label label;
	private StatefulButton proficiency;
	private Label value;
	
	public override void _ExitTree()
	{
		if(abilityColumn is not null)
			abilityColumn.AbilityChanged -= scoreChanged;
		
		if(abilityRow is not null)
			abilityRow.AbilityChanged -= scoreChanged;
		
		base._ExitTree();
	}
	
	public override void _Ready()
	{
		proficiency = GetNode<StatefulButton>(NodePaths.Proficiency);
		proficiency.StateChanged += proficiencyUpdated;
		
		label = GetNode<Label>(NodePaths.Label);
		value = GetNode<Label>(NodePaths.Value);
		
		update();
	}
	
	public void SetProficiency(Proficiency newProficiency)
	{
		proficiency.CurrentState = newProficiency.ToStatefulButtonState();
		proficiency.UpdateTexture();
		update();
	}
	
	public void TrackAbility(AbilityColumn ability)
	{
		abilityColumn = ability;
		abilityColumn.AbilityChanged += scoreChanged;
	}
	
	public void TrackAbility(AbilityRow ability)
	{
		abilityRow = ability;
		abilityRow.AbilityChanged += scoreChanged;
	}
	
	private void proficiencyUpdated(StatefulButton button)
	{
		update();
		EmitSignal(SignalName.ProficiencyChanged, (int)button.CurrentState);
	}
	
	private void scoreChanged(Transport<AbilityInfo> transport)
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
