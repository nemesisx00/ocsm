using Godot;
using System;
using System.Collections.Generic;
using OCSM;

public class MortalSheetLogic : CoreSheetLogic
{
	private const string AdvantagesPath = "Column/Row/Advantages/";
	private const string TabContainerPath = "Column/TabContainer/";
	private const string DetailsPath = TabContainerPath + "Details/";
	private const string TraitsPath = TabContainerPath + "Traits/";
	private const string AttributesPath = TraitsPath + "Attributes/";
	private const string SkillsPath = TraitsPath + "Skills/";
	private const string MeritsPath = TabContainerPath + "Merits/";
	private const string InventoryPath = TabContainerPath + "Inventory/";
	private const string GameNotesPath = TabContainerPath + "Game Notes/";
	
	private sealed class Advantage
	{
		public const string Armor = "Armor";
		public const string Beats = "Beats";
		public const string Defense = "Defense";
		public const string Conditions = "Conditions";
		public const string Experience = "Experience";
		public const string Health = "Health";
		public const string Initiative = "Initiative";
		public const string Integrity = "Integrity";
		public const string Speed = "Speed";
		public const string Vice = "Vice";
		public const string Virtue = "Virtue";
		public const string Willpower = "Willpower";
	}
	
	private sealed class Detail
	{
		public const string Age = "Age";
		public const string Aspirations = "Aspirations";
		public const string Chronicle = "Chronicle";
		public const string Concept = "Concept";
		public const string Faction = "Faction";
		public const string GroupName = "GroupName";
		public const string Name = "Name";
		public const string Player = "Player";
		public const string Vice = "Vice";
		public const string Virtue = "Virtue";
		public const string Size = "Size";
	}
	
	private const string Merits = "Merits";
	private const string SkillSpecialties = "Specialties";
	
	private Mortal sheetData { get; set; } = new Mortal();
	
    public override void _Ready()
    {
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Age, DetailsPath)), sheetData.Age > -1 ? sheetData.Age.ToString() : String.Empty, nameof(changed_Age));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Chronicle, DetailsPath)), sheetData.Chronicle, nameof(changed_Chronicle));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Concept, DetailsPath)), sheetData.Concept, nameof(changed_Concept));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Faction, DetailsPath)), sheetData.Faction, nameof(changed_Faction));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.GroupName, DetailsPath)), sheetData.GroupName, nameof(changed_GroupName));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Name, DetailsPath)), sheetData.Name, nameof(changed_Name));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Player, DetailsPath)), sheetData.Player, nameof(changed_Player));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Vice, DetailsPath)), sheetData.Vice, nameof(changed_Vice));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Virtue, DetailsPath)), sheetData.Virtue, nameof(changed_Virtue));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Size, DetailsPath)), sheetData.Size.ToString(), nameof(changed_Size));
		InitAndConnect(GetNode<ItemList>(PathBuilder.SceneUnique(Detail.Aspirations, DetailsPath)), sheetData.Aspirations, nameof(changed_Aspirations));
		
		InitAndConnect(GetNode<TrackSimple>(PathBuilder.SceneUnique(Advantage.Beats, AdvantagesPath)), sheetData.Beats, nameof(changed_Beats));
		InitAndConnect(GetNode<ItemList>(PathBuilder.SceneUnique(Advantage.Conditions, AdvantagesPath)), sheetData.Conditions, nameof(changed_Conditions));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Advantage.Experience, AdvantagesPath)), sheetData.Experience, nameof(changed_Experience));
		updateMaxHealth();
		InitAndConnect(GetNode<TrackComplex>(PathBuilder.SceneUnique(Advantage.Health, AdvantagesPath)), sheetData.HealthCurrent, nameof(changed_Health));
		updateMaxWillpower();
		InitAndConnect(GetNode<TrackSimple>(PathBuilder.SceneUnique(Advantage.Willpower, AdvantagesPath)), sheetData.WillpowerSpent.ToString(), nameof(changed_Willpower));
		InitAndConnect(GetNode<TrackSimple>(PathBuilder.SceneUnique(Advantage.Integrity, AdvantagesPath)), sheetData.Integrity.ToString(), nameof(changed_Integrity));
		GetNode<Label>(PathBuilder.SceneUnique(Advantage.Vice, AdvantagesPath)).Text = sheetData.Vice;
		GetNode<Label>(PathBuilder.SceneUnique(Advantage.Virtue, AdvantagesPath)).Text = sheetData.Virtue;
		
		foreach(var a in OCSM.Attribute.toList())
		{
			InitAndConnect(GetNode<TrackSimple>(PathBuilder.SceneUnique(a.Name, AttributesPath)), sheetData.Attributes[a], nameof(changed_Attribute), true);
		}
		
		foreach(var s in OCSM.Skill.toList())
		{
			InitAndConnect(GetNode<TrackSimple>(PathBuilder.SceneUnique(s.Name, SkillsPath)), sheetData.Skills[s], nameof(changed_Skill), true);
		}
		InitAndConnect(GetNode<SpecialtyList>(PathBuilder.SceneUnique(SkillSpecialties, SkillsPath)), sheetData.Specialties, nameof(changed_SkillSpecialty));
		
		InitAndConnect(GetNode<ItemDotsList>(PathBuilder.SceneUnique(Merits, MeritsPath)), sheetData.Merits, nameof(changed_Merits));
		
		updateDefense();
		updateInitiative();
		updateSpeed();
    }
	
	private void changed_Age(string newText)
	{
		int newAge;
		if(int.TryParse(newText, out newAge))
			sheetData.Age = newAge;
		else
			GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Age, DetailsPath)).Text = sheetData.Age.ToString();
	}
	
	private void changed_Chronicle(string newText) { sheetData.Chronicle = newText; }
	private void changed_Concept(string newText) { sheetData.Concept = newText; }
	private void changed_Faction(string newText) { sheetData.Faction = newText; }
	private void changed_GroupName(string newText) { sheetData.GroupName = newText; }
	private void changed_Name(string newText)
	{
		sheetData.Name = newText;
		Name = sheetData.Name;
	}
	private void changed_Player(string newText) { sheetData.Player = newText; }
	
	private void changed_Vice(string newText)
	{
		sheetData.Vice = newText;
		GetNode<Label>(PathBuilder.SceneUnique(Advantage.Vice, AdvantagesPath)).Text = sheetData.Vice;
	}
	
	private void changed_Virtue(string newText)
	{
		sheetData.Virtue = newText;
		GetNode<Label>(PathBuilder.SceneUnique(Advantage.Virtue, AdvantagesPath)).Text = sheetData.Virtue;
	}
	
	private void changed_Size(string newText)
	{
		int newSize;
		if(int.TryParse(newText, out newSize))
		{
			sheetData.Size = newSize;
			updateMaxHealth();
			updateSpeed();
		}
		else
			GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Size, DetailsPath)).Text = sheetData.Size.ToString();
	}
	
	private void changed_Aspirations(List<string> values)
	{
		sheetData.Aspirations = values;
	}
	
	private void changed_Beats(int value)
	{
		if(value >= 5)
		{
			sheetData.Beats = 0;
			GetNode<TrackSimple>(PathBuilder.SceneUnique(Advantage.Beats, AdvantagesPath)).updateValue(sheetData.Beats);
			sheetData.Experience++;
			GetNode<LineEdit>(PathBuilder.SceneUnique(Advantage.Experience, AdvantagesPath)).Text = sheetData.Experience.ToString();
		}
		else
			sheetData.Beats = value;
		GD.Print(sheetData);
	}
	
	private void changed_Conditions(List<string> values)
	{
		sheetData.Conditions = values;
	}
	
	private void changed_Experience(string newText)
	{
		int newXp;
		if(int.TryParse(newText, out newXp))
			sheetData.Experience = newXp;
		else
			GetNode<LineEdit>(PathBuilder.SceneUnique(Advantage.Experience, AdvantagesPath)).Text = sheetData.Experience.ToString();
	}
	
	private void changed_Health(Dictionary<string, int> values) { sheetData.HealthCurrent = values; }
	private void changed_Integrity(int value) { sheetData.Integrity = value; }
	private void changed_Willpower(int value) { sheetData.WillpowerSpent = value; }
	
	private void changed_Attribute(TrackSimple node)
	{
		var attr = OCSM.Attribute.byName(node.Name);
		if(attr != null)
		{
			sheetData.Attributes[attr] = node.Value;
			switch(node.Name)
			{
				case OCSM.Attribute.Names.Composure:
					updateMaxWillpower();
					updateInitiative();
					break;
				case OCSM.Attribute.Names.Dexterity:
					updateDefense();
					updateInitiative();
					updateSpeed();
					break;
				case OCSM.Attribute.Names.Resolve:
					updateMaxWillpower();
					break;
				case OCSM.Attribute.Names.Stamina:
					updateMaxHealth();
					break;
				case OCSM.Attribute.Names.Strength:
					updateSpeed();
					break;
				case OCSM.Attribute.Names.Wits:
					updateDefense();
					break;
			}
		}
	}
	
	private void changed_Skill(TrackSimple node)
	{
		var skill = OCSM.Skill.byName(node.Name);
		if(skill != null)
		{
			sheetData.Skills[skill] = node.Value;
			switch(node.Name)
			{
				case OCSM.Skill.Names.Athletics:
					updateDefense();
					break;
			}
		}
	}
	
	private void changed_SkillSpecialty(List<Skill.Specialty> values)
	{
		sheetData.Specialties = values;
	}
	
	private void changed_Merits(List<TextValueItem> values) { sheetData.Merits = values; }
	
	private void updateDefense()
	{
		if(sheetData.Attributes[OCSM.Attribute.Dexterity] < sheetData.Attributes[OCSM.Attribute.Wits])
			GetNode<Label>(PathBuilder.SceneUnique(Advantage.Defense, AdvantagesPath)).Text = (sheetData.Attributes[OCSM.Attribute.Dexterity] + sheetData.Skills[OCSM.Skill.Athletics]).ToString();
		else
			GetNode<Label>(PathBuilder.SceneUnique(Advantage.Defense, AdvantagesPath)).Text = (sheetData.Attributes[OCSM.Attribute.Wits] + sheetData.Skills[OCSM.Skill.Athletics]).ToString();
	}
	
	private void updateInitiative()
	{
		GetNode<Label>(PathBuilder.SceneUnique(Advantage.Initiative, AdvantagesPath)).Text = (sheetData.Attributes[OCSM.Attribute.Dexterity] + sheetData.Attributes[OCSM.Attribute.Composure]).ToString();
	}
	
	private void updateMaxHealth()
	{
		sheetData.HealthMax = sheetData.Size + sheetData.Attributes[OCSM.Attribute.Stamina];
		GetNode<TrackComplex>(PathBuilder.SceneUnique(Advantage.Health, AdvantagesPath)).updateMax(sheetData.HealthMax);
	}
	
	private void updateMaxWillpower()
	{
		sheetData.WillpowerMax = sheetData.Attributes[OCSM.Attribute.Composure] + sheetData.Attributes[OCSM.Attribute.Resolve];
		GetNode<TrackSimple>(PathBuilder.SceneUnique(Advantage.Willpower, AdvantagesPath)).updateMax(sheetData.WillpowerMax);
	}
	
	private void updateSpeed()
	{
		GetNode<Label>(PathBuilder.SceneUnique(Advantage.Speed, AdvantagesPath)).Text = (sheetData.Attributes[OCSM.Attribute.Strength] + sheetData.Attributes[OCSM.Attribute.Dexterity] + sheetData.Size).ToString();
	}
}
