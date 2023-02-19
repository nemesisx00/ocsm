using Godot;
using System.Collections.Generic;
using System.Text.Json;
using OCSM.Nodes.Meta;
using OCSM.DnD.Fifth.Meta;

namespace OCSM.Nodes.DnD.Fifth.Meta
{
	public partial class FeaturefulMetadataEntry : BasicMetadataEntry
	{
		protected const string ExistingFeaturesName = "ExistingFeatures";
		protected const string FeaturesName = "Features";
		protected const string SectionsName = "Sections";
		
		[Signal]
		public new delegate void SaveClickedEventHandler(string name, string description, Transport<List<OCSM.DnD.Fifth.FeatureSection>> sections, Transport<List<OCSM.DnD.Fifth.Feature>> features);
		
		protected List<OCSM.DnD.Fifth.Feature> Features;
		
		public override void _Ready()
		{
			Features = new List<OCSM.DnD.Fifth.Feature>();
			
			base._Ready();
			GetNode<FeatureOptionsButton>(NodePathBuilder.SceneUnique(ExistingFeaturesName)).Connect(Constants.Signal.ItemSelected,new Callable(this,nameof(featureSelected)));
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
				var instance = resource.Instantiate<Feature>();
				var button = new Button();
				button.Text = "Remove";
				button.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
				button.Pressed += () => removeFeature(new Transport<OCSM.DnD.Fifth.Feature>(feature));
				instance.AddChild(button);
				featureContainer.AddChild(instance);
				
				instance.update(feature);
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
			
			EmitSignal(nameof(SaveClicked), name, description, new Transport<List<OCSM.DnD.Fifth.FeatureSection>>(sections), new Transport<List<OCSM.DnD.Fifth.Feature>>(Features));
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
