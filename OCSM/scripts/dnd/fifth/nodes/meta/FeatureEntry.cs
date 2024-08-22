using Godot;
using System.Linq;
using System.Collections.Generic;
using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Nodes;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Dnd.Fifth.Nodes.Meta;

public partial class FeatureEntry : Container, ICanDelete
{
	private sealed class NodePath
	{
		public const string Class = "%Class";
		public const string ClassLabel = "%ClassLabel";
		public const string Description = "%Description";
		public const string Name = "%Name";
		public const string Sections = "%Sections";
		public const string Source = "%Source";
		public const string Text = "%Text";
		public const string Type = "%Type";
		public const string ClearButton = "%Clear";
		public const string DeleteButton = "%Delete";
		public const string ExistingEntryName = "%ExistingEntry";
		public const string ExistingLabelName = "%ExistingLabel";
		public const string NumericBonusEditListName = "%NumericBonuses";
		public const string RequiredLevel = "%RequiredLevel";
		public const string SaveButton = "%Save";
	}
	
	public const string MetadataTypeLabel = "Feature";
	public const string ExistingLabelFormat = "Existing {0}";
	
	[Signal]
	public delegate void SaveClickedEventHandler(Transport<Ocsm.Dnd.Fifth.Feature> feature);
	[Signal]
	public delegate void DeleteConfirmedEventHandler(string name);
	
	public Feature Feature { get; set; }
	
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
	
	public override void _ExitTree()
	{
		numericBonusesNode.ValuesChanged -= numericBonusesChanged;
		sectionsNode.ValuesChanged -= sectionsChanged;
		
		base._ExitTree();
	}
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(MetadataManager.NodePath);
		
		Feature ??= new Feature();
		
		classLabel = GetNode<Label>(NodePath.ClassLabel);
		classNode = GetNode<ClassOptionsButton>(NodePath.Class);
		descriptionNode = GetNode<TextEdit>(NodePath.Description);
		nameNode = GetNode<LineEdit>(NodePath.Name);
		numericBonusesNode = GetNode<NumericBonusEditList>(NodePath.NumericBonusEditListName);
		requiredLevel = GetNode<SpinBox>(NodePath.RequiredLevel);
		sectionsNode = GetNode<SectionList>(NodePath.Sections);
		sourceNode = GetNode<LineEdit>(NodePath.Source);
		textNode = GetNode<TextEdit>(NodePath.Text);
		typeNode = GetNode<FeatureTypeOptionsButton>(NodePath.Type);
		
		classNode.ItemSelected += classChanged;
		descriptionNode.TextChanged += descriptionChanged;
		nameNode.TextChanged += nameChanged;
		numericBonusesNode.ValuesChanged += numericBonusesChanged;
		requiredLevel.ValueChanged += requiredLevelChanged;
		sectionsNode.ValuesChanged += sectionsChanged;
		sourceNode.TextChanged += sourceChanged;
		textNode.TextChanged += textChanged;
		typeNode.ItemSelected += typeChanged;
		
		GetNode<Button>(NodePath.ClearButton).Pressed += clearInputs;
		GetNode<Button>(NodePath.SaveButton).Pressed += doSave;
		GetNode<Button>(NodePath.DeleteButton).Pressed += handleDelete;
		GetNode<FeatureOptionsButton>(NodePath.ExistingEntryName).ItemSelected += entrySelected;
		
		refreshValues();
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
			typeNode.SelectItemByText(Feature.FeatureType.GetLabel());
		}
		
		toggleClassInput();
	}
	
	private void clearInputs()
	{
		Feature = new();
		refreshValues();
	}
	
	private void doSave()
	{
		EmitSignal(SignalName.SaveClicked, new Transport<Feature>(Feature));
		clearInputs();
	}
	
	public void DoDelete()
	{
		EmitSignal(SignalName.DeleteConfirmed, Feature.Name);
		clearInputs();
	}
	
	private void handleDelete() => NodeUtilities.DisplayDeleteConfirmation(
		MetadataTypeLabel,
		this,
		this
	);
	
	private void entrySelected(long index)
	{
		var optionsButton = GetNode<FeatureOptionsButton>(NodePath.ExistingEntryName);
		var name = optionsButton.GetItemText((int)index);
		if(metadataManager.Container is DndFifthContainer container)
		{
			if(container.Features.Where(f => f.Name == name).FirstOrDefault() is Feature feature)
			{
				LoadEntry(feature);
				optionsButton.Deselect();
			}
		}
	}
	
	public void LoadEntry(Feature feature)
	{
		if(feature is not null)
		{
			Feature = feature;
			Feature.NumericBonuses.Sort();
			refreshValues();
		}
	}
	
	private void toggleClassInput()
	{
		if(Feature.FeatureType == FeatureTypes.Class)
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
	private void numericBonusesChanged(Transport<List<NumericBonus>> transport) => Feature.NumericBonuses = [.. transport?.Value.OrderBy(nb => nb)];
	private void requiredLevelChanged(double value) => Feature.RequiredLevel = (int)value;
	private void sectionsChanged(Transport<List<FeatureSection>> transport) => Feature.Sections = transport.Value;
	private void sourceChanged(string text) => Feature.Source = text;
	private void textChanged() => Feature.Text = textNode.Text;
	
	private void typeChanged(long index)
	{
		var text = typeNode.GetItemText((int)index);
		
		Feature.FeatureType = System.Enum.GetValues<FeatureTypes>()
			.FirstOrDefault(ft => ft.GetLabel() == text);
		
		if(Feature.FeatureType != FeatureTypes.Class)
			classNode.Deselect();
		
		toggleClassInput();
	}
}
