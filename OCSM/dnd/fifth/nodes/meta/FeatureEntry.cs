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
		public const string Description = "%Description";
		public const string Name = "%Name";
		public const string Sections = "%Sections";
		public const string Source = "%Source";
		public const string TagsRow = "%TagsRow";
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
	
	private HBoxContainer tagsRow;
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
		
		descriptionNode = GetNode<TextEdit>(NodePaths.Description);
		nameNode = GetNode<LineEdit>(NodePaths.Name);
		numericBonusesNode = GetNode<NumericBonusEditList>(NodePaths.NumericBonusEditListName);
		requiredLevel = GetNode<SpinBox>(NodePaths.RequiredLevel);
		sectionsNode = GetNode<SectionList>(NodePaths.Sections);
		sourceNode = GetNode<LineEdit>(NodePaths.Source);
		tagsRow = GetNode<HBoxContainer>(NodePaths.TagsRow);
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
		
		if(tagsRow.GetChildCount() < 1)
			addTagInput();
	}
	
	public void DoDelete()
	{
		EmitSignal(SignalName.DeleteConfirmed, Feature.Name);
		clearInputs();
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
	
	private void addTagInput(Metadata value = null)
	{
		var opt = new MetadataOption()
		{
			EmptyOption = true,
			MetadataTypes = [ "Background", "Class", "Species" ]
		};
		
		tagsRow.AddChild(opt);
		opt.SelectedMetadata = value;
		opt.ItemSelected += _ => updateTags();
	}
	
	private void clearInputs()
	{
		Feature = new();
		refreshValues();
	}
	
	private void descriptionChanged() => Feature.Description = descriptionNode.Text;
	
	private void doSave()
	{
		updateTags();
		EmitSignal(SignalName.SaveClicked, new Transport<Feature>(Feature));
		clearInputs();
	}
	
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
	
	private void handleDelete() => NodeUtilities.DisplayDeleteConfirmation(
		MetadataTypeLabel,
		this,
		this
	);
	
	private void nameChanged(string text) => Feature.Name = text;
	private void numericBonusesChanged(Transport<List<NumericBonus>> transport) => Feature.NumericBonuses = [.. transport?.Value.OrderBy(nb => nb)];
	
	private void refreshTags()
	{
		if(metadataManager.Container is DndFifthContainer container)
		{
			foreach(var child in tagsRow.GetChildren())
				child.QueueFree();
			
			foreach(var tag in Feature.Tags)
				addTagInput(container.Metadata.Where(m => m.Name == tag).FirstOrDefault());
			
			addTagInput();
		}
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
			
			refreshTags();
		}
	}
	
	private void requiredLevelChanged(double value) => Feature.RequiredLevel = (int)value;
	private void sectionsChanged(Transport<List<FeatureSection>> transport) => Feature.Sections = transport.Value;
	private void sourceChanged(string text) => Feature.Source = text;
	private void textChanged() => Feature.Text = textNode.Text;
	
	private void typeChanged(long index)
	{
		var text = typeNode.GetItemText((int)index);
		
		Feature.FeatureType = System.Enum.GetValues<FeatureTypes>()
			.FirstOrDefault(ft => ft.GetLabel() == text);
	}
	
	private void updateTags()
	{
		Feature.Tags.Clear();
		
		var children = tagsRow.GetChildren().Cast<MetadataOption>();
		foreach(var tag in children)
		{
			if(tag.SelectedMetadata is Metadata meta)
				Feature.Tags.Add(meta.Name);
		}
		
		refreshTags();
	}
}
