using Godot;
using System;
using System.Collections.Generic;
using OCSM.DnD.Fifth;
using OCSM.Nodes.Autoload;
using OCSM.Nodes.DnD.Fifth;
using OCSM.Nodes.Sheets;
using OCSM.DnD.Fifth.Meta;
using OCSM.DnD.Fifth.Inventory;

namespace OCSM.Nodes.DnD.Sheets
{
	public partial class DndFifthSheet : CharacterSheet<FifthAdventurer>
	{
		private sealed class NodePath
		{
			public const string Alignment = "%Alignment";
			public const string ArmorClass = "%ArmorClass";
			public const string Background = "%Background";
			public const string BackgroundFeatures = "%Background Features";
			public const string BardicInspiration = "%BardicInspiration";
			public const string BardicInspirationDie = "%BardicInspirationDie";
			public const string Bonds = "%Bonds";
			public const string CharacterName = "%CharacterName";
			public const string Copper = "%Copper";
			public const string CurrentHP = "%CurrentHP";
			public const string Electrum = "%Electrum";
			public const string Flaws = "%Flaws";
			public const string Gold = "%Gold";
			public const string HPBar = "%HPBar";
			public const string Ideals = "%Ideals";
			public const string InitiativeBonus = "%InitiativeBonus";
			public const string Inspiration = "%Inspiration";
			public const string Inventory = "%Inventory";
			public const string MaxHP = "%MaxHP";
			public const string PersonalityTraits = "%PersonalityTraits";
			public const string Platinum = "%Platinum";
			public const string PlayerName = "%PlayerName";
			public const string Race = "%Race";
			public const string RaceFeatures = "%Racial Features";
			public const string Silver = "%Silver";
			public const string Speed = "%Speed";
			public const string TempHP = "%TempHP";
		}
		
		private MetadataManager metadataManager;
		
		private Inventory inventory;
		private DieOptionsButton bardicInspirationDie;
		private TextEdit bonds;
		private TextEdit flaws;
		private TextEdit ideals;
		private TextEdit personalityTraits;
		private VBoxContainer backgroundFeatures;
		private VBoxContainer raceFeatures;
		private SpinBox armorClass;
		private SpinBox initiativeBonus;
		private SpinBox speed;
		
		public override void _Ready()
		{
			metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
			
			if(!(SheetData is FifthAdventurer))
				SheetData = new FifthAdventurer();
			
			inventory = GetNode<Inventory>(NodePath.Inventory);
			bardicInspirationDie = GetNode<DieOptionsButton>(NodePath.BardicInspirationDie);
			bonds = GetNode<TextEdit>(NodePath.Bonds);
			flaws = GetNode<TextEdit>(NodePath.Flaws);
			ideals = GetNode<TextEdit>(NodePath.Ideals);
			personalityTraits = GetNode<TextEdit>(NodePath.PersonalityTraits);
			backgroundFeatures = GetNode<VBoxContainer>(NodePath.BackgroundFeatures);
			raceFeatures = GetNode<VBoxContainer>(NodePath.RaceFeatures);
			armorClass = GetNode<SpinBox>(NodePath.ArmorClass);
			initiativeBonus = GetNode<SpinBox>(NodePath.InitiativeBonus);
			speed = GetNode<SpinBox>(NodePath.Speed);
			
			InitLineEdit(GetNode<LineEdit>(NodePath.CharacterName), SheetData.Name, changed_CharacterName);
			InitLineEdit(GetNode<LineEdit>(NodePath.PlayerName), SheetData.Player, changed_PlayerName);
			InitLineEdit(GetNode<LineEdit>(NodePath.Alignment), SheetData.Alignment, changed_Alignment);
			InitRaceOptionsButton(GetNode<RaceOptionsButton>(NodePath.Race), SheetData.Race, changed_Race);
			InitBackgroundOptionsButton(GetNode<BackgroundOptionsButton>(NodePath.Background), SheetData.Background, changed_Background);
			
			InitSpinBox(GetNode<SpinBox>(NodePath.CurrentHP), SheetData.HP.Current, changed_CurrentHP);
			InitSpinBox(GetNode<SpinBox>(NodePath.MaxHP), SheetData.HP.Max, changed_MaxHP);
			InitSpinBox(GetNode<SpinBox>(NodePath.TempHP), SheetData.HP.Temp, changed_TempHP);
			
			InitSpinBox(GetNode<SpinBox>(NodePath.Copper), SheetData.CoinPurse.Copper, changed_Copper);
			InitSpinBox(GetNode<SpinBox>(NodePath.Silver), SheetData.CoinPurse.Silver, changed_Silver);
			InitSpinBox(GetNode<SpinBox>(NodePath.Electrum), SheetData.CoinPurse.Electrum, changed_Electrum);
			InitSpinBox(GetNode<SpinBox>(NodePath.Gold), SheetData.CoinPurse.Gold, changed_Gold);
			InitSpinBox(GetNode<SpinBox>(NodePath.Platinum), SheetData.CoinPurse.Platinum, changed_Platinum);
			
			InitToggleButton(GetNode<ToggleButton>(NodePath.Inspiration), SheetData.Inspiration, changed_Inspiration);
			InitToggleButton(GetNode<ToggleButton>(NodePath.BardicInspiration), SheetData.BardicInspiration, changed_BardicInspiration);
			InitDieOptionsButton(bardicInspirationDie, SheetData.BardicInspirationDie, changed_BardicInspirationDie);
			
			InitTextEdit(personalityTraits, SheetData.PersonalityTraits, changed_PersonalityTraits);
			InitTextEdit(ideals, SheetData.Ideals, changed_Ideals);
			InitTextEdit(bonds, SheetData.Bonds, changed_Bonds);
			InitTextEdit(flaws, SheetData.Flaws, changed_Flaws);
			
			foreach(var ability in SheetData.Abilities)
			{
				InitAbilityNode(GetNode<AbilityNode>("%" + ability.Name), ability, changed_Ability);
			}
			
			InitInventory(inventory, SheetData.Inventory, changed_Inventory);
			
			base._Ready();
			refreshFeatures();
			toggleBardicInspirationDie();
			updateCalculatedTraits();
		}
		
		protected void InitAbilityNode(AbilityNode node, Ability initialValue, AbilityNode.AbilityChangedEventHandler handler)
		{
			if(node is AbilityNode)
			{
				if(initialValue is Ability)
				{
					node.Ability = initialValue;
					node.refresh();
				}
				node.AbilityChanged += handler;
			}
		}
		
		protected void InitBackgroundOptionsButton(BackgroundOptionsButton node, Background initialValue, BackgroundOptionsButton.ItemSelectedEventHandler handler)
		{
			if(node is BackgroundOptionsButton)
			{
				if(initialValue is Background && metadataManager.Container is DnDFifthContainer dfc)
				{
					var index = dfc.Backgrounds.FindIndex(b => b.Name.Equals(initialValue.Name)) + 1;
					if(index > 0)
						node.Selected = index;
				}
				node.ItemSelected += handler;
			}
		}
		
		protected void InitClassOptionsButton(ClassOptionsButton node, Class initialValue, ClassOptionsButton.ItemSelectedEventHandler handler)
		{
			if(node is ClassOptionsButton)
			{
				if(initialValue is Class && metadataManager.Container is DnDFifthContainer dfc)
				{
					var index = dfc.Classes.FindIndex(c => c.Name.Equals(initialValue.Name)) + 1;
					if(index > 0)
						node.Selected = index;
				}
				node.ItemSelected += handler;
			}
		}
		
		protected void InitDieOptionsButton(DieOptionsButton node, OCSM.DnD.Fifth.Die initialValue, DieOptionsButton.ItemSelectedEventHandler handler)
		{
			if(node is DieOptionsButton)
			{
				if(initialValue is OCSM.DnD.Fifth.Die)
				{
					for(var i = 0; i < node.ItemCount; i++)
					{
						if(node.GetItemText(i).Contains(initialValue.Sides.ToString()))
						{
							node.Selected = i;
							break;
						}
					}
				}
				node.ItemSelected += handler;
			}
		}
		
		protected void InitInventory(Inventory node, List<Item> initialValue, Inventory.ItemsChangedEventHandler handler)
		{
			if(node is Inventory)
			{
				if(SheetData.Abilities.Find(a => a.Name.Equals(Ability.Names.Strength)) is Ability strength)
					node.Strength = strength;
				if(SheetData.Abilities.Find(a => a.Name.Equals(Ability.Names.Dexterity)) is Ability dexterity)
					node.Dexterity = dexterity;
				
				if(initialValue is List<Item>)
					node.Items = initialValue;
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
				node.regenerateItems();
				node.ItemsChanged += handler;
			}
		}
		
		protected void InitRaceOptionsButton(RaceOptionsButton node, Race initialValue, RaceOptionsButton.ItemSelectedEventHandler handler)
		{
			if(node is RaceOptionsButton)
			{
				if(initialValue is Race && metadataManager.Container is DnDFifthContainer dfc)
				{
					var index = dfc.Races.FindIndex(r => r.Name.Equals(initialValue.Name)) + 1;
					if(index > 0)
						node.Selected = index;
				}
				node.ItemSelected += handler;
			}
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
				if(ability.Name.Equals(Ability.Names.Strength))
					inventory.Strength = ability;
				
				if(ability.Name.Equals(Ability.Names.Dexterity))
					inventory.Dexterity = ability;
				
				inventory.regenerateItems();
				updateCalculatedTraits();
			}
		}
		
		private void changed_Alignment(string newText) { SheetData.Alignment = newText; }
		
		private void changed_Background(long index)
		{
			if(index > 0 && metadataManager.Container is DnDFifthContainer dfc && dfc.Backgrounds[(int)index - 1] is Background background)
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
		
		private void changed_BardicInspirationDie(long index)
		{
			var text = bardicInspirationDie.GetItemText((int)index);
			if(!String.IsNullOrEmpty(text))
			{
				var die = new OCSM.DnD.Fifth.Die() { Sides = int.Parse(text.Substring(1)) };
				SheetData.BardicInspirationDie = die;
			}
			else
			{
				SheetData.BardicInspirationDie = null;
				bardicInspirationDie.Selected = 0;
			}
		}
		
		private void changed_Bonds() { SheetData.Bonds = bonds.Text; }
		
		private void changed_CharacterName(string newText)
		{
			SheetData.Name = newText;
			if(!String.IsNullOrEmpty(SheetData.Name))
				Name = SheetData.Name;
		}
		
		private void changed_Copper(double value) { SheetData.CoinPurse.Copper = (int)value; }
		private void changed_CurrentHP(double value) { SheetData.HP.Current = (int)value; }
		private void changed_Electrum(double value) { SheetData.CoinPurse.Electrum = (int)value; }
		private void changed_Flaws() { SheetData.Flaws = flaws.Text; }
		private void changed_Gold(double value) { SheetData.CoinPurse.Gold = (int)value; }
		private void changed_Ideals() { SheetData.Ideals = ideals.Text; }
		private void changed_Inspiration(ToggleButton button) { SheetData.Inspiration = button.CurrentState; }
		
		private void changed_Inventory(Transport<List<Item>> transport)
		{
			SheetData.Inventory = transport.Value;
			
			updateCalculatedTraits();
		}
		
		private void changed_MaxHP(double value) { SheetData.HP.Max = (int)value; }
		private void changed_PersonalityTraits() { SheetData.PersonalityTraits = personalityTraits.Text; }
		private void changed_Platinum(double value) { SheetData.CoinPurse.Platinum = (int)value; }
		private void changed_PlayerName(string newText) { SheetData.Player = newText; }
		
		private void changed_Race(long index)
		{
			if(index > 0 && metadataManager.Container is DnDFifthContainer dfc && dfc.Races[(int)index - 1] is Race race)
				SheetData.Race = race;
			else
				SheetData.Race = null;
			
			refreshFeatures();
		}
		
		private void changed_Silver(double value) { SheetData.CoinPurse.Silver = (int)value; }
		private void changed_TempHP(double value) { SheetData.HP.Temp = (int)value; }
		
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
			foreach(Node child in backgroundFeatures.GetChildren())
				child.QueueFree();
			foreach(Node child in raceFeatures.GetChildren())
				child.QueueFree();
			
			var resource = GD.Load<PackedScene>(Constants.Scene.DnD.Fifth.Feature);
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
				var instance = resource.Instantiate<OCSM.Nodes.DnD.Fifth.Feature>();
				node.AddChild(instance);
				instance.update(feature);
				
				if(features.IndexOf(feature) < features.Count - 1)
					node.AddChild(new HSeparator());
			}
		}
		
		private void toggleBardicInspirationDie()
		{
			if(SheetData.BardicInspiration)
				bardicInspirationDie.Show();
			else
				bardicInspirationDie.Hide();
		}
		
		private void updateCalculatedTraits()
		{
			armorClass.Value = calculateAc();
			initiativeBonus.Value = calculateInitiative();
			speed.Value = calculateSpeed();
		}
	}
}
