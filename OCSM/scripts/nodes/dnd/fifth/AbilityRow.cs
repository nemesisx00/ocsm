using Godot;
using System;
using System.Collections.Generic;
using Ocsm.Dnd.Fifth;

namespace Ocsm.Nodes.Dnd.Fifth;

public partial class AbilityRow : Container
{
	[Signal]
	public delegate void AbilityChangedEventHandler(Transport<Ability> transport);
	
	private sealed class NodePath
	{
		public const string Name = "%Name";
		public const string Score = "%Score";
		public const string Modifier = "%Modifier";
		public const string SavingThrow = "%SavingThrow";
		public const string Skills = "%Skills";
	}
	
	public Ability Ability { get; set; }
	public int ProficiencyBonus { get; set; } = 2;
	
	private Label name;
	private SpinBox score;
	private SpinBox modifier;
	private Container skillsContainer;
	private Skill savingThrow;
	
	public override void _Ready()
	{
		name = GetNode<Label>(NodePath.Name);
		score = GetNode<SpinBox>(NodePath.Score);
		modifier = GetNode<SpinBox>(NodePath.Modifier);
		skillsContainer = GetNode<Container>(NodePath.Skills);
		savingThrow = GetNode<Skill>(NodePath.SavingThrow);
		savingThrow.trackAbility(this);
		savingThrow.ProficiencyChanged += savingThrowChanged;
		
		score.ValueChanged += scoreChanged;
	}
	
	public void refresh()
	{
		if(Ability is Ability)
		{
			name.Text = Ability.Name;
			score.Value = Ability.Score;
			modifier.Value = Ability.Modifier;
			savingThrow.setProficiency(Ability.SavingThrow);
			renderSkills();
		}
	}
	
	private void calculateModifier()
	{
		modifier.Value = Ability.Modifier;
		if(modifier.Value >= 0)
			modifier.Prefix = "+";
		else
			modifier.Prefix = String.Empty;
	}
	
	private void renderSkills()
	{
		foreach(Node child in skillsContainer.GetChildren())
		{
			child.QueueFree();
		}
		
		if(Ability is Ability)
		{
			var resource = GD.Load<PackedScene>(Constants.Scene.Dnd.Fifth.Skill);
			Ability.Skills.ForEach(s => instantiateSkill(s, resource));
		}
	}
	
	private void instantiateSkill(Ocsm.Dnd.Fifth.Skill skill, PackedScene resource)
	{
		var instance = resource.Instantiate<Skill>();
		instance.AbilityModifier = Ability.Modifier;
		instance.ProficiencyBonus = ProficiencyBonus;
		instance.Label = skill.Name;
		instance.Name = skill.Name;
		instance.trackAbility(this);
		instance.ProficiencyChanged += (currentState) => proficiencyChanged(currentState, skill);
		skillsContainer.AddChild(instance);
		instance.setProficiency(skill.Proficient);
	}
	
	private void proficiencyChanged(string currentState, Ocsm.Dnd.Fifth.Skill boundSkill)
	{
		var proficiency = ProficiencyUtility.fromStatefulButtonState(currentState);
		boundSkill.Proficient = proficiency;
		if(Ability.Skills.Find(s => s.Name.Equals(boundSkill.Name)) is Ocsm.Dnd.Fifth.Skill skill)
			skill.Proficient = proficiency;
		EmitSignal(nameof(AbilityChanged), new Transport<Ability>(Ability));
	}
	
	private void savingThrowChanged(string currentState)
	{
		Ability.SavingThrow = ProficiencyUtility.fromStatefulButtonState(currentState);
		EmitSignal(nameof(AbilityChanged), new Transport<Ability>(Ability));
	}
	
	private void scoreChanged(double value)
	{
		Ability.Score = (int)value;
		calculateModifier();
		EmitSignal(nameof(AbilityChanged), new Transport<Ability>(Ability));
	}
}
