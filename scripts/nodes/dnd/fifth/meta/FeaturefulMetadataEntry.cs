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
		
		[Signal]
		public new delegate void SaveClicked(string name, string description, List<Transport<Feature>> features);
		
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
			Features = new List<OCSM.DnD.Fifth.Feature>();
			renderFeatures();
		}
		
		protected override void doSave()
		{
			var name = GetNode<LineEdit>(NodePathBuilder.SceneUnique(NameInput)).Text;
			var description = GetNode<TextEdit>(NodePathBuilder.SceneUnique(DescriptionInput)).Text;
			
			var transports = new List<Transport<OCSM.DnD.Fifth.Feature>>();
			foreach(var feature in Features)
			{
				transports.Add(new Transport<OCSM.DnD.Fifth.Feature>(feature));
			}
			
			EmitSignal(nameof(SaveClicked), name, description, transports);
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
					renderFeatures();
				}
				optionsButton.Selected = 0;
			}
		}
		
		public virtual void loadEntry(Featureful entry)
		{
			base.loadEntry(entry);
			
			Features = entry.Features;
			renderFeatures();
		}
	}
}
