using Godot;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class FeatureNode : Container
{
	public static class NodePaths
	{
		public static readonly NodePath Name = new("%Name");
		public static readonly NodePath Description = new("%Description");
		public static readonly NodePath Details = new("%Details");
		public static readonly NodePath Text = new("%Text");
		public static readonly NodePath Sections = new("%Sections");
		public static readonly NodePath ShowHide = new("%ShowHide");
	}
	
	private Label nameNode;
	private RichTextLabel descriptionNode;
	private Container detailsNode;
	private RichTextLabel textNode;
	private Container sectionsNode;
	
	public override void _Ready()
	{
		nameNode = GetNode<Label>(NodePaths.Name);
		descriptionNode = GetNode<RichTextLabel>(NodePaths.Description);
		detailsNode = GetNode<Container>(NodePaths.Details);
		textNode = GetNode<RichTextLabel>(NodePaths.Text);
		sectionsNode = GetNode<Container>(NodePaths.Sections);
		
		//nameNode.GuiInput += toggleSections;
		GetNode<TextureButton>(NodePaths.ShowHide).Pressed += toggleSections;
	}
	
	public void Update(Feature feature)
	{
		var name = feature.Name;
		
		if(!string.IsNullOrEmpty(feature.Source))
			name += $"({feature.FeatureType} Feature, {feature.Source})";
		else
			name += $"({feature.FeatureType} Feature)";
		
		nameNode.Text = name;
		descriptionNode.Text = feature.Description;
		textNode.Text = feature.Text;
		
		if(feature.Sections.Count != 0)
		{
			var resource = GD.Load<PackedScene>(ScenePaths.Dnd.Fifth.FeatureSection);
			if(resource.CanInstantiate())
			{
				foreach(var s in feature.Sections)
					instantiateSection(s, resource);
			}
		}
	}
	
	private void instantiateSection(FeatureSection section, PackedScene resource)
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
