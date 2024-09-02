using Godot;
using Ocsm.Nodes;
using Ocsm.Nodes.Autoload;
using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Dnd.Fifth.Inventory;
using System.Linq;

namespace Ocsm.Dnd.Fifth.Nodes.Meta;

public partial class ArmorEntry : Container, ICanDelete
{
	private static class NodePaths
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
	public delegate void SaveClickedEventHandler(Transport<Item> armor);
	[Signal]
	public delegate void DeleteConfirmedEventHandler(string name);
	
	[Export]
	public Script OptionsButtonScript { get; set; }
	
	private MetadataManager metadataManager;
	
	private CheckBox allowDexterityBonus;
	private CheckBox showStrengthCheck;
	private SpinBox minimumStrengthInput;
	private CheckBox limitDexterityBonus;
	private SpinBox dexterityBonusLimit;
	private LineEdit nameInput;
	private OptionButton typeInput;
	private SpinBox armorClassInput;
	private SpinBox costInput;
	private SpinBox weightInput;
	private CheckBox stealthDisadvantageInput;
	private TextEdit descriptionInput;
	private ArmorOptionsButton armorOptionsButton;
	
	public override void _ExitTree()
	{
		metadataManager.MetadataLoaded -= RefreshMetadata;
		metadataManager.MetadataSaved -= RefreshMetadata;
		
		base._ExitTree();
	}
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(MetadataManager.NodePath);
		metadataManager.MetadataLoaded += RefreshMetadata;
		metadataManager.MetadataSaved += RefreshMetadata;
		
		allowDexterityBonus = GetNode<CheckBox>(NodePaths.AllowDexterityBonus);
		showStrengthCheck = GetNode<CheckBox>(NodePaths.ShowStrengthCheck);
		minimumStrengthInput = GetNode<SpinBox>(NodePaths.MinimumStrengthInput);
		limitDexterityBonus = GetNode<CheckBox>(NodePaths.LimitDexterityBonus);
		dexterityBonusLimit = GetNode<SpinBox>(NodePaths.DexterityBonusLimit);
		nameInput = GetNode<LineEdit>(NodePaths.NameInput);
		typeInput = GetNode<OptionButton>(NodePaths.TypeInput);
		armorClassInput = GetNode<SpinBox>(NodePaths.ArmorClassInput);
		costInput = GetNode<SpinBox>(NodePaths.CostInput);
		weightInput = GetNode<SpinBox>(NodePaths.WeightInput);
		stealthDisadvantageInput = GetNode<CheckBox>(NodePaths.StealthDisadvantageInput);
		descriptionInput = GetNode<TextEdit>(NodePaths.DescriptionInput);
		armorOptionsButton = GetNode<ArmorOptionsButton>(NodePaths.ExistingEntryName);
		
		GetNode<Button>(NodePaths.ClearButton).Pressed += clearInputs;
		GetNode<Button>(NodePaths.SaveButton).Pressed += doSave;
		GetNode<Button>(NodePaths.DeleteButton).Pressed += handleDelete;
		
		armorOptionsButton.ItemSelected += entrySelected;
		allowDexterityBonus.Pressed += toggleLimitDexterity;
		limitDexterityBonus.Pressed += toggleDexterityLimit;
		showStrengthCheck.Pressed += toggleMinimumStrengthInput;
		
		RefreshMetadata();
	}
	
	public void DoDelete()
	{
		var name = nameInput.Text;
		if(!string.IsNullOrEmpty(name))
		{
			EmitSignal(SignalName.DeleteConfirmed, name);
			clearInputs();
		}
		//TODO: Display error message if name is empty
	}
	
	public void LoadEntry(Item entry)
	{
		nameInput.Text = entry.Name;
		typeInput.SelectItemByText(entry.ArmorData.ArmorType.GetLabel());
		armorClassInput.Value = entry.ArmorData.BaseArmorClass;
		costInput.Value = entry.Cost;
		weightInput.Value = entry.Weight;
		allowDexterityBonus.ButtonPressed = entry.ArmorData.AllowDexterityBonus;
		limitDexterityBonus.ButtonPressed = entry.ArmorData.LimitDexterityBonus;
		dexterityBonusLimit.Value = entry.ArmorData.DexterityBonusLimit;
		stealthDisadvantageInput.ButtonPressed = entry.ArmorData.StealthDisadvantage;
		showStrengthCheck.ButtonPressed = entry.ArmorData.MinimumStrength > 0;
		minimumStrengthInput.Value = entry.ArmorData.MinimumStrength;
		descriptionInput.Text = entry.Description;
		
		toggleLimitDexterity();
		toggleDexterityLimit();
		toggleMinimumStrengthInput();
	}
	
	public void RefreshMetadata()
	{
		if(metadataManager.Container is DndFifthContainer container)
		{
			armorOptionsButton.Clear();
			armorOptionsButton.AddItem(string.Empty);
			container.Items
				.Where(i => i.ArmorData is not null)
				.ToList()
				.ForEach(i => armorOptionsButton.AddItem(i.Name));
		}
	}
	
	private void clearInputs()
	{
		nameInput.Text = string.Empty;
		typeInput.Deselect();
		armorClassInput.Value = 0;
		costInput.Value = 0;
		weightInput.Value = 0.0;
		allowDexterityBonus.ButtonPressed = false;
		limitDexterityBonus.ButtonPressed = false;
		dexterityBonusLimit.Value = 0;
		stealthDisadvantageInput.ButtonPressed = false;
		showStrengthCheck.ButtonPressed = false;
		minimumStrengthInput.Value = 0;
		descriptionInput.Text = string.Empty;
		
		toggleLimitDexterity();
		toggleDexterityLimit();
		toggleMinimumStrengthInput();
	}
	
	private void doSave()
	{
		var name = nameInput.Text;
		var type = (ArmorTypes)typeInput.Selected;
		var ac = (int)armorClassInput.Value;
		var cost = (int)costInput.Value;
		var weight = weightInput.Value;
		var allowDex = allowDexterityBonus.ButtonPressed;
		var limitDex = limitDexterityBonus.ButtonPressed;
		var dexLimit = (int)dexterityBonusLimit.Value;
		var stealth = stealthDisadvantageInput.ButtonPressed;
		var minStr = (int)minimumStrengthInput.Value;
		var description = descriptionInput.Text;
		
		EmitSignal(SignalName.SaveClicked, new Transport<Item>(new()
		{
			ArmorData = new()
			{
				AllowDexterityBonus = allowDex,
				BaseArmorClass = ac,
				DexterityBonusLimit = dexLimit,
				LimitDexterityBonus = limitDex,
				MinimumStrength = minStr,
				StealthDisadvantage = stealth,
				ArmorType = type,
			},
			Cost = cost,
			Description = description,
			Name = name,
			Weight = weight,
		}));
		
		clearInputs();
	}
	
	private void entrySelected(long index)
	{
		var name = armorOptionsButton.GetItemText((int)index);
		if(metadataManager.Container is DndFifthContainer container)
		{
			if(container.Items.Find(i => i.Name.Equals(name)) is Item item)
			{
				LoadEntry(item);
				armorOptionsButton.Deselect();
			}
		}
	}
	
	private void handleDelete() => NodeUtilities.DisplayDeleteConfirmation(
		MetadataLabel,
		this,
		this
	);
	
	private void toggleLimitDexterity()
	{
		limitDexterityBonus.GetParent<HBoxContainer>().Visible = allowDexterityBonus.ButtonPressed;
		toggleDexterityLimit();
	}
	
	private void toggleDexterityLimit() => dexterityBonusLimit.Visible = limitDexterityBonus.ButtonPressed;
	private void toggleMinimumStrengthInput() => minimumStrengthInput.Visible = showStrengthCheck.ButtonPressed;
}
