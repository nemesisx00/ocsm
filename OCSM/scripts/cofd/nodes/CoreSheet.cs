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
	
	protected class NodePaths
	{
		public static readonly NodePath Traits = new("%Traits");
		public static readonly NodePath Advantages = new("%Advantages");
		public static readonly NodePath Attributes = new($"{Traits}/%Attributes");
		public static readonly NodePath Details = new("%Details");
		public static readonly NodePath GameNotes = new("%Game Notes");
		public static readonly NodePath Inventory = new("%Inventory");
		public static readonly NodePath Merits = new("%Merits");
		public static readonly NodePath MeritsFromMetadata = new("%MeritsFromMetadata");
		public static readonly NodePath Skills = new($"{Traits}/%Skills");
		public static readonly NodePath SkillSpecialties = new($"{Skills}/%Specialties");
		
		// Advantages
		public static readonly NodePath Armor = new($"{Advantages}/%Armor");
		public static readonly NodePath Aspirations = new($"{Advantages}/%Aspirations");
		public static readonly NodePath Beats = new($"{Advantages}/%Beats");
		public static readonly NodePath Defense = new($"{Advantages}/%Defense");
		public static readonly NodePath Conditions = new($"{Advantages}/%Conditions");
		public static readonly NodePath Experience = new($"{Advantages}/%Experience");
		public static readonly NodePath Health = new($"{Advantages}/%Health");
		public static readonly NodePath Initiative = new($"{Advantages}/%Initiative");
		public static readonly NodePath Speed = new($"{Advantages}/%Speed");
		public static readonly NodePath Willpower = new($"{Advantages}/%Willpower");
		
		// Details
		public static readonly NodePath Chronicle = new($"{Details}/%Chronicle");
		public static readonly NodePath Concept = new($"{Details}/%Concept");
		public static readonly NodePath Name = new($"{Details}/%Name");
		public static readonly NodePath Player = new($"{Details}/%Player");
		public static readonly NodePath Size = new($"{Details}/%Size");
	}
	
	protected TrackSimple beats;
	protected Label defense;
	protected SpinBox experience;
	protected TrackComplex health;
	protected Label initiative;
	protected Label speed;
	protected TrackSimple willpower;
	
	public override void _Ready()
	{
		beats = GetNode<TrackSimple>(NodePaths.Beats);
		defense = GetNode<Label>(NodePaths.Defense);
		experience = GetNode<SpinBox>(NodePaths.Experience);
		health = GetNode<TrackComplex>(NodePaths.Health);
		initiative = GetNode<Label>(NodePaths.Initiative);
		speed = GetNode<Label>(NodePaths.Speed);
		willpower = GetNode<TrackSimple>(NodePaths.Willpower);
		
		InitEntryList(GetNode<EntryList>(NodePaths.Aspirations), SheetData.Aspirations, changed_Aspirations);
		InitTrackSimple(beats, SheetData.Beats, changed_Beats);
		InitEntryList(GetNode<EntryList>(NodePaths.Conditions), SheetData.Conditions, changed_Conditions);
		InitSpinBox(experience, SheetData.Experience, changed_Experience);
		InitTrackComplex(health, SheetData.Advantages.Health.ToTrackComplex(), changed_Health, SheetData.Advantages.Health.Max);
		InitTrackSimple(willpower, SheetData.Advantages.WillpowerSpent, changed_Willpower, SheetData.Advantages.WillpowerMax);
		
		InitLineEdit(GetNode<LineEdit>(NodePaths.Chronicle), SheetData.Details.Chronicle, changed_Chronicle);
		InitLineEdit(GetNode<LineEdit>(NodePaths.Concept), SheetData.Details.Concept, changed_Concept);
		InitLineEdit(GetNode<LineEdit>(NodePaths.Name), SheetData.Name, changed_Name);
		if(!string.IsNullOrEmpty(SheetData.Name))
			Name = SheetData.Name;
		InitLineEdit(GetNode<LineEdit>(NodePaths.Player), SheetData.Player, changed_Player);
		InitSpinBox(GetNode<SpinBox>(NodePaths.Size), SheetData.Advantages.Size, changed_Size);
		
		SheetData.Attributes.Where(a => !string.IsNullOrEmpty(a.Name))
			.ToList()
			.ForEach(a => InitTrackSimple(GetNode<TrackSimple>($"{NodePaths.Attributes}/%{a.Name}"), a.Value, changed_Attribute));
		
		SheetData.Skills.Where(s => !string.IsNullOrEmpty(s.Name))
			.ToList()
			.ForEach(s => InitTrackSimple(GetNode<TrackSimple>($"{NodePaths.Skills}/%{s.Name}"), s.Value, changed_Skill));
		
		InitSpecialtyList(GetNode<SpecialtyList>(NodePaths.SkillSpecialties), SheetData.Specialties, changed_SkillSpecialty);
		InitMeritList(GetNode<MeritList>(NodePaths.Merits), SheetData.Merits, changed_Merits);
		
		updateDefense();
		updateInitiative();
		updateMaxHealth();
		updateMaxWillpower();
		updateSpeed();
		
		base._Ready();
	}
	
	protected void InitMeritList(MeritList node, List<Merit> initialValue, MeritList.MeritValueChangedEventHandler handler)
	{
		if(node is not null)
		{
			if(initialValue is not null)
				node.Values = initialValue;
			
			node.Refresh();
			node.MeritValueChanged += handler;
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
	
	protected void InitSpecialtyList(SpecialtyList node, Dictionary<Skill.EnumValues, string> initialValue, SpecialtyList.ValueChangedEventHandler handler)
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
		int newValue = 0;
		
		if(SheetData.Attributes.FirstOrDefault(a => a.Kind.Equals(Attribute.EnumValues.Dexterity)) is Attribute dex)
			newValue = dex.Value;
		
		if(SheetData.Attributes.FirstOrDefault(a => a.Kind.Equals(Attribute.EnumValues.Wits)) is Attribute wits
				&& newValue < wits.Value)
			newValue = wits.Value;
		
		if(SheetData.Skills.FirstOrDefault(s => s.Kind.Equals(Skill.EnumValues.Athletics)) is Skill athl)
			newValue += athl.Value;
		
		if(newValue > 0)
			defense.Text = newValue.ToString();
	}
	
	protected void updateInitiative()
	{
		int newValue = 0;
		
		if(SheetData.Attributes.FirstOrDefault(a => a.Kind.Equals(Attribute.EnumValues.Dexterity)) is Attribute dex)
			newValue += dex.Value;
		
		if(SheetData.Attributes.FirstOrDefault(a => a.Kind.Equals(Attribute.EnumValues.Composure)) is Attribute comp)
			newValue += comp.Value;
		
		if(newValue > 0)
			initiative.Text = newValue.ToString();
	}
	
	protected void updateMaxHealth()
	{
		if(SheetData.Attributes.FirstOrDefault(a => a.Kind.Equals(Attribute.EnumValues.Stamina)) is Attribute stam)
		{
			SheetData.Advantages.Health.Max = SheetData.Advantages.Size + stam.Value;
			health.UpdateMax(SheetData.Advantages.Health.Max);
		}
	}
	
	protected void updateMaxWillpower()
	{
		int newValue = 0;
		
		if(SheetData.Attributes.FirstOrDefault(a => a.Kind.Equals(Attribute.EnumValues.Composure)) is Attribute comp)
			newValue += comp.Value;
		
		if(SheetData.Attributes.FirstOrDefault(a => a.Kind.Equals(Attribute.EnumValues.Resolve)) is Attribute res)
			newValue += res.Value;
		
		if(newValue <= 0)
			newValue = 1;
		
		var advantages = SheetData.Advantages;
		advantages.WillpowerMax = newValue;
		SheetData.Advantages = advantages;
		willpower.Max = SheetData.Advantages.WillpowerMax;
	}
	
	protected void updateSpeed()
	{
		int newValue = SheetData.Advantages.Size;
		
		if(SheetData.Attributes.FirstOrDefault(a => a.Kind.Equals(Attribute.EnumValues.Dexterity)) is Attribute dex)
			newValue += dex.Value;
		
		if(SheetData.Attributes.FirstOrDefault(a => a.Kind.Equals(Attribute.EnumValues.Strength)) is Attribute str)
			newValue += str.Value;
		
		speed.Text = newValue.ToString();
	}
	
	private void changed_Aspirations(Transport<List<string>> transport) => SheetData.Aspirations = transport.Value;
	
	private void changed_Attribute(int value, string name)
	{
		var attr = SheetData.Attributes.FirstOrDefault(a => a.Name.Equals(name));
		if(attr is not null)
		{
			attr.Value = value;
			
			switch(attr.Kind)
			{
				case Attribute.EnumValues.Composure:
					updateMaxWillpower();
					updateInitiative();
					break;
				
				case Attribute.EnumValues.Dexterity:
					updateDefense();
					updateInitiative();
					updateSpeed();
					break;
				
				case Attribute.EnumValues.Resolve:
					updateMaxWillpower();
					break;
				
				case Attribute.EnumValues.Stamina:
					updateMaxHealth();
					break;
				
				case Attribute.EnumValues.Strength:
					updateSpeed();
					break;
				
				case Attribute.EnumValues.Wits:
					updateDefense();
					break;
			}
		}
	}
	
	private void changed_Beats(int value, string name)
	{
		if(value >= 5)
		{
			SheetData.Beats = 0;
			beats.Value = SheetData.Beats;
			SheetData.Experience++;
			experience.Value = SheetData.Experience;
		}
		else
			SheetData.Beats = value;
	}
	
	private void changed_Chronicle(string newText)
	{
		var details = SheetData.Details;
		details.Chronicle = newText;
		SheetData.Details = details;
	}
	
	private void changed_Concept(string newText)
	{
		var details = SheetData.Details;
		details.Concept = newText;
		SheetData.Details = details;
	}
	
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
	
	private void changed_Skill(int value, string name)
	{
		var skill = SheetData.Skills.FirstOrDefault(s => s.Name.Equals(name));
		if(skill is not null)
		{
			skill.Value = value;
			
			switch(skill.Kind)
			{
				case Skill.EnumValues.Athletics:
					updateDefense();
					break;
			}
		}
	}
	
	private void changed_SkillSpecialty(Transport<Dictionary<Skill.EnumValues, string>> transport) => SheetData.Specialties = transport.Value;
	
	private void changed_Size(double number)
	{
		var advantages = SheetData.Advantages;
		advantages.Size = (int)number;
		SheetData.Advantages = advantages;
		
		updateMaxHealth();
		updateSpeed();
	}
	
	private void changed_Willpower(int value, string name)
	{
		var advantages = SheetData.Advantages;
		advantages.WillpowerSpent = value;
		SheetData.Advantages = advantages;
	}
}