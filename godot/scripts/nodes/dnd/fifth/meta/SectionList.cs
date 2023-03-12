using Godot;
using System;
using System.Collections.Generic;
using OCSM.DnD.Fifth;

namespace OCSM.Nodes.DnD.Fifth.Meta
{
	public partial class SectionList : Container
	{
		[Signal]
		public delegate void ValuesChangedEventHandler(Transport<List<FeatureSection>> transport);
		
		public List<FeatureSection> Values { get; set; } = new List<FeatureSection>();
		
		public override void _Ready()
		{
			refresh();
		}
		
		public void refresh()
		{
			foreach(Node c in GetChildren())
			{
				c.QueueFree();
			}
			
			foreach(var v in Values)
			{
				if(v is FeatureSection)
					addInput(v);
			}
			
			addInput();
		}
		
		private void lineChanged(string text) { updateValues(); }
		private void textChanged() { updateValues(); }
		
		private void updateValues()
		{
			var values = new List<FeatureSection>();
			var children = GetChildren();
			foreach(Node c in children)
			{
				var section = c.GetChild<LineEdit>(0).Text;
				var sectionText = c.GetChild<TextEdit>(1).Text;
				
				if(!String.IsNullOrEmpty(section) || !String.IsNullOrEmpty(sectionText))
					values.Add(new FeatureSection(section, sectionText));
				else if(children.IndexOf(c) != children.Count - 1)
					c.QueueFree();
			}
			
			Values = values;
			EmitSignal(nameof(ValuesChanged), new Transport<List<FeatureSection>>(Values));
			
			if(children.Count <= values.Count)
			{
				addInput();
			}
		}
		
		private void addInput(FeatureSection section = null)
		{
			var resource = GD.Load<PackedScene>(Constants.Scene.DnD.Fifth.Meta.FeatureSectionEntry);
			var instance = resource.Instantiate<HBoxContainer>();
			
			AddChild(instance);
			
			var sectionNode = instance.GetChild<LineEdit>(0);
			sectionNode.TextChanged += lineChanged;
			var textNode = instance.GetChild<TextEdit>(1);
			textNode.TextChanged += textChanged;
			
			if(section is FeatureSection)
			{
				sectionNode.Text = section.Section;
				textNode.Text = section.Text;
			}
		}
	}
}
