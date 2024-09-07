using Godot;
using Ocsm.Nodes;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class AbilityRow : Container
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
	
	private Label name;
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
		name = GetNode<Label>(NodePaths.Name);
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
			name.Text = Ability.AbilityType.GetLabel();
			score.Value = Ability.Score;
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
			child.QueueFree();
		
		if(Ability is not null)
		{
			var resource = GD.Load<PackedScene>(ScenePaths.Dnd.Fifth.Skill);
			
			foreach(var s in Ability.Skills)
				instantiateSkill(s, resource);
		}
	}
	
	private void instantiateSkill(Skill skill, PackedScene resource)
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
	
	private void proficiencyChanged(StatefulButton.States currentState, Skill boundSkill)
	{
		var proficiency = currentState.ToProficiency();
		boundSkill.Proficient = proficiency;
		if(Ability.Skills.Find(s => s.SkillType == boundSkill.SkillType) is Skill skill)
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
		if(Ability.RawScore != value - Ability.BonusTotal)
			Ability.Score = (int)value;
		
		calculateModifier();
		EmitSignal(SignalName.AbilityChanged, new Transport<AbilityInfo>(Ability));
	}
}
