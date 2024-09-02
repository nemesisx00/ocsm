using Godot;
using System.Linq;
using System.Collections.Generic;
using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Nodes;
using Ocsm.Nodes.Autoload;
using Ocsm.Meta;

namespace Ocsm.Dnd.Fifth.Nodes.Meta;

public partial class FeatureEntry : Container, ICanDelete
{
	private sealed class NodePaths
	{
		public const string BackgroundTag = "%BackgroundTag";
		public const string ClassTag = "%ClassTag";
		public const string Description = "%Description";
		public const string Name = "%Name";
		public const string Sections = "%Sections";
		public const string Source = "%Source";
		public const string SpeciesTag = "%SpeciesTag";
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
	public delegate void SaveClickedEventHandler(Transport<Feature> feature);
	[Signal]
	public delegate void DeleteConfirmedEventHandler(string name);
	
	public Feature Feature { get; set; }
	
	private MetadataOption backgroundTag;
	private MetadataOption classTag;
	private TextEdit descriptionNode;
	private LineEdit nameNode;
	private NumericBonusEditList numericBonusesNode;
	private SpinBox requiredLevel;
	private SectionList sectionsNode;
	private MetadataOption speciesTag;
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
		
		backgroundTag = GetNode<MetadataOption>(NodePaths.BackgroundTag);
		classTag = GetNode<MetadataOption>(NodePaths.ClassTag);
		descriptionNode = GetNode<TextEdit>(NodePaths.Description);
		nameNode = GetNode<LineEdit>(NodePaths.Name);
		numericBonusesNode = GetNode<NumericBonusEditList>(NodePaths.NumericBonusEditListName);
		requiredLevel = GetNode<SpinBox>(NodePaths.RequiredLevel);
		sectionsNode = GetNode<SectionList>(NodePaths.Sections);
		speciesTag = GetNode<MetadataOption>(NodePaths.SpeciesTag);
		sourceNode = GetNode<LineEdit>(NodePaths.Source);
		textNode = GetNode<TextEdit>(NodePaths.Text);
		typeNode = GetNode<FeatureTypeOptionsButton>(NodePaths.Type);
		
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
	
	private void refreshValues()
	{
		if(Feature is not null)
		{
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
			
			if(metadataManager.Container is DndFifthContainer container)
			{
				Metadata bgTag = null;
				Metadata cTag = null;
				Metadata sTag = null;
				
				foreach(var tag in Feature.Tags)
				{
					var possibles = container.Metadata.Where(m => m.Name == tag);
					
					if(possibles.Where(m => m.Type == MetadataType.Dnd5eBackground).FirstOrDefault() is Metadata bgTagValue)
						bgTag = bgTagValue;
					else if(possibles.Where(m => m.Type == MetadataType.Dnd5eClass).FirstOrDefault() is Metadata cTagValue)
						cTag = cTagValue;
					else if(possibles.Where(m => m.Type == MetadataType.Dnd5eSpecies).FirstOrDefault() is Metadata sTagValue)
						sTag = sTagValue;
				}
				
				backgroundTag.SelectedMetadata = bgTag;
				classTag.SelectedMetadata = cTag;
				speciesTag.SelectedMetadata = sTag;
			}
		}
	}
	
	private void clearInputs()
	{
		Feature = new();
		refreshValues();
	}
	
	private void doSave()
	{
		updateTags();
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
		var optionsButton = GetNode<FeatureOptionsButton>(NodePaths.ExistingEntryName);
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
	
	private void descriptionChanged() => Feature.Description = descriptionNode.Text;
	private void nameChanged(string text) => Feature.Name = text;
	private void numericBonusesChanged(Transport<List<NumericBonus>> transport) => Feature.NumericBonuses = [.. transport?.Value.OrderBy(nb => nb)];
	private void requiredLevelChanged(double value) => Feature.RequiredLevel = (int)value;
	private void sectionsChanged(Transport<List<FeatureSection>> transport) => Feature.Sections = transport.Value;
	private void sourceChanged(string text) => Feature.Source = text;
	private void textChanged() => Feature.Text = textNode.Text;
	
	private void updateTags()
	{
		Feature.Tags.Clear();
		
		if(backgroundTag.SelectedMetadata is Metadata bg)
			Feature.Tags.Add(bg.Name);
		
		if(classTag.SelectedMetadata is Metadata c)
			Feature.Tags.Add(c.Name);
		
		if(speciesTag.SelectedMetadata is Metadata s)
			Feature.Tags.Add(s.Name);
	}
	
	private void typeChanged(long index)
	{
		var text = typeNode.GetItemText((int)index);
		
		Feature.FeatureType = System.Enum.GetValues<FeatureTypes>()
			.FirstOrDefault(ft => ft.GetLabel() == text);
	}
}
