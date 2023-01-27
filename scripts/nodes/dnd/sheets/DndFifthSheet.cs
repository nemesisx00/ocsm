using Godot;
using System;
using System.Collections.Generic;
using OCSM.DnD.Fifth;
using OCSM.Nodes.Autoload;
using OCSM.Nodes.Sheets;
using OCSM.DnD.Fifth.Meta;
using OCSM.Nodes.DnD.Fifth;
using OCSM.DnD.Fifth.Inventory;

namespace OCSM.Nodes.DnD.Sheets
{
	public class DndFifthSheet : CharacterSheet<FifthAdventurer>
	{
		private sealed class Names
		{
			public const string Alignment = "Alignment";
			public const string ArmorClass = "ArmorClass";
			public const string Background = "Background";
			public const string BackgroundFeatures = "Background Features";
			public const string BardicInspiration = "BardicInspiration";
			public const string BardicInspirationDie = "BardicInspirationDie";
			public const string Bonds = "Bonds";
			public const string CharacterName = "CharacterName";
			public const string Copper = "Copper";
			public const string CurrentHP = "CurrentHP";
			public const string Electrum = "Electrum";
			public const string Flaws = "Flaws";
			public const string Gold = "Gold";
			public const string HPBar = "HPBar";
			public const string Ideals = "Ideals";
			public const string InitiativeBonus = "InitiativeBonus";
			public const string Inspiration = "Inspiration";
			public const string Inventory = "Inventory";
			public const string MaxHP = "MaxHP";
			public const string PersonalityTraits = "PersonalityTraits";
			public const string Platinum = "Platinum";
			public const string PlayerName = "PlayerName";
			public const string Race = "Race";
			public const string RaceFeatures = "Racial Features";
			public const string Silver = "Silver";
			public const string Speed = "Speed";
			public const string TempHP = "TempHP";
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
			
			InitAndConnect(GetNode<SpinBox>(NodePathBuilder.SceneUnique(Names.CurrentHP)), SheetData.HP.Current, nameof(changed_CurrentHP));
			InitAndConnect(GetNode<SpinBox>(NodePathBuilder.SceneUnique(Names.MaxHP)), SheetData.HP.Max, nameof(changed_MaxHP));
			InitAndConnect(GetNode<SpinBox>(NodePathBuilder.SceneUnique(Names.TempHP)), SheetData.HP.Temp, nameof(changed_TempHP));
			
			InitAndConnect(GetNode<SpinBox>(NodePathBuilder.SceneUnique(Names.Copper)), SheetData.CoinPurse.Copper, nameof(changed_Copper));
			InitAndConnect(GetNode<SpinBox>(NodePathBuilder.SceneUnique(Names.Silver)), SheetData.CoinPurse.Silver, nameof(changed_Silver));
			InitAndConnect(GetNode<SpinBox>(NodePathBuilder.SceneUnique(Names.Electrum)), SheetData.CoinPurse.Electrum, nameof(changed_Electrum));
			InitAndConnect(GetNode<SpinBox>(NodePathBuilder.SceneUnique(Names.Gold)), SheetData.CoinPurse.Gold, nameof(changed_Gold));
			InitAndConnect(GetNode<SpinBox>(NodePathBuilder.SceneUnique(Names.Platinum)), SheetData.CoinPurse.Platinum, nameof(changed_Platinum));
			
			InitAndConnect(GetNode<ToggleButton>(NodePathBuilder.SceneUnique(Names.Inspiration)), SheetData.Inspiration, nameof(changed_Inspiration));
			InitAndConnect(GetNode<ToggleButton>(NodePathBuilder.SceneUnique(Names.BardicInspiration)), SheetData.BardicInspiration, nameof(changed_BardicInspiration));
			InitAndConnect(GetNode<DieOptionsButton>(NodePathBuilder.SceneUnique(Names.BardicInspirationDie)), SheetData.BardicInspirationDie, nameof(changed_BardicInspirationDie));
			
			InitAndConnect(GetNode<AutosizeTextEdit>(NodePathBuilder.SceneUnique(Names.PersonalityTraits)), SheetData.PersonalityTraits, nameof(changed_PersonalityTraits));
			InitAndConnect(GetNode<AutosizeTextEdit>(NodePathBuilder.SceneUnique(Names.Ideals)), SheetData.Ideals, nameof(changed_Ideals));
			InitAndConnect(GetNode<AutosizeTextEdit>(NodePathBuilder.SceneUnique(Names.Bonds)), SheetData.Bonds, nameof(changed_Bonds));
			InitAndConnect(GetNode<AutosizeTextEdit>(NodePathBuilder.SceneUnique(Names.Flaws)), SheetData.Flaws, nameof(changed_Flaws));
			
			foreach(var ability in SheetData.Abilities)
			{
				InitAndConnect(GetNode<AbilityNode>(NodePathBuilder.SceneUnique(ability.Name)), ability, nameof(changed_Ability));
			}
			
			InitAndConnect(GetNode<Inventory>(NodePathBuilder.SceneUnique(Names.Inventory)), SheetData.Inventory, nameof(changed_Inventory));
			
			base._Ready();
			refreshFeatures();
			toggleBardicInspirationDie();
			updateCalculatedTraits();
		}
		
		protected new void InitAndConnect<T1, T2>(T1 node, T2 initialValue, string handlerName, bool nodeChanged = false)
			where T1: Control
		{
			if(node is AbilityNode ab)
			{
				if(initialValue is Ability ability)
				{
					ab.Ability = ability;
					ab.refresh();
				}
				
				ab.Connect(nameof(AbilityNode.AbilityChanged), this, handlerName);
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
			else if(node is DieOptionsButton dob)
			{
				if(initialValue is OCSM.DnD.Fifth.Die die)
				{
					for(var i = 0; i < dob.GetItemCount(); i++)
					{
						if(dob.GetItemText(i).Contains(die.Sides.ToString()))
						{
							dob.Selected = i;
							break;
						}
					}
				}
				dob.Connect(Constants.Signal.ItemSelected, this, handlerName);
			}
			else if(node is Inventory i)
			{
				if(initialValue is List<Item> items)// && metadataManager.Container is DnDFifthContainer dfc)
				{
					if(SheetData.Abilities.Find(a => a.Name.Equals(Ability.Names.Strength)) is Ability strength)
						i.Strength = strength;
					if(SheetData.Abilities.Find(a => a.Name.Equals(Ability.Names.Dexterity)) is Ability dexterity)
						i.Dexterity = dexterity;
					
					i.Items = items; //new List<Item>();
					//validate each item before adding
					/*
					foreach(var item in items)
					{
						if(dfc.Items.Find(i => i.Name.Equals(item.Name)) is Item it)
						{
							i.Items.Add(it);
						}
					}
					*/
					i.regenerateItems();
				}
				i.Connect(nameof(Inventory.ItemsChanged), this, handlerName);
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
		
		private int calculateAc()
		{
			var ac = 10;
			var addDex = true;
			var dexLimit = 0;
			
			//Find the first equipped armor
			if(SheetData.Inventory.Find(i => i is ItemArmor ia && ia.Equipped) is ItemArmor armor
				&& SheetData.Abilities.Find(a => a.Name.Equals(Ability.Names.Strength)) is Ability strength
				&& strength.Score >= armor.MinimumStrength)
			{
				ac = armor.BaseArmorClass;
				addDex = armor.AllowDexterityBonus;
				if(armor.LimitDexterityBonus)
					dexLimit = armor.DexterityBonusLimit;
			}
			
			if(SheetData.Abilities.Find(a => a.Name.Equals(Ability.Names.Dexterity)) is Ability dexterity)
			{
				if(addDex)
				{
					var mod = dexterity.Modifier;
					if(dexLimit > 0 && mod > dexLimit)
						mod = dexLimit;
					ac += mod;
				}
				//Negative Dex modifiers always reduce AC
				else if(dexterity.Modifier < 0)
					ac += dexterity.Modifier;
			}
			
			foreach(var feature in collectAllFeatures())
			{
				foreach(var nb in feature.NumericBonuses.FindAll(nb => nb.Type.Equals(NumericStat.ArmorClass)))
				{
					if(!nb.Add)
						ac = nb.Value;
					else
						ac += nb.Value;
				}
			}
			
			return ac;
		}
		
		private int calculateInitiative()
		{
			var bonus = 0;
			if(SheetData.Abilities.Find(a => a.Name.Equals(Ability.Names.Dexterity)) is Ability dexterity)
				bonus += dexterity.Modifier;
			
			foreach(var feature in collectAllFeatures())
			{
				foreach(var nb in feature.NumericBonuses.FindAll(nb => nb.Type.Equals(NumericStat.Initiative)))
				{
					if(!nb.Add)
						bonus = nb.Value;
					else
						bonus += nb.Value;
				}
			}
			
			return bonus;
		}
		
		private int calculateSpeed()
		{
			var speed = 0;
			
			foreach(var feature in collectAllFeatures())
			{
				foreach(var nb in feature.NumericBonuses.FindAll(nb => nb.Type.Equals(NumericStat.Speed)))
				{
					if(!nb.Add)
						speed = nb.Value;
					else
						speed += nb.Value;
				}
			}
			
			return speed;
		}
		
		private void changed_Ability(Transport<Ability> transport)
		{
			if(SheetData.Abilities.Find(a => a.Name.Equals(transport.Value.Name)) is Ability ab)
				SheetData.Abilities.Remove(ab);
			
			var ability = transport.Value;
			SheetData.Abilities.Add(ability);
			
			if(ability.Name.Equals(Ability.Names.Strength) || ability.Name.Equals(Ability.Names.Dexterity))
			{
				var inventory = GetNode<Inventory>(NodePathBuilder.SceneUnique(Names.Inventory));
				if(ability.Name.Equals(Ability.Names.Strength))
					inventory.Strength = ability;
				
				if(ability.Name.Equals(Ability.Names.Dexterity))
					inventory.Dexterity = ability;
				
				inventory.regenerateItems();
				updateCalculatedTraits();
			}
		}
		
		private void changed_Alignment(string newText) { SheetData.Alignment = newText; }
		
		private void changed_Background(int index)
		{
			if(index > 0 && metadataManager.Container is DnDFifthContainer dfc && dfc.Backgrounds[index - 1] is Background background)
				SheetData.Background = background;
			else
				SheetData.Background = null;
			
			refreshFeatures();
		}
		
		private void changed_BardicInspiration(ToggleButton button)
		{
			SheetData.BardicInspiration = button.CurrentState;
			toggleBardicInspirationDie();
		}
		
		private void changed_BardicInspirationDie(int index)
		{
			var node = GetNode<DieOptionsButton>(NodePathBuilder.SceneUnique(Names.BardicInspirationDie));
			var text = node.GetItemText(index);
			if(!String.IsNullOrEmpty(text))
			{
				var die = new OCSM.DnD.Fifth.Die() { Sides = int.Parse(text.Substring(1)) };
				SheetData.BardicInspirationDie = die;
			}
			else
			{
				SheetData.BardicInspirationDie = null;
				node.Selected = 0;
			}
		}
		
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
		
		private void changed_Copper(float value) { SheetData.CoinPurse.Copper = (int)value; }
		private void changed_CurrentHP(float value) { SheetData.HP.Current = (int)value; }
		private void changed_Electrum(float value) { SheetData.CoinPurse.Electrum = (int)value; }
		
		private void changed_Flaws()
		{
			var text = GetNode<AutosizeTextEdit>(NodePathBuilder.SceneUnique(Names.Flaws)).Text;
			SheetData.Flaws = text;
		}
		
		private void changed_Gold(float value) { SheetData.CoinPurse.Gold = (int)value; }
		
		private void changed_Ideals()
		{
			var text = GetNode<AutosizeTextEdit>(NodePathBuilder.SceneUnique(Names.Ideals)).Text;
			SheetData.Ideals = text;
		}
		
		private void changed_Inspiration(ToggleButton button) { SheetData.Inspiration = button.CurrentState; }
		
		private void changed_Inventory(Transport<List<Item>> transport)
		{
			SheetData.Inventory = transport.Value;
			
			updateCalculatedTraits();
		}
		
		private void changed_MaxHP(float value) { SheetData.HP.Max = (int)value; }
		
		private void changed_PersonalityTraits()
		{
			var text = GetNode<AutosizeTextEdit>(NodePathBuilder.SceneUnique(Names.PersonalityTraits)).Text;
			SheetData.PersonalityTraits = text;
		}
		
		private void changed_Platinum(float value) { SheetData.CoinPurse.Platinum = (int)value; }
		private void changed_PlayerName(string newText) { SheetData.Player = newText; }
		
		private void changed_Race(int index)
		{
			if(index > 0 && metadataManager.Container is DnDFifthContainer dfc && dfc.Races[index - 1] is Race race)
				SheetData.Race = race;
			else
				SheetData.Race = null;
			
			refreshFeatures();
		}
		
		private void changed_Silver(float value) { SheetData.CoinPurse.Silver = (int)value; }
		private void changed_TempHP(float value) { SheetData.HP.Temp = (int)value; }
		
		private List<OCSM.DnD.Fifth.Feature> collectAllFeatures()
		{
			var allFeatures = new List<OCSM.DnD.Fifth.Feature>();
			if(SheetData.Background is Background background && background.Features.Count > 0)
				allFeatures.AddRange(background.Features);
			if(SheetData.Race is Race race && race.Features.Count > 0)
				allFeatures.AddRange(race.Features);
			
			return allFeatures;
		}
		
		private void refreshFeatures()
		{
			var backgroundFeatures = GetNode<VBoxContainer>(NodePathBuilder.SceneUnique(Names.BackgroundFeatures));
			foreach(Node child in backgroundFeatures.GetChildren())
				child.QueueFree();
			var raceFeatures = GetNode<VBoxContainer>(NodePathBuilder.SceneUnique(Names.RaceFeatures));
			foreach(Node child in raceFeatures.GetChildren())
				child.QueueFree();
			
			var resource = ResourceLoader.Load<PackedScene>(Constants.Scene.DnD.Fifth.Feature);
			if(SheetData.Background is Background background && background.Features.Count > 0)
				renderFeatures(backgroundFeatures, background.Features, resource);
			if(SheetData.Race is Race race && race.Features.Count > 0)
				renderFeatures(raceFeatures, race.Features, resource);
			
			updateCalculatedTraits();
		}
		
		private void renderFeatures(Container node, List<OCSM.DnD.Fifth.Feature> features, PackedScene resource)
		{
			features.Sort();
			foreach(var feature in features)
			{
				var instance = resource.Instance<OCSM.Nodes.DnD.Fifth.Feature>();
				node.AddChild(instance);
				instance.update(feature);
				
				if(features.IndexOf(feature) < features.Count - 1)
					node.AddChild(new HSeparator());
			}
		}
		
		private void toggleBardicInspirationDie()
		{
			var node = GetNode<DieOptionsButton>(NodePathBuilder.SceneUnique(Names.BardicInspirationDie));
			if(SheetData.BardicInspiration)
				node.Show();
			else
				node.Hide();
		}
		
		private void updateCalculatedTraits()
		{
			GetNode<SpinBox>(NodePathBuilder.SceneUnique(Names.ArmorClass)).Value = calculateAc();
			GetNode<SpinBox>(NodePathBuilder.SceneUnique(Names.InitiativeBonus)).Value = calculateInitiative();
			GetNode<SpinBox>(NodePathBuilder.SceneUnique(Names.Speed)).Value = calculateSpeed();
		}
	}
}
