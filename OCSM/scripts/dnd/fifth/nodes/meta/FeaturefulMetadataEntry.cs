using Godot;
using System.Collections.Generic;
using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Nodes;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class FeaturefulMetadataEntry : BasicMetadataEntry
{
	protected new class NodePaths
	{
		public static readonly NodePath ExistingFeaturesName = new("%ExistingFeatures");
		public static readonly NodePath FeaturesName = new("%Features");
		public static readonly NodePath SectionsName = new("%Sections");
	}
	
	[Signal]
	public delegate void FeaturefulSaveClickedEventHandler(string name, string description, Transport<List<FeatureSection>> sections, Transport<List<Fifth.Feature>> features);
	
	private const string RemoveButtonText = "Remove";
	
	protected List<Fifth.Feature> features = [];
	
	protected Container featureContainer;
	
	public override void _Ready()
	{
		featureContainer = GetNode<Container>(NodePaths.FeaturesName);
		
		base._Ready();
		GetNode<FeatureOptionsButton>(NodePaths.ExistingFeaturesName).ItemSelected += featureSelected;
	}
	
	protected void renderFeatures()
	{
		foreach(Node c in featureContainer.GetChildren())
		{
			c.QueueFree();
		}
		
		var resource = GD.Load<PackedScene>(Constants.Scene.Dnd.Fifth.Feature);
		features.ForEach(f => instantiateFeature(f, resource));
	}
	
	protected void instantiateFeature(Fifth.Feature feature, PackedScene resource)
	{
		var instance = resource.Instantiate<Feature>();
		
		Button button = new()
		{
			Text = RemoveButtonText,
			SizeFlagsHorizontal = SizeFlags.ShrinkCenter
		};
		
		button.Pressed += () => removeFeature(new Transport<Fifth.Feature>(feature));
		instance.AddChild(button);
		featureContainer.AddChild(instance);
		
		instance.Update(feature);
	}
	
	protected void removeFeature(Transport<Fifth.Feature> transport)
	{
		features.Remove(transport.Value);
		renderFeatures();
	}
	
	protected override void clearInputs()
	{
		base.clearInputs();
		var sections = GetNode<SectionList>(NodePaths.SectionsName);
		sections.Values = [];
		sections.Refresh();
		features = [];
		renderFeatures();
	}
	
	protected override void doSave()
	{
		var name = GetNode<LineEdit>(BasicMetadataEntry.NodePaths.NameInput).Text;
		var description = GetNode<TextEdit>(BasicMetadataEntry.NodePaths.DescriptionInput).Text;
		var sections = GetNode<SectionList>(NodePaths.SectionsName).Values;
		
		_ = EmitSignal(SignalName.FeaturefulSaveClicked, name, description, new Transport<List<FeatureSection>>(sections), new Transport<List<Fifth.Feature>>(features));
		clearInputs();
	}
	
	private void featureSelected(long index)
	{
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			var optionsButton = GetNode<FeatureOptionsButton>(NodePaths.ExistingFeaturesName);
			var name = optionsButton.GetItemText((int)index);
			if(dfc.Features.Find(f => f.Name.Equals(name)) is Fifth.Feature feature && !features.Contains(feature))
			{
				features.Add(feature);
				features.Sort();
				renderFeatures();
			}
			optionsButton.Deselect();
		}
	}
	
	public virtual void LoadEntry(Featureful entry)
	{
		base.LoadEntry(entry);
		var sections = GetNode<SectionList>(NodePaths.SectionsName);
		sections.Values = entry.Sections;
		sections.Refresh();
		features = entry.Features;
		features.Sort();
		renderFeatures();
	}
}
