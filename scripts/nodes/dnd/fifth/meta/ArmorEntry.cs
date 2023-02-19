using Godot;
using System;
using OCSM.Nodes.Autoload;
using OCSM.DnD.Fifth.Meta;
using OCSM.DnD.Fifth.Inventory;

namespace OCSM.Nodes.DnD.Fifth.Meta
{
	public sealed partial class ArmorEntry : Container, ICanDelete
	{
		private const string MetadataLabel = "Armor";
		private const string AllowDexterityBonus = "AllowDexterityBonus";
		private const string ArmorClassInput = "ArmorClass";
		private const string ClearButton = "Clear";
		private const string CostInput = "Cost";
		private const string DescriptionInput = "Description";
		private const string DeleteButton = "Delete";
		private const string DexterityBonusLimit = "DexterityBonusLimit";
		private const string ExistingEntryName = "ExistingEntry";
		private const string LimitDexterityBonus = "LimitDexterityBonus";
		private const string MinimumStrengthInput = "MinimumStrength";
		private const string NameInput = "Name";
		private const string SaveButton = "Save";
		private const string ShowStrengthCheck = "ShowStrength";
		private const string StealthDisadvantageInput = "StealthDisadvantage";
		private const string TypeInput = "Type";
		private const string WeightInput = "Weight";
		
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
			
			GetNode<ArmorOptionsButton>(NodePathBuilder.SceneUnique(ExistingEntryName)).Connect(Constants.Signal.ItemSelected,new Callable(this,nameof(entrySelected)));
			
			GetNode<Button>(NodePathBuilder.SceneUnique(ClearButton)).Connect(Constants.Signal.Pressed,new Callable(this,nameof(clearInputs)));
			GetNode<Button>(NodePathBuilder.SceneUnique(SaveButton)).Connect(Constants.Signal.Pressed,new Callable(this,nameof(doSave)));
			GetNode<Button>(NodePathBuilder.SceneUnique(DeleteButton)).Connect(Constants.Signal.Pressed,new Callable(this,nameof(handleDelete)));
			
			GetNode<CheckBox>(NodePathBuilder.SceneUnique(AllowDexterityBonus)).Connect(Constants.Signal.Pressed,new Callable(this,nameof(toggleLimitDexterity)));
			GetNode<CheckBox>(NodePathBuilder.SceneUnique(LimitDexterityBonus)).Connect(Constants.Signal.Pressed,new Callable(this,nameof(toggleDexterityLimit)));
			GetNode<CheckBox>(NodePathBuilder.SceneUnique(ShowStrengthCheck)).Connect(Constants.Signal.Pressed,new Callable(this,nameof(toggleMinimumStrengthInput)));
			
			refreshMetadata();
		}
		
		private void entrySelected(int index)
		{
			var optionsButton = GetNode<ArmorOptionsButton>(NodePathBuilder.SceneUnique(ExistingEntryName));
			var name = optionsButton.GetItemText(index);
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				if(dfc.Armors.Find(a => a.Name.Equals(name)) is ItemArmor armor)
				{
					loadEntry(armor);
					optionsButton.Selected = 0;
				}
			}
		}
		
		public void loadEntry(ItemArmor entry)
		{
			GetNode<LineEdit>(NodePathBuilder.SceneUnique(NameInput)).Text = entry.Name;
			GetNode<OptionButton>(NodePathBuilder.SceneUnique(TypeInput)).Selected = (int)entry.Type;
			GetNode<SpinBox>(NodePathBuilder.SceneUnique(ArmorClassInput)).Value = entry.BaseArmorClass;
			GetNode<SpinBox>(NodePathBuilder.SceneUnique(CostInput)).Value = entry.Cost;
			GetNode<SpinBox>(NodePathBuilder.SceneUnique(WeightInput)).Value = entry.Weight;
			GetNode<CheckBox>(NodePathBuilder.SceneUnique(AllowDexterityBonus)).ButtonPressed = entry.AllowDexterityBonus;
			GetNode<CheckBox>(NodePathBuilder.SceneUnique(LimitDexterityBonus)).ButtonPressed = entry.LimitDexterityBonus;
			GetNode<SpinBox>(NodePathBuilder.SceneUnique(DexterityBonusLimit)).Value = entry.DexterityBonusLimit;
			GetNode<CheckBox>(NodePathBuilder.SceneUnique(StealthDisadvantageInput)).ButtonPressed = entry.StealthDisadvantage;
			GetNode<CheckBox>(NodePathBuilder.SceneUnique(ShowStrengthCheck)).ButtonPressed = entry.MinimumStrength > 0;
			GetNode<SpinBox>(NodePathBuilder.SceneUnique(MinimumStrengthInput)).Value = entry.MinimumStrength;
			GetNode<AutosizeTextEdit>(NodePathBuilder.SceneUnique(DescriptionInput)).Text = entry.Description;
			
			toggleLimitDexterity();
			toggleDexterityLimit();
			toggleMinimumStrengthInput();
			NodeUtilities.autoSizeChildren(this, Constants.TextInputMinHeight);
		}
		
		public void refreshMetadata()
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				var optionButton = GetNode<ArmorOptionsButton>(NodePathBuilder.SceneUnique(ExistingEntryName));
				optionButton.Clear();
				optionButton.AddItem("");
				dfc.Armors.ForEach(a => optionButton.AddItem(a.Name));
			}
		}
		
		private void clearInputs()
		{
			GetNode<LineEdit>(NodePathBuilder.SceneUnique(NameInput)).Text = String.Empty;
			GetNode<OptionButton>(NodePathBuilder.SceneUnique(TypeInput)).Selected = 0;
			GetNode<SpinBox>(NodePathBuilder.SceneUnique(ArmorClassInput)).Value = 0;
			GetNode<SpinBox>(NodePathBuilder.SceneUnique(CostInput)).Value = 0;
			GetNode<SpinBox>(NodePathBuilder.SceneUnique(WeightInput)).Value = 0.0;
			GetNode<CheckBox>(NodePathBuilder.SceneUnique(AllowDexterityBonus)).ButtonPressed = false;
			GetNode<CheckBox>(NodePathBuilder.SceneUnique(LimitDexterityBonus)).ButtonPressed = false;
			GetNode<SpinBox>(NodePathBuilder.SceneUnique(DexterityBonusLimit)).Value = 0;
			GetNode<CheckBox>(NodePathBuilder.SceneUnique(StealthDisadvantageInput)).ButtonPressed = false;
			GetNode<CheckBox>(NodePathBuilder.SceneUnique(ShowStrengthCheck)).ButtonPressed = false;
			GetNode<SpinBox>(NodePathBuilder.SceneUnique(MinimumStrengthInput)).Value = 0;
			GetNode<AutosizeTextEdit>(NodePathBuilder.SceneUnique(DescriptionInput)).Text = String.Empty;
			
			toggleLimitDexterity();
			toggleDexterityLimit();
			toggleMinimumStrengthInput();
			NodeUtilities.autoSizeChildren(this, Constants.TextInputMinHeight);
		}
		
		public void doDelete()
		{
			var name = GetNode<LineEdit>(NodePathBuilder.SceneUnique(NameInput)).Text;
			if(!String.IsNullOrEmpty(name))
			{
				EmitSignal(nameof(DeleteConfirmed), name);
				clearInputs();
			}
			//TODO: Display error message if name is empty
		}
		
		private void doSave()
		{
			var name = GetNode<LineEdit>(NodePathBuilder.SceneUnique(NameInput)).Text;
			var type = (ItemArmor.ArmorType)GetNode<OptionButton>(NodePathBuilder.SceneUnique(TypeInput)).Selected;
			var ac = (int)GetNode<SpinBox>(NodePathBuilder.SceneUnique(ArmorClassInput)).Value;
			var cost = (int)GetNode<SpinBox>(NodePathBuilder.SceneUnique(CostInput)).Value;
			var weight = GetNode<SpinBox>(NodePathBuilder.SceneUnique(WeightInput)).Value;
			var allowDex = GetNode<CheckBox>(NodePathBuilder.SceneUnique(AllowDexterityBonus)).ButtonPressed;
			var limitDex = GetNode<CheckBox>(NodePathBuilder.SceneUnique(LimitDexterityBonus)).ButtonPressed;
			var dexLimit = (int)GetNode<SpinBox>(NodePathBuilder.SceneUnique(DexterityBonusLimit)).Value;
			var stealth = GetNode<CheckBox>(NodePathBuilder.SceneUnique(StealthDisadvantageInput)).ButtonPressed;
			var minStr = (int)GetNode<SpinBox>(NodePathBuilder.SceneUnique(MinimumStrengthInput)).Value;
			var description = GetNode<AutosizeTextEdit>(NodePathBuilder.SceneUnique(DescriptionInput)).Text;
			
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
				nameof(doDelete)
			);
		}
		
		private void toggleLimitDexterity()
		{
			var allow = GetNode<CheckBox>(NodePathBuilder.SceneUnique(AllowDexterityBonus));
			var limit = GetNode<CheckBox>(NodePathBuilder.SceneUnique(LimitDexterityBonus));
			limit.GetParent<HBoxContainer>().Visible = allow.ButtonPressed;
			toggleDexterityLimit();
		}
		
		private void toggleDexterityLimit()
		{
			var checkbox = GetNode<CheckBox>(NodePathBuilder.SceneUnique(LimitDexterityBonus));
			var input = GetNode<SpinBox>(NodePathBuilder.SceneUnique(DexterityBonusLimit));
			input.Visible = checkbox.ButtonPressed;
		}
		
		private void toggleMinimumStrengthInput()
		{
			var checkbox = GetNode<CheckBox>(NodePathBuilder.SceneUnique(ShowStrengthCheck));
			var input = GetNode<SpinBox>(NodePathBuilder.SceneUnique(MinimumStrengthInput));
			input.Visible = checkbox.ButtonPressed;
		}
	}
}
