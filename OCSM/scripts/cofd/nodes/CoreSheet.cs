using Godot;
using System.Collections.Generic;
using System.Linq;
using Ocsm.Nodes;

namespace Ocsm.Cofd.Nodes;

public abstract partial class CoreSheet<T> : CharacterSheet<T>
	where T: CofdCore
{
	protected const int DefaultAttributeMax = 5;
	protected const int DefaultIntegrityMax = 10;
	
	protected static class NodePaths
	{
		public static readonly NodePath Advantages = new("%Advantages");
		public static readonly NodePath Attributes = new("%Traits/%Attributes");
		public static readonly NodePath Details = new("%Details");
		public static readonly NodePath GameNotes = new("%Game Notes");
		public static readonly NodePath Inventory = new("%Inventory");
		public static readonly NodePath Merits = new("%Merits");
		public static readonly NodePath MeritsFromMetadata = new("%MeritsFromMetadata");
		public static readonly NodePath Skills = new("%Traits/%Skills");
		public static readonly NodePath SkillSpecialties = new("%Traits/%Skills/%Specialties");
		public static readonly NodePath Traits = new("%Traits");
		
		// Advantages
		public static readonly NodePath Armor = new("%Advantages/%Armor");
		public static readonly NodePath Aspirations = new("%Advantages/%Aspirations");
		public static readonly NodePath Beats = new("%Advantages/%Beats");
		public static readonly NodePath Defense = new("%Advantages/%Defense");
		public static readonly NodePath Conditions = new("%Advantages/%Conditions");
		public static readonly NodePath Experience = new("%Advantages/%Experience");
		public static readonly NodePath Health = new("%Advantages/%Health");
		public static readonly NodePath Initiative = new("%Advantages/%Initiative");
		public static readonly NodePath Speed = new("%Advantages/%Speed");
		public static readonly NodePath Willpower = new("%Advantages/%Willpower");
		
		// Details
		public static readonly NodePath Chronicle = new("%Details/%Chronicle");
		public static readonly NodePath Concept = new("%Details/%Concept");
		public static readonly NodePath Name = new("%Details/%Name");
		public static readonly NodePath Player = new("%Details/%Player");
		public static readonly NodePath Size = new("%Details/%Size");
	}
	
	protected EntryList aspirations;
	protected List<TrackSimple> attributes = [];
	protected TrackSimple beats;
	protected EntryList conditions;
	protected Label defense;
	protected DynamicNumericLabel experience;
	protected TrackComplex health;
	protected Label initiative;
	protected MeritList merits;
	protected List<TrackSimple> skills = [];
	protected SpecialtyList skillSpecialties;
	protected Label speed;
	protected TrackSimple willpower;
	
	public override void _ExitTree()
	{
		aspirations.ValueChanged -= changed_Aspirations;
		beats.ValueChanged -= changed_Beats;
		conditions.ValueChanged -= changed_Conditions;
		health.ValueChanged -= changed_Health;
		willpower.ValueChanged -= changed_Willpower;
		
		foreach(var a in attributes)
			a.NodeChanged -= changed_Attribute;
		
		foreach(var s in skills)
			s.NodeChanged -= changed_Skill;
		
		skillSpecialties.ValueChanged -= changed_SkillSpecialty;
		merits.MeritChanged -= changed_Merits;
		
		base._ExitTree();
	}
	
	public override void _Ready()
	{
		aspirations = GetNode<EntryList>(NodePaths.Aspirations);
		beats = GetNode<TrackSimple>(NodePaths.Beats);
		conditions = GetNode<EntryList>(NodePaths.Conditions);
		defense = GetNode<Label>(NodePaths.Defense);
		experience = GetNode<DynamicNumericLabel>(NodePaths.Experience);
		health = GetNode<TrackComplex>(NodePaths.Health);
		initiative = GetNode<Label>(NodePaths.Initiative);
		merits = GetNode<MeritList>(NodePaths.Merits);
		skillSpecialties = GetNode<SpecialtyList>(NodePaths.SkillSpecialties);
		speed = GetNode<Label>(NodePaths.Speed);
		willpower = GetNode<TrackSimple>(NodePaths.Willpower);
		
		InitEntryList(aspirations, SheetData.Aspirations, changed_Aspirations);
		InitTrackSimple(beats, SheetData.Beats, changed_Beats);
		InitEntryList(conditions, SheetData.Conditions, changed_Conditions);
		InitDynamicNumericLabel(experience, SheetData.Experience, changed_Experience);
		InitTrackComplex(health, SheetData.Advantages.Health.ToTrackComplex(), changed_Health, SheetData.Advantages.Health.Max);
		InitTrackSimple(willpower, SheetData.Advantages.WillpowerSpent, changed_Willpower, SheetData.Advantages.WillpowerMax);
		
		InitLineEdit(GetNode<LineEdit>(NodePaths.Chronicle), SheetData.Details.Chronicle, changed_Chronicle);
		InitLineEdit(GetNode<LineEdit>(NodePaths.Concept), SheetData.Details.Concept, changed_Concept);
		InitLineEdit(GetNode<LineEdit>(NodePaths.Name), SheetData.Name, changed_Name);
		if(!string.IsNullOrEmpty(SheetData.Name))
			Name = SheetData.Name;
		InitLineEdit(GetNode<LineEdit>(NodePaths.Player), SheetData.Player, changed_Player);
		InitSpinBox(GetNode<SpinBox>(NodePaths.Size), SheetData.Advantages.Size, changed_Size);
		
		foreach(var a in SheetData.Attributes.Where(a => !string.IsNullOrEmpty(a.Name)))
		{
			var attr = GetNode<TrackSimple>($"{NodePaths.Attributes}/%{a.Name}");
			InitTrackSimple(attr, a.Value, changed_Attribute);
			attributes.Add(attr);
		}
		
		foreach(var s in SheetData.Skills.Where(s => !string.IsNullOrEmpty(s.Name)))
		{
			var skill = GetNode<TrackSimple>($"{NodePaths.Skills}/%{s.Name}");
			InitTrackSimple(skill, s.Value, changed_Skill);
			skills.Add(skill);
		}
		
		InitSpecialtyList(skillSpecialties, SheetData.Specialties, changed_SkillSpecialty);
		InitMeritList(merits, SheetData.Merits, changed_Merits);
		
		updateDefense();
		updateInitiative();
		updateMaxHealth();
		updateMaxWillpower();
		updateSpeed();
		
		base._Ready();
	}
	
	protected void InitMeritList(MeritList node, List<Merit> initialValue, MeritList.MeritChangedEventHandler handler)
	{
		if(node is not null)
		{
			if(initialValue is not null)
				node.Values = initialValue;
			node.Refresh();
			node.MeritChanged += handler;
		}
	}
	
	protected void InitItemDotsList(ItemDotsList node, Dictionary<string, int> initialValue, ItemDotsList.ValueChangedEventHandler handler)
	{
		if(node is not null)
		{
			if(initialValue is not null)
				node.Values = initialValue;
			node.Refresh();
			node.ValueChanged += handler;
		}
	}
	
	protected void InitSpecialtyList(SpecialtyList node, Dictionary<Traits, string> initialValue, SpecialtyList.ValueChangedEventHandler handler)
	{
		if(node is not null)
		{
			if(initialValue is not null)
				node.Values = initialValue;
			node.Refresh();
			node.ValueChanged += handler;
		}
	}
	
	protected void updateDefense()
	{
		long newValue = 0;
		if(SheetData.Attributes.FirstOrDefault(a => a.Kind == Traits.Dexterity) is TraitDots dex)
			newValue = dex.Value;
		
		if(SheetData.Attributes.FirstOrDefault(a => a.Kind == Traits.Wits) is TraitDots wits
				&& newValue < wits.Value)
			newValue = wits.Value;
		
		if(SheetData.Skills.FirstOrDefault(s => s.Kind == Traits.Athletics) is TraitDots athl)
			newValue += athl.Value;
		
		if(newValue > 0)
			defense.Text = newValue.ToString();
	}
	
	protected void updateInitiative()
	{
		long newValue = 0;
		if(SheetData.Attributes.FirstOrDefault(a => a.Kind == Traits.Dexterity) is TraitDots dex)
			newValue += dex.Value;
		if(SheetData.Attributes.FirstOrDefault(a => a.Kind == Traits.Composure) is TraitDots comp)
			newValue += comp.Value;
		
		if(newValue > 0)
			initiative.Text = newValue.ToString();
	}
	
	protected void updateMaxHealth()
	{
		if(SheetData.Attributes.FirstOrDefault(a => a.Kind == Traits.Stamina) is TraitDots stam)
		{
			SheetData.Advantages.Health.Max = SheetData.Advantages.Size + stam.Value;
			health.UpdateMax(SheetData.Advantages.Health.Max);
		}
	}
	
	protected void updateMaxWillpower()
	{
		int newValue = 0;
		if(SheetData.Attributes.FirstOrDefault(a => a.Kind == Traits.Composure) is TraitDots comp)
			newValue += comp.Value;
		if(SheetData.Attributes.FirstOrDefault(a => a.Kind == Traits.Resolve) is TraitDots res)
			newValue += res.Value;
		
		if(newValue > 0)
		{
			SheetData.Advantages.WillpowerMax = newValue;
			willpower.UpdateMax(SheetData.Advantages.WillpowerMax);
		}
	}
	
	protected void updateSpeed()
	{
		var newValue = SheetData.Advantages.Size;
		if(SheetData.Attributes.FirstOrDefault(a => a.Kind == Traits.Dexterity) is TraitDots dex)
			newValue += dex.Value;
		if(SheetData.Attributes.FirstOrDefault(a => a.Kind == Traits.Strength) is TraitDots str)
			newValue += str.Value;
		
		speed.Text = newValue.ToString();
	}
	
	private void changed_Aspirations(Transport<List<string>> transport) { SheetData.Aspirations = transport.Value; }
	
	private void changed_Attribute(TrackSimple node)
	{
		var attr = SheetData.Attributes.FirstOrDefault(a => a.Name.Equals(node.Name));
		if(attr is not null)
		{
			attr.Value = node.Value;
			
			switch(attr.Kind)
			{
				case Traits.Composure:
					updateMaxWillpower();
					updateInitiative();
					break;
				
				case Traits.Dexterity:
					updateDefense();
					updateInitiative();
					updateSpeed();
					break;
				
				case Traits.Resolve:
					updateMaxWillpower();
					break;
				
				case Traits.Stamina:
					updateMaxHealth();
					break;
				
				case Traits.Strength:
					updateSpeed();
					break;
				
				case Traits.Wits:
					updateDefense();
					break;
			}
		}
	}
	
	private void changed_Beats(int value)
	{
		if(value >= 5)
		{
			SheetData.Beats = 0;
			beats.UpdateValue(SheetData.Beats);
			SheetData.Experience++;
			experience.Value = SheetData.Experience;
		}
		else
			SheetData.Beats = value;
	}
	
	private void changed_Chronicle(string newText) => SheetData.Details.Chronicle = newText;
	private void changed_Concept(string newText) => SheetData.Details.Concept = newText;
	private void changed_Conditions(Transport<List<string>> transport) => SheetData.Conditions = transport.Value;
	private void changed_Experience(double number) => SheetData.Experience = (int)number;
	private void changed_Health(Transport<Dictionary<StatefulButton.States, int>> transport) => SheetData.Advantages.Health.FromTrackComplex(transport.Value);
	private void changed_Merits(Transport<List<Merit>> transport) => SheetData.Merits = transport.Value;
	
	private void changed_Name(string newText)
	{
		SheetData.Name = newText;
		if(!string.IsNullOrEmpty(SheetData.Name))
			Name = SheetData.Name;
	}
	
	private void changed_Player(string newText) => SheetData.Player = newText;
	
	private void changed_Skill(TrackSimple node)
	{
		var skill = SheetData.Skills.FirstOrDefault(s => s.Name.Equals(node.Name));
		if(skill is not null)
		{
			skill.Value = node.Value;
			switch(skill.Kind)
			{
				case Traits.Athletics:
					updateDefense();
					break;
			}
		}
	}
	
	private void changed_SkillSpecialty(Transport<Dictionary<Traits, string>> transport) => SheetData.Specialties = transport.Value;
	
	private void changed_Size(double number)
	{
		SheetData.Advantages.Size = (int)number;
		updateMaxHealth();
		updateSpeed();
	}
	
	private void changed_Willpower(int value) => SheetData.Advantages.WillpowerSpent = value;
}
