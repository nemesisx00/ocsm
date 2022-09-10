using Godot;
using System.Collections.Generic;
using System.Text.Json;
using OCSM.Nodes.Meta;
using OCSM.DnD.Fifth.Meta;

namespace OCSM.Nodes.DnD.Fifth.Meta
{
	public class FeaturefulMetadataEntry : BasicMetadataEntry
	{
		protected const string ExistingFeaturesName = "ExistingFeatures";
		protected const string FeaturesName = "Features";
		protected const string SectionsName = "Sections";
		
		[Signal]
		public new delegate void SaveClicked(string name, string description, List<Transport<OCSM.DnD.Fifth.FeatureSection>> sections, List<Transport<Feature>> features);
		
		protected List<OCSM.DnD.Fifth.Feature> Features;
		
		public override void _Ready()
		{
			Features = new List<OCSM.DnD.Fifth.Feature>();
			
			base._Ready();
			GetNode<FeatureOptionsButton>(NodePathBuilder.SceneUnique(ExistingFeaturesName)).Connect(Constants.Signal.ItemSelected, this, nameof(featureSelected));
		}
		
		protected void renderFeatures()
		{
			var featureContainer = GetNode<Container>(NodePathBuilder.SceneUnique(FeaturesName));
			foreach(Node c in featureContainer.GetChildren())
			{
				c.QueueFree();
			}
			
			var resource = ResourceLoader.Load<PackedScene>(Constants.Scene.DnD.Fifth.Feature);
			foreach(var feature in Features)
			{
				var instance = resource.Instance<Feature>();
				instance.update(feature);
				var button = new Button();
				button.Text = "Remove";
				button.SizeFlagsHorizontal = (int)Control.SizeFlags.ShrinkCenter;
				button.Connect(Constants.Signal.Pressed, this, nameof(removeFeature), new Godot.Collections.Array(new Transport<OCSM.DnD.Fifth.Feature>(feature)));
				instance.AddChild(button);
				featureContainer.AddChild(instance);
			}
		}
		
		protected void removeFeature(Transport<OCSM.DnD.Fifth.Feature> transport)
		{
			Features.Remove(transport.Value);
			renderFeatures();
		}
		
		protected override void clearInputs()
		{
			base.clearInputs();
			var sections = GetNode<SectionList>(NodePathBuilder.SceneUnique(SectionsName));
			sections.Values = new List<OCSM.DnD.Fifth.FeatureSection>();
			sections.refresh();
			Features = new List<OCSM.DnD.Fifth.Feature>();
			renderFeatures();
		}
		
		protected override void doSave()
		{
			var name = GetNode<LineEdit>(NodePathBuilder.SceneUnique(NameInput)).Text;
			var description = GetNode<TextEdit>(NodePathBuilder.SceneUnique(DescriptionInput)).Text;
			var sections = GetNode<SectionList>(NodePathBuilder.SceneUnique(SectionsName)).Values;
			
			var sectionTransports = new List<Transport<OCSM.DnD.Fifth.FeatureSection>>();
			foreach(var section in sections)
			{
				sectionTransports.Add(new Transport<OCSM.DnD.Fifth.FeatureSection>(section));
			}
			
			var featureTransports = new List<Transport<OCSM.DnD.Fifth.Feature>>();
			foreach(var feature in Features)
			{
				featureTransports.Add(new Transport<OCSM.DnD.Fifth.Feature>(feature));
			}
			
			EmitSignal(nameof(SaveClicked), name, description, sectionTransports, featureTransports);
			clearInputs();
		}
		
		private void featureSelected(int index)
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				var optionsButton = GetNode<FeatureOptionsButton>(NodePathBuilder.SceneUnique(ExistingFeaturesName));
				var name = optionsButton.GetItemText(index);
				if(dfc.Features.Find(f => f.Name.Equals(name)) is OCSM.DnD.Fifth.Feature feature && !Features.Contains(feature))
				{
					Features.Add(feature);
					Features.Sort();
					renderFeatures();
				}
				optionsButton.Selected = 0;
			}
		}
		
		public virtual void loadEntry(Featureful entry)
		{
			base.loadEntry(entry);
			var sections = GetNode<SectionList>(NodePathBuilder.SceneUnique(SectionsName));
			sections.Values = entry.Sections;
			sections.refresh();
			Features = entry.Features;
			Features.Sort();
			renderFeatures();
		}
	}
}
