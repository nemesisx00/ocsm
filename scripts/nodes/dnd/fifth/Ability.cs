using Godot;
using System;
using System.Collections.Generic;
using OCSM.DnD.Fifth;

namespace OCSM.Nodes.DnD.Fifth
{
	public class Ability : Container
	{
		[Signal]
		public delegate void ScoreChanged(string abilityName, int score, int modifier);
		[Signal]
		public delegate void NodeChanged(Ability node);
		[Signal]
		public delegate void SavingThrowProficiencyChanged(string abilityName, Proficiency proficiency);
		[Signal]
		public delegate void SkillProficienyChanged(Transport<OCSM.DnD.Fifth.Skill> transport);
		
		private sealed class Names
		{
			public const string Name = "Name";
			public const string Score = "Score";
			public const string Modifier = "Modifier";
			public const string SavingThrow = "SavingThrow";
			public const string Skills = "Skills";
		}
		
		[Export(PropertyHint.Enum, AbilityScore.Names.EnumHint)]
		public string AbilityName { get; set; } = String.Empty;
		
		public int Score
		{
			get { return (int)score.Value; }
			set { score.Value = value; }
		}
		
		public int Modifier { get { return (int)(Score / 2) - 5; } }
		
		public List<OCSM.DnD.Fifth.Skill> Skills { get; set; }
		public int ProficiencyBonus { get; set; } = 2;
		
		private Label label;
		private SpinBox score;
		private SpinBox modifier;
		private Container skillsContainer;
		private Skill savingThrow;
		
		public override void _Ready()
		{
			label = GetNode<Label>(NodePathBuilder.SceneUnique(Names.Name));
			score = GetNode<SpinBox>(NodePathBuilder.SceneUnique(Names.Score));
			modifier = GetNode<SpinBox>(NodePathBuilder.SceneUnique(Names.Modifier));
			skillsContainer = GetNode<Container>(NodePathBuilder.SceneUnique(Names.Skills));
			savingThrow = GetNode<Skill>(NodePathBuilder.SceneUnique(Names.SavingThrow));
			savingThrow.trackAbility(this);
			savingThrow.Connect(nameof(Skill.ProficiencyChanged), this, nameof(savingThrowChanged));
			
			label.Text = AbilityName;
			score.Connect(Constants.Signal.ValueChanged, this, nameof(scoreChanged));
			
			if(!(Skills is List<OCSM.DnD.Fifth.Skill>) || Skills.Count < 1)
			{
				switch(AbilityName)
				{
					case AbilityScore.Names.Charisma:
						Skills = OCSM.DnD.Fifth.Skill.listCharisma();
						break;
					case AbilityScore.Names.Dexterity:
						Skills = OCSM.DnD.Fifth.Skill.listDexterity();
						break;
					case AbilityScore.Names.Intelligence:
						Skills = OCSM.DnD.Fifth.Skill.listIntelligence();
						break;
					case AbilityScore.Names.Strength:
						Skills = OCSM.DnD.Fifth.Skill.listStrength();
						break;
					case AbilityScore.Names.Wisdom:
						Skills = OCSM.DnD.Fifth.Skill.listWisdom();
						break;
					default:
						Skills = new List<OCSM.DnD.Fifth.Skill>();
						break;
				}
			}
			
			renderSkills();
		}
		
		private void calculateModifier()
		{
			modifier.Value = Modifier;
			if(modifier.Value >= 0)
				modifier.Prefix = "+";
			else
				modifier.Prefix = String.Empty;
		}
		
		public void renderSkills()
		{
			foreach(Node child in skillsContainer.GetChildren())
			{
				child.QueueFree();
			}
			
			var resource = ResourceLoader.Load<PackedScene>(Constants.Scene.DnD.Fifth.Skill);
			foreach(var skill in Skills)
			{
				var instance = resource.Instance<Skill>();
				instance.AbilityModifier = Modifier;
				instance.ProficiencyBonus = ProficiencyBonus;
				instance.Label = skill.Name;
				instance.Name = skill.Name;
				instance.trackAbility(this);
				instance.Connect(nameof(Skill.ProficiencyChanged), this, nameof(proficiencyChanged), new Godot.Collections.Array(new Transport<OCSM.DnD.Fifth.Skill>(skill)));
				skillsContainer.AddChild(instance);
				instance.setProficiency(skill.Proficient);
			}
		}
		
		public void setSavingThrowProficiency(Proficiency proficiency)
		{
			savingThrow.setProficiency(proficiency);
		}
		
		private void proficiencyChanged(string currentState, Transport<OCSM.DnD.Fifth.Skill> transport)
		{
			var proficiency = ProficiencyUtility.fromStatefulButtonState(currentState);
			transport.Value.Proficient = proficiency;
			if(Skills.Find(s => s.Name.Equals(transport.Value.Name)) is OCSM.DnD.Fifth.Skill skill)
				skill.Proficient = proficiency;
			EmitSignal(nameof(SkillProficienyChanged), transport);
		}
		
		private void savingThrowChanged(string currentState)
		{
			EmitSignal(nameof(SavingThrowProficiencyChanged), AbilityName, ProficiencyUtility.fromStatefulButtonState(currentState));
		}
		
		private void scoreChanged(float value)
		{
			calculateModifier();
			EmitSignal(nameof(ScoreChanged), AbilityName, Score, Modifier);
			EmitSignal(nameof(NodeChanged), this);
		}
	}
}
