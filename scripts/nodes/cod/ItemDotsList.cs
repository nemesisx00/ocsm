using Godot;
using System;
using System.Collections.Generic;
using OCSM;

public class ItemDotsList : ScrollContainer
{
	private const string InputContainer = "Column";
	
	[Signal]
	public delegate void ValueChanged(List<TextValueItem> values);
	
	public List<TextValueItem> Values { get; set; } = new List<TextValueItem>();
	
	public override void _Ready()
	{
		refresh();
	}
	
	public void refresh()
	{
		foreach(Node c in GetNode(InputContainer).GetChildren())
		{
			c.QueueFree();
		}
		
		foreach(var v in Values)
		{
			if(!String.IsNullOrEmpty(v.Text))
				addInput(v.Text, v.Value);
		}
		
		addInput();
	}
	
	private void addInput(string text = "", int dots = 0)
	{
		var resource = GD.Load<PackedScene>(Constants.Scene.CoD.ItemDots);
		var node = resource.Instance();
		var lineEdit = node.GetChild<LineEdit>(0);
		lineEdit.Text = text;
		lineEdit.HintTooltip = "Enter a new " + Name.Substring(0, Name.Length - 1);
		
		var track = node.GetChild<TrackSimple>(1);
		track.Value = dots;
		
		GetNode(InputContainer).AddChild(node);
		lineEdit.Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
		track.Connect(Constants.Signal.NodeChanged, this, nameof(dotsChanged));
	}
	
	private void textChanged(string newText)
	{
		var values = new List<TextValueItem>();
		var children = GetNode(InputContainer).GetChildren();
		foreach(Node c in children)
		{
			var le = c.GetChild<LineEdit>(0);
			var dots = c.GetChild<TrackSimple>(1).Value;
			
			if(!String.IsNullOrEmpty(le.Text))
				values.Add(new TextValueItem(le.Text, dots));
			else if(children.IndexOf(c) != children.Count - 1)
				le.QueueFree();
		}
		
		Values = values;
		EmitSignal(nameof(ValueChanged), Values);
		
		if(children.Count <= Values.Count)
		{
			addInput();
		}
	}
	
	private void dotsChanged(TrackSimple node)
	{
		var dots = node.Value;
		var index = node.GetParent().GetIndex();
		if(index >= Values.Count)
			Values.Add(new TextValueItem(String.Empty, dots));
		else
			Values[index].Value = dots;
		
		EmitSignal(nameof(ValueChanged), Values);
	}
}
