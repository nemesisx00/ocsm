using Godot;
using System.Collections.Generic;
using OCSM.Nodes.Meta;
using OCSM.DnD.Fifth.Meta;

namespace OCSM.Nodes.DnD.Fifth.Meta
{
	public partial class FeaturefulMetadataEntry : BasicMetadataEntry
	{
		protected new class NodePath : BasicMetadataEntry.NodePath
		{
			public const string ExistingFeaturesName = "%ExistingFeatures";
			public const string FeaturesName = "%Features";
			public const string SectionsName = "%Sections";
		}
		
		[Signal]
		public new delegate void SaveClickedEventHandler(string name, string description, Transport<List<OCSM.DnD.Fifth.FeatureSection>> sections, Transport<List<OCSM.DnD.Fifth.Feature>> features);
		
		protected List<OCSM.DnD.Fifth.Feature> Features;
		
		protected Container featureContainer;
		
		public new void _Ready()
		{
			Features = new List<OCSM.DnD.Fifth.Feature>();
			
			featureContainer = GetNode<Container>(NodePath.FeaturesName);
			
			base._Ready();
			GetNode<FeatureOptionsButton>(NodePath.ExistingFeaturesName).ItemSelected += featureSelected;
		}
		
		protected void renderFeatures()
		{
			foreach(Node c in featureContainer.GetChildren())
			{
				c.QueueFree();
			}
			
			var resource = GD.Load<PackedScene>(Constants.Scene.DnD.Fifth.Feature);
			Features.ForEach(f => instantiateFeature(f, resource));
		}
		
		protected void instantiateFeature(OCSM.DnD.Fifth.Feature feature, PackedScene resource)
		{
			var instance = resource.Instantiate<Feature>();
			var button = new Button();
			button.Text = "Remove";
			button.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
			button.Pressed += () => removeFeature(new Transport<OCSM.DnD.Fifth.Feature>(feature));
			instance.AddChild(button);
			featureContainer.AddChild(instance);
			
			instance.update(feature);
		}
		
		protected void removeFeature(Transport<OCSM.DnD.Fifth.Feature> transport)
		{
			Features.Remove(transport.Value);
			renderFeatures();
		}
		
		protected override void clearInputs()
		{
			base.clearInputs();
			var sections = GetNode<SectionList>(NodePath.SectionsName);
			sections.Values = new List<OCSM.DnD.Fifth.FeatureSection>();
			sections.refresh();
			Features = new List<OCSM.DnD.Fifth.Feature>();
			renderFeatures();
		}
		
		protected override void doSave()
		{
			var name = GetNode<LineEdit>(NodePath.NameInput).Text;
			var description = GetNode<TextEdit>(NodePath.DescriptionInput).Text;
			var sections = GetNode<SectionList>(NodePath.SectionsName).Values;
			
			EmitSignal(nameof(SaveClicked), name, description, new Transport<List<OCSM.DnD.Fifth.FeatureSection>>(sections), new Transport<List<OCSM.DnD.Fifth.Feature>>(Features));
			clearInputs();
		}
		
		private void featureSelected(long index)
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				var optionsButton = GetNode<FeatureOptionsButton>(NodePath.ExistingFeaturesName);
				var name = optionsButton.GetItemText((int)index);
				if(dfc.Features.Find(f => f.Name.Equals(name)) is OCSM.DnD.Fifth.Feature feature && !Features.Contains(feature))
				{
					Features.Add(feature);
					Features.Sort();
					renderFeatures();
				}
				optionsButton.Deselect();
			}
		}
		
		public virtual void loadEntry(Featureful entry)
		{
			base.loadEntry(entry);
			var sections = GetNode<SectionList>(NodePath.SectionsName);
			sections.Values = entry.Sections;
			sections.refresh();
			Features = entry.Features;
			Features.Sort();
			renderFeatures();
		}
	}
}
