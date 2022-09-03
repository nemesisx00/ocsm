using Godot;
using System;
using System.Collections.Generic;

namespace OCSM.Nodes.CoD
{
	public class ItemDotsList : Container
	{
		[Signal]
		public delegate void ValueChanged(Dictionary<string, int> values);
		
		public Dictionary<string, int> Values { get; set; } = new Dictionary<string, int>();
		
		public override void _Ready()
		{
			refresh();
		}
		
		public virtual void refresh()
		{
			foreach(Node c in GetChildren())
			{
				c.QueueFree();
			}
			
			foreach(var key in Values.Keys)
			{
				if(!String.IsNullOrEmpty(key))
					addInput(key, Values[key]);
			}
			
			addInput();
		}
		
		protected virtual void addInput(string text = "", int dots = 0)
		{
			var resource = ResourceLoader.Load<PackedScene>(Constants.Scene.CoD.ItemDots);
			var node = resource.Instance();
			var lineEdit = node.GetChild<LineEdit>(0);
			lineEdit.Text = text;
			lineEdit.HintTooltip = "Enter a new " + Name.Substring(0, Name.Length - 1);
			
			var track = node.GetChild<TrackSimple>(1);
			track.Value = dots;
			
			AddChild(node);
			lineEdit.Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
			track.Connect(Constants.Signal.NodeChanged, this, nameof(dotsChanged));
		}
		
		protected void textChanged(string newText)
		{
			updateValues();
		}
		
		protected void dotsChanged(TrackSimple node)
		{
			updateValues();
		}
		
		protected virtual void updateValues()
		{
			var values = new Dictionary<string, int>();
			var children = GetChildren();
			foreach(Node c in children)
			{
				var le = c.GetChild<LineEdit>(0);
				var dots = c.GetChild<TrackSimple>(1).Value;
				
				if(!String.IsNullOrEmpty(le.Text))
					values.Add(le.Text, dots);
				else if(children.IndexOf(c) != children.Count - 1)
					c.QueueFree();
			}
			
			Values = values;
			EmitSignal(nameof(ValueChanged), Values);
			
			if(children.Count <= Values.Count)
			{
				addInput();
			}
		}
	}
}
