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
		protected const long AttributeMax = 5;
		protected const long IntegrityMax = 10;
		
		protected class NodePath
		{
			public const string Advantages = "%Advantages";
			public const string Attributes = NodePath.Traits + "/%Attributes";
			public const string Details = "%Details";
			public const string GameNotes = "%Game Notes";
			public const string Inventory = "%Inventory";
			public const string Merits = "%Merits";
			public const string MeritsFromMetadata = "%MeritsFromMetadata";
			public const string Skills = NodePath.Traits + "/%Skills";
			public const string SkillSpecialties = NodePath.Skills + "/%Specialties";
			public const string Traits = "%Traits";
			
			// Advantages
			public const string Armor = NodePath.Advantages + "/%Armor";
			public const string Aspirations = NodePath.Advantages + "/%Aspirations";
			public const string Beats = NodePath.Advantages + "/%Beats";
			public const string Defense = NodePath.Advantages + "/%Defense";
			public const string Conditions = NodePath.Advantages + "/%Conditions";
			public const string Experience = NodePath.Advantages + "/%Experience";
			public const string Health = NodePath.Advantages + "/%Health";
			public const string Initiative = NodePath.Advantages + "/%Initiative";
			public const string Speed = NodePath.Advantages + "/%Speed";
			public const string Willpower = NodePath.Advantages + "/%Willpower";
			
			// Details
			public const string Chronicle = NodePath.Details + "/%Chronicle";
			public const string Concept = NodePath.Details + "/%Concept";
			public const string Name = NodePath.Details + "/%Name";
			public const string Player = NodePath.Details + "/%Player";
			public const string Size = NodePath.Details + "/%Size";
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
			beats = GetNode<TrackSimple>(NodePath.Beats);
			defense = GetNode<Label>(NodePath.Defense);
			experience = GetNode<SpinBox>(NodePath.Experience);
			health = GetNode<TrackComplex>(NodePath.Health);
			initiative = GetNode<Label>(NodePath.Initiative);
			speed = GetNode<Label>(NodePath.Speed);
			willpower = GetNode<TrackSimple>(NodePath.Willpower);
			
			InitEntryList(GetNode<EntryList>(NodePath.Aspirations), SheetData.Aspirations, changed_Aspirations);
			InitTrackSimple(beats, SheetData.Beats, changed_Beats);
			InitEntryList(GetNode<EntryList>(NodePath.Conditions), SheetData.Conditions, changed_Conditions);
			InitSpinBox(experience, SheetData.Experience, changed_Experience);
			InitTrackComplex(health, SheetData.Advantages.Health.toTrackComplex(), changed_Health, SheetData.Advantages.Health.Max);
			InitTrackSimple(willpower, SheetData.Advantages.WillpowerSpent, changed_Willpower, SheetData.Advantages.WillpowerMax);
			
			InitLineEdit(GetNode<LineEdit>(NodePath.Chronicle), SheetData.Details.Chronicle, changed_Chronicle);
			InitLineEdit(GetNode<LineEdit>(NodePath.Concept), SheetData.Details.Concept, changed_Concept);
			InitLineEdit(GetNode<LineEdit>(NodePath.Name), SheetData.Name, changed_Name);
			if(!String.IsNullOrEmpty(SheetData.Name))
				Name = SheetData.Name;
			InitLineEdit(GetNode<LineEdit>(NodePath.Player), SheetData.Player, changed_Player);
			InitSpinBox(GetNode<SpinBox>(NodePath.Size), SheetData.Advantages.Size, changed_Size);
			
			foreach(var a in SheetData.Attributes)
			{
				if(!String.IsNullOrEmpty(a.Name))
					InitTrackSimple(GetNode<TrackSimple>(NodePath.Attributes + "/%" + a.Name), a.Value, changed_Attribute);
			}
			
			foreach(var s in SheetData.Skills)
			{
				if(!String.IsNullOrEmpty(s.Name))
					InitTrackSimple(GetNode<TrackSimple>(NodePath.Skills + "/%" + s.Name), s.Value, changed_Skill);
			}
			
			InitSpecialtyList(GetNode<SpecialtyList>(NodePath.SkillSpecialties), SheetData.Specialties, changed_SkillSpecialty);
			InitMeritList(GetNode<MeritList>(NodePath.Merits), SheetData.Merits, changed_Merits);
			
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
		
		protected void InitSpecialtyList(SpecialtyList node, List<Pair<string, string>> initialValue, SpecialtyList.ValueChangedEventHandler handler)
		{
			if(node is SpecialtyList)
			{
				if(initialValue is List<Pair<string, string>>)
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
					defense.Text = (dex.Value + athl.Value).ToString();
				else
					defense.Text = (wits.Value + athl.Value).ToString();
			}
		}
		
		protected void updateInitiative()
		{
			var dex = SheetData.Attributes.FirstOrDefault(a => a.Name.Equals(OCSM.CoD.Attribute.Dexterity.Name));
			var comp = SheetData.Attributes.FirstOrDefault(a => a.Name.Equals(OCSM.CoD.Attribute.Composure.Name));
			
			if(dex is OCSM.CoD.Attribute && comp is OCSM.CoD.Attribute)
			{
				initiative.Text = (dex.Value + comp.Value).ToString();
			}
		}
		
		protected void updateMaxHealth()
		{
			var stam = SheetData.Attributes.FirstOrDefault(a => a.Name.Equals(OCSM.CoD.Attribute.Stamina.Name));
			
			if(stam is OCSM.CoD.Attribute)
			{
				SheetData.Advantages.Health.Max = SheetData.Advantages.Size + stam.Value;
				health.updateMax(SheetData.Advantages.Health.Max);
			}
		}
		
		protected void updateMaxWillpower()
		{
			var comp = SheetData.Attributes.FirstOrDefault(a => a.Name.Equals(OCSM.CoD.Attribute.Composure.Name));
			var res = SheetData.Attributes.FirstOrDefault(a => a.Name.Equals(OCSM.CoD.Attribute.Resolve.Name));
			
			if(comp is OCSM.CoD.Attribute && res is OCSM.CoD.Attribute)
			{
				SheetData.Advantages.WillpowerMax = comp.Value + res.Value;
				willpower.updateMax(SheetData.Advantages.WillpowerMax);
			}
		}
		
		protected void updateSpeed()
		{
			var str = SheetData.Attributes.FirstOrDefault(a => a.Name.Equals(OCSM.CoD.Attribute.Strength.Name));
			var dex = SheetData.Attributes.FirstOrDefault(a => a.Name.Equals(OCSM.CoD.Attribute.Dexterity.Name));
			
			if(str is OCSM.CoD.Attribute && dex is OCSM.CoD.Attribute)
			{
				speed.Text = (str.Value + dex.Value + SheetData.Advantages.Size).ToString();
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
				beats.updateValue(SheetData.Beats);
				SheetData.Experience++;
				experience.Value = SheetData.Experience;
			}
			else
				SheetData.Beats = value;
		}
		
		private void changed_Chronicle(string newText) { SheetData.Details.Chronicle = newText; }
		private void changed_Concept(string newText) { SheetData.Details.Concept = newText; }
		private void changed_Conditions(Transport<List<string>> transport) { SheetData.Conditions = transport.Value; }
		private void changed_Experience(double number) { SheetData.Experience = (long)number; }
		private void changed_Health(Transport<Dictionary<string, long>> transport) { SheetData.Advantages.Health.fromTrackComplex(transport.Value); }
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
		
		private void changed_SkillSpecialty(Transport<List<Pair<string, string>>> transport) { SheetData.Specialties = transport.Value; }
		
		private void changed_Size(double number)
		{
			SheetData.Advantages.Size = (long)number;
			updateMaxHealth();
			updateSpeed();
		}
		
		private void changed_Willpower(long value) { SheetData.Advantages.WillpowerSpent = value; }
	}
}
