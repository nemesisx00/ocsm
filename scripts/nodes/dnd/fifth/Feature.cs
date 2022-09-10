using Godot;
using System;

namespace OCSM.Nodes.DnD.Fifth
{
	public class Feature : Container
	{
		private const string FormatType = " ({0} Feature)";
		private const string FormatTypeAndSource = " ({0} Feature, {1})";
		
		public override void _Ready()
		{
			GetNode<TextureButton>(NodePathBuilder.SceneUnique("ShowHide")).Connect(Constants.Signal.Pressed, this, nameof(toggleSections));
		}
		
		public void update(OCSM.DnD.Fifth.Feature feature)
		{
			var label = GetNode<Label>(NodePathBuilder.SceneUnique("Name"));
			var name = feature.Name;
			if(!String.IsNullOrEmpty(feature.Type))
			{
				if(!String.IsNullOrEmpty(feature.Source))
					name += String.Format(FormatTypeAndSource, feature.Type, feature.Source);
				else
					name += String.Format(FormatType, feature.Type);
			}
			label.Text = name;
			
			var description = GetNode<RichTextLabel>(NodePathBuilder.SceneUnique("Description"));
			description.Text = feature.Description;
			
			var text = GetNode<RichTextLabel>(NodePathBuilder.SceneUnique("Text"));
			text.Text = feature.Text;
			
			if(feature.Sections.Count > 0)
			{
				var resource = ResourceLoader.Load<PackedScene>(Constants.Scene.DnD.Fifth.FeatureSection);
				var sections = GetNode<Container>(NodePathBuilder.SceneUnique("Sections"));
				foreach(var section in feature.Sections)
				{
					var instance = resource.Instance<VBoxContainer>();
					instance.GetChild<Label>(0).Text = section.Section;
					instance.GetChild<RichTextLabel>(1).Text = section.Text;
					sections.AddChild(instance);
				}
			}
		}
		
		private void toggleSections()
		{
			var node = GetNode<Container>(NodePathBuilder.SceneUnique("Details"));
			if(node.Visible)
				node.Hide();
			else
				node.Show();
		}
	}
}
