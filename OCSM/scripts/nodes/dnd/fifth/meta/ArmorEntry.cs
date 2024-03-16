using Godot;
using System;
using Ocsm.Nodes.Autoload;
using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Dnd.Fifth.Inventory;

namespace Ocsm.Nodes.Dnd.Fifth.Meta;

public sealed partial class ArmorEntry : Container, ICanDelete
{
	private sealed class NodePath
	{
		public const string AllowDexterityBonus = "%AllowDexterityBonus";
		public const string ArmorClassInput = "%ArmorClass";
		public const string ClearButton = "%Clear";
		public const string CostInput = "%Cost";
		public const string DescriptionInput = "%Description";
		public const string DeleteButton = "%Delete";
		public const string DexterityBonusLimit = "%DexterityBonusLimit";
		public const string ExistingEntryName = "%ExistingEntry";
		public const string LimitDexterityBonus = "%LimitDexterityBonus";
		public const string MinimumStrengthInput = "%MinimumStrength";
		public const string NameInput = "%Name";
		public const string SaveButton = "%Save";
		public const string ShowStrengthCheck = "%ShowStrength";
		public const string StealthDisadvantageInput = "%StealthDisadvantage";
		public const string TypeInput = "%Type";
		public const string WeightInput = "%Weight";
	}
	
	public const string MetadataLabel = "Armor";
	
	[Signal]
	public delegate void SaveClickedEventHandler(Transport<ItemArmor> armor);
	[Signal]
	public delegate void DeleteConfirmedEventHandler(string name);
	
	[Export]
	public Script OptionsButtonScript { get; set; } = null;
	
	private MetadataManager metadataManager;
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
		metadataManager.MetadataLoaded += refreshMetadata;
		metadataManager.MetadataSaved += refreshMetadata;
		
		GetNode<ArmorOptionsButton>(NodePath.ExistingEntryName).ItemSelected += entrySelected;
		
		GetNode<Button>(NodePath.ClearButton).Pressed += clearInputs;
		GetNode<Button>(NodePath.SaveButton).Pressed += doSave;
		GetNode<Button>(NodePath.DeleteButton).Pressed += handleDelete;
		
		GetNode<CheckBox>(NodePath.AllowDexterityBonus).Pressed += toggleLimitDexterity;
		GetNode<CheckBox>(NodePath.LimitDexterityBonus).Pressed += toggleDexterityLimit;
		GetNode<CheckBox>(NodePath.ShowStrengthCheck).Pressed += toggleMinimumStrengthInput;
		
		refreshMetadata();
	}
	
	private void entrySelected(long index)
	{
		var optionsButton = GetNode<ArmorOptionsButton>(NodePath.ExistingEntryName);
		var name = optionsButton.GetItemText((int)index);
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			if(dfc.Armors.Find(a => a.Name.Equals(name)) is ItemArmor armor)
			{
				loadEntry(armor);
				optionsButton.Deselect();
			}
		}
	}
	
	public void loadEntry(ItemArmor entry)
	{
		GetNode<LineEdit>(NodePath.NameInput).Text = entry.Name;
		GetNode<OptionButton>(NodePath.TypeInput).SelectItemByText(entry.ArmorType.GetLabel());
		GetNode<SpinBox>(NodePath.ArmorClassInput).Value = entry.BaseArmorClass;
		GetNode<SpinBox>(NodePath.CostInput).Value = entry.Cost;
		GetNode<SpinBox>(NodePath.WeightInput).Value = entry.Weight;
		GetNode<CheckBox>(NodePath.AllowDexterityBonus).ButtonPressed = entry.AllowDexterityBonus;
		GetNode<CheckBox>(NodePath.LimitDexterityBonus).ButtonPressed = entry.LimitDexterityBonus;
		GetNode<SpinBox>(NodePath.DexterityBonusLimit).Value = entry.DexterityBonusLimit;
		GetNode<CheckBox>(NodePath.StealthDisadvantageInput).ButtonPressed = entry.StealthDisadvantage;
		GetNode<CheckBox>(NodePath.ShowStrengthCheck).ButtonPressed = entry.MinimumStrength > 0;
		GetNode<SpinBox>(NodePath.MinimumStrengthInput).Value = entry.MinimumStrength;
		GetNode<TextEdit>(NodePath.DescriptionInput).Text = entry.Description;
		
		toggleLimitDexterity();
		toggleDexterityLimit();
		toggleMinimumStrengthInput();
	}
	
	public void refreshMetadata()
	{
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			var optionButton = GetNode<ArmorOptionsButton>(NodePath.ExistingEntryName);
			optionButton.Clear();
			optionButton.AddItem("");
			dfc.Armors.ForEach(a => optionButton.AddItem(a.Name));
		}
	}
	
	private void clearInputs()
	{
		GetNode<LineEdit>(NodePath.NameInput).Text = String.Empty;
		GetNode<OptionButton>(NodePath.TypeInput).Deselect();
		GetNode<SpinBox>(NodePath.ArmorClassInput).Value = 0;
		GetNode<SpinBox>(NodePath.CostInput).Value = 0;
		GetNode<SpinBox>(NodePath.WeightInput).Value = 0.0;
		GetNode<CheckBox>(NodePath.AllowDexterityBonus).ButtonPressed = false;
		GetNode<CheckBox>(NodePath.LimitDexterityBonus).ButtonPressed = false;
		GetNode<SpinBox>(NodePath.DexterityBonusLimit).Value = 0;
		GetNode<CheckBox>(NodePath.StealthDisadvantageInput).ButtonPressed = false;
		GetNode<CheckBox>(NodePath.ShowStrengthCheck).ButtonPressed = false;
		GetNode<SpinBox>(NodePath.MinimumStrengthInput).Value = 0;
		GetNode<TextEdit>(NodePath.DescriptionInput).Text = String.Empty;
		
		toggleLimitDexterity();
		toggleDexterityLimit();
		toggleMinimumStrengthInput();
	}
	
	public void DoDelete()
	{
		var name = GetNode<LineEdit>(NodePath.NameInput).Text;
		if(!String.IsNullOrEmpty(name))
		{
			EmitSignal(nameof(DeleteConfirmed), name);
			clearInputs();
		}
		//TODO: Display error message if name is empty
	}
	
	private void doSave()
	{
		var name = GetNode<LineEdit>(NodePath.NameInput).Text;
		var type = (ItemArmor.ArmorTypes)GetNode<OptionButton>(NodePath.TypeInput).Selected;
		var ac = (int)GetNode<SpinBox>(NodePath.ArmorClassInput).Value;
		var cost = (int)GetNode<SpinBox>(NodePath.CostInput).Value;
		var weight = GetNode<SpinBox>(NodePath.WeightInput).Value;
		var allowDex = GetNode<CheckBox>(NodePath.AllowDexterityBonus).ButtonPressed;
		var limitDex = GetNode<CheckBox>(NodePath.LimitDexterityBonus).ButtonPressed;
		var dexLimit = (int)GetNode<SpinBox>(NodePath.DexterityBonusLimit).Value;
		var stealth = GetNode<CheckBox>(NodePath.StealthDisadvantageInput).ButtonPressed;
		var minStr = (int)GetNode<SpinBox>(NodePath.MinimumStrengthInput).Value;
		var description = GetNode<TextEdit>(NodePath.DescriptionInput).Text;
		
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
			ArmorType = type,
			Weight = weight,
		};
		
		EmitSignal(nameof(SaveClicked), new Transport<ItemArmor>(armor));
		clearInputs();
	}
	
	private void handleDelete()
	{
		NodeUtilities.displayDeleteConfirmation(
			MetadataLabel,
			GetTree().CurrentScene,
			GetViewportRect().GetCenter(),
			this,
			nameof(DoDelete)
		);
	}
	
	private void toggleLimitDexterity()
	{
		var allow = GetNode<CheckBox>(NodePath.AllowDexterityBonus);
		var limit = GetNode<CheckBox>(NodePath.LimitDexterityBonus);
		limit.GetParent<HBoxContainer>().Visible = allow.ButtonPressed;
		toggleDexterityLimit();
	}
	
	private void toggleDexterityLimit()
	{
		var checkbox = GetNode<CheckBox>(NodePath.LimitDexterityBonus);
		var input = GetNode<SpinBox>(NodePath.DexterityBonusLimit);
		input.Visible = checkbox.ButtonPressed;
	}
	
	private void toggleMinimumStrengthInput()
	{
		var checkbox = GetNode<CheckBox>(NodePath.ShowStrengthCheck);
		var input = GetNode<SpinBox>(NodePath.MinimumStrengthInput);
		input.Visible = checkbox.ButtonPressed;
	}
}
