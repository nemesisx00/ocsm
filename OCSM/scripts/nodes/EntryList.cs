using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Ocsm.Nodes
{
	public partial class EntryList : Container
	{
		[Signal]
		public delegate void ValueChangedEventHandler(Transport<List<string>> values);
		
		public List<string> Values { get; set; } = new List<string>();
		
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
			
			Values.Where(v => !String.IsNullOrEmpty(v))
				.ToList()
				.ForEach(v => addInput(v));
			
			addInput();
		}
		
		private void textChanged()
		{
			var values = new List<string>();
			var children = GetChildren();
			foreach(TextEdit c in children)
			{
				if(!String.IsNullOrEmpty(c.Text))
					values.Add(c.Text);
				else if(children.IndexOf(c) != children.Count - 1)
					c.QueueFree();
			}
			
			EmitSignal(nameof(ValueChanged), new Transport<List<string>>(values));
			
			if(children.Count <= values.Count)
			{
				addInput();
			}
		}
		
		private void addInput(string value = "")
		{
			var stringName = Name.ToString();
			
			var node = new TextEdit();
			node.CustomMinimumSize = new Vector2(0, 25);
			node.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
			node.SizeFlagsVertical = Control.SizeFlags.Fill;
			node.ScrollFitContentHeight = true;
			node.Text = value;
			node.TextChanged += textChanged;
			node.TooltipText = "Enter a new " + stringName.Substring(0, stringName.Length - 1);
			node.WrapMode = TextEdit.LineWrappingMode.Boundary;
			AddChild(node);
		}
	}
}
