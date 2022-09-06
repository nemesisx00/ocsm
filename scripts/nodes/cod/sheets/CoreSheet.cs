using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using OCSM.CoD;
using OCSM.Nodes.Sheets;

namespace OCSM.Nodes.CoD.Sheets
{
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
			InitAndConnect(GetNode<EntryList>(NodePathBuilder.SceneUnique(Advantage.Aspirations, AdvantagesPath)), SheetData.Aspirations, nameof(changed_Aspirations));
			InitAndConnect(GetNode<TrackSimple>(NodePathBuilder.SceneUnique(Advantage.Beats, AdvantagesPath)), SheetData.Beats, nameof(changed_Beats));
			InitAndConnect(GetNode<EntryList>(NodePathBuilder.SceneUnique(Advantage.Conditions, AdvantagesPath)), SheetData.Conditions, nameof(changed_Conditions));
			InitAndConnect(GetNode<SpinBox>(NodePathBuilder.SceneUnique(Advantage.Experience, AdvantagesPath)), SheetData.Experience, nameof(changed_Experience));
			InitAndConnect(GetNode<TrackComplex>(NodePathBuilder.SceneUnique(Advantage.Health, AdvantagesPath)), SheetData.HealthCurrent, nameof(changed_Health));
			InitAndConnect(GetNode<TrackSimple>(NodePathBuilder.SceneUnique(Advantage.Willpower, AdvantagesPath)), SheetData.WillpowerSpent, nameof(changed_Willpower));
			
			InitAndConnect(GetNode<LineEdit>(NodePathBuilder.SceneUnique(Detail.Chronicle, DetailsPath)), SheetData.Chronicle, nameof(changed_Chronicle));
			InitAndConnect(GetNode<LineEdit>(NodePathBuilder.SceneUnique(Detail.Concept, DetailsPath)), SheetData.Concept, nameof(changed_Concept));
			InitAndConnect(GetNode<LineEdit>(NodePathBuilder.SceneUnique(Detail.Name, DetailsPath)), SheetData.Name, nameof(changed_Name));
			if(!String.IsNullOrEmpty(SheetData.Name))
				Name = SheetData.Name;
			InitAndConnect(GetNode<LineEdit>(NodePathBuilder.SceneUnique(Detail.Player, DetailsPath)), SheetData.Player, nameof(changed_Player));
			InitAndConnect(GetNode<SpinBox>(NodePathBuilder.SceneUnique(Detail.Size, DetailsPath)), SheetData.Size, nameof(changed_Size));
			
			foreach(var a in SheetData.Attributes)
			{
				if(!String.IsNullOrEmpty(a.Name))
					InitAndConnect(GetNode<TrackSimple>(NodePathBuilder.SceneUnique(a.Name, AttributesPath)), a.Value, nameof(changed_Attribute), true);
			}
			
			foreach(var s in SheetData.Skills)
			{
				if(!String.IsNullOrEmpty(s.Name))
					InitAndConnect(GetNode<TrackSimple>(NodePathBuilder.SceneUnique(s.Name, SkillsPath)), s.Value, nameof(changed_Skill), true);
			}
			
			InitAndConnect(GetNode<SpecialtyList>(NodePathBuilder.SceneUnique(SkillSpecialties, SkillsPath)), SheetData.Specialties, nameof(changed_SkillSpecialty));
			
			InitAndConnect(GetNode<ItemDotsList>(NodePathBuilder.SceneUnique(Merits)), SheetData.Merits, nameof(changed_Merits));
			
			updateDefense();
			updateInitiative();
			updateMaxHealth();
			updateMaxWillpower();
			updateSpeed();
			
			base._Ready();
		}
		
		protected new void InitAndConnect<T1, T2>(T1 node, T2 initialValue, string handlerName, bool nodeChanged = false)
			where T1: Control
		{
			if(node is MeritList ml)
			{
				if(initialValue is List<Merit> merits)
					ml.Values = merits;
				ml.refresh();
				ml.Connect(nameof(MeritList.ValueChanged), this, handlerName);
			}
			else if(node is ItemDotsList idl)
			{
				if(initialValue is Dictionary<string, int> entries)
					idl.Values = entries;
				idl.refresh();
				idl.Connect(nameof(ItemDotsList.ValueChanged), this, handlerName);
			}
			else if(node is SpecialtyList sl)
			{
				if(initialValue is Dictionary<string, string> entries)
					sl.Values = entries;
				sl.refresh();
				sl.Connect(nameof(SpecialtyList.ValueChanged), this, handlerName);
			}
			else
				base.InitAndConnect<T1, T2>(node, initialValue, handlerName, nodeChanged);
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
		}private void changed_Aspirations(List<string> values) { SheetData.Aspirations = values; }
		
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
		
		private void changed_Beats(int value)
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
		private void changed_Conditions(List<string> values) { SheetData.Conditions = values; }
		private void changed_Experience(float number) { SheetData.Experience = (int)number; }
		private void changed_Health(Dictionary<string, int> values) { SheetData.HealthCurrent = values; }
		private void changed_Merits(List<Transport<Merit>> values)
		{
			var list = new List<Merit>();
			foreach(var mt in values)
			{
				list.Add(mt.Value);
			}
			
			SheetData.Merits = list;
		}
		
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
		
		private void changed_SkillSpecialty(Dictionary<string, string> values) { SheetData.Specialties = values; }
		
		private void changed_Size(float number)
		{
			SheetData.Size = (int)number;
			updateMaxHealth();
			updateSpeed();
		}
		
		private void changed_Willpower(int value) { SheetData.WillpowerSpent = value; }
	}
}
