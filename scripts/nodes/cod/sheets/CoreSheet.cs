using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using OCSM;

public abstract class CoreSheet<T> : CharacterSheet<T>
	where T: CodCore
{
	protected const string AdvantagesPath = "Column/Advantages/";
	protected const string TabContainerPath = "Column/TabContainer/";
	protected const string DetailsPath = TabContainerPath + "Details/";
	protected const string TraitsPath = TabContainerPath + "Traits/";
	protected const string AttributesPath = TraitsPath + "Attributes/";
	protected const string SkillsPath = TraitsPath + "Skills/";
	protected const string InventoryPath = TabContainerPath + "Inventory/";
	protected const string GameNotesPath = TabContainerPath + "Game Notes/";
	
	protected const string Flaws = "Flaws";
	protected const string Merits = "Merits";
	protected const string SkillSpecialties = "Specialties";
	
	protected class Advantage
	{
		public const string Armor = "Armor";
		public const string Aspirations = "Aspirations";
		public const string Beats = "Beats";
		public const string Defense = "Defense";
		public const string Conditions = "Conditions";
		public const string Experience = "Experience";
		public const string Health = "Health";
		public const string Initiative = "Initiative";
		public const string Speed = "Speed";
		public const string Willpower = "Willpower";
	}
	
	protected class Detail
	{
		public const string Chronicle = "Chronicle";
		public const string Concept = "Concept";
		public const string Name = "Name";
		public const string Player = "Player";
		public const string Size = "Size";
	}
	
	public override void _Ready()
	{
		InitAndConnect(GetNode<ItemList>(PathBuilder.SceneUnique(Advantage.Aspirations, AdvantagesPath)), SheetData.Aspirations, nameof(changed_Aspirations));
		InitAndConnect(GetNode<TrackSimple>(PathBuilder.SceneUnique(Advantage.Beats, AdvantagesPath)), SheetData.Beats, nameof(changed_Beats));
		InitAndConnect(GetNode<ItemList>(PathBuilder.SceneUnique(Advantage.Conditions, AdvantagesPath)), SheetData.Conditions, nameof(changed_Conditions));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Advantage.Experience, AdvantagesPath)), SheetData.Experience.ToString(), nameof(changed_Experience));
		InitAndConnect(GetNode<TrackComplex>(PathBuilder.SceneUnique(Advantage.Health, AdvantagesPath)), SheetData.HealthCurrent, nameof(changed_Health));
		InitAndConnect(GetNode<TrackSimple>(PathBuilder.SceneUnique(Advantage.Willpower, AdvantagesPath)), SheetData.WillpowerSpent, nameof(changed_Willpower));
		
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Chronicle, DetailsPath)), SheetData.Chronicle, nameof(changed_Chronicle));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Concept, DetailsPath)), SheetData.Concept, nameof(changed_Concept));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Name, DetailsPath)), SheetData.Name, nameof(changed_Name));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Player, DetailsPath)), SheetData.Player, nameof(changed_Player));
		InitAndConnect(GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Size, DetailsPath)), SheetData.Size.ToString(), nameof(changed_Size));
		
		foreach(var a in SheetData.Attributes)
		{
			if(!String.IsNullOrEmpty(a.Name))
				InitAndConnect(GetNode<TrackSimple>(PathBuilder.SceneUnique(a.Name, AttributesPath)), a.Value, nameof(changed_Attribute), true);
		}
		
		foreach(var s in SheetData.Skills)
		{
			if(!String.IsNullOrEmpty(s.Name))
				InitAndConnect(GetNode<TrackSimple>(PathBuilder.SceneUnique(s.Name, SkillsPath)), s.Value, nameof(changed_Skill), true);
		}
		
		InitAndConnect(GetNode<SpecialtyList>(PathBuilder.SceneUnique(SkillSpecialties, SkillsPath)), SheetData.Specialties, nameof(changed_SkillSpecialty));
		
		InitAndConnect(GetNode<ItemDotsList>(PathBuilder.SceneUnique(Merits)), SheetData.Merits, nameof(changed_Merits));
		InitAndConnect(GetNode<ItemDotsList>(PathBuilder.SceneUnique(Flaws)), SheetData.Flaws, nameof(changed_Flaws));
		
		updateDefense();
		updateInitiative();
		updateMaxHealth();
		updateMaxWillpower();
		updateSpeed();
	}
	
	protected void InitAndConnect<T1, T2>(T1 node, T2 initialValue, string handlerName, bool nodeChanged = false)
		where T1: Control
	{
		if(node is LineEdit le)
		{
			le.Text = initialValue as string;
			le.Connect(Constants.Signal.TextChanged, this, handlerName);
		}
		else if(node is TrackSimple ts)
		{
			var val = 0;
			if(initialValue is int)
				val = int.Parse(initialValue.ToString());
			if(val > 0)
				ts.updateValue(val);
			
			if(nodeChanged)
				ts.Connect(Constants.Signal.NodeChanged, this, handlerName);
			else
				ts.Connect(Constants.Signal.ValueChanged, this, handlerName);
		}
		else if(node is TrackComplex tc)
		{
			tc.Values = initialValue as Dictionary<string, int>;
			tc.Connect(Constants.Signal.ValueChanged, this, handlerName);
		}
		else if(node is ItemList il)
		{
			il.Values = initialValue as List<string>;
			il.refresh();
			il.Connect(Constants.Signal.ValueChanged, this, handlerName);
		}
		else if(node is ItemDotsList idl)
		{
			idl.Values = initialValue as Dictionary<string, int>;
			idl.refresh();
			idl.Connect(Constants.Signal.ValueChanged, this, handlerName);
		}
		else if(node is SpecialtyList sl)
		{
			sl.Values = initialValue as Dictionary<string, string>;
			sl.refresh();
			sl.Connect(Constants.Signal.ValueChanged, this, handlerName);
		}
	}
	
	protected void updateDefense()
	{
		var dex = SheetData.Attributes.FirstOrDefault(a => a.Name.Equals(OCSM.Attribute.Dexterity.Name));
		var wits = SheetData.Attributes.FirstOrDefault(a => a.Name.Equals(OCSM.Attribute.Wits.Name));
		var athl = SheetData.Skills.FirstOrDefault(s => s.Name.Equals(OCSM.Skill.Athletics.Name));
		
		if(dex is OCSM.Attribute && wits is OCSM.Attribute && athl is OCSM.Skill)
		{
			if(dex.Value < wits.Value)
				GetNode<Label>(PathBuilder.SceneUnique(Advantage.Defense, AdvantagesPath)).Text = (dex.Value + athl.Value).ToString();
			else
				GetNode<Label>(PathBuilder.SceneUnique(Advantage.Defense, AdvantagesPath)).Text = (wits.Value + athl.Value).ToString();
		}
	}
	
	protected void updateInitiative()
	{
		var dex = SheetData.Attributes.FirstOrDefault(a => a.Name.Equals(OCSM.Attribute.Dexterity.Name));
		var comp = SheetData.Attributes.FirstOrDefault(a => a.Name.Equals(OCSM.Attribute.Composure.Name));
		
		if(dex is OCSM.Attribute && comp is OCSM.Attribute)
		{
			GetNode<Label>(PathBuilder.SceneUnique(Advantage.Initiative, AdvantagesPath)).Text = (dex.Value + comp.Value).ToString();
		}
	}
	
	protected void updateMaxHealth()
	{
		var stam = SheetData.Attributes.FirstOrDefault(a => a.Name.Equals(OCSM.Attribute.Stamina.Name));
		
		if(stam is OCSM.Attribute)
		{
			SheetData.HealthMax = SheetData.Size + stam.Value;
			GetNode<TrackComplex>(PathBuilder.SceneUnique(Advantage.Health, AdvantagesPath)).updateMax(SheetData.HealthMax);
		}
	}
	
	protected void updateMaxWillpower()
	{
		var comp = SheetData.Attributes.FirstOrDefault(a => a.Name.Equals(OCSM.Attribute.Composure.Name));
		var res = SheetData.Attributes.FirstOrDefault(a => a.Name.Equals(OCSM.Attribute.Resolve.Name));
		
		if(comp is OCSM.Attribute && res is OCSM.Attribute)
		{
			SheetData.WillpowerMax = comp.Value + res.Value;
			GetNode<TrackSimple>(PathBuilder.SceneUnique(Advantage.Willpower, AdvantagesPath)).updateMax(SheetData.WillpowerMax);
		}
	}
	
	protected void updateSpeed()
	{
		var str = SheetData.Attributes.FirstOrDefault(a => a.Name.Equals(OCSM.Attribute.Strength.Name));
		var dex = SheetData.Attributes.FirstOrDefault(a => a.Name.Equals(OCSM.Attribute.Dexterity.Name));
		
		if(str is OCSM.Attribute && dex is OCSM.Attribute)
		{
			GetNode<Label>(PathBuilder.SceneUnique(Advantage.Speed, AdvantagesPath)).Text = (str.Value + dex.Value + SheetData.Size).ToString();
		}
	}private void changed_Aspirations(List<string> values) { SheetData.Aspirations = values; }
	
	private void changed_Attribute(TrackSimple node)
	{
		var attr = SheetData.Attributes.FirstOrDefault(a => a.Name.Equals(node.Name));
		if(attr is OCSM.Attribute)
			attr.Value = node.Value;
		
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
	
	private void changed_Beats(int value)
	{
		if(value >= 5)
		{
			SheetData.Beats = 0;
			GetNode<TrackSimple>(PathBuilder.SceneUnique(Advantage.Beats, AdvantagesPath)).updateValue(SheetData.Beats);
			SheetData.Experience++;
			GetNode<LineEdit>(PathBuilder.SceneUnique(Advantage.Experience, AdvantagesPath)).Text = SheetData.Experience.ToString();
		}
		else
			SheetData.Beats = value;
	}
	
	private void changed_Chronicle(string newText) { SheetData.Chronicle = newText; }
	private void changed_Concept(string newText) { SheetData.Concept = newText; }
	private void changed_Conditions(List<string> values) { SheetData.Conditions = values; }
	
	private void changed_Experience(string newText)
	{
		int newXp;
		if(int.TryParse(newText, out newXp))
			SheetData.Experience = newXp;
		else
			GetNode<LineEdit>(PathBuilder.SceneUnique(Advantage.Experience, AdvantagesPath)).Text = SheetData.Experience.ToString();
	}
	private void changed_Flaws(Dictionary<string, int> values) { SheetData.Flaws = values; }
	private void changed_Health(Dictionary<string, int> values) { SheetData.HealthCurrent = values; }
	private void changed_Merits(Dictionary<string, int> values) { SheetData.Merits = values; }
	
	private void changed_Name(string newText)
	{
		SheetData.Name = newText;
		if(!String.IsNullOrEmpty(SheetData.Name))
			Name = SheetData.Name;
	}
	
	private void changed_Player(string newText) { SheetData.Player = newText; }
	
	private void changed_Skill(TrackSimple node)
	{
		var skill = SheetData.Skills.FirstOrDefault(s => s.Name.Equals(node.Name));
		if(skill is OCSM.Skill)
			skill.Value = node.Value;
		
		switch(node.Name)
		{
			case OCSM.Skill.Names.Athletics:
				updateDefense();
				break;
		}
	}
	
	private void changed_SkillSpecialty(Dictionary<string, string> values) { SheetData.Specialties = values; }
	
	private void changed_Size(string newText)
	{
		int newSize;
		if(int.TryParse(newText, out newSize))
		{
			SheetData.Size = newSize;
			updateMaxHealth();
			updateSpeed();
		}
		else
			GetNode<LineEdit>(PathBuilder.SceneUnique(Detail.Size, DetailsPath)).Text = SheetData.Size.ToString();
	}
	
	private void changed_Willpower(int value) { SheetData.WillpowerSpent = value; }
}
