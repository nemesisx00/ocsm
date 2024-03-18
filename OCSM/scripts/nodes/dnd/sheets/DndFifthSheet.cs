using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Ocsm.Dnd.Fifth;
using Ocsm.Nodes.Autoload;
using Ocsm.Nodes.Dnd.Fifth;
using Ocsm.Nodes.Sheets;
using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Dnd.Fifth.Inventory;

namespace Ocsm.Nodes.Dnd.Sheets;

public partial class DndFifthSheet : CharacterSheet<FifthAdventurer>
{
	private sealed class NodePaths
	{
		public const string AbilityScores = "%Ability Scores";
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
		
		inventory = GetNode<Inventory>(NodePaths.Inventory);
		bardicInspirationDie = GetNode<DieOptionsButton>(NodePaths.BardicInspirationDie);
		bonds = GetNode<TextEdit>(NodePaths.Bonds);
		flaws = GetNode<TextEdit>(NodePaths.Flaws);
		ideals = GetNode<TextEdit>(NodePaths.Ideals);
		personalityTraits = GetNode<TextEdit>(NodePaths.PersonalityTraits);
		backgroundFeatures = GetNode<VBoxContainer>(NodePaths.BackgroundFeatures);
		raceFeatures = GetNode<VBoxContainer>(NodePaths.RaceFeatures);
		armorClass = GetNode<SpinBox>(NodePaths.ArmorClass);
		initiativeBonus = GetNode<SpinBox>(NodePaths.InitiativeBonus);
		speed = GetNode<SpinBox>(NodePaths.Speed);
		
		InitLineEdit(GetNode<LineEdit>(NodePaths.CharacterName), SheetData.Name, changed_CharacterName);
		InitLineEdit(GetNode<LineEdit>(NodePaths.PlayerName), SheetData.Player, changed_PlayerName);
		InitLineEdit(GetNode<LineEdit>(NodePaths.Alignment), SheetData.Alignment, changed_Alignment);
		InitRaceOptionsButton(GetNode<RaceOptionsButton>(NodePaths.Race), SheetData.Race, changed_Race);
		InitBackgroundOptionsButton(GetNode<BackgroundOptionsButton>(NodePaths.Background), SheetData.Background, changed_Background);
		
		InitSpinBox(GetNode<SpinBox>(NodePaths.CurrentHP), SheetData.HP.Current, changed_CurrentHP);
		InitSpinBox(GetNode<SpinBox>(NodePaths.MaxHP), SheetData.HP.Max, changed_MaxHP);
		InitSpinBox(GetNode<SpinBox>(NodePaths.TempHP), SheetData.HP.Temp, changed_TempHP);
		
		InitSpinBox(GetNode<SpinBox>(NodePaths.Copper), SheetData.CoinPurse.Copper, changed_Copper);
		InitSpinBox(GetNode<SpinBox>(NodePaths.Silver), SheetData.CoinPurse.Silver, changed_Silver);
		InitSpinBox(GetNode<SpinBox>(NodePaths.Electrum), SheetData.CoinPurse.Electrum, changed_Electrum);
		InitSpinBox(GetNode<SpinBox>(NodePaths.Gold), SheetData.CoinPurse.Gold, changed_Gold);
		InitSpinBox(GetNode<SpinBox>(NodePaths.Platinum), SheetData.CoinPurse.Platinum, changed_Platinum);
		
		InitToggleButton(GetNode<ToggleButton>(NodePaths.Inspiration), SheetData.Inspiration, changed_Inspiration);
		InitToggleButton(GetNode<ToggleButton>(NodePaths.BardicInspiration), SheetData.BardicInspiration, changed_BardicInspiration);
		InitDieOptionsButton(bardicInspirationDie, SheetData.BardicInspirationDie, changed_BardicInspirationDie);
		
		InitTextEdit(personalityTraits, SheetData.PersonalityTraits, changed_PersonalityTraits);
		InitTextEdit(ideals, SheetData.Ideals, changed_Ideals);
		InitTextEdit(bonds, SheetData.Bonds, changed_Bonds);
		InitTextEdit(flaws, SheetData.Flaws, changed_Flaws);
		
		GetNode<AbilityScores>(NodePaths.AbilityScores)
			.initialize<AbilityRow>(SheetData.Abilities, changed_Ability);
		
		InitInventory(inventory, SheetData.Inventory, changed_Inventory);
		
		base._Ready();
		refreshFeatures();
		toggleBardicInspirationDie();
		updateCalculatedTraits();
	}
	
	protected void InitBackgroundOptionsButton(BackgroundOptionsButton node, Background initialValue, BackgroundOptionsButton.ItemSelectedEventHandler handler)
	{
		if(node is BackgroundOptionsButton)
		{
			if(initialValue is Background)
				node.SelectItemByText(initialValue.Name);
			node.ItemSelected += handler;
		}
	}
	
	protected void InitClassOptionsButton(ClassOptionsButton node, Class initialValue, ClassOptionsButton.ItemSelectedEventHandler handler)
	{
		if(node is ClassOptionsButton)
		{
			if(initialValue is Class)
				node.SelectItemByText(initialValue.Name);
			node.ItemSelected += handler;
		}
	}
	
	protected void InitDieOptionsButton(DieOptionsButton node, Ocsm.Dnd.Fifth.Die initialValue, DieOptionsButton.ItemSelectedEventHandler handler)
	{
		if(node is DieOptionsButton)
		{
			if(initialValue is Ocsm.Dnd.Fifth.Die)
				node.SelectItemByText(initialValue.ToString());
			node.ItemSelected += handler;
		}
	}
	
	protected void InitInventory(Inventory node, List<Item> initialValue, Inventory.ItemsChangedEventHandler handler)
	{
		if(node is Inventory)
		{
			if(SheetData.Abilities.Find(a => a.Name.Equals(AbilityInfo.Abilities.Strength)) is AbilityInfo strength)
				node.Strength = strength;
			if(SheetData.Abilities.Find(a => a.Name.Equals(AbilityInfo.Abilities.Dexterity)) is AbilityInfo dexterity)
				node.Dexterity = dexterity;
			
			if(initialValue is List<Item>)
				node.Items = initialValue;
			//validate each item before adding
			
			/*
			if(metadataManager.Container is DndFifthContainer dfc)
				dfc.Items.Where(item => items.Find(i => i.Name.Equals(item.Name)).Any())
					.ToList()
					.ForEach(item => node.Items.Add(item));
			*/
			node.regenerateItems();
			node.ItemsChanged += handler;
		}
	}
	
	protected void InitRaceOptionsButton(RaceOptionsButton node, Race initialValue, RaceOptionsButton.ItemSelectedEventHandler handler)
	{
		if(node is RaceOptionsButton)
		{
			if(initialValue is Race)
				node.SelectItemByText(initialValue.Name);
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
			&& SheetData.Abilities.Find(a => a.Name.Equals(AbilityInfo.Abilities.Strength)) is AbilityInfo strength
			&& strength.Score >= armor.MinimumStrength)
		{
			ac = armor.BaseArmorClass;
			addDex = armor.AllowDexterityBonus;
			if(armor.LimitDexterityBonus)
				dexLimit = armor.DexterityBonusLimit;
		}
		
		if(SheetData.Abilities.Find(a => a.Name.Equals(AbilityInfo.Abilities.Dexterity)) is AbilityInfo dexterity)
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
		
		collectAllFeatures().ForEach(f => {
			f.NumericBonuses.FindAll(nb => nb.Type.Equals(NumericStat.ArmorClass))
				.ForEach(nb => {
					if(!nb.Add)
						ac = nb.Value;
					else
						ac += nb.Value;
				});
		});
		
		return ac;
	}
	
	private int calculateInitiative()
	{
		var bonus = 0;
		if(SheetData.Abilities.Find(a => a.Name.Equals(AbilityInfo.Abilities.Dexterity)) is AbilityInfo dexterity)
			bonus += dexterity.Modifier;
		
		collectAllFeatures().ForEach(f => {
			f.NumericBonuses.FindAll(nb => nb.Type.Equals(NumericStat.Initiative))
				.ForEach(nb => {
					if(!nb.Add)
						bonus = nb.Value;
					else
						bonus += nb.Value;
				});
		});
		
		return bonus;
	}
	
	private int calculateSpeed()
	{
		var speed = 0;
		
		collectAllFeatures().ForEach(f => {
			f.NumericBonuses.FindAll(nb => nb.Type.Equals(NumericStat.Speed))
				.ForEach(nb => {
					if(!nb.Add)
						speed = nb.Value;
					else
						speed += nb.Value;
				});
		});
		
		return speed;
	}
	
	private void changed_Ability(Transport<AbilityInfo> transport)
	{
		if(SheetData.Abilities.Find(a => a.Name.Equals(transport.Value.Name)) is AbilityInfo ab)
			SheetData.Abilities.Remove(ab);
		
		var ability = transport.Value;
		SheetData.Abilities.Add(ability);
		
		if(ability.Name.Equals(AbilityInfo.Abilities.Strength) || ability.Name.Equals(AbilityInfo.Abilities.Dexterity))
		{
			if(ability.Name.Equals(AbilityInfo.Abilities.Strength))
				inventory.Strength = ability;
			
			if(ability.Name.Equals(AbilityInfo.Abilities.Dexterity))
				inventory.Dexterity = ability;
			
			inventory.regenerateItems();
			updateCalculatedTraits();
		}
	}
	
	private void changed_Alignment(string newText) { SheetData.Alignment = newText; }
	
	private void changed_Background(long index)
	{
		if(index > 0 && metadataManager.Container is DndFifthContainer dfc && dfc.Backgrounds[(int)index - 1] is Background background)
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
			var die = new Ocsm.Dnd.Fifth.Die() { Sides = int.Parse(text.Substring(1)) };
			SheetData.BardicInspirationDie = die;
		}
		else
		{
			SheetData.BardicInspirationDie = null;
			bardicInspirationDie.Deselect();
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
		if(index > 0 && metadataManager.Container is DndFifthContainer dfc && dfc.Races[(int)index - 1] is Race race)
			SheetData.Race = race;
		else
			SheetData.Race = null;
		
		refreshFeatures();
	}
	
	private void changed_Silver(double value) { SheetData.CoinPurse.Silver = (int)value; }
	private void changed_TempHP(double value) { SheetData.HP.Temp = (int)value; }
	
	private List<Ocsm.Dnd.Fifth.Feature> collectAllFeatures()
	{
		var allFeatures = new List<Ocsm.Dnd.Fifth.Feature>();
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
		
		var resource = GD.Load<PackedScene>(Constants.Scene.Dnd.Fifth.Feature);
		if(SheetData.Background is Background background && background.Features.Any())
			background.Features.ForEach(f => renderFeature(backgroundFeatures, f, resource, f.Equals(background.Features.Last())));
		if(SheetData.Race is Race race && race.Features.Any())
			race.Features.ForEach(f => renderFeature(raceFeatures, f, resource, f.Equals(race.Features.Last())));
		
		updateCalculatedTraits();
	}
	
	private void renderFeature(Container node, Ocsm.Dnd.Fifth.Feature feature, PackedScene resource, bool separator = false)
	{
		var instance = resource.Instantiate<Ocsm.Nodes.Dnd.Fifth.Feature>();
		node.AddChild(instance);
		instance.update(feature);
		
		if(separator)
			node.AddChild(new HSeparator());
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
