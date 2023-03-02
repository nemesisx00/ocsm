using Godot;
using System;
using System.Collections.Generic;

namespace OCSM.Nodes.CoD
{
	public partial class ItemDotsList : Container
	{
		[Signal]
		public delegate void ValueChangedEventHandler(Transport<Dictionary<string, long>> transport);
		
		public Dictionary<string, long> Values { get; set; } = new Dictionary<string, long>();
		
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
		
		protected virtual void addInput(string text = "", long dots = 0)
		{
			var stringName = Name.ToString();
			
			var resource = ResourceLoader.Load<PackedScene>(Constants.Scene.CoD.ItemDots);
			var node = resource.Instantiate();
			var lineEdit = node.GetChild<LineEdit>(0);
			lineEdit.Text = text;
			lineEdit.TooltipText = "Enter a new " + stringName.Substring(0, stringName.Length - 1);
			
			var track = node.GetChild<TrackSimple>(1);
			track.Value = dots;
			
			AddChild(node);
			lineEdit.TextChanged += textChanged;
			track.NodeChanged += dotsChanged;
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
			var values = new Dictionary<string, long>();
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
			EmitSignal(nameof(ValueChanged), new Transport<Dictionary<string, long>>(Values));
			
			if(children.Count <= Values.Count)
			{
				addInput();
			}
		}
	}
}
