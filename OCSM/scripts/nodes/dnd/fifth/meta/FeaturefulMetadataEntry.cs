using Godot;
using System.Collections.Generic;
using Ocsm.Nodes.Meta;
using Ocsm.Dnd.Fifth.Meta;

namespace Ocsm.Nodes.Dnd.Fifth.Meta;

public partial class FeaturefulMetadataEntry : BasicMetadataEntry
{
	protected new static class NodePaths
	{
		public const string ExistingFeaturesName = "%ExistingFeatures";
		public const string FeaturesName = "%Features";
		public const string SectionsName = "%Sections";
	}
	
	[Signal]
	public new delegate void SaveClickedEventHandler(string name, string description, Transport<List<Ocsm.Dnd.Fifth.FeatureSection>> sections, Transport<List<Ocsm.Dnd.Fifth.Feature>> features);
	
	protected List<Ocsm.Dnd.Fifth.Feature> features = [];
	protected Container featureContainer;
	
	public override void _Ready()
	{
		featureContainer = GetNode<Container>(NodePaths.FeaturesName);
		
		base._Ready();
		GetNode<FeatureOptionsButton>(NodePaths.ExistingFeaturesName).ItemSelected += featureSelected;
	}
	
	public virtual void LoadEntry(Featureful entry)
	{
		base.LoadEntry(entry);
		var sections = GetNode<SectionList>(NodePaths.SectionsName);
		sections.Values = entry.Sections;
		sections.refresh();
		features = entry.Features;
		features.Sort();
		renderFeatures();
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
	
	protected void instantiateFeature(Ocsm.Dnd.Fifth.Feature feature, PackedScene resource)
	{
		var instance = resource.Instantiate<Feature>();
		
		Button button = new()
		{
			Text = "Remove",
			SizeFlagsHorizontal = SizeFlags.ShrinkCenter
		};
		
		button.Pressed += () => removeFeature(new Transport<Ocsm.Dnd.Fifth.Feature>(feature));
		instance.AddChild(button);
		featureContainer.AddChild(instance);
		
		instance.update(feature);
	}
	
	protected void removeFeature(Transport<Ocsm.Dnd.Fifth.Feature> transport)
	{
		features.Remove(transport.Value);
		renderFeatures();
	}
	
	protected override void clearInputs()
	{
		base.clearInputs();
		var sections = GetNode<SectionList>(NodePaths.SectionsName);
		sections.Values = [];
		sections.refresh();
		features = [];
		renderFeatures();
	}
	
	protected override void doSave()
	{
		var name = nameInput.Text;
		var description = descriptionInput.Text;
		var sections = GetNode<SectionList>(NodePaths.SectionsName).Values;
		
		EmitSignal(SignalName.SaveClicked, name, description, new Transport<List<Ocsm.Dnd.Fifth.FeatureSection>>(sections), new Transport<List<Ocsm.Dnd.Fifth.Feature>>(features));
		clearInputs();
	}
	
	private void featureSelected(long index)
	{
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			var optionsButton = GetNode<FeatureOptionsButton>(NodePaths.ExistingFeaturesName);
			var name = optionsButton.GetItemText((int)index);
			if(dfc.Features.Find(f => f.Name.Equals(name)) is Ocsm.Dnd.Fifth.Feature feature && !features.Contains(feature))
			{
				features.Add(feature);
				features.Sort();
				renderFeatures();
			}
			optionsButton.Deselect();
		}
	}
}
