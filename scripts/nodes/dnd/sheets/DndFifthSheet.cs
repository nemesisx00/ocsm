using Godot;
using System;
using System.Text.Json;
using OCSM.DnD.Fifth;
using OCSM.Nodes.Autoload;
using OCSM.Nodes.Sheets;
using OCSM.DnD.Fifth.Meta;
using OCSM.Nodes.DnD.Fifth;

namespace OCSM.Nodes.DnD.Sheets
{
	public class DndFifthSheet : CharacterSheet<FifthAdventurer>
	{
		private sealed class Names
		{
			public const string Alignment = "Alignment";
			public const string Background = "Background";
			public const string BardicInspiration = "BardicInspiration";
			public const string Bonds = "Bonds";
			public const string CharacterName = "CharacterName";
			public const string Flaws = "Flaws";
			public const string Ideals = "Ideals";
			public const string Inspiration = "Inspiration";
			public const string PersonalityTraits = "PersonalityTraits";
			public const string PlayerName = "PlayerName";
			public const string Race = "Race";
		}
		
		private MetadataManager metadataManager;
		
		public override void _Ready()
		{
			metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
			
			if(!(SheetData is FifthAdventurer))
				SheetData = new FifthAdventurer();
			
			InitAndConnect(GetNode<LineEdit>(NodePathBuilder.SceneUnique(Names.CharacterName)), SheetData.Name, nameof(changed_CharacterName));
			InitAndConnect(GetNode<LineEdit>(NodePathBuilder.SceneUnique(Names.PlayerName)), SheetData.Player, nameof(changed_PlayerName));
			InitAndConnect(GetNode<LineEdit>(NodePathBuilder.SceneUnique(Names.Alignment)), SheetData.Alignment, nameof(changed_Alignment));
			InitAndConnect(GetNode<RaceOptionsButton>(NodePathBuilder.SceneUnique(Names.Race)), SheetData.Race, nameof(changed_Race));
			InitAndConnect(GetNode<BackgroundOptionsButton>(NodePathBuilder.SceneUnique(Names.Background)), SheetData.Background, nameof(changed_Background));
			
			InitAndConnect(GetNode<ToggleButton>(NodePathBuilder.SceneUnique(Names.Inspiration)), SheetData.Inspiration, nameof(changed_Inspiration));
			InitAndConnect(GetNode<ToggleButton>(NodePathBuilder.SceneUnique(Names.BardicInspiration)), SheetData.BardicInspiration, nameof(changed_BardicInspiration));
			
			InitAndConnect(GetNode<AutosizeTextEdit>(NodePathBuilder.SceneUnique(Names.PersonalityTraits)), SheetData.PersonalityTraits, nameof(changed_PersonalityTraits));
			InitAndConnect(GetNode<AutosizeTextEdit>(NodePathBuilder.SceneUnique(Names.Ideals)), SheetData.Ideals, nameof(changed_Ideals));
			InitAndConnect(GetNode<AutosizeTextEdit>(NodePathBuilder.SceneUnique(Names.Bonds)), SheetData.Bonds, nameof(changed_Bonds));
			InitAndConnect(GetNode<AutosizeTextEdit>(NodePathBuilder.SceneUnique(Names.Flaws)), SheetData.Flaws, nameof(changed_Flaws));
			
			foreach(var score in SheetData.AbilityScores)
			{
				InitAndConnect(GetNode<Ability>(NodePathBuilder.SceneUnique(score.Name)), score, nameof(changed_AbilityScore));
			}
			
			base._Ready();
		}
		
		protected new void InitAndConnect<T1, T2>(T1 node, T2 initialValue, string handlerName, bool nodeChanged = false)
			where T1: Control
		{
			if(node is Ability a)
			{
				if(initialValue is AbilityScore abilityScore)
					a.Score = abilityScore.Score;
				
				//This got more complex than I had originally intended but ç'est la vie...
				if(SheetData.SavingThrows.Find(st => st.Name.Equals(a.AbilityName)) is SavingThrow savingThrow)
					a.setSavingThrowProficiency(savingThrow.Proficient);
				
				foreach(var s in a.Skills)
				{
					if(SheetData.Skills.Find(ss => ss.Name.Equals(s.Name)) is OCSM.DnD.Fifth.Skill skill)
						s.Proficient = skill.Proficient;
				}
				a.renderSkills();
				
				a.Connect(nameof(Ability.SavingThrowProficiencyChanged), this, nameof(changed_SavingThrowProficiency));
				a.Connect(nameof(Ability.SkillProficienyChanged), this, nameof(changed_SkillProficiency));
				a.Connect(nameof(Ability.NodeChanged), this, handlerName);
			}
			else if(node is BackgroundOptionsButton bob)
			{
				if(initialValue is Background background && metadataManager.Container is DnDFifthContainer dfc)
				{
					var index = dfc.Backgrounds.FindIndex(b => b.Name.Equals(background.Name)) + 1;
					if(index > 0)
						bob.Selected = index;
				}
				bob.Connect(Constants.Signal.ItemSelected, this, handlerName);
			}
			else if(node is ClassOptionsButton cob)
			{
				if(initialValue is Class clazz && metadataManager.Container is DnDFifthContainer dfc)
				{
					var index = dfc.Classes.FindIndex(c => c.Name.Equals(clazz.Name)) + 1;
					if(index > 0)
						cob.Selected = index;
				}
				cob.Connect(Constants.Signal.ItemSelected, this, handlerName);
			}
			else if(node is RaceOptionsButton rob)
			{
				if(initialValue is Race race && metadataManager.Container is DnDFifthContainer dfc)
				{
					var index = dfc.Races.FindIndex(r => r.Name.Equals(race.Name)) + 1;
					if(index > 0)
						rob.Selected = index;
				}
				rob.Connect(Constants.Signal.ItemSelected, this, handlerName);
			}
			else
				base.InitAndConnect(node, initialValue, handlerName);
		}
		
		private void changed_AbilityScore(Ability node)
		{
			if(SheetData.AbilityScores.Find(a => a.Name.Equals(node.AbilityName)) is AbilityScore abilityScore)
				abilityScore.Score = node.Score;
		}
		
		private void changed_Alignment(string newText) { SheetData.Alignment = newText; }
		
		private void changed_Background(int index)
		{
			if(index > 0 && metadataManager.Container is DnDFifthContainer dfc && dfc.Backgrounds[index - 1] is Background background)
				SheetData.Background = background;
			else
				SheetData.Background = null;
		}
		
		private void changed_BardicInspiration(ToggleButton button) { SheetData.BardicInspiration = button.CurrentState; }
		
		private void changed_Bonds()
		{
			var text = GetNode<AutosizeTextEdit>(NodePathBuilder.SceneUnique(Names.Bonds)).Text;
			SheetData.Bonds = text;
		}
		
		private void changed_CharacterName(string newText)
		{
			SheetData.Name = newText;
			if(!String.IsNullOrEmpty(SheetData.Name))
				Name = SheetData.Name;
		}
		
		private void changed_Flaws()
		{
			var text = GetNode<AutosizeTextEdit>(NodePathBuilder.SceneUnique(Names.Flaws)).Text;
			SheetData.Flaws = text;
		}
		
		private void changed_Ideals()
		{
			var text = GetNode<AutosizeTextEdit>(NodePathBuilder.SceneUnique(Names.Ideals)).Text;
			SheetData.Ideals = text;
		}
		
		private void changed_Inspiration(ToggleButton button) { SheetData.Inspiration = button.CurrentState; }
		
		private void changed_PersonalityTraits()
		{
			var text = GetNode<AutosizeTextEdit>(NodePathBuilder.SceneUnique(Names.PersonalityTraits)).Text;
			SheetData.PersonalityTraits = text;
		}
		
		private void changed_PlayerName(string newText) { SheetData.Player = newText; }
		
		private void changed_SavingThrowProficiency(string abilityName, Proficiency proficiency)
		{
			if(SheetData.SavingThrows.Find(st => st.Name.Equals(abilityName)) is SavingThrow savingThrow)
			{
				savingThrow.Proficient = proficiency;
			}
		}
		
		private void changed_SkillProficiency(Transport<OCSM.DnD.Fifth.Skill> transport)
		{
			if(SheetData.Skills.Find(s => s.Name.Equals(transport.Value.Name)) is OCSM.DnD.Fifth.Skill skill)
			{
				skill.Proficient = transport.Value.Proficient;
			}
		}
		
		private void changed_Race(int index)
		{
			if(index > 0 && metadataManager.Container is DnDFifthContainer dfc && dfc.Races[index - 1] is Race race)
				SheetData.Race = race;
			else
				SheetData.Race = null;
		}
	}
}
