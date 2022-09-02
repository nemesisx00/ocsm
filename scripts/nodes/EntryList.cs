using Godot;
using System;
using System.Collections.Generic;

namespace OCSM.Nodes
{
	public class EntryList : Container
	{
		[Signal]
		public delegate void ValueChanged(List<string> values);
		
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
			
			foreach(var v in Values)
			{
				if(!String.IsNullOrEmpty(v))
					addInput(v);
			}
			
			addInput();
		}
		
		private void textChanged(string text)
		{
			var values = new List<string>();
			var children = GetChildren();
			foreach(LineEdit c in children)
			{
				if(!String.IsNullOrEmpty(c.Text))
					values.Add(c.Text);
				else if(children.IndexOf(c) != children.Count - 1)
					c.QueueFree();
			}
			
			EmitSignal(nameof(ValueChanged), values);
			
			if(children.Count <= values.Count)
			{
				addInput();
			}
		}
		
		private void addInput(string value = "")
		{
			var node = new LineEdit();
			node.Text = value;
			node.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;
			node.SizeFlagsVertical = (int)Control.SizeFlags.ExpandFill;
			node.RectMinSize = new Vector2(0, 25);
			node.HintTooltip = "Enter a new " + Name.Substring(0, Name.Length - 1);
			AddChild(node);
			node.Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
		}
	}
}
