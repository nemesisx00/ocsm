using Godot;
using System;
using OCSM.Nodes.Autoload;
using OCSM.DnD.Fifth;
using OCSM.DnD.Fifth.Meta;
using OCSM.Nodes.Meta;

namespace OCSM.Nodes.DnD.Fifth.Meta
{
	public sealed class ArmorEntry : Container
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
		public delegate void SaveClicked(Transport<InventoryArmor> armor);
		[Signal]
		public delegate void DeleteConfirmed(string name);
		
		[Export]
		public Script OptionsButtonScript { get; set; } = null;
		
		private MetadataManager metadataManager;
		
		public override void _Ready()
		{
			metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
			metadataManager.Connect(nameof(MetadataManager.MetadataSaved), this, nameof(refreshMetadata));
			metadataManager.Connect(nameof(MetadataManager.MetadataLoaded), this, nameof(refreshMetadata));
			
			GetNode<ArmorOptionsButton>(NodePathBuilder.SceneUnique(ExistingEntryName)).Connect(Constants.Signal.ItemSelected, this, nameof(entrySelected));
			
			GetNode<Button>(NodePathBuilder.SceneUnique(ClearButton)).Connect(Constants.Signal.Pressed, this, nameof(clearInputs));
			GetNode<Button>(NodePathBuilder.SceneUnique(SaveButton)).Connect(Constants.Signal.Pressed, this, nameof(doSave));
			GetNode<Button>(NodePathBuilder.SceneUnique(DeleteButton)).Connect(Constants.Signal.Pressed, this, nameof(handleDelete));
			
			GetNode<CheckBox>(NodePathBuilder.SceneUnique(AllowDexterityBonus)).Connect(Constants.Signal.Pressed, this, nameof(toggleLimitDexterity));
			GetNode<CheckBox>(NodePathBuilder.SceneUnique(LimitDexterityBonus)).Connect(Constants.Signal.Pressed, this, nameof(toggleDexterityLimit));
			GetNode<CheckBox>(NodePathBuilder.SceneUnique(ShowStrengthCheck)).Connect(Constants.Signal.Pressed, this, nameof(toggleMinimumStrengthInput));
			
			refreshMetadata();
		}
		
		private void entrySelected(int index)
		{
			var optionsButton = GetNode<ArmorOptionsButton>(NodePathBuilder.SceneUnique(ExistingEntryName));
			var name = optionsButton.GetItemText(index);
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				if(dfc.Armor.Find(a => a.Name.Equals(name)) is InventoryArmor armor)
				{
					loadEntry(armor);
					optionsButton.Selected = 0;
				}
			}
		}
		
		public void loadEntry(InventoryArmor entry)
		{
			System.Console.WriteLine("Entry to be laoded: {0}", entry);
			GetNode<LineEdit>(NodePathBuilder.SceneUnique(NameInput)).Text = entry.Name;
			GetNode<OptionButton>(NodePathBuilder.SceneUnique(TypeInput)).Selected = (int)entry.Type;
			GetNode<SpinBox>(NodePathBuilder.SceneUnique(ArmorClassInput)).Value = entry.BaseArmorClass;
			GetNode<SpinBox>(NodePathBuilder.SceneUnique(CostInput)).Value = entry.Cost;
			GetNode<SpinBox>(NodePathBuilder.SceneUnique(WeightInput)).Value = entry.Weight;
			GetNode<CheckBox>(NodePathBuilder.SceneUnique(AllowDexterityBonus)).Pressed = entry.AllowDexterityBonus;
			GetNode<CheckBox>(NodePathBuilder.SceneUnique(LimitDexterityBonus)).Pressed = entry.LimitDexterityBonus;
			GetNode<SpinBox>(NodePathBuilder.SceneUnique(DexterityBonusLimit)).Value = entry.DexterityBonusLimit;
			GetNode<CheckBox>(NodePathBuilder.SceneUnique(StealthDisadvantageInput)).Pressed = entry.StealthDisadvantage;
			GetNode<CheckBox>(NodePathBuilder.SceneUnique(ShowStrengthCheck)).Pressed = entry.MinimumStrength > 0;
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
				foreach(var a in dfc.Armor)
				{
					optionButton.AddItem(a.Name);
				}
			}
		}
		
		private void clearInputs()
		{
			GetNode<LineEdit>(NodePathBuilder.SceneUnique(NameInput)).Text = String.Empty;
			GetNode<OptionButton>(NodePathBuilder.SceneUnique(TypeInput)).Selected = 0;
			GetNode<SpinBox>(NodePathBuilder.SceneUnique(ArmorClassInput)).Value = 0;
			GetNode<SpinBox>(NodePathBuilder.SceneUnique(CostInput)).Value = 0;
			GetNode<SpinBox>(NodePathBuilder.SceneUnique(WeightInput)).Value = 0.0;
			GetNode<CheckBox>(NodePathBuilder.SceneUnique(AllowDexterityBonus)).Pressed = false;
			GetNode<CheckBox>(NodePathBuilder.SceneUnique(LimitDexterityBonus)).Pressed = false;
			GetNode<SpinBox>(NodePathBuilder.SceneUnique(DexterityBonusLimit)).Value = 0;
			GetNode<CheckBox>(NodePathBuilder.SceneUnique(StealthDisadvantageInput)).Pressed = false;
			GetNode<CheckBox>(NodePathBuilder.SceneUnique(ShowStrengthCheck)).Pressed = false;
			GetNode<SpinBox>(NodePathBuilder.SceneUnique(MinimumStrengthInput)).Value = 0;
			GetNode<AutosizeTextEdit>(NodePathBuilder.SceneUnique(DescriptionInput)).Text = String.Empty;
			
			toggleLimitDexterity();
			toggleDexterityLimit();
			toggleMinimumStrengthInput();
			NodeUtilities.autoSizeChildren(this, Constants.TextInputMinHeight);
		}
		
		private void doDelete()
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
			var type = (ArmorType)GetNode<OptionButton>(NodePathBuilder.SceneUnique(TypeInput)).Selected;
			var ac = (int)GetNode<SpinBox>(NodePathBuilder.SceneUnique(ArmorClassInput)).Value;
			var cost = (int)GetNode<SpinBox>(NodePathBuilder.SceneUnique(CostInput)).Value;
			var weight = GetNode<SpinBox>(NodePathBuilder.SceneUnique(WeightInput)).Value;
			var allowDex = GetNode<CheckBox>(NodePathBuilder.SceneUnique(AllowDexterityBonus)).Pressed;
			var limitDex = GetNode<CheckBox>(NodePathBuilder.SceneUnique(LimitDexterityBonus)).Pressed;
			var dexLimit = (int)GetNode<SpinBox>(NodePathBuilder.SceneUnique(DexterityBonusLimit)).Value;
			var stealth = GetNode<CheckBox>(NodePathBuilder.SceneUnique(StealthDisadvantageInput)).Pressed;
			var minStr = (int)GetNode<SpinBox>(NodePathBuilder.SceneUnique(MinimumStrengthInput)).Value;
			var description = GetNode<AutosizeTextEdit>(NodePathBuilder.SceneUnique(DescriptionInput)).Text;
			
			var armor = new InventoryArmor(name, description);
			armor.AllowDexterityBonus = allowDex;
			armor.BaseArmorClass = ac;
			armor.Cost = cost;
			armor.DexterityBonusLimit = dexLimit;
			armor.LimitDexterityBonus = limitDex;
			armor.MinimumStrength = minStr;
			armor.StealthDisadvantage = stealth;
			armor.Type = type;
			armor.Weight = weight;
			
			EmitSignal(nameof(SaveClicked), new Transport<InventoryArmor>(armor));
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
			limit.GetParent<HBoxContainer>().Visible = allow.Pressed;
			toggleDexterityLimit();
		}
		
		private void toggleDexterityLimit()
		{
			var checkbox = GetNode<CheckBox>(NodePathBuilder.SceneUnique(LimitDexterityBonus));
			var input = GetNode<SpinBox>(NodePathBuilder.SceneUnique(DexterityBonusLimit));
			input.Visible = checkbox.Pressed;
		}
		
		private void toggleMinimumStrengthInput()
		{
			var checkbox = GetNode<CheckBox>(NodePathBuilder.SceneUnique(ShowStrengthCheck));
			var input = GetNode<SpinBox>(NodePathBuilder.SceneUnique(MinimumStrengthInput));
			input.Visible = checkbox.Pressed;
		}
	}
}
