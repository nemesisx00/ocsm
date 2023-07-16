using Godot;
using System;
using System.Linq;

namespace Ocsm.Nodes.Dnd.Fifth
{
	public partial class Feature : Container
	{
		public sealed class NodePath
		{
			public const string Name = "%Name";
			public const string Description = "%Description";
			public const string Details = "%Details";
			public const string Text = "%Text";
			public const string Sections = "%Sections";
			public const string ShowHide = "%ShowHide";
		}
		
		private const string FormatType = " ({0} Feature)";
		private const string FormatTypeAndSource = " ({0} Feature, {1})";
		
		private Label nameNode;
		private RichTextLabel descriptionNode;
		private Container detailsNode;
		private RichTextLabel textNode;
		private Container sectionsNode;
		
		public override void _Ready()
		{
			nameNode = GetNode<Label>(NodePath.Name);
			descriptionNode = GetNode<RichTextLabel>(NodePath.Description);
			detailsNode = GetNode<Container>(NodePath.Details);
			textNode = GetNode<RichTextLabel>(NodePath.Text);
			sectionsNode = GetNode<Container>(NodePath.Sections);
			
			//nameNode.GuiInput += toggleSections;
			GetNode<TextureButton>(NodePath.ShowHide).Pressed += toggleSections;
		}
		
		public void update(Ocsm.Dnd.Fifth.Feature feature)
		{
			var name = feature.Name;
			if(!String.IsNullOrEmpty(feature.Type))
			{
				if(!String.IsNullOrEmpty(feature.Source))
					name += String.Format(FormatTypeAndSource, feature.Type, feature.Source);
				else
					name += String.Format(FormatType, feature.Type);
			}
			
			nameNode.Text = name;
			descriptionNode.Text = feature.Description;
			textNode.Text = feature.Text;
			
			if(feature.Sections.Any())
			{
				var resource = GD.Load<PackedScene>(Constants.Scene.Dnd.Fifth.FeatureSection);
				feature.Sections.ForEach(s => instantiateSection(s, resource));
			}
		}
		
		private void instantiateSection(Ocsm.Dnd.Fifth.FeatureSection section, PackedScene resource)
		{
			var instance = resource.Instantiate<VBoxContainer>();
			instance.GetChild<Label>(0).Text = section.Section;
			instance.GetChild<RichTextLabel>(1).Text = section.Text;
			sectionsNode.AddChild(instance);
		}
		
		private void toggleSections()
		{
			if(detailsNode.Visible)
			{
				detailsNode.Hide();
				textNode.Hide();
				descriptionNode.Show();
			}
			else
			{
				descriptionNode.Hide();
				detailsNode.Show();
				textNode.Show();
			}
		}
	}
}
