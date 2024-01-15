using Godot;
using Ocsm.Nodes;
using System;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class AbilityColumn : Container
{
	[Signal]
	public delegate void AbilityChangedEventHandler(Transport<Ability> transport);
	
	private sealed class NodePaths
	{
		public static readonly NodePath Name = new("%Name");
		public static readonly NodePath Score = new("%Score");
		public static readonly NodePath Modifier = new("%Modifier");
		public static readonly NodePath SavingThrow = new("%SavingThrow");
		public static readonly NodePath Skills = new("%Skills");
	}
	
	private const string Plus = "+";
	
	public Ability Ability { get; set; }
	public int ProficiencyBonus { get; set; } = 2;
	
	private Label label;
	private SpinBox score;
	private SpinBox modifier;
	private Container skillsContainer;
	private Skill savingThrow;
	
	public override void _Ready()
	{
		label = GetNode<Label>(NodePaths.Name);
		score = GetNode<SpinBox>(NodePaths.Score);
		modifier = GetNode<SpinBox>(NodePaths.Modifier);
		skillsContainer = GetNode<Container>(NodePaths.Skills);
		savingThrow = GetNode<Skill>(NodePaths.SavingThrow);
		savingThrow.TrackAbility(this);
		savingThrow.ProficiencyChanged += savingThrowChanged;
		
		score.ValueChanged += scoreChanged;
	}
	
	public void Refresh()
	{
		if(Ability is not null)
		{
			label.Text = Ability.Name;
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
			modifier.Prefix = Plus;
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
			Ability.Skills.ForEach(s => instantiateSkill(s, resource));
		}
	}
	
	private void instantiateSkill(Fifth.Skill skill, PackedScene resource)
	{
		var instance = resource.Instantiate<Skill>();
		instance.AbilityModifier = Ability.Modifier;
		instance.ProficiencyBonus = ProficiencyBonus;
		instance.Label = skill.Name;
		instance.Name = skill.Name;
		instance.TrackAbility(this);
		instance.ProficiencyChanged += (currentState) => proficiencyChanged(currentState, skill);
		skillsContainer.AddChild(instance);
		instance.SetProficiency(skill.Proficient);
	}
	
	private void proficiencyChanged(int currentState, Fifth.Skill boundSkill)
	{
		var proficiency = ProficiencyUtility.FromStatefulButtonState((StatefulButton.States)currentState);
		boundSkill.Proficient = proficiency;
		
		if(Ability.Skills.Find(s => s.Name.Equals(boundSkill.Name)) is Fifth.Skill skill)
			skill.Proficient = proficiency;
		
		_ = EmitSignal(SignalName.AbilityChanged, new Transport<Ability>(Ability));
	}
	
	private void savingThrowChanged(int currentState)
	{
		Ability.SavingThrow = ProficiencyUtility.FromStatefulButtonState((StatefulButton.States)currentState);
		_ = EmitSignal(SignalName.AbilityChanged, new Transport<Ability>(Ability));
	}
	
	private void scoreChanged(double value)
	{
		Ability.Score = (int)value;
		calculateModifier();
		_ = EmitSignal(SignalName.AbilityChanged, new Transport<Ability>(Ability));
	}
}
