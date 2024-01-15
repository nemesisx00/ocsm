using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Nodes;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class FeatureEntry : Container, ICanDelete
{
	private sealed class NodePaths
	{
		public static readonly NodePath Class = new("%Class");
		public static readonly NodePath ClassLabel = new("%ClassLabel");
		public static readonly NodePath Description = new("%Description");
		public static readonly NodePath Name = new("%Name");
		public static readonly NodePath Sections = new("%Sections");
		public static readonly NodePath Source = new("%Source");
		public static readonly NodePath Text = new("%Text");
		public static readonly NodePath Type = new("%Type");
		public static readonly NodePath ClearButton = new("%Clear");
		public static readonly NodePath DeleteButton = new("%Delete");
		public static readonly NodePath ExistingEntryName = new("%ExistingEntry");
		public static readonly NodePath ExistingLabelName = new("%ExistingLabel");
		public static readonly NodePath NumericBonusEditListName = new("%NumericBonuses");
		public static readonly NodePath RequiredLevel = new("%RequiredLevel");
		public static readonly NodePath SaveButton = new("%Save");
	}
	
	public const string MetadataTypeLabel = "Feature";
	public const string ExistingLabelFormat = "Existing {0}";
	
	[Signal]
	public delegate void SaveClickedEventHandler(Transport<Fifth.Feature> feature);
	[Signal]
	public delegate void DeleteConfirmedEventHandler(string name);
	
	public Fifth.Feature Feature { get; set; }
	
	private Label classLabel;
	private ClassOptionsButton classNode;
	private TextEdit descriptionNode;
	private LineEdit nameNode;
	private NumericBonusEditList numericBonusesNode;
	private SpinBox requiredLevel;
	private SectionList sectionsNode;
	private LineEdit sourceNode;
	private TextEdit textNode;
	private FeatureTypeOptionsButton typeNode;
	
	private MetadataManager metadataManager;
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(MetadataManager.NodePath);
		
		Feature ??= new Fifth.Feature();
		
		classLabel = GetNode<Label>(NodePaths.ClassLabel);
		classNode = GetNode<ClassOptionsButton>(NodePaths.Class);
		descriptionNode = GetNode<TextEdit>(NodePaths.Description);
		nameNode = GetNode<LineEdit>(NodePaths.Name);
		numericBonusesNode = GetNode<NumericBonusEditList>(NodePaths.NumericBonusEditListName);
		requiredLevel = GetNode<SpinBox>(NodePaths.RequiredLevel);
		sectionsNode = GetNode<SectionList>(NodePaths.Sections);
		sourceNode = GetNode<LineEdit>(NodePaths.Source);
		textNode = GetNode<TextEdit>(NodePaths.Text);
		typeNode = GetNode<FeatureTypeOptionsButton>(NodePaths.Type);
		
		classNode.ItemSelected += classChanged;
		descriptionNode.TextChanged += descriptionChanged;
		nameNode.TextChanged += nameChanged;
		numericBonusesNode.ValuesChanged += numericBonusesChanged;
		requiredLevel.ValueChanged += requiredLevelChanged;
		sectionsNode.ValuesChanged += sectionsChanged;
		sourceNode.TextChanged += sourceChanged;
		textNode.TextChanged += textChanged;
		typeNode.ItemSelected += typeChanged;
		
		GetNode<Button>(NodePaths.ClearButton).Pressed += clearInputs;
		GetNode<Button>(NodePaths.SaveButton).Pressed += doSave;
		GetNode<Button>(NodePaths.DeleteButton).Pressed += handleDelete;
		GetNode<FeatureOptionsButton>(NodePaths.ExistingEntryName).ItemSelected += entrySelected;
		
		refreshValues();
	}
	
	public void DoDelete()
	{
		_ = EmitSignal(SignalName.DeleteConfirmed, Feature.Name);
		clearInputs();
	}
	
	public void LoadEntry(Fifth.Feature feature)
	{
		if(feature is not null)
		{
			Feature = feature;
			Feature.NumericBonuses.Sort();
			refreshValues();
		}
	}
	
	private void refreshValues()
	{
		if(Feature is not null)
		{
			classNode.Select(Feature.ClassName);
			descriptionNode.Text = Feature.Description;
			nameNode.Text = Feature.Name;
			numericBonusesNode.Values = Feature.NumericBonuses;
			numericBonusesNode.Refresh();
			requiredLevel.Value = Feature.RequiredLevel;
			sectionsNode.Values = Feature.Sections;
			sectionsNode.Refresh();
			sourceNode.Text = Feature.Source;
			textNode.Text = Feature.Text;
			typeNode.SelectItemByText(Feature.Type.ToString());
		}
		
		toggleClassInput();
	}
	
	private void clearInputs()
	{
		Feature = new Fifth.Feature();
		refreshValues();
	}
	
	private void doSave()
	{
		_ = EmitSignal(SignalName.SaveClicked, new Transport<Fifth.Feature>(Feature));
		clearInputs();
	}
	
	private void handleDelete() => NodeUtilities.DisplayDeleteConfirmation(
		MetadataTypeLabel,
		GetTree().CurrentScene,
		this
	);
	
	private void entrySelected(long index)
	{
		var optionsButton = GetNode<FeatureOptionsButton>(NodePaths.ExistingEntryName);
		var name = optionsButton.GetItemText((int)index);
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			if(dfc.Features.Find(f => f.Name.Equals(name)) is Fifth.Feature feature)
			{
				LoadEntry(feature);
				optionsButton.Deselect();
			}
		}
	}
	
	private void toggleClassInput()
	{
		if(Feature.Type.Equals(FeatureTypes.Class))
		{
			classLabel.Show();
			classNode.Show();
		}
		else
		{
			classLabel.Hide();
			classNode.Hide();
		}
	}
	
	private void classChanged(long index) => Feature.ClassName = classNode.GetItemText((int)index);
	private void descriptionChanged() => Feature.Description = descriptionNode.Text;
	private void nameChanged(string text) => Feature.Name = text;
	
	private void numericBonusesChanged(Transport<List<NumericBonus>> transport)
		=> Feature.NumericBonuses = [.. transport.Value.OrderBy(nb => nb)];
	
	private void requiredLevelChanged(double value) => Feature.RequiredLevel = (int)value;
	private void sectionsChanged(Transport<List<FeatureSection>> transport) => Feature.Sections = transport.Value;
	private void sourceChanged(string text) => Feature.Source = text;
	private void textChanged() => Feature.Text = textNode.Text;
	
	private void typeChanged(long index)
	{
		Feature.Type = Enum.GetValues<FeatureTypes>()
			.ToList()
			.Find(ft => ft.ToString().Equals(typeNode.GetItemText((int)index)));
		
		if(!Feature.Type.Equals(FeatureTypes.Class))
			classNode.Deselect();
		
		toggleClassInput();
	}
}
