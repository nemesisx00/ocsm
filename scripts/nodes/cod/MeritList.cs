using Godot;
using System;
using System.Collections.Generic;
using OCSM;
using OCSM.CoD;

public class MeritList : ItemDotsList
{
	[Signal]
	public new delegate void ValueChanged(List<Transport<Merit>> values);
	
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
	
	protected override void addInput(string text = "", int dots = 0)
	{
		var resource = GD.Load<PackedScene>(Constants.Scene.CoD.ItemDots);
		var node = resource.Instance();
		var lineEdit = node.GetChild<LineEdit>(0);
		lineEdit.Text = text;
		lineEdit.HintTooltip = "Enter a new Merit";
		
		var track = node.GetChild<TrackSimple>(1);
		track.updateValue(dots);
		
		AddChild(node);
		lineEdit.Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
		track.Connect(Constants.Signal.NodeChanged, this, nameof(dotsChanged));
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
				values.Add(new Merit(le.Text, String.Empty, dots));
			else if(children.IndexOf(c) != children.Count - 1)
				c.QueueFree();
		}
		
		Values = values;
		
		doEmitSignal();
		
		if(children.Count <= Values.Count)
		{
			addInput();
		}
	}
	
	private void doEmitSignal()
	{
		var list = new List<Transport<Merit>>();
		foreach(var merit in Values)
		{
			list.Add(new Transport<Merit>(merit));
		}
		EmitSignal(nameof(ValueChanged), list);
	}
}
