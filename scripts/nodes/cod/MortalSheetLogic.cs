using Godot;
using System;
using System.Collections.Generic;
using OCSM;

public class MortalSheetLogic : ScrollContainer
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
	
	private Mortal sheetData { get; set; } = new Mortal();
	
    public override void _Ready()
    {
		InitAndConnect(GetNode<TextEdit>(PathBuilder.SceneUnique(Detail.Age, DetailsPath)), sheetData.Age > -1 ? sheetData.Age.ToString() : String.Empty, nameof(changed_Age));
		InitAndConnect(GetNode<TextEdit>(PathBuilder.SceneUnique(Detail.Chronicle, DetailsPath)), sheetData.Chronicle, nameof(changed_Chronicle));
		InitAndConnect(GetNode<TextEdit>(PathBuilder.SceneUnique(Detail.Concept, DetailsPath)), sheetData.Concept, nameof(changed_Concept));
		InitAndConnect(GetNode<TextEdit>(PathBuilder.SceneUnique(Detail.Faction, DetailsPath)), sheetData.Faction, nameof(changed_Faction));
		InitAndConnect(GetNode<TextEdit>(PathBuilder.SceneUnique(Detail.GroupName, DetailsPath)), sheetData.GroupName, nameof(changed_GroupName));
		InitAndConnect(GetNode<TextEdit>(PathBuilder.SceneUnique(Detail.Name, DetailsPath)), sheetData.Name, nameof(changed_Name));
		InitAndConnect(GetNode<TextEdit>(PathBuilder.SceneUnique(Detail.Player, DetailsPath)), sheetData.Player, nameof(changed_Player));
		InitAndConnect(GetNode<TextEdit>(PathBuilder.SceneUnique(Detail.Vice, DetailsPath)), sheetData.Vice, nameof(changed_Vice));
		InitAndConnect(GetNode<TextEdit>(PathBuilder.SceneUnique(Detail.Virtue, DetailsPath)), sheetData.Virtue, nameof(changed_Virtue));
		InitAndConnect(GetNode<TextEdit>(PathBuilder.SceneUnique(Detail.Size, DetailsPath)), sheetData.Size.ToString(), nameof(changed_Size));
		
		InitAndConnect(GetNode<TrackSimple>(PathBuilder.SceneUnique(Advantage.Beats, AdvantagesPath)), sheetData.Beats, nameof(changed_Beats));
		InitAndConnect(GetNode<TextEdit>(PathBuilder.SceneUnique(Advantage.Experience, AdvantagesPath)), sheetData.Experience, nameof(changed_Experience));
		updateMaxHealth();
		InitAndConnect(GetNode<TrackComplex>(PathBuilder.SceneUnique(Advantage.Health, AdvantagesPath)), sheetData.HealthCurrent, nameof(changed_Health));
		updateMaxWillpower();
		InitAndConnect(GetNode<TrackSimple>(PathBuilder.SceneUnique(Advantage.Willpower, AdvantagesPath)), sheetData.WillpowerSpent.ToString(), nameof(changed_Willpower));
		InitAndConnect(GetNode<TrackSimple>(PathBuilder.SceneUnique(Advantage.Integrity, AdvantagesPath)), sheetData.Integrity.ToString(), nameof(changed_Integrity));
		GetNode<Label>(PathBuilder.SceneUnique(Advantage.Vice, AdvantagesPath)).Text = sheetData.Vice;
		GetNode<Label>(PathBuilder.SceneUnique(Advantage.Virtue, AdvantagesPath)).Text = sheetData.Virtue;
		
		foreach(var a in OCSM.Attribute.toList())
		{
			InitAndConnect(GetNode<TrackSimple>(PathBuilder.SceneUnique(a.Name, AttributesPath)), sheetData.Attributes[a], String.Format("changed_{0}", a.Name));
		}
		
		foreach(var s in OCSM.Skill.toList())
		{
			InitAndConnect(GetNode<TrackSimple>(PathBuilder.SceneUnique(s.Name, SkillsPath)), sheetData.Skills[s], String.Format("changed_{0}", s.Name.Replace(" ", "")));
		}
		
		updateDefense();
		updateInitiative();
		updateSpeed();
    }
	
	private void InitAndConnect<T1, T2>(T1 node, T2 initialValue, string handlerName)
		where T1: Control
	{
		if(node is TextEdit te)
		{
			te.Text = initialValue as string;
			te.Connect(Constants.Signal.TextChanged, this, handlerName);
		}
		else if(node is TrackSimple ts)
		{
			var val = 0;
			if(initialValue is int)
				val = int.Parse(initialValue.ToString());
			if(val > 0)
				ts.updateValue(val);
			ts.Connect(Constants.Signal.ValueChanged, this, handlerName);
		}
		else if(node is TrackComplex tc)
		{
			tc.Values = initialValue as Dictionary<string, int>;
			tc.Connect(Constants.Signal.ValueChanged, this, handlerName);
		}
	}
	
	private void changed_Age()
	{
		var node = GetNode<TextEdit>(PathBuilder.SceneUnique(Detail.Age, DetailsPath));
		var newText = node.Text;
		int newAge;
		if(int.TryParse(newText, out newAge))
			sheetData.Age = newAge;
		else
			node.Text = sheetData.Age.ToString();
	}
	
	private void changed_Chronicle() { sheetData.Chronicle = GetNode<TextEdit>(PathBuilder.SceneUnique(Detail.Chronicle, DetailsPath)).Text; }
	private void changed_Concept() { sheetData.Concept = GetNode<TextEdit>(PathBuilder.SceneUnique(Detail.Concept, DetailsPath)).Text; }
	private void changed_Faction() { sheetData.Faction = GetNode<TextEdit>(PathBuilder.SceneUnique(Detail.Faction, DetailsPath)).Text; }
	private void changed_GroupName() { sheetData.GroupName = GetNode<TextEdit>(PathBuilder.SceneUnique(Detail.GroupName, DetailsPath)).Text; }
	private void changed_Name()
	{
		sheetData.Name = GetNode<TextEdit>(PathBuilder.SceneUnique(Detail.Name, DetailsPath)).Text;
		Name = sheetData.Name;
	}
	private void changed_Player() { sheetData.Player = GetNode<TextEdit>(PathBuilder.SceneUnique(Detail.Player, DetailsPath)).Text; }
	
	private void changed_Vice()
	{
		sheetData.Vice = GetNode<TextEdit>(PathBuilder.SceneUnique(Detail.Vice, DetailsPath)).Text;
		GetNode<Label>(PathBuilder.SceneUnique(Advantage.Vice, AdvantagesPath)).Text = sheetData.Vice;
	}
	
	private void changed_Virtue()
	{
		sheetData.Virtue = GetNode<TextEdit>(PathBuilder.SceneUnique(Detail.Virtue, DetailsPath)).Text;
		GetNode<Label>(PathBuilder.SceneUnique(Advantage.Virtue, AdvantagesPath)).Text = sheetData.Virtue;
	}
	
	private void changed_Size()
	{
		var node = GetNode<TextEdit>(PathBuilder.SceneUnique(Detail.Size, DetailsPath));
		var newText = node.Text;
		int newSize;
		if(int.TryParse(newText, out newSize))
		{
			sheetData.Size = newSize;
			
			updateMaxHealth();
			updateSpeed();
		}
		else
			node.Text = sheetData.Size.ToString();
	}
	
	private void changed_Beats(int value)
	{
		if(value >= 5)
		{
			sheetData.Beats = 0;
			GetNode<TrackSimple>(PathBuilder.SceneUnique(Advantage.Beats, AdvantagesPath)).updateValue(sheetData.Beats);
			sheetData.Experience++;
			GetNode<TextEdit>(PathBuilder.SceneUnique(Advantage.Experience, AdvantagesPath)).Text = sheetData.Experience.ToString();
		}
		else
			sheetData.Beats = value;
	}
	
	private void changed_Experience()
	{
		var node = GetNode<TextEdit>(PathBuilder.SceneUnique(Advantage.Experience, AdvantagesPath));
		var newText = node.Text;
		int newXp;
		if(int.TryParse(newText, out newXp))
			sheetData.Experience = newXp;
		else
			node.Text = sheetData.Experience.ToString();
	}
	
	private void changed_Health(Dictionary<string, int> values) { sheetData.HealthCurrent = values; }
	private void changed_Integrity(int value) { sheetData.Integrity = value; }
	private void changed_Willpower(int value) { sheetData.WillpowerSpent = value; }
	
	private void changed_Composure(int value)
	{
		sheetData.Attributes[OCSM.Attribute.Composure] = value;
		
		updateMaxWillpower();
		updateInitiative();
	}
	
	private void changed_Dexterity(int value)
	{
		sheetData.Attributes[OCSM.Attribute.Dexterity] = value;
		
		updateDefense();
		updateInitiative();
		updateSpeed();
	}
	
	private void changed_Intelligence(int value) { sheetData.Attributes[OCSM.Attribute.Intelligence] = value; }
	private void changed_Manipulation(int value) { sheetData.Attributes[OCSM.Attribute.Manipulation] = value; }
	private void changed_Presence(int value) { sheetData.Attributes[OCSM.Attribute.Presence] = value; }
	
	private void changed_Resolve(int value)
	{
		sheetData.Attributes[OCSM.Attribute.Resolve] = value;
		updateMaxWillpower();
	}
	
	private void changed_Stamina(int value)
	{
		sheetData.Attributes[OCSM.Attribute.Stamina] = value;
		updateMaxHealth();
	}
	
	private void changed_Strength(int value)
	{
		sheetData.Attributes[OCSM.Attribute.Strength] = value;
		
		updateSpeed();
	}
	private void changed_Wits(int value)
	{
		sheetData.Attributes[OCSM.Attribute.Wits] = value;
		
		updateDefense();
	}
	
	private void changed_Academics(int value) { sheetData.Skills[OCSM.Skill.Academics] = value; }
	private void changed_Athletics(int value)
	{
		sheetData.Skills[OCSM.Skill.Athletics] = value;
		
		updateDefense();
	}
	private void changed_AnimalKen(int value) { sheetData.Skills[OCSM.Skill.AnimalKen] = value; }
	private void changed_Brawl(int value) { sheetData.Skills[OCSM.Skill.Brawl] = value; }
	private void changed_Computer(int value) { sheetData.Skills[OCSM.Skill.Computer] = value; }
	private void changed_Crafts(int value) { sheetData.Skills[OCSM.Skill.Crafts] = value; }
	private void changed_Drive(int value) { sheetData.Skills[OCSM.Skill.Drive] = value; }
	private void changed_Empathy(int value) { sheetData.Skills[OCSM.Skill.Empathy] = value; }
	private void changed_Expression(int value) { sheetData.Skills[OCSM.Skill.Expression] = value; }
	private void changed_Firearms(int value) { sheetData.Skills[OCSM.Skill.Firearms] = value; }
	private void changed_Intimidation(int value) { sheetData.Skills[OCSM.Skill.Intimidation] = value; }
	private void changed_Investigation(int value) { sheetData.Skills[OCSM.Skill.Investigation] = value; }
	private void changed_Larceny(int value) { sheetData.Skills[OCSM.Skill.Larceny] = value; }
	private void changed_Medicine(int value) { sheetData.Skills[OCSM.Skill.Medicine] = value; }
	private void changed_Occult(int value) { sheetData.Skills[OCSM.Skill.Occult] = value; }
	private void changed_Persuasion(int value) { sheetData.Skills[OCSM.Skill.Persuasion] = value; }
	private void changed_Politics(int value) { sheetData.Skills[OCSM.Skill.Politics] = value; }
	private void changed_Science(int value) { sheetData.Skills[OCSM.Skill.Science] = value; }
	private void changed_Socialize(int value) { sheetData.Skills[OCSM.Skill.Socialize] = value; }
	private void changed_Stealth(int value) { sheetData.Skills[OCSM.Skill.Stealth] = value; }
	private void changed_Streetwise(int value) { sheetData.Skills[OCSM.Skill.Streetwise] = value; }
	private void changed_Subterfuge(int value) { sheetData.Skills[OCSM.Skill.Subterfuge] = value; }
	private void changed_Survival(int value) { sheetData.Skills[OCSM.Skill.Survival] = value; }
	private void changed_Weaponry(int value) { sheetData.Skills[OCSM.Skill.Weaponry] = value; }
	
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
