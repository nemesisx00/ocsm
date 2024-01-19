using Godot;
using System.Collections.Generic;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class SectionList : Container
{
	[Signal]
	public delegate void ValuesChangedEventHandler(Transport<List<FeatureSection>> transport);
	
	public List<FeatureSection> Values { get; set; } = [];
	
	public override void _Ready()
	{
		Refresh();
	}
	
	public void Refresh()
	{
		foreach(Node c in GetChildren())
		{
			c.QueueFree();
		}
		
		Values.ForEach(v => addInput(v));
		
		addInput();
	}
	
	private void lineChanged(string text) => updateValues();
	private void textChanged() => updateValues();
	
	private void updateValues()
	{
		List<FeatureSection> values = [];
		var children = GetChildren();
		
		foreach(Node c in children)
		{
			var section = c.GetChild<LineEdit>(0).Text;
			var sectionText = c.GetChild<TextEdit>(1).Text;
			
			if(!string.IsNullOrEmpty(section) || !string.IsNullOrEmpty(sectionText))
				values.Add(new(section, sectionText));
			else if(children.IndexOf(c) != children.Count - 1)
				c.QueueFree();
		}
		
		Values = values;
		_ = EmitSignal(SignalName.ValuesChanged, new Transport<List<FeatureSection>>(Values));
		
		if(children.Count <= values.Count)
			addInput();
	}
	
	private void addInput(FeatureSection section = null)
	{
		var resource = GD.Load<PackedScene>(ScenePaths.Dnd.Fifth.Meta.FeatureSectionEntry);
		var instance = resource.Instantiate<HBoxContainer>();
		
		AddChild(instance);
		
		var sectionNode = instance.GetChild<LineEdit>(0);
		sectionNode.TextChanged += lineChanged;
		var textNode = instance.GetChild<TextEdit>(1);
		textNode.TextChanged += textChanged;
		
		if(section is not null)
		{
			sectionNode.Text = section.Section;
			textNode.Text = section.Text;
		}
	}
}