using Godot;
using System.Collections.Generic;
using Ocsm.Dnd.Fifth;
using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Nodes.Autoload;
using System.Linq;

namespace Ocsm.Nodes.Dnd.Fifth.Meta;

public partial class FeaturefulEntry : Container, ICanDelete
{
	private static class NodePaths
	{
		public static readonly NodePath ClearButton = new("%Clear");
		public static readonly NodePath DescriptionInput = new("%Description");
		public static readonly NodePath DeleteButton = new("%Delete");
		public static readonly NodePath ExistingEntryName = new("%ExistingEntry");
		public static readonly NodePath ExistingFeaturesName = new("%ExistingFeatures");
		public static readonly NodePath ExistingLabelName = new("%ExistingLabel");
		public static readonly NodePath FeaturesName = new("%Features");
		public static readonly NodePath NameInput = new("%Name");
		public static readonly NodePath SaveButton = new("%Save");
		public static readonly NodePath SectionsName = new("%Sections");
	}
	
	[Signal]
	public delegate void SaveClickedEventHandler(MetadataType type, string name, string description, Transport<List<FeatureSection>> sections, Transport<List<Feature>> features);
	[Signal]
	public delegate void DeleteConfirmedEventHandler(MetadataType type, string name);
	
	[Export]
	public MetadataType Type { get; set; }
	[Export]
	public string MetadataTypeLabel { get; set; } = string.Empty;
	
	private List<Feature> features = [];
	private Container featureContainer;
	
	private MetadataManager metadataManager;
	
	private LineEdit nameInput;
	private TextEdit descriptionInput;
	private OptionButton entryOptions;
	private FeatureOptionsButton featureOptions;
	
	public override void _Ready()
	{
		
		metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
		metadataManager.MetadataLoaded += RefreshMetadata;
		metadataManager.MetadataSaved += RefreshMetadata;
		
		nameInput = GetNode<LineEdit>(NodePaths.NameInput);
		descriptionInput = GetNode<TextEdit>(NodePaths.DescriptionInput);
		entryOptions = GetNode<OptionButton>(NodePaths.ExistingEntryName);
		featureContainer = GetNode<Container>(NodePaths.FeaturesName);
		featureOptions = GetNode<FeatureOptionsButton>(NodePaths.ExistingFeaturesName);
		
		GetNode<Button>(NodePaths.ClearButton).Pressed += clearInputs;
		GetNode<Button>(NodePaths.SaveButton).Pressed += doSave;
		GetNode<Button>(NodePaths.DeleteButton).Pressed += handleDelete;
		
		GetNode<Label>(NodePaths.ExistingLabelName).Text = $"Existing {MetadataTypeLabel}";
		
		entryOptions.ItemSelected += entrySelected;
		featureOptions.ItemSelected += featureSelected;
		
		RefreshMetadata();
	}
	
	public void DoDelete()
	{
		var name = GetNode<LineEdit>(NodePaths.NameInput).Text;
		if(!string.IsNullOrEmpty(name))
		{
			EmitSignal(SignalName.DeleteConfirmed, (int)Type, name);
			clearInputs();
		}
		//TODO: Display error message if name is empty
	}
	
	public virtual void LoadEntry(Featureful entry)
	{
		nameInput.Text = entry.Name;
		descriptionInput.Text = entry.Description;
		
		var sections = GetNode<SectionList>(NodePaths.SectionsName);
		sections.Values = entry.Sections;
		sections.Refresh();
		features = entry.Features;
		features.Sort();
		renderFeatures();
	}
	
	public void RefreshMetadata()
	{
		if(metadataManager.Container is DndFifthContainer container
			&& container.Featurefuls.Where(f => f.Type == Type).ToList() is List<Featureful> entries)
		{
			entryOptions.Clear();
			entryOptions.AddItem(string.Empty);
			entries.ForEach(b => entryOptions.AddItem(b.Name));
		}
	}
	
	private void clearInputs()
	{
		nameInput.Text = string.Empty;
		descriptionInput.Text = string.Empty;
		
		var sections = GetNode<SectionList>(NodePaths.SectionsName);
		sections.Values = [];
		sections.Refresh();
		features = [];
		renderFeatures();
	}
	
	private void doSave()
	{
		var name = nameInput.Text;
		var description = descriptionInput.Text;
		var sections = GetNode<SectionList>(NodePaths.SectionsName).Values;
		
		EmitSignal(SignalName.SaveClicked, name, description, new Transport<List<FeatureSection>>(sections), new Transport<List<Feature>>(features));
		clearInputs();
	}
	
	protected void entrySelected(long index)
	{
		var name = entryOptions.GetItemText((int)index);
		if(metadataManager.Container is DndFifthContainer container
			&& container.Featurefuls.Where(f => f.Type == Type && f.Name == name).FirstOrDefault() is Featureful entry)
		{
			LoadEntry(entry);
			entryOptions.Deselect();
		}
	}
	
	private void featureSelected(long index)
	{
		if(metadataManager.Container is DndFifthContainer container)
		{
			var optionsButton = GetNode<FeatureOptionsButton>(NodePaths.ExistingFeaturesName);
			var name = optionsButton.GetItemText((int)index);
			if(container.Features.Find(f => f.Name.Equals(name)) is Feature feature && !features.Contains(feature))
			{
				features.Add(feature);
				features.Sort();
				renderFeatures();
			}
			optionsButton.Deselect();
		}
	}
	
	protected void handleDelete() => NodeUtilities.DisplayDeleteConfirmation(
		MetadataTypeLabel,
		this,
		this
	);
	
	private void instantiateFeature(Feature feature, PackedScene resource)
	{
		var instance = resource.Instantiate<FeatureNode>();
		
		Button button = new()
		{
			Text = "Remove",
			SizeFlagsHorizontal = SizeFlags.ShrinkCenter
		};
		
		button.Pressed += () => removeFeature(new Transport<Feature>(feature));
		instance.AddChild(button);
		featureContainer.AddChild(instance);
		
		instance.Update(feature);
	}
	
	private void removeFeature(Transport<Feature> transport)
	{
		features.Remove(transport.Value);
		renderFeatures();
	}
	
	private void renderFeatures()
	{
		foreach(Node c in featureContainer.GetChildren())
		{
			c.QueueFree();
		}
		
		var resource = GD.Load<PackedScene>(Constants.Scene.Dnd.Fifth.Feature);
		features.ForEach(f => instantiateFeature(f, resource));
	}
}
