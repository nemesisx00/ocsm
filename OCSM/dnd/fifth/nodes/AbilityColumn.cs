using Godot;
using Ocsm.Nodes;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class AbilityColumn : Container
{
	[Signal]
	public delegate void AbilityChangedEventHandler(Transport<AbilityInfo> transport);
	
	private static class NodePaths
	{
		public static readonly NodePath Name = new("%Name");
		public static readonly NodePath Score = new("%Score");
		public static readonly NodePath Modifier = new("%Modifier");
		public static readonly NodePath SavingThrow = new("%SavingThrow");
		public static readonly NodePath Skills = new("%Skills");
	}
	
	public AbilityInfo Ability { get; set; }
	public int ProficiencyBonus { get; set; } = 2;
	
	private Label label;
	private SpinBox score;
	private SpinBox modifier;
	private Container skillsContainer;
	private SkillNode savingThrow;
	
	public override void _ExitTree()
	{
		score.ValueChanged -= scoreChanged;
		
		base._ExitTree();
	}
	
	public override void _Ready()
	{
		label = GetNode<Label>(NodePaths.Name);
		score = GetNode<SpinBox>(NodePaths.Score);
		modifier = GetNode<SpinBox>(NodePaths.Modifier);
		skillsContainer = GetNode<Container>(NodePaths.Skills);
		savingThrow = GetNode<SkillNode>(NodePaths.SavingThrow);
		savingThrow.TrackAbility(this);
		savingThrow.ProficiencyChanged += savingThrowChanged;
		
		score.ValueChanged += scoreChanged;
	}
	
	public void Refresh()
	{
		if(Ability is not null)
		{
			label.Text = Ability.AbilityType.GetLabel();
			score.Value = Ability.TotalScore;
			modifier.Value = Ability.Modifier;
			savingThrow.SetProficiency(Ability.SavingThrow);
			renderSkills();
		}
	}
	
	private void calculateModifier()
	{
		modifier.Value = Ability.Modifier;
		
		if(modifier.Value >= 0)
			modifier.Prefix = "+";
		else
			modifier.Prefix = string.Empty;
	}
	
	private void renderSkills()
	{
		foreach(Node child in skillsContainer.GetChildren())
		{
			child.QueueFree();
		}
		
		if(Ability is not null)
		{
			var resource = GD.Load<PackedScene>(ResourcePaths.Fifth.Skill);
			Ability.Skills.ForEach(s => instantiateSkill(s, resource));
		}
	}
	
	private void instantiateSkill(Ocsm.Dnd.Fifth.Skill skill, PackedScene resource)
	{
		var instance = resource.Instantiate<SkillNode>();
		instance.AbilityModifier = Ability.Modifier;
		instance.ProficiencyBonus = ProficiencyBonus;
		instance.Label = skill.SkillType.GetLabel();
		instance.Name = skill.SkillType.GetLabel();
		instance.TrackAbility(this);
		instance.ProficiencyChanged += (currentState) => proficiencyChanged(currentState, skill);
		skillsContainer.AddChild(instance);
		instance.SetProficiency(skill.Proficient);
	}
	
	private void proficiencyChanged(StatefulButton.States currentState, Ocsm.Dnd.Fifth.Skill boundSkill)
	{
		var proficiency = currentState.ToProficiency();
		boundSkill.Proficient = proficiency;
		if(Ability.Skills.Find(s => s.SkillType == boundSkill.SkillType) is Ocsm.Dnd.Fifth.Skill skill)
			skill.Proficient = proficiency;
		EmitSignal(SignalName.AbilityChanged, new Transport<AbilityInfo>(Ability));
	}
	
	private void savingThrowChanged(StatefulButton.States currentState)
	{
		Ability.SavingThrow = currentState.ToProficiency();
		EmitSignal(SignalName.AbilityChanged, new Transport<AbilityInfo>(Ability));
	}
	
	private void scoreChanged(double value)
	{
		if(Ability.Score != value - Ability.BonusTotal)
			Ability.Score = (int)value;
		
		calculateModifier();
		EmitSignal(SignalName.AbilityChanged, new Transport<AbilityInfo>(Ability));
	}
}
