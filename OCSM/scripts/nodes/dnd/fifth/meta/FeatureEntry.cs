using Godot;
using System.Linq;
using System.Collections.Generic;
using Ocsm.Dnd.Fifth;
using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Nodes.Dnd.Fifth.Meta;

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
	
	public Ocsm.Dnd.Fifth.Feature Feature { get; set; }
	
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
		metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
		
		if(!(Feature is Ocsm.Dnd.Fifth.Feature))
			Feature = new Ocsm.Dnd.Fifth.Feature();
		
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
		if(Feature is Ocsm.Dnd.Fifth.Feature)
		{
			classNode.select(Feature.ClassName);
			descriptionNode.Text = Feature.Description;
			nameNode.Text = Feature.Name;
			numericBonusesNode.Values = Feature.NumericBonuses;
			numericBonusesNode.refresh();
			requiredLevel.Value = Feature.RequiredLevel;
			sectionsNode.Values = Feature.Sections;
			sectionsNode.refresh();
			sourceNode.Text = Feature.Source;
			textNode.Text = Feature.Text;
			typeNode.SelectItemByText(Feature.Type);
		}
		
		toggleClassInput();
	}
	
	private void clearInputs()
	{
		Feature = new Ocsm.Dnd.Fifth.Feature();
		refreshValues();
	}
	
	private void doSave()
	{
		EmitSignal(nameof(SaveClicked), new Transport<Ocsm.Dnd.Fifth.Feature>(Feature));
		clearInputs();
	}
	
	public void doDelete()
	{
		EmitSignal(nameof(DeleteConfirmed), Feature.Name);
		clearInputs();
	}
	
	private void handleDelete()
	{
		NodeUtilities.displayDeleteConfirmation(
			MetadataTypeLabel,
			GetTree().CurrentScene,
			GetViewportRect().GetCenter(),
			this,
			nameof(doDelete)
		);
	}
	
	private void entrySelected(long index)
	{
		var optionsButton = GetNode<FeatureOptionsButton>(NodePath.ExistingEntryName);
		var name = optionsButton.GetItemText((int)index);
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			if(dfc.Features.Find(f => f.Name.Equals(name)) is Ocsm.Dnd.Fifth.Feature feature)
			{
				loadEntry(feature);
				optionsButton.Deselect();
			}
		}
	}
	
	public void loadEntry(Ocsm.Dnd.Fifth.Feature feature)
	{
		if(feature is Ocsm.Dnd.Fifth.Feature)
		{
			Feature = feature;
			Feature.NumericBonuses.Sort();
			refreshValues();
		}
	}
	
	private void toggleClassInput()
	{
		if(Feature.Type.Equals(FeatureType.Class))
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
	
	private void classChanged(long index) { Feature.ClassName = classNode.GetItemText((int)index); }
	private void descriptionChanged() { Feature.Description = descriptionNode.Text; }
	private void nameChanged(string text) { Feature.Name = text; }
	private void numericBonusesChanged(Transport<List<NumericBonus>> transport) { Feature.NumericBonuses = transport.Value.OrderBy(nb => nb).ToList(); }
	private void requiredLevelChanged(double value) { Feature.RequiredLevel = (int)value; }
	private void sectionsChanged(Transport<List<FeatureSection>> transport) { Feature.Sections = transport.Value; }
	private void sourceChanged(string text) { Feature.Source = text; }
	private void textChanged() { Feature.Text = textNode.Text; }
	
	private void typeChanged(long index)
	{
		Feature.Type = typeNode.GetItemText((int)index);
		
		if(!Feature.Type.Equals(FeatureType.Class))
			classNode.Deselect();
		toggleClassInput();
	}
}
