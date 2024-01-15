using Godot;
using System;
using Ocsm.Nodes.Autoload;
using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Dnd.Fifth.Inventory;
using Ocsm.Nodes;

namespace Ocsm.Dnd.Fifth.Nodes;

public sealed partial class ArmorEntry : Container, ICanDelete
{
	private sealed class NodePaths
	{
		public static readonly NodePath AllowDexterityBonus = new("%AllowDexterityBonus");
		public static readonly NodePath ArmorClassInput = new("%ArmorClass");
		public static readonly NodePath ClearButton = new("%Clear");
		public static readonly NodePath CostInput = new("%Cost");
		public static readonly NodePath DescriptionInput = new("%Description");
		public static readonly NodePath DeleteButton = new("%Delete");
		public static readonly NodePath DexterityBonusLimit = new("%DexterityBonusLimit");
		public static readonly NodePath ExistingEntryName = new("%ExistingEntry");
		public static readonly NodePath LimitDexterityBonus = new("%LimitDexterityBonus");
		public static readonly NodePath MinimumStrengthInput = new("%MinimumStrength");
		public static readonly NodePath NameInput = new("%Name");
		public static readonly NodePath SaveButton = new("%Save");
		public static readonly NodePath ShowStrengthCheck = new("%ShowStrength");
		public static readonly NodePath StealthDisadvantageInput = new("%StealthDisadvantage");
		public static readonly NodePath TypeInput = new("%Type");
		public static readonly NodePath WeightInput = new("%Weight");
	}
	
	public const string MetadataLabel = "Armor";
	
	[Signal]
	public delegate void SaveClickedEventHandler(Transport<ItemArmor> armor);
	[Signal]
	public delegate void DeleteConfirmedEventHandler(string name);
	
	[Export]
	public Script OptionsButtonScript { get; set; }
	
	private MetadataManager metadataManager;
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(MetadataManager.NodePath);
		metadataManager.MetadataLoaded += RefreshMetadata;
		metadataManager.MetadataSaved += RefreshMetadata;
		
		GetNode<ArmorOptionsButton>(NodePaths.ExistingEntryName).ItemSelected += entrySelected;
		
		GetNode<Button>(NodePaths.ClearButton).Pressed += clearInputs;
		GetNode<Button>(NodePaths.SaveButton).Pressed += doSave;
		GetNode<Button>(NodePaths.DeleteButton).Pressed += handleDelete;
		
		GetNode<CheckBox>(NodePaths.AllowDexterityBonus).Pressed += toggleLimitDexterity;
		GetNode<CheckBox>(NodePaths.LimitDexterityBonus).Pressed += toggleDexterityLimit;
		GetNode<CheckBox>(NodePaths.ShowStrengthCheck).Pressed += toggleMinimumStrengthInput;
		
		RefreshMetadata();
	}
	
	public void LoadEntry(ItemArmor entry)
	{
		GetNode<LineEdit>(NodePaths.NameInput).Text = entry.Name;
		GetNode<OptionButton>(NodePaths.TypeInput).SelectItemByText(entry.Type.GetLabel());
		GetNode<SpinBox>(NodePaths.ArmorClassInput).Value = entry.BaseArmorClass;
		GetNode<SpinBox>(NodePaths.CostInput).Value = entry.Cost;
		GetNode<SpinBox>(NodePaths.WeightInput).Value = entry.Weight;
		GetNode<CheckBox>(NodePaths.AllowDexterityBonus).ButtonPressed = entry.AllowDexterityBonus;
		GetNode<CheckBox>(NodePaths.LimitDexterityBonus).ButtonPressed = entry.LimitDexterityBonus;
		GetNode<SpinBox>(NodePaths.DexterityBonusLimit).Value = entry.DexterityBonusLimit;
		GetNode<CheckBox>(NodePaths.StealthDisadvantageInput).ButtonPressed = entry.StealthDisadvantage;
		GetNode<CheckBox>(NodePaths.ShowStrengthCheck).ButtonPressed = entry.MinimumStrength > 0;
		GetNode<SpinBox>(NodePaths.MinimumStrengthInput).Value = entry.MinimumStrength;
		GetNode<TextEdit>(NodePaths.DescriptionInput).Text = entry.Description;
		
		toggleLimitDexterity();
		toggleDexterityLimit();
		toggleMinimumStrengthInput();
	}
	
	public void RefreshMetadata()
	{
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			var optionButton = GetNode<ArmorOptionsButton>(NodePaths.ExistingEntryName);
			optionButton.Clear();
			optionButton.AddItem(string.Empty);
			dfc.Armors.ForEach(a => optionButton.AddItem(a.Name));
		}
	}
	
	public void DoDelete()
	{
		var name = GetNode<LineEdit>(NodePaths.NameInput).Text;
		if(!string.IsNullOrEmpty(name))
		{
			EmitSignal(SignalName.DeleteConfirmed, name);
			clearInputs();
		}
		//TODO: Display error message if name is empty
	}
	
	private void clearInputs()
	{
		GetNode<LineEdit>(NodePaths.NameInput).Text = string.Empty;
		GetNode<OptionButton>(NodePaths.TypeInput).Deselect();
		GetNode<SpinBox>(NodePaths.ArmorClassInput).Value = 0;
		GetNode<SpinBox>(NodePaths.CostInput).Value = 0;
		GetNode<SpinBox>(NodePaths.WeightInput).Value = 0.0;
		GetNode<CheckBox>(NodePaths.AllowDexterityBonus).ButtonPressed = false;
		GetNode<CheckBox>(NodePaths.LimitDexterityBonus).ButtonPressed = false;
		GetNode<SpinBox>(NodePaths.DexterityBonusLimit).Value = 0;
		GetNode<CheckBox>(NodePaths.StealthDisadvantageInput).ButtonPressed = false;
		GetNode<CheckBox>(NodePaths.ShowStrengthCheck).ButtonPressed = false;
		GetNode<SpinBox>(NodePaths.MinimumStrengthInput).Value = 0;
		GetNode<TextEdit>(NodePaths.DescriptionInput).Text = string.Empty;
		
		toggleLimitDexterity();
		toggleDexterityLimit();
		toggleMinimumStrengthInput();
	}
	
	private void doSave()
	{
		var name = GetNode<LineEdit>(NodePaths.NameInput).Text;
		var type = (ItemArmor.ArmorType)GetNode<OptionButton>(NodePaths.TypeInput).Selected;
		var ac = (int)GetNode<SpinBox>(NodePaths.ArmorClassInput).Value;
		var cost = (int)GetNode<SpinBox>(NodePaths.CostInput).Value;
		var weight = GetNode<SpinBox>(NodePaths.WeightInput).Value;
		var allowDex = GetNode<CheckBox>(NodePaths.AllowDexterityBonus).ButtonPressed;
		var limitDex = GetNode<CheckBox>(NodePaths.LimitDexterityBonus).ButtonPressed;
		var dexLimit = (int)GetNode<SpinBox>(NodePaths.DexterityBonusLimit).Value;
		var stealth = GetNode<CheckBox>(NodePaths.StealthDisadvantageInput).ButtonPressed;
		var minStr = (int)GetNode<SpinBox>(NodePaths.MinimumStrengthInput).Value;
		var description = GetNode<TextEdit>(NodePaths.DescriptionInput).Text;
		
		var armor = new ItemArmor()
		{
			AllowDexterityBonus = allowDex,
			BaseArmorClass = ac,
			Cost = cost,
			DexterityBonusLimit = dexLimit,
			Description = description,
			LimitDexterityBonus = limitDex,
			MinimumStrength = minStr,
			Name = name,
			StealthDisadvantage = stealth,
			Type = type,
			Weight = weight,
		};
		
		EmitSignal(SignalName.SaveClicked, new Transport<ItemArmor>(armor));
		clearInputs();
	}
	
	private void entrySelected(long index)
	{
		var optionsButton = GetNode<ArmorOptionsButton>(NodePaths.ExistingEntryName);
		var name = optionsButton.GetItemText((int)index);
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			if(dfc.Armors.Find(a => a.Name.Equals(name)) is ItemArmor armor)
			{
				LoadEntry(armor);
				optionsButton.Deselect();
			}
		}
	}
	
	private void handleDelete() => NodeUtilities.DisplayDeleteConfirmation(
		MetadataLabel,
		GetTree().CurrentScene,
		this
	);
	
	private void toggleLimitDexterity()
	{
		var allow = GetNode<CheckBox>(NodePaths.AllowDexterityBonus);
		var limit = GetNode<CheckBox>(NodePaths.LimitDexterityBonus);
		limit.GetParent<HBoxContainer>().Visible = allow.ButtonPressed;
		toggleDexterityLimit();
	}
	
	private void toggleDexterityLimit()
	{
		var checkbox = GetNode<CheckBox>(NodePaths.LimitDexterityBonus);
		var input = GetNode<SpinBox>(NodePaths.DexterityBonusLimit);
		input.Visible = checkbox.ButtonPressed;
	}
	
	private void toggleMinimumStrengthInput()
	{
		var checkbox = GetNode<CheckBox>(NodePaths.ShowStrengthCheck);
		var input = GetNode<SpinBox>(NodePaths.MinimumStrengthInput);
		input.Visible = checkbox.ButtonPressed;
	}
}
