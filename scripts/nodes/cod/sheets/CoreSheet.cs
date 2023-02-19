using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using OCSM.CoD;
using OCSM.Nodes.Sheets;

namespace OCSM.Nodes.CoD.Sheets
{
	public abstract partial class CoreSheet<T> : CharacterSheet<T>
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
			InitEntryList(GetNode<EntryList>(NodePathBuilder.SceneUnique(Advantage.Aspirations, AdvantagesPath)), SheetData.Aspirations, changed_Aspirations);
			InitTrackSimple(GetNode<TrackSimple>(NodePathBuilder.SceneUnique(Advantage.Beats, AdvantagesPath)), SheetData.Beats, changed_Beats);
			InitEntryList(GetNode<EntryList>(NodePathBuilder.SceneUnique(Advantage.Conditions, AdvantagesPath)), SheetData.Conditions, changed_Conditions);
			InitSpinBox(GetNode<SpinBox>(NodePathBuilder.SceneUnique(Advantage.Experience, AdvantagesPath)), SheetData.Experience, changed_Experience);
			InitTrackComplex(GetNode<TrackComplex>(NodePathBuilder.SceneUnique(Advantage.Health, AdvantagesPath)), SheetData.HealthCurrent, changed_Health);
			InitTrackSimple(GetNode<TrackSimple>(NodePathBuilder.SceneUnique(Advantage.Willpower, AdvantagesPath)), SheetData.WillpowerSpent, changed_Willpower);
			
			InitLineEdit(GetNode<LineEdit>(NodePathBuilder.SceneUnique(Detail.Chronicle, DetailsPath)), SheetData.Chronicle, changed_Chronicle);
			InitLineEdit(GetNode<LineEdit>(NodePathBuilder.SceneUnique(Detail.Concept, DetailsPath)), SheetData.Concept, changed_Concept);
			InitLineEdit(GetNode<LineEdit>(NodePathBuilder.SceneUnique(Detail.Name, DetailsPath)), SheetData.Name, changed_Name);
			if(!String.IsNullOrEmpty(SheetData.Name))
				Name = SheetData.Name;
			InitLineEdit(GetNode<LineEdit>(NodePathBuilder.SceneUnique(Detail.Player, DetailsPath)), SheetData.Player, changed_Player);
			InitSpinBox(GetNode<SpinBox>(NodePathBuilder.SceneUnique(Detail.Size, DetailsPath)), SheetData.Size, changed_Size);
			
			foreach(var a in SheetData.Attributes)
			{
				if(!String.IsNullOrEmpty(a.Name))
					InitTrackSimple(GetNode<TrackSimple>(NodePathBuilder.SceneUnique(a.Name, AttributesPath)), a.Value, changed_Attribute);
			}
			
			foreach(var s in SheetData.Skills)
			{
				if(!String.IsNullOrEmpty(s.Name))
					InitTrackSimple(GetNode<TrackSimple>(NodePathBuilder.SceneUnique(s.Name, SkillsPath)), s.Value, changed_Skill);
			}
			
			InitSpecialtyList(GetNode<SpecialtyList>(NodePathBuilder.SceneUnique(SkillSpecialties, SkillsPath)), SheetData.Specialties, changed_SkillSpecialty);
			InitMeritList(GetNode<MeritList>(NodePathBuilder.SceneUnique(Merits)), SheetData.Merits, changed_Merits);
			
			updateDefense();
			updateInitiative();
			updateMaxHealth();
			updateMaxWillpower();
			updateSpeed();
			
			base._Ready();
		}
		
		protected void InitMeritList(MeritList node, List<Merit> initialValue, MeritList.ValueChangedEventHandler handler)
		{
			if(node is MeritList)
			{
				if(initialValue is List<Merit>)
					node.Values = initialValue;
				node.refresh();
				node.ValueChanged += handler;
			}
		}
		
		protected void InitItemDotsList(ItemDotsList node, Dictionary<string, long> initialValue, ItemDotsList.ValueChangedEventHandler handler)
		{
			if(node is ItemDotsList)
			{
				if(initialValue is Dictionary<string, long>)
					node.Values = initialValue;
				node.refresh();
				node.ValueChanged += handler;
			}
		}
		
		protected void InitSpecialtyList(SpecialtyList node, Dictionary<string, string> initialValue, SpecialtyList.ValueChangedEventHandler handler)
		{
			if(node is SpecialtyList)
			{
				if(initialValue is Dictionary<string, string>)
					node.Values = initialValue;
				node.refresh();
				node.ValueChanged += handler;
			}
		}
		
		protected void updateDefense()
		{
			var dex = SheetData.Attributes.FirstOrDefault(a => a.Name.Equals(OCSM.CoD.Attribute.Dexterity.Name));
			var wits = SheetData.Attributes.FirstOrDefault(a => a.Name.Equals(OCSM.CoD.Attribute.Wits.Name));
			var athl = SheetData.Skills.FirstOrDefault(s => s.Name.Equals(Skill.Athletics.Name));
			
			if(dex is OCSM.CoD.Attribute && wits is OCSM.CoD.Attribute && athl is Skill)
			{
				if(dex.Value < wits.Value)
					GetNode<Label>(NodePathBuilder.SceneUnique(Advantage.Defense, AdvantagesPath)).Text = (dex.Value + athl.Value).ToString();
				else
					GetNode<Label>(NodePathBuilder.SceneUnique(Advantage.Defense, AdvantagesPath)).Text = (wits.Value + athl.Value).ToString();
			}
		}
		
		protected void updateInitiative()
		{
			var dex = SheetData.Attributes.FirstOrDefault(a => a.Name.Equals(OCSM.CoD.Attribute.Dexterity.Name));
			var comp = SheetData.Attributes.FirstOrDefault(a => a.Name.Equals(OCSM.CoD.Attribute.Composure.Name));
			
			if(dex is OCSM.CoD.Attribute && comp is OCSM.CoD.Attribute)
			{
				GetNode<Label>(NodePathBuilder.SceneUnique(Advantage.Initiative, AdvantagesPath)).Text = (dex.Value + comp.Value).ToString();
			}
		}
		
		protected void updateMaxHealth()
		{
			var stam = SheetData.Attributes.FirstOrDefault(a => a.Name.Equals(OCSM.CoD.Attribute.Stamina.Name));
			
			if(stam is OCSM.CoD.Attribute)
			{
				SheetData.HealthMax = SheetData.Size + stam.Value;
				GetNode<TrackComplex>(NodePathBuilder.SceneUnique(Advantage.Health, AdvantagesPath)).updateMax(SheetData.HealthMax);
			}
		}
		
		protected void updateMaxWillpower()
		{
			var comp = SheetData.Attributes.FirstOrDefault(a => a.Name.Equals(OCSM.CoD.Attribute.Composure.Name));
			var res = SheetData.Attributes.FirstOrDefault(a => a.Name.Equals(OCSM.CoD.Attribute.Resolve.Name));
			
			if(comp is OCSM.CoD.Attribute && res is OCSM.CoD.Attribute)
			{
				SheetData.WillpowerMax = comp.Value + res.Value;
				GetNode<TrackSimple>(NodePathBuilder.SceneUnique(Advantage.Willpower, AdvantagesPath)).updateMax(SheetData.WillpowerMax);
			}
		}
		
		protected void updateSpeed()
		{
			var str = SheetData.Attributes.FirstOrDefault(a => a.Name.Equals(OCSM.CoD.Attribute.Strength.Name));
			var dex = SheetData.Attributes.FirstOrDefault(a => a.Name.Equals(OCSM.CoD.Attribute.Dexterity.Name));
			
			if(str is OCSM.CoD.Attribute && dex is OCSM.CoD.Attribute)
			{
				GetNode<Label>(NodePathBuilder.SceneUnique(Advantage.Speed, AdvantagesPath)).Text = (str.Value + dex.Value + SheetData.Size).ToString();
			}
		}private void changed_Aspirations(Transport<List<string>> transport) { SheetData.Aspirations = transport.Value; }
		
		private void changed_Attribute(TrackSimple node)
		{
			var attr = SheetData.Attributes.FirstOrDefault(a => a.Name.Equals(node.Name));
			if(attr is OCSM.CoD.Attribute)
				attr.Value = node.Value;
			
			switch(node.Name)
			{
				case OCSM.CoD.Attribute.Names.Composure:
					updateMaxWillpower();
					updateInitiative();
					break;
				case OCSM.CoD.Attribute.Names.Dexterity:
					updateDefense();
					updateInitiative();
					updateSpeed();
					break;
				case OCSM.CoD.Attribute.Names.Resolve:
					updateMaxWillpower();
					break;
				case OCSM.CoD.Attribute.Names.Stamina:
					updateMaxHealth();
					break;
				case OCSM.CoD.Attribute.Names.Strength:
					updateSpeed();
					break;
				case OCSM.CoD.Attribute.Names.Wits:
					updateDefense();
					break;
			}
		}
		
		private void changed_Beats(long value)
		{
			if(value >= 5)
			{
				SheetData.Beats = 0;
				GetNode<TrackSimple>(NodePathBuilder.SceneUnique(Advantage.Beats, AdvantagesPath)).updateValue(SheetData.Beats);
				SheetData.Experience++;
				GetNode<SpinBox>(NodePathBuilder.SceneUnique(Advantage.Experience, AdvantagesPath)).Value = SheetData.Experience;
			}
			else
				SheetData.Beats = value;
		}
		
		private void changed_Chronicle(string newText) { SheetData.Chronicle = newText; }
		private void changed_Concept(string newText) { SheetData.Concept = newText; }
		private void changed_Conditions(Transport<List<string>> transport) { SheetData.Conditions = transport.Value; }
		private void changed_Experience(double number) { SheetData.Experience = (long)number; }
		private void changed_Health(Transport<Dictionary<string, long>> transport) { SheetData.HealthCurrent = transport.Value; }
		private void changed_Merits(Transport<List<Merit>> transport) { SheetData.Merits = transport.Value; }
		
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
			if(skill is Skill)
				skill.Value = node.Value;
			
			switch(node.Name)
			{
				case Skill.Names.Athletics:
					updateDefense();
					break;
			}
		}
		
		private void changed_SkillSpecialty(Transport<Dictionary<string, string>> transport) { SheetData.Specialties = transport.Value; }
		
		private void changed_Size(double number)
		{
			SheetData.Size = (long)number;
			updateMaxHealth();
			updateSpeed();
		}
		
		private void changed_Willpower(long value) { SheetData.WillpowerSpent = value; }
	}
}
