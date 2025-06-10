using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Ocsm.Nodes;

public partial class EntryList : Container
{
	[Signal]
	public delegate void ValueChangedEventHandler(Transport<List<string>> values);
	
	public List<string> Values { get; set; } = [];
	
	public override void _Ready() => Refresh();
	
	public void Refresh()
	{
		foreach(Node c in GetChildren())
		{
			c.QueueFree();
		}
		
		Values.Where(v => !string.IsNullOrEmpty(v))
			.ToList()
			.ForEach(v => addInput(v));
		
		addInput();
	}
	
	private void textChanged()
	{
		var values = new List<string>();
		var children = GetChildren();
		foreach(var c in children.Cast<TextEdit>())
		{
			if(!string.IsNullOrEmpty(c.Text))
				values.Add(c.Text);
			else if(children.IndexOf(c) != children.Count - 1)
				c.QueueFree();
		}
		
		EmitSignal(SignalName.ValueChanged, new Transport<List<string>>(values));
		
		if(children.Count <= values.Count)
			addInput();
	}
	
	private void addInput(string value = "")
	{
		var stringName = Name.ToString();

		TextEdit node = new()
		{
			CustomMinimumSize = new Vector2(0, 25),
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			SizeFlagsVertical = SizeFlags.Fill,
			ScrollFitContentHeight = true,
			Text = value
		};
		
		node.TextChanged += textChanged;
		node.TooltipText = string.Concat("Enter a new ", stringName.AsSpan(0, stringName.Length - 1));
		node.WrapMode = TextEdit.LineWrappingMode.Boundary;
		AddChild(node);
	}
}
