using Godot;
using System.Collections.Generic;
using System.Linq;
using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Dnd.Fifth.Inventory;
using Ocsm.Nodes;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class DndFifthSheet : CharacterSheet<FifthAdventurer>
{
	private sealed class NodePaths
	{
		public static readonly NodePath AbilityScores = new("%Ability Scores");
		public static readonly NodePath Alignment = new("%Alignment");
		public static readonly NodePath ArmorClass = new("%ArmorClass");
		public static readonly NodePath Background = new("%Background");
		public static readonly NodePath BackgroundFeatures = new("%Background Features");
		public static readonly NodePath BardicInspiration = new("%BardicInspiration");
		public static readonly NodePath BardicInspirationDie = new("%BardicInspirationDie");
		public static readonly NodePath Bonds = new("%Bonds");
		public static readonly NodePath CharacterName = new("%CharacterName");
		public static readonly NodePath Copper = new("%Copper");
		public static readonly NodePath CurrentHP = new("%CurrentHP");
		public static readonly NodePath Electrum = new("%Electrum");
		public static readonly NodePath Flaws = new("%Flaws");
		public static readonly NodePath Gold = new("%Gold");
		public static readonly NodePath HPBar = new("%HPBar");
		public static readonly NodePath Ideals = new("%Ideals");
		public static readonly NodePath InitiativeBonus = new("%InitiativeBonus");
		public static readonly NodePath Inspiration = new("%Inspiration");
		public static readonly NodePath Inventory = new("%Inventory");
		public static readonly NodePath MaxHP = new("%MaxHP");
		public static readonly NodePath PersonalityTraits = new("%PersonalityTraits");
		public static readonly NodePath Platinum = new("%Platinum");
		public static readonly NodePath PlayerName = new("%PlayerName");
		public static readonly NodePath Race = new("%Race");
		public static readonly NodePath RaceFeatures = new("%Racial Features");
		public static readonly NodePath Silver = new("%Silver");
		public static readonly NodePath Speed = new("%Speed");
		public static readonly NodePath TempHP = new("%TempHP");
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
		metadataManager = GetNode<MetadataManager>(MetadataManager.NodePath);
		
		SheetData ??= new FifthAdventurer();
		
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
			.Initialize<AbilityRow>(SheetData.Abilities, changed_Ability);
		
		InitInventory(inventory, SheetData.Inventory, changed_Inventory);
		
		base._Ready();
		refreshFeatures();
		toggleBardicInspirationDie();
		updateCalculatedTraits();
	}
	
	protected static void InitBackgroundOptionsButton(BackgroundOptionsButton node, Background initialValue, OptionButton.ItemSelectedEventHandler handler)
	{
		if(node is not null)
		{
			if(initialValue is not null)
				node.SelectItemByText(initialValue.Name);
			
			node.ItemSelected += handler;
		}
	}
	
	protected static void InitClassOptionsButton(ClassOptionsButton node, Class initialValue, OptionButton.ItemSelectedEventHandler handler)
	{
		if(node is not null)
		{
			if(initialValue is not null)
				node.SelectItemByText(initialValue.Name);
			
			node.ItemSelected += handler;
		}
	}
	
	protected static void InitDieOptionsButton(DieOptionsButton node, Die initialValue, OptionButton.ItemSelectedEventHandler handler)
	{
		if(node is not null)
		{
			if(initialValue is not null)
				node.SelectItemByText(initialValue.ToString());
			
			node.ItemSelected += handler;
		}
	}
	
	protected void InitInventory(Inventory node, List<Item> initialValue, Inventory.ItemsChangedEventHandler handler)
	{
		if(node is not null)
		{
			if(SheetData.Abilities.Find(a => a.Name.Equals(Ability.Names.Strength)) is Ability strength)
				node.Strength = strength;
			
			if(SheetData.Abilities.Find(a => a.Name.Equals(Ability.Names.Dexterity)) is Ability dexterity)
				node.Dexterity = dexterity;
			
			if(initialValue is not null)
				node.Items = initialValue;
			//validate each item before adding
			
			/*
			if(metadataManager.Container is DndFifthContainer dfc)
				dfc.Items.Where(item => items.Find(i => i.Name.Equals(item.Name)).Any())
					.ToList()
					.ForEach(item => node.Items.Add(item));
			*/
			node.RegenerateItems();
			node.ItemsChanged += handler;
		}
	}
	
	protected static void InitRaceOptionsButton(RaceOptionsButton node, Race initialValue, OptionButton.ItemSelectedEventHandler handler)
	{
		if(node is not null)
		{
			if(initialValue is not null)
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
		
		collectAllFeatures().ForEach(f => f.NumericBonuses
			.FindAll(nb => nb.Type.Equals(NumericBonus.StatTypes.ArmorClass))
			.ForEach(nb => {
				if(!nb.Add)
					ac = nb.Value;
				else
					ac += nb.Value;
			})
		);
		
		return ac;
	}
	
	private int calculateInitiative()
	{
		var bonus = 0;
		if(SheetData.Abilities.Find(a => a.Name.Equals(Ability.Names.Dexterity)) is Ability dexterity)
			bonus += dexterity.Modifier;
		
		collectAllFeatures().ForEach(f => f.NumericBonuses
			.FindAll(nb => nb.Type.Equals(NumericBonus.StatTypes.Initiative))
			.ForEach(nb => {
				if(!nb.Add)
					bonus = nb.Value;
				else
					bonus += nb.Value;
			})
		);
		
		return bonus;
	}
	
	private int calculateSpeed()
	{
		var speed = 0;
		
		collectAllFeatures().ForEach(f => f.NumericBonuses
			.FindAll(nb => nb.Type.Equals(NumericBonus.StatTypes.Speed))
			.ForEach(nb => {
				if(!nb.Add)
					speed = nb.Value;
				else
					speed += nb.Value;
			})
		);
		
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
			
			inventory.RegenerateItems();
			updateCalculatedTraits();
		}
	}
	
	private void changed_Alignment(string newText) => SheetData.Alignment = newText;
	
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
		SheetData.BardicInspiration = button.State;
		toggleBardicInspirationDie();
	}
	
	private void changed_BardicInspirationDie(long index)
	{
		var text = bardicInspirationDie.GetItemText((int)index);
		if(!string.IsNullOrEmpty(text))
		{
			var die = new Die() { Sides = int.Parse(text[1..]) };
			SheetData.BardicInspirationDie = die;
		}
		else
		{
			SheetData.BardicInspirationDie = null;
			bardicInspirationDie.Deselect();
		}
	}
	
	private void changed_Bonds() => SheetData.Bonds = bonds.Text;
	
	private void changed_CharacterName(string newText)
	{
		SheetData.Name = newText;
		
		if(!string.IsNullOrEmpty(SheetData.Name))
			Name = SheetData.Name;
	}
	
	private void changed_Copper(double value) => SheetData.CoinPurse.Copper = (int)value;
	
	private void changed_CurrentHP(double value)
	{
		var hp = SheetData.HP;
		hp.Current = (int)value;
		SheetData.HP = hp;
	}
	
	private void changed_Electrum(double value) => SheetData.CoinPurse.Electrum = (int)value;
	private void changed_Flaws() => SheetData.Flaws = flaws.Text;
	private void changed_Gold(double value) => SheetData.CoinPurse.Gold = (int)value;
	private void changed_Ideals() => SheetData.Ideals = ideals.Text;
	private void changed_Inspiration(ToggleButton button) => SheetData.Inspiration = button.State;
	
	private void changed_Inventory(Transport<List<Item>> transport)
	{
		SheetData.Inventory = transport.Value;
		updateCalculatedTraits();
	}
	
	private void changed_MaxHP(double value)
	{
		var hp = SheetData.HP;
		hp.Max = (int)value;
		SheetData.HP = hp;
	}
	
	private void changed_PersonalityTraits() => SheetData.PersonalityTraits = personalityTraits.Text;
	private void changed_Platinum(double value) => SheetData.CoinPurse.Platinum = (int)value;
	private void changed_PlayerName(string newText) => SheetData.Player = newText;
	
	private void changed_Race(long index)
	{
		if(index > 0 && metadataManager.Container is DndFifthContainer dfc && dfc.Races[(int)index - 1] is Race race)
			SheetData.Race = race;
		else
			SheetData.Race = null;
		
		refreshFeatures();
	}
	
	private void changed_Silver(double value) => SheetData.CoinPurse.Silver = (int)value;
	
	private void changed_TempHP(double value)
	{
		var hp = SheetData.HP;
		hp.Temp = (int)value;
		SheetData.HP = hp;
	}
	
	private List<Fifth.Feature> collectAllFeatures()
	{
		List<Fifth.Feature> allFeatures = [];
		
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
		
		var resource = GD.Load<PackedScene>(ScenePaths.Dnd.Fifth.Feature);
		
		if(SheetData.Background is Background background && background.Features.Count != 0)
			background.Features.ForEach(f => renderFeature(backgroundFeatures, f, resource, f.Equals(background.Features.Last())));
		
		if(SheetData.Race is Race race && race.Features.Count != 0)
			race.Features.ForEach(f => renderFeature(raceFeatures, f, resource, f.Equals(race.Features.Last())));
		
		updateCalculatedTraits();
	}
	
	private static void renderFeature(Container node, Fifth.Feature feature, PackedScene resource, bool separator = false)
	{
		var instance = resource.Instantiate<Feature>();
		node.AddChild(instance);
		instance.Update(feature);
		
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
