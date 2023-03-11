using Godot;
using System;
using System.Collections.Generic;
using OCSM.CoD;

namespace OCSM.Nodes.CoD
{
	public partial class MeritList : ItemDotsList
	{
		[Signal]
		public new delegate void ValueChangedEventHandler(Transport<List<Merit>> transport);
		
		public new List<Merit> Values { get; set; } = new List<Merit>();
		
		public override void _Ready()
		{
			refresh();
		}
		
		public override void refresh()
		{
			foreach(Node c in GetChildren())
			{
				c.QueueFree();
			}
			
			foreach(var merit in Values)
			{
				if(merit is Merit)
					addInput(merit.Name, merit.Value);
			}
			
			addInput();
		}
		
		protected override void addInput(string text = "", long dots = 0)
		{
			var resource = GD.Load<PackedScene>(Constants.Scene.CoD.ItemDots);
			var node = resource.Instantiate();
			var lineEdit = node.GetChild<LineEdit>(0);
			lineEdit.Text = text;
			lineEdit.TooltipText = "Enter a new Merit";
			
			var track = node.GetChild<TrackSimple>(1);
			track.updateValue(dots);
			
			AddChild(node);
			lineEdit.TextChanged += textChanged;
			track.NodeChanged += dotsChanged;
		}
		
		protected override void updateValues()
		{
			var values = new List<Merit>();
			var children = GetChildren();
			foreach(Node c in children)
			{
				var le = c.GetChild<LineEdit>(0);
				var dots = c.GetChild<TrackSimple>(1).Value;
				if(!String.IsNullOrEmpty(le.Text))
					values.Add(new Merit() { Name = le.Text, Value = dots });
				else if(children.IndexOf(c) != children.Count - 1)
					c.QueueFree();
			}
			
			Values = values;
			EmitSignal(nameof(ValueChanged), new Transport<List<Merit>>(Values));
			
			if(children.Count <= Values.Count)
			{
				addInput();
			}
		}
	}
}
