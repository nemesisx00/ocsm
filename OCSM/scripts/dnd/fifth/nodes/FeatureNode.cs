using Godot;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class FeatureNode : Container
{
	public static class NodePaths
	{
		public static readonly NodePath Name = new("%Name");
		public static readonly NodePath Description = new("%Description");
		public static readonly NodePath Details = new("%Details");
		public static readonly NodePath Sections = new("%Sections");
		public static readonly NodePath ShowHide = new("%ShowHide");
		public static readonly NodePath Source = new("%Source");
		public static readonly NodePath Text = new("%Text");
	}
	
	private Label nameNode;
	private RichTextLabel descriptionNode;
	private Container detailsNode;
	private Container sectionsNode;
	private Label sourceNode;
	private TextureButton showHide;
	private RichTextLabel textNode;
	
	public override void _GuiInput(InputEvent evt)
	{
		if(evt.IsActionReleased(Actions.Click))
		{
			showHide.EmitSignal(GodotActions.Pressed);
			showHide.ButtonPressed = !showHide.ButtonPressed;
		}
	}
	
	public override void _Ready()
	{
		nameNode = GetNode<Label>(NodePaths.Name);
		descriptionNode = GetNode<RichTextLabel>(NodePaths.Description);
		detailsNode = GetNode<Container>(NodePaths.Details);
		sectionsNode = GetNode<Container>(NodePaths.Sections);
		sourceNode = GetNode<Label>(NodePaths.Source);
		showHide = GetNode<TextureButton>(NodePaths.ShowHide);
		textNode = GetNode<RichTextLabel>(NodePaths.Text);
		
		showHide.Pressed += toggleSections;
	}
	
	public void Update(Feature feature)
	{
		nameNode.Text = feature.Name;
		descriptionNode.Text = feature.Description;
		sourceNode.Text = feature.Source;
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
